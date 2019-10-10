Namespace Items.Gems

    <Item(644, "Dark Gem")>
    Public Class DarkGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of darkness. When held, it strengthens the power of a Dark-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Dark)
        End Sub

    End Class

End Namespace
