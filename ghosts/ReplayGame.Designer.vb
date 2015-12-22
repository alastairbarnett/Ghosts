<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReplayGame
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
        Me.components = New System.ComponentModel.Container()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnAutomate = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.timAutomationTimer = New System.Windows.Forms.Timer(Me.components)
        Me.lblTaken = New System.Windows.Forms.Label()
        Me.lblLost = New System.Windows.Forms.Label()
        Me.lblInterval = New System.Windows.Forms.Label()
        Me.nupInterval = New System.Windows.Forms.NumericUpDown()
        CType(Me.nupInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(12, 532)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(75, 23)
        Me.btnNext.TabIndex = 0
        Me.btnNext.Text = "Next"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnAutomate
        '
        Me.btnAutomate.Location = New System.Drawing.Point(145, 532)
        Me.btnAutomate.Name = "btnAutomate"
        Me.btnAutomate.Size = New System.Drawing.Size(75, 23)
        Me.btnAutomate.TabIndex = 1
        Me.btnAutomate.Text = "Automate"
        Me.btnAutomate.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(401, 532)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'timAutomationTimer
        '
        Me.timAutomationTimer.Interval = 500
        '
        'lblTaken
        '
        Me.lblTaken.AutoSize = True
        Me.lblTaken.Location = New System.Drawing.Point(6, 24)
        Me.lblTaken.Name = "lblTaken"
        Me.lblTaken.Size = New System.Drawing.Size(38, 13)
        Me.lblTaken.TabIndex = 4
        Me.lblTaken.Text = "Taken"
        '
        'lblLost
        '
        Me.lblLost.AutoSize = True
        Me.lblLost.Location = New System.Drawing.Point(6, 94)
        Me.lblLost.Name = "lblLost"
        Me.lblLost.Size = New System.Drawing.Size(27, 13)
        Me.lblLost.TabIndex = 3
        Me.lblLost.Text = "Lost"
        '
        'lblInterval
        '
        Me.lblInterval.AutoSize = True
        Me.lblInterval.Location = New System.Drawing.Point(227, 537)
        Me.lblInterval.Name = "lblInterval"
        Me.lblInterval.Size = New System.Drawing.Size(91, 13)
        Me.lblInterval.TabIndex = 6
        Me.lblInterval.Text = "Interval (seconds)"
        '
        'nupInterval
        '
        Me.nupInterval.DecimalPlaces = 2
        Me.nupInterval.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nupInterval.Location = New System.Drawing.Point(320, 535)
        Me.nupInterval.Maximum = New Decimal(New Integer() {10, 0, 0, 65536})
        Me.nupInterval.Minimum = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nupInterval.Name = "nupInterval"
        Me.nupInterval.Size = New System.Drawing.Size(51, 20)
        Me.nupInterval.TabIndex = 5
        Me.nupInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nupInterval.Value = New Decimal(New Integer() {8, 0, 0, 65536})
        '
        'frmReplayGame
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 562)
        Me.Controls.Add(Me.lblInterval)
        Me.Controls.Add(Me.nupInterval)
        Me.Controls.Add(Me.lblTaken)
        Me.Controls.Add(Me.lblLost)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnAutomate)
        Me.Controls.Add(Me.btnNext)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmReplayGame"
        Me.Text = "Replay Game"
        CType(Me.nupInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnAutomate As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents timAutomationTimer As System.Windows.Forms.Timer
    Friend WithEvents lblTaken As System.Windows.Forms.Label
    Friend WithEvents lblLost As System.Windows.Forms.Label
    Friend WithEvents lblInterval As System.Windows.Forms.Label
    Friend WithEvents nupInterval As System.Windows.Forms.NumericUpDown
End Class
