Namespace Items.Gems

    <Item(643, "Dragon Gem")>
    Public Class DragonGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with a draconic essence. When held, it strengthens the power of a Dragon-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Dragon)
        End Sub

    End Class

End Namespace
