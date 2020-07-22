Namespace Items.Standard

    <Item(1998, "Douse Drive")>
    Public Class DouseDrive

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Makes Techno Blast a Water-type move if held by Genesect."
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(48, 312, 24, 24)
        End Sub

    End Class

End Namespace
