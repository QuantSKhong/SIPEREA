<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_ImageViewer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(Frm_ImageViewer))
        ToolStrip = New ToolStrip()
        Cmd_Copy = New ToolStripButton()
        ToolStripSeparator1 = New ToolStripSeparator()
        Cmd_Zoom100 = New ToolStripButton()
        Cmd_ZoomIn = New ToolStripButton()
        Cmd_ZoomOut = New ToolStripButton()
        ToolStripDropDownButton1 = New ToolStripDropDownButton()
        Menu_ResetZoom = New ToolStripMenuItem()
        Menu_Stretchtofit = New ToolStripMenuItem()
        Menu_Fittoscreen = New ToolStripMenuItem()
        ToolStripSeparator3 = New ToolStripSeparator()
        Cmd_QuickAnim = New ToolStripMenuItem()
        ToolStripLabel1 = New ToolStripLabel()
        Combo_AnimInterval = New ToolStripComboBox()
        Cmd_ShowRegions = New ToolStripButton()
        ToolStripSeparator2 = New ToolStripSeparator()
        Cmd_ShowLabels = New ToolStripButton()
        Timer_Animation = New Timer(components)
        Canvas = New Queens_ImageControl.ImageControl()
        ToolStrip.SuspendLayout()
        SuspendLayout()
        ' 
        ' ToolStrip
        ' 
        ToolStrip.Font = New Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point)
        ToolStrip.ImageScalingSize = New Size(20, 20)
        ToolStrip.Items.AddRange(New ToolStripItem() {Cmd_Copy, ToolStripSeparator1, Cmd_Zoom100, Cmd_ZoomIn, Cmd_ZoomOut, ToolStripDropDownButton1, ToolStripSeparator3, Cmd_QuickAnim, ToolStripLabel1, Combo_AnimInterval, Cmd_ShowRegions, ToolStripSeparator2, Cmd_ShowLabels})
        ToolStrip.Location = New Point(0, 0)
        ToolStrip.Name = "ToolStrip"
        ToolStrip.Size = New Size(1083, 28)
        ToolStrip.TabIndex = 19
        ToolStrip.Text = "ToolStrip1"
        ' 
        ' Cmd_Copy
        ' 
        Cmd_Copy.Image = CType(resources.GetObject("Cmd_Copy.Image"), Image)
        Cmd_Copy.ImageTransparentColor = Color.Magenta
        Cmd_Copy.Name = "Cmd_Copy"
        Cmd_Copy.Size = New Size(29, 25)
        Cmd_Copy.ToolTipText = "Copy"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(6, 28)
        ' 
        ' Cmd_Zoom100
        ' 
        Cmd_Zoom100.Image = CType(resources.GetObject("Cmd_Zoom100.Image"), Image)
        Cmd_Zoom100.ImageTransparentColor = Color.Black
        Cmd_Zoom100.Name = "Cmd_Zoom100"
        Cmd_Zoom100.Size = New Size(29, 25)
        Cmd_Zoom100.ToolTipText = "Zoom 100%"
        ' 
        ' Cmd_ZoomIn
        ' 
        Cmd_ZoomIn.Image = CType(resources.GetObject("Cmd_ZoomIn.Image"), Image)
        Cmd_ZoomIn.ImageTransparentColor = Color.Black
        Cmd_ZoomIn.Name = "Cmd_ZoomIn"
        Cmd_ZoomIn.Size = New Size(29, 25)
        Cmd_ZoomIn.ToolTipText = "Zoom in"
        ' 
        ' Cmd_ZoomOut
        ' 
        Cmd_ZoomOut.Image = CType(resources.GetObject("Cmd_ZoomOut.Image"), Image)
        Cmd_ZoomOut.ImageTransparentColor = Color.Black
        Cmd_ZoomOut.Name = "Cmd_ZoomOut"
        Cmd_ZoomOut.Size = New Size(29, 25)
        Cmd_ZoomOut.ToolTipText = "Zoom out"
        ' 
        ' ToolStripDropDownButton1
        ' 
        ToolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image
        ToolStripDropDownButton1.DropDownItems.AddRange(New ToolStripItem() {Menu_ResetZoom, Menu_Stretchtofit, Menu_Fittoscreen})
        ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), Image)
        ToolStripDropDownButton1.ImageTransparentColor = Color.Black
        ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        ToolStripDropDownButton1.Size = New Size(34, 25)
        ToolStripDropDownButton1.ToolTipText = "Zoom options"
        ' 
        ' Menu_ResetZoom
        ' 
        Menu_ResetZoom.Name = "Menu_ResetZoom"
        Menu_ResetZoom.Size = New Size(180, 26)
        Menu_ResetZoom.Text = "Reset"
        ' 
        ' Menu_Stretchtofit
        ' 
        Menu_Stretchtofit.Name = "Menu_Stretchtofit"
        Menu_Stretchtofit.Size = New Size(180, 26)
        Menu_Stretchtofit.Text = "Stretch to fit"
        ' 
        ' Menu_Fittoscreen
        ' 
        Menu_Fittoscreen.Name = "Menu_Fittoscreen"
        Menu_Fittoscreen.Size = New Size(180, 26)
        Menu_Fittoscreen.Text = "Fit to screen"
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(6, 28)
        ' 
        ' Cmd_QuickAnim
        ' 
        Cmd_QuickAnim.Image = CType(resources.GetObject("Cmd_QuickAnim.Image"), Image)
        Cmd_QuickAnim.ImageTransparentColor = Color.Magenta
        Cmd_QuickAnim.Name = "Cmd_QuickAnim"
        Cmd_QuickAnim.Size = New Size(136, 28)
        Cmd_QuickAnim.Text = "No animation"
        ' 
        ' ToolStripLabel1
        ' 
        ToolStripLabel1.Name = "ToolStripLabel1"
        ToolStripLabel1.Size = New Size(61, 25)
        ToolStripLabel1.Text = "Interval"
        ' 
        ' Combo_AnimInterval
        ' 
        Combo_AnimInterval.AutoSize = False
        Combo_AnimInterval.DropDownStyle = ComboBoxStyle.DropDownList
        Combo_AnimInterval.DropDownWidth = 55
        Combo_AnimInterval.Items.AddRange(New Object() {"100", "200", "300", "400", "500", "600", "700", "800", "1000", "1500", "2000"})
        Combo_AnimInterval.Name = "Combo_AnimInterval"
        Combo_AnimInterval.Size = New Size(84, 28)
        ' 
        ' Cmd_ShowRegions
        ' 
        Cmd_ShowRegions.DisplayStyle = ToolStripItemDisplayStyle.Text
        Cmd_ShowRegions.Image = CType(resources.GetObject("Cmd_ShowRegions.Image"), Image)
        Cmd_ShowRegions.ImageTransparentColor = Color.Magenta
        Cmd_ShowRegions.Name = "Cmd_ShowRegions"
        Cmd_ShowRegions.Size = New Size(109, 25)
        Cmd_ShowRegions.Text = "Show regions"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(6, 28)
        ' 
        ' Cmd_ShowLabels
        ' 
        Cmd_ShowLabels.DisplayStyle = ToolStripItemDisplayStyle.Text
        Cmd_ShowLabels.Image = CType(resources.GetObject("Cmd_ShowLabels.Image"), Image)
        Cmd_ShowLabels.ImageTransparentColor = Color.Magenta
        Cmd_ShowLabels.Name = "Cmd_ShowLabels"
        Cmd_ShowLabels.Size = New Size(98, 25)
        Cmd_ShowLabels.Text = "Show labels"
        ' 
        ' Timer_Animation
        ' 
        ' 
        ' Canvas
        ' 
        Canvas.Dock = DockStyle.Fill
        Canvas.Image = Nothing
        Canvas.initialimage = Nothing
        Canvas.Location = New Point(0, 28)
        Canvas.Margin = New Padding(3, 4, 3, 4)
        Canvas.Name = "Canvas"
        Canvas.Origin = New Point(0, 0)
        Canvas.PanButton = MouseButtons.Left
        Canvas.PanMode = True
        Canvas.ScrollbarsVisible = True
        Canvas.Size = New Size(1083, 599)
        Canvas.StretchImageToFit = False
        Canvas.TabIndex = 20
        Canvas.ZoomFactor = 1R
        Canvas.ZoomOnMouseWheel = True
        ' 
        ' Frm_ImageViewer
        ' 
        AutoScaleDimensions = New SizeF(9F, 18F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1083, 627)
        Controls.Add(Canvas)
        Controls.Add(ToolStrip)
        Font = New Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(3, 4, 3, 4)
        Name = "Frm_ImageViewer"
        StartPosition = FormStartPosition.Manual
        Text = "Image Viewer"
        ToolStrip.ResumeLayout(False)
        ToolStrip.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents ToolStrip As ToolStrip
    Friend WithEvents Cmd_Copy As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents Cmd_Zoom100 As Windows.Forms.ToolStripButton
    Friend WithEvents Cmd_ZoomIn As Windows.Forms.ToolStripButton
    Friend WithEvents Cmd_ZoomOut As Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripDropDownButton1 As Windows.Forms.ToolStripDropDownButton
    Friend WithEvents Menu_ResetZoom As Windows.Forms.ToolStripMenuItem
    Friend WithEvents Menu_Stretchtofit As Windows.Forms.ToolStripMenuItem
    Friend WithEvents Menu_Fittoscreen As Windows.Forms.ToolStripMenuItem
    Friend WithEvents Cmd_QuickAnim As ToolStripMenuItem
    Friend WithEvents Combo_AnimInterval As ToolStripComboBox
    Friend WithEvents Timer_Animation As Timer
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents Canvas As Queens_ImageControl.ImageControl
    Friend WithEvents Cmd_ShowRegions As ToolStripButton
    Friend WithEvents Cmd_ShowLabels As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
End Class
