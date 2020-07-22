Namespace Items.Gems

    <Item(642, "Ghost Gem")>
    Public Class GhostGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with a spectral essence. When held, it strengthens the power of a Ghost-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Ghost)
        End Sub

    End Class

End Namespace
