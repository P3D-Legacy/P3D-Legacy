Namespace Exceptions

    ''' <summary>
    ''' An exception to be thrown when an entity conversion encounters a type error.
    ''' </summary>
    Public Class InvalidEntityTypeException

        Inherits Exception

        Public Sub New(ByVal FromType As String, ByVal ToType As String)
            MyBase.New("Invalid conversion from entity type """ & FromType & """ to type """ & ToType & """.")
        End Sub

    End Class

End Namespace