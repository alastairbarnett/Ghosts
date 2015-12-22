Module AIModule

    'Declare global variables
    Const boardWidth = GameModule.boardWidth
    Const boardHeight = GameModule.boardHeight

    'The total number of pieces on the board (at the start of the game)
    Public Const totalPieces = 2 * (boardWidth - 2) * 2

    'A length higher than all possible distances
    Const largeDistance = boardHeight * boardWidth + 1

    Public suspicion = 1.75
    Public badRate = 0.5

    Dim topLeft As recPosition = GameModule.MakeNewPosition(0, 0)
    Dim topRight As recPosition = GameModule.MakeNewPosition(boardWidth - 1, 0)
    Dim bottomLeft As recPosition = GameModule.MakeNewPosition(0, boardHeight - 1)
    Dim bottomRight As recPosition = GameModule.MakeNewPosition(boardWidth - 1, boardHeight - 1)

    'AIKnowledge structure used to store data for the AI
    Public Structure recAIKnowledge
        Dim suspectedIdentity() As Single
        Dim focalPiece As Integer
        Dim goal As recPosition
        Dim blacklist() As Integer
        Dim closestAIToTopLeft As Integer
        Dim closestAIToTopRight As Integer
        Dim playerBadConfirmed As Boolean
        Dim canTakeBadPlayer As Boolean
        Dim doWallStrategy As Boolean
    End Structure

    'AI choose good pieces. This is done randomly
    Public Sub ChooseGoodPieces(ByVal gameDetails As recGameDetails)
        Dim badAI = CInt(totalPieces / 2)
        'While there are more than 4 bad AI pieces, try to choose a good piece
        While badAI > CInt(totalPieces / 4)
            'Select a random piece
            Dim pieceToChangeIndex As Integer = GeneralModule.RandomInteger(AIModule.totalPieces)

            'Test if the piece is suitable (bad AI piece)
            If Not gameDetails.pieceTable(pieceToChangeIndex).isPlayers And Not gameDetails.pieceTable(pieceToChangeIndex).isGood Then
                'Make the piece good
                gameDetails.pieceTable(pieceToChangeIndex).isGood = True
                'Indicate the new number of bad pieces
                badAI = badAI - 1
            End If
        End While
    End Sub

    Public Sub SetUpAI(ByRef AIKnowledge As recAIKnowledge, ByVal pieceTable() As recGamePiece)

        'Iterate through all pieces that exist
        For i = 0 To totalPieces - 1
            ReDim Preserve AIKnowledge.suspectedIdentity(i)
            'If the piece is the player's initialise its suspected identity to unknown
            If pieceTable(i).isPlayers Then
                AIKnowledge.suspectedIdentity(i) = 0.5
            Else
                'If the piece is the AI's set its suspected identity to what it is
                If pieceTable(i).isGood Then
                    AIKnowledge.suspectedIdentity(i) = 1.0
                Else
                    AIKnowledge.suspectedIdentity(i) = 0.0
                End If
            End If
        Next i
        'Choose the rate at which bad AI are sent to opposite ends
        badRate = Random() * 0.7 + 0.3

        'Use the wall strategy for impossible AI some of the time.
        AIKnowledge.doWallStrategy = GeneralModule.Threshold(0.3)

        'Initialise variables
        AIKnowledge.focalPiece = -1
        AIKnowledge.blacklist = {}
        AIKnowledge.playerBadConfirmed = False
        AIKnowledge.closestAIToTopLeft = -1
        AIKnowledge.closestAIToTopRight = -1
        AIKnowledge.canTakeBadPlayer = True
    End Sub

    'Choose a valid random move
    Private Function RandomMove(ByRef gameDetails As GameModule.recGameDetails)
        Dim AIMove As GameModule.recGameMove

        'The moves that can be made, their frequency is chosen
        Dim allMoves() = {0, 1, 1, 2, 2, 2, 3, 3}
        'Shuffle the moves
        ShuffleArrayOfIntegers(allMoves)

        'Declare a list of moves to use
        Dim moves() As Integer = {}

        'Ensure that there is only one occurrence of each move type
        'Iterate through the moves with their distributions intact
        For k = 0 To allMoves.Length - 1
            'If the current move is not in the array then append it
            If Not InArray(allMoves(k), moves) Then
                GeneralModule.AppendIntegerToArray(moves, allMoves(k))
            End If
        Next

        'Declare variables
        Dim piecesToTry() As Integer = {}
        Dim i As Integer
        Dim j As Integer

        'Create a list of pieces that can be moved
        For i = 0 To totalPieces - 1
            'If the piece is on the board and the piece is an AI piece
            If Not gameDetails.pieceTable(i).isPlayers And gameDetails.pieceTable(i).isAlive Then
                'Add the piece to the array of pieces to be moved
                AppendIntegerToArray(piecesToTry, i)
            End If
        Next
        'Shuffle the pieces
        ShuffleArrayOfIntegers(piecesToTry)

        'Initialise counters
        i = 0
        j = 0

        'Declare a flag to indicate that a move has been found
        Dim validMoveFound As Boolean = False

        'While a move has not been found and all pieces have not been considered
        While Not validMoveFound And i < piecesToTry.Length
            'Choose a random valid move
            AIMove.pieceIdentifier = piecesToTry(i)
            j = 0
            'While all moves have not been considered and no move has been found
            While Not validMoveFound And j < moves.Length
                'Load the current move into the temporary move variable
                AIMove.moveType = moves(j)

                'Increment the move counter
                j = j + 1

                'Test the current move
                validMoveFound = GameModule.ValidMove(AIMove, gameDetails)
            End While
            'Increment the piece counter
            i = i + 1
        End While

        'Return the chosen move
        Return AIMove
    End Function

    'Count pieces given a specified criteria
    Private Function CountPieces(ByVal isPlayers As Boolean, ByVal mustBePlayers As Boolean, ByVal isGood As Boolean, ByVal mustBeGood As Boolean, ByRef gameDetails As GameModule.recGameDetails) As Integer
        'Initialise the count to 0
        CountPieces = 0

        'Iterate through all pieces
        For i = 0 To totalPieces - 1
            'If the piece is on the board
            If gameDetails.pieceTable(i).isAlive Then
                'If the piece fits the specified criteria
                If (gameDetails.pieceTable(i).isPlayers = isPlayers Or Not mustBePlayers) And (gameDetails.pieceTable(i).isGood = isGood Or Not mustBeGood) Then
                    'Increment the count of pieces
                    CountPieces = CountPieces + 1
                End If
            End If
        Next i
    End Function

    'The absolute value function for real numbers
    Private Function ABS(ByVal number)
        If number < 0 Then
            'Make negative values positive
            Return -number
        Else
            Return number
        End If
    End Function

    'Returns the distance in steps (not units of length) between two points
    Public Function Distance(ByVal a As recPosition, ByVal b As recPosition)
        Distance = ABS(a.y - b.y) + ABS(a.x - b.x)
    End Function

    'Returns true if the specified value is in the specified array, else it returns false
    Public Function InArray(ByVal value As Integer, ByVal arrayToSearch() As Integer) As Boolean
        'Assume not in the array
        InArray = False

        'Iterate through all indexes of the array
        For i = 0 To arrayToSearch.Length - 1
            'If the value is in the array at this index, set the return value to true
            If arrayToSearch(i) = value Then
                InArray = True
            End If
        Next i
    End Function

    'Finds the piece that is closest to a given square fitting a given criteria
    Private Function ClosestPieceToSquare(ByVal playersPiece As Boolean, ByVal isGood As Boolean, ByVal mustBeGood As Boolean, ByVal ignoreSuspectedBad As Boolean, ByVal ignoreGuards As Boolean, ByVal square As recPosition, ByRef gameDetails As GameModule.recGameDetails) As Integer
        'Assume that there is no suitable piece
        ClosestPieceToSquare = -1

        'Iterate through all pieces
        For i = 0 To totalPieces - 1
            'If the piece fits the primary criteria specified
            If gameDetails.pieceTable(i).isPlayers = playersPiece And gameDetails.pieceTable(i).isAlive And _
                (gameDetails.pieceTable(i).isGood = isGood Or (Not mustBeGood)) And ((Not ignoreSuspectedBad) Or (Not playersPiece) Or gameDetails.AIKnowledge.suspectedIdentity(i) <> 0.0) Then
                'If there is no piece chosen already, choose the current piece
                If ClosestPieceToSquare = -1 Then
                    ClosestPieceToSquare = i
                End If

                'If the piece fits the secondary criteria specified
                If Not InArray(i, gameDetails.AIKnowledge.blacklist) And _
                        ((Not ignoreGuards) Or (Not (SamePosition(gameDetails.pieceTable(i).position, GameModule.MakeNewPosition(0, 0)) Or SamePosition(gameDetails.pieceTable(i).position, GameModule.MakeNewPosition(boardWidth - 1, 0))))) Then
                    'If the current piece is closer to the specified square than the current chosen piece
                    If Distance(gameDetails.pieceTable(i).position, square) < _
                            Distance(gameDetails.pieceTable(ClosestPieceToSquare).position, square) Then
                        'Choose the current piece
                        ClosestPieceToSquare = i
                    End If
                End If
            End If
        Next i
    End Function

    'Return true if the two positions that are input have the same x and y values, otherwise false is returned
    Private Function SamePosition(ByVal a As recPosition, ByVal b As recPosition)
        Return a.x = b.x And a.y = b.y
    End Function

    'Returns true if the specified square is occupied by a piece fitting a given criteria, otherwise false is returned
    Private Function SquareOccupied(ByVal position As recPosition, ByVal isPlayers As Boolean, ByVal isGood As Boolean, ByVal definiteIdentity As Boolean, ByRef gameDetails As GameModule.recGameDetails) As Boolean
        'If the square is occupied
        If gameDetails.gameBoard(position.x, position.y) <> -1 Then
            'Get the information of the piece on the square
            Dim currentPiece As recGamePiece = gameDetails.pieceTable(gameDetails.gameBoard(position.x, position.y))

            'If the piece fits the specified criteria, return true
            If currentPiece.isPlayers = isPlayers And (isGood = currentPiece.isGood Or (Not definiteIdentity)) And currentPiece.isAlive Then
                Return True
            End If
        End If

        'Return false by default
        Return False
    End Function

    'Selects an identity to assume that a given player piece is
    Private Sub DecideIdentity(ByVal pieceID As Integer, ByVal gameDetails As recGameDetails)
        'Only continue if the piece does not already have a suspected identity
        If gameDetails.AIKnowledge.suspectedIdentity(pieceID) = 0.5 Then
            'Decide if the piece is good probabilistically

            'Determine how many bad pieces have been lost
            Dim lostBad As Integer = 0
            For i = 0 To gameDetails.playerDead.Length - 1
                If Not gameDetails.pieceTable(gameDetails.playerDead(i)).isGood Then
                    lostBad = lostBad + 1
                End If
            Next

            'The probability of assuming that the piece is good
            Dim goodProbability As Single = gameDetails.AIKnowledge.suspectedIdentity(pieceID) * suspicion
            Dim badProbability As Single = (1 - (goodProbability))

            'Adjust probability based on piece determinations
            Select Case lostBad
                'If there are few bad ghosts taken, increase the probability of taking a piece.
                Case 0 To CInt(totalPieces / 4) - 3
                    goodProbability = 1 - badProbability / 2

                'If there are only one or two bad ghosts left, decrease the probability of taking a piece.
                Case CInt(totalPieces / 4) - 2
                    goodProbability = 1 - badProbability * 3
                Case CInt(totalPieces / 4) - 1
                    goodProbability = 1 - badProbability * 5
            End Select

            GeneralModule.Log("Deciding Piece Identity: " & goodProbability & " good", gameDetails)

            'Decide the identity of the piece probabilistically
            If GeneralModule.Threshold(goodProbability) Then
                gameDetails.AIKnowledge.suspectedIdentity(pieceID) = 1.0
                GeneralModule.Log("Confirmed Good: Piece " & pieceID & " (" & gameDetails.pieceTable(pieceID).position.x & "," & gameDetails.pieceTable(pieceID).position.y & ")", gameDetails)
            Else
                'If there is a piece that is known to be bad, there is no risk in taking a bad piece
                'so all pieces are assumed to be good, Otherwise the piece is assumed to be bad
                If gameDetails.AIKnowledge.playerBadConfirmed Then
                    gameDetails.AIKnowledge.suspectedIdentity(pieceID) = 1.0
                    GeneralModule.Log("Confirmed Good: Piece " & pieceID & " (" & gameDetails.pieceTable(pieceID).position.x & "," & gameDetails.pieceTable(pieceID).position.y & ")", gameDetails)
                Else
                    gameDetails.AIKnowledge.suspectedIdentity(pieceID) = 0.0
                    GeneralModule.Log("Confirmed Bad: Piece " & pieceID & " (" & gameDetails.pieceTable(pieceID).position.x & "," & gameDetails.pieceTable(pieceID).position.y & ")", gameDetails)
                End If
                'Indicate to assume that all unknown pieces are bad
                gameDetails.AIKnowledge.playerBadConfirmed = True
            End If
        End If
    End Sub

    'Returns a move that needs to be made to prevent a player from moving to a goal
    Private Function HandleRaceToPlayerGoal(ByVal closestAI As Integer, ByVal closestPlayer As Integer, ByVal goalPosition As recPosition, ByRef gameDetails As recGameDetails) As recGameMove
        'Assume that no move needs to be made
        HandleRaceToPlayerGoal.moveType = -1

        'A move needs to be made if the closest AI piece is only one step closer to the specified goal
        'than the closest player piece given as input
        If Distance(gameDetails.pieceTable(closestAI).position, goalPosition) + 1 = _
            Distance(gameDetails.pieceTable(closestPlayer).position, goalPosition) Then
            'If the closest AI piece is not on the specified goal
            If Not SamePosition(gameDetails.pieceTable(closestAI).position, goalPosition) Then
                'Decide how to treat the piece that is invading
                DecideIdentity(closestPlayer, gameDetails)

                'If the piece invading is not suspected of being bad
                If gameDetails.AIKnowledge.suspectedIdentity(closestPlayer) <> 0.0 Then
                    'Move the closest AI piece to the goal
                    HandleRaceToPlayerGoal.pieceIdentifier = closestAI

                    'If the two pieces are adjacent
                    If Distance(gameDetails.pieceTable(closestAI).position, gameDetails.pieceTable(closestPlayer).position) = 1 Then
                        'Take the piece
                        HandleRaceToPlayerGoal.moveType = GameModule.RootMoveCode(gameDetails.pieceTable(closestAI).position, gameDetails.pieceTable(closestPlayer).position)
                    Else
                        'If this piece is involved in a plan, cancel the plan
                        If gameDetails.AIKnowledge.focalPiece = HandleRaceToPlayerGoal.pieceIdentifier Then
                            gameDetails.AIKnowledge.focalPiece = -1
                        End If

                        'If the piece is not in the first or last column then move horizontal otherwise move vertical (up)
                        If gameDetails.pieceTable(closestAI).position.x <> goalPosition.x Then
                            If SamePosition(goalPosition, topRight) Then
                                HandleRaceToPlayerGoal.moveType = 1
                            Else
                                HandleRaceToPlayerGoal.moveType = 3
                            End If
                        Else
                            HandleRaceToPlayerGoal.moveType = 0
                        End If
                    End If
                End If
            End If
        End If
            'If the AI piece is able to reach the goal without being stopped then it must be assumed to be bad.
            'This is because there is no possible advantage to assuming that it is good.
            If Distance(gameDetails.pieceTable(closestAI).position, goalPosition) + 1 > _
                Distance(gameDetails.pieceTable(closestPlayer).position, goalPosition) Then
                'If the identity of the piece is unknown
                If gameDetails.AIKnowledge.suspectedIdentity(closestPlayer) = 0.5 Then
                    'If there is a suspected bad piece found then assume that this piece is good, otherwise assume it is bad
                    If gameDetails.AIKnowledge.playerBadConfirmed Then
                        gameDetails.AIKnowledge.suspectedIdentity(closestPlayer) = 1.0
                        GeneralModule.Log("Confirmed Good: Piece " & closestPlayer & " (" & gameDetails.pieceTable(closestPlayer).position.x & "," & gameDetails.pieceTable(closestPlayer).position.y & ")", gameDetails)
                    Else
                        gameDetails.AIKnowledge.suspectedIdentity(closestPlayer) = 0.0
                        GeneralModule.Log("Confirmed Bad: Piece " & closestPlayer & " (" & gameDetails.pieceTable(closestPlayer).position.x & "," & gameDetails.pieceTable(closestPlayer).position.y & ")", gameDetails)
                    End If
                    'Indicate that there is a bad player piece found
                    gameDetails.AIKnowledge.playerBadConfirmed = True
                End If
            End If
    End Function

    'Find and return a move that needs to be made to block the player from winning in the future
    Private Function BlockingMoveSearch(ByRef gameDetails As GameModule.recGameDetails) As recGameMove
        'Assume no move will be made
        BlockingMoveSearch.moveType = -1

        'Find the closest pieces to both top right squares for AI and player
        Dim closestAIToTopRight As Integer = ClosestPieceToSquare(False, False, False, False, False, topRight, gameDetails)
        Dim closestAIToTopLeft As Integer = ClosestPieceToSquare(False, False, False, False, False, topLeft, gameDetails)
        Dim closestPlayerToTopRight As Integer = ClosestPieceToSquare(True, False, False, True, False, topRight, gameDetails)
        Dim closestPlayerToTopLeft As Integer = ClosestPieceToSquare(True, False, False, True, False, topLeft, gameDetails)

        'If there are no valid AI pieces that are identified as closest to the player goals, exit the function
        If closestPlayerToTopLeft = -1 Or closestPlayerToTopRight = -1 Then
            Return BlockingMoveSearch
        End If

        'See if the top right must be defended
        BlockingMoveSearch = HandleRaceToPlayerGoal(closestAIToTopRight, closestPlayerToTopRight, topRight, gameDetails)
        'If so, return the required move
        If BlockingMoveSearch.moveType <> -1 Then
            Return BlockingMoveSearch
        End If

        'See if the top left must be defended
        BlockingMoveSearch = HandleRaceToPlayerGoal(closestAIToTopLeft, closestPlayerToTopLeft, topLeft, gameDetails)
        'If so, return the required move
        If BlockingMoveSearch.moveType <> -1 Then
            Return BlockingMoveSearch
        End If

    End Function

    'The core of the Breadth First Search algorithm, this uses a flood fill method to find paths
    Private Function BFSFloodFill(ByRef discoveredBy(,) As recPosition, ByVal start As recPosition, ByVal finish As recPosition) As Boolean
        'Indicate that the start piece has been found
        discoveredBy(start.x, start.y) = start

        'Add the start to the queue of squares to investigate
        Dim pointQueue As Queue = New Queue
        pointQueue.Enqueue(start)

        'Indicates if the square being searched for has been found
        Dim endFound = False

        'While there are paths that have not been searched and the end has not been found
        While pointQueue.Count <> 0 And Not endFound
            'Retrieve the next square to search from the queue
            Dim current As recPosition = pointQueue.Dequeue()

            'Initialise the list of pieces that will be added
            Dim positionsToAdd() As recPosition = {}

            'Iterate through all connected squares to the current square
            'dy and dx denote change in x and y like in calculus
            For dx = -1 To 1
                For dy = -1 To 1
                    'If exactly one of the coordinates changes
                    If dx * dy = 0 And dy <> dx Then
                        'Find the new position
                        Dim x As Integer = current.x + dx
                        Dim y As Integer = current.y + dy

                        'If the new position is on the board
                        If x >= 0 And x < boardWidth And y >= 0 And y < boardHeight Then
                            'If the position has not yet been searched
                            If SamePosition(discoveredBy(x, y), GameModule.MakeNewPosition(-1, -1)) Then
                                'If the new position is the end position
                                If SamePosition(finish, GameModule.MakeNewPosition(x, y)) Then
                                    'Indicate that the end has been found
                                    endFound = True
                                End If

                                'Indicate that the new square has been found by the current square
                                discoveredBy(x, y) = current

                                'Add the new position to the list of positions to add to the queue
                                ReDim Preserve positionsToAdd(positionsToAdd.Length)
                                positionsToAdd(positionsToAdd.Length - 1) = GameModule.MakeNewPosition(x, y)
                            End If
                        End If
                    End If
                Next dy
            Next dx

            'Randomise the found squares.
            'This still takes a shortest path but randomises it to allow for random appearing behaviour
            GeneralModule.ShuffleArrayOfPositions(positionsToAdd)

            'Add the new positions to the queue
            If positionsToAdd.Length <> 0 Then
                For i = 0 To positionsToAdd.Length - 1
                    pointQueue.Enqueue(positionsToAdd(i))
                Next i
            End If
        End While

        'Return an indicator if the end was found
        Return endFound
    End Function

    'Initialise a BFS board
    Private Sub FillBFSBoardForAI(ByRef discoveredBy(,) As recPosition, ByVal gameDetails As recGameDetails)
        'Define constants (as variables)
        Dim wallBlock = GameModule.MakeNewPosition(-2, -2)
        Dim clearBlock = GameModule.MakeNewPosition(-1, -1)

        'Iterate through all positions on the board
        For i = 0 To boardWidth - 1
            For j = 0 To boardHeight - 1
                'Fill the current square with a clear block
                discoveredBy(i, j) = clearBlock

                'If the square is occupied with an AI piece then make it a wall
                If SquareOccupied(GameModule.MakeNewPosition(i, j), False, False, False, gameDetails) Then
                    discoveredBy(i, j) = wallBlock
                End If

                'If the square is occupied with a suspected bad player piece (and taking these pieces is forbidden) then make it a wall
                If SquareOccupied(GameModule.MakeNewPosition(i, j), True, False, Not gameDetails.AIKnowledge.canTakeBadPlayer, gameDetails) Then
                    If gameDetails.AIKnowledge.suspectedIdentity(gameDetails.gameBoard(i, j)) = 0.0 Then
                        discoveredBy(i, j) = wallBlock
                    End If
                End If
            Next j
        Next i
    End Sub

    'Conduct a breadth first search
    Private Function BreadthFirstSearch(ByVal start As recPosition, ByVal finish As recPosition, ByVal returnDistance As Boolean, ByRef gameDetails As recGameDetails) As Integer
        'Initialise the board
        Dim discoveredBy(boardWidth - 1, boardHeight - 1) As recPosition
        FillBFSBoardForAI(discoveredBy, gameDetails)

        'Search for a path between the two given positions
        Dim endFound As Boolean = BFSFloodFill(discoveredBy, start, finish)

        'Initialise the default values in case endFound was false
        If returnDistance Then
            'Distance is only found for the purpose comparison
            'largeDistance indicates that no path exists (as a default)
            BreadthFirstSearch = largeDistance
        Else
            'Indicates that no path was found (as a default)
            BreadthFirstSearch = -1
        End If

        'If there is a path between the two positions
        If endFound Then
            Dim nextPosition As recPosition = finish
            Dim totalDistance As Integer = 0

            'Backtrack from the end position to the start, counting the number of steps
            While Not SamePosition(discoveredBy(nextPosition.x, nextPosition.y), start)
                nextPosition = discoveredBy(nextPosition.x, nextPosition.y)
                totalDistance = totalDistance + 1
            End While

            'Return the appropriate information
            If returnDistance Then
                BreadthFirstSearch = totalDistance
            Else
                BreadthFirstSearch = GameModule.RootMoveCode(start, nextPosition)
            End If
        End If
    End Function

    'Move a good AI piece to a given goal if possible
    Private Function MoveToWinIfPossible(ByVal start As recPosition, ByVal finish As recPosition, ByRef gameDetails As GameModule.recGameDetails)
        Dim moveToMake As GameModule.recGameMove
        'Assume that no move is possible
        moveToMake.moveType = -1

        'If the specified start is occupied
        If gameDetails.gameBoard(start.x, start.y) <> -1 Then
            Dim currentPieceID As Integer = gameDetails.gameBoard(start.x, start.y)
            'If the piece in the square is good or the difficulty is impossible
            If (gameDetails.pieceTable(currentPieceID).isGood Or gameDetails.difficulty = 1.0) And Not gameDetails.pieceTable(currentPieceID).isPlayers Then
                'Move to the specified goal from the specified starting piece
                moveToMake.pieceIdentifier = currentPieceID
                moveToMake.moveType = GameModule.RootMoveCode(GameModule.MakeNewPosition(start.x, start.y), finish)
                Return moveToMake
            End If
        End If
        Return moveToMake
    End Function

    'Move a good AI piece to an AI goal if possible
    Private Function SearchForWinningMove(ByRef gameDetails As GameModule.recGameDetails)
        Dim move As GameModule.recGameMove

        'Try the bottom left goal from above
        move = MoveToWinIfPossible(GameModule.MakeNewPosition(0, boardHeight - 2), GameModule.MakeNewPosition(0, boardHeight - 1), gameDetails)
        If move.moveType <> -1 Then
            Return move
        End If

        'Try the bottom left goal from the right
        move = MoveToWinIfPossible(GameModule.MakeNewPosition(1, boardHeight - 1), GameModule.MakeNewPosition(0, boardHeight - 1), gameDetails)
        If move.moveType <> -1 Then
            Return move
        End If

        'Try the bottom right goal from the left
        move = MoveToWinIfPossible(GameModule.MakeNewPosition(boardWidth - 2, boardHeight - 1), GameModule.MakeNewPosition(0, boardHeight - 1), gameDetails)
        If move.moveType <> -1 Then
            Return move
        End If

        'Try the bottom right goal from above
        move = MoveToWinIfPossible(GameModule.MakeNewPosition(boardWidth - 1, boardHeight - 2), GameModule.MakeNewPosition(0, boardHeight - 1), gameDetails)
        If move.moveType <> -1 Then
            Return move
        End If

        'If no move has been found, return the move structure with -1 as its move (i.e. no move)
        Return move
    End Function

    'Search for a good AI piece with an unobstructed path to an AI goal.
    Private Function SearchForFutureWinningMove(ByRef gameDetails As GameModule.recGameDetails) As recGameMove
        'Assume that there is no move required
        SearchForFutureWinningMove.moveType = -1

        'Find closest good AI and player pieces to AI goals
        'If the difficulty is impossible bad AI may also be chosen
        Dim closestGoodAIToBottomRight As Integer = ClosestPieceToSquare(False, True, Not (gameDetails.difficulty = 1.0), False, False, bottomRight, gameDetails)
        Dim closestGoodAIToBottomLeft As Integer = ClosestPieceToSquare(False, True, Not (gameDetails.difficulty = 1.0), False, False, bottomLeft, gameDetails)
        Dim closestPlayerToBottomRight As Integer = ClosestPieceToSquare(True, False, False, False, False, bottomRight, gameDetails)
        Dim closestPlayerToBottomLeft As Integer = ClosestPieceToSquare(True, False, False, False, False, bottomLeft, gameDetails)

        'Find the closest player pieces to top corner squares that may pose a threat
        Dim closestPlayerToTopRight As Integer = ClosestPieceToSquare(True, False, False, True, False, topRight, gameDetails)
        Dim closestPlayerToTopLeft As Integer = ClosestPieceToSquare(True, False, False, True, False, topLeft, gameDetails)

        'Find the minimum distance that these player pieces are from their goal
        'Do not limit the distance by default
        Dim minimumDistance As Integer = largeDistance

        'Check if each value is -1 first to avoid a crash.
        If closestPlayerToTopLeft <> -1 Then
            minimumDistance = GeneralModule.Minimum(minimumDistance, Distance(gameDetails.pieceTable(closestPlayerToTopLeft).position, topLeft))
        End If
        If closestPlayerToTopRight <> -1 Then
            minimumDistance = GeneralModule.Minimum(minimumDistance, Distance(gameDetails.pieceTable(closestPlayerToTopRight).position, topRight))
        End If

        'Calculate distances for the bottom right corner
        Dim AIDistanceToBottomRight = Distance(gameDetails.pieceTable(closestGoodAIToBottomRight).position, bottomRight)
        Dim playerDistanceToBottomRight = Distance(gameDetails.pieceTable(closestPlayerToBottomRight).position, bottomRight)

        'If a suitable AI piece is closer to the bottom right corner than any player piece
        If AIDistanceToBottomRight <= playerDistanceToBottomRight And AIDistanceToBottomRight <= minimumDistance Then
            'If the piece is not on the goal, move to it using BFS (around obstacles)
            If Not SamePosition(gameDetails.pieceTable(closestGoodAIToBottomRight).position, bottomRight) Then
                SearchForFutureWinningMove.pieceIdentifier = closestGoodAIToBottomRight
                SearchForFutureWinningMove.moveType = BreadthFirstSearch(gameDetails.pieceTable(SearchForFutureWinningMove.pieceIdentifier).position, bottomRight, False, gameDetails)
            End If
        End If

        'Calculate distances for the bottom left corner
        Dim AIDistanceToBottomLeft = Distance(gameDetails.pieceTable(closestGoodAIToBottomLeft).position, bottomLeft)
        Dim playerDistanceToBottomLeft = Distance(gameDetails.pieceTable(closestPlayerToBottomLeft).position, bottomLeft)

        'If a suitable AI piece is closer to the bottom left corner than any player piece
        If AIDistanceToBottomLeft <= playerDistanceToBottomLeft And AIDistanceToBottomLeft <= minimumDistance Then
            'If the piece is not on the goal, move to it using BFS (around obstacles)
            If Not SamePosition(gameDetails.pieceTable(closestGoodAIToBottomLeft).position, bottomLeft) Then
                SearchForFutureWinningMove.pieceIdentifier = closestGoodAIToBottomLeft
                SearchForFutureWinningMove.moveType = BreadthFirstSearch(gameDetails.pieceTable(SearchForFutureWinningMove.pieceIdentifier).position, bottomLeft, False, gameDetails)
            End If
        End If
    End Function

    'Check a given square for an invading piece that must be taken and return the move required
    Private Function CheckInvaderSquare(ByVal square As recPosition, ByVal goal As recPosition, ByRef gameDetails As recGameDetails) As recGameMove
        Dim move As recGameMove
        'Assume no move is needed
        move.moveType = -1

        'If the square is occupied with an AI piece
        If SquareOccupied(GameModule.MakeNewPosition(square.x, square.y), True, False, False, gameDetails) Then
            'Decide the identity of the piece
            DecideIdentity(gameDetails.gameBoard(square.x, square.y), gameDetails)

            'Take the AI piece if its suspected identity is good (probability may also decide this)
            If GeneralModule.Threshold(gameDetails.AIKnowledge.suspectedIdentity(gameDetails.gameBoard(square.x, square.y)) * suspicion) Then
                move.moveType = GameModule.RootMoveCode(goal, square)
                move.pieceIdentifier = gameDetails.gameBoard(goal.x, goal.y)
            End If
        End If

        'Return the move that needs to be made
        Return move
    End Function

    'Check if a move needs to be made to stop the player from reaching a goal and if so return it
    Private Function StopNextMoveWin(ByRef gameDetails As GameModule.recGameDetails) As recGameMove
        Dim move As recGameMove
        'Assume no move is needed
        move.moveType = -1

        'If the top left square is protected then check the squares adjacent to it
        If SquareOccupied(topLeft, False, False, False, gameDetails) Then
            'Check to the right
            move = CheckInvaderSquare(GameModule.MakeNewPosition(1, 0), topLeft, gameDetails)
            If move.moveType <> -1 Then
                Return move
            End If

            'Check down
            move = CheckInvaderSquare(GameModule.MakeNewPosition(0, 1), topLeft, gameDetails)
            If move.moveType <> -1 Then
                Return move
            End If
        End If

        'If the top right square is protected then check the squares adjacent to it
        If SquareOccupied(topRight, False, False, False, gameDetails) Then
            'check down
            move = CheckInvaderSquare(GameModule.MakeNewPosition(boardWidth - 1, 1), topRight, gameDetails)
            If move.moveType <> -1 Then
                Return move
            End If

            'Check to the left
            move = CheckInvaderSquare(GameModule.MakeNewPosition(boardWidth - 2, 0), topRight, gameDetails)
            If move.moveType <> -1 Then
                Return move
            End If
        End If

        'Return the default value if nothing else has been returned
        Return move
    End Function

    'Find a piece to attempt to move to an AI goal
    Private Sub FindSuitablePieceToMove(ByVal isGood As Boolean, ByRef gameDetails As GameModule.recGameDetails)
        'Assume that there is no possible piece
        Dim closestFreePiece As Integer = -1

        'Set the default lowest distance higher than any distance on the board
        'This is not actually a distance but a suitability score
        'Actual distance will influence this score
        Dim currentLowestDistance As Integer = 8 * largeDistance
        Dim goal As recPosition = GameModule.MakeNewPosition(-1, -1)

        'Iterate through all pieces
        For i = 0 To totalPieces - 1
            'If the piece is on the board and is an AI piece
            If gameDetails.pieceTable(i).isAlive And Not gameDetails.pieceTable(i).isPlayers Then
                'The position of the current piece
                Dim currentPosition As recPosition = gameDetails.pieceTable(i).position

                'Indicates if the current piece is a guard (i.e. on a player goal)
                Dim isGuard As Boolean = i = gameDetails.AIKnowledge.closestAIToTopLeft Or i = gameDetails.AIKnowledge.closestAIToTopRight

                'Indicates if the current piece is the desired identity
                Dim isDesiredIdentity As Boolean = (gameDetails.pieceTable(i).isGood = isGood)

                'A variable which adjusts the distance to become a distance score
                Dim addedDistance = 0

                'Take in to account non length based factors
                If isDesiredIdentity Then
                    addedDistance = addedDistance + largeDistance
                End If
                If isGuard Then
                    addedDistance = addedDistance + 2 * largeDistance
                End If
                If InArray(i, gameDetails.AIKnowledge.blacklist) Then
                    addedDistance = addedDistance + 4 * largeDistance
                End If

                'If the current piece is the closest found so far (in score) to the bottom left then choose it as the new best piece
                If Distance(currentPosition, bottomLeft) + addedDistance < currentLowestDistance And BreadthFirstSearch(currentPosition, bottomLeft, False, gameDetails) <> -1 Then
                    currentLowestDistance = Distance(currentPosition, bottomLeft) + addedDistance
                    closestFreePiece = i
                    goal = bottomLeft
                End If

                'If the current piece is the closest found so far (in score) to the bottom right then choose it as the new best piece
                If Distance(currentPosition, bottomRight) + addedDistance < currentLowestDistance And BreadthFirstSearch(currentPosition, bottomRight, False, gameDetails) <> -1 Then
                    currentLowestDistance = Distance(currentPosition, bottomRight) + addedDistance
                    closestFreePiece = i
                    goal = bottomRight
                End If
            End If
        Next i

        'If a piece was chosen
        If closestFreePiece <> -1 Then

            'Make the goal an adjacent square to the actual goal
            'This is chosen probabilistically (above or side approach)
            If GeneralModule.Threshold(0.75) Then
                goal.y = goal.y - 1
            Else
                If SamePosition(goal, bottomLeft) Then
                    goal.x = goal.x + 1
                Else
                    goal.x = goal.x - 1
                End If

            End If

            'Enter the data into the AI data (focal piece and goal)
            gameDetails.AIKnowledge.focalPiece = closestFreePiece
            gameDetails.AIKnowledge.goal = goal

        Else
            'Set the focal piece to no piece
            gameDetails.AIKnowledge.focalPiece = -1
            GeneralModule.Log("Could not find suitable piece for plan", gameDetails)
        End If
    End Sub

    'Continue or start a plan
    Private Function StepToGoal(ByRef gameDetails As GameModule.recGameDetails, ByVal recursionDepth As Integer) As recGameMove
        'Assume no move needs to be made
        StepToGoal.moveType = -1

        'Ensure that the function is not called too many layers deep through recursion
        'because that can cause a crash.
        If recursionDepth > 10 Then
            GeneralModule.Log("Step to goal maximum recursion depth exceeded", gameDetails)
            Return StepToGoal
        End If

        'Indicates if there is a focal piece
        Dim noFocalPiece = (gameDetails.AIKnowledge.focalPiece = -1)

        'Indicates if the current focal piece is valid
        Dim validFocalPiece = False

        'Indicates if the current objective is complete
        Dim objectiveComplete = False

        'If there is a focal piece
        If Not noFocalPiece Then
            'If the focal piece is alive then state that it is valid
            If gameDetails.pieceTable(gameDetails.AIKnowledge.focalPiece).isAlive Then
                validFocalPiece = True
            End If

            'If the focal piece has reached its goal then then state this and add it to the blacklist
            If SamePosition(gameDetails.pieceTable(gameDetails.AIKnowledge.focalPiece).position, gameDetails.AIKnowledge.goal) Or _
                Distance(gameDetails.pieceTable(gameDetails.AIKnowledge.focalPiece).position, bottomLeft) = 1 Or _
                Distance(gameDetails.pieceTable(gameDetails.AIKnowledge.focalPiece).position, bottomRight) = 1 Then
                objectiveComplete = True
                BlacklistPiece(gameDetails.AIKnowledge.focalPiece, gameDetails)
            End If
        End If

        'If a new piece must be found then find one
        If noFocalPiece Or Not validFocalPiece Or objectiveComplete Then
            'Prefer bad AI at the rate of badRate
            Dim isGood = GeneralModule.Threshold(badRate)
            FindSuitablePieceToMove(isGood, gameDetails)
        End If

        'If there is a focal piece
        If gameDetails.AIKnowledge.focalPiece <> -1 Then
            'If the piece is in a side column ensure that its goal is directly above an AI goal
            If gameDetails.pieceTable(gameDetails.AIKnowledge.focalPiece).position.x = 0 Or gameDetails.pieceTable(gameDetails.AIKnowledge.focalPiece).position.x = boardWidth - 1 Then
                gameDetails.AIKnowledge.goal.x = gameDetails.pieceTable(gameDetails.AIKnowledge.focalPiece).position.x
                gameDetails.AIKnowledge.goal.y = boardHeight - 2
            End If

            'Set up the move data
            StepToGoal.pieceIdentifier = gameDetails.AIKnowledge.focalPiece
            StepToGoal.moveType = BreadthFirstSearch(gameDetails.pieceTable(StepToGoal.pieceIdentifier).position, gameDetails.AIKnowledge.goal, False, gameDetails)

            'If there is no possible move on the path
            If StepToGoal.moveType = -1 Then

                'Abandon  the current plan
                gameDetails.AIKnowledge.focalPiece = -1

                'Find a new plan (recursively)
                StepToGoal = StepToGoal(gameDetails, recursionDepth + 1)
            End If
        End If
    End Function

    'Add a given piece to the blacklist
    Private Sub BlacklistPiece(ByVal pieceID As Integer, ByRef gameDetails As recGameDetails)
        'Only add the piece if it is not yet in the list
        If Not InArray(pieceID, gameDetails.AIKnowledge.blacklist) Then
            GeneralModule.AppendIntegerToArray(gameDetails.AIKnowledge.blacklist, pieceID)
            GeneralModule.Log("Added piece to blacklist " & gameDetails.AIKnowledge.blacklist(gameDetails.AIKnowledge.blacklist.Length - 1), gameDetails)
        End If
    End Sub

    'Move towards a suspected good player piece
    Private Function ChaseGoodPlayerGhost(ByRef gameDetails As GameModule.recGameDetails) As recGameMove
        'Assume that no move needs to be made
        ChaseGoodPlayerGhost.moveType = -1

        Dim closestID As Integer = -1
        Dim targetID = -1
        Dim closestDistance As Integer = largeDistance

        'Iterate through all combination of piece
        For i = 0 To totalPieces - 1
            For j = 0 To totalPieces - 1
                'If the first piece is an AI piece, the second piece is a suspected good player piece and both pieces are on the board (alive)
                If gameDetails.pieceTable(j).isPlayers And gameDetails.pieceTable(j).isAlive And gameDetails.AIKnowledge.suspectedIdentity(j) = 1.0 And _
                    Not gameDetails.pieceTable(i).isPlayers And gameDetails.pieceTable(i).isAlive Then

                    'Find the positions of each piece
                    Dim chaserPosition As recPosition = gameDetails.pieceTable(i).position
                    Dim targetPosition As recPosition = gameDetails.pieceTable(j).position

                    'If the AI piece is not a guard
                    If Not (i = gameDetails.AIKnowledge.closestAIToTopLeft Or i = gameDetails.AIKnowledge.closestAIToTopRight) Then
                        'Find the distance of the shortest path to the AI piece
                        Dim currentDistance As Integer = BreadthFirstSearch(chaserPosition, targetPosition, True, gameDetails)

                        'If the current distance is lower than the distance between any other pair
                        If currentDistance < closestDistance Then
                            'Choose the current pair as the closest pair
                            closestID = i
                            targetID = j
                            closestDistance = currentDistance
                        End If
                    End If

                End If
            Next j
        Next i
        'If a pair of player and AI pieces have been found
        If closestID <> -1 And targetID <> -1 Then
            'Determine the move that needs to be made to take the player piece
            ChaseGoodPlayerGhost.pieceIdentifier = closestID
            ChaseGoodPlayerGhost.moveType = BreadthFirstSearch(gameDetails.pieceTable(closestID).position, gameDetails.pieceTable(targetID).position, False, gameDetails)
        End If

    End Function

    'Move a piece off a goal
    Private Function ClearPath(ByRef gameDetails As recGameDetails) As recGameMove
        'If the bottom left corner has a bad AI piece on it.
        If SquareOccupied(bottomLeft, False, False, True, gameDetails) Then
            ClearPath.pieceIdentifier = gameDetails.gameBoard(bottomLeft.x, bottomLeft.y)
            'Try to clear the path by moving to the side
            ClearPath.moveType = 1
            If GameModule.ValidMove(ClearPath, gameDetails) Then
                'Blacklist the piece on the square
                BlacklistPiece(ClearPath.pieceIdentifier, gameDetails)

                'Abandon the current plan
                gameDetails.AIKnowledge.focalPiece = -1

                'Return the move that needs to be made
                Return ClearPath
            End If
            'Clear the path by moving up
            ClearPath.moveType = 0
            If GameModule.ValidMove(ClearPath, gameDetails) Then
                'Blacklist the piece on the square
                BlacklistPiece(ClearPath.pieceIdentifier, gameDetails)

                'Abandon the current plan
                gameDetails.AIKnowledge.focalPiece = -1

                'Return the move that needs to be made
                Return ClearPath
            End If
        End If

        'If the bottom right corner has a bad AI piece on it.
        If SquareOccupied(bottomRight, False, False, True, gameDetails) Then
            ClearPath.pieceIdentifier = gameDetails.gameBoard(bottomRight.x, bottomRight.y)
            'Try to clear the path by moving to the side
            ClearPath.moveType = 3
            If GameModule.ValidMove(ClearPath, gameDetails) Then
                'Blacklist the piece on the square
                BlacklistPiece(ClearPath.pieceIdentifier, gameDetails)

                'Abandon the current plan
                gameDetails.AIKnowledge.focalPiece = -1

                'Return the move that needs to be made
                Return ClearPath
            End If

            'Clear the path by moving up
            ClearPath.moveType = 0
            If GameModule.ValidMove(ClearPath, gameDetails) Then
                'Blacklist the piece on the square
                BlacklistPiece(ClearPath.pieceIdentifier, gameDetails)

                'Abandon the current plan
                gameDetails.AIKnowledge.focalPiece = -1

                'Return the move that needs to be made
                Return ClearPath
            End If
        End If
        ClearPath.moveType = -1
    End Function

    'Take a suspected good player piece that is adjacent to an AI piece
    Private Function TakeAdjacentGoodGhost(ByVal gameDetails As recGameDetails) As recGameMove
        'Assume that no move needs to be made
        TakeAdjacentGoodGhost.moveType = -1

        'Iterate through the loop twice, some conditions will only pass on the second iteration
        For isSecondIteration = 0 To 1
            'Iterate through all combinations of piece
            For i = 0 To totalPieces - 1
                For j = 0 To totalPieces - 1
                    Dim firstPiece As recGamePiece = gameDetails.pieceTable(i)
                    Dim secondPiece As recGamePiece = gameDetails.pieceTable(j)

                    'If the pieces are adjacent, the first piece is an AI piece and the second piece is a suspected good AI piece (or can be assumed to be one).
                    'If the difficulty is impossible and this is the second loop iteration then allow taking bad pieces.
                    If Distance(firstPiece.position, secondPiece.position) = 1 And Not firstPiece.isPlayers And secondPiece.isPlayers And _
                        firstPiece.isAlive And secondPiece.isAlive And _
                        (gameDetails.AIKnowledge.suspectedIdentity(j) = 1.0 Or _
                        (gameDetails.AIKnowledge.playerBadConfirmed And gameDetails.AIKnowledge.suspectedIdentity(j) <> 0.0) Or _
                        (gameDetails.AIKnowledge.canTakeBadPlayer And gameDetails.difficulty = 1.0 And isSecondIteration = 1)) Then

                        'Return the move to take the piece
                        TakeAdjacentGoodGhost.pieceIdentifier = i
                        TakeAdjacentGoodGhost.moveType = GameModule.RootMoveCode(firstPiece.position, secondPiece.position)

                        'Exit the function
                        Return TakeAdjacentGoodGhost
                    End If
                Next j
            Next i
        Next isSecondIteration
    End Function

    'Deduce the number of good and bad player pieces remaining based on the number that have currently been taken
    Private Sub MakeDeductions(ByVal gameDetails As GameModule.recGameDetails)
        'Number of pieces remaining on the board
        Dim remainingPlayerGood As Integer = CountPieces(True, True, True, True, gameDetails)
        Dim remainingPlayerBad As Integer = CountPieces(True, True, False, True, gameDetails)

        'Number of pieces that have their identities suspected
        Dim foundPlayerGood As Integer = 0
        Dim foundPlayerBad As Integer = 0

        'Iterate through all pieces
        For i = 0 To totalPieces - 1
            'If the piece is the players and is on the board
            If gameDetails.pieceTable(i).isPlayers And gameDetails.pieceTable(i).isAlive Then
                'If the piece is suspected to be good then increment the count of found good player pieces
                If gameDetails.AIKnowledge.suspectedIdentity(i) >= 1.0 Then
                    foundPlayerGood = foundPlayerGood + 1
                End If
                'If the piece is suspected to be bad then increment the count of found bad player pieces
                If gameDetails.AIKnowledge.suspectedIdentity(i) <= 0.0 Then
                    foundPlayerBad = foundPlayerBad + 1
                End If
            End If
        Next i

        'If all good player pieces have been found then suspect all other player pieces of being bad
        If foundPlayerGood = remainingPlayerGood And foundPlayerBad <> remainingPlayerBad Then
            GeneralModule.Log("Found all good pieces, filling in bad pieces (" & remainingPlayerBad & "," & remainingPlayerGood & ")", gameDetails)
            For i = 0 To totalPieces - 1
                If gameDetails.pieceTable(i).isPlayers And gameDetails.pieceTable(i).isAlive And gameDetails.AIKnowledge.suspectedIdentity(i) <> 1.0 Then
                    gameDetails.AIKnowledge.suspectedIdentity(i) = 0.0
                End If
            Next i
        End If

        'If all bad player pieces have been found then suspect all other player pieces of being good
        If foundPlayerBad = remainingPlayerBad And foundPlayerGood <> remainingPlayerGood Then
            GeneralModule.Log("Found all bad pieces, filling in good pieces (" & remainingPlayerBad & "," & remainingPlayerGood & ")", gameDetails)
            For i = 0 To totalPieces - 1
                If gameDetails.pieceTable(i).isPlayers And gameDetails.pieceTable(i).isAlive And gameDetails.AIKnowledge.suspectedIdentity(i) <> 0.0 Then
                    gameDetails.AIKnowledge.suspectedIdentity(i) = 1.0
                End If
            Next i
        End If
    End Sub

    'Determine if any AI pieces that are the closest to an AI goal cannot be moved
    Private Sub DetermineSafety(ByRef gameDetails As recGameDetails)
        'The 'goals'
        Dim squares() As recPosition = {topLeft, topRight}

        'Iterate through the goals
        For j = 0 To squares.Length - 1
            'Find the current goal
            Dim square As recPosition = squares(j)

            'Initialize variables
            Dim closestPiece = -1
            Dim lowestDistance = largeDistance
            Dim uniqueDistance = True

            'Iterate through all pieces
            For i = 0 To totalPieces - 1
                'If the piece is the AIs and on the board
                If Not gameDetails.pieceTable(i).isPlayers And gameDetails.pieceTable(i).isAlive Then
                    'Find the distance of the current piece to the goal
                    Dim currentDistance = Distance(gameDetails.pieceTable(i).position, square)

                    'If this distance is lower than the current lowest distance
                    If currentDistance < lowestDistance Then
                        'Choose the new closest piece
                        closestPiece = i
                        lowestDistance = currentDistance

                        'As far as is known now, this distance is unique
                        uniqueDistance = True
                    Else
                        'If the distance is not unique
                        If currentDistance = lowestDistance Then
                            'Indicate that the distance is not unique
                            uniqueDistance = False
                        End If
                    End If
                End If
            Next i

            'If the distance is not unique, disregard the piece
            If Not uniqueDistance Then
                closestPiece = -1
            End If

            'Save the data in its correct field
            If SamePosition(square, topLeft) Then
                gameDetails.AIKnowledge.closestAIToTopLeft = closestPiece
            Else
                gameDetails.AIKnowledge.closestAIToTopRight = closestPiece
            End If
        Next j
    End Sub

    Private Sub RevealPlayerPieces(ByRef gameDetails As GameModule.recGameDetails)
        'Iterate through all pieces and set their suspected identities to their real identities
        For i = 0 To totalPieces - 1
            If gameDetails.pieceTable(i).isGood Then
                gameDetails.AIKnowledge.suspectedIdentity(i) = 1.0
            Else
                gameDetails.AIKnowledge.suspectedIdentity(i) = 0.0
            End If
        Next i
    End Sub



    'Execute all methods of finding moves to make in order of importance
    'The function is exited when the first required move is found
    Private Function AdvancedMove(ByRef gameDetails As GameModule.recGameDetails) As GameModule.recGameMove
        Dim move As GameModule.recGameMove

        'The rate at which found moves are not skipped by easy AI 
        Const easyDoRate = 0.4

        'If the difficulty is impossible reveal the player pieces
        If gameDetails.difficulty = 1.0 Then
            RevealPlayerPieces(gameDetails)
        End If

        'Determine if taking bad player pieces can be done without losing
        If CountPieces(True, True, False, True, gameDetails) = 1 Then
            gameDetails.AIKnowledge.canTakeBadPlayer = False
        End If

        'Make deductions about the pieces identities
        MakeDeductions(gameDetails)

        'Determine which AI can be safely moved
        DetermineSafety(gameDetails)

        'Try to win next move
        move = SearchForWinningMove(gameDetails)
        'Return the move if one was found, possibly skip the move it difficulty is easy
        If move.moveType <> -1 And (GeneralModule.Threshold(easyDoRate) Or gameDetails.difficulty <> 0.0) Then
            GeneralModule.Log("Win " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Try to stop the player from winning next turn
        move = StopNextMoveWin(gameDetails)
        If move.moveType <> -1 And (GeneralModule.Threshold(easyDoRate) Or gameDetails.difficulty <> 0.0) Then
            GeneralModule.Log("Stop Win " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Try to move unobstructed to an AI goal
        move = SearchForFutureWinningMove(gameDetails)
        If move.moveType <> -1 And (GeneralModule.Threshold(easyDoRate) Or gameDetails.difficulty <> 0.0) Then
            GeneralModule.Log("Chase Win " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Prevent the player from reaching their goal in the future
        move = BlockingMoveSearch(gameDetails)
        If move.moveType <> -1 And (GeneralModule.Threshold(easyDoRate) Or gameDetails.difficulty <> 0.0) Then
            GeneralModule.Log("Stop Player " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Take an adjacent suspected good ghost
        move = TakeAdjacentGoodGhost(gameDetails)
        If move.moveType <> -1 And (GeneralModule.Threshold(easyDoRate) Or gameDetails.difficulty <> 0.0) Then
            GeneralModule.Log("Take " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Move toward adjacent suspected good ghost
        move = ChaseGoodPlayerGhost(gameDetails)
        If move.moveType <> -1 And (GeneralModule.Threshold(easyDoRate) Or gameDetails.difficulty <> 0.0) Then
            GeneralModule.Log("Chase Good " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Move a piece off an AI goal
        move = ClearPath(gameDetails)
        If move.moveType <> -1 And (GeneralModule.Threshold(easyDoRate) Or gameDetails.difficulty <> 0.0) Then
            GeneralModule.Log("Clear " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Start or continue a plan, initialising the recursive depth to 1
        move = StepToGoal(gameDetails, 1)
        If move.moveType <> -1 And (GeneralModule.Threshold(easyDoRate) Or gameDetails.difficulty <> 0.0) Then
            GeneralModule.Log("Plan Continue " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") to (" & gameDetails.AIKnowledge.goal.x & "," & gameDetails.AIKnowledge.goal.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Make a random valid move
        move = RandomMove(gameDetails)
        GeneralModule.Log("Random Move " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
        Return move
    End Function

    'Define the row of the wall
    Const wallRow = 1

    'Take a player piece in the wall if there is one to take
    Private Function TakeInvaderInWall(ByVal gameDetails As recGameDetails) As recGameMove
        'By default there is no move that needs to be made
        TakeInvaderInWall.moveType = -1

        'The position of the piece that needs to be taken
        Dim invaderPosition As recPosition = GameModule.MakeNewPosition(-1, -1)

        'Iterate through the wall to find an invader
        For i = 0 To boardWidth - 1
            Dim currentID As Integer = gameDetails.gameBoard(i, wallRow)

            'If the current square is occupied
            If currentID <> -1 Then
                Dim currentPiece As recGamePiece = gameDetails.pieceTable(currentID)
                'If the current piece is an invader and it can be taken (i.e. no the last bad player piece)
                'then note its position
                If currentPiece.isAlive And currentPiece.isPlayers And (gameDetails.AIKnowledge.canTakeBadPlayer Or gameDetails.AIKnowledge.suspectedIdentity(currentID) = 1.0) Then
                    invaderPosition = GameModule.MakeNewPosition(i, wallRow)
                End If
            End If
        Next i

        'If there is an invader to take
        If Not SamePosition(invaderPosition, GameModule.MakeNewPosition(-1, -1)) Then
            'Generate the positions to the left, the right and above the invader.
            Dim squareToLeft As recPosition = invaderPosition
            squareToLeft.x = squareToLeft.x - 1
            Dim squareToRight As recPosition = invaderPosition
            squareToRight.x = squareToRight.x + 1
            Dim squareAbove As recPosition = invaderPosition
            squareAbove.y = squareToRight.y - 1

            Dim positionsToTry() As recPosition

            'Choose the order in which to try positions.
            'This moves pieces from above the tries to move pieces from the middle of the wall as the middle is stronger
            'and less vulnerable
            If invaderPosition.x >= CInt(boardWidth / 2) Then
                positionsToTry = {squareAbove, squareToLeft, squareToRight}
            Else
                positionsToTry = {squareAbove, squareToRight, squareToLeft}
            End If

            'Iterate through these positions
            For i = 0 To positionsToTry.Length - 1
                Dim currentPosition As recPosition = positionsToTry(i)
                'If the position exists on the board
                If currentPosition.x >= 0 And currentPosition.x < boardWidth Then
                    'If the position is occupied by an AI piece
                    If SquareOccupied(currentPosition, False, False, False, gameDetails) Then
                        'Take the invader from the current position
                        TakeInvaderInWall.pieceIdentifier = gameDetails.gameBoard(currentPosition.x, currentPosition.y)
                        TakeInvaderInWall.moveType = RootMoveCode(currentPosition, invaderPosition)
                        Return TakeInvaderInWall
                    End If
                End If
            Next
        End If
    End Function

    'If there is a hole in the wall and a piece is able to repair the hole then
    'action is taken to repair the hole.
    Private Function FillHoleInWall(ByVal gameDetails As recGameDetails) As recGameMove
        'By default there is no move that needs to be made
        FillHoleInWall.moveType = -1

        'Initialise variables
        Dim closestHoleDistance As Integer = largeDistance
        Dim pieceToMove As Integer = -1
        Dim holePosition As recPosition = GameModule.MakeNewPosition(-1, -1)

        'Find the closest free AI piece to a hole in the wall
        For i = 0 To boardWidth - 1
            'If there is a hole at the current position
            If gameDetails.gameBoard(i, wallRow) = -1 Then
                'Find the closest piece outside of the wall to fill the hole
                For j = 0 To totalPieces - 1
                    Dim currentHolePosition As recPosition = GameModule.MakeNewPosition(i, wallRow)
                    Dim currentID As Integer = j
                    Dim currentPiece As recGamePiece = gameDetails.pieceTable(currentID)

                    'If the piece is on the board and is an AI piece
                    If currentPiece.isAlive And Not currentPiece.isPlayers Then
                        'If the current piece is closer than any previous piece than note this piece down as the piece to move.
                        Dim currentDistance As Integer = BreadthFirstSearch(currentPiece.position, currentHolePosition, True, gameDetails)
                        If currentDistance < closestHoleDistance And currentPiece.position.y <> wallRow Then
                            pieceToMove = j
                            holePosition = currentHolePosition
                            closestHoleDistance = currentDistance
                        End If
                    End If
                Next j
            End If
        Next i
        'If there is a piece to move then make the move
        If pieceToMove <> -1 Then
            FillHoleInWall.moveType = BreadthFirstSearch(gameDetails.pieceTable(pieceToMove).position, holePosition, False, gameDetails)
            FillHoleInWall.pieceIdentifier = pieceToMove
        End If
    End Function

    'Returns true if the input move is valid and does not 
    Private Function WallSafeMove(ByVal move As recGameMove, ByVal gameDetails As recGameDetails) As Boolean
        'If the move is not valid then false must be returned
        If Not ValidMove(move, gameDetails) Then
            Return False
        End If
        Dim newPosition As recPosition = GameModule.ResultantPosition(move, gameDetails)
        Dim pieceInSquare As Integer = gameDetails.gameBoard(newPosition.x, newPosition.y)
        'If the square being moved to contains a bad player piece and that piece cannot safely be taken then the move is unsafe
        If pieceInSquare <> -1 Then
            If gameDetails.AIKnowledge.suspectedIdentity(pieceInSquare) = 0.0 And Not gameDetails.AIKnowledge.canTakeBadPlayer Then
                Return False
            End If
        End If

        'Otherwise the move is assumed to be safe.
        Return True
    End Function

    'Make a move that will not destroy the integrity of the wall
    Private Function WallSafeRandomMove(ByVal gameDetails As recGameDetails) As recGameMove
        'Assume that there is no move that needs to be made by default
        WallSafeRandomMove.moveType = -1

        'List the preferences of moves
        Dim columnIterationOrder(boardWidth - 1) As Integer
        For i = 0 To CInt(boardWidth / 2 - 1)
            'Iterate from the middle to the start of the board
            columnIterationOrder(2 * i) = CInt(boardWidth / 2 - 1) - i
            'Iterate from the middle to the end of the board
            columnIterationOrder(2 * i + 1) = CInt(boardWidth / 2 - 1) + i + 1
        Next i

        'Iterate through all pieces before and in the wall, considering the pieces before the wall first
        For j = 0 To wallRow
            For i = 0 To boardWidth - 1
                'Choose the current square based on preferences
                'this allows iteration from the middle out
                Dim currentID As Integer = gameDetails.gameBoard(columnIterationOrder(i), j)

                'If the current square is not empty then try to make a move
                If currentID <> -1 Then
                    Dim currentPiece As recGamePiece = gameDetails.pieceTable(currentID)
                    If currentPiece.isAlive And Not currentPiece.isPlayers Then
                        Dim tempMove As recGameMove
                        Dim movesToTry() As Integer
                        'If there are no AI pieces taken then move pieces to the square above the wall 
                        'that is adjacent to the player goals, otherwise move pieces to the middle of the board
                        If CountPieces(False, True, False, False, gameDetails) = CInt(totalPieces / 2) Then
                            If currentPiece.position.x < CInt(boardWidth / 2) Then
                                movesToTry = {3, 1}
                            Else
                                movesToTry = {1, 3}
                            End If
                        Else
                            If currentPiece.position.x < CInt(boardWidth / 2) Then
                                movesToTry = {1, 3}
                            Else
                                movesToTry = {3, 1}
                            End If
                        End If
                        'Iterate through the preferences and make the first one possible
                        For k = 0 To movesToTry.Length - 1
                            tempMove.pieceIdentifier = currentID
                            tempMove.moveType = movesToTry(k)
                            'If the move is possible make it
                            If WallSafeMove(tempMove, gameDetails) Then
                                Return tempMove
                            End If
                        Next k
                    End If
                End If
            Next i
        Next j

        'If no preferred move could be made then make a move up
        For i = 0 To boardWidth - 1
            'Iterate from the middle out
            Dim currentID As Integer = gameDetails.gameBoard(columnIterationOrder(i), wallRow)

            'If the square is not occupied
            If currentID <> -1 Then
                Dim currentPiece As recGamePiece = gameDetails.pieceTable(currentID)

                'If the current piece is alive and is the players
                If currentPiece.isAlive And Not currentPiece.isPlayers Then
                    Dim tempMove As recGameMove
                    'If it is possible to make a move up, then do so
                    tempMove.pieceIdentifier = currentID
                    tempMove.moveType = 0
                    If WallSafeMove(tempMove, gameDetails) Then
                        Return tempMove
                    End If
                End If
            End If
        Next i

    End Function

    'This subroutine uses a strategy of creating a wall for impossible difficulty AI
    Private Function WallAdvancedMove(ByVal gameDetails As recGameDetails) As recGameMove
        Dim move As recGameMove
        'There is no default move
        move.moveType = -1

        'Reveal the good and bad player pieces
        RevealPlayerPieces(gameDetails)

        'Determine if it is unsafe to take a bad player piece
        If CountPieces(True, True, False, True, gameDetails) = 1 Then
            gameDetails.AIKnowledge.canTakeBadPlayer = False
        End If

        'See if there is an invader in the wall that needs to be taken, it so take it
        move = TakeInvaderInWall(gameDetails)
        If move.moveType <> -1 Then
            GeneralModule.Log("Hard: Take Invader " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'see if there is a hole in the wall and attempt to fill it if there is one
        move = FillHoleInWall(gameDetails)
        If move.moveType <> -1 Then
            GeneralModule.Log("Hard: Fill Hole " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'Make a random move that will not interfere with the strategy
        move = WallSafeRandomMove(gameDetails)
        If move.moveType <> -1 Then
            GeneralModule.Log("Hard: Random Move " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
            Return move
        End If

        'If there is no move found that can be made, return an empty move to indicate that a random move must be generated.
        Return move
    End Function

    'Attempt to find a move to make, if this fails handle exceptions
    Public Function AIMakeMove(ByRef gameDetails As GameModule.recGameDetails) As GameModule.recGameMove
        Dim move As GameModule.recGameMove
        'Assume that no error has occurred
        Dim erroroccurred = False

        'Attempt to find a move to make
        Try
            'If the difficulty level is impossible then
            If gameDetails.difficulty = 1.0 Then
                'Use the wall strategy some of the time and the regular strategy other times
                If gameDetails.AIKnowledge.doWallStrategy Then
                    move = WallAdvancedMove(gameDetails)
                Else
                    move = AdvancedMove(gameDetails)
                End If
            Else
                'Otherwise use the standard AI always
                move = AdvancedMove(gameDetails)
            End If
        Catch ex As Exception
            GeneralModule.Log("Exception occurred in AI (Crash)", gameDetails)
            'In the case of a crash this will execute
            erroroccurred = True
        End Try

        'Test if that the move given by the AI is valid
        Dim isValidAIPiece As Boolean = False
        Dim pieceID As Integer = move.pieceIdentifier
        If pieceID >= 0 And pieceID < totalPieces Then
            'Test if the piece is an AI piece
            If Not gameDetails.pieceTable(pieceID).isPlayers Then
                isValidAIPiece = True
            End If
        End If

        'If the current move is invalid because of reasons other than a crash
        If (Not GameModule.ValidMove(move, gameDetails) Or Not isValidAIPiece) And Not erroroccurred Then
            GeneralModule.Log("Exception occurred in AI (Invalid Move)", gameDetails)
            erroroccurred = True
        End If


        'If there is no valid move that has been generated, generate a random valid move to avoid a crash
        If move.moveType = -1 Or erroroccurred Then
            move = RandomMove(gameDetails)
            GeneralModule.Log("Random Move " & move.pieceIdentifier & " (" & gameDetails.pieceTable(move.pieceIdentifier).position.x & "," & gameDetails.pieceTable(move.pieceIdentifier).position.y & ") " & move.moveType, gameDetails)
        End If

        'Return the move that will be made
        Return move
    End Function
End Module
