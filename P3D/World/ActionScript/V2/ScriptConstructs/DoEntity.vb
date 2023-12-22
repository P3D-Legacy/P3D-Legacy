Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <entity> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoEntity(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Dim entID As Integer = int(argument.GetSplit(0))
            Dim ent As Entity = Screen.Level.GetEntity(entID)

            If Not ent Is Nothing Then
                Select Case command.ToLower()
                    Case "visible"
                        Return ReturnBoolean(ent.Visible)
                    Case "opacity"
                        Return ent.Opacity * 100
                    Case "position"
                        Return ent.Position.X.ToString().ReplaceDecSeparator & "," & ent.Position.Y.ToString().ReplaceDecSeparator & "," & ent.Position.Z.ToString().ReplaceDecSeparator
                    Case "positiony"
                        Return ent.Position.Y.ToString.ReplaceDecSeparator
                    Case "positionz"
                        Return ent.Position.Z.ToString.ReplaceDecSeparator
                    Case "positionx"
                        Return ent.Position.X.ToString.ReplaceDecSeparator
                    Case "rotation"
                        Return ent.Rotation.X.ToString() & "," & ent.Rotation.Y.ToString() & "," & ent.Rotation.Z.ToString()
                    Case "scale"
                        Return ent.Scale.X.ToString().ReplaceDecSeparator & "," & ent.Scale.Y.ToString().ReplaceDecSeparator & "," & ent.Scale.Z.ToString().ReplaceDecSeparator
                    Case "additionalvalue"
                        Return ent.AdditionalValue
                    Case "collision"
                        Return ReturnBoolean(ent.Collision)
                End Select
            End If

            Return DEFAULTNULL
        End Function

    End Class

End Namespace