Namespace Items.Berries

    Public Class EnigmaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2059, "Enigma", 86400, "A Berry to be consumed by Pokémon. If a Pokémon holds one, being hit by a supereffective attack will restore its HP.", "15.5cm", "Hard", 1, 2)

            Me.Spicy = 40
            Me.Dry = 10
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Bug
            Me.Power = 80
        End Sub

    End Class

End Namespace