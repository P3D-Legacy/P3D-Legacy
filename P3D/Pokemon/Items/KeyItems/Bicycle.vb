Namespace Items.KeyItems

    <Item(6, "Bicycle")>
    Public Class Bicycle

        Inherits KeyItem
        Public Overrides ReadOnly Property CanBeUsed As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A folding Bicycle that enables much faster movement than the Running Shoes."

        Public Sub New()
            _textureRectangle = New Rectangle(120, 0, 24, 24)
        End Sub

		Public Overrides Sub Use()
			If GameModeManager.ActiveGameMode.IsDefaultGamemode = False AndAlso Core.Player.IsGameJoltSave = False Then
				If Screen.Level.Riding = True Then
					If Screen.Level.RideType = 3 Then
						Screen.TextBox.Show(Localization.GetString("item_6_cannot_walk", "You cannot walk here!"), {}, True, False)
					Else
						Screen.Level.Riding = False
						Screen.Level.OwnPlayer.SetTexture(Core.Player.TempRideSkin, True)
						Core.Player.Skin = Core.Player.TempRideSkin

						Screen.TextBox.Show(Localization.GetString("item_use_6", "<player.name> stepped~off the Bicycle."))
						While Core.CurrentScreen.Identification <> Screen.Identifications.OverworldScreen
							Core.CurrentScreen = Core.CurrentScreen.PreScreen
						End While

						If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
							MusicManager.Play(Screen.Level.MusicLoop)
						End If
					End If
				Else
					If Screen.Level.Surfing = False AndAlso Screen.Level.Riding = False AndAlso Screen.Camera.IsMoving() = False AndAlso Screen.Camera.Turning = False And Screen.Level.CanRide() = True Then
						Dim BikeSkin As String = Core.Player.Skin & "_Bike"

						If File.Exists(GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Textures\NPC\" & BikeSkin & ".png") = True Then

							Core.Player.TempRideSkin = Core.Player.Skin
							Screen.Level.Riding = True

							Screen.Level.OwnPlayer.SetTexture(BikeSkin, False)
							Core.Player.Skin = BikeSkin
							Dim s As Screen = CurrentScreen
							While s.Identification <> Screen.Identifications.OverworldScreen
								s = s.PreScreen
							End While
							If s.Identification = Screen.Identifications.OverworldScreen Then
								Core.SetScreen(s)

								If GameModeManager.ContentFileExists("Sounds\Bicycle.wav") OrElse GameModeManager.ContentFileExists("Sounds\Bicycle.xnb") Then
									SoundManager.PlaySound("Bicycle")
								End If
								Dim RideMusicPath As String = "Songs\" & "Ride_" & Screen.Level.CurrentRegion

								If GameModeManager.ContentFileExists(RideMusicPath & ".ogg") OrElse GameModeManager.ContentFileExists(RideMusicPath & ".mp3") OrElse GameModeManager.ContentFileExists(RideMusicPath & ".wma") Then
									MusicManager.Play("Ride_" & Screen.Level.CurrentRegion, True)
								Else
									MusicManager.Play("Ride", True)
								End If
							Else
								Screen.TextBox.Show(Localization.GetString("item_cannot_use", "Now is not the time~to use that."), {}, True, True)
							End If
						Else
							Screen.TextBox.Show(Localization.GetString("item_6_missingskin", "You can't use this item~without a bicycle skin.*Its name should be the~same as your current one,~but with ""_bike"" at the end."), {}, True, False)
						End If
					Else
						Screen.TextBox.Show(Localization.GetString("item_cannot_use", "Now is not the time~to use that."), {}, True, True)
					End If
				End If
			Else
				Screen.TextBox.Show(Localization.GetString("item_6_only_custom_gamemodes", "This item can't be used~on this GameMode."), {}, True, True)
			End If
		End Sub

	End Class

End Namespace
