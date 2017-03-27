Namespace GameModes.Maps

    ''' <summary>
    ''' Flags indicating which parts of the entity world matrix are dirty.
    ''' </summary>
    <Flags>
    Enum EntityWorldDirtyFlags
        None = 0
        Scale = 1
        Rotation = 2
        Position = 4
    End Enum

    ''' <summary>
    ''' Used to create the world matrix, bounding box and view box for entities and keep track of changes.
    ''' </summary>
    Class EntityWorldCreator

        Private _parent As Entity

        Private Const ALL_DIRTY As EntityWorldDirtyFlags = EntityWorldDirtyFlags.Scale Or EntityWorldDirtyFlags.Rotation Or EntityWorldDirtyFlags.Position
        Private Const BOUNDINGBOX_SCALE As Single = 1.25F

        Private _dirtyFlags As EntityWorldDirtyFlags = ALL_DIRTY
        Private _isBoundingBoxCreated As Boolean = False
        Private _isViewBoxCreated As Boolean = False

        Private _scaleM As Matrix
        Private _rotationM As Matrix
        Private _positionM As Matrix

        Private _worldM As Matrix
        Private _boundingBox As BoundingBox
        Private _viewBox As BoundingBox

        ''' <summary>
        ''' Creates a new intance of the <see cref="EntityWorldCreator"/>.
        ''' </summary>
        Public Sub New(ByVal parent As Entity)
            _parent = parent
        End Sub

        Private Function IsDirty(ByVal worldPart As EntityWorldDirtyFlags) As Boolean
            Return (_dirtyFlags And worldPart) = worldPart
        End Function

        ''' <summary>
        ''' Sets part of the world matrix as dirty and marks it for recreation.
        ''' </summary>
        Public Sub SetDirty(ByVal worldPart As EntityWorldDirtyFlags)
            _dirtyFlags = _dirtyFlags Or worldPart

            If worldPart = EntityWorldDirtyFlags.Position Then
                _isBoundingBoxCreated = False
            End If

            _isViewBoxCreated = False
        End Sub

        ''' <summary>
        ''' Returns the current world matrix and recreates it if needed.
        ''' </summary>
        Public Function GetWorldMatrix() As Matrix
            If _dirtyFlags <> EntityWorldDirtyFlags.None Then
                If IsDirty(EntityWorldDirtyFlags.Scale) Then
                    _dirtyFlags = _dirtyFlags And Not EntityWorldDirtyFlags.Scale
                    _scaleM = Matrix.CreateScale(_parent.Scale)
                End If
                If IsDirty(EntityWorldDirtyFlags.Rotation) Then
                    _dirtyFlags = _dirtyFlags And Not EntityWorldDirtyFlags.Rotation
                    _rotationM = Matrix.CreateFromYawPitchRoll(_parent.Rotation.Y, _parent.Rotation.X, _parent.Rotation.Z)
                End If
                If IsDirty(EntityWorldDirtyFlags.Position) Then
                    _dirtyFlags = _dirtyFlags And Not EntityWorldDirtyFlags.Position
                    _positionM = Matrix.CreateTranslation(_parent.Position)
                End If

                _worldM = _scaleM * _rotationM * _positionM
            End If

            Return _worldM
        End Function

        ''' <summary>
        ''' Returns and creates the bounding box, if necessary.
        ''' </summary>
        Public Function GetBoundingBox() As BoundingBox
            If Not _isBoundingBoxCreated Then
                Dim boxCorner As New Vector3(0.5F)

                _boundingBox = New BoundingBox(
                    boxCorner * -1 * BOUNDINGBOX_SCALE + _parent.Position,
                    boxCorner * BOUNDINGBOX_SCALE + _parent.Position
                )

                _isBoundingBoxCreated = True
            End If

            Return _boundingBox
        End Function

        ''' <summary>
        ''' Returns and creates the view box, if necessary.
        ''' </summary>
        Public Function GetViewBox() As BoundingBox
            If Not _isViewBoxCreated Then
                Dim boxCorner As Vector3 = _parent.Scale / 2

                If _parent.FaceDirection = EntityFaceDirection.East Or _parent.FaceDirection = EntityFaceDirection.West Then
                    'When the object is turned by 90°/270°, invert X and Z:
                    Dim tempZ As Single = boxCorner.Z
                    boxCorner.Z = boxCorner.X
                    boxCorner.X = tempZ
                End If

                _viewBox = New BoundingBox(
                    boxCorner * -1 + _parent.Position,
                    boxCorner + _parent.Position
                )

                _isViewBoxCreated = True
            End If

            Return _viewBox
        End Function

    End Class

End Namespace
