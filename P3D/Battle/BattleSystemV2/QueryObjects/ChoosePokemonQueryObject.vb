Namespace BattleSystem

    Public Class ChoosePokemonQueryObject

        Inherits QueryObject

        Dim _ready As Boolean = False
        Dim _opened As Boolean = False

        Public Sub New()
            MyBase.New(QueryTypes.ChoosePokemon)
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)

        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return _ready
            End Get
        End Property

    End Class

End Namespace