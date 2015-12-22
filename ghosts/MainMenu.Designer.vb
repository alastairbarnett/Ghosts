<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainMenu
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
        Me.btnNewGame = New System.Windows.Forms.Button()
        Me.btnBrowsePlayedGames = New System.Windows.Forms.Button()
        Me.btnHighScores = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnNewGame
        '
        Me.btnNewGame.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewGame.Location = New System.Drawing.Point(12, 109)
        Me.btnNewGame.Name = "btnNewGame"
        Me.btnNewGame.Size = New System.Drawing.Size(284, 114)
        Me.btnNewGame.TabIndex = 0
        Me.btnNewGame.Text = "Start New Game"
        Me.btnNewGame.UseVisualStyleBackColor = True
        '
        'btnBrowsePlayedGames
        '
        Me.btnBrowsePlayedGames.Location = New System.Drawing.Point(366, 110)
        Me.btnBrowsePlayedGames.Name = "btnBrowsePlayedGames"
        Me.btnBrowsePlayedGames.Size = New System.Drawing.Size(123, 23)
        Me.btnBrowsePlayedGames.TabIndex = 1
        Me.btnBrowsePlayedGames.Text = "Browse Played Games"
        Me.btnBrowsePlayedGames.UseVisualStyleBackColor = True
        '
        'btnHighScores
        '
        Me.btnHighScores.Location = New System.Drawing.Point(366, 140)
        Me.btnHighScores.Name = "btnHighScores"
        Me.btnHighScores.Size = New System.Drawing.Size(122, 23)
        Me.btnHighScores.TabIndex = 2
        Me.btnHighScores.Text = "High Scores"
        Me.btnHighScores.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.Location = New System.Drawing.Point(366, 170)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(122, 23)
        Me.btnHelp.TabIndex = 3
        Me.btnHelp.Text = "Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(366, 200)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(121, 23)
        Me.btnExit.TabIndex = 4
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'frmMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(553, 236)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnHighScores)
        Me.Controls.Add(Me.btnBrowsePlayedGames)
        Me.Controls.Add(Me.btnNewGame)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmMainMenu"
        Me.Text = "Main Menu"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnNewGame As System.Windows.Forms.Button
    Friend WithEvents btnBrowsePlayedGames As System.Windows.Forms.Button
    Friend WithEvents btnHighScores As System.Windows.Forms.Button
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button

End Class
