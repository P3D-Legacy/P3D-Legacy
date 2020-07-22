Namespace Items.Gems

    <Item(645, "Steel Gem")>
    Public Class SteelGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of steel. When held, it strengthens the power of a Steel-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Steel)
        End Sub

    End Class

End Namespace
