<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHelp
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
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnGameRules = New System.Windows.Forms.Button()
        Me.btnReplay = New System.Windows.Forms.Button()
        Me.btnPlay = New System.Windows.Forms.Button()
        Me.btnHighScores = New System.Windows.Forms.Button()
        Me.btnChoosing = New System.Windows.Forms.Button()
        Me.btnMoving = New System.Windows.Forms.Button()
        Me.btnTaking = New System.Windows.Forms.Button()
        Me.btnWinning = New System.Windows.Forms.Button()
        Me.btnAvoiding = New System.Windows.Forms.Button()
        Me.grpRulesContainer = New System.Windows.Forms.GroupBox()
        Me.grpOtherTutorials = New System.Windows.Forms.GroupBox()
        Me.grpExternalHelp = New System.Windows.Forms.GroupBox()
        Me.btnHelpFile = New System.Windows.Forms.Button()
        Me.btnIntroduction = New System.Windows.Forms.Button()
        Me.grpRulesContainer.SuspendLayout()
        Me.grpOtherTutorials.SuspendLayout()
        Me.grpExternalHelp.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(19, 226)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(75, 23)
        Me.btnBack.TabIndex = 0
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnGameRules
        '
        Me.btnGameRules.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGameRules.Location = New System.Drawing.Point(6, 19)
        Me.btnGameRules.Name = "btnGameRules"
        Me.btnGameRules.Size = New System.Drawing.Size(102, 23)
        Me.btnGameRules.TabIndex = 1
        Me.btnGameRules.Text = "All Game Rules"
        Me.btnGameRules.UseVisualStyleBackColor = True
        '
        'btnReplay
        '
        Me.btnReplay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReplay.Location = New System.Drawing.Point(6, 50)
        Me.btnReplay.Name = "btnReplay"
        Me.btnReplay.Size = New System.Drawing.Size(114, 23)
        Me.btnReplay.TabIndex = 2
        Me.btnReplay.Text = "Replaying Games"
        Me.btnReplay.UseVisualStyleBackColor = True
        '
        'btnPlay
        '
        Me.btnPlay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPlay.Location = New System.Drawing.Point(6, 21)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(114, 23)
        Me.btnPlay.TabIndex = 3
        Me.btnPlay.Text = "Playing Games"
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'btnHighScores
        '
        Me.btnHighScores.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHighScores.Location = New System.Drawing.Point(6, 79)
        Me.btnHighScores.Name = "btnHighScores"
        Me.btnHighScores.Size = New System.Drawing.Size(114, 23)
        Me.btnHighScores.TabIndex = 4
        Me.btnHighScores.Text = "High Scores"
        Me.btnHighScores.UseVisualStyleBackColor = True
        '
        'btnChoosing
        '
        Me.btnChoosing.Location = New System.Drawing.Point(6, 58)
        Me.btnChoosing.Name = "btnChoosing"
        Me.btnChoosing.Size = New System.Drawing.Size(102, 23)
        Me.btnChoosing.TabIndex = 5
        Me.btnChoosing.Text = " Choosing Pieces"
        Me.btnChoosing.UseVisualStyleBackColor = True
        '
        'btnMoving
        '
        Me.btnMoving.Location = New System.Drawing.Point(6, 87)
        Me.btnMoving.Name = "btnMoving"
        Me.btnMoving.Size = New System.Drawing.Size(102, 23)
        Me.btnMoving.TabIndex = 6
        Me.btnMoving.Text = "Moving"
        Me.btnMoving.UseVisualStyleBackColor = True
        '
        'btnTaking
        '
        Me.btnTaking.Location = New System.Drawing.Point(6, 116)
        Me.btnTaking.Name = "btnTaking"
        Me.btnTaking.Size = New System.Drawing.Size(102, 23)
        Me.btnTaking.TabIndex = 7
        Me.btnTaking.Text = "Taking Pieces"
        Me.btnTaking.UseVisualStyleBackColor = True
        '
        'btnWinning
        '
        Me.btnWinning.Location = New System.Drawing.Point(6, 145)
        Me.btnWinning.Name = "btnWinning"
        Me.btnWinning.Size = New System.Drawing.Size(102, 23)
        Me.btnWinning.TabIndex = 8
        Me.btnWinning.Text = "Winning"
        Me.btnWinning.UseVisualStyleBackColor = True
        '
        'btnAvoiding
        '
        Me.btnAvoiding.Location = New System.Drawing.Point(6, 174)
        Me.btnAvoiding.Name = "btnAvoiding"
        Me.btnAvoiding.Size = New System.Drawing.Size(102, 23)
        Me.btnAvoiding.TabIndex = 9
        Me.btnAvoiding.Text = "Avoiding Defeat"
        Me.btnAvoiding.UseVisualStyleBackColor = True
        '
        'grpRulesContainer
        '
        Me.grpRulesContainer.Controls.Add(Me.btnChoosing)
        Me.grpRulesContainer.Controls.Add(Me.btnAvoiding)
        Me.grpRulesContainer.Controls.Add(Me.btnMoving)
        Me.grpRulesContainer.Controls.Add(Me.btnWinning)
        Me.grpRulesContainer.Controls.Add(Me.btnGameRules)
        Me.grpRulesContainer.Controls.Add(Me.btnTaking)
        Me.grpRulesContainer.Location = New System.Drawing.Point(13, 11)
        Me.grpRulesContainer.Name = "grpRulesContainer"
        Me.grpRulesContainer.Size = New System.Drawing.Size(129, 209)
        Me.grpRulesContainer.TabIndex = 10
        Me.grpRulesContainer.TabStop = False
        Me.grpRulesContainer.Text = "Game Rules Tutorials"
        '
        'grpOtherTutorials
        '
        Me.grpOtherTutorials.Controls.Add(Me.btnHighScores)
        Me.grpOtherTutorials.Controls.Add(Me.btnReplay)
        Me.grpOtherTutorials.Controls.Add(Me.btnPlay)
        Me.grpOtherTutorials.Location = New System.Drawing.Point(157, 11)
        Me.grpOtherTutorials.Name = "grpOtherTutorials"
        Me.grpOtherTutorials.Size = New System.Drawing.Size(138, 110)
        Me.grpOtherTutorials.TabIndex = 11
        Me.grpOtherTutorials.TabStop = False
        Me.grpOtherTutorials.Text = "Other Tutorials"
        '
        'grpExternalHelp
        '
        Me.grpExternalHelp.Controls.Add(Me.btnHelpFile)
        Me.grpExternalHelp.Controls.Add(Me.btnIntroduction)
        Me.grpExternalHelp.Location = New System.Drawing.Point(301, 12)
        Me.grpExternalHelp.Name = "grpExternalHelp"
        Me.grpExternalHelp.Size = New System.Drawing.Size(138, 80)
        Me.grpExternalHelp.TabIndex = 12
        Me.grpExternalHelp.TabStop = False
        Me.grpExternalHelp.Text = "External Help"
        '
        'btnHelpFile
        '
        Me.btnHelpFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHelpFile.Location = New System.Drawing.Point(6, 50)
        Me.btnHelpFile.Name = "btnHelpFile"
        Me.btnHelpFile.Size = New System.Drawing.Size(114, 23)
        Me.btnHelpFile.TabIndex = 2
        Me.btnHelpFile.Text = "Help File"
        Me.btnHelpFile.UseVisualStyleBackColor = True
        '
        'btnIntroduction
        '
        Me.btnIntroduction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnIntroduction.Location = New System.Drawing.Point(6, 21)
        Me.btnIntroduction.Name = "btnIntroduction"
        Me.btnIntroduction.Size = New System.Drawing.Size(114, 23)
        Me.btnIntroduction.TabIndex = 3
        Me.btnIntroduction.Text = "Introduction"
        Me.btnIntroduction.UseVisualStyleBackColor = True
        '
        'frmHelp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(451, 259)
        Me.Controls.Add(Me.grpExternalHelp)
        Me.Controls.Add(Me.grpOtherTutorials)
        Me.Controls.Add(Me.grpRulesContainer)
        Me.Controls.Add(Me.btnBack)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmHelp"
        Me.Text = "Help"
        Me.grpRulesContainer.ResumeLayout(False)
        Me.grpOtherTutorials.ResumeLayout(False)
        Me.grpExternalHelp.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnGameRules As System.Windows.Forms.Button
    Friend WithEvents btnReplay As System.Windows.Forms.Button
    Friend WithEvents btnPlay As System.Windows.Forms.Button
    Friend WithEvents btnHighScores As System.Windows.Forms.Button
    Friend WithEvents btnChoosing As System.Windows.Forms.Button
    Friend WithEvents btnMoving As System.Windows.Forms.Button
    Friend WithEvents btnTaking As System.Windows.Forms.Button
    Friend WithEvents btnWinning As System.Windows.Forms.Button
    Friend WithEvents btnAvoiding As System.Windows.Forms.Button
    Friend WithEvents grpRulesContainer As System.Windows.Forms.GroupBox
    Friend WithEvents grpOtherTutorials As System.Windows.Forms.GroupBox
    Friend WithEvents grpExternalHelp As System.Windows.Forms.GroupBox
    Friend WithEvents btnHelpFile As System.Windows.Forms.Button
    Friend WithEvents btnIntroduction As System.Windows.Forms.Button
End Class
