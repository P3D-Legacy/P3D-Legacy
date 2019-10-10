Namespace Items.Gems

    <Item(631, "Water Gem")>
    Public Class WaterGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of water. When held, it strengthens the power of a Water-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Water)
        End Sub

    End Class

End Namespace
