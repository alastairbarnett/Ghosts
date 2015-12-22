Public Class frmHighScores

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'Close the high scores
        Me.Close()
    End Sub

    Private Sub frmHighScores_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise the window
        GeneralModule.InitialiseWindow(Me)

        'Select the OK button by default
        btnOK.Select()

        'Create a new data table and add the columns
        Dim scoreTable = New DataTable
        scoreTable.Columns.Add("Rank")
        scoreTable.Columns.Add("Name")
        scoreTable.Columns.Add("Score")

        'Load the high scores from the high scores file
        Dim highScoresData() As recHighScore = LoadHighScores()

        'Iterate through high scores and add them to the table in the format (rank, name, score)
        Dim counter = 0
        While counter < highScoresData.Length
            scoreTable.Rows.Add(counter + 1, highScoresData(counter).playerName, highScoresData(counter).score)
            counter = counter + 1
        End While

        'Display the constructed table on the board
        grdHighscoresTable.DataSource = scoreTable

        'Disallow sorting
        For counter = 0 To 2
            grdHighscoresTable.Columns.Item(counter).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef message As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys) As Boolean
        'If F1 was pressed then call the 'F1Pressed' subroutine
        If key = Keys.F1 Then
            frmHelp.F1Pressed()
        End If
        Return True
    End Function
End Class