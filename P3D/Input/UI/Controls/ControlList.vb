Namespace UI.GameControls

    ''' <summary>
    ''' A list of controls.
    ''' </summary>
    Public Class ControlList

        Private _list As New List(Of Control)

        ''' <summary>
        ''' Adds a control to the list.
        ''' </summary>
        ''' <param name="ctl">The control.</param>
        Public Sub Add(ByVal ctl As Control)
            AddHandler ctl.Focused, AddressOf FocusedControl
            _list.Add(ctl)
        End Sub

        ''' <summary>
        ''' Adds a range of controls to the list.
        ''' </summary>
        ''' <param name="ctls">The controls to add.</param>
        Public Sub AddRange(ByVal ctls As Control())
            For Each ctl In ctls
                Add(ctl)
            Next
        End Sub

        ''' <summary>
        ''' Removes a control from the list.
        ''' </summary>
        ''' <param name="ctl">The control.</param>
        Public Sub Remove(ByVal ctl As Control)
            If _list.Contains(ctl) = True Then
                RemoveHandler ctl.Focused, AddressOf FocusedControl
                _list.Remove(ctl)
            End If
        End Sub

        Private Sub FocusedControl(ByVal sender As Object, ByVal e As EventArgs)
            For Each ctl As Control In _list
                If sender.Equals(ctl) = False Then
                    ctl.IsFocused = False
                End If
            Next
        End Sub

        ''' <summary>
        ''' Updates the control list to take control switching input.
        ''' </summary>
        Public Sub Update()
            If (KeyBoardHandler.KeyPressed(Keys.Tab) Or
                Controls.Down(True, False, False, False, True, True) Or
                Controls.Up(True, False, False, False, True, True)) And
                _list.Count > 0 Then

                If _list.Count = 1 Then
                    If _list(0).IsFocused = False Then
                        _list(0).IsFocused = True
                    End If
                Else
                    Dim hasFocusedControl As Boolean = False
                    Dim controlListIndex As Integer = 0

                    For i = 0 To _list.Count - 1
                        If _list(i).IsFocused Then
                            hasFocusedControl = True
                            controlListIndex = i
                        End If
                    Next

                    If hasFocusedControl Then
                        Dim tabDirection As Integer = 1
                        If Controls.ShiftDown(TriggerButtons:=False) = True Or Controls.Up(False, False, False, False, True, True) Then
                            tabDirection = -1
                        End If

                        Dim focusIndex As Integer = controlListIndex + tabDirection
                        If focusIndex = _list.Count Then
                            focusIndex = 0
                        ElseIf focusIndex = -1 Then
                            focusIndex = _list.Count - 1
                        End If
                        _list(focusIndex).IsFocused = True
                    Else
                        _list(0).IsFocused = True
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace