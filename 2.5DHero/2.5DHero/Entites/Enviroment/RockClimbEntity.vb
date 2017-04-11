Public Class RockClimbEntity

    Inherits Entity

    Dim TempScriptEntity As ScriptBlock = Nothing
    Dim TempClicked As Boolean = False 'If true, walk up.

    Public Overrides Sub ClickFunction()
        If Badge.CanUseHMMove(Badge.HMMoves.RockClimb) = True Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
            TempClicked = True
            If GetRockClimbPokemon() Is Nothing Then
                Screen.TextBox.Show("A Pokémon could~climb this rock...", {Me}, True, True)
            Else
                Screen.TextBox.Show("A Pokémon could~climb this rock.*Do you want to~use Rock Climb?%Yes|No%", {Me}, True, True)
            End If
        End If
    End Sub

    Public Overrides Sub WalkOntoFunction()
        If Badge.CanUseHMMove(Badge.HMMoves.RockClimb) = True Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
            TempClicked = False
            If GetRockClimbPokemon() Is Nothing Then
                Screen.TextBox.Show("A Pokémon could~climb this rock...", {Me}, True, True)
            Else
                Screen.TextBox.Show("A Pokémon could~climb this rock.*Do you want to~use Rock Climb?%Yes|No%", {Me}, True, True)
            End If
            SoundManager.PlaySound("select")
        Else
            Screen.TextBox.Show("A path is engraved~into this rock...", {Me}, True, True)
        End If
    End Sub

    Public Overrides Sub ResultFunction(ByVal Result As Integer)
        If Result = 0 Then
            If Me.TempClicked = True Then
                Me.WalkUp()
            Else
                Me.WalkDown()
            End If
        End If
    End Sub

    Private Function GetRockClimbPokemon() As Pokemon
        For Each teamPokemon As Pokemon In Core.Player.Pokemons
            If teamPokemon.IsEgg() = False Then
                For Each a As BattleSystem.Attack In teamPokemon.Attacks
                    If a.Name.ToLower() = "rock climb" Then
                        Return teamPokemon
                    End If
                Next
            End If
        Next

        'No rock climb in team:
        If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
            If Core.Player.Pokemons.Count > 0 Then
                Return Core.Player.Pokemons(0)
            Else
                Dim p As Pokemon = Pokemon.GetPokemonByID(10)
                p.Generate(10, True)
                Return p
            End If
        Else
            Return Nothing
        End If
    End Function

    Private Sub WalkUp()
        Dim facing As Integer = CInt(Me.Rotation.Y / MathHelper.PiOver2)
        facing -= 2
        If facing < 0 Then
            facing += 4
        End If

        Screen.Camera.PlannedMovement = Vector3.Zero

        If Screen.Camera.GetPlayerFacingDirection() = facing And Screen.Camera.IsMoving = False Then
            Dim Steps As Integer = 0

            Dim checkPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
            checkPosition.Y = checkPosition.Y.ToInteger()

            Dim foundSteps As Boolean = True
            While foundSteps = True
                Dim e As Entity = GetEntity(Screen.Level.Entities, checkPosition, True, {GetType(RockClimbEntity), GetType(ScriptBlock), GetType(WarpBlock)})
                If Not e Is Nothing Then
                    If e.EntityID.ToLower() = "rockclimbentity" Then
                        Steps += 1
                        checkPosition.X += Screen.Camera.GetMoveDirection().X
                        checkPosition.Z += Screen.Camera.GetMoveDirection().Z
                        checkPosition.Y += 1
                    Else
                        If e.EntityID = "ScriptBlock" Then
                            TempScriptEntity = CType(e, ScriptBlock)
                        ElseIf e.EntityID = "WarpBlock" Then
                            CType(e, WarpBlock).WalkAgainstFunction()
                        End If
                        foundSteps = False
                    End If
                Else
                    foundSteps = False
                End If
            End While

            Screen.Level.OverworldPokemon.Visible = False
            Screen.Level.OverworldPokemon.warped = True

            Dim tempSkin As String = Core.Player.Skin

            Dim RockClimbPokemon As Pokemon = GetRockClimbPokemon()

            Screen.Level.OwnPlayer.Texture = RockClimbPokemon.GetOverworldTexture()
            Screen.Level.OwnPlayer.ChangeTexture()

            Dim s As String = "@pokemon.cry(" & RockClimbPokemon.Number & ")" & vbNewLine &
                "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",1," & Screen.Camera.GetMoveDirection().Z & ")" & vbNewLine &
                "@sound.play(destroy)" & vbNewLine &
                "@player.move(" & Steps & ")" & vbNewLine &
                "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",0," & Screen.Camera.GetMoveDirection().Z & ")" & vbNewLine &
                "@overworldpokemon.hide" & vbNewLine &
                "@player.move(1)" & vbNewLine &
                "@overworldpokemon.hide" & vbNewLine &
                "@player.wearskin(" & tempSkin & ")"

            If Not Me.TempScriptEntity Is Nothing Then
                s &= vbNewLine & GetScriptStartLine(Me.TempScriptEntity)
                Me.TempScriptEntity = Nothing
            End If

            'Reset the player's transparency:
            Screen.Level.OwnPlayer.Opacity = 1.0F

            Construct.Controller.GetInstance().RunFromString(s)
        End If

        facing = CInt(Me.Rotation.Y / MathHelper.PiOver2)
        If facing < 0 Then
            facing += 4
        End If
    End Sub

    Private Sub WalkDown()
        Dim facing As Integer = CInt(Me.Rotation.Y / MathHelper.PiOver2)

        Screen.Camera.PlannedMovement = Vector3.Zero

        If Screen.Camera.GetPlayerFacingDirection() = facing Then
            Dim Steps As Integer = 0

            Dim checkPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
            checkPosition.Y = checkPosition.Y.ToInteger() - 1

            Dim foundSteps As Boolean = True
            While foundSteps = True
                Dim e As Entity = GetEntity(Screen.Level.Entities, checkPosition, True, {GetType(RockClimbEntity), GetType(ScriptBlock), GetType(WarpBlock)})
                If Not e Is Nothing Then
                    If e.EntityID = "RockClimbEntity" Then
                        Steps += 1
                        checkPosition.X += Screen.Camera.GetMoveDirection().X
                        checkPosition.Z += Screen.Camera.GetMoveDirection().Z
                        checkPosition.Y -= 1
                    Else
                        If e.EntityID = "ScriptBlock" Then
                            Me.TempScriptEntity = CType(e, ScriptBlock)
                        ElseIf e.EntityID = "WarpBlock" Then
                            CType(e, WarpBlock).WalkAgainstFunction()
                        End If
                        foundSteps = False
                    End If
                Else
                    foundSteps = False
                End If
            End While

            Screen.Level.OverworldPokemon.Visible = False
            Screen.Level.OverworldPokemon.warped = True

            Dim tempSkin As String = Core.Player.Skin

            Dim RockClimbPokemon As Pokemon = GetRockClimbPokemon()

            Screen.Level.OwnPlayer.Texture = RockClimbPokemon.GetOverworldTexture()
            Screen.Level.OwnPlayer.ChangeTexture()

            Dim s As String = "@pokemon.cry(" & RockClimbPokemon.Number & ")" & vbNewLine &
            "@player.move(1)" & vbNewLine &
            "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",-1," & Screen.Camera.GetMoveDirection().Z & ")" & vbNewLine &
            "@sound.play(destroy)" & vbNewLine &
            "@player.move(" & Steps & ")" & vbNewLine &
            "@overworldpokemon.hide" & vbNewLine &
            "@player.wearskin(" & tempSkin & ")"

            If Not Me.TempScriptEntity Is Nothing Then
                s &= vbNewLine & GetScriptStartLine(Me.TempScriptEntity)
                Me.TempScriptEntity = Nothing
            End If

            'Reset the player's transparency:
            Screen.Level.OwnPlayer.Opacity = 1.0F

            Construct.Controller.GetInstance().RunFromString(s)
        End If
    End Sub

    Private Function GetScriptStartLine(ByVal ScriptEntity As ScriptBlock) As String
        If Not ScriptEntity Is Nothing Then
            If ScriptEntity.CorrectRotation() = True Then
                Select Case ScriptEntity.GetActivationID()
                    Case 0
                        Return "@script.start(" & ScriptEntity.ScriptID & ")"
                    Case 1
                        Return "@script.text(" & ScriptEntity.ScriptID & ")"
                    Case 2
                        Return "@script.run(" & ScriptEntity.ScriptID & ")"
                End Select
            End If
        End If

        Return ""
    End Function

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

End Class