Namespace BattleSystem

    Public Class RoamingPokemonFledQueryObject

        Inherits QueryObject

        Dim _ready As Boolean = False

        Public Sub New()
            MyBase.New(QueryTypes.RoamingPokemonFled)
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            Me._ready = True

            BV2Screen.FieldEffects.RoamingFled = True
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return _ready
            End Get
        End Property

    End Class

End Namespace