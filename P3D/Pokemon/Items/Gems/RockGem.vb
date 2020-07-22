Namespace Items.Gems

    <Item(641, "Rock Gem")>
    Public Class RockGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of rock. When held, it strengthens the power of a Rock-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Rock)
        End Sub

    End Class

End Namespace
