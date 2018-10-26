Namespace Items.Standard

    <Item(1999, "Shock Drive")>
    Public Class ShockDrive

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Makes Techno Blast an Electric-type move if held by Genesect."
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(72, 312, 24, 24)
        End Sub

    End Class

End Namespace
