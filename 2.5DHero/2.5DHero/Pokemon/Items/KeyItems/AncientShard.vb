Namespace Items.KeyItems

    <Item(286, "Ancient Shard")>
    Public Class AncientShard

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A piece of a very ancient wall. It seems like it fits somewhere."

        Public Sub New()
            _textureRectangle = New Rectangle(360, 288, 24, 24)
        End Sub

    End Class

End Namespace
