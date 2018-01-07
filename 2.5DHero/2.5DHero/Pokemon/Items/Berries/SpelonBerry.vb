Namespace Items.Berries

    <Item(2030, "Spelon")>
    Public Class SpelonBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(64800, "Pokéblock ingredient." & Environment.NewLine & "Plant in loamy soil to grow Spelon.", "13.2cm", "Soft", 1, 2)

            Me.Spicy = 30
            Me.Dry = 10
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Dark
            Me.Power = 70
        End Sub

    End Class

End Namespace
