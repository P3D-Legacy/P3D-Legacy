Namespace Items.Apricorns

    Public MustInherit Class Apricorn

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100

    End Class

End Namespace
