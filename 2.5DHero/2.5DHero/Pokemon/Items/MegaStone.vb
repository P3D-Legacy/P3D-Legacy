Namespace Items

    ''' <summary>
    ''' The base class for all Mega Stone items.
    ''' </summary>
    Public Class MegaStone

        Inherits Item

        Private _megaPokemonNumber As Integer = 0

        Public Sub New(ByVal Name As String, ByVal ID As Integer, ByVal TextureRectangle As Rectangle, ByVal MegaPokemonName As String, ByVal MegaPokemonNumber As Integer)
            MyBase.New(Name, 100, ItemTypes.Standard, ID, 1.0F, 0, TextureRectangle, "One variety of the mysterious Mega Stones. Have " & MegaPokemonName & " hold it, and this stone will enable it to Mega Evolve during battle.")

            Me._texture = TextureManager.GetTexture("Items\MegaStones", TextureRectangle, "")

            Me._canBeHold = True
            Me._canBeTossed = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._isMegaStone = True
            Me._megaPokemonNumber = MegaPokemonNumber
        End Sub

        Public ReadOnly Property MegaPokemonNumber() As Integer
            Get
                Return Me._megaPokemonNumber
            End Get
        End Property

    End Class

End Namespace
