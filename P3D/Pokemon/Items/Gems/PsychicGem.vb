Namespace Items.Gems

    <Item(639, "Psychic Gem")>
    Public Class PsychicGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of the mind. When held, it strengthens the power of a Psychic-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Psychic)
        End Sub

    End Class

End Namespace
