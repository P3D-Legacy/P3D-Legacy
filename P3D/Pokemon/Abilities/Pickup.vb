Namespace Abilities

    Public Class Pickup

        Inherits Ability

        Public Sub New()
            MyBase.New(53, "Pickup", "The Pokémon may pick up items.")
        End Sub

        Public Shared Sub Pickup()
            For Each p As Pokemon In Core.Player.Pokemons
                If p.Ability.Name.ToLower() = "pickup" Then
                    If p.Item Is Nothing Then
                        Dim chance As Integer = Core.Random.Next(0, 100)
                        Dim itemList As New List(Of Integer)
                        If chance < 30 Then
                            itemList = Get30(p)
                        ElseIf chance >= 30 And chance < 40 Then
                            itemList = Get10(p)
                        ElseIf chance >= 40 And chance < 44 Then
                            itemList = Get4(p)
                        ElseIf chance = 44 Then
                            itemList = Get1(p)
                        End If
                        If itemList.Count > 0 Then
                            p.Item = Item.GetItemByID(itemList(Core.Random.Next(0, itemList.Count)))
                        End If
                    End If
                End If
            Next
        End Sub

        Private Shared Function GetLevelStep(ByVal p As Pokemon) As Integer
            Return CInt(Math.Ceiling(p.Level / 10))
        End Function

        Private Shared Function Get30(ByVal p As Pokemon) As List(Of Integer)
            Dim l As New List(Of Integer)
            Select Case GetLevelStep(p)
                Case 1
                    l.Add(18)
                Case 2
                    l.Add(9)
                Case 3
                    l.Add(17)
                Case 4
                    l.Add(4)
                Case 5
                    l.Add(20)
                Case 6
                    l.Add(19)
                Case 7
                    l.Add(38)
                Case 8
                    l.Add(16)
                Case 9
                    l.Add(2)
                Case 10
                    l.Add(39)
            End Select
            Return l
        End Function

        Private Shared Function Get10(ByVal p As Pokemon) As List(Of Integer)
            Dim l As New List(Of Integer)
            Select Case GetLevelStep(p)
                Case 1
                    l.AddRange({9, 17, 4, 20, 19, 38})
                Case 2
                    l.AddRange({17, 4, 20, 19, 38, 16})
                Case 3
                    l.AddRange({4, 20, 19, 38, 16, 2})
                Case 4
                    l.AddRange({20, 19, 38, 16, 2, 39})
                Case 5
                    l.AddRange({19, 38, 16, 2, 39, 32})
                Case 6
                    l.AddRange({38, 16, 2, 39, 32, 169})
                Case 7
                    l.AddRange({16, 2, 39, 32, 169, 8})
                Case 8
                    l.AddRange({2, 39, 32, 169, 8, 190})
                Case 9
                    l.AddRange({39, 32, 169, 8, 190, 14})
                Case 10
                    l.AddRange({32, 169, 8, 190, 14, 40})
            End Select
            Return l
        End Function

        Private Shared Function Get4(ByVal p As Pokemon) As List(Of Integer)
            Dim l As New List(Of Integer)
            Select Case GetLevelStep(p)
                Case 1
                    l.AddRange({16, 2})
                Case 2
                    l.AddRange({2, 39})
                Case 3
                    l.AddRange({39, 32})
                Case 4
                    l.AddRange({32, 169})
                Case 5
                    l.AddRange({169, 8})
                Case 6
                    l.AddRange({8, 190})
                Case 7
                    l.AddRange({190, 14})
                Case 8
                    l.AddRange({14, 40})
                Case 9
                    l.AddRange({40, 62})
                Case 10
                    l.AddRange({62, 21})
            End Select
            Return l
        End Function

        Private Shared Function Get1(ByVal p As Pokemon) As List(Of Integer)
            Dim l As New List(Of Integer)
            Select Case GetLevelStep(p)
                Case 1
                    l.AddRange({36, 32})
                Case 2
                    l.AddRange({32, 82})
                Case 3
                    l.AddRange({82, 14})
                Case 4
                    l.AddRange({14, 63})
                Case 5
                    l.AddRange({63, 28})
                Case 6
                    l.AddRange({28, 83})
                Case 7
                    l.AddRange({83, 112})
                Case 8
                    l.AddRange({65, 83})
                Case 9
                    l.AddRange({83, 146})
                Case 10
                    l.AddRange({146, 83})
            End Select
            Return l
        End Function

    End Class

End Namespace