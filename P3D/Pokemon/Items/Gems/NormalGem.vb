Namespace Items.Gems

    <Item(646, "Normal Gem")>
    Public Class NormalGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an ordinary essence. When held, it strengthens the power of a Normal-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Normal)
        End Sub

    End Class

End Namespace
