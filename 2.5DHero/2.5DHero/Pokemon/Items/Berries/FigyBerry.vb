Namespace Items.Berries

    Public Class FigyBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2010, "Figy", 18000, "If held by a Pokémon, it restores the user's HP in a pinch, but it will cause confusion if the user hates the taste.", "10.0cm", "Soft", 2, 3)

            Me.Spicy = 15
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Bug
            Me.Power = 60
        End Sub

    End Class

End Namespace