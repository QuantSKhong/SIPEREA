Imports System.IO
Imports System.Drawing.Imaging


Public Class Frm_ImageViewer


    Dim IsFormLoading As Boolean = True
    Private FileSaveDialog As New SaveFileDialog
    Dim m_PanStartPoint As Point
    Private _DrawingFont As New Font("Microsoft Sans Serif", 12,
                                    FontStyle.Regular, GraphicsUnit.Pixel)

    Public SourceImage As Image


    Private Sub Me_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        Me.Hide()
    End Sub


    Private Sub Frm_ImageViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Combo_AnimInterval.Text = "500"
        IsFormLoading = False

        Canvas.PanMode = True
    End Sub


    Public Sub New()
        InitializeComponent()

        Me.MdiParent = MDI_Main
    End Sub



    Private Sub Cmd_Copy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cmd_Copy.Click
        Copy()
    End Sub


    Public Sub Copy()
        Try
            Clipboard.Clear()
            Clipboard.SetImage(Canvas.Image)
        Catch
            MsgBox("Failed to copy image!", MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Sub Display_Image(SourceImage As Image)
        Canvas.Image = SourceImage.Clone

        DrawRegions()
    End Sub

    Public Sub Cmd_Zoom100_Click(sender As Object, e As EventArgs) Handles Cmd_Zoom100.Click
        Canvas.ZoomFactor = 1
    End Sub

    Public Sub Cmd_ZoomIn_Click(sender As Object, e As EventArgs) Handles Cmd_ZoomIn.Click
        Canvas.ZoomIn()
    End Sub

    Public Sub Cmd_ZoomOut_Click(sender As Object, e As EventArgs) Handles Cmd_ZoomOut.Click
        Canvas.ZoomOut()
    End Sub

    Public Sub Menu_ResetZoom_Click(sender As Object, e As EventArgs) Handles Menu_ResetZoom.Click
        If Menu_Stretchtofit.Checked Then
            Call Menu_Stretchtofit_Click(Nothing, Nothing)
        End If

        Canvas.Origin = New Point(0, 0)
        Canvas.ZoomFactor = 1
    End Sub

    Public Sub Menu_Stretchtofit_Click(sender As Object, e As EventArgs) Handles Menu_Stretchtofit.Click
        Menu_Stretchtofit.Checked = Not (Menu_Stretchtofit.Checked)
        Canvas.StretchImageToFit = Menu_Stretchtofit.Checked

        Cmd_Zoom100.Enabled = Not (Menu_Stretchtofit.Checked)
        Cmd_ZoomIn.Enabled = Not (Menu_Stretchtofit.Checked)
        Cmd_ZoomOut.Enabled = Not (Menu_Stretchtofit.Checked)
    End Sub

    Public Sub Menu_Fittoscreen_Click(sender As Object, e As EventArgs) Handles Menu_Fittoscreen.Click
        If Menu_Stretchtofit.Checked Then
            Call Menu_Stretchtofit_Click(Nothing, Nothing)
        End If
        Canvas.fittoscreen()

    End Sub


    Public Sub Cmd_QuickAnim_Click(sender As Object, e As EventArgs) Handles Cmd_QuickAnim.Click
        If Timer_Animation.Enabled = False Then
            Cmd_QuickAnim.Text = "Animate"

            Try
                Timer_Animation.Interval = CInt(Combo_AnimInterval.Text)
                Timer_Animation.Enabled = True
            Catch
            End Try
        Else
            Cmd_QuickAnim.Text = "No animation"
            Timer_Animation.Enabled = False
        End If
    End Sub


    Private Sub Timer_Animation_Tick(sender As Object, e As EventArgs) Handles Timer_Animation.Tick
        Static CurrentImageNum As Integer = 0

        If CurrentImageNum = 0 Then
            Canvas.Image = CType(MyAnalyzer.Image_Source, Image)
            CurrentImageNum = 1
        Else
            Canvas.Image = CType(MyAnalyzer.Image_BlackWhite, Image)
            CurrentImageNum = 0
        End If

        DrawRegions()
    End Sub


    Private Sub Combo_AnimInterval_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Combo_AnimInterval.SelectedIndexChanged
        If IsFormLoading Then Exit Sub

        Timer_Animation.Interval = CInt(Combo_AnimInterval.Text)
    End Sub


    Private Sub Frm_ImageViewer_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        Canvas.Width = Me.ClientSize.Width
        Canvas.Height = Me.ClientSize.Height - ToolStrip.Height
    End Sub

    Private Sub Cmd_ShowRegions_Click(sender As Object, e As EventArgs) Handles Cmd_ShowRegions.Click
        If Cmd_ShowRegions.Text = "Show regions" Then
            Cmd_ShowRegions.Text = "Hide regions"
        Else
            Cmd_ShowRegions.Text = "Show regions"
        End If

        Display_Image(MyAnalyzer.Image_Source)
    End Sub

    Private Sub Cmd_ShowLabels_Click(sender As Object, e As EventArgs) Handles Cmd_ShowLabels.Click
        If Cmd_ShowLabels.Text = "Show labels" Then
            Cmd_ShowLabels.Text = "Hide labels"
        Else
            Cmd_ShowLabels.Text = "Show labels"
        End If

        Display_Image(MyAnalyzer.Image_Source)
    End Sub

    Public Sub DrawRegions()
        If ROI Is Nothing OrElse Canvas.Image Is Nothing Then Exit Sub


        Using bm As New Bitmap(Canvas.Image), GraphBox As Graphics = Graphics.FromImage(bm)


            For q As Integer = 0 To ROI.GetUpperBound(0)
                If Cmd_ShowRegions.Text = "Show regions" Then
                    If ROI(q).Shape = "rectangle" Then
                        GraphBox.DrawRectangle(Pens.Red, ROI(q).Boundary)
                    Else
                        GraphBox.DrawEllipse(Pens.Red, ROI(q).Boundary)
                    End If
                End If


                If Cmd_ShowLabels.Text = "Show labels" Then
                    Call MyImgProc.Draw_Text_Outlined_Graphics(
                                     GraphBox, ROI(q).Boundary.Left, ROI(q).Boundary.Top - 25,
                                      "[" & (q + 1).ToString.Trim & "] " &
                                      Get_ROIinfo(q, False, " "),
                                     _DrawingFont, Color.Yellow, Color.Black)
                End If

            Next


            Canvas.Image = CType(bm.Clone, Bitmap)
        End Using
    End Sub
End Class