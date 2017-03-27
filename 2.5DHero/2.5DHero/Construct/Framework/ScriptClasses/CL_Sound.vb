Namespace Construct.Framework.Classes

    <ScriptClass("Sound")>
    <ScriptDescription("Plays sounds.")>
    Public Class CL_Sound

        Inherits ScriptClass

        <ScriptCommand("Play")>
        <ScriptDescription("Plays a sound.")>
        Private Function M_Play(ByVal argument As String) As String
            Dim sound As String = argument
            Dim stopMusic As Boolean = False

            If argument.Contains(",") = True Then
                sound = argument.GetSplit(0)
                stopMusic = Bool(argument.GetSplit(1))
            End If

            SoundManager.PlaySound(sound, stopMusic)

            Return Core.Null
        End Function

        <ScriptCommand("PlayAdvanced")>
        <ScriptDescription("Plays a sound with advanced options.")>
        Private Function M_PlayAdvanced(ByVal argument As String) As String
            Dim args() As String = argument.Split(CChar(","))

            Dim sound As String = args(0)
            Dim stopMusic As Boolean = Bool(args(1))
            Dim pitch As Single = Sng(args(2))
            Dim pan As Single = Sng(args(3))
            Dim volume As Single = Sng(args(4))

            SoundManager.PlaySound(sound, pitch, pan, volume, stopMusic)

            Return Core.Null
        End Function

    End Class

End Namespace