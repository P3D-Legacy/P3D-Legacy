Namespace Screens.UI

    Public Interface ISelectionScreen

        ''' <summary>
        ''' The modes of this screen.
        ''' </summary>
        Enum ScreenMode As Integer
            [Default] = 0
            ''' <summary>
            ''' Used to select a single Item for another screen.
            ''' </summary>
            Selection = 1
        End Enum

        ''' <summary>
        ''' The current <see cref="ScreenMode"/> of the screen.
        ''' </summary>
        Property Mode As ScreenMode

        ''' <summary>
        ''' The event that gets fired when a selection is done on the screen.
        ''' </summary>
        Event SelectedObject(ByVal params As Object())

        ''' <summary>
        ''' If the user can exit the screen when in selection mode.
        ''' </summary>
        Property CanExit As Boolean

    End Interface

End Namespace
