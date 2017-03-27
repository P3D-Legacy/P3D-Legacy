Namespace Construct.Framework.Classes

    <ScriptClass("HallOfFame")>
    <ScriptDescription("A class to access the Hall of Fame of the game.")>
    Public Class CL_HallOfFame

        Inherits ScriptClass

        <ScriptCommand("Register")>
        <ScriptDescription("Registers a new Hall Of Fame entry.")>
        Private Function M_Register(ByVal argument As String) As String
            Dim count As Integer = -1

            If Game.Core.Player.HallOfFameData <> "" Then
                Dim data() As String = Game.Core.Player.HallOfFameData.SplitAtNewline()

                For Each l As String In data
                    Dim id As Integer = CInt(l.Remove(l.IndexOf(",")))
                    If id > count Then
                        count = id
                    End If
                Next
            End If

            count += 1

            Dim time As String = TimeHelpers.GetDisplayTime(TimeHelpers.GetCurrentPlayTime(), True)

            Dim newData As String

            If Game.Core.Player.IsGameJoltSave Then
                newData = count & ",(" & Game.Core.Player.Name & "|" & time & "|" & GameJoltSave.Points & "|" & Game.Core.Player.OT & "|" & Game.Core.Player.Skin & ")"
            Else
                newData = count & ",(" & Game.Core.Player.Name & "|" & time & "|" & Game.Core.Player.Points & "|" & Game.Core.Player.OT & "|" & Game.Core.Player.Skin & ")"
            End If

            For Each p As Pokemon In Game.Core.Player.Pokemons
                If p.IsEgg() = False Then
                    Dim pData As String = p.GetSaveData()
                    newData &= vbNewLine & count & "," & pData
                End If
            Next

            If Game.Core.Player.HallOfFameData <> "" Then
                Game.Core.Player.HallOfFameData &= vbNewLine
            End If

            Game.Core.Player.HallOfFameData &= newData

            Return Core.Null
        End Function

        <ScriptConstruct("Count")>
        <ScriptDescription("Returns the amount of hall of fame entries.")>
        Private Function F_Count(ByVal argument As String) As String
            Return ToString(HallOfFameScreen.GetHallOfFameCount())
        End Function

    End Class

End Namespace