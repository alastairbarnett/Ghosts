Public Class frmHelp
    'List the slides and titles for each slide for the different tutorials.
    'These are not declared as constants because attempting to do so causes errors
    'and declaring as variables does not result in any errors.
    Dim gameRulesSlides() As Image = {
            My.Resources.Help_GameRules_ChoosingPieces_1,
            My.Resources.Help_GameRules_ChoosingPieces_2,
            My.Resources.Help_GameRules_Moving_1,
            My.Resources.Help_GameRules_TakingPieces_1,
            My.Resources.Help_GameRules_TakingPieces_2,
            My.Resources.Help_GameRules_Winning_1,
            My.Resources.Help_GameRules_AvoidingDefeat_1
    }
    Dim gameRulesTitles() As String = {
            "Game Rules - Choosing Pieces",
            "Game Rules - Choosing Pieces",
            "Game Rules - Moving",
            "Game Rules - Taking Pieces",
            "Game Rules - Taking Pieces",
            "Game Rules - Winning",
            "Game Rules - Avoiding Defeat"
    }

    Dim replaySlides() As Image = {
            My.Resources.Help_ReplayingGames_1,
            My.Resources.Help_ReplayingGames_2,
            My.Resources.Help_ReplayingGames_3,
            My.Resources.Help_ReplayingGames_4,
            My.Resources.Help_ReplayingGames_5,
            My.Resources.Help_ReplayingGames_6
    }
    Dim replayTitles() As String = {
            "Replaying Games",
            "Replaying Games",
            "Replaying Games",
            "Replaying Games",
            "Replaying Games",
            "Replaying Games"
    }

    Dim playSlides() As Image = {
            My.Resources.Help_PlayingGames_1,
            My.Resources.Help_PlayingGames_2,
            My.Resources.Help_PlayingGames_3,
            My.Resources.Help_PlayingGames_4,
            My.Resources.Help_PlayingGames_5,
            My.Resources.Help_PlayingGames_6
        }
    Dim playTitles() As String = {
            "Playing Games",
            "Playing Games",
            "Playing Games",
            "Playing Games",
            "Playing Games",
            "Playing Games"
    }

    Dim highScoresSlides() As Image = {
        My.Resources.Help_HighScores_1,
        My.Resources.Help_HighScores_2
    }
    Dim highScoresTitles() As String = {
            "High Scores",
            "High Scores"
    }

    'This is called if F1 is pressed anywhere in the program
    'This is a subroutine in case the action needs to be changed everywhere
    Public Sub F1Pressed()
        'Open the help file
        GeneralModule.OpenFileWithDefaultProgram(frmMainMenu.helpFileFilename)
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Exit the window
        Me.Close()
    End Sub

    Private Sub frmHelp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise the window
        GeneralModule.InitialiseWindow(Me)
    End Sub

    'Show a help slideshow
    Private Sub ShowHelp(ByVal slides() As Image, ByVal titles() As String, ByVal startIndex As Integer, ByVal preGameTutorial As Boolean)
        'Initialise slides
        frmHelpInformation.slides = slides

        'Initialise titles
        frmHelpInformation.titles = titles

        'Initialise the starting index
        frmHelpInformation.index = startIndex

        'Indicate if this is the pre-game tutorial
        frmHelpInformation.prePlayTutorial = preGameTutorial

        'Show the slideshow
        frmHelpInformation.ShowDialog()
    End Sub

    'Display the game rules to the user with a Yes/No box.
    'This is for displaying to the user at the start of the game.
    Public Sub DisplayGameRules()
        Dim userResponse As Integer
        userResponse = MsgBox("Would you like to view a tutorial explaining the rules of the game?" & vbNewLine & "This tutorial can be viewed at any time by navigating to Help>All Game Rules.", MsgBoxStyle.YesNo)
        If userResponse = MsgBoxResult.Yes Then
            ShowHelp(gameRulesSlides, gameRulesTitles, 0, True)
        End If
    End Sub

    'Display the game rules
    Private Sub btnGameRules_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGameRules.Click
        ShowHelp(gameRulesSlides, gameRulesTitles, 0, False)
    End Sub

    'Display help for playing the game
    Private Sub btnPlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlay.Click
        ShowHelp(playSlides, playTitles, 0, False)
    End Sub

    'Display help for viewing replays
    Private Sub btnReplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplay.Click
        ShowHelp(replaySlides, replayTitles, 0, False)
    End Sub

    'Display help for viewing high scores
    Private Sub btnHighScores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHighScores.Click
        ShowHelp(highScoresSlides, highScoresTitles, 0, False)
    End Sub

    'Display help for the 'choosing' aspect of the game
    Private Sub btnChoosing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChoosing.Click
        ShowHelp(gameRulesSlides, gameRulesTitles, 0, False)
    End Sub

    'Display help for the 'moving' aspect of the game
    Private Sub btnMoving_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoving.Click
        ShowHelp(gameRulesSlides, gameRulesTitles, 2, False)
    End Sub

    'Display help for the 'taking' aspect of the game
    Private Sub btnTaking_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTaking.Click
        ShowHelp(gameRulesSlides, gameRulesTitles, 3, False)
    End Sub

    'Display help for the 'winning' aspect of the game
    Private Sub btnWinning_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinning.Click
        ShowHelp(gameRulesSlides, gameRulesTitles, 5, False)
    End Sub

    'Display help for the 'avoiding loss' aspect of the game
    Private Sub btnAvoiding_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAvoiding.Click
        ShowHelp(gameRulesSlides, gameRulesTitles, 6, False)
    End Sub

    'Open the introduction tutorial
    Private Sub btnIntroduction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIntroduction.Click
        'Open the tutorial
        GeneralModule.OpenFileWithDefaultProgram(frmMainMenu.tutorialFilename)
    End Sub

    'Open the help file for the program
    Private Sub btnHelpFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelpFile.Click
        'Open the help file
        GeneralModule.OpenFileWithDefaultProgram(frmMainMenu.helpFileFilename)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef message As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys) As Boolean
        'If F1 was pressed then call the 'F1Pressed' subroutine
        If key = Keys.F1 Then
            F1Pressed()
        End If
        Return True
    End Function
End Class