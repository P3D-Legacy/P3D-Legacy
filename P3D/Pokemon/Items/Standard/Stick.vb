Namespace Items.Standard

    <Item(105, "Stick")>
    Public Class Stick

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by Farfetch'd. It is a very long and stiff stalk of leek that boosts the critical-hit ratio."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property FlingDamage As Integer = 60
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(168, 96, 24, 24)
        End Sub

    End Class

End Namespace
