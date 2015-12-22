Public Class frmPlayGhosts

    'Declare the game board
    Dim gameDetails As GameModule.recGameDetails

    'Declare the selection on the board
    Public selectionX As Integer = -1
    Public selectionY As Integer = -1

    'Declare the number of good ghosts selected by the player
    Dim goodGhostsSelected = 0

    'Declare a buffer to draw to the screen.
    Dim page As Graphics

    'Declare the difficulty
    Public difficultySelection = 0.5

    'Declare the flag to indicate if an action is currently in progress
    Dim locked = False

    'Declare the flag to indicate if the game is over
    Dim gameOver = False

    'Declare the array of rectangles for squares on the board
    Dim rectangles(GameModule.boardWidth - 1, GameModule.boardHeight - 1) As Rectangle

    'Time in milliseconds that the program stalls for between moves
    Const AIThinkTime = 0

    'Draws the board for play
    Private Sub DrawBoardForPlay()
        DrawBoardModule.DrawBoard(page, gameDetails, Me.ClientSize.Width, Me.ClientSize.Height, gameOver)
    End Sub

    'Handle the termination of the game
    Private Sub HandleGameTermination(ByVal gameState As Integer)
        UpdateInstruction("Game Over", Color.Black)

        'Inform the user of the game state
        AlertUserOfGameState(gameState, False)

        'Calculate a score for the game
        Dim playerScore = ScoreGame(gameState, gameDetails)

        'Find out the player's name
        GetPlayerName()

        'Insert the player's name and score into the high scores
        SaveHighScores(frmMainMenu.playerName, playerScore)

        'Append the game to the past games file
        EncodeAndSaveGame(playerScore, gameState, gameDetails)
        GeneralModule.Log("Suspect Intruider " & CInt(100.0 * AIModule.suspicion / 2.0) & "%", gameDetails)
        GeneralModule.Log("Send Bad " & CInt(100.0 * AIModule.badRate) & "%", gameDetails)
    End Sub

    'Alert the user of the state of the game through a message box
    Public Sub AlertUserOfGameState(ByVal gameState As Integer, ByVal isReplay As Boolean)
        Select Case gameState
            Case 1 'The AI won
                '"Computer" is a more user friendly term than "AI"
                MsgBox("The Computer Won!")
                If Not isReplay Then
                    GeneralModule.Log("AI Won", gameDetails)
                End If

            Case 2 'The player won
                If Not isReplay Then
                    'The player is the current user
                    MsgBox("The Player (you) Won!")
                Else
                    'The person replaying the game may not be the original player
                    MsgBox("The Player Won!")
                End If

                If Not isReplay Then
                    GeneralModule.Log("Player Won", gameDetails)
                End If

            Case 3 'A tie occurred
                MsgBox("A Tie occurred!")
                If Not isReplay Then
                    GeneralModule.Log("Tie", gameDetails)
                End If
        End Select
    End Sub

    'Calculate a score for a game
    Private Function ScoreGame(ByVal winner As Integer, ByVal gameDetails As recGameDetails) As Integer
        'Start the score at 2000
        Dim score As Integer = 2000

        'If the player wins
        If winner = 2 Then
            'Add a score based on the difficulty
            score = score + Int(gameDetails.difficulty * 8000) + 1000
        End If

        'If a tie occurs
        'This extremely rare case, so the score that it generates is high
        If winner = 3 Then
            'Add a score based on the difficulty
            score = score + Int(gameDetails.difficulty * 5000) + 5000
        End If

        'Initialise the counts of each type of piece
        Dim AIGoodTaken = 0
        Dim AIBadTaken = 0
        Dim playerGoodTaken = 0
        Dim playerBadTaken = 0

        'Iterate through the list of taken player pieces
        For i = 0 To (gameDetails.playerDead.Length) - 1
            'Increment the count of each type as it is found
            If gameDetails.pieceTable(gameDetails.playerDead(i)).isGood Then
                playerGoodTaken = playerGoodTaken + 1
            Else
                playerBadTaken = playerBadTaken + 1
            End If
        Next i

        'Iterate through the list of taken AI pieces
        For i = 0 To (gameDetails.AIDead.Length) - 1
            'Increment the count of each type as it is found
            If gameDetails.pieceTable(gameDetails.AIDead(i)).isGood Then
                AIGoodTaken = AIGoodTaken + 1
            Else
                AIBadTaken = AIBadTaken + 1
            End If
        Next i

        'Adjust the score taking the dead pieces into account
        score = score + 250 * (AIGoodTaken - AIBadTaken + playerBadTaken - playerBadTaken)

        'Integer is taken in case the variable has been automatically converted to a single
        Return CInt(score)
    End Function

    'Encode the selections of good pieces
    Private Function EncodeGoodPieceSelection(ByVal gameDetails) As String
        EncodeGoodPieceSelection = ""
        'Iterate through all pieces
        For i = 0 To totalPieces - 1
            'If the current piece is good
            If gameDetails.pieceTable(i).isGood Then
                'Append the location of the piece to the return string in the format XY
                EncodeGoodPieceSelection = EncodeGoodPieceSelection & CStr(gameDetails.pieceTable(i).originalPosition.x) & CStr(gameDetails.pieceTable(i).originalPosition.y)
            End If
        Next i
    End Function

    'Encode the current game and append it to the past games file
    Private Sub EncodeAndSaveGame(ByVal playerScore As Integer, ByVal gameState As Integer, ByVal gameDetails As GameModule.recGameDetails)
        'Declare the lines to write to the file with a header
        Dim linesInFile = {"[BEGIN]"}

        'Append the name of the player to the array
        AppendStringToArray(linesInFile, frmMainMenu.playerName)

        'Append the score of the game to the array
        AppendStringToArray(linesInFile, CStr(playerScore))

        'Append the difficulty to the array
        AppendStringToArray(linesInFile, CStr(gameDetails.difficulty))

        'Append an indicator of if the player won to the array
        AppendStringToArray(linesInFile, CStr(gameState = 2 Or gameState = 3))

        'Append the piece selections to the array
        AppendStringToArray(linesInFile, EncodeGoodPieceSelection(gameDetails))

        'Append the moves made in the game to the array
        For i = 0 To (gameDetails.encodedMoveLog.Length) - 1
            AppendStringToArray(linesInFile, gameDetails.encodedMoveLog(i))
        Next i

        'Append the array to the past games file
        AppendArrayToFile(frmMainMenu.pastGamesFilename, linesInFile)
    End Sub

    'Returns true if the string only contains alpha numeric characters (letters and numbers)
    Private Function UsesAlphaNumeric(ByVal text As String) As Boolean
        'Only alpha numeric characters by default.
        UsesAlphaNumeric = True

        'Iterate through all of the letters in the string
        For i = 0 To text.Length - 1
            Dim current As Integer = Asc(text.Chars(i))
            'If the current character is not within one of the ASCII ranges to be a letter or number
            'then return false
            If Not ((current >= Asc("A") And current <= Asc("Z")) _
                    Or (current >= Asc("a") And current <= Asc("z")) _
                    Or (current >= Asc("0") And current <= Asc("9"))) Then
                Return False
            End If
        Next i
    End Function

    'Get the name of the player
    Private Sub GetPlayerName()
        Dim validName As Boolean

        'Execute until the player name is valid
        'Iteration occurs at least once
        Do
            'Prompt the user for a name
            frmMainMenu.playerName = InputBox("Please enter your name and press enter:", "Enter Name", frmMainMenu.playerName)

            'Determine if the name is too long or uses characters that are not letters or numbers
            validName = frmMainMenu.playerName.length <= 10 And UsesAlphaNumeric(frmMainMenu.playerName)

            'Alert the user if the name is invalid
            If Not validName Then
                MsgBox("The name must be 10 characters or less and can only contain letters and numbers.")
            End If

            'Break out of the loop if the name is valid
        Loop Until validName

        'If no name was given, list the score with the name "Anonymous"
        If frmMainMenu.playerName = "" Then
            frmMainMenu.playerName = "Anonymous"
        End If
    End Sub

    'Move a piece from one position to another
    Private Sub MovePiece(ByVal startX As Integer, ByVal startY As Integer, ByVal endX As Integer, ByVal endY As Integer)
        'Fixes strange error
        selectionX = -1
        selectionY = -1

        'Find the piece being moved
        Dim pieceID As Integer = gameDetails.gameBoard(startX, startY)

        'Create a move structure
        Dim moveToMake As recGameMove
        moveToMake.moveType = GameModule.RootMoveCode(GameModule.MakeNewPosition(startX, startY), GameModule.MakeNewPosition(endX, endY))
        moveToMake.pieceIdentifier = pieceID

        'Apply the move to the board
        Dim gameState As Integer = GameModule.ApplyMove(moveToMake, gameDetails)

        'Draw the board
        DrawBoardForPlay()

        'If the game has finished
        If gameState <> 0 Then
            'Indicate that the game is over
            gameOver = True

            'Draw the board to reveal AI
            DrawBoardForPlay()

            'Handle the end of the game
            HandleGameTermination(gameState)

            'Attempt to close the window
            Me.Close()
        End If
    End Sub

    'Retrieve an AI move and apply the move
    Private Sub MoveAI()
        'Get the move to make
        Dim AIMove As GameModule.recGameMove = AIModule.AIMakeMove(gameDetails)

        'Find the position being moved to
        Dim newPosition As recPosition = GameModule.ResultantPosition(AIMove, gameDetails)

        'Move the piece
        MovePiece(gameDetails.pieceTable(AIMove.pieceIdentifier).position.x, gameDetails.pieceTable(AIMove.pieceIdentifier).position.y, newPosition.x, newPosition.y)
    End Sub

    'Handle the selection of a move by the player
    Private Sub HandleMoveSelection(ByVal x As Integer, ByVal y As Integer, ByVal currentSquarePlayers As Boolean)
        'If there is a selection that is not the square just clicked
        'and the current square is not the player's and the clicked square is adjacent to the selected square
        If selectionX <> -1 And selectionY <> -1 And Not (x = selectionX And y = selectionY) And _
                Not currentSquarePlayers And AIModule.Distance(GameModule.MakeNewPosition(selectionX, selectionY), GameModule.MakeNewPosition(x, y)) = 1 Then
            'If the selected square is not empty
            If gameDetails.gameBoard(selectionX, selectionY) <> -1 Then
                'Lock the board
                locked = True

                'Make the move if the game is not over
                If Not gameOver Then
                    MovePiece(selectionX, selectionY, x, y)
                End If

                'For "AI is thinking" suspense effect
                Threading.Thread.Sleep(AIThinkTime)

                'Make an AI move if the game is not over
                If Not gameOver Then
                    MoveAI()
                    UpdateInstruction("Please make a move.", Color.Black)
                End If

                'Unlock the board
                locked = False
            End If
        Else
            'If the selected square was clicked or a non-adjacent square was clicked
            If x = selectionX And y = selectionY Or Not currentSquarePlayers Then
                'Invalidate the current selection
                selectionX = -1
                selectionY = -1
            Else
                'Select the clicked square
                selectionX = x
                selectionY = y
            End If
        End If
    End Sub

    'Handle the section of pieces
    Private Sub HandleGoodPieceSelection(ByVal x As Integer, ByVal y As Integer)
        'If the clicked square is not empty
        If gameDetails.gameBoard(x, y) <> -1 Then
            'If the clicked square is a bad player piece
            If gameDetails.pieceTable(gameDetails.gameBoard(x, y)).isPlayers And Not gameDetails.pieceTable(gameDetails.gameBoard(x, y)).isGood Then
                'Make the piece good
                gameDetails.pieceTable(gameDetails.gameBoard(x, y)).isGood = True

                'Increment the number of good pieces
                goodGhostsSelected = goodGhostsSelected + 1

                If goodGhostsSelected = 3 Then
                    UpdateInstruction("Please choose 1 more good piece.", Color.Red)
                End If
                If goodGhostsSelected < 3 Then
                    UpdateInstruction("Please choose " & (4 - goodGhostsSelected) & " more good pieces.", Color.Red)
                End If

                'If all pieces have been selected
                If goodGhostsSelected = CInt(totalPieces / 4) Then
                    'Allow the AI to make a move 50% of the time
                    If GeneralModule.Threshold(0.5) Then
                        MoveAI()
                        UpdateInstruction("The computer moved first. Please make a move.", Color.Red)
                    Else
                        UpdateInstruction("You may move first. Please make a move.", Color.Red)
                    End If
                End If
            End If
        End If
    End Sub

    'Handles clicks on the board
    Private Sub HandleLeftMouseClick(ByVal x As Integer, ByVal y As Integer)
        'Iterate through all positions on the board
        For i = 0 To GameModule.boardWidth - 1
            For j = 0 To GameModule.boardHeight - 1
                'If the click occurred in the rectangle of the current square
                If rectangles(i, j).Contains(x, y) And Not locked Then
                    'Find out if the square contains a player piece
                    Dim currentSquarePlayers = False
                    If gameDetails.gameBoard(i, j) <> -1 Then
                        currentSquarePlayers = gameDetails.pieceTable(gameDetails.gameBoard(i, j)).isPlayers
                    End If

                    'If there are enough good ghosts selected then treat the click as a move,
                    'otherwise treat it as choosing a good a piece
                    If goodGhostsSelected = CInt(totalPieces / 4) Then
                        HandleMoveSelection(i, j, currentSquarePlayers)
                    Else
                        HandleGoodPieceSelection(i, j)
                    End If
                End If
            Next j
        Next i

        'Redraw the board
        DrawBoardForPlay()
    End Sub

    Private Sub FrmPlayGhosts_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        'If the left mouse button was clicked
        If e.Button = MouseButtons.Left Then
            'Handle the click
            HandleLeftMouseClick(e.X, e.Y)
        End If
    End Sub

    'Initialise the game for playing a game
    Private Sub InitialiseGameForPlay()
        'Set up the game
        gameDetails = Nothing
        gameOver = False
        selectionX = -1
        selectionY = -1
        goodGhostsSelected = 0
        GameModule.SetUpGame(gameDetails, False)
        gameDetails.difficulty = difficultySelection

        'Initialise the instruction label
        UpdateInstruction("Please choose 4 good pieces.", Color.Red)

        'Initialize graphics
        page = Me.CreateGraphics
        DrawBoardForPlay()
    End Sub

    Private Sub frmPlayGhosts_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise the window
        GeneralModule.InitialiseWindow(Me)

        'Set up clickable rectangles
        For i = 0 To GameModule.boardWidth - 1
            For j = 0 To GameModule.boardHeight - 1
                rectangles(i, j) = New Rectangle(cellWidth * i + boardStartX, cellHeight * j + boardStartY, cellWidth, cellHeight)
            Next j
        Next i

        'Seed the PRNG based on the time of day (hour, minute and second)
        Dim seed = TimeOfDay.Second + 60 * TimeOfDay.Minute + 60 * 60 * TimeOfDay.Hour + TimeOfDay.Day * 60 * 60 * 24
        Randomize(seed)

        'Stir up the PRNG, the first few outputs seem to be unresponsive to small changes in seed
        'invoking the first few avoids this problem
        Dim totalIterations As Integer = RandomInteger(30)
        For i = 0 To 10 + totalIterations
            Rnd()
        Next i

        'Initialise the game
        InitialiseGameForPlay()
    End Sub

    'Draw the board when the screen is refreshed
    Private Sub frmPlayGhosts_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        DrawBoardForPlay()
    End Sub

    'Leave the game
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        gameOver = True
        Me.Close()
    End Sub

    Private Sub btnRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestart.Click
        'Clarify choice with the user
        If MessageBox.Show("Are you sure that you want to restart the current game?", "Ghosts", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            'Restart the game
            InitialiseGameForPlay()
        End If
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        'Display help as dialog
        frmHelp.ShowDialog()
    End Sub

    Private Sub UpdateInstruction(ByVal newText As String, ByVal textColour As Color)
        'Update the instruction label
        lblInstruction.Text = newText

        'Set the text colour
        lblInstruction.ForeColor = textColour

        'Centre the instruction label
        lblInstruction.Location = New Point(CInt((Me.Width - lblInstruction.Width) / 2), lblInstruction.Location.Y)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef message As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys) As Boolean
        'If F1 was pressed then call the 'F1Pressed' subroutine
        If key = Keys.F1 Then
            frmHelp.F1Pressed()
        End If
        Return True
    End Function
End Class