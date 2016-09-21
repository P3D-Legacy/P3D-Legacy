Namespace Items.XItems
    Public MustInherit Class XItem

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property BattleSelectPokemon As Boolean = True
        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.BattleItems

    End Class

End Namespace

