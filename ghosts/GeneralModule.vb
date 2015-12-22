Module GeneralModule
    'Buffer size constants
    Public Const bufferSize = 10000
    Public Const smallBufferSize = 100

    Public Structure recHighScore
        Dim playerName As String
        Dim score As Integer
    End Structure

    Public Sub InitialiseWindow(ByRef window As Form)
        'Set the window icon
        window.Icon = My.Resources.Icon

        'Centre the window
        Dim windowStartX As Integer = CInt((Screen.PrimaryScreen.Bounds.Width - window.Width) / 2)
        Dim windowStartY As Integer = CInt((Screen.PrimaryScreen.Bounds.Height - window.Height) / 2)
        window.Location = New Point(windowStartX, windowStartY)
    End Sub

    'Append an integer to an array of integers
    Public Sub AppendIntegerToArray(ByRef items() As Integer, ByVal item As Integer)
        ReDim Preserve items(items.Length)
        items(items.Length - 1) = item
    End Sub

    'Append a boolean to an array of booleans
    Public Sub AppendBooleanToArray(ByRef items() As Boolean, ByVal item As Boolean)
        ReDim Preserve items(items.Length)
        items(items.Length - 1) = item
    End Sub

    'Append a string to an array of strings
    Public Sub AppendStringToArray(ByRef items() As String, ByVal item As String)
        'Fixes crash
        Dim originalLength As Integer = 0
        If Not items Is Nothing Then
            originalLength = items.Length
        End If

        ReDim Preserve items(originalLength)
        items(items.Length - 1) = item
    End Sub

    'Outputs a line to a the console (for development)
    'and records it in the game log as a comment
    Public Sub Log(ByVal line As String, ByRef gameDetails As GameModule.recGameDetails)
        'The logic of this subroutine is commented out for release versions of the program
        'Console.WriteLine(line)
        'AppendStringToArray(gameDetails.encodedMoveLog, "*" & line & "*")
    End Sub

    'Append an array of strings to a file
    Public Sub AppendArrayToFile(ByVal filename As String, ByVal data() As String)
        'Open the file for appending
        FileOpen(1, filename, OpenMode.Append)

        'Create an index
        Dim counter As Integer = 0

        'Iterate through the array and write each line to the file
        While counter < data.Length
            WriteLine(1, CStr(data(counter)))
            counter = counter + 1
        End While

        'Close the file
        FileClose(1)
    End Sub

    'Write an array of strings to a file
    Public Sub WriteArrayToFile(ByVal filename As String, ByVal data() As String)
        'Open the file for writing
        FileOpen(1, filename, OpenMode.Output)

        'Create an index
        Dim counter As Integer = 0

        'Iterate through the array and write each line to the file
        While counter < data.Length
            WriteLine(1, CStr(data(counter)))
            counter = counter + 1
        End While

        'Close the file
        FileClose(1)
    End Sub

    'Read an array of string from a file
    'If the file does not exist an empty array will be returned
    Public Function ReadArrayFromFile(ByVal filename As String) As Array
        'Create the array return and an index
        Dim arrayToReturn() As String = {}
        Dim lastInArray As Integer = 0

        'If file exists
        If System.IO.File.Exists(filename) Then

            'Open the file for reading
            FileOpen(1, filename, OpenMode.Input)

            'While not at end of file (iterate through all lines)
            While Not (EOF(1))
                'Initialize buffer
                Dim buffer As String = ""

                'Read in a line
                Input(1, buffer)

                'Recreate the array to accommodate the new line with buffer space
                'Buffer space prevents reinitialising the array for every iteration
                If lastInArray = arrayToReturn.Length Then
                    ReDim Preserve arrayToReturn(arrayToReturn.Length + bufferSize - 1)
                End If

                'add the new line to the array
                arrayToReturn(lastInArray) = buffer

                'Increment index
                lastInArray = lastInArray + 1
            End While

            'Close the file
            FileClose(1)
        End If

        'Trim off unused buffer space
        ReDim Preserve arrayToReturn(lastInArray - 1)

        'Return the array of lines
        Return arrayToReturn
    End Function

    'Return a random floating point number n such that 0<=n<1
    Public Function Random() As Single
        Random = Rnd()
    End Function

    'Return a random integer lower than possibilities and at minimum 0
    'i.e. 0, 1, ... (possibilities - 1) is the set of possible values
    Public Function RandomInteger(ByVal possibilities As Integer) As Integer
        Return CInt(Int(possibilities * Random()))
    End Function

    'Will return true at the rate 'bar' (e.g. If bar=0.1 then true will be returned around 10% of the time)
    Public Function Threshold(ByVal bar As Single) As Boolean
        Return Random() < bar
    End Function

    'Find the minimum of the two input integers and return it
    ' 'a' and 'b' are used as arbitrary number identifiers
    Public Function Minimum(ByVal a As Integer, ByVal b As Integer)
        If a < b Then
            Return a
        Else
            Return b
        End If
    End Function

    'Shuffles an array of positions
    Public Function ShuffleArrayOfPositions(ByRef arrayToShuffle() As recPosition)
        'Iterate through the array
        For i = 0 To arrayToShuffle.Length - 1
            'Find another random index of the array
            Dim randomIndex As Integer = RandomInteger(arrayToShuffle.Length)

            'Swap the element at the current index with that at the random index.
            Dim temp As recPosition = arrayToShuffle(i)
            arrayToShuffle(i) = arrayToShuffle(randomIndex)
            arrayToShuffle(randomIndex) = temp
        Next i

        'Return the shuffled array
        Return arrayToShuffle
    End Function

    Public Function ShuffleArrayOfIntegers(ByRef arrayToShuffle() As Integer)
        'Iterate through the array
        For i = 0 To arrayToShuffle.Length - 1
            'Find another random index of the array
            Dim randomIndex As Integer = RandomInteger(arrayToShuffle.Length)

            'Swap the element at the current index with that at the random index.
            Dim temp As Integer = arrayToShuffle(i)
            arrayToShuffle(i) = arrayToShuffle(randomIndex)
            arrayToShuffle(randomIndex) = temp
        Next i

        'Return the shuffled array
        Return arrayToShuffle
    End Function

    'Loads the high scores from their file
    Public Function LoadHighScores() As recHighScore()
        'This is an array of recHighScore records
        Dim highScoresArray() As recHighScore = {}

        'Read the lines of the file
        Dim linesInFile() As String = ReadArrayFromFile(frmMainMenu.highScoresFilename)

        'Resize the high scores storage array
        ReDim Preserve highScoresArray(Int(linesInFile.Length / 2) - 1)

        'Iterate through all of the high scores slots
        For i = 0 To highScoresArray.Length - 1
            'Add the data from the file to the array for the current index
            Dim currentHighScore = New recHighScore
            currentHighScore.playerName = linesInFile(2 * i)
            currentHighScore.score = Int(linesInFile(2 * i + 1))
            highScoresArray(i) = currentHighScore
        Next i

        'Return the high scores
        Return highScoresArray
    End Function

    'Sort an array of high scores using selection sort descendingly
    Public Sub SortHighScoresByScore(ByVal highScores() As recHighScore)
        'Iterate through all unsorted array starting positions
        For i = 0 To highScores.Length - 2
            'Iterate through the unsorted array
            For j = i + 1 To highScores.Length - 1
                'If the current item has a greater score than the first item
                If highScores(j).score > highScores(i).score Then
                    'Swap the item at the current index with the first item
                    Dim temp As recHighScore = highScores(j)
                    highScores(j) = highScores(i)
                    highScores(i) = temp
                End If
            Next j
        Next i
    End Sub

    'Add a new high score, sort the high scores and save them
    Public Sub SaveHighScores(ByVal newPlayerName, ByVal newScore)
        'Load the high scores
        Dim highScores() As recHighScore = LoadHighScores()

        'Add the current name and score to the high scores
        ReDim Preserve highScores(highScores.Length)
        highScores(highScores.Length - 1).playerName = newPlayerName
        highScores(highScores.Length - 1).score = newScore

        'Sort the high scores
        SortHighScoresByScore(highScores)

        'Ensure that totalHighScores is lower than or equal to 16
        Dim totalHighScores As Integer = highScores.Length
        If totalHighScores > 16 Then
            totalHighScores = 16
        End If

        'Declare the array to write to the high scores file
        Dim linesInFile(totalHighScores * 2 - 1) As String

        'Iterate through the high scores data for the number of elements specified
        For i = 0 To totalHighScores - 1
            'Add the current high score to the array of lines
            linesInFile(2 * i) = highScores(i).playerName
            linesInFile(2 * i + 1) = CStr(highScores(i).score)
        Next i

        'Save the high scores new file
        WriteArrayToFile(frmMainMenu.highScoresFilename, linesInFile)
    End Sub

    'This subroutine opens a file as a process, this is essentially
    'the same as opening the file in windows explorer,
    'Non executables will be opened with their default programs.
    Public Sub OpenFileWithDefaultProgram(ByVal filename As String)
        'Avoid a potential crash
        Try
            'Declare variables to start a process
            Dim process As New System.Diagnostics.Process
            Dim startInfo As New System.Diagnostics.ProcessStartInfo(filename)

            'Use the operating system to start the process
            startInfo.UseShellExecute = True

            'Allow the process to appear normally
            startInfo.WindowStyle = ProcessWindowStyle.Normal

            'Start the process
            process.StartInfo = startInfo
            process.Start()
        Catch ex As Exception
            'Explain to the user why the file was not opened

            'Define a quote character to insert it into the text
            Dim quote As String = Chr(34)
            MsgBox("Error: The file " & quote & filename & quote & " could not be opened.")
        End Try

    End Sub
End Module
