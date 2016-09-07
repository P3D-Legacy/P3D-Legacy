Namespace BattleSystem

    Public Class PlaySoundQueryObject

        Inherits QueryObject

        Dim _sound As String = ""
        Dim _isPokemonSound As Boolean = False
        Dim _delay As Single = 0.0F

        Public Sub New(ByVal sound As String, ByVal isPokemonSound As Boolean)
            Me.New(sound, isPokemonSound, 0.0F)
        End Sub

        Public Sub New(ByVal sound As String, ByVal isPokemonSound As Boolean, ByVal delay As Single)
            MyBase.New(QueryTypes.PlaySound)

            Me._sound = sound
            Me._isPokemonSound = isPokemonSound
            Me._delay = delay
            Me.PassThis = True
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If Me._delay > 0.0F Then
                Me._delay -= 0.1F
                If Me._delay <= 0.0F Then
                    Me.InternalPlaySound()
                End If
            Else
                Me.InternalPlaySound()
            End If
        End Sub

        Private Sub InternalPlaySound()
            If _isPokemonSound = True Then
                SoundManager.PlayPokemonCry(CInt(_sound))
            Else
                SoundManager.PlaySound(Me._sound, False)
            End If
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return Me._delay <= 0.0F
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Dim d() As String = input.Split(CChar("|"))

            Return New PlaySoundQueryObject(d(0), CBool(d(1)), CSng(d(2).Replace(".", GameController.DecSeparator)))
        End Function

        Public Overrides Function ToString() As String
            Return "{SOUND|" & Me._sound & "|" & Me._isPokemonSound.ToNumberString() & "|" & Me._delay.ToString().Replace(GameController.DecSeparator, ".") & "}"
        End Function

    End Class

End Namespace