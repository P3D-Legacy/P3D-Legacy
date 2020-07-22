Namespace Items.Gems

    <Item(638, "Flying Gem")>
    Public Class FlyingGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of air. When held, it strengthens the power of a Flying-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Flying)
        End Sub

    End Class

End Namespace
