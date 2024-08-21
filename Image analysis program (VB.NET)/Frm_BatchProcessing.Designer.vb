<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_BatchProcessing
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_BatchProcessing))
        TextBox_Msg = New TextBox()
        Cmd_RunBatchProcessing = New Button()
        Cmd_Clear = New Button()
        FileDDBox = New FileDragDropBox()
        Cmd_SelectFolder = New Button()
        FolderDialog = New FolderBrowserDialog()
        Label1 = New Label()
        Text_Interval = New TextBox()
        SuspendLayout()
        ' 
        ' TextBox_Msg
        ' 
        TextBox_Msg.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox_Msg.Location = New Point(23, 334)
        TextBox_Msg.Name = "TextBox_Msg"
        TextBox_Msg.ReadOnly = True
        TextBox_Msg.Size = New Size(695, 24)
        TextBox_Msg.TabIndex = 19
        ' 
        ' Cmd_RunBatchProcessing
        ' 
        Cmd_RunBatchProcessing.Location = New Point(382, 9)
        Cmd_RunBatchProcessing.Name = "Cmd_RunBatchProcessing"
        Cmd_RunBatchProcessing.Size = New Size(199, 32)
        Cmd_RunBatchProcessing.TabIndex = 18
        Cmd_RunBatchProcessing.Text = "Run batch processing"
        Cmd_RunBatchProcessing.UseVisualStyleBackColor = True
        ' 
        ' Cmd_Clear
        ' 
        Cmd_Clear.Location = New Point(23, 9)
        Cmd_Clear.Name = "Cmd_Clear"
        Cmd_Clear.Size = New Size(118, 32)
        Cmd_Clear.TabIndex = 16
        Cmd_Clear.Text = "Clear"
        Cmd_Clear.UseVisualStyleBackColor = True
        ' 
        ' FileDDBox
        ' 
        FileDDBox.AllowDrop = True
        FileDDBox.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        FileDDBox.Location = New Point(23, 52)
        FileDDBox.Margin = New Padding(3, 2, 3, 2)
        FileDDBox.Name = "FileDDBox"
        FileDDBox.Size = New Size(696, 277)
        FileDDBox.TabIndex = 15
        ' 
        ' Cmd_SelectFolder
        ' 
        Cmd_SelectFolder.Location = New Point(159, 9)
        Cmd_SelectFolder.Name = "Cmd_SelectFolder"
        Cmd_SelectFolder.Size = New Size(147, 32)
        Cmd_SelectFolder.TabIndex = 20
        Cmd_SelectFolder.Text = "Select folder"
        Cmd_SelectFolder.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(587, 19)
        Label1.Name = "Label1"
        Label1.Size = New Size(54, 18)
        Label1.TabIndex = 21
        Label1.Text = "Interval"
        ' 
        ' Text_Interval
        ' 
        Text_Interval.Location = New Point(662, 13)
        Text_Interval.Name = "Text_Interval"
        Text_Interval.Size = New Size(56, 24)
        Text_Interval.TabIndex = 22
        Text_Interval.Text = "1"
        ' 
        ' Frm_BatchProcessing
        ' 
        AutoScaleDimensions = New SizeF(9F, 18F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(737, 371)
        Controls.Add(Text_Interval)
        Controls.Add(Label1)
        Controls.Add(Cmd_SelectFolder)
        Controls.Add(TextBox_Msg)
        Controls.Add(Cmd_RunBatchProcessing)
        Controls.Add(Cmd_Clear)
        Controls.Add(FileDDBox)
        Font = New Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "Frm_BatchProcessing"
        Text = "Batch Processing"
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents TextBox_Msg As TextBox
    Friend WithEvents Cmd_RunBatchProcessing As Button
    Friend WithEvents Cmd_Clear As Button
    Friend WithEvents FileDDBox As FileDragDropBox
    Friend WithEvents Cmd_SelectFolder As Button
    Friend WithEvents FolderDialog As FolderBrowserDialog
    Friend WithEvents Label1 As Label
    Friend WithEvents Text_Interval As TextBox
End Class
