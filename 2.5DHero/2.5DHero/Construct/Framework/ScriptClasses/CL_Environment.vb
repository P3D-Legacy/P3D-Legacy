Namespace Construct.Framework.Classes

    <ScriptClass("Environment")>
    <ScriptDescription("A class to change environment setttings of a level.")>
    Public Class CL_Environment

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("SetWeather")>
        <ScriptDescription("Sets the weather type of the current level.")>
        Private Function M_SetWeather(ByVal argument As String) As String
            Screen.Level.WeatherType = Int(argument)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("SetRegionWeather")>
        <ScriptDescription("Sets the region weather.")>
        Private Function M_SetRegionWeather(ByVal argument As String) As String
            World.RegionWeather = CType(Int(argument), World.Weathers)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("SetCanFly")>
        <ScriptDescription("Sets if the player can use Fly on the map.")>
        Private Function M_SetCanFly(ByVal argument As String) As String
            Screen.Level.CanFly = Bool(argument)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("SetCanDig")>
        <ScriptDescription("Sets if the player can use Dig on the map.")>
        Private Function M_SetCanDig(ByVal argument As String) As String
            Screen.Level.CanDig = Bool(argument)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("SetCanTeleport")>
        <ScriptDescription("Sets if the player can use Teleport on the map.")>
        Private Function M_SetCanTeleport(ByVal argument As String) As String
            Screen.Level.CanTeleport = Bool(argument)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("SetWildPokemonGrass")>
        <ScriptDescription("Sets if one can encounter wild Pokémon in the grass.")>
        Private Function M_SetWildPokemonGrass(ByVal argument As String) As String
            Screen.Level.WildPokemonGrass = Bool(argument)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("SetWildPokemonWater")>
        <ScriptDescription("Sets if one can encounter wild Pokémon on the water while surfing.")>
        Private Function M_SetWildPokemonWater(ByVal argument As String) As String
            Screen.Level.WildPokemonWater = Bool(argument)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("WildPokemonEverywhere")>
        <ScriptDescription("Sets if one can encounter wild Pokémon on every floor tile.")>
        Private Function M_WildPokemonEverywhere(ByVal argument As String) As String
            Screen.Level.WildPokemonFloor = Bool(argument)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("SetIsDark")>
        <ScriptDescription("Sets if the current level needs to be lit up using Flash.")>
        Private Function M_SetIsDark(ByVal argument As String) As String
            Screen.Level.IsDark = Bool(argument)

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

        <ScriptCommand("SetRenderDistance")>
        <ScriptDescription("Sets the render distance of the current player.")>
        Private Function M_SetRenderDistance(ByVal argument As String) As String
            Select Case argument.ToLower()
                Case "0", "tiny"
                    Game.Core.GameOptions.RenderDistance = 0
                Case "1", "small"
                    Game.Core.GameOptions.RenderDistance = 1
                Case "2", "normal"
                    Game.Core.GameOptions.RenderDistance = 2
                Case "3", "far"
                    Game.Core.GameOptions.RenderDistance = 3
                Case "4", "extreme"
                    Game.Core.GameOptions.RenderDistance = 4
            End Select

            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("DayTime")>
        <ScriptDescription("Returns the current time of day.")>
        Private Function F_DayTime(ByVal argument As String) As String
            Return World.GetTime.ToString()
        End Function

        <ScriptConstruct("DayTimeID")>
        <ScriptDescription("Returns the daytime ID.")>
        Private Function F_DayTimeID(ByVal argument As String) As String
            Return ToString(CInt(World.GetTime))
        End Function

        <ScriptConstruct("Season")>
        <ScriptDescription("Returns the current season.")>
        Private Function F_Season(ByVal argument As String) As String
            Return World.CurrentSeason.ToString()
        End Function

        <ScriptConstruct("SeasonID")>
        <ScriptDescription("Returns the current season's ID.")>
        Private Function F_SeasonID(ByVal argument As String) As String
            Return ToString(CInt(World.CurrentSeason))
        End Function

        <ScriptConstruct("DayInformation")>
        <ScriptDescription("Returns information on the current day and time.")>
        Private Function F_DayInformation(ByVal argument As String) As String
            Return My.Computer.Clock.LocalTime.DayOfWeek.ToString() & "," &
                World.GetTime.ToString()
        End Function

        <ScriptConstruct("Weather")>
        <ScriptDescription("Returns the current map weather.")>
        Private Function F_Weather(ByVal argument As String) As String
            Return Screen.Level.World.CurrentMapWeather.ToString()
        End Function

        <ScriptConstruct("WeatherID")>
        <ScriptDescription("Returns the current map weather ID.")>
        Private Function F_WeatherID(ByVal argument As String) As String
            Return ToString(CInt(Screen.Level.World.CurrentMapWeather))
        End Function

        <ScriptConstruct("RegionWeather")>
        <ScriptDescription("Returns the region weather.")>
        Private Function F_RegionWeather(ByVal argument As String) As String
            Return World.GetCurrentRegionWeather().ToString()
        End Function

        <ScriptConstruct("RegionWeatherID")>
        <ScriptDescription("Returns the region weather ID.")>
        Private Function F_RegionWeatherID(ByVal argument As String) As String
            Return ToString(CInt(World.GetCurrentRegionWeather()))
        End Function

        <ScriptConstruct("CanFly")>
        <ScriptDescription("Returns if the player can Fly in the current map.")>
        Private Function F_CanFly(ByVal argument As String) As String
            Return ToString(Screen.Level.CanFly)
        End Function

        <ScriptConstruct("CanDig")>
        <ScriptDescription("Returns if the player can Dig in the current map.")>
        Private Function F_CanDig(ByVal argument As String) As String
            Return ToString(Screen.Level.CanDig)
        End Function

        <ScriptConstruct("CanTeleport")>
        <ScriptDescription("Returns if the player can Teleport in the current map.")>
        Private Function F_CanTeleport(ByVal argument As String) As String
            Return ToString(Screen.Level.CanTeleport)
        End Function

        <ScriptConstruct("WildPokemonGrass")>
        <ScriptDescription("Returns if the player can meet wild Pokémon in grass.")>
        Private Function F_WildPokemonGrass(ByVal argument As String) As String
            Return ToString(Screen.Level.WildPokemonGrass)
        End Function

        <ScriptConstruct("WildPokemonWater")>
        <ScriptDescription("Returns if the player can meet wild Pokémon while surfing.")>
        Private Function F_WildPokemonWater(ByVal argument As String) As String
            Return ToString(Screen.Level.WildPokemonWater)
        End Function

        <ScriptConstruct("WildPokemonEverywhere")>
        <ScriptDescription("Returns if the player can meet wild Pokémon on every floor tile.")>
        Private Function F_WildPokemonEverywhere(ByVal argument As String) As String
            Return ToString(Screen.Level.WildPokemonFloor)
        End Function

        <ScriptConstruct("IsDark")>
        <ScriptDescription("Returns if the level is lit up right now.")>
        Private Function F_IsDark(ByVal argument As String) As String
            Return ToString(Screen.Level.IsDark)
        End Function

#End Region

    End Class

End Namespace