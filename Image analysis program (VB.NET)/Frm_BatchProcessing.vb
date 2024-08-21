Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Frm_BatchProcessing
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MdiParent = MDI_Main
    End Sub
    Private Sub Me_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        Me.Hide()
    End Sub


    Private Sub Cmd_Clear_Click(sender As Object, e As EventArgs) Handles Cmd_Clear.Click
        FileDDBox.Clear()
        TextBox_Msg.Text = ""
    End Sub

    Private Sub Cmd_RunBatchProcessing_Click(sender As Object, e As EventArgs) Handles Cmd_RunBatchProcessing.Click
        If FileDDBox.FilesCount = 0 Then
            MsgBox("No files found", MsgBoxStyle.Critical, "Batch processing")
            Exit Sub
        End If

        If Cmd_RunBatchProcessing.Text = "Stop processing" Then
            MyAnalyzer.Stop_Processing()
            Exit Sub
        End If


        Cmd_RunBatchProcessing.Text = "Stop processing"


        With Frm_ImageViewer
            .Cmd_QuickAnim.Text = "No animation"
            .Timer_Animation.Enabled = False
        End With


        Dim analysisInterval As Integer
        Try
            analysisInterval = CInt(Text_Interval.Text)
        Catch ex As Exception
            Text_Interval.Text = "10"
            analysisInterval = 10
        End Try


        Dim OutStr As New StringBuilder
        Dim OutElapsed As New StringBuilder
        Dim watch As Stopwatch = Stopwatch.StartNew()


        Frm_ImageAnalysis.Read_ProcessingParams()

        Dim beginDate As Date = Get_DateFromFilename(FileDDBox.FileName(0))


        For CurIndex As Integer = 0 To FileDDBox.FilesCount - 1 Step analysisInterval
            FileDDBox.ListView.SelectedIndices.Clear()
            FileDDBox.ListView.SelectedIndices.Add(CurIndex)
            FileDDBox.ListView.EnsureVisible(CurIndex)


            TextBox_Msg.Text = "Processing  " + FileDDBox.FileName(CurIndex) +
                              "    [" + (CurIndex + 1).ToString.Trim + " of " +
                              FileDDBox.FilesCount.ToString.Trim + "]"
            TextBox_Msg.Refresh()
            Application.DoEvents()

            watch.Reset()
            watch.Start()

            With MyAnalyzer
                Frm_ImageAnalysis.Text_SourceImage.Text =
                        FileDDBox.Get_FullFileNames(CurIndex)

                .Image_Source = MyImgProc.Image_FromFile(
                                FileDDBox.Get_FullFileNames(CurIndex))

                If CurIndex = 0 Then
                    Dim RetMsg As String = Read_ROI(FileDDBox.Get_FullFileNames(0))
                    If RetMsg <> "" Then
                        Set_DefaultROI(MyAnalyzer.Image_Source.Width, MyAnalyzer.Image_Source.Height)
                    End If

                    OutStr.Append("File name,Time(day),Processing time (ms),")
                    For q As Integer = 0 To ROI.GetUpperBound(0) - 1
                        OutStr.Append(Get_ROIinfo(q, False) + ",")
                    Next
                    OutStr.Append(Get_ROIinfo(ROI.GetUpperBound(0), False) + vbCrLf)

                End If


                If .ProcessingParams.IsResize Then
                    If .Image_Source.Width <> .ProcessingParams.ResizeWidth OrElse
                        .Image_Source.Height <> .ProcessingParams.ResizeHeight Then
                        .Image_Source = MyImgProc.Resize(.Image_Source,
                                                        .ProcessingParams.ResizeWidth,
                                                        .ProcessingParams.ResizeHeight)
                    End If
                End If

                Frm_ImageViewer.Display_Image(.Image_Source)
            End With





            Dim RetString As String
            RetString = MyAnalyzer.AreaAnalyzer(
                                    FileDDBox.Get_FullFileNames(CurIndex),
                                    False, True, ",")

            Frm_ImageViewer.Display_Image(MyAnalyzer.Image_BlackWhite)

            Dim curDate As Date = Get_DateFromFilename(FileDDBox.FileName(CurIndex))
            Dim duration As TimeSpan = curDate - beginDate

            watch.Stop()

            OutStr.Append(FileDDBox.FileName(CurIndex) + "," +
                         Format(duration.TotalDays, "0.000") + "," +
                         watch.ElapsedMilliseconds.ToString + "," +
                         RetString)

            Application.DoEvents()

            If MyAnalyzer.UserStopped Then Exit For



        Next



        Try
            Dim OutputFile As String = MyFileSysEng.Get_OnlyPath_FullFileName(
                                        FileDDBox.Get_FullFileNames(0)) +
                                        Path.DirectorySeparatorChar + OutputFilename

            File.WriteAllText(OutputFile, OutStr.ToString)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "File save")
        End Try



        If MyAnalyzer.UserStopped Then
            TextBox_Msg.Text = "Batch processing terminated by user"
        Else
            TextBox_Msg.Text = "Batch processing completed"
        End If
        Cmd_RunBatchProcessing.Text = "Run batch processing"
    End Sub

    Private Sub Frm_BatchProcessing_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Cmd_SelectFolder_Click(sender As Object, e As EventArgs) Handles Cmd_SelectFolder.Click
        With FolderDialog
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                FileDDBox.Clear()
                FileDDBox.Add(MyFileSysEng.Get_AllFullFileNamesInFolder(.SelectedPath))
                FileDDBox.ListView.Items(0).Selected = True
            End If
        End With

    End Sub


    Private Sub FileDDBox_FileSelected(FileName As String, Index As Integer, DirectoryPath As String) Handles FileDDBox.FileSelected
        TextBox_Msg.Text = "The " + Get_nth(Index + 1) + " file selected among the " +
                            FileDDBox.FilesCount.ToString.Trim + " files"
        Frm_ImageAnalysis.Text_SourceImage.Text = DirectoryPath + PathDelimit + FileName
        Frm_ImageAnalysis.Cmd_ReadImage_Click(Nothing, Nothing)
    End Sub

    Private Sub FileDDBox_Load(sender As Object, e As EventArgs) Handles FileDDBox.Load

    End Sub

    Private Sub FileDDBox_FilesAdded(FileCount As Integer) Handles FileDDBox.FilesAdded
        TextBox_Msg.Text = "Total " + FileCount.ToString.Trim + " files added"
    End Sub

    Private Sub FileDDBox_FileClicked(FileName As String, Index As Integer, DirectoryPath As String) Handles FileDDBox.FileClicked
        Frm_ImageAnalysis.Text_SourceImage.Text = DirectoryPath + "\" + FileName
        Frm_ImageAnalysis.Cmd_ReadImage_Click(Nothing, Nothing)
    End Sub
End Class