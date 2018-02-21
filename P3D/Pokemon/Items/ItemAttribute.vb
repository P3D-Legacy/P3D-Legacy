Namespace Items

    <AttributeUsage(AttributeTargets.Class)>
    Friend Class ItemAttribute

        Inherits Attribute

        Public ReadOnly Property Id As Integer
        Public ReadOnly Property Name As String

        Public Sub New(id As Integer, name As String)
            Me.Id = id
            Me.Name = name
        End Sub

    End Class

    Friend Structure ItemIdentifier
        Public Name As String
        Public Id As Integer
    End Structure

End Namespace
