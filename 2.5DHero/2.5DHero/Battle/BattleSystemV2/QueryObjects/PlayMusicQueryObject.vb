Namespace BattleSystem

    Public Class PlayMusicQueryObject

        Inherits QueryObject

        Dim _music As String = ""
        Dim fade As Boolean = False

        Public Sub New(ByVal music As String)
            MyBase.New(QueryTypes.PlayMusic)

            Me._music = music
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If fade = True Then
                MusicPlayer.GetInstance().Play(Me._music, True)
            Else
                MusicPlayer.GetInstance().Play(Me._music, True, 0.0F, 0.0F)
            End If
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Return New PlayMusicQueryObject(input)
        End Function

        Public Overrides Function ToString() As String
            Return "{MUSIC|" & Me._music & "}"
        End Function

    End Class

End Namespace