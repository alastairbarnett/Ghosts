<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPlayGhosts
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
        Me.lblLost = New System.Windows.Forms.Label()
        Me.lblTaken = New System.Windows.Forms.Label()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.btnRestart = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.lblInstruction = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblLost
        '
        Me.lblLost.AutoSize = True
        Me.lblLost.Location = New System.Drawing.Point(6, 94)
        Me.lblLost.Name = "lblLost"
        Me.lblLost.Size = New System.Drawing.Size(27, 13)
        Me.lblLost.TabIndex = 0
        Me.lblLost.Text = "Lost"
        '
        'lblTaken
        '
        Me.lblTaken.AutoSize = True
        Me.lblTaken.Location = New System.Drawing.Point(6, 24)
        Me.lblTaken.Name = "lblTaken"
        Me.lblTaken.Size = New System.Drawing.Size(38, 13)
        Me.lblTaken.TabIndex = 1
        Me.lblTaken.Text = "Taken"
        '
        'btnHelp
        '
        Me.btnHelp.Location = New System.Drawing.Point(13, 558)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 23)
        Me.btnHelp.TabIndex = 2
        Me.btnHelp.Text = "Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'btnRestart
        '
        Me.btnRestart.Location = New System.Drawing.Point(209, 558)
        Me.btnRestart.Name = "btnRestart"
        Me.btnRestart.Size = New System.Drawing.Size(75, 23)
        Me.btnRestart.TabIndex = 3
        Me.btnRestart.Text = "Restart"
        Me.btnRestart.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(397, 558)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 4
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'lblInstruction
        '
        Me.lblInstruction.AutoSize = True
        Me.lblInstruction.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(199, 526)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.Size = New System.Drawing.Size(105, 24)
        Me.lblInstruction.TabIndex = 5
        Me.lblInstruction.Text = "[Instruction]"
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmPlayGhosts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 592)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnRestart)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.lblTaken)
        Me.Controls.Add(Me.lblLost)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmPlayGhosts"
        Me.Text = "Play Ghosts"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblLost As System.Windows.Forms.Label
    Friend WithEvents lblTaken As System.Windows.Forms.Label
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents btnRestart As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblInstruction As System.Windows.Forms.Label
End Class
