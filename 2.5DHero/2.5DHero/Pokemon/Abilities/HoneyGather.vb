Namespace Abilities

    Public Class HoneyGather

        Inherits Ability

        Public Sub New()
            MyBase.New(118, "Honey Gather", "The Pokémon may gather Honey from somewhere.")
        End Sub

        Public Shared Sub GatherHoney()
            For Each p As Pokemon In Core.Player.Pokemons
                If p.Ability.Name.ToLower() = "honey gather" Then
                    If p.Item Is Nothing Then
                        Dim chance As Integer = CInt(Math.Ceiling(p.Level / 10) * 5)
                        If Core.Random.Next(0, 100) < chance Then
                            p.Item = Item.GetItemByID(253)
                        End If
                    End If
                End If
            Next
        End Sub

    End Class

End Namespace