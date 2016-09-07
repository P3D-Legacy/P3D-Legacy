Namespace Items.Berries

    Public Class IapapaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2014, "Iapapa", 21600, "If held by a Pokémon, it restores the user's HP in a pinch, but it will cause confusion if the user hates the taste.", "22.3cm", "Soft", 2, 3)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 15

            Me.Type = Element.Types.Dark
            Me.Power = 60
        End Sub

    End Class

End Namespace