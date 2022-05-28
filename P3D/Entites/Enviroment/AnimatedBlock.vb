﻿Public Class AnimatedBlock

    Inherits Entity

    Shared BlockTexturesTemp As New Dictionary(Of String, Texture2D)


    Dim AnimationNames As List(Of String)

    Dim Animations As List(Of Animation)
    Dim currentRectangle As List(Of Rectangle)

    Dim X, Y, width, height, rows, columns, animationSpeed, startRow, startColumn As List(Of Integer)

    Dim AnimCount As Integer = 0


    Sub New()
        X = New List(Of Integer)
        Y = New List(Of Integer)
        width = New List(Of Integer)
        height = New List(Of Integer)
        rows = New List(Of Integer)
        columns = New List(Of Integer)
        animationSpeed = New List(Of Integer)
        startRow = New List(Of Integer)
        startColumn = New List(Of Integer)

        AnimationNames = New List(Of String)
        currentRectangle = New List(Of Rectangle)
        Animations = New List(Of Animation)
    End Sub

    Public Overloads Sub Initialize(ByVal AnimationData As List(Of List(Of Integer)))

        MyBase.Initialize()
        For i = 0 To AnimationData.Count - 1
            X.Add(AnimationData(i)(0))
            Y.Add(AnimationData(i)(1))
            width.Add(AnimationData(i)(2))
            height.Add(AnimationData(i)(3))
            rows.Add(AnimationData(i)(4))
            columns.Add(AnimationData(i)(5))
            animationSpeed.Add(AnimationData(i)(6))
            startRow.Add(AnimationData(i)(7))
            startColumn.Add(AnimationData(i)(8))

            AnimationNames.Add("")
            currentRectangle.Add(New Rectangle(0, 0, 0, 0))

            Animations.Add(New Animation(TextureManager.GetTexture("Textures\Routes"), rows(i), columns(i), 16, 16, animationSpeed(i), startRow(i), startColumn(i)))

            AnimCount += 1
        Next

        CreateBlockTextureTemp()
    End Sub

    Public Shared Sub ClearAnimationResources()
        BlockTexturesTemp.Clear()
    End Sub

    Private Sub CreateBlockTextureTemp()
        'If Core.GameOptions.GraphicStyle = 1 Then

        For n = 0 To Animations.Count - 1
            Dim r As New Rectangle(X(n), Y(n), width(n), height(n))
            Me.AnimationNames(n) = AdditionalValue & "," & X(n) & "," & Y(n) & "," & height(n) & "," & width(n)
            If BlockTexturesTemp.ContainsKey(AnimationNames(n) & "_0") = False Then
                For i = 0 To Me.rows(n) - 1
                    For j = 0 To Me.columns(n) - 1
                        BlockTexturesTemp.Add(AnimationNames(n) & "_" & (j + columns(n) * i).ToString, TextureManager.GetTexture(AdditionalValue, New Rectangle(r.X + r.Width * j, r.Y + r.Height * i, r.Width, r.Height)))
                    Next
                Next
            End If
        Next

    End Sub

    Public Overrides Sub ClickFunction()
        Me.Surf()
    End Sub

    Public Overrides Function WalkAgainstFunction() As Boolean
        WalkOntoFunction()
        Return MyBase.WalkAgainstFunction()
    End Function

    Public Overrides Function WalkIntoFunction() As Boolean
        WalkOntoFunction()
        Return MyBase.WalkIntoFunction()
    End Function

    Public Overrides Sub WalkOntoFunction()
        If Screen.Level.Surfing = True Then
            Dim canSurf As Boolean = False

            For Each Entity As Entity In Screen.Level.Entities
                If Entity.boundingBox.Contains(Screen.Camera.GetForwardMovedPosition()) = ContainmentType.Contains Then
                    If Entity.ActionValue = 0 AndAlso (Entity.EntityID = "AnimatedBlock" OrElse Entity.EntityID = "Water") Then
                        canSurf = True
                    Else
                        If Entity.Collision = True Then
                            canSurf = False
                            Exit For
                        End If
                    End If
                End If
            Next

            If canSurf = True Then
                If CType(Screen.Camera, OverworldCamera)._debugWalk = False Then
                    Screen.Camera.Move(1)
                End If

                Screen.Level.PokemonEncounter.TryEncounterWildPokemon(Me.Position, Spawner.EncounterMethods.Surfing, "")
            End If
        End If
    End Sub

    Private Sub Surf()
        If Screen.Camera.Turning = False Then
            If Screen.Level.Surfing = False Then
                If Badge.CanUseHMMove(Badge.HMMoves.Surf) = True Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                    If Screen.ChooseBox.Showing = False Then
                        Dim canSurf As Boolean = False

                        If Me.ActionValue = 0 Then
                            For Each Entity As Entity In Screen.Level.Entities
                                If Entity.boundingBox.Contains(Screen.Camera.GetForwardMovedPosition()) = ContainmentType.Contains Then
                                    If Entity.EntityID = "AnimatedBlock" Then
                                        If Core.Player.SurfPokemon > -1 Then
                                            canSurf = True
                                        End If
                                    Else
                                        If Entity.Collision = True Then
                                            canSurf = False
                                            Exit For
                                        End If
                                    End If
                                End If
                            Next
                        End If

                        If Screen.Level.Riding = True Then
                            canSurf = False
                        End If

                        If canSurf = True Then
                            Dim message As String = "Do you want to Surf?%Yes|No%"
                            Screen.TextBox.Show(message, {Me}, True, True)
                            SoundManager.PlaySound("select")
                        End If
                    End If
                End If
            End If
        End If
    End Sub


    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) As Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.25F
    End Function

    Public Overrides Sub UpdateEntity()
        If Not Animations Is Nothing Then
            For n = 0 To Animations.Count - 1
                Animations(n).Update(0.01)
                If currentRectangle(n) <> Animations(n).TextureRectangle Then
                    ChangeTexture(n)

                    currentRectangle(n) = Animations(n).TextureRectangle
                End If
            Next
        End If
        MyBase.UpdateEntity()
    End Sub

    Private Sub ChangeTexture(ByVal n As Integer)
        'If Core.GameOptions.GraphicStyle = 1 Then

        If BlockTexturesTemp.Count = 0 Then
            ClearAnimationResources()
            CreateBlockTextureTemp()
        End If
        Dim i = Animations(n).CurrentRow
        Dim j = Animations(n).CurrentColumn
        Me.Textures(n) = BlockTexturesTemp(AnimationNames(n) & "_" & (j + columns(n) * i))

        'End If
    End Sub

    Public Overrides Sub ResultFunction(ByVal Result As Integer)
        If Result = 0 Then
            Screen.TextBox.Show(Core.Player.Pokemons(Core.Player.SurfPokemon).GetDisplayName() & " used~Surf!", {Me})
            Screen.Level.Surfing = True
            Screen.Camera.Move(1)
            PlayerStatistics.Track("Surf used", 1)

            With Screen.Level.OwnPlayer
                Core.Player.TempSurfSkin = .SkinName

                Dim pokemonNumber As Integer = Core.Player.Pokemons(Core.Player.SurfPokemon).Number
                Dim SkinName As String = "[POKEMON|N]" & pokemonNumber & PokemonForms.GetOverworldAddition(Core.Player.Pokemons(Core.Player.SurfPokemon))
                If Core.Player.Pokemons(Core.Player.SurfPokemon).IsShiny = True Then
                    SkinName = "[POKEMON|S]" & pokemonNumber & PokemonForms.GetOverworldAddition(Core.Player.Pokemons(Core.Player.SurfPokemon))
                End If

                .SetTexture(SkinName, False)

                .UpdateEntity()

                SoundManager.PlayPokemonCry(pokemonNumber)

                If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
                    MusicManager.Play("surf", True)
                End If
            End With
        End If
    End Sub

    Public Overrides Sub Render()
        Dim setRasterizerState As Boolean = Me.Model.ID <> 0

        Me.Draw(Me.Model, Textures, setRasterizerState)
    End Sub

End Class