<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileDragDropBox
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        ListView = New ListView()
        Column_FileName = New ColumnHeader()
        Column_Size = New ColumnHeader()
        Column_Folder = New ColumnHeader()
        SuspendLayout()
        ' 
        ' ListView
        ' 
        ListView.AllowDrop = True
        ListView.BackColor = SystemColors.ControlLightLight
        ListView.BorderStyle = BorderStyle.FixedSingle
        ListView.Columns.AddRange(New ColumnHeader() {Column_FileName, Column_Size, Column_Folder})
        ListView.Font = New Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point)
        ListView.FullRowSelect = True
        ListView.GridLines = True
        ListView.Location = New Point(3, 44)
        ListView.Margin = New Padding(3, 4, 3, 4)
        ListView.Name = "ListView"
        ListView.Size = New Size(511, 224)
        ListView.Sorting = SortOrder.Ascending
        ListView.TabIndex = 2
        ListView.UseCompatibleStateImageBehavior = False
        ListView.View = View.Details
        ' 
        ' Column_FileName
        ' 
        Column_FileName.Text = "File name"
        Column_FileName.Width = 300
        ' 
        ' Column_Size
        ' 
        Column_Size.Text = "Size (kb)"
        Column_Size.TextAlign = HorizontalAlignment.Right
        Column_Size.Width = 100
        ' 
        ' Column_Folder
        ' 
        Column_Folder.Text = "Folder"
        Column_Folder.Width = 500
        ' 
        ' FileDragDropBox
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(ListView)
        Margin = New Padding(3, 4, 3, 4)
        Name = "FileDragDropBox"
        Size = New Size(543, 349)
        ResumeLayout(False)

    End Sub
    Friend WithEvents ListView As System.Windows.Forms.ListView
    Friend WithEvents Column_FileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents Column_Size As System.Windows.Forms.ColumnHeader
    Friend WithEvents Column_Folder As System.Windows.Forms.ColumnHeader

End Class
