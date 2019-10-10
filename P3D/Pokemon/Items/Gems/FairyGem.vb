Namespace Items.Gems

    <Item(647, "Fairy Gem")>
    Public Class FairyGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of the fey. When held, it strengthens the power of a Fairy-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Fairy)
        End Sub

    End Class

End Namespace
