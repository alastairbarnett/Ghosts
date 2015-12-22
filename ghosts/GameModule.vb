Module GameModule
    'Define constants
    Public Const boardWidth = 6
    Public Const boardHeight = boardWidth

    'Define structures
    Public Structure recPosition
        Dim x As Integer
        Dim y As Integer
    End Structure

    Public Structure recGameMove
        Dim pieceIdentifier As Integer
        Dim moveType As Integer
    End Structure

    Public Structure recGamePiece
        Dim isPlayers As Boolean
        Dim isGood As Boolean
        Dim isAlive As Boolean
        Dim position As recPosition
        Dim originalPosition As recPosition
    End Structure

    Public Structure recGameDetails
        Dim gameBoard(,) As Integer
        Dim pieceTable() As recGamePiece
        Dim playerWasFirst As Boolean
        Dim moveNumber As Integer
        Dim encodedMoveLog() As String
        Dim AIDead() As Integer
        Dim playerDead() As Integer
        Dim AIKnowledge As AIModule.recAIKnowledge
        Dim previousPosition As recPosition
        Dim previousMoveType As Integer
        Dim difficulty As Single
    End Structure

    'Make a new recPosition record with the specified coordinates
    Public Function MakeNewPosition(ByVal x As Integer, ByVal y As Integer) As recPosition
        MakeNewPosition.x = x
        MakeNewPosition.y = y
    End Function

    'Place a piece on the board at the given coordinates with the given identity and owner
    Private Sub PlacePiece(ByVal x As Integer, ByVal y As Integer, ByVal isPlayers As Boolean, ByVal isGood As Boolean, ByRef gameDetails As recGameDetails)
        'Generate a new piece ID
        Dim pieceID = gameDetails.pieceTable.Length

        'Place the piece on the board
        gameDetails.gameBoard(x, y) = pieceID

        'Set variables
        Dim currentPiece As recGamePiece
        currentPiece.position.x = x
        currentPiece.position.y = y
        currentPiece.originalPosition.x = x
        currentPiece.originalPosition.y = y
        currentPiece.isPlayers = isPlayers
        currentPiece.isGood = isGood
        currentPiece.isAlive = True

        'Add the piece to the piece table
        ReDim Preserve gameDetails.pieceTable(pieceID)
        gameDetails.pieceTable(pieceID) = currentPiece
    End Sub

    'Fill the board with bad pieces in starting positions
    Private Sub FillBoard(ByRef gameDetails As recGameDetails)
        'Empty the board
        For i = 0 To boardWidth - 1
            For j = 0 To boardHeight - 1
                gameDetails.gameBoard(i, j) = -1
            Next j
        Next i

        'Place the pieces
        For i = 1 To boardWidth - 2
            'Place pieces in top and bottom two rows, skipping the first and last column
            PlacePiece(i, 0, False, False, gameDetails)
            PlacePiece(i, 1, False, False, gameDetails)
            PlacePiece(i, boardHeight - 2, True, False, gameDetails)
            PlacePiece(i, boardHeight - 1, True, False, gameDetails)
        Next i
    End Sub

    Public Sub SetUpGame(ByRef gameDetails As recGameDetails, ByVal isReplay As Boolean)
        'Resize the board
        ReDim gameDetails.gameBoard(boardWidth - 1, boardHeight - 1)

        'Initialize Variables
        gameDetails.playerWasFirst = True
        gameDetails.pieceTable = {}
        gameDetails.encodedMoveLog = {}
        gameDetails.AIDead = {}
        gameDetails.playerDead = {}
        gameDetails.moveNumber = 0
        gameDetails.previousMoveType = -1

        'Fill the board
        FillBoard(gameDetails)

        'No AI is needed in a replay
        If Not isReplay Then
            'AI piece selection
            AIModule.ChooseGoodPieces(gameDetails)

            'Initialize the AI
            AIModule.SetUpAI(gameDetails.AIKnowledge, gameDetails.pieceTable)
        End If

        'Set to normal difficulty. This can be adjusted externally
        gameDetails.difficulty = 0.5
    End Sub

    'Determine the new position reached if a given move is made
    Public Function ResultantPosition(ByVal move As recGameMove, ByRef gameDetails As recGameDetails) As recPosition
        'Find the current position
        Dim x = gameDetails.pieceTable(move.pieceIdentifier).position.x
        Dim y = gameDetails.pieceTable(move.pieceIdentifier).position.y

        'Adjust the current position based on the move code (0-3)
        Select Case move.moveType
            Case 0
                y = y - 1
            Case 1
                x = x + 1
            Case 2
                y = y + 1
            Case 3
                x = x - 1
        End Select

        'Return the result as a recPosition
        Dim result As recPosition
        result.x = x
        result.y = y
        Return result
    End Function

    'Returns the move code required to move from one point to another.
    'The points must be adjacent.
    Public Function RootMoveCode(ByVal start As recPosition, ByVal finish As recPosition)
        'Find the change in y and x
        Dim dy As Integer = finish.y - start.y
        Dim dx As Integer = finish.x - start.x

        'Use nested binary selection to find the correct code
        If dy = 0 Then
            If dx = -1 Then
                RootMoveCode = 3
            Else
                RootMoveCode = 1
            End If
        Else
            If dy = -1 Then
                RootMoveCode = 0
            Else
                RootMoveCode = 2
            End If
        End If
    End Function

    'Tests if a given move is valid to make
    Public Function ValidMove(ByVal move As recGameMove, ByRef gameDetails As recGameDetails)
        'If there is an error than this will be caught by the try-catch statement and it will be indicated that the move is invalid
        Try
            'Test if the bounds of the move variable is valid.
            If move.moveType < 0 Or move.moveType > 3 Or move.pieceIdentifier < 0 Or move.pieceIdentifier >= totalPieces Then
                Return False
            End If

            'Find the position being moved to
            Dim newPosition As recPosition = ResultantPosition(move, gameDetails)

            'Test if the position is on the board
            If newPosition.x >= 0 And newPosition.x < boardWidth And newPosition.y >= 0 And newPosition.y < boardHeight Then
                'If the square is not empty
                If gameDetails.gameBoard(newPosition.x, newPosition.y) <> -1 Then
                    'Test if the piece in the square has a different owner to the piece being moved
                    If gameDetails.pieceTable(gameDetails.gameBoard(newPosition.x, newPosition.y)).isPlayers <> gameDetails.pieceTable(move.pieceIdentifier).isPlayers Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            'An error has occurred so there must be invalid data in the game or move (i.e. the move is most likely invalid)
            Return False
        End Try
    End Function

    'Tests if there is a piece fitting a given criteria at a given position
    Private Function PieceAtPosition(ByVal x As Integer, ByVal y As Integer, ByVal isPlayers As Boolean, ByVal isGood As Boolean, ByRef gameDetails As recGameDetails)
        'if the board is occupied on the given square
        If gameDetails.gameBoard(x, y) <> -1 Then
            'Retrieve the information of the current piece
            Dim currentPiece As recGamePiece = gameDetails.pieceTable(gameDetails.gameBoard(x, y))

            'If the piece fits the criteria then return true
            If currentPiece.isGood = isGood And currentPiece.isPlayers = isPlayers Then
                Return True
            End If
        End If
        'Return false by default
        Return False
    End Function

    'Check if there is a winner, the return values are as follows:
    '0: No winner, game is not finished
    '1: AI Wins
    '2: Player Wins
    '3: Tie
    Private Function CheckForWin(ByVal gameDetails As recGameDetails)
        'Initialise variables
        Dim playerGoodExists = False
        Dim playerBadExists = False
        Dim AIGoodExists = False
        Dim AIBadExists = False

        'Iterate through the pieces on the board
        For i = 0 To totalPieces - 1
            Dim currentPiece As recGamePiece = gameDetails.pieceTable(i)
            'If the current piece is on the board
            If currentPiece.isAlive Then
                'Specify that the current type of piece exists on the board
                If currentPiece.isPlayers Then
                    If currentPiece.isGood Then
                        playerGoodExists = True
                    Else
                        playerBadExists = True
                    End If
                Else
                    If currentPiece.isGood Then
                        AIGoodExists = True
                    Else
                        AIBadExists = True
                    End If
                End If
            End If
        Next i

        'Test if there is a good player piece on a player goal
        Dim playerAtEnd = PieceAtPosition(0, 0, True, True, gameDetails) Or PieceAtPosition(boardWidth - 1, 0, True, True, gameDetails)

        'If the difficulty is impossible
        If gameDetails.difficulty = 1.0 Then
            'If there is a bad AI on an AI goal, change its identity to good
            If PieceAtPosition(0, boardHeight - 1, False, False, gameDetails) Then
                GeneralModule.Log("Cheat: All AI are good", gameDetails)
                ChangePieceIdentity(gameDetails.gameBoard(0, boardHeight - 1), True, gameDetails)
            End If
            If PieceAtPosition(boardWidth - 1, boardHeight - 1, False, False, gameDetails) Then
                GeneralModule.Log("Cheat: All AI are good", gameDetails)
                ChangePieceIdentity(gameDetails.gameBoard(boardWidth - 1, boardHeight - 1), True, gameDetails)
            End If
        End If

        'Test if there is a good AI piece on an AI goal
        Dim AIAtEnd = PieceAtPosition(0, boardHeight - 1, False, True, gameDetails) Or PieceAtPosition(boardWidth - 1, boardHeight - 1, False, True, gameDetails)

        'Assume that there is no win
        CheckForWin = 0

        'Test if the player won
        If playerAtEnd Or (Not playerBadExists) Or (Not AIGoodExists) Then
            CheckForWin = 2
        End If

        'Test if the AI won
        If AIAtEnd Or (Not AIBadExists) Or (Not playerGoodExists) Then
            CheckForWin = 1
        End If

        'Test for a tie
        'This is a very rare case
        If (playerAtEnd And (Not AIBadExists)) Or (AIAtEnd And (Not playerBadExists)) Then
            CheckForWin = 3
        End If
    End Function

    'Remove a piece from play
    Private Sub KillPiece(ByVal pieceID As Integer, ByRef gameDetails As recGameDetails)
        'Specify that the piece is not alive
        gameDetails.pieceTable(pieceID).isAlive = False

        'Add the piece to the appropriate list of taken pieces
        If gameDetails.pieceTable(pieceID).isPlayers Then
            GeneralModule.AppendIntegerToArray(gameDetails.playerDead, pieceID)
        Else
            GeneralModule.AppendIntegerToArray(gameDetails.AIDead, pieceID)
        End If
    End Sub

    'Apply a given move to the board and then check for a winner
    Public Function ApplyMove(ByVal move As recGameMove, ByRef gameDetails As recGameDetails) As Integer

        'Test if the move is valid.
        If Not ValidMove(move, gameDetails) Then
            GeneralModule.Log("Error: An Invalid move has occurred.", gameDetails)
            MsgBox("Error: An Invalid move has occurred.")
            Return 0
        End If

        'Append the move to the encoded move log
        GeneralModule.AppendStringToArray(gameDetails.encodedMoveLog, CStr(gameDetails.pieceTable(move.pieceIdentifier).position.x) & CStr(gameDetails.pieceTable(move.pieceIdentifier).position.y) & CStr(move.moveType))

        'Record the move as the most recent move
        gameDetails.previousPosition = gameDetails.pieceTable(move.pieceIdentifier).position
        gameDetails.previousMoveType = move.moveType

        'Find the position being moved to
        Dim newPosition As recPosition = ResultantPosition(move, gameDetails)

        'Find the state of the position being moved to
        Dim pieceIdentifier = gameDetails.gameBoard(newPosition.x, newPosition.y)

        'Specify that the position being moved from is now empty
        gameDetails.gameBoard(gameDetails.pieceTable(move.pieceIdentifier).position.x, gameDetails.pieceTable(move.pieceIdentifier).position.y) = -1

        'If a piece is being taken
        If gameDetails.gameBoard(newPosition.x, newPosition.y) <> -1 Then
            'Get the information of the piece being taken
            Dim currentID As Integer = gameDetails.gameBoard(newPosition.x, newPosition.y)
            'If the difficulty is impossible and the piece is a good AI piece then swap its identity with a bad AI.
            If gameDetails.pieceTable(currentID).isGood And Not gameDetails.pieceTable(currentID).isPlayers And gameDetails.difficulty = 1.0 Then
                ChangePieceIdentity(currentID, False, gameDetails)
            End If

            'Take the piece being taken out of play
            KillPiece(currentID, gameDetails)
        End If

        'Make the move on the board
        gameDetails.gameBoard(newPosition.x, newPosition.y) = move.pieceIdentifier

        'Make the move in the piece table
        gameDetails.pieceTable(move.pieceIdentifier).position = newPosition

        'Increment the move number
        gameDetails.moveNumber = gameDetails.moveNumber + 1

        'Return the state of the game
        Return CheckForWin(gameDetails)
    End Function

    'Change the identity of a given AI piece
    Private Sub ChangePieceIdentity(ByVal pieceID As Integer, ByVal changeToGood As Boolean, ByVal gameDetails As recGameDetails)
        'The piece to swap with
        Dim swapAI = -1

        'Iterate through all pieces
        For i = 0 To totalPieces - 1
            Dim currentPiece = gameDetails.pieceTable(i)
            'Test if the current piece is an AI piece on the board with the desired identity
            If Not currentPiece.isPlayers And currentPiece.isAlive And currentPiece.isGood = changeToGood And i <> pieceID Then
                'Choose the piece
                swapAI = i
            End If
        Next i
        'Swap the identities of each piece
        gameDetails.pieceTable(swapAI).isGood = Not changeToGood
        gameDetails.pieceTable(pieceID).isGood = changeToGood
    End Sub

End Module
