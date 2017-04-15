Namespace DataModel.Json

    Class EntityPropertyExtension

        Private _entityModel As Game.EntityModel

        Public Sub New(ByVal entityModel As Game.EntityModel)
            _entityModel = entityModel
        End Sub

        Private Property _PropertyValue(ByVal propertyName As String) As String
            Get
                For Each p As Game.EntityModel.PropertyDataModel In _entityModel.Properties
                    If p.Name = propertyName Then
                        Return p.Value
                    End If
                Next
                Return ""
            End Get
            Set(value As String)
                For Each p As Game.EntityModel.PropertyDataModel In _entityModel.Properties
                    If p.Name = propertyName Then
                        p.Value = value
                    End If
                Next
            End Set
        End Property

#Region "Converters"

        Private Function Bool(ByVal s As String) As Boolean
            Return Boolean.Parse(s)
        End Function

#End Region

#Region "Floor"

        Public ReadOnly Property HasIce() As Boolean
            Get
                Return Bool(_PropertyValue("HasIce"))
            End Get
        End Property

#End Region

    End Class

End Namespace