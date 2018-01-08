Imports Kolben
Imports Kolben.Adapters
Imports Kolben.Types

Namespace Scripting.V3.ApiClasses

    <ApiClass("Text")>
    Friend NotInheritable Class Text

        Inherits ApiClass

        <ApiClassMethod>
        Public Shared Function show(processor As ScriptProcessor, parameters As SObject()) As SObject

            Dim netObjects As Object() = Nothing
            If EnsureTypeContract(parameters, {GetType(String)}, netObjects) Then

                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(CType(netObjects(0), String), {}, False, False)

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return ScriptInAdapter.GetUndefined(processor)

        End Function

        <ApiClassMethod>
        Public Shared Function setFont(processor As ScriptProcessor, parameters As SObject()) As SObject

            Dim netObjects = New Object(0) {}
            If EnsureTypeContract(parameters, {GetType(String)}, netObjects) Then

                Dim fontName = CType(netObjects(0), String)
                Dim f As FontContainer = FontManager.GetFontContainer(fontName)
                If Not f Is Nothing Then
                    Screen.TextBox.TextFont = f
                Else
                    Screen.TextBox.TextFont = FontManager.GetFontContainer("textfont")
                End If

            End If

            Return ScriptInAdapter.Translate(processor, Screen.TextBox.TextFont.FontName)

        End Function

        <ApiClassMethod>
        Public Shared Function debug(processor As ScriptProcessor, parameters As SObject()) As SObject

            Dim netObjects = New Object(0) {}
            If EnsureTypeContract(parameters, {GetType(String)}, netObjects) Then

                Dim text = CType(netObjects(0), String)
                Logger.Debug("DEBUG: " & text)

            End If

            Return ScriptInAdapter.GetUndefined(processor)

        End Function

        <ApiClassMethod>
        Public Shared Function log(processor As ScriptProcessor, parameters As SObject()) As SObject

            Dim netObjects = New Object(0) {}
            If EnsureTypeContract(parameters, {GetType(String)}, netObjects) Then

                Dim text = CType(netObjects(0), String)
                Logger.Log(Logger.LogTypes.Debug, text)

            End If

            Return ScriptInAdapter.GetUndefined(processor)

        End Function

        <ApiClassMethod>
        Public Shared Function setColor(processor As ScriptProcessor, parameters As SObject()) As SObject

            Dim netObjects As Object() = Nothing
            If EnsureTypeContract(parameters, {GetType(String)}, netObjects) Then

                Dim colorType = CType(netObjects(0), String)

                Select Case colorType.ToLowerInvariant()
                    Case "playercolor", "player"
                        Screen.TextBox.TextColor = TextBox.PlayerColor
                    Case "defaultcolor", "default"
                        Screen.TextBox.TextColor = TextBox.DefaultColor
                    Case Else 'Try to convert the input color name into a color: (https://msdn.microsoft.com/en-us/library/system.drawing.knowncolor%28v=vs.110%29.aspx)
                        Screen.TextBox.TextColor = Drawing.Color.FromName(colorType).ToXNA()
                End Select

            ElseIf EnsureTypeContract(parameters, {GetType(Integer), GetType(Integer), GetType(Integer), GetType(Integer)}, netObjects, 1) Then

                Dim helper = New ParamHelper(netObjects)

                Dim r = helper.Pop(Of Integer)
                Dim g = helper.Pop(Of Integer)
                Dim b = helper.Pop(Of Integer)
                Dim a = helper.Pop(255)

                Screen.TextBox.TextColor = New Color(r, g, b, a)

            End If

            Return ScriptInAdapter.GetUndefined(processor)

        End Function

    End Class

End Namespace
