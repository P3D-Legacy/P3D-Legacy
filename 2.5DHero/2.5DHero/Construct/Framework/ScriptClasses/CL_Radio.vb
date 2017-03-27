Namespace Construct.Framework.Classes

    <ScriptClass("Radio")>
    <ScriptDescription("A class to use the Pokégear's radio.")>
    Public Class CL_Radio

        Inherits ScriptClass

        <ScriptCommand("AllowChannel")>
        <ScriptDescription("Allows a channel to be played.")>
        Private Function M_AllowChannel(ByVal argument As String) As String
            Screen.Level.AllowedRadioChannels.Add(CDec(Dbl(argument)))
            Return Core.Null
        End Function

        <ScriptCommand("BlockChannel")>
        <ScriptDescription("Blocks a channel.")>
        Private Function M_BlockChannel(ByVal argument As String) As String
            Dim d As Decimal = CDec(Dbl(argument))
            If Screen.Level.AllowedRadioChannels.Contains(d) = True Then
                Screen.Level.AllowedRadioChannels.Remove(d)
            End If

            Return Core.Null
        End Function

        <ScriptConstruct("CurrentChannel")>
        <ScriptDescription("Returns the currently played channel.")>
        Private Function F_CurrentChannel(ByVal argument As String) As String
            If Screen.Level.SelectedRadioStation Is Nothing Then
                Return Core.Null
            Else
                Return Screen.Level.SelectedRadioStation.Name
            End If
        End Function

    End Class

End Namespace