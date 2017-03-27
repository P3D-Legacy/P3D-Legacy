Namespace Construct.Framework.Classes

    <ScriptClass("Text")>
    <ScriptDescription("A class to work with textboxes in the game.")>
    Public Class CL_Text

        Inherits ScriptClass

        Public Sub New()
            MyBase.New(True)
        End Sub

#Region "Commands"

        <ScriptCommand("Show")>
        <ScriptDescription("Displays a text box with the given text.")>
        Private Function M_Show(ByVal argument As String) As String
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(argument, {}, False, False)

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("SetFont")>
        <ScriptDescription("Sets the font of the next textbox. The font will reset after the next textbox.")>
        Private Function M_SetFont(ByVal argument As String) As String
            Dim f As FontContainer = FontManager.GetFontContainer(argument)
            If Not f Is Nothing Then
                Screen.TextBox.TextFont = f
            Else
                Screen.TextBox.TextFont = FontManager.GetFontContainer("textfont")
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Debug")>
        <ScriptDescription("Prints a text to the debug log.")>
        Private Function M_Debug(ByVal argument As String) As String
            Logger.Debug("024", "DEBUG: " & argument.ToString())

            Return Core.Null
        End Function

        <ScriptCommand("Log")>
        <ScriptDescription("Prints a text to the output log.")>
        Private Function M_Log(ByVal argument As String) As String
            Logger.Log("182", Logger.LogTypes.Debug, argument)

            Return Core.Null
        End Function

        <ScriptCommand("Color")>
        <ScriptDescription("Sets the color of the text in the textbox.")>
        Private Function M_Color(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(","))

            If args.Length = 1 Then
                Select Case args(0).ToLower()
                    Case "playercolor", "player"
                        Screen.TextBox.TextColor = TextBox.PlayerColor
                    Case "defaultcolor", "default"
                        Screen.TextBox.TextColor = TextBox.DefaultColor
                    Case Else 'Try to convert the input color name into a color: (https://msdn.microsoft.com/en-us/library/system.drawing.knowncolor%28v=vs.110%29.aspx)
                        Screen.TextBox.TextColor = Drawing.Color.FromName(args(0)).ToXNA()
                End Select
            ElseIf args.Length = 3 Then
                Screen.TextBox.TextColor = New Color(Int(args(0)), Int(args(1)), Int(args(2)))
            End If

            Return Core.Null
        End Function

#End Region

    End Class

End Namespace