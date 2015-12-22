Public Class frmSelectReplay

    'Past game structure
    Public Structure recPastGame
        Dim playerName As String
        Dim playerWon As Boolean
        Dim difficulty As Single
        Dim score As Integer
        Dim encodedMoveLog() As String
    End Structure

    Private pastGamesArray() As recPastGame
    'Load the past games from the past games file
    Public Function LoadPastGames() As recPastGame()
        Dim pastGamesArrayToReturn() As recPastGame = {}

        'Read the lines from the past games file
        Dim linesInFile() As String = ReadArrayFromFile(frmMainMenu.pastGamesFilename)

        'If the past games file is not empty
        If linesInFile.Length <> 0 Then
            'Declare a comment free array
            Dim noComments() As String = {}
            Dim i As Integer
            Dim validLines As Integer = 0
            'Iterate through the read in lines
            For i = 0 To linesInFile.Length - 1
                'Allocate more buffer space if the array is full
                If validLines = noComments.Length Then
                    ReDim Preserve noComments(noComments.Length + GeneralModule.bufferSize - 1)
                End If

                'If the line is a comment add it to the array
                If linesInFile(i).Chars(0) <> "*" Then
                    noComments(validLines) = linesInFile(i)
                    validLines = validLines + 1
                End If
            Next

            'Trim off unused buffer space
            ReDim Preserve noComments(validLines - 1)

            'Replace the original array with the comment free array
            linesInFile = noComments

            'Iterate through the past games file

            Dim gameNumber As Integer = 0

            'Skip the first "[BEGIN]" by initialising at 1 instead of 0
            i = 1
            While i < linesInFile.Length
                'Allocate storage space for the current past game
                Dim currentPastGame As recPastGame = New recPastGame

                'The iteration variable "i" is incremented after each line access
                'to move to the next line

                'Get the player's name
                currentPastGame.playerName = linesInFile(i)
                i = i + 1

                'Get the player's score
                currentPastGame.score = CInt(linesInFile(i))
                i = i + 1

                'Get the difficulty of the game
                currentPastGame.difficulty = CSng(linesInFile(i))
                i = i + 1

                'Get the winner of the game
                currentPastGame.playerWon = (linesInFile(i) = "True")
                i = i + 1

                'Initialise the move list for the game
                currentPastGame.encodedMoveLog = {}

                'Initialise a line counter
                Dim lineNumber As Integer = 0

                Dim newRecordFound As Boolean = False

                'While the current line exists
                While i < linesInFile.Length And Not newRecordFound
                    'If the current line is the start of a new record
                    If linesInFile(i) = "[BEGIN]" Then
                        'Use a flag to exit the current loop
                        newRecordFound = True
                    Else
                        'Add the current line to the move list for the game
                        If lineNumber >= currentPastGame.encodedMoveLog.Length Then
                            ReDim Preserve currentPastGame.encodedMoveLog(currentPastGame.encodedMoveLog.Length + GeneralModule.smallBufferSize - 1)
                        End If
                        currentPastGame.encodedMoveLog(lineNumber) = linesInFile(i)
                        lineNumber = lineNumber + 1
                    End If
                    'Go to the next line
                    i = i + 1
                End While
                ReDim Preserve currentPastGame.encodedMoveLog(lineNumber - 1)
                'Add the current past game to the past games array

                If gameNumber >= pastGamesArrayToReturn.Length Then
                    ReDim Preserve pastGamesArrayToReturn(pastGamesArrayToReturn.Length + GeneralModule.smallBufferSize - 1)
                End If

                pastGamesArrayToReturn(gameNumber) = currentPastGame
                gameNumber = gameNumber + 1
            End While
            'Trim off the unused buffer space
            ReDim Preserve pastGamesArrayToReturn(gameNumber - 1)
        End If

        'Return the past games array
        Return pastGamesArrayToReturn
    End Function


    Private Sub frmSelectReplay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Initialise the window
        GeneralModule.InitialiseWindow(Me)

        'Select the "Select" button by default
        btnSelect.Select()

        'Create a new data table and add the columns
        Dim gameTable = New DataTable
        gameTable.Columns.Add("Name")
        gameTable.Columns.Add("Winner")
        gameTable.Columns.Add("Difficulty")
        gameTable.Columns.Add("Score")

        'Iterate through past games and add them to the table in the format (name, winner, difficulty, score)
        pastGamesArray = LoadPastGames()

        'Start with the most recent game (i.e. iterate descendingly)
        Dim i = pastGamesArray.Length - 1

        'Iterate through the past games
        While i >= 0
            'Convert the winner to a string
            Dim winner As String = "Computer"
            If pastGamesArray(i).playerWon Then
                winner = "Player"
            End If

            'Convert the difficulty to a string
            Dim difficulty As String = "Normal"
            If pastGamesArray(i).difficulty = 0.0 Then
                difficulty = "Easy"
            End If
            If pastGamesArray(i).difficulty = 1.0 Then
                difficulty = "Impossible"
            End If

            'Add the information to the table
            gameTable.Rows.Add(pastGamesArray(i).playerName, winner, difficulty, CStr(pastGamesArray(i).score))
            i = i - 1
        End While

        'Display the created table
        grdPastGamesTable.DataSource = gameTable

        'Disable sorting for the columns of the table
        For counter = 0 To 3
            grdPastGamesTable.Columns.Item(counter).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    'Replay the currently selected past game
    Private Sub PastGameChosen()
        'If the table of games is not empty
        If pastGamesArray.Length <> 0 Then
            Dim indexOnTable = grdPastGamesTable.CurrentRow.Index
            'Convert the index on the table to the index of the past games array
            Dim gameIndex = (pastGamesArray.Length - 1) - indexOnTable

            'Replay the selected game
            frmReplayGame.pastGame = pastGamesArray(gameIndex)
            frmReplayGame.ShowDialog()
        Else
            'Inform the user why they could not make a selection
            MsgBox("There are no games to select.")
        End If
    End Sub

    'The select button was clicked
    Private Sub btnSelect_Click(sender As System.Object, e As System.EventArgs) Handles btnSelect.Click
        'Handle a game selection
        PastGameChosen()
    End Sub

    'A row in the table was double clicked
    'This will also run upon the double click of the top row
    Private Sub grdPastGamesTable_CellMouseDoubleClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles grdPastGamesTable.CellMouseDoubleClick
        'Handle a game selection
        PastGameChosen()
    End Sub

    'Exit the form
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
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