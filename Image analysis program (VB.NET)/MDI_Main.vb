﻿Public Class MDI_Main
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

    End Sub

    Private Sub Me_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Application.Exit()
    End Sub

    Private Sub MDImain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'DeleteSomeFiles()
        Threading.Thread.CurrentThread.CurrentCulture =
                        New Globalization.CultureInfo("en-US", True)


        Application.EnableVisualStyles()

        'Menu_About_Click(Nothing, Nothing)

        With Frm_ImageAnalysis
            .Show()
            .Left = 0
            .Top = 0
            .BringToFront()
        End With

        With Frm_ImageViewer
            .Show()
            .Left = Frm_ImageAnalysis.Width + 2
            .Top = 0
            .Width = 900
            .BringToFront()
        End With

        With Frm_BatchProcessing
            .Show()
            .Left = 0
            .Top = Frm_ImageAnalysis.Height + 2
            .BringToFront()
        End With

    End Sub

    Sub DeleteSomeFiles()
        Dim targetfolder As String = Application.StartupPath +
                                    "Training images\C elegans (small set)\1"

        Dim files() As String = MyFileSysEng.Get_AllFileNamesInFolder(targetfolder)

        For q As Integer = 0 To files.GetUpperBound(0)
            If Rnd() > 0.5 Then
                IO.File.Delete(targetfolder + "\" + files(q))
            End If
        Next

    End Sub

    Private Sub Menu_Exit_Click(sender As Object, e As EventArgs) Handles Menu_Exit.Click
        Application.Exit()
    End Sub

    Private Sub Menu_BatchProcessing_Click(sender As Object, e As EventArgs) Handles Menu_BatchProcessing.Click
        With Frm_BatchProcessing
            .Show()
            .BringToFront()
        End With

    End Sub

    Private Sub Cmd_BatchProcessing_Click(sender As Object, e As EventArgs) Handles Cmd_BatchProcessing.Click
        Menu_BatchProcessing_Click(Nothing, Nothing)
    End Sub

    Private Sub Menu_ImageProcessing_Click(sender As Object, e As EventArgs) Handles Menu_ImageProcessing.Click
        With Frm_ImageAnalysis
            .Show()
            .BringToFront()
        End With
    End Sub

    Private Sub Cmd_ImageProcessing_Click(sender As Object, e As EventArgs) Handles Cmd_ImageProcessing.Click
        Menu_ImageProcessing_Click(Nothing, Nothing)
    End Sub

    Private Sub Menu_About_Click(sender As Object, e As EventArgs) Handles Menu_About.Click
        With Frm_About
            .Show()
            .BringToFront()
        End With
    End Sub

    Private Sub Menu_Copy_Click(sender As Object, e As EventArgs) Handles Menu_Copy.Click
        If Frm_ImageViewer.Canvas.Image IsNot Nothing Then
            Clipboard.SetImage(Frm_ImageViewer.Canvas.Image)
        End If
    End Sub

    Private Sub Menu_Open_Click(sender As Object, e As EventArgs) Handles Menu_Open.Click
        Frm_ImageAnalysis.Cmd_SelectFile_Click(Nothing, Nothing)
    End Sub

End Class