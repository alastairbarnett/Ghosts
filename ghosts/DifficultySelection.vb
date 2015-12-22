Public Class frmDifficultySelection
    Public playClicked As Boolean = False
    Private Sub btnPlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlay.Click
        'Easy difficulty selected
        If rdoEasy.Checked Then
            frmPlayGhosts.difficultySelection = 0.0
        End If

        'Normal difficulty selected
        If rdoNormal.Checked Then
            frmPlayGhosts.difficultySelection = 0.5
        End If

        'Impossible difficulty selected
        If rdoImpossible.Checked Then
            frmPlayGhosts.difficultySelection = 1.0
        End If

        'Specify that the play button has been clicked
        playClicked = True

        'Close the window
        Me.Close()
    End Sub

    Private Sub frmDifficultySelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise the window
        GeneralModule.InitialiseWindow(Me)

        'Automatically select the play button
        btnPlay.Select()

        'Assume that the play button will not be clicked
        playClicked = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Exit without starting a game
        Me.Close()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef message As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys) As Boolean
        'If F1 was pressed then call the 'F1Pressed' subroutine
        If key = Keys.F1 Then
            frmHelp.F1Pressed()
        End If
        Return True
    End Function
End Class