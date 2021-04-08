Imports System.Runtime.InteropServices

Public Module RPC

    Public Const DLL_NAME As String = "discord-rpc.dll"

    <StructLayout(LayoutKind.Sequential)>
    Public Structure DiscordRichPresence
        Public State As String
        Public Details As String
        Public StartTimestamp As Long
        Public EndTimestamp As Long
        Public LargeImageKey As String
        Public LargeImageText As String
        Public SmallImageKey As String
        Public SmallImageText As String
        Public PartyID As String
        Public PartySize As Integer
        Public PartyMax As Integer
        Public MatchSecret As String
        Public JoinSecret As String
        Public SpectateSecret As String
        Public Instance As Boolean
    End Structure

    Public Enum DiscordReply
        No
        Yes
        Ignore
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure DiscordUser
        Public UserID As String
        Public Username As String
        Public Discriminator As String
        Public Avatar As String
    End Structure

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Delegate Sub Discord_Ready(ByRef Request As DiscordUser)

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Delegate Sub Discord_Disconnected(ByVal ErrorCode As Integer, ByVal Message As String)

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Delegate Sub Discord_Errored(ByVal ErrorCode As Integer, ByVal Message As String)

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Delegate Sub Discord_JoinGame(ByVal JoinSecret As String)

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Delegate Sub Discord_SpectateGame(ByVal SpectateSecret As String)

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Delegate Sub Discord_JoinRequest(ByRef Request As DiscordUser)

    <StructLayout(LayoutKind.Sequential)>
    Public Structure DiscordEventHandlers
        Public Ready As Discord_Ready
        Public Disconnected As Discord_Disconnected
        Public Errored As Discord_Errored
        Public JoinGame As Discord_JoinGame
        Public SpectateGame As Discord_SpectateGame
        Public JoinRequest As Discord_JoinRequest
    End Structure

    <DllImport(DLL_NAME,
               EntryPoint:="Discord_Initialize",
               CallingConvention:=CallingConvention.Cdecl)>
    Public Sub Discord_Initialize(ByVal ApplicationID As String,
                                       ByRef Handlers As DiscordEventHandlers,
                                       ByVal AutoRegister As Integer,
                                       ByVal OptionalSteamID As String)
    End Sub

    <DllImport(DLL_NAME,
               EntryPoint:="Discord_Shutdown",
               CallingConvention:=CallingConvention.Cdecl)>
    Public Sub Discord_Shutdown()
    End Sub

    <DllImport(DLL_NAME,
               EntryPoint:="Discord_RunCallbacks",
               CallingConvention:=CallingConvention.Cdecl)>
    Public Sub Discord_RunCallbacks()
    End Sub

    <DllImport(DLL_NAME,
               EntryPoint:="Discord_UpdateConnection",
               CallingConvention:=CallingConvention.Cdecl)>
    Public Sub Discord_UpdateConnection()
    End Sub

    <DllImport(DLL_NAME,
               EntryPoint:="Discord_UpdatePresence",
               CallingConvention:=CallingConvention.Cdecl)>
    Public Sub Discord_UpdatePresence(ByRef Presence As DiscordRichPresence)
    End Sub

    <DllImport(DLL_NAME,
               EntryPoint:="Discord_ClearPresence",
               CallingConvention:=CallingConvention.Cdecl)>
    Public Sub Discord_ClearPresence()
    End Sub

    <DllImport(DLL_NAME,
               EntryPoint:="Discord_Respond",
               CallingConvention:=CallingConvention.Cdecl)>
    Public Sub Discord_Respond(ByVal UserID As String,
                                    ByVal Reply As DiscordReply)
    End Sub

    <DllImport(DLL_NAME,
               EntryPoint:="Discord_UpdateHandlers",
               CallingConvention:=CallingConvention.Cdecl)>
    Public Sub Discord_UpdateHandlers(ByRef Handlers As DiscordEventHandlers)
    End Sub

End Module