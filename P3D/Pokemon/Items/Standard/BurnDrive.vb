Namespace Items.Standard

    <Item(1996, "Burn Drive")>
    Public Class BurnDrive

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Makes Techno Blast a Fire-type move if held by Genesect."
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(0, 312, 24, 24)
        End Sub

    End Class

End Namespace
