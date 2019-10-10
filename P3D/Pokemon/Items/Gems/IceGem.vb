Namespace Items.Gems

    <Item(634, "Ice Gem")>
    Public Class IceGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of ice. When held, it strengthens the power of an Ice-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Ice)
        End Sub

    End Class

End Namespace
