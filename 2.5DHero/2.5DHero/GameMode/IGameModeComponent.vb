Namespace GameModes

    ''' <summary>
    ''' Every component that is GameMode specific has to implement this interface.
    ''' </summary>
    Interface IGameModeComponent

        ''' <summary>
        ''' Gets called when the GameMode frees all resources because it gets unloaded.
        ''' </summary>
        Sub FreeResources()

        ''' <summary>
        ''' When this component gets added to the GameMode, this gets called. Implements lazy loading for GameMode components.
        ''' </summary>
        Sub Activated(ByVal GameMode As GameMode)

    End Interface

End Namespace
