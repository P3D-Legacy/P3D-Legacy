﻿Imports System.Windows.Forms
Imports GameDevCommon

Public Class Classified
    Public Shared Remote_Texture_URL As String = "" ' CLASSIFIED
    Public Const GameJolt_Game_ID As String = "" ' CLASSIFIED
    Public Const GameJolt_Game_Key As String = "" ' CLASSIFIED
    Public Shared Encryption_Password As String = "" ' CLASSIFIED
    Public Shared Discord_Application_ID As String = "" ' CLASSIFIED
End Class

''' <summary>
''' Controls the game's main workflow.
''' </summary>
Public Class GameController

    Inherits Microsoft.Xna.Framework.Game
    Implements IGame

    ''' <summary>
    ''' The current version of the game.
    ''' </summary>
    Public Const GAMEVERSION As String = "0.58.1"

    ''' <summary>
    ''' The number of released iterations of the game.
    ''' </summary>
    Public Const RELEASEVERSION As String = "101"

    ''' <summary>
    ''' The development stage the game is in.
    ''' </summary>
    Public Const GAMEDEVELOPMENTSTAGE As String = "Indev"

    ''' <summary>
    ''' The name of the game.
    ''' </summary>
    Public Const GAMENAME As String = "Pokémon 3D"

    ''' <summary>
    ''' The name of the developer that appears on the title screen.
    ''' </summary>
    Public Const DEVELOPER_NAME As String = "P3D Team"

    ''' <summary>
    ''' If the Debug Mode is active.
    ''' </summary>
#If DEBUG Or DEBUGNOCONTENT Then
    Public Const IS_DEBUG_ACTIVE As Boolean = True
#Else
    Public Const IS_DEBUG_ACTIVE As Boolean = False
#End If

    ''' <summary>
    ''' If the game should set the GameJolt online version to the current online version.
    ''' </summary>
    Public Const UPDATEONLINEVERSION As Boolean = False

    Public Graphics As GraphicsDeviceManager
    Public FPSMonitor As FPSMonitor

    Private window_change As Boolean = False
    Public Shared UpdateChecked As Boolean = False

    Private _componentManager As ComponentManager

    Public Sub New()
        Me.window_change = False
        Graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"

        Window.AllowUserResizing = True
        AddHandler Window.ClientSizeChanged, AddressOf Window_ClientSizeChanged

        'Dim gameForm As Form = CType(Form.FromHandle(Window.Handle), Form)
        'gameForm.MinimumSize = New System.Drawing.Size(600, 360)

        FPSMonitor = New FPSMonitor()

        GameHacked = System.IO.File.Exists(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\temp")
        If GameHacked = True Then
            Security.HackerAlerts.Activate()
        End If

        _componentManager = New ComponentManager()
        GameInstanceProvider.SetInstance(Me)
    End Sub

    Protected Overrides Sub Initialize()
        _componentManager.LoadComponents()
        Core.Initialize(Me)
        MyBase.Initialize()
    End Sub

    Protected Overrides Sub LoadContent()
    End Sub

    Protected Overrides Sub UnloadContent()

    End Sub

    Protected Overrides Sub Update(ByVal gameTime As GameTime)
        If Me.window_change Then
            SetWindowSize(New Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height))
            Me.window_change = Not Me.window_change
        End If
        Core.Update(gameTime)
        MyBase.Update(gameTime)

        GameJolt.SessionManager.Update()
        FPSMonitor.Update(gameTime)
    End Sub

    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        Core.Draw()

        MyBase.Draw(gameTime)

        FPSMonitor.DrawnFrame()
    End Sub

    Public Shared ReadOnly Property DecSeparator As String
        Get
            Return My.Application.Culture.NumberFormat.NumberDecimalSeparator
        End Get
    End Property

    Protected Overrides Sub OnExiting(sender As Object, args As System.EventArgs)
        GameJolt.SessionManager.Close()

        If Core.ServersManager.ServerConnection.Connected = True Then
            Core.ServersManager.ServerConnection.Abort()
        End If

        Logger.Debug("---Exit Game---")
    End Sub

    Protected Sub Window_ClientSizeChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.window_change = True
        Core.windowSize = New Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height)


        If Not Core.CurrentScreen Is Nothing Then
            Core.CurrentScreen.SizeChanged()
            Screen.TextBox.PositionY = Core.windowSize.Height - 160.0F
        End If
        NetworkPlayer.ScreenRegionChanged()
    End Sub

    Private Sub DGame_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        NetworkPlayer.ScreenRegionChanged()
    End Sub

    Private Sub DGame_Deactivated(sender As Object, e As System.EventArgs) Handles Me.Deactivated
        NetworkPlayer.ScreenRegionChanged()
    End Sub

    Public Function GetGame() As Game Implements IGame.GetGame
        Return Me
    End Function

    Public Function GetComponentManager() As ComponentManager Implements IGame.GetComponentManager
        Return _componentManager
    End Function

    Private Shared GameHacked As Boolean = False 'Temp value that stores if a hacking file was detected at game start.

    ''' <summary>
    ''' If the player hacked any instance of Pokémon3D at some point.
    ''' </summary>
    Public Shared ReadOnly Property Hacker() As Boolean
        Get
            Return GameHacked
        End Get
    End Property

    ''' <summary>
    ''' The path to the game folder.
    ''' </summary>
    Public Shared ReadOnly Property GamePath() As String
        Get
            Return My.Application.Info.DirectoryPath
        End Get
    End Property

End Class