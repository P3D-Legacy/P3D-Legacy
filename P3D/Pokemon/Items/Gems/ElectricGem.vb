Namespace Items.Gems

    <Item(632, "Electric Gem")>
    Public Class ElectricGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of electricity. When held, it strengthens the power of an Electric-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Electric)
        End Sub

    End Class

End Namespace
