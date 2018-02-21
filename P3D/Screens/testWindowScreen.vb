Public Class testWindowScreen

    Inherits WindowScreen

    Dim t As KeyboardInput.Textbox

    Public Sub New(ByVal currentScreen As Screen)
        MyBase.New(currentScreen, Identifications.BerryScreen, "TestScreen")

        t = New KeyboardInput.Textbox(FontManager.TextFont)
    End Sub

    Public Overrides Sub Update()
        MyBase.Update()

        Me.t.Update()
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        MyBase.Draw()

        If FadedIn = True Then
            Me.t.Draw()
        End If
    End Sub

End Class