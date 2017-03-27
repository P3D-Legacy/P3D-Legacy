Namespace GameModes.Maps.EntityProperties

    ''' <summary>
    ''' This entity property makes the entity act like stairs.
    ''' </summary>
    Class StairsEntityProperty

        Inherits EntityProperty

        Public Sub New(ByVal params As EntityPropertyDataCreationStruct)
            MyBase.New(params)
        End Sub

        Public Overrides Function WalkAgainst() As FunctionResponse
            Dim facing As Integer = Parent.FaceDirection
            facing -= 2
            If facing < 0 Then
                facing += 4
            End If

            If Screen.Camera.GetFacingDirection() = facing And
                Not Screen.Camera.IsMoving() Then

                'Set this so the sound "bump" doesn't play.
                CType(Screen.Camera, OverworldCamera).DidWalkAgainst = False

                Dim steps As Integer = 0
                Dim checkPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
                Dim foundSteps As Boolean = True
                Dim moveDirection As Vector3 = Screen.Camera.GetMoveDirection()
                Dim tempScriptTriggerEntity As Entity = Nothing

                'Search how many steps there are in a row.
                'This needs to be done, because the player moves up all steps that are in a row.
                While foundSteps
                    Dim e As Entity = Nothing 'TODO: Adjust the level to return the new entity type.

                    If e IsNot Nothing Then
                        'Check if found entity has stair properties:

                        If e.OwnsProperty(PROPERTY_NAME_STAIRS) Then
                            steps += 1

                            checkPosition.X += moveDirection.X
                            checkPosition.Z += moveDirection.Z
                            checkPosition.Y += 1
                        Else
                            foundSteps = False

                            If e.OwnsProperty(PROPERTY_NAME_SCRIPTTRIGGER) Then
                                tempScriptTriggerEntity = e
                            Else
                                If e.OwnsProperty(PROPERTY_NAME_WARP) Then
                                    e.WalkAgainst()
                                End If
                            End If
                        End If
                    Else
                        foundSteps = False
                    End If
                End While

                Screen.Level.OverworldPokemon.Visible = False
                Screen.Level.OverworldPokemon.warped = True

                Dim s As String =
                    "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",1," & Screen.Camera.GetMoveDirection().Z & ")" & vbNewLine &
                    "@player.move(" & steps & ")" & vbNewLine &
                    "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",0," & Screen.Camera.GetMoveDirection().Z & ")" & vbNewLine &
                    "@pokemon.hide" & vbNewLine &
                    "@player.move(1)" & vbNewLine &
                    "@overworldpokemon.hide"

                If tempScriptTriggerEntity IsNot Nothing Then

                End If

                Construct.Controller.GetInstance().RunFromString(s, {})
                'Block the player from moving through the stairs:
                Return FunctionResponse.ValueTrue
            End If

            'Be able to walk through the backside of the stairs: 
            If Parent.FaceDirection = Screen.Camera.GetPlayerFacingDirection() Then
                Return FunctionResponse.ValueFalse
            End If

            'If the player walks against the stairs from either side, block
            Return FunctionResponse.ValueTrue
        End Function

        Public Overrides Sub WalkOnto()
            Dim facing As Integer = Parent.FaceDirection

            'Reset planned movement because this entity sets its own movement.
            Screen.Camera.PlannedMovement = Vector3.Zero

            If Screen.Camera.GetPlayerFacingDirection() = facing Then
                CType(Screen.Camera, OverworldCamera).DidWalkAgainst = False

                Dim steps As Integer = 0
                Dim checkPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
                Dim foundSteps As Boolean = True
                Dim moveDirection As Vector3 = Screen.Camera.GetMoveDirection()
                Dim tempScriptTriggerEntity As Entity = Nothing

                checkPosition.Y = checkPosition.Y.ToInteger() - 1

                While foundSteps
                    Dim e As Entity = Nothing 'TODO: Adjust the level to return the new entity type.

                    If e IsNot Nothing Then
                        If e.OwnsProperty(PROPERTY_NAME_STAIRS) Then
                            steps += 1

                            checkPosition.X += moveDirection.X
                            checkPosition.Z += moveDirection.Z
                            checkPosition.Y -= 1
                        Else
                            foundSteps = False

                            If e.OwnsProperty(PROPERTY_NAME_SCRIPTTRIGGER) Then
                                tempScriptTriggerEntity = e
                            Else
                                If e.OwnsProperty(PROPERTY_NAME_WARP) Then
                                    e.WalkAgainst()
                                End If
                            End If
                        End If
                    Else
                        foundSteps = False
                    End If
                End While

                Screen.Level.OverworldPokemon.Visible = False
                Screen.Level.OverworldPokemon.warped = True

                Dim s As String =
                    "@player.move(1)" & vbNewLine &
                    "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",-1," & Screen.Camera.GetMoveDirection().Z & ")" & vbNewLine &
                    "@player.move(" & steps & ")" & vbNewLine &
                    "@overworldpokemon.hide"

                If tempScriptTriggerEntity IsNot Nothing Then

                End If

                Construct.Controller.GetInstance().RunFromString(s, {})
            End If

        End Sub

    End Class

End Namespace
