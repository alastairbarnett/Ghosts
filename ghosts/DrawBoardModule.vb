Module DrawBoardModule
    'Declare screen constants

    'Board size and position constants
    Public Const cellWidth = 60
    Public Const cellHeight = cellWidth
    Public Const boardStartX = 60
    Public Const boardStartY = 150

    'Dead piece constants
    Const deadSpace = 4

    'Define the start of the dead piece containers
    Const takenStartX = 40
    Const takenStartY = 0 + deadSpace
    Const lostStartX = 40
    Const lostStartY = 60 + deadSpace * 4

    'Define the size of the dead pieces
    Const deadWidth = 60
    Const deadHeight = deadWidth

    'Declare a buffer bitmap variable
    Dim backBuffer As Bitmap

    'Draw a rectangle at a given position with a given size.
    'The corners of the rectangle are rounded and a specified colour is used to draw the rectangle.
    Private Sub DrawRoundedRectangle(ByRef graphicsObject As Graphics, ByVal drawingPen As Pen,
                                     ByVal startX As Integer, ByVal startY As Integer,
                                     ByVal rectangleWidth As Integer, ByVal rectangleHeight As Integer,
                                     ByVal arcRadius As Integer)

        'Define a right angle
        Const rightAngle As Integer = 90

        'Declare the base rectangle
        Dim baseRectangle As New Rectangle(startX, startY, rectangleWidth, rectangleHeight)

        'Declare a temporary rectangle for drawing corner arcs
        Dim arcRectangle As New Rectangle(baseRectangle.Location, New Size(2 * arcRadius, 2 * arcRadius))

        ' +1 -1 in the lines is to fix a gap error

        'Draw top line
        graphicsObject.DrawLine(drawingPen,
                                baseRectangle.Left + arcRadius - 1, baseRectangle.Top,
                                baseRectangle.Right - arcRadius + 1, baseRectangle.Top)
        'Draw bottom line
        graphicsObject.DrawLine(drawingPen,
                                baseRectangle.Left + arcRadius - 1, baseRectangle.Bottom,
                                baseRectangle.Right - arcRadius + 1, baseRectangle.Bottom)

        'Draw right line
        graphicsObject.DrawLine(drawingPen,
                                baseRectangle.Right, baseRectangle.Top + arcRadius - 1,
                                baseRectangle.Right, baseRectangle.Bottom - arcRadius + 1)

        'Draw left line
        graphicsObject.DrawLine(drawingPen,
                                baseRectangle.Left, baseRectangle.Top + arcRadius - 1,
                                baseRectangle.Left, baseRectangle.Bottom - arcRadius + 1)

        'Draw bottom right arc
        arcRectangle.Y = baseRectangle.Bottom - arcRadius * 2
        arcRectangle.X = baseRectangle.Right - arcRadius * 2
        graphicsObject.DrawArc(drawingPen, arcRectangle, 0 * rightAngle, rightAngle)

        'Draw bottom left arc
        arcRectangle.Y = baseRectangle.Bottom - arcRadius * 2
        arcRectangle.X = baseRectangle.Left
        graphicsObject.DrawArc(drawingPen, arcRectangle, 1 * rightAngle, rightAngle)

        'Draw top left arc
        arcRectangle.Y = baseRectangle.Top
        arcRectangle.X = baseRectangle.Left
        graphicsObject.DrawArc(drawingPen, arcRectangle, 2 * rightAngle, rightAngle)

        'Draw top right arc
        arcRectangle.Y = baseRectangle.Top
        arcRectangle.X = baseRectangle.Right - arcRadius * 2
        graphicsObject.DrawArc(drawingPen, arcRectangle, 3 * rightAngle, rightAngle)
    End Sub

    'Draw a blank board
    Public Sub DrawBlankBoard(ByRef graphicsObject As Graphics, gameDetails As recGameDetails)
        'Draw each cell as a blank square
        For i = 0 To GameModule.boardWidth - 1
            For j = 0 To GameModule.boardHeight - 1
                graphicsObject.DrawImage(My.Resources.SingleBlankStep, cellWidth * i + boardStartX, cellHeight * j + boardStartY, cellWidth, cellHeight)
            Next j
        Next i
        'Draw orange squares on each corner cell
        graphicsObject.DrawImage(My.Resources.OrangeSquare, cellWidth * 0 + boardStartX, cellHeight * 0 + boardStartY, cellWidth, cellHeight)
        graphicsObject.DrawImage(My.Resources.OrangeSquare, cellWidth * (boardWidth - 1) + boardStartX, cellHeight * 0 + boardStartY, cellWidth, cellHeight)
        graphicsObject.DrawImage(My.Resources.OrangeSquare, cellWidth * 0 + boardStartX, cellHeight * (boardHeight - 1) + boardStartY, cellWidth, cellHeight)
        graphicsObject.DrawImage(My.Resources.OrangeSquare, cellWidth * (boardWidth - 1) + boardStartX, cellHeight * (boardHeight - 1) + boardStartY, cellWidth, cellHeight)
    End Sub

    'Color cells to indicate the current selection
    Public Sub DrawSelection(ByRef graphicsObject As Graphics, ByVal gameDetails As recGameDetails)
        'Get the current selection
        Dim selectionX = frmPlayGhosts.selectionX
        Dim selectionY = frmPlayGhosts.selectionY

        'If there is a selection
        If selectionX <> -1 And selectionY <> -1 Then
            'If a piece is selected
            If gameDetails.gameBoard(selectionX, selectionY) <> -1 Then
                'If the selected piece is a player piece
                If gameDetails.pieceTable(gameDetails.gameBoard(selectionX, selectionY)).isPlayers Then
                    'Iterate through all connected squares to the selected square
                    For dx = -1 To 1
                        For dy = -1 To 1
                            'Find the coordinates of the current square
                            Dim x = selectionX + dx
                            Dim y = selectionY + dy

                            'If the current square is on the board and strictly adjacent to the selected square
                            If x >= 0 And x <= GameModule.boardWidth - 1 And y >= 0 And y <= GameModule.boardHeight - 1 And (dx = 0 Or dy = 0) And Not (x = selectionX And y = selectionY) Then
                                'If the current square is occupied
                                If gameDetails.gameBoard(x, y) <> -1 Then
                                    'If the piece in the square is the players
                                    If Not gameDetails.pieceTable(gameDetails.gameBoard(x, y)).isPlayers Then
                                        'Draw a green square at the position (can be moved to)
                                        graphicsObject.DrawImage(My.Resources.GreenSquare, cellWidth * x + boardStartX, cellHeight * y + boardStartY, cellWidth, cellHeight)
                                    Else
                                        'Draw a red square at the position (cannot be moved to)
                                        graphicsObject.DrawImage(My.Resources.RedSquare, cellWidth * x + boardStartX, cellHeight * y + boardStartY, cellWidth, cellHeight)
                                    End If
                                Else
                                    'Draw a green square at the position (can be moved to)
                                    graphicsObject.DrawImage(My.Resources.GreenSquare, cellWidth * x + boardStartX, cellHeight * y + boardStartY, cellWidth, cellHeight)
                                End If
                            End If
                        Next dy
                    Next dx
                    'Draw a black square at the selected position (currently selected)
                    graphicsObject.DrawImage(My.Resources.BlackSquare, cellWidth * (selectionX) + boardStartX, cellHeight * (selectionY) + boardStartY, cellWidth, cellHeight)
                End If
            End If
        End If
    End Sub

    'Draw the pieces on the board
    Public Sub DrawPieces(ByRef graphicsObject As Graphics, gameDetails As recGameDetails, revealAI As Boolean)
        'Iterate through each board position
        For i = 0 To GameModule.boardWidth - 1
            For j = 0 To GameModule.boardHeight - 1
                'If the position is occupied
                If gameDetails.gameBoard(i, j) <> -1 Then
                    Dim mainImage As Image

                    'If the piece on the square is the player's
                    If gameDetails.pieceTable(gameDetails.gameBoard(i, j)).isPlayers Then
                        'Select a green ghost for good pieces and a red ghost for bad pieces
                        If gameDetails.pieceTable(gameDetails.gameBoard(i, j)).isGood Then
                            mainImage = My.Resources._60by60GreenGhost
                        Else
                            mainImage = My.Resources._60by60RedGhost
                        End If
                    Else
                        'If the AI is being revealed (e.g. in a replay)
                        If revealAI Then
                            'Select a pale green ghost for good pieces and a pale red ghost for bad pieces
                            If gameDetails.pieceTable(gameDetails.gameBoard(i, j)).isGood Then
                                mainImage = My.Resources._60by60PaleGreenGhost
                            Else
                                mainImage = My.Resources._60by60PaleRedGhost
                            End If
                        Else
                            'Select a white ghost
                            mainImage = My.Resources._60by60Ghost
                        End If
                    End If
                    'Actually draw the image on the current square
                    graphicsObject.DrawImage(mainImage, cellWidth * i + boardStartX, cellHeight * j + boardStartY, cellWidth, cellHeight)
                End If
            Next j
        Next i
    End Sub

    'Draw an array of the dead pieces
    Public Sub DrawTakenPieces(ByRef graphicsObject As Graphics, ByVal gameDetails As recGameDetails)
        'Iterate through the dead AI pieces
        Dim drawingPen As New Pen(Color.Black, 4)

        For i = 0 To gameDetails.AIDead.Length - 1
            Dim mainImage As Image
            'If the piece is good select a green ghost otherwise select a red ghost
            If gameDetails.pieceTable(gameDetails.AIDead(i)).isGood Then
                mainImage = My.Resources._60by60PaleGreenGhost
            Else
                mainImage = My.Resources._60by60PaleRedGhost
            End If

            'Draw the selected image
            graphicsObject.DrawImage(mainImage, deadWidth * i + takenStartX, takenStartY, deadWidth, deadHeight)
        Next i
        'Draw taken container
        DrawRoundedRectangle(graphicsObject, drawingPen, CInt(deadSpace / 2), takenStartY - deadSpace, deadWidth * (totalPieces / 2), deadHeight + deadSpace, 10)

        'Iterate through the dead player pieces
        For i = 0 To gameDetails.playerDead.Length - 1
            Dim mainImage As Image
            'If the piece is good select a green ghost otherwise select a red ghost
            If gameDetails.pieceTable(gameDetails.playerDead(i)).isGood Then
                mainImage = My.Resources._60by60GreenGhost
            Else
                mainImage = My.Resources._60by60RedGhost
            End If

            'Draw the selected image
            graphicsObject.DrawImage(mainImage, deadWidth * i + lostStartX, lostStartY, deadWidth, deadHeight)
        Next i
        'Draw lost container
        DrawRoundedRectangle(graphicsObject, drawingPen, CInt(deadSpace / 2), lostStartY - deadSpace, deadWidth * (totalPieces / 2), deadHeight + deadSpace, 10)
    End Sub

    'Draws arrows to indicate the direction of the most recent move
    Public Sub DrawMoveIndicator(ByRef graphicsObject As Graphics, gameDetails As recGameDetails)
        'If this is not the first move
        If gameDetails.previousMoveType <> -1 Then
            Dim mainImage As Image = Nothing

            'Select an image based on the move
            Select Case gameDetails.previousMoveType
                Case 0
                    'Up
                    mainImage = My.Resources._60by60RedArrowUp
                Case 1
                    'Right
                    mainImage = My.Resources._60by60RedArrowRight
                Case 2
                    'Down
                    mainImage = My.Resources._60by60RedArrowDown
                Case 3
                    'Left
                    mainImage = My.Resources._60by60RedArrowLeft
            End Select

            'Determine the position of the move
            Dim x As Integer = gameDetails.previousPosition.x
            Dim y As Integer = gameDetails.previousPosition.y

            'Draw the move indicator
            graphicsObject.DrawImage(mainImage, cellWidth * x + boardStartX, cellHeight * y + boardStartY, cellWidth, cellHeight)
        End If
    End Sub

    Public Sub DrawBoard(ByRef graphicsObject As Graphics, gameDetails As recGameDetails, width As Integer, height As Integer, revealAI As Boolean)
        'Enclose the code in error catching statements
        Try
            'Create a bitmap to use
            If backBuffer Is Nothing Then
                backBuffer = New Bitmap(width, height)
            End If

            'Create a graphics object to use as a buffer (prevents flicker)
            Dim bufferGraphicsObject As Graphics = Graphics.FromImage(backBuffer)

            'Ensure that anti-aliasing is enabled
            bufferGraphicsObject.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            'Clear the screen
            bufferGraphicsObject.Clear(SystemColors.Control)

            'Draw the blank board
            DrawBlankBoard(bufferGraphicsObject, gameDetails)

            'Indicate the previous move that has been made
            DrawMoveIndicator(bufferGraphicsObject, gameDetails)

            'If AI are revealed the game is not in play so the users selections do not need to be drawn
            If Not revealAI Then
                'Draw the users selections
                DrawSelection(bufferGraphicsObject, gameDetails)
            End If

            'Draw a boarder for the board
            DrawRoundedRectangle(bufferGraphicsObject, New Pen(Color.Black, 10), boardStartX, boardStartY, boardWidth * cellWidth, boardHeight * cellHeight, 10)

            'Draw the pieces on the board
            DrawPieces(bufferGraphicsObject, gameDetails, revealAI)

            'Draw the pieces that have been taken
            DrawTakenPieces(bufferGraphicsObject, gameDetails)

            'Draw the buffer to the screen
            graphicsObject.DrawImage(backBuffer, 0, 0)
        Catch ex As Exception
            'Drawing the screen failed
            GeneralModule.Log("Failed to draw screen", gameDetails)
        End Try
    End Sub

End Module
