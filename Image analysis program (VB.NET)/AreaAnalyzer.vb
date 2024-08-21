Imports System.Text

Public Class AreaAnalyzer
    Public Structure Type_ProcessingParameters
        Dim IsResize As Boolean
        Dim ResizeWidth As Integer
        Dim ResizeHeight As Integer
        Dim AdaptiveBoxSize As Integer
        Dim AdaptiveThreshold As Integer
        Dim IsObjectWhite As Boolean
    End Structure

    Public Image_Source,
           Image_Gray,
           Image_BlackWhite,
           Image_BlackWhite_Selected As Image

    Public UserStopped As Boolean = False

    Public ProcessingParams As Type_ProcessingParameters


    Public Sub New()

    End Sub

    Public Event ProgressStatus(Msg As String, ProgressValue As Integer)


    Public Sub Stop_Processing()
        UserStopped = True
    End Sub


    Public Function AreaAnalyzer(SourceImageFullFilename As String,
                                 IsReadFile As Boolean,
                                 IsComputeArea As Boolean,
                                 DelimitChar As String) As String

        UserStopped = False


        RaiseEvent ProgressStatus("Creating output folders...", 0)



        RaiseEvent ProgressStatus("Preparing image analysis...", 5)
        If IsReadFile Then
            Image_Source = MyImgProc.Image_FromFile(SourceImageFullFilename)

            If ProcessingParams.IsResize Then
                If Image_Source.Width <> ProcessingParams.ResizeWidth OrElse
                    Image_Source.Height <> ProcessingParams.ResizeHeight Then
                    Image_Source = MyImgProc.Resize(Image_Source,
                                                    ProcessingParams.ResizeWidth,
                                                    ProcessingParams.ResizeHeight)
                End If
            End If
        End If




        RaiseEvent ProgressStatus("Blurring and grayscaling...", 10)

        Dim MaskImage As Image

        Dim MaskRects(ROI.GetUpperBound(0)) As Rectangle
        Dim ROI_Shape(ROI.GetUpperBound(0)) As String
        For q As Integer = 0 To ROI.GetUpperBound(0)
            MaskRects(q) = ROI(q).Boundary
            ROI_Shape(q) = ROI(q).Shape
        Next


        Dim ROI_Rect As New Rectangle(0, 0, Image_Source.Width, Image_Source.Height)

        MaskImage = MyImgProc.Create_MaskImage(Image_Source.Width, Image_Source.Height,
                                               MaskRects, ROI_Shape)


        Image_Gray = MyImgProc.GrayScaling(Image_Gray, MaskImage, ROI_Rect, False)
        If UserStopped Then
            RaiseEvent ProgressStatus("Processing terminated by a user", 100)
            Return ""
        End If
        Application.DoEvents()


        Image_Gray = MyImgProc.BoxAveraging_ByIntegralMap(Image_Source, 1,
                                                          MaskImage, ROI_Rect, False)
        If UserStopped Then
            RaiseEvent ProgressStatus("Processing terminated by a user", 100)
            Return ""
        End If
        Application.DoEvents()



        RaiseEvent ProgressStatus("Applying adaptive thresholding...", 20)
        Image_BlackWhite = MyImgProc.BWLeveling_Using_AdaptiveThreshold(
                                                   Image_Gray,
                                                   ProcessingParams.AdaptiveBoxSize,
                                                   ProcessingParams.IsObjectWhite,
                                                   ProcessingParams.AdaptiveThreshold,
                                                   MaskImage, ROI_Rect, False)
        Image_BlackWhite = MyImgProc.Invert(Image_BlackWhite, MaskImage, ROI_Rect, False)

        Image_BlackWhite_Selected = MyImgProc.FillRegion(Image_BlackWhite, 0, 0, 0, False,
                                                MaskImage, ROI_Rect, False)

        'Clipboard.SetImage(Image_BlackWhite)

        ' Clipboard.SetImage(Image_BlackWhite_Selected)


        If UserStopped Then
            RaiseEvent ProgressStatus("Processing terminated by a user", 100)
            Return ""
        End If
        Application.DoEvents()


        Dim OutStr As New StringBuilder

        If IsComputeArea Then
            Dim Image_BlackWhite_SelectedArray(,) As Byte =
                    MyImgProc.Convert_GrayImage_To_GrayByteArray(Image_BlackWhite_Selected)

            For q As Integer = 0 To ROI.GetUpperBound(0)

                MaskRects(q) = ROI(q).Boundary
                ROI_Shape(q) = ROI(q).Shape

                With ROI(q).Boundary

                    Dim PixelCount As Integer =
                                MyImgProc.Count_ColorPixelOfInterest_FromGrayArray(
                                                Image_BlackWhite_SelectedArray,
                                                255,
                                               .X, .Y, .X + .Width - 1, .Y + .Height - 1)

                    OutStr.Append(PixelCount.ToString.Trim + DelimitChar)
                End With

            Next

            OutStr.Append(vbCrLf)

        Else
            OutStr.Append("")
        End If


        RaiseEvent ProgressStatus("Analysis completed...", 100)


        Return OutStr.ToString
    End Function

End Class
