Namespace Items.Gems

    <Item(637, "Ground Gem")>
    Public Class GroundGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of land. When held, it strengthens the power of a Ground-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Ground)
        End Sub

    End Class

End Namespace
