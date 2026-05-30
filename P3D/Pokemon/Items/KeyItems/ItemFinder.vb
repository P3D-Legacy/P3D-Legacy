Namespace Items.KeyItems

    <Item(55, "Itemfinder")>
    Public Class Itemfinder

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "It checks for unseen items in the area and makes noise and lights when it finds something."
        Public Overrides ReadOnly Property CanBeUsed As Boolean = True

        Public Sub New()
            _textureRectangle = New Rectangle(192, 48, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Dim found As Boolean = False

            Dim PlayerPosition As Vector3 = Screen.Camera.Position
            Dim ClosestItem As Vector3 = Nothing
            Dim Text As String = Localization.GetString("item_use_55_NoHiddenItems", "There are no hidden items~nearby.")
            For Each e As Entity In Screen.Level.Entities
                If e.EntityID = "ItemObject" Then
                    Dim i As ItemObject = CType(e, ItemObject)
                    If i.IsHiddenItem() = True Then
                        If ClosestItem = Nothing Then
                            ClosestItem = e.Position
                        Else
                            'Dim OldDistance As New Vector3(Math.Abs(ClosestItem.X - PlayerPosition.X), Math.Abs(ClosestItem.Y - PlayerPosition.Y), Math.Abs(ClosestItem.Z - PlayerPosition.Z))
                            'Dim NewDistance As New Vector3(Math.Abs(e.Position.X - PlayerPosition.X), Math.Abs(e.Position.Y - PlayerPosition.Y), Math.Abs(e.Position.Z - PlayerPosition.Z))

                            Dim OldDistance As Single = Vector3.Distance(ClosestItem, PlayerPosition)
                            Dim newDistance As Single = Vector3.Distance(e.Position, PlayerPosition)
                            Dim xSmaller As Boolean = False
                            Dim ySmaller As Boolean = False
                            Dim zSmaller As Boolean = False

                            If Math.Round(newDistance) < Math.Round(OldDistance) Then
                                ClosestItem = e.Position
                            ElseIf Math.Round(newDistance) = Math.Round(OldDistance) Then
                                If Random.Next(0, 2) = 1 Then 'If two items are the same distance
                                    ClosestItem = e.Position
                                End If
                            End If
                        End If

                        If i.PickupType = PickupTypes.Item Then
                            i.Opacity = 1.0F
                            i.NormalOpacity = 1.0F
                            i.Visible = True
                            i.HiddenDelay = Date.Now + New TimeSpan(0, 0, 3)
                        End If
                        found = True
                    End If
                End If
            Next

            While Core.CurrentScreen.Identification <> Screen.Identifications.OverworldScreen
                Core.SetScreen(Core.CurrentScreen.PreScreen)
            End While

            Dim Direction As Integer = Screen.Camera.GetPlayerFacingDirection()
            If found = True Then
                Dim xORz As Boolean = False 'x = closer
                If Math.Abs(ClosestItem.Z - PlayerPosition.Z) <= Math.Abs(ClosestItem.X - PlayerPosition.X) Then
                    xORz = True 'z = closer
                End If

                If ClosestItem.Z < PlayerPosition.Z Then
                    Direction = 0
                    Text = Localization.GetString("item_use_55_Found_North", "It's pointing to the north.")
                    If ClosestItem.X < PlayerPosition.X Then 'northwest
                        If xORz = False Then
                            Direction = 1
                        End If
                        Text = Localization.GetString("item_use_55_Found_NorthWest", "It's pointing to the~northwest.")
                    ElseIf ClosestItem.X > PlayerPosition.X Then 'northeast
                        If xORz = False Then
                            Direction = 3
                        End If
                        Text = Localization.GetString("item_use_55_Found_NorthEast", "It's pointing to the~northeast.")
                    End If
                ElseIf ClosestItem.Z > PlayerPosition.Z Then
                    Direction = 2
                    Text = Localization.GetString("item_use_55_Found_South", "It's pointing to the south.")
                    If ClosestItem.X < PlayerPosition.X Then 'southwest
                        If xORz = False Then
                            Direction = 1
                        End If
                        Text = Localization.GetString("item_use_55_Found_SouthWest", "It's pointing to the~southwest.")
                    ElseIf ClosestItem.X > PlayerPosition.X Then 'southeast
                        If xORz = False Then
                            Direction = 1
                        End If
                        Text = Localization.GetString("item_use_55_Found_SouthEast", "It's pointing to the~southeast.")
                    End If
                Else
                    If ClosestItem.X < PlayerPosition.X Then 'west
                        Direction = 1
                        Text = Localization.GetString("item_use_55_Found_West", "It's pointing to the west.")
                    ElseIf ClosestItem.X > PlayerPosition.X Then 'east
                        Direction = 3
                        Text = Localization.GetString("item_use_55_Found_East", "It's pointing to the east.")
                    Else 'below or above
                        If Math.Round(ClosestItem.Y) < Math.Round(PlayerPosition.Y) Then 'below
                            Text = Localization.GetString("item_use_55_Found_Down", "It's pointing downwards.")
                        ElseIf Math.Round(ClosestItem.Y) > Math.Round(PlayerPosition.Y) Then 'above
                            Text = Localization.GetString("item_use_55_Found_Up", "It's pointing upwards.")
                        Else 'same level
                            Text = Localization.GetString("item_use_55_Found_Here", "It's pointing to where I'm standing.")
                        End If
                    End If
                End If
                Select Case Direction
                    Case 0, 2
                        If Math.Abs(ClosestItem.Z - PlayerPosition.Z) <= 1 AndAlso Math.Abs(ClosestItem.Z - PlayerPosition.Z) > 0 AndAlso
                            Math.Abs(ClosestItem.X - PlayerPosition.X) = 0 AndAlso
                            Math.Round(Math.Abs(ClosestItem.Y - PlayerPosition.Y)) = 0 Then
                            Text &= "~" & Localization.GetString("item_use_55_Found_InFrontOfMe", "It's right in front of me!")
                        End If
                    Case 1, 2
                        If Math.Abs(ClosestItem.X - PlayerPosition.X) <= 1 AndAlso Math.Abs(ClosestItem.X - PlayerPosition.X) > 0 AndAlso
                            Math.Abs(ClosestItem.Z - PlayerPosition.Z) = 0 AndAlso
                            Math.Round(Math.Abs(ClosestItem.Y - PlayerPosition.Y)) = 0 Then
                            Text &= "~" & Localization.GetString("item_use_55_Found_InFrontOfMe", "It's right in front of me!")
                        End If
                End Select
                Dim s As String = "version=2" & Environment.NewLine &
                    "@sound.play(itemfinder)" & Environment.NewLine &
                    "@player.turnto(" & Direction & ")" & Environment.NewLine &
                    "@text.show(" & Text & ")" & Environment.NewLine &
                    ":end"

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            Else
                Screen.TextBox.Show(Text, {}, True, False)
            End If

        End Sub

    End Class

End Namespace
