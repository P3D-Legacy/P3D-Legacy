Namespace Construct.Framework.Classes

    <ScriptClass("Title")>
    <ScriptDescription("A class to display titles on the screen.")>
    Public Class CL_Title

        Inherits ScriptClass

        <ScriptCommand("Add", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Adds a new title to the screen.")>
        Private Function M_Add(ByVal argument As String) As String
            Dim t As New OverworldScreen.Title()

            Dim args As String() = argument.Split(CChar(","))
            For i = 0 To args.Count - 1
                Dim arg As String = args(i)
                Select Case i
                    Case 0 'text
                        t.Text = arg
                    Case 1 'delay
                        t.Delay = Sng(arg)
                    Case 2 'R
                        t.TextColor = New Color(CByte(Int(arg).Clamp(0, 255)), t.TextColor.G, t.TextColor.B)
                    Case 3 'G
                        t.TextColor = New Color(t.TextColor.R, CByte(Int(arg).Clamp(0, 255)), t.TextColor.B)
                    Case 4 'B
                        t.TextColor = New Color(t.TextColor.R, t.TextColor.G, CByte(Int(arg).Clamp(0, 255)))
                    Case 5 'Scale
                        t.Scale = Sng(arg)
                    Case 6 'IsCentered
                        t.IsCentered = Bool(arg)
                    Case 7 'X
                        t.Position = New Vector2(Sng(arg), t.Position.Y)
                    Case 8 'Y
                        t.Position = New Vector2(t.Position.X, Sng(arg))
                End Select
            Next

            CType(Game.Core.CurrentScreen, OverworldScreen).Titles.Add(t)

            Return Core.Null
        End Function

        <ScriptCommand("Clear", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Clears all titles from the screen.")>
        Private Function M_Clear(ByVal argument As String) As String
            CType(Game.Core.CurrentScreen, OverworldScreen).Titles.Clear()

            Return Core.Null
        End Function

    End Class

End Namespace