Namespace Items.Gems

    <Item(640, "Bug Gem")>
    Public Class BugGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an insect-like essence. When held, it strengthens the power of a Bug-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Bug)
        End Sub

    End Class

End Namespace
