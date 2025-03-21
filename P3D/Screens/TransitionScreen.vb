﻿Public Class TransitionScreen

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

    Public Overrides Sub Draw()
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
                Dim screens() As Screen.Identifications = {Screen.Identifications.PokegearScreen, Screen.Identifications.OverworldScreen}
                If screens.Contains(NewScreen.Identification) Then
                    'Play music depending on the player state in the level (surfing and riding):
                    If Screen.Level.Surfing = True Then
                        MusicManager.Play("surf", True) 'Play "surf" when player is surfing.
                    Else
                        If Screen.Level.Riding = True Then
                            MusicManager.Play("ride", True) 'Play "ride" when player is riding.
                        Else
                            MusicManager.Play(Level.MusicLoop, True, 0.02F) 'Play default MusicLoop.
                        End If
                    End If
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
        If OldScreen.Identification = Identifications.BattleScreen And CurrentScreen.Identification = Identifications.OverworldScreen Then
            If Core.Player.UsedItemsToCheckScriptDelayFor.Count > 0 Then
                For Each itemEntry As String In Core.Player.UsedItemsToCheckScriptDelayFor
                    Core.Player.CheckItemCountScriptDelay(itemEntry)
                Next
                Core.Player.UsedItemsToCheckScriptDelayFor.Clear()
            End If
        End If
    End Sub

    Public Delegate Sub DoStuff()

End Class