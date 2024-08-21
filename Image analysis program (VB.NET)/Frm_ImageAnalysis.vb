Imports System.IO

Public Class Frm_ImageAnalysis

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        Me.MdiParent = MDI_Main
    End Sub


    Public Sub Read_ProcessingParams()
        With MyAnalyzer.ProcessingParams
            .IsResize = Check_Resize.Checked

            Try
                .ResizeWidth = CInt(Text_Resize_Width.Text)
            Catch
                Text_Resize_Width.Text = "1024"
                .ResizeWidth = CInt(Text_Resize_Width.Text)
            End Try

            Try
                .ResizeHeight = CInt(Text_Resize_Height.Text)
            Catch
                Text_Resize_Height.Text = "768"
                .ResizeHeight = CInt(Text_Resize_Height.Text)
            End Try

            Try
                .AdaptiveBoxSize = CInt(Text_Adaptive_BoxSize.Text)
            Catch
                Text_Adaptive_BoxSize.Text = "25"
                .AdaptiveBoxSize = CInt(Text_Adaptive_BoxSize.Text)
            End Try

            Try
                .AdaptiveThreshold = CInt(Text_Adaptive_Threshold.Text)
            Catch
                Text_Adaptive_BoxSize.Text = "25"
                .AdaptiveThreshold = CInt(Text_Adaptive_Threshold.Text)
            End Try

            .IsObjectWhite = Not (Check_AnimalInBlack.Checked)
        End With
    End Sub

    Private Sub Me_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        Me.Hide()
    End Sub

    Private Sub Frm_ImageAnalysis_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Text_SourceImage.Text = Application.StartupPath +
        '                        "Test source images\" +
        '                         "29.jpg"
        Text_SourceImage.Text = "D:\My Imaging\2023-08-22 (Day and night imaging, 3 LEDs)\cam100\2023-09-03 (12-26-06-463)-(12-26-06-859).jpg"


        If MyFileSysEng.FileExists(Text_SourceImage.Text) Then
            Cmd_ReadImage_Click(Nothing, Nothing)
        Else
            Frm_ImageViewer.Canvas.Image = New Bitmap(100, 100)
        End If
    End Sub



    Private Sub Cmd_RunAnalysis_Click(sender As Object, e As EventArgs) Handles Cmd_RunAnalysis.Click

        If MyFileSysEng.FileExists(Text_SourceImage.Text) = False Then
            MsgBox("Image file not found!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        Cmd_RunAnalysis.Text = "Stop processing"
        Cmd_RunAnalysis.Refresh()


        Read_ProcessingParams()

        MyAnalyzer.AreaAnalyzer(Text_SourceImage.Text, True, True, ",")


        If MyAnalyzer.UserStopped = False Then
            With Frm_ImageViewer
                If .Timer_Animation.Enabled = False Then
                    .Cmd_QuickAnim_Click(Nothing, Nothing)
                End If
            End With
        End If

        Cmd_RunAnalysis.Text = "Run analysis"
    End Sub



    Private Sub Cmd_Test_AdaptiveThresholding_Click(sender As Object, e As EventArgs) Handles Cmd_Test_AdaptiveThresholding.Click

        If MyFileSysEng.FileExists(Text_SourceImage.Text) = False Then
            MsgBox("Image file not found!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        With Frm_ImageViewer
            If .Cmd_QuickAnim.Text <> "Animate" Then
                .Cmd_QuickAnim_Click(Nothing, Nothing)
            End If
        End With

        Read_ProcessingParams()

        MyAnalyzer.AreaAnalyzer(Text_SourceImage.Text, True, False, ",")

        Frm_ImageViewer.Canvas.Image = CType(MyAnalyzer.Image_BlackWhite.Clone, Bitmap)

        With Frm_ImageViewer
            If .Cmd_QuickAnim.Text = "Animate" Then
                .Cmd_QuickAnim_Click(Nothing, Nothing)
            End If
        End With

    End Sub





    Private Sub Text_SourceImage_DragDrop(sender As Object, e As DragEventArgs) Handles Text_SourceImage.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim MyFiles() As String
            MyFiles = CType(e.Data.GetData(DataFormats.FileDrop), String())
            sender.Text = MyFiles(0)

            Cmd_ReadImage_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub Text_SourceImage_DragEnter(sender As Object, e As DragEventArgs) Handles Text_SourceImage.DragEnter
        Dim FileExtStr As String = ".jpg .jpeg .bmp .png .gif .tif .tiff"

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim MyFiles() As String

            MyFiles = CType(e.Data.GetData(DataFormats.FileDrop), String())

            If InStr(FileExtStr, Path.GetExtension(MyFiles(0)).ToLower) <> 0 Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        End If


    End Sub

    Public Sub Cmd_SelectFile_Click(sender As Object, e As EventArgs) Handles Cmd_SelectFile.Click
        Static Pre_Path As String = ""

        With Frm_ImageViewer
            .Cmd_QuickAnim.Text = "No animation"
            .Timer_Animation.Enabled = False
        End With


        With OpenFileDialog
            If Pre_Path = "" Then
                If MyFileSysEng.FolderExists(
                        MyFileSysEng.Get_OnlyPath_FullFileName(
                                Text_SourceImage.Text)) Then
                    Pre_Path = MyFileSysEng.Get_OnlyPath_FullFileName(
                                        Text_SourceImage.Text)
                Else
                    Pre_Path = .InitialDirectory
                End If
            End If
            .InitialDirectory = Pre_Path

            .FileName = ""
            .Title = "Select image file"
            .CheckFileExists = True
            .CheckPathExists = True
            .ShowReadOnly = False
            .Filter = "Image file|*.jpeg;*.jpg;*.png;*.bmp;*.gif;*.tiff;*.tif"
            .FilterIndex = 1

            If .ShowDialog = DialogResult.OK Then
                Text_SourceImage.Text = .FileName

                Cmd_ReadImage_Click(Nothing, Nothing)

                Pre_Path = MyFileSysEng.Get_OnlyPath_FullFileName(.FileName)
            End If
        End With
    End Sub


    Public Sub Cmd_ReadImage_Click(sender As Object, e As EventArgs) Handles Cmd_ReadImage.Click

        If MyFileSysEng.FileExists(Text_SourceImage.Text) = False Then
            MsgBox("Image file not found!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        With Frm_ImageViewer
            .Cmd_QuickAnim.Text = "No animation"
            .Timer_Animation.Enabled = False
        End With


        MyAnalyzer.Image_Source = MyImgProc.Image_FromFile(Text_SourceImage.Text)

        Dim RetMsg As String = Read_ROI(Text_SourceImage.Text)
        If RetMsg <> "" Then
            Set_DefaultROI(MyAnalyzer.Image_Source.Width, MyAnalyzer.Image_Source.Height)
        End If

        Read_ProcessingParams()


        With MyAnalyzer.ProcessingParams
            If .IsResize Then
                If .ResizeWidth <> MyAnalyzer.Image_Source.Width OrElse
                        .ResizeHeight <> MyAnalyzer.Image_Source.Height Then

                    MyAnalyzer.Image_Source = MyImgProc.Resize(MyAnalyzer.Image_Source,
                                                               .ResizeWidth, .ResizeHeight)
                End If
            End If
        End With


        Frm_ImageViewer.Display_Image(MyAnalyzer.Image_Source)
    End Sub

    Private Sub Text_SourceImage_TextChanged(sender As Object, e As EventArgs) Handles Text_SourceImage.TextChanged

    End Sub
End Class



