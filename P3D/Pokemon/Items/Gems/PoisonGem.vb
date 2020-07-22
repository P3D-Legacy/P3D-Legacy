Namespace Items.Gems

    <Item(636, "Poison Gem")>
    Public Class PoisonGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of poison. When held, it strengthens the power of a Poison-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Poison)
        End Sub

    End Class

End Namespace
