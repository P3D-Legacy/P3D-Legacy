Namespace GameModes.Maps.EntityProperties

    ''' <summary>
    ''' The state describing if and how this entity property renders something special.
    ''' </summary>
    Enum EntityPropertyRenderResultType
        ''' <summary>
        ''' This entity property rendered something and was the last entity property in this entity to render something.
        ''' </summary>
        Rendered
        ''' <summary>
        ''' This entity property rendered something, but the next entity property can also render something.
        ''' </summary>
        RenderedButPassed
        ''' <summary>
        ''' This entity property did not render something special.
        ''' </summary>
        Passed
    End Enum

    ''' <summary>
    ''' The response from a player interaction with this entity property.
    ''' </summary>
    Enum FunctionResponse
        ValueFalse
        ValueTrue
        ''' <summary>
        ''' The function returned an undefined state.
        ''' </summary>
        NoValue
    End Enum

    ''' <summary>
    ''' The base class for a property of an Entity.
    ''' </summary>
    MustInherit Class EntityProperty

        ''' <summary>
        ''' The original name of this property.
        ''' </summary>
        Public ReadOnly Property Name As String
        ''' <summary>
        ''' The data of this property.
        ''' </summary>
        Public Property Data As String

        ''' <summary>
        ''' The owner entity of this property.
        ''' </summary>
        Protected ReadOnly Property Parent As Entity

        Public Sub New(ByVal params As EntityPropertyDataCreationStruct)
            Name = params.Name
            Data = params.Data
            Parent = params.Parent
        End Sub

        ''' <summary>
        ''' Updates the property's logic.
        ''' </summary>
        Public Overridable Sub Update()
        End Sub

        ''' <summary>
        ''' Renders this property, if this property has special render settings for the entity.
        ''' </summary>
        Public Overridable Function Render() As EntityPropertyRenderResultType
            Return EntityPropertyRenderResultType.Passed
        End Function

#Region "Behaviour"

        Public Overridable Sub ChooseBoxResult(ByVal resultIndex As Integer)
        End Sub

        ''' <summary>
        ''' Gets executed when the player clicks on the entity.
        ''' </summary>
        Public Overridable Sub Click()
        End Sub

        ''' <summary>
        ''' Gets executed when the player walks on top of the entity.
        ''' </summary>
        Public Overridable Sub WalkOnto()
        End Sub

        ''' <summary>
        ''' When the player walks against the entity and would collide with it.
        ''' </summary>
        ''' <remarks>
        ''' Return types:
        ''' True: Player collides with entity
        ''' False: Player does not collide with entity
        ''' Undefined: Check next.
        ''' </remarks>
        Public Overridable Function WalkAgainst() As FunctionResponse
            Return FunctionResponse.NoValue
        End Function

        ''' <summary>
        ''' When the player walks into the entity and would not collide with it.
        ''' </summary>
        Public Overridable Function WalkInto() As FunctionResponse
            Return FunctionResponse.NoValue
        End Function

        ''' <summary>
        ''' If the entity would let the player move from it.
        ''' </summary>
        Public Overridable Function LetPlayerMove() As FunctionResponse
            Return FunctionResponse.NoValue
        End Function

#End Region

        ''' <summary>
        ''' Converts the data to the T type.
        ''' </summary>
        Public Function GetData(Of T)() As T
            Select Case GetType(T)
                Case GetType(Vector3)
                    Return CType(EntityPropertyTypeConverter.ToVector3(Data), T)
                Case GetType(Vector2)
                    Return CType(EntityPropertyTypeConverter.ToVector2(Data), T)
                Case GetType(Single)
                    Return CType(EntityPropertyTypeConverter.ToSingle(Data), T)
                Case GetType(Integer)
                    Return CType(EntityPropertyTypeConverter.ToInteger(Data), T)
                Case GetType(String)
                    Return CType(CObj(Data), T)
            End Select
        End Function

    End Class

End Namespace
