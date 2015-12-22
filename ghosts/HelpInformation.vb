Public Class frmHelpInformation
    'Declare global variables
    Public slides() As Image
    Public titles() As String
    Public index As Integer
    Public prePlayTutorial As Boolean

    'Declare the constants for image display
    Const infoHeight = 250
    Const infoWidth = 600
    Const infoStartX = 100
    Const infoStartY = 100
    Const buttonSpace = 50

    'Refresh the screen
    Private Sub RefreshScreen()
        'Create a bitmap
        Dim backBuffer = New Bitmap(Me.Width, Me.Height)

        'Create a graphics object to draw on
        Dim bufferGraphicsObject As Graphics = Graphics.FromImage(backBuffer)

        'Clear the graphics object
        bufferGraphicsObject.Clear(SystemColors.Control)

        'Declare the dimensions of the new image
        Dim newWidth As Integer = infoWidth
        Dim newHeight As Integer = infoHeight

        'Centre the frame of the image
        Dim newX As Integer = CInt((Me.Width - infoWidth) / 2)
        'Account for the array of buttons at the bottom of the screen
        Dim newY As Integer = CInt(((Me.Height - buttonSpace) - infoHeight) / 2)

        'bufferGraphicsObject.DrawRectangle(New Pen(Color.Black, 3), newX, newY, infoWidth, infoHeight)

        'Adjust the dimensions of the image to fit the frame and position of the image
        If slides(index).Width > slides(index).Height Then
            'The width will fit the frame, the height will be adjusted
            newHeight = CInt(CSng(infoWidth) * CSng(slides(index).Height) / CSng(slides(index).Width))
            newY = newY + CInt((infoHeight - newHeight) / 2)
        Else
            'The height will fit the frame, the width will be adjusted
            newWidth = CInt(CSng(infoHeight) * CSng(slides(index).Width) / CSng(slides(index).Height))
            newX = newX + CInt((infoWidth - newWidth) / 2)
        End If

        'Draw the current image
        bufferGraphicsObject.DrawImage(slides(index), newX, newY, newWidth, newHeight)

        'Draw the image to the screen
        Me.CreateGraphics().DrawImage(backBuffer, 0, 0)
    End Sub

    'Change the image on the screen or initialise the screen
    Private Sub AdjustCounter(ByVal change As Integer)
        'Apply the change to the current image
        index = index + change

        'Ensure that the 'Previous' button is only enabled if the index is not 0
        If index = 0 Then
            btnPrevious.Text = "Back"
        Else
            btnPrevious.Text = "Previous"
        End If

        'Ensure that the 'Next' button is only enabled if a higher index exists
        If index = slides.Length - 1 Then
            If prePlayTutorial Then
                btnNext.Text = "Play"
            Else
                btnNext.Text = "Exit"
            End If
        Else
            btnNext.Text = "Next"
        End If

        If index >= 0 And index < slides.Length Then
            'Set the appropriate title
            Me.Text = "Help: " & titles(index) & " (" & CStr(index + 1) & "/" & CStr(slides.Length) & ")"

            'Refresh the screen
            RefreshScreen()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub HelpGhostsRules_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise the window
        InitialiseWindow(Me)

        'Initialise the slideshow
        AdjustCounter(0)
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        'Go to the next image in the slideshow
        AdjustCounter(1)
    End Sub

    Private Sub btnPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
        'Go to the previous image in the slideshow
        AdjustCounter(-1)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Close the window
        Me.Close()
    End Sub

    Private Sub frmHelpInformation_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        'Refresh the screen
        RefreshScreen()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef message As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys) As Boolean
        'If F1 was pressed then call the 'F1Pressed' subroutine
        If key = Keys.F1 Then
            frmHelp.F1Pressed()
        End If
        Return True
    End Function
End Class