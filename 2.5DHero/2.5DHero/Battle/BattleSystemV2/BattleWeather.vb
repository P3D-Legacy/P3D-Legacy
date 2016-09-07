Public Class BattleWeather

    Public Enum WeatherTypes As Integer
        Clear = 0
        Rain = 1
        Sunny = 2
        Sandstorm = 3
        Hailstorm = 4
        Foggy = 5
        Snow = 6
    End Enum

    Public Shared Function GetWorldWeather(ByVal FieldWeather As BattleWeather.WeatherTypes) As World.Weathers
        Select Case FieldWeather
            Case WeatherTypes.Clear
                Return World.Weathers.Clear
            Case WeatherTypes.Foggy
                Return World.Weathers.Fog
            Case WeatherTypes.Hailstorm
                Return World.Weathers.Blizzard
            Case WeatherTypes.Rain
                Return World.Weathers.Rain
            Case WeatherTypes.Sandstorm
                Return World.Weathers.Sandstorm
            Case WeatherTypes.Sunny
                Return World.Weathers.Sunny
            Case WeatherTypes.Snow
                Return World.Weathers.Snow
            Case Else
                Return World.Weathers.Clear
        End Select
    End Function

    Public Shared Function GetBattleWeather(ByVal WorldWeather As World.Weathers) As BattleWeather.WeatherTypes
        Select Case WorldWeather
            Case World.Weathers.Blizzard
                Return WeatherTypes.Hailstorm
            Case World.Weathers.Snow
                Return WeatherTypes.Snow
            Case World.Weathers.Clear
                Return WeatherTypes.Clear
            Case World.Weathers.Fog
                Return WeatherTypes.Foggy
            Case World.Weathers.Rain, World.Weathers.Thunderstorm
                Return WeatherTypes.Rain
            Case World.Weathers.Sandstorm
                Return WeatherTypes.Sandstorm
            Case World.Weathers.Sunny
                Return WeatherTypes.Sunny
            Case Else
                Return WeatherTypes.Clear
        End Select
    End Function

End Class