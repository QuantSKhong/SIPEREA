<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm_ImageAnalysis
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Public components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(Frm_ImageAnalysis))
        Cmd_SelectFile = New Button()
        Label10 = New Label()
        Text_SourceImage = New TextBox()
        Label1 = New Label()
        GroupBox5 = New GroupBox()
        Label19 = New Label()
        Text_Resize_Height = New TextBox()
        Text_Resize_Width = New TextBox()
        Label7 = New Label()
        Check_Resize = New CheckBox()
        GroupBox2 = New GroupBox()
        Check_AnimalInBlack = New CheckBox()
        Cmd_Test_AdaptiveThresholding = New Button()
        Text_Adaptive_Threshold = New TextBox()
        Label8 = New Label()
        Text_Adaptive_BoxSize = New TextBox()
        Label9 = New Label()
        OpenFileDialog = New OpenFileDialog()
        FolderDialog = New FolderBrowserDialog()
        Cmd_ReadImage = New Button()
        Cmd_RunAnalysis = New Button()
        GroupBox5.SuspendLayout()
        GroupBox2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Cmd_SelectFile
        ' 
        Cmd_SelectFile.Location = New Point(397, 12)
        Cmd_SelectFile.Name = "Cmd_SelectFile"
        Cmd_SelectFile.Size = New Size(145, 32)
        Cmd_SelectFile.TabIndex = 3
        Cmd_SelectFile.Text = "Select file"
        Cmd_SelectFile.UseVisualStyleBackColor = True
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.ForeColor = Color.FromArgb(CByte(0), CByte(0), CByte(192))
        Label10.Location = New Point(542, 106)
        Label10.Name = "Label10"
        Label10.Size = New Size(160, 18)
        Label10.TabIndex = 2
        Label10.Text = "Drag and drop imag file"
        ' 
        ' Text_SourceImage
        ' 
        Text_SourceImage.AllowDrop = True
        Text_SourceImage.Location = New Point(16, 50)
        Text_SourceImage.Multiline = True
        Text_SourceImage.Name = "Text_SourceImage"
        Text_SourceImage.Size = New Size(701, 54)
        Text_SourceImage.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(16, 19)
        Label1.Name = "Label1"
        Label1.Size = New Size(122, 18)
        Label1.TabIndex = 0
        Label1.Text = "Source image file"
        ' 
        ' GroupBox5
        ' 
        GroupBox5.Controls.Add(Label19)
        GroupBox5.Controls.Add(Text_Resize_Height)
        GroupBox5.Controls.Add(Text_Resize_Width)
        GroupBox5.Controls.Add(Label7)
        GroupBox5.Controls.Add(Check_Resize)
        GroupBox5.Location = New Point(16, 137)
        GroupBox5.Name = "GroupBox5"
        GroupBox5.Size = New Size(701, 62)
        GroupBox5.TabIndex = 12
        GroupBox5.TabStop = False
        GroupBox5.Text = "Resize image"
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(381, 30)
        Label19.Name = "Label19"
        Label19.Size = New Size(50, 18)
        Label19.TabIndex = 12
        Label19.Text = "Height"
        ' 
        ' Text_Resize_Height
        ' 
        Text_Resize_Height.Location = New Point(473, 25)
        Text_Resize_Height.Name = "Text_Resize_Height"
        Text_Resize_Height.Size = New Size(67, 24)
        Text_Resize_Height.TabIndex = 3
        Text_Resize_Height.Text = "768"
        ' 
        ' Text_Resize_Width
        ' 
        Text_Resize_Width.Location = New Point(258, 25)
        Text_Resize_Width.Name = "Text_Resize_Width"
        Text_Resize_Width.Size = New Size(67, 24)
        Text_Resize_Width.TabIndex = 2
        Text_Resize_Width.Text = "1024"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(177, 30)
        Label7.Name = "Label7"
        Label7.Size = New Size(46, 18)
        Label7.TabIndex = 1
        Label7.Text = "Width"
        ' 
        ' Check_Resize
        ' 
        Check_Resize.AutoSize = True
        Check_Resize.Checked = True
        Check_Resize.CheckState = CheckState.Checked
        Check_Resize.Location = New Point(27, 29)
        Check_Resize.Name = "Check_Resize"
        Check_Resize.Size = New Size(76, 22)
        Check_Resize.TabIndex = 14
        Check_Resize.Text = "Resize"
        Check_Resize.UseVisualStyleBackColor = True
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(Check_AnimalInBlack)
        GroupBox2.Controls.Add(Cmd_Test_AdaptiveThresholding)
        GroupBox2.Controls.Add(Text_Adaptive_Threshold)
        GroupBox2.Controls.Add(Label8)
        GroupBox2.Controls.Add(Text_Adaptive_BoxSize)
        GroupBox2.Controls.Add(Label9)
        GroupBox2.Location = New Point(16, 215)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(702, 102)
        GroupBox2.TabIndex = 10
        GroupBox2.TabStop = False
        GroupBox2.Text = "Adaptive thresholding binarization"
        ' 
        ' Check_AnimalInBlack
        ' 
        Check_AnimalInBlack.AutoSize = True
        Check_AnimalInBlack.Checked = True
        Check_AnimalInBlack.CheckState = CheckState.Checked
        Check_AnimalInBlack.Location = New Point(29, 72)
        Check_AnimalInBlack.Name = "Check_AnimalInBlack"
        Check_AnimalInBlack.Size = New Size(217, 22)
        Check_AnimalInBlack.TabIndex = 9
        Check_AnimalInBlack.Text = "Animals shaded in the image"
        Check_AnimalInBlack.UseVisualStyleBackColor = True
        ' 
        ' Cmd_Test_AdaptiveThresholding
        ' 
        Cmd_Test_AdaptiveThresholding.Location = New Point(526, 23)
        Cmd_Test_AdaptiveThresholding.Name = "Cmd_Test_AdaptiveThresholding"
        Cmd_Test_AdaptiveThresholding.Size = New Size(154, 56)
        Cmd_Test_AdaptiveThresholding.TabIndex = 8
        Cmd_Test_AdaptiveThresholding.Text = "Create binarized image"
        Cmd_Test_AdaptiveThresholding.UseVisualStyleBackColor = True
        ' 
        ' Text_Adaptive_Threshold
        ' 
        Text_Adaptive_Threshold.Location = New Point(414, 33)
        Text_Adaptive_Threshold.Name = "Text_Adaptive_Threshold"
        Text_Adaptive_Threshold.Size = New Size(69, 24)
        Text_Adaptive_Threshold.TabIndex = 7
        Text_Adaptive_Threshold.Text = "75"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(293, 33)
        Label8.Name = "Label8"
        Label8.Size = New Size(74, 18)
        Label8.TabIndex = 6
        Label8.Text = "Threshold"
        ' 
        ' Text_Adaptive_BoxSize
        ' 
        Text_Adaptive_BoxSize.Location = New Point(160, 33)
        Text_Adaptive_BoxSize.Name = "Text_Adaptive_BoxSize"
        Text_Adaptive_BoxSize.Size = New Size(67, 24)
        Text_Adaptive_BoxSize.TabIndex = 5
        Text_Adaptive_BoxSize.Text = "40"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(29, 33)
        Label9.Name = "Label9"
        Label9.Size = New Size(65, 18)
        Label9.TabIndex = 4
        Label9.Text = "Box size"
        ' 
        ' Cmd_ReadImage
        ' 
        Cmd_ReadImage.Location = New Point(572, 11)
        Cmd_ReadImage.Name = "Cmd_ReadImage"
        Cmd_ReadImage.Size = New Size(145, 32)
        Cmd_ReadImage.TabIndex = 13
        Cmd_ReadImage.Text = "Read image"
        Cmd_ReadImage.UseVisualStyleBackColor = True
        ' 
        ' Cmd_RunAnalysis
        ' 
        Cmd_RunAnalysis.Location = New Point(274, 326)
        Cmd_RunAnalysis.Name = "Cmd_RunAnalysis"
        Cmd_RunAnalysis.Size = New Size(154, 50)
        Cmd_RunAnalysis.TabIndex = 8
        Cmd_RunAnalysis.Text = "Run analysis"
        Cmd_RunAnalysis.UseVisualStyleBackColor = True
        ' 
        ' Frm_ImageAnalysis
        ' 
        AutoScaleDimensions = New SizeF(9F, 18F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(737, 386)
        Controls.Add(Cmd_ReadImage)
        Controls.Add(GroupBox5)
        Controls.Add(Cmd_SelectFile)
        Controls.Add(Cmd_RunAnalysis)
        Controls.Add(Label10)
        Controls.Add(GroupBox2)
        Controls.Add(Text_SourceImage)
        Controls.Add(Label1)
        Font = New Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "Frm_ImageAnalysis"
        StartPosition = FormStartPosition.Manual
        Text = "Image Analysis"
        GroupBox5.ResumeLayout(False)
        GroupBox5.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents Text_SourceImage As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Text_Adaptive_Threshold As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Text_Adaptive_BoxSize As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Cmd_Test_AdaptiveThresholding As Button
    Friend WithEvents Cmd_SelectFile As Button
    Friend WithEvents Label10 As Label
    Friend WithEvents OpenFileDialog As OpenFileDialog
    Friend WithEvents Check_AnimalInBlack As CheckBox
    Friend WithEvents FolderDialog As FolderBrowserDialog
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents Label19 As Label
    Friend WithEvents Text_Resize_Height As TextBox
    Friend WithEvents Text_Resize_Width As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Cmd_ReadImage As Button
    Friend WithEvents Check_Resize As CheckBox
    Friend WithEvents Cmd_RunAnalysis As Button
End Class
