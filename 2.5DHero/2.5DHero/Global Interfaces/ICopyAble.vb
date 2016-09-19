''' <summary>
''' An interface for classes to implement that can create a copied instance member.
''' </summary>
Public Interface ICopyAble

    ''' <summary>
    ''' Returns a new instance of the class in use.
    ''' </summary>
    Function Copy() As Object

End Interface