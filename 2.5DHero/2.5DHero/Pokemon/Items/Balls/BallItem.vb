Namespace Items.Balls

    Public MustInherit Class BallItem

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Pokéballs
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1000

    End Class

End Namespace
