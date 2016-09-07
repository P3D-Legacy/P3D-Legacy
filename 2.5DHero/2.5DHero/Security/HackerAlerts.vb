Namespace Security

    Public Class HackerAlerts

        Shared t As Timers.Timer

        Public Shared Sub Activate()
            t = New Timers.Timer()
            AddHandler t.Elapsed, AddressOf TimerTick
            t.AutoReset = True
            t.Interval = 25000
            t.Start()
        End Sub

        Private Shared Sub TimerTick()
            t.Stop()
            Dim text As String = "Hey, we have detected that you attempted to hack the game. Please don't do that. We are trying to create an online experience where everyone is set equal, where everyone can play the game and achieve about the same things by playing the same amount of time.
So please reconsider utilizing certain RAM editing tools in Pokémon3D.
Of course, you are free to use those whenever you want in Offline Mode, since that is essentially a Sandbox for you to play around in.
To remove these messages and the reverse texture effects, contact a staff member on the forums here: 

http://www.pokemon3d.net/forum/

Thank you for your cooperation."

            MsgBox(text, MsgBoxStyle.OkOnly, "Pokémon3D Injection Shield")

            t.Start()
        End Sub

    End Class

End Namespace