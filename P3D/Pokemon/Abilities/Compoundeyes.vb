Namespace Abilities

    Public Class Compoundeyes

        Inherits Ability

        Public Sub New()
            MyBase.New(14, "Compoundeyes", "The Pokémon's accuracy is boosted.")
        End Sub

        Public Shared Function ConvertItemChances(ByVal WildItems As Dictionary(Of Integer, Integer)) As Dictionary(Of Integer, Integer)
            Dim d As New Dictionary(Of Integer, Integer)

            For i = 0 To WildItems.Count - 1
                Dim chance As Integer = WildItems.Keys(i)
                Dim itemID As Integer = WildItems.Values(i)

                Select Case chance
                    Case 50
                        chance = 60
                    Case 5
                        chance = 20
                    Case 1
                        chance = 5
                End Select

                d.Add(chance, itemID)
            Next
            Return d
        End Function

    End Class

End Namespace