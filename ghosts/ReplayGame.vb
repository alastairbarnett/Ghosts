Public Class frmReplayGame
    'Declare global game variables
    Public pastGame As frmSelectReplay.recPastGame
    Dim replayGameDetails As recGameDetails
    Dim page As Graphics

    'Globally declare timing constants
    'Allows easy reset of settings
    Const minimumInterval = 0.05
    Const maximumInterval = 1.0
    Const defaultInterval = 0.8
    Const millisecondsPerSecond = 1000

    'Draw the board
    Private Sub DrawBoardForReplay()
        DrawBoardModule.DrawBoard(page, replayGameDetails, Me.ClientSize.Width, Me.ClientSize.Height, True)
    End Sub

    'Initialise the board for a replay
    Private Sub InitialiseGameForReplay()
        'Empty the board
        replayGameDetails = Nothing

        'Set up the board
        SetUpGame(replayGameDetails, True)

        'Select the good pieces
        For i = 0 To CInt(totalPieces / 2) - 1
            'Get the x and y coordinates of the next good piece
            Dim x As Integer = CInt(CStr(pastGame.encodedMoveLog(0).Chars(2 * i)))
            Dim y As Integer = CInt(CStr(pastGame.encodedMoveLog(0).Chars(2 * i + 1)))

            'Make the piece at that position good
            Dim currentID As Integer = replayGameDetails.gameBoard(x, y)
            replayGameDetails.pieceTable(currentID).isGood = True
        Next i
    End Sub

    'Retrieve and apply the next move
    Private Sub ReplayNextMove()
        'Declare a structure for the next move
        Dim newMove As recGameMove = Nothing
        Dim x = CInt(CStr(pastGame.encodedMoveLog(replayGameDetails.moveNumber + 1).Chars(0)))
        Dim y = CInt(CStr(pastGame.encodedMoveLog(replayGameDetails.moveNumber + 1).Chars(1)))
        newMove.pieceIdentifier = replayGameDetails.gameBoard(x, y)
        newMove.moveType = CInt(CStr(pastGame.encodedMoveLog(replayGameDetails.moveNumber + 1).Chars(2)))

        'Make the move and observe the resulting game state
        Dim replayGameState = ApplyMove(newMove, replayGameDetails)

        'Refresh the game board
        DrawBoardForReplay()

        'If the game has finished
        If replayGameState <> 0 Then
            'Stop the automation timer
            timAutomationTimer.Stop()

            'Alert the user of the state of the game
            frmPlayGhosts.AlertUserOfGameState(replayGameState, True)

            'Closer the window
            Me.Close()
        End If
    End Sub

    Private Sub frmReplayGame_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise the window
        GeneralModule.InitialiseWindow(Me)

        'Initialise the board
        InitialiseGameForReplay()

        'Initialise the automation system
        btnAutomate.Text = "Automate"
        btnNext.Enabled = True

        'Initialise the interval selection tool
        nupInterval.Value = defaultInterval
        nupInterval.Minimum = minimumInterval
        nupInterval.Maximum = maximumInterval

        'Draw the board
        page = Me.CreateGraphics
        DrawBoardForReplay()
    End Sub

    'Replay the next move upon the click of the next button
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        ReplayNextMove()
    End Sub

    'Draw the screen
    Private Sub frmPlayGhosts_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        DrawBoardForReplay()
    End Sub

    Private Sub btnAutomate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutomate.Click
        'If automation if enabled
        If timAutomationTimer.Enabled Then
            'Disable automation
            timAutomationTimer.Stop()

            'Allow the user to re-enable automation
            btnAutomate.Text = "Automate"

            'Enable manual input
            btnNext.Enabled = True
        Else
            'Enable automation
            timAutomationTimer.Interval = CInt(nupInterval.Value * millisecondsPerSecond)
            timAutomationTimer.Start()

            'Make the next move instantly
            ReplayNextMove()

            'Allow the user to disable automation
            btnAutomate.Text = "Manual"

            'Disable manual input
            btnNext.Enabled = False
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Exit the form
        Me.Close()
    End Sub

    Private Sub timAutomationTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timAutomationTimer.Tick
        'Use automation to make the next move
        ReplayNextMove()
    End Sub

    Private Sub frmReplayGame_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Stop the automation timer when the form exits to avoid a crash
        timAutomationTimer.Stop()
    End Sub

    'Handle the changing of the automation timer interval by the user
    Private Sub nupInterval_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nupInterval.ValueChanged
        'Validate input to be above minimum
        If nupInterval.Value < minimumInterval Then
            nupInterval.Value = minimumInterval
        End If

        'Validate input to be below maximum
        If nupInterval.Value > maximumInterval Then
            nupInterval.Value = maximumInterval
        End If

        'Save the value into the timer interval
        timAutomationTimer.Interval = CInt(nupInterval.Value * millisecondsPerSecond)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef message As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys) As Boolean
        'If F1 was pressed then call the 'F1Pressed' subroutine
        If key = Keys.F1 Then
            frmHelp.F1Pressed()
        End If
        Return True
    End Function
End Class