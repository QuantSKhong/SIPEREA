Option Explicit On
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices.Marshal
Imports System.Math
Imports System.IO


Public Class CompactImgProcessing

    Declare Function GetPixel Lib "gdi32" (ByVal hDC As Integer,
                                      ByVal x As Integer,
                                      ByVal y As Integer) As Integer

    Structure Def_PixelIndex
        Dim Red As Integer
        Dim Green As Integer
        Dim Blue As Integer
    End Structure


    Public Const WhiteColorInt As Integer = 16777215

    Dim _ImageTag As String

    Dim _IsImageLocked As Boolean = False
    Dim _StartedAt As Date
    Dim _CompletedAt As Date
    Dim _LapseAsMilliSecond As Integer = 0

    Dim _SrcBitmap As Bitmap
    Shared _SrcBitmapWidth, _SrcBitmapHeight As Integer
    Dim Prev_SrcBitmapWidth, Prev_SrcBitmapHeight As Integer
    Dim _SrcBMD As BitmapData
    Shared _SrcPixels(0), _OutPixels(0) As Byte
    Dim _PixelIndexUpper As Integer
    Shared _PixelIndex(0, 0) As Def_PixelIndex
    Dim _GrayImage(0, 0) As Byte
    Dim _MaskBitmap As Bitmap
    Dim _MaskPixels(0) As Byte
    Dim _MaskBMD As BitmapData
    Dim _MaskBMDScan0 As IntPtr
    Dim _IsMaskImageAvailable As Boolean = False


    Dim x1, y1, x2, y2 As Integer
    Dim _BMDScan0 As IntPtr
    Dim _Red, _Green, _Blue As Single

    Shared LUT_RGB_To_PartialGray(3, 255) As Single


    Public Event Started()
    Public Shared Event Processing(ByVal CurrentPercent As Single)
    Public Event Completed(ByVal LapseAsMilliSecond As Integer)


    Public Sub ImageTo_SrcPixels(ByRef SourceImage As Image, Optional ByRef MaskImage As Image = Nothing)
        _IsImageLocked = True

        RaiseEvent Started()
        If SourceImage.Tag Is Nothing Then
            _ImageTag = ""
        Else
            _ImageTag = CStr(SourceImage.Tag)
        End If

        _StartedAt = Now

        _SrcBitmap = CType(SourceImage.Clone, Bitmap)
        _SrcBitmapHeight = _SrcBitmap.Height
        _SrcBitmapWidth = _SrcBitmap.Width
        _SrcBMD = _SrcBitmap.LockBits(New Rectangle(0, 0, _SrcBitmapWidth, _SrcBitmapHeight),
                    System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

        _BMDScan0 = _SrcBMD.Scan0

        ReDim _SrcPixels(_SrcBMD.Stride * _SrcBitmapHeight - 1)
        ReDim _OutPixels(_SrcBMD.Stride * _SrcBitmapHeight - 1)

        _PixelIndexUpper = _SrcPixels.Length
        Copy(_BMDScan0, _SrcPixels, 0, _PixelIndexUpper)
        Copy(_BMDScan0, _OutPixels, 0, _PixelIndexUpper)

        If MaskImage Is Nothing Then
            _IsMaskImageAvailable = False
        Else
            _IsMaskImageAvailable = True
            _MaskBitmap = CType(MaskImage.Clone, Bitmap)
            _MaskBMD = _MaskBitmap.LockBits(New Rectangle(0, 0, _SrcBitmapWidth, _SrcBitmapHeight),
                    System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)
            _MaskBMDScan0 = _MaskBMD.Scan0

            ReDim _MaskPixels(_SrcBMD.Stride * _SrcBitmapHeight - 1)

            Copy(_MaskBMDScan0, _MaskPixels, 0, _PixelIndexUpper)
        End If



        If Prev_SrcBitmapHeight <> _SrcBitmapHeight _
                            OrElse Prev_SrcBitmapWidth <> _SrcBitmapWidth Then

            ReDim _PixelIndex(_SrcBitmapWidth - 1, _SrcBitmapHeight - 1)

            Call Build_PixelIndex()

            Prev_SrcBitmapHeight = _SrcBitmapHeight
            Prev_SrcBitmapWidth = _SrcBitmapWidth
        End If
    End Sub


    Public Function OutPixelsToImage() As Bitmap

        Copy(_OutPixels, 0, _BMDScan0, _PixelIndexUpper)
        _SrcBitmap.UnlockBits(_SrcBMD)


        If _IsMaskImageAvailable Then
            _MaskBitmap.UnlockBits(_MaskBMD)
        End If


        _CompletedAt = Now
        Call Process_TimeLapseCalculation()


        RaiseEvent Completed(_LapseAsMilliSecond)


        OutPixelsToImage = CType(_SrcBitmap.Clone, Bitmap)

        OutPixelsToImage.Tag = _ImageTag
        _IsImageLocked = False
    End Function

    Private Sub Process_TimeLapseCalculation()
        _LapseAsMilliSecond = CInt((TimeSpan.FromTicks(_CompletedAt.Ticks).TotalMilliseconds _
                            - TimeSpan.FromTicks(_StartedAt.Ticks).TotalMilliseconds))
    End Sub



    Public Shared Function RGBToGray(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer) As Byte
        Return CByte(LUT_RGB_To_PartialGray(1, r) +
                    LUT_RGB_To_PartialGray(2, g) +
                    LUT_RGB_To_PartialGray(3, b))
    End Function


    Private Sub Build_PixelIndex()
        Dim z As Integer
        Dim ColumnsPixelCount As Integer
        Dim Stride As Integer = _SrcBMD.Stride

        For y As Integer = 0 To _SrcBitmapHeight - 1
            ColumnsPixelCount = y * Stride

            For x As Integer = 0 To _SrcBitmapWidth - 1
                z = ColumnsPixelCount + x * 3
                With _PixelIndex(x, y)
                    '.Alpha = z + 3
                    .Red = z + 2
                    .Green = z + 1
                    .Blue = z
                End With

            Next
        Next
    End Sub


    Protected Friend Function Image_FromFile(ByVal FileName As String) As Image

        Using ImageFileStream As FileStream =
                            New FileStream(FileName, FileMode.Open, FileAccess.Read)


            Dim ImageBytes(CInt(ImageFileStream.Length)) As Byte

            ImageFileStream.Read(ImageBytes, 0, CInt(ImageBytes.Length))
            ImageFileStream.Close()

            Image_FromFile = Image.FromStream(New MemoryStream(ImageBytes))

            ImageBytes = Nothing
        End Using
    End Function


    Public ReadOnly Property LapseAsMilliSecond() As Integer
        Get
            Return _LapseAsMilliSecond
        End Get
    End Property




    Public Sub New()
        Dim i As Integer

        'Generate LUT for gray scaling
        'Formula: Gray=(0.299 * r) + (0.587 * g) + (0.114 * b)
        For i = 0 To 255
            LUT_RGB_To_PartialGray(1, i) = CSng(0.299 * i)
            LUT_RGB_To_PartialGray(2, i) = CSng(0.587 * i)
            LUT_RGB_To_PartialGray(3, i) = CSng(0.114 * i)
        Next
    End Sub

    Private Sub Process_BoundaryRegion(ByVal PicWidthUpper As Integer, ByVal PicHeightUpper As Integer,
                                     ByVal BorderWidthUpper As Integer, ByVal BorderHeightUpper As Integer,
                                     ByRef CurX As Integer, ByRef CurY As Integer,
                                     ByVal FilterBoxHalfSize As Integer,
                                     ByRef CellXStart As Integer, ByRef CellXEnd As Integer,
                                     ByRef CellYStart As Integer, ByRef CellYEnd As Integer,
                                     ByRef CellXPixelCount As Integer,
                                     ByRef CellYPixelCount As Integer,
                                     ByRef CellPixelCount As Integer)


        If CurX = FilterBoxHalfSize Then
            If CurY > FilterBoxHalfSize Then
                If CurY < BorderHeightUpper Then
                    CurX = PicWidthUpper - FilterBoxHalfSize + 1
                End If
            End If
        End If


        If CurX < FilterBoxHalfSize Then
            CellXStart = Math.Max(0, CurX - FilterBoxHalfSize)
            CellXEnd = CurX + FilterBoxHalfSize
        Else
            CellXStart = CurX - FilterBoxHalfSize
            CellXEnd = Math.Min(PicWidthUpper, CurX + FilterBoxHalfSize)
        End If

        If CurY < FilterBoxHalfSize Then
            CellYStart = Math.Max(0, CurY - FilterBoxHalfSize)
            CellYEnd = CurY + FilterBoxHalfSize
        Else
            CellYStart = CurY - FilterBoxHalfSize
            CellYEnd = Math.Min(PicHeightUpper, CurY + FilterBoxHalfSize)
        End If


        CellXPixelCount = CellXEnd - CellXStart + 1
        CellYPixelCount = CellYEnd - CellYStart + 1
        CellPixelCount = CellXPixelCount * CellYPixelCount

    End Sub


    Public Function DeepCopy(ByVal SourceImage As Image,
                           Optional ByVal MaskImage As Image = Nothing,
                           Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                           Optional ByVal IsMaskAllRegionSelected As Boolean = True) As Bitmap

        If SourceImage Is Nothing Then Return CType(SourceImage, Bitmap)

        Call ImageTo_SrcPixels(SourceImage, MaskImage)

        Return OutPixelsToImage()
    End Function

    Public Function Resize(ByVal SourceImage As Image,
                      ByVal nWidth As Integer,
                      ByVal nHeight As Integer) As Bitmap

        If SourceImage Is Nothing Then Return Nothing


        _StartedAt = Now


        Dim bm As New Bitmap(SourceImage)

        Dim Temp_bm As New Bitmap(nWidth, nHeight)

        Dim g As Graphics = Graphics.FromImage(Temp_bm)

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

        g.DrawImage(bm, New Rectangle(0, 0, nWidth, nHeight),
                    New Rectangle(0, 0, bm.Width, bm.Height),
                    GraphicsUnit.Pixel)

        g.Dispose()
        g = Nothing
        bm.Dispose()
        bm = Nothing

        Resize = CType(Temp_bm.Clone, Bitmap)
        Temp_bm.Dispose()
        Temp_bm = Nothing

        _CompletedAt = Now
        Call Process_TimeLapseCalculation()
        RaiseEvent Completed(_LapseAsMilliSecond)

    End Function

    Public Function BoxAveraging_ByIntegralMap(ByVal SourceImage As Image,
                                    ByVal BoxSizeHalf As Integer,
                                Optional ByVal MaskImage As Image = Nothing,
                                Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                                Optional ByVal IsMaskAllRegionSelected As Boolean = True) As Bitmap
        If SourceImage Is Nothing Then Return CType(SourceImage, Bitmap)



        Call ImageTo_SrcPixels(SourceImage, MaskImage)


        Dim CurColorArray(_SrcBitmapWidth - 1, _SrcBitmapHeight - 1) As Byte
        Dim Temp_IntegralGrayMap(,) As Long
        Dim BoxSize As Integer = BoxSizeHalf * 2 + 1



        If IsMaskAllRegionSelected Then

            For CurColorChannel As Integer = 0 To 2
                For y As Integer = 0 To _SrcBitmapHeight - 1
                    For x As Integer = 0 To _SrcBitmapWidth - 1
                        With _PixelIndex(x, y)
                            CurColorArray(x, y) = _OutPixels(.Blue + CurColorChannel)
                        End With
                    Next
                Next

                Temp_IntegralGrayMap =
                    Build_IntegralGrayImageMap_From2DByteArray(CurColorArray)


                For y As Integer = 0 To _SrcBitmapHeight - 1

                    For x As Integer = 0 To _SrcBitmapWidth - 1

                        With _PixelIndex(x, y)
                            _OutPixels(.Blue + CurColorChannel) =
                                Calculate_LocalGrayAvgValue(Temp_IntegralGrayMap,
                                                      BoxSize, x, y)
                        End With
                    Next x

                    RaiseEvent Processing(CSng(y / _SrcBitmapHeight * 33) + CurColorChannel * 33)
                Next y

            Next
        Else


            With MaskRegionBoundary
                x1 = .X
                y1 = .Y
                x2 = .X + .Width - 1
                y2 = .Y + .Height - 1
            End With


            For CurColorChannel As Integer = 0 To 2
                For y As Integer = 0 To _SrcBitmapHeight - 1
                    For x As Integer = 0 To _SrcBitmapWidth - 1
                        With _PixelIndex(x, y)
                            CurColorArray(x, y) = _OutPixels(.Blue + CurColorChannel)
                        End With
                    Next
                Next

                Temp_IntegralGrayMap =
                    Build_IntegralGrayImageMap_From2DByteArray(CurColorArray)


                For y As Integer = y1 To y2
                    For x As Integer = x1 To x2
                        With _PixelIndex(x, y)
                            If _MaskPixels(.Blue) = 0 Then
                                _OutPixels(.Blue + CurColorChannel) =
                                Calculate_LocalGrayAvgValue(Temp_IntegralGrayMap,
                                                      BoxSize, x, y)
                            End If
                        End With
                    Next x

                    RaiseEvent Processing(CSng((y - y1) / _SrcBitmapHeight * 33) + CurColorChannel * 33)
                Next y
            Next
        End If


        RaiseEvent Completed(_LapseAsMilliSecond)

        Return OutPixelsToImage()

    End Function



    Public Function Build_IntegralGrayImageMap_From2DByteArray(SrcByteArray(,) As Byte) As Long(,)
        x1 = 0
        y1 = 0
        x2 = SrcByteArray.GetUpperBound(0)
        y2 = SrcByteArray.GetUpperBound(1)

        Dim Temp_IntegralGrayImageMap(x2, y2) As Long

        'Computing integral sum of the first row
        Temp_IntegralGrayImageMap(x1, y1) = SrcByteArray(x1, y1)
        For x As Integer = x1 + 1 To x2
            Temp_IntegralGrayImageMap(x, y1) = Temp_IntegralGrayImageMap(x - 1, y1) +
                                             SrcByteArray(x1, y1)
        Next
        'Computing integral sum of the first column
        For y As Integer = y1 + 1 To y2
            Temp_IntegralGrayImageMap(x1, y) = Temp_IntegralGrayImageMap(x1, y - 1) +
                                           SrcByteArray(x1, y1)
        Next

        'Computing integral sum for the rest of points
        For y As Integer = y1 + 1 To y2
            For x As Integer = x1 + 1 To x2
                Temp_IntegralGrayImageMap(x, y) = SrcByteArray(x, y) +
                                                Temp_IntegralGrayImageMap(x - 1, y) +
                                                Temp_IntegralGrayImageMap(x, y - 1) -
                                                Temp_IntegralGrayImageMap(x - 1, y - 1)

            Next
        Next


        Return Temp_IntegralGrayImageMap
    End Function



    'Default source is _OutPixels
    Public Function Build_IntegralGrayImageMap() As Long(,)


        Dim Temp_IntegralGrayImageMap(_SrcBitmapWidth - 1,
                                      _SrcBitmapHeight - 1) As Long


        x1 = 0
        y1 = 0
        x2 = _SrcBitmapWidth - 1
        y2 = _SrcBitmapHeight - 1



        'Computing integral sum of the first row
        Temp_IntegralGrayImageMap(x1, y1) = _OutPixels(_PixelIndex(x1, y1).Blue)
        For x As Integer = x1 + 1 To x2
            Temp_IntegralGrayImageMap(x, y1) = Temp_IntegralGrayImageMap(x - 1, y1) +
                                             _OutPixels(_PixelIndex(x1, y1).Blue)
        Next
        'Computing integral sum of the first column
        For y As Integer = y1 + 1 To y2
            Temp_IntegralGrayImageMap(x1, y) = Temp_IntegralGrayImageMap(x1, y - 1) +
                                            _OutPixels(_PixelIndex(x1, y1).Blue)
        Next

        'Computing integral sum for the rest of points
        For y As Integer = y1 + 1 To y2
            For x As Integer = x1 + 1 To x2
                Temp_IntegralGrayImageMap(x, y) = _OutPixels(_PixelIndex(x, y).Blue) +
                                                Temp_IntegralGrayImageMap(x - 1, y) +
                                                Temp_IntegralGrayImageMap(x, y - 1) -
                                                Temp_IntegralGrayImageMap(x - 1, y - 1)

            Next
        Next


        Return Temp_IntegralGrayImageMap
    End Function


    Shared Function Calculate_LocalGrayAvgValue(ByRef IntegralGrayImageMap(,) As Long,
                                                ByVal LocalBoxSize As Integer,
                                                ByVal CenterX As Integer,
                                                ByVal CenterY As Integer) As Byte
        Dim LocalBoxSum As Long
        Dim LocalBoxAvg As Byte
        Dim LocalBoxX1, LocalBoxX2, LocalBoxY1, LocalBoxY2 As Integer
        Dim LocalBoxHalfSize As Integer = CInt((LocalBoxSize - 1) / 2)

        LocalBoxX1 = Max(CenterX - LocalBoxHalfSize, 1)
        LocalBoxY1 = Max(CenterY - LocalBoxHalfSize, 1)

        LocalBoxX2 = Min(CenterX + LocalBoxHalfSize,
                         IntegralGrayImageMap.GetUpperBound(0))
        LocalBoxY2 = Min(CenterY + LocalBoxHalfSize,
                         IntegralGrayImageMap.GetUpperBound(1))


        LocalBoxSum = IntegralGrayImageMap(LocalBoxX2, LocalBoxY2)
        LocalBoxSum -= IntegralGrayImageMap(LocalBoxX2, LocalBoxY1 - 1)
        LocalBoxSum -= IntegralGrayImageMap(LocalBoxX1 - 1, LocalBoxY2)
        LocalBoxSum += IntegralGrayImageMap(LocalBoxX1 - 1, LocalBoxY1 - 1)

        LocalBoxAvg = CByte(LocalBoxSum / ((LocalBoxX2 - LocalBoxX1 + 1) *
                                          (LocalBoxY2 - LocalBoxY1 + 1)))

        Return LocalBoxAvg
    End Function

    Public Function FillRegion(ByVal SourceImage As Image,
                               ByVal FillColorRed As Byte,
                               ByVal FillColorGreen As Byte,
                               ByVal FillColorBlue As Byte,
                               ByVal IsFillInside As Boolean,
                                Optional ByVal MaskImage As Image = Nothing,
                                Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                                Optional ByVal IsMaskAllRegionSelected As Boolean = True) As Bitmap


        If SourceImage Is Nothing Then Return CType(SourceImage, Bitmap)


        Call ImageTo_SrcPixels(SourceImage, MaskImage)

        If IsMaskAllRegionSelected Then

            For y As Integer = 0 To _SrcBitmapHeight - 1
                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        _OutPixels(.Red) = FillColorRed
                        _OutPixels(.Green) = FillColorGreen
                        _OutPixels(.Blue) = FillColorBlue
                    End With

                Next x
            Next y

        Else
            Get_X1Y1X2Y2_From_Rectangle(MaskRegionBoundary, x1, y1, x2, y2)


            If IsFillInside Then
                For y As Integer = y1 To y2
                    For x As Integer = x1 To x2
                        With _PixelIndex(x, y)
                            If _MaskPixels(.Blue) = 0 Then
                                _OutPixels(.Red) = FillColorRed
                                _OutPixels(.Green) = FillColorGreen
                                _OutPixels(.Blue) = FillColorBlue
                            End If
                        End With

                    Next x
                Next
            Else
                For y As Integer = 0 To _SrcBitmapHeight - 1
                    For x As Integer = 0 To _SrcBitmapWidth - 1
                        With _PixelIndex(x, y)
                            If _MaskPixels(.Blue) = 255 Then
                                _OutPixels(.Red) = FillColorRed
                                _OutPixels(.Green) = FillColorGreen
                                _OutPixels(.Blue) = FillColorBlue
                            End If
                        End With

                    Next x
                Next

            End If
        End If


        Return OutPixelsToImage()
    End Function


    Public Function BWLeveling_Using_AdaptiveThreshold(ByVal SourceImage As Image,
                              ByVal LocalBoxSizeValue As Integer,
                              ByVal IsObjectWhite As Boolean,
                              ByVal ThresholdLevel As Integer,
                              Optional ByVal MaskImage As Image = Nothing,
                              Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                              Optional ByVal IsMaskAllRegionSelected As Boolean = True) As Bitmap


        If SourceImage Is Nothing Then Return CType(SourceImage, Bitmap)


        Static LocalBoxSize As Integer = 15

        If LocalBoxSizeValue > 0 Then
            LocalBoxSize = LocalBoxSizeValue
        End If


        Call ImageTo_SrcPixels(SourceImage, MaskImage)


        Dim Temp_IntegralGrayMap(,) As Long
        Dim LocalGrayAvg As Byte
        Dim Gray As Byte
        Dim tPercentValue As Single = CSng(ThresholdLevel / 100)
        Dim Color_Background As Byte
        Dim Color_Object As Byte



        'Grayscaling and inverting...
        If IsObjectWhite Then
            For y As Integer = 0 To _SrcBitmapHeight - 1
                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        Gray = CByte(255 - RGBToGray(_SrcPixels(.Red),
                                             _SrcPixels(.Green),
                                             _SrcPixels(.Blue)))
                        _OutPixels(.Red) = Gray
                        _OutPixels(.Green) = Gray
                        _OutPixels(.Blue) = Gray
                    End With

                Next x

            Next y

            Color_Object = 0
            Color_Background = 255
        Else
            For y As Integer = 0 To _SrcBitmapHeight - 1
                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        Gray = RGBToGray(_SrcPixels(.Red),
                                             _SrcPixels(.Green),
                                             _SrcPixels(.Blue))
                        _OutPixels(.Red) = Gray
                        _OutPixels(.Green) = Gray
                        _OutPixels(.Blue) = Gray
                    End With

                Next x
            Next y


            Color_Object = 255
            Color_Background = 0
        End If


        Temp_IntegralGrayMap = Build_IntegralGrayImageMap()



        If IsMaskAllRegionSelected Then


            For y As Integer = 0 To _SrcBitmapHeight - 1
                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        LocalGrayAvg = Calculate_LocalGrayAvgValue(Temp_IntegralGrayMap,
                                                          LocalBoxSize, x, y)
                        If _OutPixels(.Red) < LocalGrayAvg * tPercentValue Then
                            _OutPixels(.Red) = Color_Background
                            _OutPixels(.Green) = Color_Background
                            _OutPixels(.Blue) = Color_Background
                        Else
                            _OutPixels(.Red) = Color_Object
                            _OutPixels(.Green) = Color_Object
                            _OutPixels(.Blue) = Color_Object
                        End If
                    End With

                Next x

                RaiseEvent Processing(CSng(y / _SrcBitmapHeight * 100))
            Next y

        Else


            For y As Integer = 0 To _SrcBitmapHeight - 1
                For x As Integer = 0 To _SrcBitmapWidth - 1
                    With _PixelIndex(x, y)
                        If _MaskPixels(.Blue) = 0 Then
                            LocalGrayAvg = Calculate_LocalGrayAvgValue(Temp_IntegralGrayMap,
                                                          LocalBoxSize, x, y)
                            If _OutPixels(.Red) < LocalGrayAvg * tPercentValue Then
                                _OutPixels(.Red) = Color_Background
                                _OutPixels(.Green) = Color_Background
                                _OutPixels(.Blue) = Color_Background
                            Else
                                _OutPixels(.Red) = Color_Object
                                _OutPixels(.Green) = Color_Object
                                _OutPixels(.Blue) = Color_Object
                            End If
                        Else
                            _OutPixels(.Red) = _SrcPixels(.Red)
                            _OutPixels(.Green) = _SrcPixels(.Green)
                            _OutPixels(.Blue) = _SrcPixels(.Blue)

                        End If
                    End With

                Next x

                RaiseEvent Processing(CSng((y - y1) / (y2 - y1 + 1) * 100))
            Next y

        End If




        RaiseEvent Completed(_LapseAsMilliSecond)

        Return OutPixelsToImage()

    End Function

    Public Function RegionExtract_Crop_LabelIDMap_To_2DByte(
                                 ByVal LabelIDMap As Integer(,),
                                 ByVal IDInfoIndex As Integer,
                                 ByVal X1 As Integer, X2 As Integer,
                                 ByVal Y1 As Integer, Y2 As Integer) As Byte(,)

        Dim Temp2DByte(X2 - X1, Y2 - Y1) As Byte


        For y As Integer = Y1 To Y2
            For x As Integer = X1 To X2
                If LabelIDMap(x, y) = IDInfoIndex Then
                    Temp2DByte(x - X1, y - Y1) = 255
                Else
                    Temp2DByte(x - X1, y - Y1) = 0
                End If
            Next x

        Next y

        Return Temp2DByte
    End Function


    Public Function Convert_GrayByteArray_To_RGBImage(ByRef SrcByteArray(,) As Byte) As Bitmap

        Using SourceImage As Image =
                New Bitmap(SrcByteArray.GetUpperBound(0) + 1, SrcByteArray.GetUpperBound(1) + 1)


            Call ImageTo_SrcPixels(SourceImage)

            For y As Integer = 0 To _SrcBitmapHeight - 1

                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        _OutPixels(.Red) = SrcByteArray(x, y)
                        _OutPixels(.Green) = SrcByteArray(x, y)
                        _OutPixels(.Blue) = SrcByteArray(x, y)
                    End With
                Next x

            Next y

            Convert_GrayByteArray_To_RGBImage = OutPixelsToImage()

        End Using
    End Function



    Public Function Maximize(ByVal SourceImage As Image,
                                        ByVal BoxSizeHalf As Integer,
                                Optional ByVal MaskImage As Image = Nothing,
                                Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                                Optional ByVal IsMaskAllRegionSelected As Boolean = True,
                                Optional ByVal IsDoGrayScaling As Boolean = True) As Bitmap


        If SourceImage Is Nothing Then Return CType(SourceImage, Bitmap)



        Call ImageTo_SrcPixels(SourceImage, MaskImage)


        Dim PixelCount As Integer
        Dim BitmapHeightUpper, BitmapWidthUpper As Integer
        Dim BorderHeightUpper, BorderWidthUpper As Integer
        Dim CellHeightStart, CellHeightEnd As Integer
        Dim CellWidthStart, CellWidthEnd As Integer
        Dim CellWidthPixelCount, CellHeightPixelCount As Integer
        Dim SelectRegionWidthStart, SelectRegionWidthEnd As Integer
        Dim SelectRegionHeightStart, SelectRegionHeightEnd As Integer
        Dim Cur_GrayMax, Cur_Gray As Byte
        Dim Cur_Red, Cur_Blue, Cur_Green As Byte

        BitmapHeightUpper = _SrcBitmapHeight - 1
        BitmapWidthUpper = _SrcBitmapWidth - 1
        BorderHeightUpper = _SrcBitmapHeight - BoxSizeHalf - 1
        BorderWidthUpper = _SrcBitmapWidth - BoxSizeHalf - 1


        ReDim _GrayImage(BitmapWidthUpper, BitmapHeightUpper)


        If IsMaskAllRegionSelected Then

            If IsDoGrayScaling Then
                'Grayscaling
                For y As Integer = 0 To BitmapHeightUpper
                    For x As Integer = 0 To BitmapWidthUpper
                        With _PixelIndex(x, y)
                            _GrayImage(x, y) = CByte(RGBToGray(_SrcPixels(.Red),
                                              _SrcPixels(.Green),
                                              _SrcPixels(.Blue)))
                        End With
                    Next x
                Next y
            Else
                For y As Integer = 0 To BitmapHeightUpper
                    For x As Integer = 0 To BitmapWidthUpper
                        With _PixelIndex(x, y)
                            _GrayImage(x, y) = _SrcPixels(.Red)
                        End With
                    Next x
                Next y
            End If



            PixelCount = CInt((BoxSizeHalf * 2 + 1) ^ 2)
            For y As Integer = BoxSizeHalf To BorderHeightUpper
                For x As Integer = BoxSizeHalf To BorderWidthUpper

                    Cur_GrayMax = 0 : Cur_Red = 0 : Cur_Green = 0 : Cur_Blue = 0
                    CellWidthStart = x - BoxSizeHalf
                    CellWidthEnd = x + BoxSizeHalf
                    CellHeightStart = y - BoxSizeHalf
                    CellHeightEnd = y + BoxSizeHalf

                    For y2 As Integer = CellHeightStart To CellHeightEnd
                        For x2 As Integer = CellWidthStart To CellWidthEnd
                            Cur_Gray = _GrayImage(x2, y2)
                            If Cur_Gray > Cur_GrayMax Then
                                With _PixelIndex(x2, y2)
                                    Cur_Red = _SrcPixels(.Red)
                                    Cur_Green = _SrcPixels(.Green)
                                    Cur_Blue = _SrcPixels(.Blue)
                                End With
                                Cur_GrayMax = Cur_Gray
                            End If
                        Next
                    Next

                    With _PixelIndex(x, y)
                        _OutPixels(.Red) = Cur_Red
                        _OutPixels(.Green) = Cur_Green
                        _OutPixels(.Blue) = Cur_Blue
                    End With
                Next x

                RaiseEvent Processing(CSng(y / _SrcBitmapHeight * 90))
            Next y


            For y As Integer = 0 To BitmapHeightUpper
                For x As Integer = 0 To BitmapWidthUpper

                    Call Process_BoundaryRegion(BitmapWidthUpper, BitmapHeightUpper,
                                                BorderWidthUpper, BorderHeightUpper,
                                                x, y, BoxSizeHalf,
                                                CellWidthStart, CellWidthEnd,
                                                CellHeightStart, CellHeightEnd,
                                                CellWidthPixelCount, CellHeightPixelCount,
                                                PixelCount)


                    Cur_GrayMax = 0 : Cur_Red = 0 : Cur_Green = 0 : Cur_Blue = 0
                    For y2 As Integer = CellHeightStart To CellHeightEnd
                        For x2 As Integer = CellWidthStart To CellWidthEnd
                            Cur_Gray = _GrayImage(x2, y2)
                            If Cur_Gray > Cur_GrayMax Then
                                With _PixelIndex(x2, y2)
                                    Cur_Red = _SrcPixels(.Red)
                                    Cur_Green = _SrcPixels(.Green)
                                    Cur_Blue = _SrcPixels(.Blue)
                                End With
                                Cur_GrayMax = Cur_Gray
                            End If
                        Next
                    Next

                    With _PixelIndex(x, y)
                        _OutPixels(.Red) = Cur_Red
                        _OutPixels(.Green) = Cur_Green
                        _OutPixels(.Blue) = Cur_Blue
                    End With
                Next x

                RaiseEvent Processing(CSng(y / _SrcBitmapHeight * 10) + 90)
            Next y



        Else


            'Generate gray image
            CellWidthStart = Math.Max(0, MaskRegionBoundary.Left - BoxSizeHalf)
            CellWidthEnd = Math.Min(BitmapWidthUpper, MaskRegionBoundary.Right + BoxSizeHalf)
            CellHeightStart = Math.Max(0, MaskRegionBoundary.Top - BoxSizeHalf)
            CellHeightEnd = Math.Min(BitmapHeightUpper, MaskRegionBoundary.Bottom + BoxSizeHalf)


            If IsDoGrayScaling Then
                For y As Integer = CellHeightStart To CellHeightEnd
                    For x As Integer = CellWidthStart To CellWidthEnd
                        With _PixelIndex(x, y)
                            _GrayImage(x, y) = CByte(RGBToGray(_SrcPixels(.Red),
                                              _SrcPixels(.Green),
                                              _SrcPixels(.Blue)))
                        End With
                    Next x
                Next y
            Else
                For y As Integer = CellHeightStart To CellHeightEnd
                    For x As Integer = CellWidthStart To CellWidthEnd
                        With _PixelIndex(x, y)
                            _GrayImage(x, y) = _SrcPixels(.Red)
                        End With
                    Next x
                Next y
            End If


            With MaskRegionBoundary
                SelectRegionWidthStart = Math.Max(BoxSizeHalf, .Left)
                SelectRegionWidthEnd = Math.Min(BorderWidthUpper, .Right)
                SelectRegionHeightStart = Math.Max(BoxSizeHalf, .Top)
                SelectRegionHeightEnd = Math.Min(BorderHeightUpper, .Bottom)
            End With


            PixelCount = CInt((BoxSizeHalf * 2 + 1) ^ 2)
            For y As Integer = SelectRegionHeightStart To SelectRegionHeightEnd
                For x As Integer = SelectRegionWidthStart To SelectRegionWidthEnd

                    If _MaskPixels(_PixelIndex(x, y).Red) = 0 Then
                        Cur_GrayMax = 0 : Cur_Red = 0 : Cur_Green = 0 : Cur_Blue = 0
                        CellWidthStart = x - BoxSizeHalf
                        CellWidthEnd = x + BoxSizeHalf
                        CellHeightStart = y - BoxSizeHalf
                        CellHeightEnd = y + BoxSizeHalf

                        For y2 As Integer = CellHeightStart To CellHeightEnd
                            For x2 As Integer = CellWidthStart To CellWidthEnd
                                Cur_Gray = _GrayImage(x2, y2)
                                If Cur_Gray > Cur_GrayMax Then
                                    With _PixelIndex(x2, y2)
                                        Cur_Red = _SrcPixels(.Red)
                                        Cur_Green = _SrcPixels(.Green)
                                        Cur_Blue = _SrcPixels(.Blue)
                                    End With
                                    Cur_GrayMax = Cur_Gray
                                End If
                            Next
                        Next

                        With _PixelIndex(x, y)
                            _OutPixels(.Red) = Cur_Red
                            _OutPixels(.Green) = Cur_Green
                            _OutPixels(.Blue) = Cur_Blue
                        End With
                    End If
                Next x

                RaiseEvent Processing(CSng((y - SelectRegionHeightStart) /
                                      (SelectRegionHeightEnd - SelectRegionHeightStart + 1) * 90))
            Next y



            For y As Integer = 0 To BitmapHeightUpper
                For x As Integer = 0 To BitmapWidthUpper
                    Call Process_BoundaryRegion(BitmapWidthUpper, BitmapHeightUpper,
                                                BorderWidthUpper, BorderHeightUpper,
                                                x, y, BoxSizeHalf,
                                                CellWidthStart, CellWidthEnd,
                                                CellHeightStart, CellHeightEnd,
                                                CellWidthPixelCount, CellHeightPixelCount,
                                                PixelCount)


                    If _MaskPixels(_PixelIndex(x, y).Red) = 0 Then

                        Cur_GrayMax = 0 : Cur_Red = 0 : Cur_Green = 0 : Cur_Blue = 0
                        For y2 As Integer = CellHeightStart To CellHeightEnd
                            For x2 As Integer = CellWidthStart To CellWidthEnd
                                Cur_Gray = _GrayImage(x2, y2)
                                If Cur_Gray > Cur_GrayMax Then
                                    With _PixelIndex(x2, y2)
                                        Cur_Red = _SrcPixels(.Red)
                                        Cur_Green = _SrcPixels(.Green)
                                        Cur_Blue = _SrcPixels(.Blue)
                                    End With
                                    Cur_GrayMax = Cur_Gray
                                End If
                            Next
                        Next

                        With _PixelIndex(x, y)
                            _OutPixels(.Red) = Cur_Red
                            _OutPixels(.Green) = Cur_Green
                            _OutPixels(.Blue) = Cur_Blue
                        End With
                    End If
                Next x

                RaiseEvent Processing(CSng(y / _SrcBitmapHeight * 10) + 90)
            Next y
        End If

        Return OutPixelsToImage()

    End Function




    Public Function Crop(ByVal SourceImage As Image,
                    ByVal nWidth As Integer,
                    ByVal nHeight As Integer,
                    ByVal TargetX As Integer,
                    ByVal TargetY As Integer) As Bitmap

        Using bm As New Bitmap(SourceImage),
                Temp_bm As New Bitmap(nWidth, nHeight),
                g As Graphics = Graphics.FromImage(Temp_bm)

            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

            g.DrawImage(bm, New Rectangle(0, 0, nWidth, nHeight),
                        TargetX, TargetY, nWidth, nHeight,
                        GraphicsUnit.Pixel)

            Crop = CType(Temp_bm.Clone, Bitmap)
            Crop.Tag = SourceImage.Tag
        End Using
    End Function



    Public Function Create_MaskRectangle(CanvasWidth As Integer,
                              CanvasHeighT As Integer,
                              MaskRect As Rectangle) As Bitmap

        Using Temp_MaskBitmap As Bitmap = New Bitmap(CanvasWidth, CanvasHeighT),
                    Temp_grMaskBitmap As Graphics = Graphics.FromImage(Temp_MaskBitmap)

            Temp_grMaskBitmap.Clear(Color.White)
            Temp_grMaskBitmap.FillRectangle(New SolidBrush(Color.Black), MaskRect)

            Create_MaskRectangle = CType(Temp_MaskBitmap.Clone, Bitmap)
        End Using
    End Function

    Public Function Create_MaskImage(CanvasWidth As Integer,
                              CanvasHeight As Integer,
                              MaskRects() As Rectangle,
                              ROIShape() As String) As Bitmap

        Using Temp_MaskBitmap As Bitmap = New Bitmap(CanvasWidth, CanvasHeight),
                    Temp_grMaskBitmap As Graphics = Graphics.FromImage(Temp_MaskBitmap)

            Temp_grMaskBitmap.Clear(Color.White)
            For q As Integer = 0 To MaskRects.GetUpperBound(0)
                If ROIShape(q) = "rectangle" Then
                    Temp_grMaskBitmap.FillRectangle(New SolidBrush(Color.Black), MaskRects(q))
                ElseIf ROIShape(q) = "ellipse" Or ROIShape(q) = "circle" Then
                    Temp_grMaskBitmap.FillEllipse(New SolidBrush(Color.Black), MaskRects(q))
                End If
            Next
            Create_MaskImage = CType(Temp_MaskBitmap.Clone, Bitmap)
        End Using
    End Function


    Public Function Convert_RGBImage_To_RGBByteArray(ByVal SourceImage As Image,
                             Optional ByVal MaskImage As Image = Nothing,
                             Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                             Optional ByVal IsMaskAllRegionSelected As Boolean = True) As Byte(,,)


        If SourceImage Is Nothing Then Return Nothing

        Call ImageTo_SrcPixels(SourceImage, MaskImage)

        Dim Temp_3DByteArray(_SrcBitmapWidth - 1, _SrcBitmapHeight - 1, 2) As Byte

        If IsMaskAllRegionSelected Then
            For y As Integer = 0 To _SrcBitmapHeight - 1

                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        Temp_3DByteArray(x, y, 0) = _OutPixels(.Red)
                        Temp_3DByteArray(x, y, 1) = _OutPixels(.Green)
                        Temp_3DByteArray(x, y, 2) = _OutPixels(.Blue)
                    End With
                Next x
            Next y

        Else

            With MaskRegionBoundary
                x1 = .X
                y1 = .Y
                x2 = .X + .Width - 1
                y2 = .Y + .Height - 1
            End With


            For y As Integer = y1 To y2
                For x As Integer = x1 To x2
                    With _PixelIndex(x, y)
                        If _MaskPixels(.Blue) = 0 Then
                            Temp_3DByteArray(x, y, 0) = _OutPixels(.Red)
                            Temp_3DByteArray(x, y, 1) = _OutPixels(.Green)
                            Temp_3DByteArray(x, y, 2) = _OutPixels(.Blue)
                        End If
                    End With
                Next x

            Next y
        End If

        OutPixelsToImage()


        Return Temp_3DByteArray
    End Function


    Public Function RegionExtract_Convert_LabelIDMap_To_Image(
                                 ByVal LabelIDMap As Integer(,),
                                 Optional ByVal ObjectRed As Byte = 255,
                                 Optional ByVal ObjectGreen As Byte = 255,
                                 Optional ByVal ObjectBlue As Byte = 255) As Bitmap

        Using SourceImage As Image =
                New Bitmap(LabelIDMap.GetUpperBound(0) + 1, LabelIDMap.GetUpperBound(1) + 1)

            Call ImageTo_SrcPixels(SourceImage)

            For y As Integer = 0 To _SrcBitmapHeight - 1

                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        If LabelIDMap(x, y) <> 0 Then
                            _OutPixels(.Red) = ObjectRed
                            _OutPixels(.Green) = ObjectGreen
                            _OutPixels(.Blue) = ObjectBlue
                        End If
                    End With
                Next x

            Next y

            RegionExtract_Convert_LabelIDMap_To_Image = OutPixelsToImage()

        End Using

    End Function


    Public Function Convert_RGBByteArray_To_RGBImage(ByVal SrcByteArray(,,) As Byte) As Bitmap

        Using SourceImage As Image =
                New Bitmap(SrcByteArray.GetUpperBound(0) + 1, SrcByteArray.GetUpperBound(1) + 1)


            Call ImageTo_SrcPixels(SourceImage)

            For y As Integer = 0 To _SrcBitmapHeight - 1

                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        _OutPixels(.Red) = SrcByteArray(x, y, 0)
                        _OutPixels(.Green) = SrcByteArray(x, y, 1)
                        _OutPixels(.Blue) = SrcByteArray(x, y, 2)
                    End With
                Next x

            Next y

            Convert_RGBByteArray_To_RGBImage = OutPixelsToImage()

        End Using
    End Function





    Public Function Count_ColorPixelOfInterest_FromGrayArray(ByVal ImageGrayArray(,) As Byte,
                                                             ByVal ColorOfInterestByte As Byte,
                                                             ByVal X1 As Integer, Y1 As Integer,
                                                             ByVal X2 As Integer, Y2 As Integer) As Integer

        If X1 < 0 OrElse X2 > ImageGrayArray.GetUpperBound(0) OrElse
            Y1 < 0 OrElse Y2 > ImageGrayArray.GetUpperBound(1) Then
            Return -1
        End If

        Dim pixelCount As Long = 0
        For i As Integer = X1 To X2
            For j As Integer = Y1 To Y2
                If ImageGrayArray(i, j) = ColorOfInterestByte Then
                    pixelCount += 1
                End If
            Next j
        Next i
        Return pixelCount
    End Function

    Public Function Convert_GrayImage_To_GrayByteArray(ByVal SourceImage As Image,
                               Optional ByVal MaskImage As Image = Nothing,
                               Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                               Optional ByVal IsMaskAllRegionSelected As Boolean = True) As Byte(,)


        If SourceImage Is Nothing Then Return Nothing

        Call ImageTo_SrcPixels(SourceImage, MaskImage)

        Dim Temp_2DByteArray(_SrcBitmapWidth - 1, _SrcBitmapHeight - 1) As Byte

        If IsMaskAllRegionSelected Then
            For y As Integer = 0 To _SrcBitmapHeight - 1

                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        Temp_2DByteArray(x, y) = _OutPixels(.Blue)
                    End With
                Next x
            Next y

        Else

            Get_X1Y1X2Y2_From_Rectangle(MaskRegionBoundary, x1, y1, x2, y2)



            For y As Integer = y1 To y2
                For x As Integer = x1 To x2
                    With _PixelIndex(x, y)
                        If _MaskPixels(.Blue) = 0 Then
                            Temp_2DByteArray(x, y) = _OutPixels(.Blue)
                        End If
                    End With
                Next x

            Next y
        End If

        OutPixelsToImage()


        Return Temp_2DByteArray
    End Function


    Private Sub Get_X1Y1X2Y2_From_Rectangle(ByVal SourceRectangle As Rectangle,
                                            ByRef Ret_x1 As Integer,
                                            ByRef Ret_y1 As Integer,
                                            ByRef Ret_x2 As Integer,
                                            ByRef Ret_y2 As Integer)
        Ret_x1 = SourceRectangle.Left
        Ret_y1 = SourceRectangle.Top
        Ret_x2 = SourceRectangle.X + SourceRectangle.Width - 1
        Ret_y2 = SourceRectangle.Y + SourceRectangle.Height - 1
    End Sub


    Public Function RegionExtract_Create_WhiteObjecDilatedImage(ByRef SourceLabelIDMap(,) As Integer,
                                                                 SourceIDINfoIndex As Integer,
                                                                 x1 As Integer,
                                                                 x2 As Integer,
                                                                 y1 As Integer,
                                                                 y2 As Integer,
                                                                 DilateBoxSize As Integer,
                                                                 MaximizeRepeat As Integer) As Image


        Using ObjectImage As Image = RegionExtract_Crop_WhiteObjectImage(SourceLabelIDMap,
                                                                                   SourceIDINfoIndex,
                                                                                   x1, x2, y1, y2)
            Dim ObjectDilatedImage As Image = Maximize(ObjectImage, DilateBoxSize)

            For q As Integer = 0 To MaximizeRepeat - 2
                ObjectDilatedImage = Maximize(ObjectDilatedImage, DilateBoxSize)
            Next

            RegionExtract_Create_WhiteObjecDilatedImage = DeepCopy(ObjectDilatedImage)
        End Using

    End Function


    Public Function RegionExtract_Crop_WhiteObjectImage(ByRef SourceLabelIDMap(,) As Integer,
                                                             SourceIDINfoIndex As Integer,
                                                             x1 As Integer,
                                                             x2 As Integer,
                                                             y1 As Integer,
                                                             y2 As Integer) As Image

        Dim IDMapObject(,) As Byte
        IDMapObject = RegionExtract_Crop_LabelIDMap_To_2DByte(
                                       SourceLabelIDMap, SourceIDINfoIndex,
                                       x1, x2, y1, y2)

        Return Convert_GrayByteArray_To_RGBImage(IDMapObject)

    End Function


    Public Function GrayScaling(ByVal SourceImage As Image,
                                Optional ByVal MaskImage As Image = Nothing,
                                Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                                Optional ByVal IsMaskAllRegionSelected As Boolean = True) As Bitmap


        If SourceImage Is Nothing Then Return CType(SourceImage, Bitmap)


        Call ImageTo_SrcPixels(SourceImage, MaskImage)

        Dim Gray As Byte


        If IsMaskAllRegionSelected Then
            For y As Integer = 0 To _SrcBitmapHeight - 1

                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        Gray = RGBToGray(_SrcPixels(.Red),
                                          _SrcPixels(.Green),
                                          _SrcPixels(.Blue))

                        _OutPixels(.Red) = Gray
                        _OutPixels(.Green) = Gray
                        _OutPixels(.Blue) = Gray
                    End With
                Next x

                If (CInt(y / _SrcBitmapHeight * 100) Mod 5) = 0 Then
                    RaiseEvent Processing(CSng(y / _SrcBitmapHeight * 100))
                End If

            Next y

        Else

            With MaskRegionBoundary
                x1 = .X
                y1 = .Y
                x2 = .X + .Width - 1
                y2 = .Y + .Height - 1
            End With


            For y As Integer = y1 To y2
                For x As Integer = x1 To x2
                    With _PixelIndex(x, y)
                        If _MaskPixels(.Blue) = 0 Then
                            Gray = RGBToGray(_SrcPixels(.Red),
                                              _SrcPixels(.Green),
                                              _SrcPixels(.Blue))

                            _OutPixels(.Red) = Gray
                            _OutPixels(.Green) = Gray
                            _OutPixels(.Blue) = Gray
                        End If
                    End With
                Next x

                If (CInt((y - y1) / (y2 - y1 + 1) * 100) Mod 5) = 0 Then
                    RaiseEvent Processing(CSng((y - y1) / (y2 - y1 + 1) * 100))
                End If

            Next y
        End If


        RaiseEvent Completed(_LapseAsMilliSecond)

        Return OutPixelsToImage()
    End Function


    Public Sub Draw_Text_Outlined_Graphics(SourceGraphics As Graphics,
                               ByVal x As Integer,
                               ByVal y As Integer,
                               ByVal TextString As String,
                               ByVal TextFont As Font,
                               ByVal InnerColor As Color,
                               ByVal OuterColor As Color)



        With SourceGraphics
            Dim DrawingTextBrush As Brush = New SolidBrush(OuterColor)
            .DrawString(TextString, TextFont, DrawingTextBrush, x - 1, y - 1)
            .DrawString(TextString, TextFont, DrawingTextBrush, x - 1, y + 1)
            .DrawString(TextString, TextFont, DrawingTextBrush, x + 1, y + 1)
            .DrawString(TextString, TextFont, DrawingTextBrush, x + 1, y - 1)

            DrawingTextBrush = New SolidBrush(InnerColor)
            .DrawString(TextString, TextFont, DrawingTextBrush, x, y)

            DrawingTextBrush.Dispose()
            DrawingTextBrush = Nothing
        End With
    End Sub



    Public Function Invert(ByVal SourceImage As Image,
                                Optional ByVal MaskImage As Image = Nothing,
                                Optional ByVal MaskRegionBoundary As Rectangle = Nothing,
                                Optional ByVal IsMaskAllRegionSelected As Boolean = True) As Bitmap


        If SourceImage Is Nothing Then Return CType(SourceImage, Bitmap)


        Call ImageTo_SrcPixels(SourceImage, MaskImage)

        If IsMaskAllRegionSelected Then

            For y As Integer = 0 To _SrcBitmapHeight - 1
                For x As Integer = 0 To _SrcBitmapWidth - 1

                    With _PixelIndex(x, y)
                        _OutPixels(.Red) = CByte(255 - _SrcPixels(.Red))
                        _OutPixels(.Green) = CByte(255 - _SrcPixels(.Green))
                        _OutPixels(.Blue) = CByte(255 - _SrcPixels(.Blue))
                    End With

                Next x

                RaiseEvent Processing(CSng(y / _SrcBitmapHeight * 100))
            Next y

        Else
            With MaskRegionBoundary
                x1 = .X
                y1 = .Y
                x2 = .X + .Width - 1
                y2 = .Y + .Height - 1
            End With


            For y As Integer = y1 To y2
                For x As Integer = x1 To x2
                    With _PixelIndex(x, y)
                        If _MaskPixels(.Blue) = 0 Then
                            _OutPixels(.Red) = CByte(255 - _SrcPixels(.Red))
                            _OutPixels(.Green) = CByte(255 - _SrcPixels(.Green))
                            _OutPixels(.Blue) = CByte(255 - _SrcPixels(.Blue))
                        End If
                    End With

                Next x

                RaiseEvent Processing(CSng((y - y1) / (y2 - y1 + 1) * 100))
            Next y
        End If


        Return OutPixelsToImage()
    End Function


End Class


