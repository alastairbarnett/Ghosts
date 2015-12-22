'For directory operations
Imports System.IO

Public Class frmMainMenu
    'Define filenames
    'Define the game data directory
    Public gameDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Ghosts Game\"
    Public Const highScoresBaseFilename = "HighScores.txt"
    Public Const pastGamesBaseFilename = "PastGames.txt"
    Public highScoresFilename = gameDataDirectory & highScoresBaseFilename
    Public pastGamesFilename = gameDataDirectory & pastGamesBaseFilename
    Public tutorialFilename = "Tutorial.ppsx"
    Public helpFileFilename = "GhostsHelpFile.chm"


    'Declare the default name
    Public playerName = "Anonymous"

    'Exit the program
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        End
    End Sub

    'Display the help dialog
    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        frmHelp.ShowDialog()
    End Sub

    'Display the high scores
    Private Sub btnHighScores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHighScores.Click
        frmHighScores.ShowDialog()
    End Sub

    'Start a new game
    Private Sub btnNewGame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewGame.Click
        'If there is no high scores file then assume that the player has never played before
        'and show them the help dialog
        If Not My.Computer.FileSystem.FileExists(highScoresFilename) And Not My.Computer.FileSystem.FileExists(pastGamesFilename) Then
            'Show help
            frmHelp.DisplayGameRules()


            'Create the game data folder if it does not already exist
            If Not Directory.Exists(gameDataDirectory) Then
                Directory.CreateDirectory(gameDataDirectory)
            End If

            'Create a high scores file and a past games file so help is not re-displayed
            'FileOpen is used because using File.Create causes an error in some cases
            FileOpen(1, highScoresFilename, OpenMode.Output)
            FileClose(1)

            FileOpen(1, pastGamesFilename, OpenMode.Output)
            FileClose(1)

        End If
        'Allow the user to select a difficulty
        frmDifficultySelection.ShowDialog()

        'If the player did not cancel the game
        If frmDifficultySelection.playClicked Then
            'Start a new game
            frmPlayGhosts.ShowDialog()
        End If
    End Sub

    'Display a menu to browse previously played games
    Private Sub btnBrowsePlayedGames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowsePlayedGames.Click
        frmSelectReplay.ShowDialog()
    End Sub

    'Define the position and size of the banner
    Const bannerX = 0
    Const bannerY = 0
    Const bannerWidth = 550
    Const bannerHeight = 110

    'Draw a banner regardless of the background colour
    Private Sub frmMainMenu_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        'Create a bitmap
        Dim backBuffer = New Bitmap(Me.Width, Me.Height)

        'Create a graphics object to draw on
        Dim bufferGraphicsObject As Graphics = Graphics.FromImage(backBuffer)

        'Clear the graphics object
        bufferGraphicsObject.Clear(SystemColors.Control)

        'Draw the banner
        bufferGraphicsObject.DrawImage(My.Resources.MenuBanner, bannerX, bannerY, bannerWidth, bannerHeight)

        'Draw the image to the screen
        Me.CreateGraphics().DrawImage(backBuffer, 0, 0)
    End Sub

    Private Sub frmMainMenu_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise the window
        GeneralModule.InitialiseWindow(Me)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef message As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys) As Boolean
        'If F1 was pressed then call the 'F1Pressed' subroutine
        If key = Keys.F1 Then
            frmHelp.F1Pressed()
        End If
        Return True
    End Function

    Private Sub frmMainMenu_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        'If there is no game directory display optional help to the user
        If Not Directory.Exists(gameDataDirectory) Then
            'Create the game directory
            Directory.CreateDirectory(gameDataDirectory)

            'If the user wants to view the tutorial then show it
            Dim userResponse As Integer
            userResponse = MsgBox("Would you like to view an introductory tutorial explaining the basics of the program?" & vbNewLine & "This tutorial can be viewed at any time by navigating to Help>Introduction.", MsgBoxStyle.YesNo)

            If userResponse = MsgBoxResult.Yes Then
                'Open the tutorial
                GeneralModule.OpenFileWithDefaultProgram(tutorialFilename)
            End If
        End If
    End Sub
End Class
