Namespace Items.Standard

    <Item(83, "Prism Scale")>
    Public Class PrismScale

        Inherits Item

        Public Sub New()
            MyBase.New("Prism Scale", 500, ItemTypes.Standard, 83, 1, 1, New Rectangle(72, 168, 24, 24), "A mysterious scale that causes a certain Pokémon to evolve. It shines in rainbow colors.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace
