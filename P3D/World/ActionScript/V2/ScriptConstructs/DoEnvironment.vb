Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <environment> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoEnvironment(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "daytime"
                    Return World.GetTime.ToString()
                Case "daytimeid"
                    Return int(CInt(World.GetTime)).ToString()
                Case "season"
                    Return World.CurrentSeason.ToString()
                Case "seasonid"
                    Return int(CInt(World.CurrentSeason)).ToString()
                Case "day"
                    Return DateTime.Now.DayOfWeek.ToString()
                Case "dayofyear"
                    Return DateTime.Now.DayOfYear().ToString()
                Case "dayinformation"
                    Return DateTime.Now.DayOfWeek.ToString() & "," & World.GetTime.ToString()
                Case "week"
                    Return World.WeekOfYear.ToString()
                Case "hour"
                    Return DateTime.Now.Hour.ToString()
                Case "year"
                    Return DateTime.Now.Year.ToString()
                Case "weather", "mapweather", "currentmapweather"
                    Return Screen.Level.World.CurrentMapWeather.ToString()
                Case "weatherid", "mapweatherid", "currentmapweatherid"
                    Return int(CInt(Screen.Level.World.CurrentMapWeather)).ToString()
                Case "regionweather"
                    Return World.GetCurrentRegionWeather().ToString()
                Case "regionweatherid"
                    Return int(CInt(World.GetCurrentRegionWeather())).ToString()
                Case "canfly"
                    Return ReturnBoolean(Screen.Level.CanFly)
                Case "candig"
                    Return ReturnBoolean(Screen.Level.CanDig)
                Case "canteleport"
                    Return ReturnBoolean(Screen.Level.CanTeleport)
                Case "wildpokemongrass"
                    Return ReturnBoolean(Screen.Level.WildPokemonGrass)
                Case "wildpokemonwater"
                    Return ReturnBoolean(Screen.Level.WildPokemonWater)
                Case "wildpokemoneverywhere"
                    Return ReturnBoolean(Screen.Level.WildPokemonFloor)
                Case "isdark"
                    Return ReturnBoolean(Screen.Level.IsDark)
                Case "region"
                    If argument <> "" Then
                        Return Screen.Level.CurrentRegion.Split(CChar(","))(int(argument))
                    End If
                    Return Screen.Level.CurrentRegion
                Case "regionalform"
                    If argument <> "" Then
                        Return Screen.Level.RegionalForm.Split(CChar(","))(int(argument))
                    End If
                    Return Screen.Level.RegionalForm
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace