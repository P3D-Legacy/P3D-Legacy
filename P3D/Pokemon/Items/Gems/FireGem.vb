Namespace Items.Gems

    <Item(630, "Fire Gem")>
    Public Class FireGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of fire. When held, it strengthens the power of a Fire-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Fire)
        End Sub

    End Class

End Namespace
