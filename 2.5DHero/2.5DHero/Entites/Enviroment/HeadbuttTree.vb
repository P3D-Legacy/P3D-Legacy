Public Class HeadbuttTree

    Inherits Entity

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
            CreatedWorld = False
        End If

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub ClickFunction()
        If Screen.Level.Surfing = False Then
            Dim pName As String = ""

            For Each p As Pokemon In Core.Player.Pokemons
                If p.IsEgg() = False Then
                    For Each a As BattleSystem.Attack In p.Attacks
                        If a.Name = "Headbutt" Then
                            pName = p.GetDisplayName()
                            Exit For
                        End If
                    Next

                    If pName <> "" Then
                        Exit For
                    End If
                End If
            Next

            If pName <> "" And Core.Player.Badges.Contains(10) Then
                Dim text As String = "This tree could have~a Pokémon in it.*Do you want to~use Headbutt?%Yes|No%"
                Screen.TextBox.Show(text, {Me})
                SoundManager.PlaySound("select")
            End If
        End If
    End Sub

    Public Overrides Sub ResultFunction(Result As Integer)
        If Result = 0 Then
            Dim pName As String = ""

            For Each p As Pokemon In Core.Player.Pokemons
                For Each a As BattleSystem.Attack In p.Attacks
                    If a.Name = "Headbutt" Then
                        pName = p.GetDisplayName()
                        Exit For
                    End If
                Next

                If pName <> "" Then
                    Exit For
                End If
            Next

            Dim spawnedPokemon As Pokemon = Spawner.GetPokemon(Screen.Level.LevelFile, Spawner.EncounterMethods.Headbutt, False)
            If spawnedPokemon Is Nothing Then
                Dim s As String = "@text.show(" & pName & " used~Headbutt!)" & vbNewLine &
                    "@sound.play(destroy,0)" & vbNewLine &
                    "@level.wait(20)" & vbNewLine &
                    "@text.show(Nothing happened...)"
                Construct.Controller.GetInstance().RunFromString(s, {Construct.Controller.ScriptRunOptions.CheckDelay})
            Else
                Dim s As String = "@text.show(" & pName & " used~Headbutt!)" & vbNewLine &
                    "@sound.play(destroy,0)" & vbNewLine &
                    "@level.wait(20)" & vbNewLine &
                    "@text.show(A wild Pokémon~appeared!)" & vbNewLine &
                    "@battle.wild(" & spawnedPokemon.Number & "," & spawnedPokemon.Level & ")"
                Construct.Controller.GetInstance().RunFromString(s, {Construct.Controller.ScriptRunOptions.CheckDelay})
            End If
        End If
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

End Class