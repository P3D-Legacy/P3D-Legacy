Namespace Items

    ''' <summary>
    ''' The base class for all Mega Stone items.
    ''' </summary>
    Public MustInherit Class MegaStone

        Inherits Item

        Public ReadOnly Property MegaPokemonNumber As Integer

        Public Overrides ReadOnly Property Description As String
        Public Overrides ReadOnly Property CanBeTossed As Boolean = False
        Public Overrides ReadOnly Property CanBeTraded As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public Sub New(ByVal MegaPokemonName As String, ByVal _megaPokemonNumber As Integer)
            Description = "One variety of the mysterious Mega Stones. Have " & MegaPokemonName & " hold it, and this stone will enable it to Mega Evolve during battle."
            _textureSource = "Items\MegaStones"

            MegaPokemonNumber = _megaPokemonNumber
        End Sub

    End Class

End Namespace
