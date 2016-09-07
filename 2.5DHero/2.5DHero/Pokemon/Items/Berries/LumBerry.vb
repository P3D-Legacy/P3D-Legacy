Namespace Items.Berries

    Public Class LumBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2008, "Lum", 43200, "A berry to be consumed by a Pokémon. If a Pokémon holds one, it can recover from any status condition during battle.", "3.4cm", "Super Hard", 1, 2)

            Me.Spicy = 10
            Me.Dry = 10
            Me.Sweet = 10
            Me.Bitter = 10
            Me.Sour = 0

            Me.Type = Element.Types.Flying
            Me.Power = 60
        End Sub

    End Class

End Namespace