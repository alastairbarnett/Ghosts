<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHighScores
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grdHighscoresTable = New System.Windows.Forms.DataGridView()
        CType(Me.grdHighscoresTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(99, 405)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'grdHighscoresTable
        '
        Me.grdHighscoresTable.AllowUserToAddRows = False
        Me.grdHighscoresTable.AllowUserToDeleteRows = False
        Me.grdHighscoresTable.AllowUserToResizeColumns = False
        Me.grdHighscoresTable.AllowUserToResizeRows = False
        Me.grdHighscoresTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.grdHighscoresTable.BackgroundColor = System.Drawing.SystemColors.Control
        Me.grdHighscoresTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdHighscoresTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.grdHighscoresTable.Location = New System.Drawing.Point(13, 13)
        Me.grdHighscoresTable.MultiSelect = False
        Me.grdHighscoresTable.Name = "grdHighscoresTable"
        Me.grdHighscoresTable.ReadOnly = True
        Me.grdHighscoresTable.RowHeadersVisible = False
        Me.grdHighscoresTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.grdHighscoresTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdHighscoresTable.ShowEditingIcon = False
        Me.grdHighscoresTable.Size = New System.Drawing.Size(259, 383)
        Me.grdHighscoresTable.TabIndex = 1
        '
        'frmHighScores
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 437)
        Me.Controls.Add(Me.grdHighscoresTable)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmHighScores"
        Me.Text = "High Scores"
        CType(Me.grdHighscoresTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents grdHighscoresTable As System.Windows.Forms.DataGridView
End Class
