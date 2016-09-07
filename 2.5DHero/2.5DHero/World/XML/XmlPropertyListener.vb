Namespace XmlLevel

    Public MustInherit Class XmlPropertyListener

        Public XmlEntity As XmlEntity

        Private _associatedPropertyName As String = ""
        Private _implementWalkAgainst As Boolean = False
        Private _implementWalkInto As Boolean = False
        Private _implementLetPlayerMove As Boolean = False

        Public Sub New(ByVal XmlEntityReference As XmlEntity, ByVal AssociatedPropertyName As String)
            Me.XmlEntity = XmlEntityReference
            Me._associatedPropertyName = AssociatedPropertyName
        End Sub

        Public Overridable Sub UpdateEntity()
        End Sub
        Public Overridable Sub Update()
        End Sub
        Public Overridable Sub Draw()
        End Sub
        Public Overridable Sub Render()
        End Sub
        Public Overridable Sub PlayerInteraction()
        End Sub
        Public Overridable Function WalkAgainst() As Boolean
            Return True
        End Function
        Public Overridable Function WalkInto() As Boolean
            Return False
        End Function
        Public Overridable Sub WalkOnto()
        End Sub
        Public Overridable Sub ResultFunction(ByVal Result As Integer)
        End Sub
        Public Overridable Function LetPlayerMove() As Boolean
            Return True
        End Function

        Public Shared Function GetPropertyListeners(ByVal XmlEntity As XmlEntity) As List(Of XmlPropertyListener)
            Dim l As New List(Of XmlPropertyListener)

            For Each n As String In XmlEntity.GetPropertyNameList()
                Select Case n.ToLower()
                    Case "isscripttrigger"
                        If XmlEntity.GetPropertyValue(Of Boolean)("isscripttrigger") = True Then
                            l.Add(New ScriptBlockPropertyListener(XmlEntity))
                        End If
                    Case "faceplayer"
                        If XmlEntity.GetPropertyValue(Of Boolean)("faceplayer") = True Then
                            l.Add(New FacePlayerPropertyListener(XmlEntity))
                        End If
                    Case "isstairs"
                        If XmlEntity.GetPropertyValue(Of Boolean)("isstairs") = True Then
                            l.Add(New StairsPropertyListener(XmlEntity))
                        End If
                End Select
            Next

            Return l
        End Function

        Public ReadOnly Property AssociatedPropertyName() As String
            Get
                Return Me._associatedPropertyName
            End Get
        End Property

        Public Property ImplementWalkAgainst() As Boolean
            Get
                Return Me._implementWalkAgainst
            End Get
            Set(value As Boolean)
                Me._implementWalkAgainst = value
            End Set
        End Property

        Public Property ImplementWalkInto() As Boolean
            Get
                Return Me._implementWalkInto
            End Get
            Set(value As Boolean)
                Me._implementWalkInto = value
            End Set
        End Property

        Public Property ImplementLetPlayerMove() As Boolean
            Get
                Return Me._implementLetPlayerMove
            End Get
            Set(value As Boolean)
                Me._implementLetPlayerMove = value
            End Set
        End Property

    End Class

End Namespace