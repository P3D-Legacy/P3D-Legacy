Public Class PokedexEntry

    Public Species As String = ""
    Public Text As String = ""
    Public Weight As Single = 0
    Public Height As Single = 0
    Public Color As Color

    Public Sub New(ByVal Text As String, ByVal Species As String, ByVal Weight As Single, ByVal Height As Single, ByVal Color As Color)
        Me.Text = Text
        Me.Species = Species
        Me.Weight = Weight
        Me.Height = Height
        Me.Color = Color
    End Sub

End Class