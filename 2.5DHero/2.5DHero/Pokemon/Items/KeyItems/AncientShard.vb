Namespace Items.KeyItems

    <Item(286, "Ancient Shard")>
    Public Class AncientShard

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A fragment of an ancient structure. It seems like it could fit somewhere."

        Public Sub New()
            _textureRectangle = New Rectangle(360, 288, 24, 24)
        End Sub

    End Class

End Namespace
