Namespace Items

    ''' <summary>
    ''' The base class for all fossil items.
    ''' </summary>
    Public MustInherit Class FossilItem

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 3500
        Public Overrides ReadOnly Property FlingDamage As Integer = 100
        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Standard
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public Sub New()
            _textureSource = "Items\Fossils"
        End Sub

    End Class

End Namespace
