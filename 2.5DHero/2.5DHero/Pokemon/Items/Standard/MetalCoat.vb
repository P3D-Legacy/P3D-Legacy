Namespace Items.Standard

    Public Class MetalCoat

        Inherits Item

        Public Sub New()
            MyBase.New("Metal Coat", 100, ItemTypes.Standard, 143, 1, 0, New Rectangle(408, 120, 24, 24), "An item to be held by a Pokémon. It is a special metallic film that can boost the power of Steel-type moves.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace