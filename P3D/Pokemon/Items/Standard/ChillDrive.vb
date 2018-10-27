Namespace Items.Standard

    <Item(1997, "Chill Drive")>
    Public Class ChillDrive

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Makes Techno Blast an Ice-type move if held by Genesect."
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(24, 312, 24, 24)
        End Sub

    End Class

End Namespace
