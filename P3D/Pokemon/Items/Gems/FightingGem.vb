Namespace Items.Gems

    <Item(635, "Fighting Gem")>
    Public Class FightingGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of combat. When held, it strengthens the power of a Fighting-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Fighting)
        End Sub

    End Class

End Namespace
