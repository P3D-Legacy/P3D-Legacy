﻿Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <battle> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoBattle(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower(Globalization.CultureInfo.InvariantCulture)
                Case "defeatmessage"
                    Dim t As New Trainer(argument)

                    Return t.DefeatMessage
                Case "intromessage"
                    Dim t As New Trainer(argument)

                    Return t.IntroMessage
                Case "outromessage"
                    Dim t As New Trainer(argument)

                    Return t.OutroMessage
                Case "won"
                    Return ReturnBoolean(BattleSystem.Battle.Won)
                Case "caught"
                    Return ReturnBoolean(BattleSystem.Battle.Caught)
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace