Namespace Construct.Framework.Classes

    <ScriptClass("Music")>
    <ScriptDescription("A class to control music playback.")>
    Public Class CL_Music

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("Play")>
        <ScriptDescription("Plays music and sets the music loop.")>
        Private Function M_Play(ByVal argument As String) As String
            MusicPlayer.GetInstance().Play(argument, True)

            If Screen.Level IsNot Nothing Then
                Screen.Level.MusicLoop = argument
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetMusicLoop")>
        <ScriptDescription("Sets the music loop without playing music.")>
        Private Function M_SetMusicLoop(ByVal argument As String) As String
            If Screen.Level IsNot Nothing Then
                Screen.Level.MusicLoop = argument
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Stop")>
        <ScriptDescription("Stops music playback.")>
        Private Function M_Stop(ByVal argument As String) As String
            MusicPlayer.GetInstance().Stop()

            Return Core.Null
        End Function

        <ScriptCommand("Pause")>
        <ScriptDescription("Pauses music playback.")>
        Private Function M_Pause(ByVal argument As String) As String
            MusicPlayer.GetInstance().Pause()

            Return Core.Null
        End Function

        <ScriptCommand("Resume")>
        <ScriptDescription("Resumes music playback.")>
        Private Function M_Resume(ByVal argument As String) As String
            MusicPlayer.GetInstance().Resume()

            Return Core.Null
        End Function

#End Region

    End Class

End Namespace