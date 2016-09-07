Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @environment commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoEnvironment(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "setweather"
                    Screen.Level.WeatherType = int(argument)
                Case "setregionweather"
                    World.RegionWeather = CType(int(argument), World.Weathers)
                Case "setcanfly"
                    Screen.Level.CanFly = CBool(argument)
                Case "setcandig"
                    Screen.Level.CanDig = CBool(argument)
                Case "setcanteleport"
                    Screen.Level.CanTeleport = CBool(argument)
                Case "setwildpokemongrass"
                    Screen.Level.WildPokemonGrass = CBool(argument)
                Case "setwildpokemonwater"
                    Screen.Level.WildPokemonWater = CBool(argument)
                Case "setwildpokemoneverywhere"
                    Screen.Level.WildPokemonFloor = CBool(argument)
                Case "setisdark"
                    Screen.Level.IsDark = CBool(argument)
                Case "setrenderdistance"
                    Select Case argument.ToLower()
                        Case "0", "tiny"
                            Core.GameOptions.RenderDistance = 0
                        Case "1", "small"
                            Core.GameOptions.RenderDistance = 1
                        Case "2", "normal"
                            Core.GameOptions.RenderDistance = 2
                        Case "3", "far"
                            Core.GameOptions.RenderDistance = 3
                        Case "4", "extreme"
                            Core.GameOptions.RenderDistance = 4
                    End Select
                Case "toggledarkness"
                    Screen.Level.IsDark = Not Screen.Level.IsDark
            End Select

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            IsReady = True
        End Sub

    End Class

End Namespace