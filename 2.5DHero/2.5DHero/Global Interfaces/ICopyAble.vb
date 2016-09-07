''' <summary>
''' An interface for classes to implement that can create a copied instance member.
''' </summary>
''' <remarks></remarks>
Public Interface ICopyAble

    ''' <summary>
    ''' Returns a new instance of the class in use.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Copy() As Object

End Interface