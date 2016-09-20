Namespace Items.KeyItems

    Public MustInherit Class KeyItem

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.KeyItems
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeTraded As Boolean = False
        Public Overrides ReadOnly Property CanBeTossed As Boolean = False
        Public Overrides ReadOnly Property CanBeHold As Boolean = False
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9800

    End Class

End Namespace
