Public Class TransitionScreen

    Inherits Screen

    Public OldScreen As Screen
    Public NewScreen As Screen

    Dim alpha As Integer = 0
    Dim reduce As Boolean = False
    Dim doSub As DoStuff
    Dim Color As Color
    Dim noStuff As Boolean = False
    Dim Speed As Integer

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal Color As Color, ByVal noBlackIn As Boolean)
        Me.New(OldScreen, NewScreen, Color, noBlackIn, 10)
    End Sub

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal Color As Color, ByVal noBlackIn As Boolean, ByVal Speed As Integer)
        Me.Identification = Identifications.TransitionScreen

        Me.OldScreen = OldScreen
        Me.NewScreen = NewScreen
        Me.CanChat = False

        Me.Color = Color
        Me.noStuff = True
        Me.Speed = Speed

        If noBlackIn = True Then
            alpha = 255
            reduce = True
            Me.CanBePaused = NewScreen.CanBePaused
        Else
            Me.CanBePaused = OldScreen.CanBePaused
        End If
    End Sub

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal Color As Color, ByVal noBlackIn As Boolean, ByVal doSub As DoStuff)
        Me.New(OldScreen, NewScreen, Color, noBlackIn, doSub, 10)
    End Sub

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal Color As Color, ByVal noBlackIn As Boolean, ByVal doSub As DoStuff, ByVal Speed As Integer)
        Me.OldScreen = OldScreen
        Me.NewScreen = NewScreen
        Me.CanChat = False

        Me.Color = Color
        Me.doSub = doSub
        Me.Speed = Speed

        If noBlackIn = True Then
            doSub()
            alpha = 255
            reduce = True
            Me.CanBePaused = NewScreen.CanBePaused
        Else
            Me.CanBePaused = OldScreen.CanBePaused
        End If
    End Sub

    Public Overrides  Sub Draw()
        If reduce = False Then
            OldScreen.Draw()
        Else
            NewScreen.Draw()
        End If

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height), New Color(Color.R, Color.G, Color.B, alpha))
    End Sub

    Public Overrides Sub Update()
        If reduce = False Then
            alpha += Speed
            If OldScreen.UpdateFadeOut = True Then
                OldScreen.Update()
            End If
            If alpha >= 255 Then
                Me.CanBePaused = NewScreen.CanBePaused
                reduce = True
                If noStuff = False Then
                    doSub()
                End If
            End If
        Else
            alpha -= Speed
            If NewScreen.UpdateFadeIn = True Then
                NewScreen.Update()
            End If
            If alpha = 0 Then
                ChangeScreen()
            End If
        End If
    End Sub

    Private Sub ChangeScreen()
        Core.SetScreen(NewScreen)
    End Sub

    Public Delegate Sub DoStuff()

End Class