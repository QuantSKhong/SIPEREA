<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDI_Main
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDI_Main))
        ToolStrip1 = New ToolStrip()
        Cmd_ImageProcessing = New ToolStripButton()
        ToolStripSeparator2 = New ToolStripSeparator()
        Cmd_BatchProcessing = New ToolStripButton()
        ToolStripSeparator1 = New ToolStripSeparator()
        MenuStrip1 = New MenuStrip()
        Menu_File = New ToolStripMenuItem()
        Menu_Open = New ToolStripMenuItem()
        toolStripSeparator = New ToolStripSeparator()
        Menu_Exit = New ToolStripMenuItem()
        Menu_Edit = New ToolStripMenuItem()
        Menu_Copy = New ToolStripMenuItem()
        Menu_Window = New ToolStripMenuItem()
        Menu_ImageProcessing = New ToolStripMenuItem()
        Menu_BatchProcessing = New ToolStripMenuItem()
        Menu_About = New ToolStripMenuItem()
        StatusStrip_Status = New StatusStrip()
        Status_Info = New ToolStripStatusLabel()
        Status_Progressbar = New ToolStripProgressBar()
        Status_Info2 = New ToolStripStatusLabel()
        Status_Info3 = New ToolStripStatusLabel()
        ToolStrip1.SuspendLayout()
        MenuStrip1.SuspendLayout()
        StatusStrip_Status.SuspendLayout()
        SuspendLayout()
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.ImageScalingSize = New Size(20, 20)
        ToolStrip1.Items.AddRange(New ToolStripItem() {Cmd_ImageProcessing, ToolStripSeparator2, Cmd_BatchProcessing, ToolStripSeparator1})
        ToolStrip1.Location = New Point(0, 28)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Size = New Size(1095, 27)
        ToolStrip1.TabIndex = 1
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' Cmd_ImageProcessing
        ' 
        Cmd_ImageProcessing.DisplayStyle = ToolStripItemDisplayStyle.Text
        Cmd_ImageProcessing.Image = CType(resources.GetObject("Cmd_ImageProcessing.Image"), Image)
        Cmd_ImageProcessing.ImageTransparentColor = Color.Magenta
        Cmd_ImageProcessing.Name = "Cmd_ImageProcessing"
        Cmd_ImageProcessing.Size = New Size(130, 24)
        Cmd_ImageProcessing.Text = "Image processing"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(6, 27)
        ' 
        ' Cmd_BatchProcessing
        ' 
        Cmd_BatchProcessing.DisplayStyle = ToolStripItemDisplayStyle.Text
        Cmd_BatchProcessing.Image = CType(resources.GetObject("Cmd_BatchProcessing.Image"), Image)
        Cmd_BatchProcessing.ImageTransparentColor = Color.Magenta
        Cmd_BatchProcessing.Name = "Cmd_BatchProcessing"
        Cmd_BatchProcessing.Size = New Size(125, 24)
        Cmd_BatchProcessing.Text = "Batch processing"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(6, 27)
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.ImageScalingSize = New Size(20, 20)
        MenuStrip1.Items.AddRange(New ToolStripItem() {Menu_File, Menu_Edit, Menu_Window, Menu_About})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1095, 28)
        MenuStrip1.TabIndex = 2
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' Menu_File
        ' 
        Menu_File.DropDownItems.AddRange(New ToolStripItem() {Menu_Open, toolStripSeparator, Menu_Exit})
        Menu_File.Name = "Menu_File"
        Menu_File.Size = New Size(46, 24)
        Menu_File.Text = "&File"
        ' 
        ' Menu_Open
        ' 
        Menu_Open.Image = CType(resources.GetObject("Menu_Open.Image"), Image)
        Menu_Open.ImageTransparentColor = Color.Magenta
        Menu_Open.Name = "Menu_Open"
        Menu_Open.ShortcutKeys = Keys.Control Or Keys.O
        Menu_Open.Size = New Size(181, 26)
        Menu_Open.Text = "&Open"
        ' 
        ' toolStripSeparator
        ' 
        toolStripSeparator.Name = "toolStripSeparator"
        toolStripSeparator.Size = New Size(178, 6)
        ' 
        ' Menu_Exit
        ' 
        Menu_Exit.Name = "Menu_Exit"
        Menu_Exit.Size = New Size(181, 26)
        Menu_Exit.Text = "E&xit"
        ' 
        ' Menu_Edit
        ' 
        Menu_Edit.DropDownItems.AddRange(New ToolStripItem() {Menu_Copy})
        Menu_Edit.Name = "Menu_Edit"
        Menu_Edit.Size = New Size(49, 24)
        Menu_Edit.Text = "&Edit"
        ' 
        ' Menu_Copy
        ' 
        Menu_Copy.Image = CType(resources.GetObject("Menu_Copy.Image"), Image)
        Menu_Copy.ImageTransparentColor = Color.Magenta
        Menu_Copy.Name = "Menu_Copy"
        Menu_Copy.ShortcutKeys = Keys.Control Or Keys.C
        Menu_Copy.Size = New Size(177, 26)
        Menu_Copy.Text = "&Copy"
        ' 
        ' Menu_Window
        ' 
        Menu_Window.DropDownItems.AddRange(New ToolStripItem() {Menu_ImageProcessing, Menu_BatchProcessing})
        Menu_Window.Name = "Menu_Window"
        Menu_Window.Size = New Size(78, 24)
        Menu_Window.Text = "&Window"
        ' 
        ' Menu_ImageProcessing
        ' 
        Menu_ImageProcessing.Name = "Menu_ImageProcessing"
        Menu_ImageProcessing.Size = New Size(208, 26)
        Menu_ImageProcessing.Text = "Image Processing"
        ' 
        ' Menu_BatchProcessing
        ' 
        Menu_BatchProcessing.Name = "Menu_BatchProcessing"
        Menu_BatchProcessing.Size = New Size(208, 26)
        Menu_BatchProcessing.Text = "&Batch processing"
        ' 
        ' Menu_About
        ' 
        Menu_About.Name = "Menu_About"
        Menu_About.Size = New Size(64, 24)
        Menu_About.Text = "About"
        ' 
        ' StatusStrip_Status
        ' 
        StatusStrip_Status.AutoSize = False
        StatusStrip_Status.ImageScalingSize = New Size(20, 20)
        StatusStrip_Status.Items.AddRange(New ToolStripItem() {Status_Info, Status_Progressbar, Status_Info2, Status_Info3})
        StatusStrip_Status.Location = New Point(0, 452)
        StatusStrip_Status.Name = "StatusStrip_Status"
        StatusStrip_Status.Size = New Size(1095, 31)
        StatusStrip_Status.TabIndex = 8
        ' 
        ' Status_Info
        ' 
        Status_Info.AutoSize = False
        Status_Info.Name = "Status_Info"
        Status_Info.Size = New Size(500, 25)
        Status_Info.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Status_Progressbar
        ' 
        Status_Progressbar.AutoSize = False
        Status_Progressbar.Name = "Status_Progressbar"
        Status_Progressbar.Size = New Size(300, 23)
        ' 
        ' Status_Info2
        ' 
        Status_Info2.Name = "Status_Info2"
        Status_Info2.Size = New Size(0, 25)
        Status_Info2.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Status_Info3
        ' 
        Status_Info3.Name = "Status_Info3"
        Status_Info3.Size = New Size(0, 25)
        Status_Info3.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' MDI_Main
        ' 
        AutoScaleDimensions = New SizeF(9F, 18F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1095, 483)
        Controls.Add(StatusStrip_Status)
        Controls.Add(ToolStrip1)
        Controls.Add(MenuStrip1)
        Font = New Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        IsMdiContainer = True
        Name = "MDI_Main"
        Text = "Image anlaysis program of SIPEREA 1.0"
        WindowState = FormWindowState.Maximized
        ToolStrip1.ResumeLayout(False)
        ToolStrip1.PerformLayout()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        StatusStrip_Status.ResumeLayout(False)
        StatusStrip_Status.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents Menu_File As ToolStripMenuItem
    Friend WithEvents Menu_Open As ToolStripMenuItem
    Friend WithEvents toolStripSeparator As ToolStripSeparator
    Friend WithEvents Menu_Exit As ToolStripMenuItem
    Friend WithEvents Menu_Edit As ToolStripMenuItem
    Friend WithEvents Menu_Copy As ToolStripMenuItem
    Friend WithEvents Menu_Window As ToolStripMenuItem
    Friend WithEvents Menu_Batch As ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Menu_BatchProcessing As ToolStripMenuItem
    Friend WithEvents StatusStrip_Status As StatusStrip
    Friend WithEvents Status_Info As ToolStripStatusLabel
    Friend WithEvents Status_Progressbar As ToolStripProgressBar
    Friend WithEvents Status_Info2 As ToolStripStatusLabel
    Friend WithEvents Status_Info3 As ToolStripStatusLabel
    Friend WithEvents Cmd_BatchProcessing As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents Cmd_ImageProcessing As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents Menu_ImageProcessing As ToolStripMenuItem
    Friend WithEvents Menu_About As ToolStripMenuItem
End Class
