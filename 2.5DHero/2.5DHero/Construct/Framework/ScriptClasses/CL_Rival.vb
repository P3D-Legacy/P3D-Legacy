Namespace Construct.Framework.Classes

    <ScriptClass("Rival")>
    <ScriptDescription("A class to access the rival of the game.")>
    Public Class CL_Rival

        Inherits ScriptClass

        <ScriptCommand("Rename")>
        <ScriptDescription("Opens the ""Rename Rival"" screen.")>
        Private Function M_Rename(ByVal argument As String) As String
            Game.Core.SetScreen(
                    New NameObjectScreen(Game.Core.CurrentScreen,
                                         TextureManager.GetTexture("NPC\4", New Rectangle(0, 64, 32, 32)),
                                         False,
                                         False,
                                         "rival",
                                         "Silver",
                                         Sub(Name As String)
                                             Game.Core.Player.RivalName = Name
                                         End Sub
                                        )
                                    )

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("SetName")>
        <ScriptDescription("Sets a name for the rival.")>
        Private Function M_SetName(ByVal argument As String) As String
            Game.Core.Player.RivalName = argument
            Return Core.Null
        End Function

        <ScriptConstruct("Name")>
        <ScriptDescription("Returns the name of the rival.")>
        Private Function F_Name(ByVal argument As String) As String
            Return Game.Core.Player.RivalName
        End Function

    End Class

End Namespace