Public Class Chat

    Public Const NEWMESSAGEDELAY As Single = 35.0F

    Private Shared _transferedLines As New List(Of ChatMessage)

    Public Shared Function TransferedLines(ByVal currentChatState As ChatScreen.ChatStates, ByVal currentPMChat As String) As List(Of ChatMessage)
        Dim l As New List(Of ChatMessage)
        Dim currentMessageArray As New List(Of ChatMessage)
        currentMessageArray.AddRange(_transferedLines.ToArray())

        Select Case currentChatState
            Case ChatScreen.ChatStates.Command
                For Each m In currentMessageArray
                    If m.MessageType = ChatMessage.MessageTypes.CommandMessage Then
                        l.Add(m)
                    End If
                Next
            Case ChatScreen.ChatStates.Global
                For Each m In currentMessageArray
                    If m.MessageType = ChatMessage.MessageTypes.GlobalMessage Then
                        l.Add(m)
                    End If
                Next
            Case ChatScreen.ChatStates.PM
                For Each m In currentMessageArray
                    If m.MessageType = ChatMessage.MessageTypes.PMMessage Then
                        If m.Sender = currentPMChat Or m.PMChatInclude = currentPMChat Then
                            l.Add(m)
                        End If
                    End If
                Next
        End Select
        Return l
    End Function

    Private Shared _serverColor As Color = Color.Orange
    Private Shared _friendColor As Color = Color.LightGreen
    Private Shared _ownColor As Color = New Color(39, 206, 249)
    Private Shared _pmColor As Color = Color.Violet

    Public Shared ReadOnly Property ServerColor() As Color
        Get
            Return Chat._serverColor
        End Get
    End Property

    Public Shared ReadOnly Property FriendColor() As Color
        Get
            Return Chat._friendColor
        End Get
    End Property

    Public Shared ReadOnly Property OwnColor() As Color
        Get
            Return Chat._ownColor
        End Get
    End Property

    Public Class ChatMessage

        Public Enum MessageTypes
            GlobalMessage
            PMMessage
            CommandMessage
        End Enum

        Public Sender As String = ""
        Public Message As String = ""
        Public GJID As String = ""
        Public MessageType As MessageTypes = MessageTypes.GlobalMessage
        Public PMChatInclude As String = ""

        Public Sub New(ByVal sender As String, ByVal message As String, ByVal GJID As String, ByVal MessageType As MessageTypes)
            Me.Sender = sender
            Me.Message = message
            Me.GJID = GJID
            Me.MessageType = MessageType
        End Sub

        Public Overrides Function ToString() As String
            If Sender = "" Then
                Return Me.Message
            Else
                If Sender = "[SERVER]" Then
                    Return Sender & ": " & Message
                Else
                    Return "<" & Sender & "> " & Message
                End If
            End If
        End Function

        Private Function IsFriend() As Boolean
            If GJID = "" Then
                Return False
            End If
            If Core.Player.IsGameJoltSave = True Then
                If Core.GameJoltSave.Friends.Contains(GJID) = True Then
                    Return True
                End If
            End If
            Return False
        End Function

        Private Function IsMe() As Boolean
            If Sender.ToLower() = Core.Player.Name.ToLower() Or PMChatInclude <> "" Then
                Return True
            End If
            Return False
        End Function

        Public Function MessageColor() As Color
            If Sender = "[SERVER]" Then
                Return ServerColor
            End If
            If Sender = "[HELP]" Then
                Return Color.White
            End If
            If IsMe() = True Then
                Return OwnColor
            End If
            If IsFriend() = True And Me.MessageType = MessageTypes.GlobalMessage Then
                Return FriendColor
            End If
            Return Color.White
        End Function

    End Class

    ''' <summary>
    ''' Writes the message into the chat and returns True, if the message was a command. Only sends non-command messages to servers.
    ''' </summary>
    Public Shared Sub WriteLine(ByVal chatMessage As ChatMessage)
        Logger.Log(Logger.LogTypes.Entry, "Chat.vb: " & chatMessage.Message)

        Dim commandReturn As String = RunCommand(chatMessage.Message, False)

        If commandReturn = "" Then
            If JoinServerScreen.Online = True Then
                Core.ServersManager.ServerConnection.SendChatMessage(chatMessage.Message)
            Else
                _transferedLines.Add(chatMessage)
                ChatScreen.InsertNewMessage(chatMessage)
            End If
        Else
            _transferedLines.Add(New ChatMessage("", commandReturn, "", ChatMessage.MessageTypes.CommandMessage))

            ChatScreen.InsertNewMessage(New ChatMessage("", commandReturn, "", ChatMessage.MessageTypes.CommandMessage))
        End If
    End Sub

    Public Shared Sub AddLine(ByVal chatMessage As ChatMessage)
        _transferedLines.Add(chatMessage)

        ChatScreen.InsertNewMessage(chatMessage)

        While ChatScreen.newMessages.Count > 4
            ChatScreen.newMessages.RemoveAt(4)
            ChatScreen.newMessagesDelays.RemoveAt(4)
        End While
    End Sub

    Public Shared Sub ClearChat()
        ChatScreen.ResetChatState()
        _transferedLines.Clear()
    End Sub

    Public Shared Sub ClearChatMessages(ByVal currentChatState As ChatScreen.ChatStates, ByVal currentPMChat As String)
        Dim deleteList As New List(Of ChatMessage)
        Select Case currentChatState
            Case ChatScreen.ChatStates.Command
                deleteList = (From m As ChatMessage In _transferedLines Select m Where m.MessageType = ChatMessage.MessageTypes.CommandMessage).ToList()
            Case ChatScreen.ChatStates.Global
                deleteList = (From m As ChatMessage In _transferedLines Select m Where m.MessageType = ChatMessage.MessageTypes.GlobalMessage).ToList()
            Case ChatScreen.ChatStates.PM
                deleteList = (From m As ChatMessage In _transferedLines Select m Where m.MessageType = ChatMessage.MessageTypes.PMMessage And (m.Sender = currentPMChat Or m.PMChatInclude = currentPMChat)).ToList()
        End Select

        If deleteList.Count > 0 Then
            For Each m In deleteList
                _transferedLines.Remove(m)
            Next
        End If
    End Sub

    Public Shared Function IsCommandMessage(ByVal text As String) As Boolean
        Return RunCommand(text, True) <> ""
    End Function

    Private Shared Function RunCommand(ByVal text As String, ByVal testForScript As Boolean) As String
        If GameController.IS_DEBUG_ACTIVE = True Or (Core.Player.IsGameJoltSave = False And Core.Player.SandBoxMode = True) Then
            Select Case text(0).ToString().ToLower()
                Case "@"
                    Try
                        Dim s As Screen = Core.CurrentScreen
                        While Not s.PreScreen Is Nothing And s.Identification <> Screen.Identifications.OverworldScreen
                            s = s.PreScreen
                        End While

                        If s.Identification = Screen.Identifications.OverworldScreen Then
                            If testForScript = False Then
                                CType(s, OverworldScreen).ActionScript.StartScript("version=2" & vbNewLine & text & vbNewLine & ":end", 2, False)
                            End If

                            Return text
                        Else
                            Return "Invalid script environment."
                        End If
                    Catch ex As Exception
                        Return "Invalid script: """ & text & """"
                    End Try
                Case "<"
                    Try
                        Return text & ": " & ScriptVersion2.ScriptComparer.EvaluateConstruct(text).ToString()
                    Catch ex As Exception
                        Return "Invalid construct: """ & text & """"
                    End Try
                Case "#"
                    Return text.Remove(0, 1)
            End Select
        End If

        Return ""
    End Function

End Class