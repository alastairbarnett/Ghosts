<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectReplay
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
        Me.grdPastGamesTable = New System.Windows.Forms.DataGridView()
        Me.btnSelect = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        CType(Me.grdPastGamesTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdPastGamesTable
        '
        Me.grdPastGamesTable.AllowUserToAddRows = False
        Me.grdPastGamesTable.AllowUserToDeleteRows = False
        Me.grdPastGamesTable.AllowUserToResizeColumns = False
        Me.grdPastGamesTable.AllowUserToResizeRows = False
        Me.grdPastGamesTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.grdPastGamesTable.BackgroundColor = System.Drawing.SystemColors.Control
        Me.grdPastGamesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdPastGamesTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.grdPastGamesTable.Location = New System.Drawing.Point(13, 12)
        Me.grdPastGamesTable.MultiSelect = False
        Me.grdPastGamesTable.Name = "grdPastGamesTable"
        Me.grdPastGamesTable.ReadOnly = True
        Me.grdPastGamesTable.RowHeadersVisible = False
        Me.grdPastGamesTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.grdPastGamesTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdPastGamesTable.ShowEditingIcon = False
        Me.grdPastGamesTable.Size = New System.Drawing.Size(439, 329)
        Me.grdPastGamesTable.TabIndex = 2
        '
        'btnSelect
        '
        Me.btnSelect.Location = New System.Drawing.Point(377, 346)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(75, 23)
        Me.btnSelect.TabIndex = 3
        Me.btnSelect.Text = "Select"
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(13, 347)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(75, 23)
        Me.btnBack.TabIndex = 4
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'frmSelectReplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(464, 381)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.grdPastGamesTable)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmSelectReplay"
        Me.Text = "Select Replay"
        CType(Me.grdPastGamesTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdPastGamesTable As System.Windows.Forms.DataGridView
    Friend WithEvents btnSelect As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
End Class
