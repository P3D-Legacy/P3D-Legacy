Namespace Items.Berries

    Public Class AguavBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2013, "Aguav", 21600, "If held by a Pokémon, it restores the user's HP in a pinch, but it will cause confusion if the user hates the taste.", "6.4cm", "Super Hard", 2, 3)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 10
            Me.Sour = 0

            Me.Type = Element.Types.Dragon
            Me.Power = 60
        End Sub

    End Class

End Namespace