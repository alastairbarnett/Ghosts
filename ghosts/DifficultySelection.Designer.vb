<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDifficultySelection
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
        Me.rdoEasy = New System.Windows.Forms.RadioButton()
        Me.rdoNormal = New System.Windows.Forms.RadioButton()
        Me.rdoImpossible = New System.Windows.Forms.RadioButton()
        Me.btnPlay = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblInstruction = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'rdoEasy
        '
        Me.rdoEasy.AutoSize = True
        Me.rdoEasy.Location = New System.Drawing.Point(13, 37)
        Me.rdoEasy.Name = "rdoEasy"
        Me.rdoEasy.Size = New System.Drawing.Size(173, 17)
        Me.rdoEasy.TabIndex = 0
        Me.rdoEasy.Text = "Easy - Winning should be easy."
        Me.rdoEasy.UseVisualStyleBackColor = True
        '
        'rdoNormal
        '
        Me.rdoNormal.AutoSize = True
        Me.rdoNormal.Checked = True
        Me.rdoNormal.Location = New System.Drawing.Point(13, 61)
        Me.rdoNormal.Name = "rdoNormal"
        Me.rdoNormal.Size = New System.Drawing.Size(241, 17)
        Me.rdoNormal.TabIndex = 1
        Me.rdoNormal.TabStop = True
        Me.rdoNormal.Text = "Normal - Winning should be more challenging."
        Me.rdoNormal.UseVisualStyleBackColor = True
        '
        'rdoImpossible
        '
        Me.rdoImpossible.AutoSize = True
        Me.rdoImpossible.Location = New System.Drawing.Point(13, 85)
        Me.rdoImpossible.Name = "rdoImpossible"
        Me.rdoImpossible.Size = New System.Drawing.Size(210, 17)
        Me.rdoImpossible.TabIndex = 2
        Me.rdoImpossible.Text = "Impossible - Winning should be difficult."
        Me.rdoImpossible.UseVisualStyleBackColor = True
        '
        'btnPlay
        '
        Me.btnPlay.Location = New System.Drawing.Point(13, 113)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(75, 23)
        Me.btnPlay.TabIndex = 3
        Me.btnPlay.Text = "Play"
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(181, 112)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblInstruction
        '
        Me.lblInstruction.AutoSize = True
        Me.lblInstruction.Location = New System.Drawing.Point(13, 13)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.Size = New System.Drawing.Size(156, 13)
        Me.lblInstruction.TabIndex = 5
        Me.lblInstruction.Text = "Please Choose a difficulty level:"
        '
        'frmDifficultySelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(268, 147)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnPlay)
        Me.Controls.Add(Me.rdoImpossible)
        Me.Controls.Add(Me.rdoNormal)
        Me.Controls.Add(Me.rdoEasy)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmDifficultySelection"
        Me.Text = "Difficulty Selection"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rdoEasy As System.Windows.Forms.RadioButton
    Friend WithEvents rdoNormal As System.Windows.Forms.RadioButton
    Friend WithEvents rdoImpossible As System.Windows.Forms.RadioButton
    Friend WithEvents btnPlay As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblInstruction As System.Windows.Forms.Label
End Class
