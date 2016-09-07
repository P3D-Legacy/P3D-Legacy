Namespace Items

    ''' <summary>
    ''' Represents a Vitamin Item.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VitaminItem

        Inherits Item

        ''' <summary>
        ''' Creates a new instance of the Item class.
        ''' </summary>
        ''' <param name="Name">The name of the Item.</param>
        ''' <param name="Price">The purchase price.</param>
        ''' <param name="ItemType">The type of Item.</param>
        ''' <param name="ID">The ID of this Item.</param>
        ''' <param name="CatchMultiplier">The CatchMultiplier of this Item.</param>
        ''' <param name="SortValue">The SortValue of this Item.</param>
        ''' <param name="TextureRectangle">The TextureRectangle from the "Items\ItemSheet" texture.</param>
        ''' <param name="Description">The description of this Item.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Name As String, ByVal Price As Integer, ByVal ItemType As ItemTypes, ByVal ID As Integer, ByVal CatchMultiplier As Single, ByVal SortValue As Integer, ByVal TextureRectangle As Rectangle, ByVal Description As String)
            MyBase.New(Name, Price, ItemType, ID, CatchMultiplier, SortValue, TextureRectangle, Description)
        End Sub

        ''' <summary>
        ''' Checks if a Vitamin can be used on a Pokémon.
        ''' </summary>
        ''' <param name="stat">An integer representing the stat that should be upped by the Vitamin.</param>
        ''' <param name="p">The Pokémon that the Vitamin should be used on.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function CanUseVitamin(ByVal stat As Integer, ByVal p As Pokemon) As Boolean
            If stat < 100 Then
                Dim allStats As Integer = p.EVAttack + p.EVDefense + p.EVSpAttack + p.EVSpDefense + p.EVHP + p.EVSpeed
                If allStats < 510 Then
                    Return True
                End If
            End If

            Return False
        End Function

    End Class

End Namespace