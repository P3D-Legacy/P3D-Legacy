Namespace GameModes.Maps.EntityProperties

    ''' <summary>
    ''' A property for movable entities.
    ''' </summary>
    Class MovableEntityProperty

        Inherits EntityProperty

        Private _moved As Single = 0F
        Private _speed As Single = 0.04F

        Public Sub New(ByVal params As EntityPropertyDataCreationStruct)
            MyBase.New(params) : End Sub

        Private Function GetSpeed() As Single
            ' If the entity owns a speed property, set that one as speed:
            If Parent.OwnsProperty(PROPERTY_NAME_SPEED) Then
                _speed = Parent.GetProperty(PROPERTY_NAME_SPEED).GetData(Of Single)
            End If
        
            Return _speed
        End Function

        ''' <summary>
        ''' Sets this entity in motion.
        ''' </summary>
        Public Sub Move(ByVal amount As Single)
            If _moved = 0F Then
                _moved = amount
            End If
        End Sub
        
        Public Overrides Sub Update()
            MyBase.Update()

            Dim speed As Single = GetSpeed()
            Dim movement As Vector3
            
            Select Case Parent.FaceDirection
                Case EntityFaceDirection.North
                    movement = New Vector3(0, 0, -1)
                Case EntityFaceDirection.West
                    movement = New Vector3(-1, 0, 0)
                Case EntityFaceDirection.South
                    movement = New Vector3(0, 0, 1)
                Case EntityFaceDirection.East
                    movement = New Vector3(1, 0, 0)
            End Select

            movement *= speed

            Parent.Position += movement

            _moved -= speed

            If _moved <= 0F Then
                _moved = 0F

                'Once the entity arrives at its destination, snap it back to the grid.
                Parent.SnapToGrid()
            End If
        End Sub

    End Class

End Namespace
