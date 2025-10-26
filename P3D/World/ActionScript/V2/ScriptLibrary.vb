Namespace ScriptVersion2

    Public Class ScriptLibrary

        ' Last Library update: 0.52.1

#Region "Initialize"

        ''' <summary>
        ''' Call this at the initialize phase of the game. Fills the library with the script content.
        ''' </summary>
        Public Shared Sub InitializeLibrary()
            Dim sw As New Stopwatch()
            sw.Start()

            Scripts.Clear()

            DoFileSystem()

            DoRadio()

            DoPokedex()

            DoMath()

            DoRival()

            DoDaycare()

            DoPokemon()

            DoOverworldPokemon()

            DoNPC()

            DoPlayer()

            DoEnvironment()

            DoSystem()

            DoStorage()

            DoRegister()

            DoScript()

            DoScreen()

            DoChat()

            DoInventory()

            DoItem()

            DoPhone()

            DoEntity()

            DoLevel()

            DoBattle()

            DoMusic()

            DoSound()

            DoText()

            DoOptions()

            DoCamera()

            DoTitle()

            Scripts = (From s As ScriptCommand In Scripts Order By s.MainClass & "." & CInt(s.IsConstruct) & s.SubClass Ascending).ToList()
            sw.Stop()
            Logger.Debug("Initialized script library in " & sw.ElapsedMilliseconds & " milliseconds with " & Scripts.Count & " entries.")
        End Sub

        Private Shared Sub DoFileSystem()
            ' Constructs:
            r(New ScriptCommand("FileSystem", "PathSplit", "str", {New ScriptArgument("Index", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("Path", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the part of the path that is at the position of Index.", ",", True))
            r(New ScriptCommand("FileSystem", "PathSplitCount", "int", {New ScriptArgument("Path", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the amount of parts in the given path.", ",", True))
            r(New ScriptCommand("FileSystem", "PathUp", "str", {New ScriptArgument("Path", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the parent path to the given path if possible.", ",", True))
        End Sub

        Private Shared Sub DoTitle()
            ' Commands:
            r(New ScriptCommand("Title", "Add", {New ScriptArgument("Text", ScriptArgument.ArgumentTypes.Str, True, "Sample Text"),
                                              New ScriptArgument("Delay", ScriptArgument.ArgumentTypes.Sng, True, "20.0"),
                                              New ScriptArgument("ColorR", ScriptArgument.ArgumentTypes.Int, True, "255"),
                                              New ScriptArgument("ColorG", ScriptArgument.ArgumentTypes.Int, True, "255"),
                                              New ScriptArgument("ColorB", ScriptArgument.ArgumentTypes.Int, True, "255"),
                                              New ScriptArgument("Scale", ScriptArgument.ArgumentTypes.Sng, True, "10.0"),
                                              New ScriptArgument("IsCentered", ScriptArgument.ArgumentTypes.Bool, True, "true"),
                                              New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng, True, "0.0"),
                                              New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng, True, "0.0")}.ToList(), "Adds a new title for the game to display during gameplay."))
            r(New ScriptCommand("Title", "Clear", "Clears all titles that are currently being displayed."))
        End Sub

        Private Shared Sub DoCamera()
            ' Commands:
            r(New ScriptCommand("Camera", "ActivateThirdPerson", {New ScriptArgument("UpdateCamera", ScriptArgument.ArgumentTypes.Bool, True, "True")}.ToList(), "Activates the third person camera."))
            r(New ScriptCommand("Camera", "DeactivateThirdPerson", {New ScriptArgument("UpdateCamera", ScriptArgument.ArgumentTypes.Bool, True, "True")}.ToList(), "Deactivates the third person camera."))
            r(New ScriptCommand("Camera", "ToggleThirdPerson", {New ScriptArgument("UpdateCamera", ScriptArgument.ArgumentTypes.Bool, True, "True")}.ToList(), "Sets the camera to the opposite of the current perspective mode (first person or third person)."))
            r(New ScriptCommand("Camera", "SetThirdPerson", {New ScriptArgument("PerspectiveMode", ScriptArgument.ArgumentTypes.Bool),
                                                         New ScriptArgument("UpdateCamera", ScriptArgument.ArgumentTypes.Bool, True, "True")}.ToList(), "Sets the camera to the desired perspective mode."))
            r(New ScriptCommand("Camera", "Fix", "Fixes the camera to the current position."))
            r(New ScriptCommand("Camera", "Defix", "Defixes the camera so that it clips behind the player again."))
            r(New ScriptCommand("Camera", "ToggleFix", "Sets the fix state of the camera to the opposite of the current state."))
            r(New ScriptCommand("Camera", "Set", {New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("Z", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("Yaw", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("Pitch", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the position and rotation of the camera."))
            r(New ScriptCommand("Camera", "SetPitch", {New ScriptArgument("Pitch", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the Pitch (vertical) rotation of the camera."))
            r(New ScriptCommand("Camera", "SetYaw", {New ScriptArgument("Yaw", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the Yaw (horizontal) rotation of the camera."))
            r(New ScriptCommand("Camera", "SetPosition", {New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("Z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the position of the camera."))
            r(New ScriptCommand("Camera", "SetX", {New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the X-coordinate of the camera (left/right)."))
            r(New ScriptCommand("Camera", "SetY", {New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the Y-coordinate of the camera (up/down)."))
            r(New ScriptCommand("Camera", "SetZ", {New ScriptArgument("Z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the Z-coordinate of the camera (forward/backward)."))
            r(New ScriptCommand("Camera", "SetFocus", {New ScriptArgument("FocusType", ScriptArgument.ArgumentTypes.Str, {"Player", "NPC", "Entity"}),
                                                   New ScriptArgument("FocusID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Focuses the camera on an object (with the given ID for NPCs & Entities)."))
            r(New ScriptCommand("Camera", "SetFocusType", {New ScriptArgument("FocusType", ScriptArgument.ArgumentTypes.Str, {"Player", "NPC", "Entity"})}.ToList(), "Sets the focus type for the camera."))
            r(New ScriptCommand("Camera", "SetFocusID", {New ScriptArgument("FocusID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the ID of the focus target for the camera."))
            r(New ScriptCommand("Camera", "SetToPlayerFacing", "Sets the Yaw rotation of the camera to the direction in which the player is facing."))
            r(New ScriptCommand("Camera", "Reset", "Resets the camera to its default location and rotation."))
            r(New ScriptCommand("Camera", "Update", "Updates the camera. Used for things like fading the screen during a script or when a property of the camera is changed."))

            ' Constructs:
            r(New ScriptCommand("Camera", "IsFixed", "bool", "Returns if the camera is fixed to a specific position.", ",", True))
            r(New ScriptCommand("Camera", "X", "sng", "Returns the current X position of the camera.", ",", True))
            r(New ScriptCommand("Camera", "Y", "sng", "Returns the current Y position of the camera.", ",", True))
            r(New ScriptCommand("Camera", "Z", "sng", "Returns the current Z position of the camera.", ",", True))
            r(New ScriptCommand("Camera", "Pitch", "sng", "Returns the current Pitch (vertical) rotation of the camera.", ",", True))
            r(New ScriptCommand("Camera", "Yaw", "sng", "Returns the current Yaw (horizontal) rotation of the camera.", ",", True))
            r(New ScriptCommand("Camera", "ThirdPerson", "bool", "Returns if the camera is in third person mode.", ",", True))
        End Sub

        Private Shared Sub DoOptions()
            ' Commands:
            r(New ScriptCommand("options", "show", {New ScriptArgument("options", ScriptArgument.ArgumentTypes.StrArr),
                                                New ScriptArgument("flag", ScriptArgument.ArgumentTypes.Str, {"[TEXT=FALSE]"}, True, "")}.ToList(), "Displays a choose box with the given options."))
            r(New ScriptCommand("options", "setcancelindex", {New ScriptArgument("index", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the cancel index of the next choose box. This index gets choosen when the player presses a back key."))
        End Sub

        Private Shared Sub DoText()
            ' Commands:
            r(New ScriptCommand("text", "show", {New ScriptArgument("text", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Displays a textbox with the given text."))
            r(New ScriptCommand("text", "setfont", {New ScriptArgument("font", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the font of the textbox. All fonts from loaded ContentPacks, GameModes and the standard game can be loaded."))
            r(New ScriptCommand("text", "notification", {New ScriptArgument("message", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("delay", ScriptArgument.ArgumentTypes.Int, True, "500"), New ScriptArgument("backgroundindex", ScriptArgument.ArgumentTypes.Int, True, "0"), New ScriptArgument("iconindex", ScriptArgument.ArgumentTypes.Int, True, "0"), New ScriptArgument("soundeffect", ScriptArgument.ArgumentTypes.Str, True), New ScriptArgument("scriptfile", ScriptArgument.ArgumentTypes.Str, True)}.ToList(), "Displays a textbox with the given text."))
            r(New ScriptCommand("text", "debug", {New ScriptArgument("text", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Prints the ""text"" argument to the immediate window console."))
            r(New ScriptCommand("text", "log", {New ScriptArgument("text", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Logs the ""text"" argument into the log.dat file."))
            r(New ScriptCommand("text", "color", {New ScriptArgument("colorName", ScriptArgument.ArgumentTypes.Str, {"playercolor", "defaultcolor"})}.ToList(), "Changes the font color to a preset. You can also use a VB.NET compatible color in the ""KnownColor"" enum instead."))
            r(New ScriptCommand("text", "color", {New ScriptArgument("Red", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("Green", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("Blue", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the font color to the specified RGB values."))
        End Sub

        Private Shared Sub DoSound()
            'Commands:
            r(New ScriptCommand("sound", "play", {New ScriptArgument("soundFile", ScriptArgument.ArgumentTypes.Str),
                                              New ScriptArgument("stopBackgroundMusic", ScriptArgument.ArgumentTypes.Bool, True, "false")}.ToList(), "Plays a sound."))
            r(New ScriptCommand("sound", "playadvanced", {New ScriptArgument("soundFile", ScriptArgument.ArgumentTypes.Str),
                                              New ScriptArgument("stopBackgroundMusic", ScriptArgument.ArgumentTypes.Bool),
                                              New ScriptArgument("pitch", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("pan", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("volume", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Plays a sound with advanced parameters."))
        End Sub

        Private Shared Sub DoMusic()
            ' Commands:
            r(New ScriptCommand("music", "play", {New ScriptArgument("musicFile", ScriptArgument.ArgumentTypes.Str),
                                 New ScriptArgument("loopSong", ScriptArgument.ArgumentTypes.Bool, True, "true"), New ScriptArgument("fadeIntoSong", ScriptArgument.ArgumentTypes.Bool, True, "false")}.ToList(), "Changes the currently playing music to a new one."))
            r(New ScriptCommand("music", "forceplay", {New ScriptArgument("musicFile", ScriptArgument.ArgumentTypes.Str),
                                 New ScriptArgument("loopSong", ScriptArgument.ArgumentTypes.Bool, True, "true"), New ScriptArgument("fadeIntoSong", ScriptArgument.ArgumentTypes.Bool, True, "false")}.ToList(), "Changes the currently playing music to a new one and prevents the music from being changed by warps or surfing/riding and such."))
            r(New ScriptCommand("music", "unforce", "Allows warps and surfing/riding etc. to change the music again."))
            r(New ScriptCommand("music", "setmusicloop", {New ScriptArgument("musicFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the map musicloop to a new musicfile."))
            r(New ScriptCommand("music", "stop", "Stops the music playback."))
            r(New ScriptCommand("music", "mute", "Silences the music playback."))
            r(New ScriptCommand("music", "unmute", "Reverts the volume of the music playback."))
            r(New ScriptCommand("music", "pause", "Pauses the music playback."))
            r(New ScriptCommand("music", "resume", "Resumes the music playback."))
        End Sub

        Private Shared Sub DoBattle()
            ' Commands:
            r(New ScriptCommand("Battle", "StartTrainer", {New ScriptArgument("TrainerFilePath", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Initializes a trainer interaction and checks the register if the player has already beaten that trainer."))
            r(New ScriptCommand("Battle", "Trainer", {New ScriptArgument("TrainerFilePath", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Initializes a trainer battle without displaying an intro message or checking the register."))
            r(New ScriptCommand("Battle", "Wild", {New ScriptArgument("PokémonData", ScriptArgument.ArgumentTypes.PokemonData),
                                               New ScriptArgument("IntroMusic", ScriptArgument.ArgumentTypes.Str, True, "")}.ToList(), "Initializes a wild battle against the given Pokémon."))
            r(New ScriptCommand("Battle", "Wild", {New ScriptArgument("PokémonID", ScriptArgument.ArgumentTypes.Int),
                                               New ScriptArgument("Level", ScriptArgument.ArgumentTypes.Int),
                                               New ScriptArgument("Shiny", ScriptArgument.ArgumentTypes.Int, True, "-1"),
                                               New ScriptArgument("IntroMusic", ScriptArgument.ArgumentTypes.Str, True, ""),
                                               New ScriptArgument("IntroType", ScriptArgument.ArgumentTypes.Int, True, "0-10"),
                                               New ScriptArgument("Gender", ScriptArgument.ArgumentTypes.Int, True, "")}.ToList(), "Initializes a wild battle against the given Pokémon."))
            r(New ScriptCommand("Battle", "SetVar", {New ScriptArgument("VarName", ScriptArgument.ArgumentTypes.Str, {"CanRun", "CanCatch", "CanBlackout", "CanReceiveExp", "CanUseItems", "FrontierTrainer", "DiveBattle", "InverseBattle, CustomBattleMusic, HiddenAbilityChance"}),
                                                 New ScriptArgument("VarValue", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the given battle variable to the given value."))
            r(New ScriptCommand("Battle", "ResetVars", "Resets the battle variables to their default value."))
            ' Constructs:
            r(New ScriptCommand("Battle", "DefeatMessage", "str", {New ScriptArgument("TrainerFilePath", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the defeat message of the trainer loaded from the given ""TrainerFilePath"".", ",", True))
            r(New ScriptCommand("Battle", "IntroMessage", "str", {New ScriptArgument("TrainerFilePath", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the intro message of the trainer loaded from the given ""TrainerFilePath"".", ",", True))
            r(New ScriptCommand("Battle", "OutroMessage", "str", {New ScriptArgument("TrainerFilePath", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the outro message of the trainer loaded from the given ""TrainerFilePath"".", ",", True))
            r(New ScriptCommand("Battle", "Won", "bool", "Returns ""true"" if the player won the last battle. Returns ""false"" otherwise.", ",", True))
            r(New ScriptCommand("Battle", "Caught", "bool", "Returns ""true"" if the player caught the Pokémon in the last battle. Returns ""false"" otherwise.", ",", True))
        End Sub

        Private Shared Sub DoLevel()
            ' Commands:
            r(New ScriptCommand("level", "wait", {New ScriptArgument("ticks", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Makes the level idle for the duration of the given ticks."))
            r(New ScriptCommand("level", "update", "Updates the level and all entities once."))
            r(New ScriptCommand("level", "waitforevents", "Makes the level idle until every NPC movement is done."))
            r(New ScriptCommand("level", "waitforsave", "Makes the level idle until the current saving of an GameJolt save is done."))
            r(New ScriptCommand("level", "reload", "Reloads the current map."))
            r(New ScriptCommand("level", "setsafari", {New ScriptArgument("safari", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets if the current map is a Safari Zone (influences battle style)."))
            r(New ScriptCommand("level", "setridetype", {New ScriptArgument("rideType", ScriptArgument.ArgumentTypes.Int, {"0-3"})}.ToList(), "Sets the Ride Type of the current map. (0 = Depends on CanDig and CanFly tags, 1 = Can ride, 2 = Can not ride, 3 = Can't stop riding once started)"))
            ' Constructs:
            r(New ScriptCommand("level", "mapfile", "str", "Returns the mapfile of the currently loaded map.", ",", True))
            r(New ScriptCommand("level", "levelfile", "str", "Returns the mapfile of the currently loaded map.", ",", True))
            r(New ScriptCommand("level", "filename", "str", "Returns only the name of the current map file, without path and extension.", ",", True))
            r(New ScriptCommand("level", "riding", "bool", "Returns if the player is Riding a Pokémon right now.", ",", True))
            r(New ScriptCommand("level", "surfing", "bool", "Returns if the player is Suring on a Pokémon right now.", ",", True))
            r(New ScriptCommand("level", "musicloop", "str", "Returns only the name of the current played song, without path and extension.", ",", True))
            r(New ScriptCommand("level", "daytime", "int", "Returns the DayTime of the current map.", ",", True))
            r(New ScriptCommand("level", "environmenttype", "int", "Returns the EnvironmentType of the current map.", ",", True))
            r(New ScriptCommand("level", "loadoffsetmaps", "bool", "Returns if OffsetMaps are being loaded (based on the Offset Map Quality option in the Options Menu).", ",", True))
        End Sub

        Private Shared Sub DoEntity()
            ' Commands:
            r(New ScriptCommand("Entity", "ShowMessageBulb", {New ScriptArgument("BulbID", ScriptArgument.ArgumentTypes.Int, {"0-15"}),
                                                          New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("Z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Displays the given Message Bulb at the given position.", "|"))
            r(New ScriptCommand("Entity", "Warp", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                          New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("Z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Warps the given entity to the given position on the map."))
            r(New ScriptCommand("Entity", "AddToPosition", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                         New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng),
                                                         New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng),
                                                         New ScriptArgument("Z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Adds the given coordinates to the position of the given entity."))
            r(New ScriptCommand("Entity", "Remove", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the given entity from the map (when it updates) until the map is loaded again."))
            r(New ScriptCommand("Entity", "SetID", {New ScriptArgument("OldID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("NewID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the ID of the given entity."))
            r(New ScriptCommand("Entity", "SetScale", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                          New ScriptArgument("xScale", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("yScale", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("zScale", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the Scale property of the given entity."))
            r(New ScriptCommand("Entity", "SetOpacity", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                     New ScriptArgument("Opacity", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the Opacity (transparency) property of the given entity to the given value (in %)."))
            r(New ScriptCommand("Entity", "SetVisible", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("Visible", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Changes the whether the entity is visible or not."))
            r(New ScriptCommand("Entity", "SetAdditionalValue", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("AdditionalValue", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the AdditionalValue property of the given entity."))
            r(New ScriptCommand("Entity", "SetCollision", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("Collision", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the Collision property of the given entity."))
            r(New ScriptCommand("Entity", "SetTexture", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("TextureIndex", ScriptArgument.ArgumentTypes.Str),
                                                New ScriptArgument("TextureName", ScriptArgument.ArgumentTypes.Str),
                                                New ScriptArgument("RectangleX", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("RectangleY", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("RectangleWidth", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("RectangleHeight", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the texture in the selected entity's texture array. Example: @Entity.SetTexture(0,0,Routes,112,64,16,32)"))
            r(New ScriptCommand("Entity", "SetModelPath", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("ModelPath", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the ModelPath property of the given entity."))

            ' Constructs:
            r(New ScriptCommand("Entity", "Visible", "bool", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Visible property of the given entity.", ",", True))
            r(New ScriptCommand("Entity", "Opacity", "int", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Opacity property of the given entity.", ",", True))
            r(New ScriptCommand("Entity", "Position", "sngArr", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Position of the given entity in the pattern ""x,y,z"".", ",", True))
            r(New ScriptCommand("Entity", "PositionX", "sng", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the X Position of the given entity.", ",", True))
            r(New ScriptCommand("Entity", "PositionY", "sng", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Y Position of the given entity.", ",", True))
            r(New ScriptCommand("Entity", "PositionZ", "sng", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Z Position of the given entity.", ",", True))
            r(New ScriptCommand("Entity", "Scale", "sngArr", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Scale property of the given entity in the pattern ""x,y,z"".", ",", True))
            r(New ScriptCommand("Entity", "Rotation", "sngArr", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Rotation property of the given entity in the pattern ""x,y,z"".", ",", True))
            r(New ScriptCommand("Entity", "AdditionalValue", "str", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the AdditionalValue property of the given entity.", ",", True))
            r(New ScriptCommand("Entity", "Collision", "bool", {New ScriptArgument("EntityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Collision property of the given entity.", ",", True))
        End Sub

        Private Shared Sub DoPhone()
            ' Constructs:
            r(New ScriptCommand("phone", "callflag", "str", "Returns if the Pokégear is calling or is being called. Values: ""calling"", ""receiving""", ",", True))
            r(New ScriptCommand("phone", "got", "bool", "Returns if the player got the Pokégear.", ",", True))
        End Sub

        Private Shared Sub DoItem()
            ' Commands:
            r(New ScriptCommand("item", "give", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Str),
                                             New ScriptArgument("Amount", ScriptArgument.ArgumentTypes.Int, True, "1")}.ToList(), "Adds the given amount of items to the player's inventory."))
            r(New ScriptCommand("item", "remove", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Str),
                                             New ScriptArgument("Amount", ScriptArgument.ArgumentTypes.Int, True, "1"),
                                               New ScriptArgument("showMessage", ScriptArgument.ArgumentTypes.Bool, True, "true")}.ToList(), "Removes the given amount of items from the player's inventory. Displays a message afterwards, if ""showMessage"" is true."))
            r(New ScriptCommand("item", "clearitem", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Str, True, "")}.ToList(), "Clears all items with the given ID from the player's inventory. Clears the whole inventory if ItemID is empty."))
            r(New ScriptCommand("item", "messagegive", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Str),
                                             New ScriptArgument("Amount", ScriptArgument.ArgumentTypes.Int, True, "1")}.ToList(), "Displays a message for getting the specified amount of items."))
            r(New ScriptCommand("item", "repel", {New ScriptArgument("RepelItemID", ScriptArgument.ArgumentTypes.Int, {"20", "42", "43"})}.ToList(), "Adds the steps of the Repel to the Repel steps of the player."))
            r(New ScriptCommand("item", "use", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Uses the specified item if the player has it."))
            r(New ScriptCommand("item", "select", {New ScriptArgument("AllowedPages", ScriptArgument.ArgumentTypes.Str, True, "-1"),
                                             New ScriptArgument("AllowedItems", ScriptArgument.ArgumentTypes.Str, True, "-1")}.ToList(), "Opens an item select screen with only the specified item type pages (separated with "";"", e.g. ""0;1;2"" or ""standard;medicine;plants"") and possible item IDs (single items separated with "";"", or with a ""-"" if you want a range, e.g. ""2000-2066"")."))

        End Sub

        Private Shared Sub DoInventory()
            ' Constructs:
            r(New ScriptCommand("inventory", "countitem", "int", {New ScriptArgument("itemID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of the Item with the given ID in the player's inventory.", ",", True))
            r(New ScriptCommand("inventory", "countitems", "int", "Counts all items in the player's inventory.", ",", True))
            r(New ScriptCommand("inventory", "name", "str", {New ScriptArgument("itemID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the name of an Item by its ItemID.", ",", True))
            r(New ScriptCommand("inventory", "id", "int", {New ScriptArgument("itemName", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the ID of an Item by its Name.", ",", True))
            r(New ScriptCommand("inventory", "selected", "str", "Returns the item ID of the item selected with @item.select.", ",", True))
        End Sub

        Private Shared Sub DoChat()
            ' Commands:
            r(New ScriptCommand("Chat", "Clear", "Clears the chat."))
        End Sub

        Private Shared Sub DoScreen()
            ' Commands:
            r(New ScriptCommand("screen", "storagesystem", "Opens the storage system."))
            r(New ScriptCommand("screen", "apricornkurt", "Opens the Apricorn Screen."))
            r(New ScriptCommand("screen", "trade", {New ScriptArgument("tradeItems", ScriptArgument.ArgumentTypes.ItemCollection),
                                                New ScriptArgument("canBuy", ScriptArgument.ArgumentTypes.Bool),
                                                New ScriptArgument("canSell", ScriptArgument.ArgumentTypes.Bool),
                                                New ScriptArgument("Currency", ScriptArgument.ArgumentTypes.Str, {"P", "BP", "C"}, True, "P"),
                                                New ScriptArgument("shopIdentifier", ScriptArgument.ArgumentTypes.Str, {}, True, "")}.ToList(), "Opens a new trade screen with the given items in stock. tradeItems: {itemID|amount|price}{...}..., amount and price are default for -1. Currency defaults to ""P"" and shopIdentifier is optional.", ","))
            r(New ScriptCommand("screen", "townmap", {New ScriptArgument("regionList", ScriptArgument.ArgumentTypes.StrArr)}.ToList(), "Opens the map screen with the given regions."))
            r(New ScriptCommand("screen", "donation", "Opens the donation screen."))
            r(New ScriptCommand("screen", "blackout", "Opens the blackout screen and warps the player back to the last restplace."))
            r(New ScriptCommand("screen", "fadein", {New ScriptArgument("fadeSpeed", ScriptArgument.ArgumentTypes.Int, True, "5")}.ToList(), "Fades the screen back in."))
            r(New ScriptCommand("screen", "fadeout", {New ScriptArgument("fadeSpeed", ScriptArgument.ArgumentTypes.Int, True, "5")}.ToList(), "Fades the screen to black."))
            r(New ScriptCommand("screen", "fadeoutcolor", {New ScriptArgument("color", ScriptArgument.ArgumentTypes.IntArr, {"0-255"}, True, "0,0,0")}.ToList(), "Sets the color of the screen fade."))
            r(New ScriptCommand("screen", "setfade", {New ScriptArgument("alpha", ScriptArgument.ArgumentTypes.Int, {"0-255"})}.ToList(), "Sets the alpha value of the screen fade."))
            r(New ScriptCommand("screen", "credits", {New ScriptArgument("ending", ScriptArgument.ArgumentTypes.Str, True, "Johto")}.ToList(), "Displays the credits scene."))
            r(New ScriptCommand("screen", "halloffame", {New ScriptArgument("displayEntryIndex", ScriptArgument.ArgumentTypes.Int, True, "")}.ToList(), "Displays the Hall of Fame. If the argument ""displayEntryIndex"" is not empty, it displays only that entry."))
            r(New ScriptCommand("screen", "teachmoves", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                     New ScriptArgument("moveIDs", ScriptArgument.ArgumentTypes.IntArr, True, "")}.ToList(), "Displays a move learn screen. If the argument ""moveIDs"" is left empty, it defaults to the Pokémon's tutor moves."))
            r(New ScriptCommand("screen", "mailsystem", "Opens the PC Inbox screen."))
            r(New ScriptCommand("screen", "pvp", "Opens the PvP lobby screen (not finished yet, don't use)."))
            r(New ScriptCommand("screen", "input", {New ScriptArgument("defaultName", ScriptArgument.ArgumentTypes.Str, True, ""),
                                                New ScriptArgument("inputMode", ScriptArgument.ArgumentTypes.Str, {"0-2", "name", "numbers", "text"}, True, "0"),
                                                New ScriptArgument("currentText", ScriptArgument.ArgumentTypes.Str, True, ""),
                                                New ScriptArgument("maxChars", ScriptArgument.ArgumentTypes.Int, True, "14")}.ToList(), "Displays the Input screen. The input can be retrieved with <system.lastinput>."))
            r(New ScriptCommand("screen", "mysteryevent", "Opens the Mystery Event screen."))
            r(New ScriptCommand("screen", "showpokemon", {New ScriptArgument("PokemonID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("Shiny", ScriptArgument.ArgumentTypes.Bool),
                                                New ScriptArgument("Front", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Displays a box and an image of the specified Pokemon."))
            r(New ScriptCommand("screen", "showimage", {New ScriptArgument("Texture", ScriptArgument.ArgumentTypes.Str),
                                                New ScriptArgument("SoundEffect", ScriptArgument.ArgumentTypes.Str, True, ""),
                                                New ScriptArgument("X", ScriptArgument.ArgumentTypes.Int, True),
                                                New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Int, True),
                                                New ScriptArgument("Width", ScriptArgument.ArgumentTypes.Int, True),
                                                New ScriptArgument("Height", ScriptArgument.ArgumentTypes.Int, True)}.ToList(), "Displays a box and (part of a) texture image."))
            r(New ScriptCommand("screen", "showmessagebox", {New ScriptArgument("Message", ScriptArgument.ArgumentTypes.Str),
                                                New ScriptArgument("BackgroundColor", ScriptArgument.ArgumentTypes.IntArr, {"0-255"}, True),
                                                New ScriptArgument("FontColor", ScriptArgument.ArgumentTypes.IntArr, {"0-255"}, True),
                                                New ScriptArgument("BorderColor", ScriptArgument.ArgumentTypes.IntArr, {"0-255"}, True)}.ToList(), "Displays a dynamically sized customizable message box in the center of the screen."))
            r(New ScriptCommand("screen", "secretbase", "Opens the Secret Base screen."))
            r(New ScriptCommand("screen", "skinselection", "Opens the Player Skin selection screen."))
            r(New ScriptCommand("screen", "voltorbflip", "Opens the Voltorb Flip minigame screen."))
            ' Constructs:
            r(New ScriptCommand("screen", "selectedskin", "str", "Returns the texture name of the skin selected by using @Screen.SkinSelection", ",", True))
            r(New ScriptCommand("screen", "selectedname", "str", "Returns the default name assigned to the skin selected by using @Screen.SkinSelection", ",", True))
            r(New ScriptCommand("screen", "selectedname", "str", "Returns the default gender assigned to the skin selected by using @Screen.SkinSelection", ",", True))

        End Sub

        Private Shared Sub DoScript()
            ' Commands:
            r(New ScriptCommand("script", "start", {New ScriptArgument("scriptFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Starts a script with the given filename (without file extension)."))
            r(New ScriptCommand("script", "text", {New ScriptArgument("text", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Starts a script with a simple text to display."))
            r(New ScriptCommand("script", "run", {New ScriptArgument("scriptContent", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Runs script content. New lines are represented with ""^""."))
            r(New ScriptCommand("script", "delay", {New ScriptArgument("delayID", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("scriptPath", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("delayType", ScriptArgument.ArgumentTypes.Str, {"steps", "itemcount"}), New ScriptArgument("valueArguments", ScriptArgument.ArgumentTypes.StrArr)}.ToList(), "Executes a script file after something happened (like having moved a certain amount of steps)."))
            r(New ScriptCommand("script", "cleardelay", {New ScriptArgument("delayID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Removes the register with the specified identifier (delayID) created with @script.delay, preventing the script from being executed."))

            ' Constructs:
            r(New ScriptCommand("script", "delay", "str,int", {New ScriptArgument("delayID", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("returnType", ScriptArgument.ArgumentTypes.Str, {"type", "script", "value"})}.ToList(), "Returns the ""type"", ""scriptpath"" or ""value"" of what will trigger the script, like the number of steps.", ",", True))
        End Sub

        Private Shared Sub DoRegister()
            ' Commands:
            r(New ScriptCommand("register", "register", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Registers a new register with the given name."))
            r(New ScriptCommand("register", "register", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str),
                                                     New ScriptArgument("type", ScriptArgument.ArgumentTypes.Str, {"str", "int", "sng", "bool"}),
                                                     New ScriptArgument("value", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Registers a new register with the given name, type and value."))
            r(New ScriptCommand("register", "unregister", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Unregisters a register with the given name."))
            r(New ScriptCommand("register", "unregister", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str),
                                                       New ScriptArgument("type", ScriptArgument.ArgumentTypes.Str, {"str", "int", "sng", "bool"})}.ToList(), "Unregisters a register with the given name and type that has a value."))
            r(New ScriptCommand("register", "registertime", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str),
                                                         New ScriptArgument("time", ScriptArgument.ArgumentTypes.Int),
                                                         New ScriptArgument("timeFormat", ScriptArgument.ArgumentTypes.Str, {"days", "hours", "minutes", "seconds", "years", "weeks", "months", "dayofweek"})}.ToList(), "Registers a time based register."))
            r(New ScriptCommand("register", "change", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("value", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the specified register's stored value to a new value."))

            ' Constructs:
            r(New ScriptCommand("register", "registered", "bool", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if a register with the given name is registered.", ",", True))
            r(New ScriptCommand("register", "count", "int", "Counts all registers.", ",", True))
            r(New ScriptCommand("register", "type", "str", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the type of a register with the given name.", ",", True))
            r(New ScriptCommand("register", "value", "str,int,bool,sng", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the value of a register with the given name as its type.", ",", True))
        End Sub

        Private Shared Sub DoStorage()
            ' Commands:
            r(New ScriptCommand("storage", "set", {New ScriptArgument("type", ScriptArgument.ArgumentTypes.Str, {"pokemon", "item", "string", "integer", "boolean", "single", "str", "int", "bool", "sng"}),
                                               New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str),
                                               New ScriptArgument("value", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Creates or overwrites a storage with the given name and type."))
            r(New ScriptCommand("storage", "clear", "Clears all storage items."))
            r(New ScriptCommand("storage", "update", {New ScriptArgument("type", ScriptArgument.ArgumentTypes.Str, {"pokemon", "item", "string", "integer", "boolean", "single", "str", "int", "bool", "sng"}),
                                               New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str),
                                               New ScriptArgument("operation", ScriptArgument.ArgumentTypes.Str, {"add", "substract", "multiply", "divide"}),
                                               New ScriptArgument("value", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Updates the value in a storage with the given name and type."))

            ' Constructs:
            r(New ScriptCommand("storage", "get", "str", {New ScriptArgument("type", ScriptArgument.ArgumentTypes.Str, {"pokemon", "item", "string", "integer", "boolean", "single", "str", "int", "bool", "sng"}),
                                                      New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the value for the storage with the type ""type"" and name ""name"".", ",", True))
            r(New ScriptCommand("storage", "count", "int", {New ScriptArgument("type", ScriptArgument.ArgumentTypes.Str, {"pokemon", "item", "string", "integer", "boolean", "single", "str", "int", "bool", "sng"})}.ToList(), "Returns the amount of items in the storage for a specific type.", ",", True))
        End Sub

        Private Shared Sub DoSystem()
            ' Commands:
            r(New ScriptCommand("system", "endnewgame", {New ScriptArgument("Map", ScriptArgument.ArgumentTypes.Str),
                                               New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng, True, "~"),
                                               New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng, True, "~"),
                                               New ScriptArgument("Z", ScriptArgument.ArgumentTypes.Sng, True, "~"),
                                               New ScriptArgument("Rotation", ScriptArgument.ArgumentTypes.Int, {"0-3"}, True, "0")}.ToList(), "Ends the 3D new game intro screen and creates a savefile."))
            r(New ScriptCommand("system", "replacetextures", {New ScriptArgument("textureReplacementFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Loads the given texture replacement file (without file extension)."))

            ' Constructs:
            r(New ScriptCommand("system", "random", "int", {New ScriptArgument("min", ScriptArgument.ArgumentTypes.Int, True, "1"),
                                                        New ScriptArgument("max", ScriptArgument.ArgumentTypes.Int, True, "2")}.ToList(), "Generates a random number between min and max, inclusive.", ",", True))
            r(New ScriptCommand("system", "unixtimestamp", "int", "Returns the UNIX timestamp for the current computer time.", ",", True))
            r(New ScriptCommand("system", "dayofyear", "int", "Returns the day of the year (Outdated, use <environment.dayofyear> instead).", ",", True))
            r(New ScriptCommand("system", "year", "int", "Returns the current year (Outdated, use <environment.year> instead).", ",", True))
            r(New ScriptCommand("system", "booltoint", "int", {New ScriptArgument("bool", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Converts a boolean into an integer (Outdated, use <math.int> instead).", ",", True))
            r(New ScriptCommand("system", "calcint", "int", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Converts the expression to an integer (Outdated, use <math.int> instead).", ",", True))
            r(New ScriptCommand("system", "int", "int", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Converts the expression to an integer (Outdated, use <math.int> instead).", ",", True))
            r(New ScriptCommand("system", "calcsng", "sng", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Converts the expression to a single (Outdated, use <math.sng> instead).", ",", True))
            r(New ScriptCommand("system", "sng", "sng", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Converts the expression to a single (Outdated, use <math.sng> instead).", ",", True))
            r(New ScriptCommand("system", "sort", "str", {New ScriptArgument("sortMode", ScriptArgument.ArgumentTypes.Str, {"ascending", "descending"}),
                                                         New ScriptArgument("returnIndex", ScriptArgument.ArgumentTypes.Int),
                                                         New ScriptArgument("list", ScriptArgument.ArgumentTypes.Arr)}.ToList(), "Sorts the list after sortmode and returns the item at the given index.", ",", True))
            r(New ScriptCommand("system", "scripttrigger", "string", "Returns what triggered the current script (NPCInSight, NPCInteract, ScriptBlockWalkOn, ScriptBlockInteract, Notification, PhoneReceive, PhoneCall, StartScript, ScriptCommand, StrengthTrigger, MapScript, ChatCommand).", ",", True))
            r(New ScriptCommand("system", "isinsightscript", "bool", "Returns if the running script was triggered by the inSight function of an NPC.", ",", True))
            r(New ScriptCommand("system", "lastinput", "str", "Returns the last input received from the input screen (@screen.input).", ",", True))
            r(New ScriptCommand("system", "return", "str", "Returns the value set with the "":return"" switch.", ",", True))
            r(New ScriptCommand("system", "isint", "bool", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if the expression is an integer (Outdated, use <math.isint> instead).", ",", True))
            r(New ScriptCommand("system", "issng", "bool", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if the expression is a single (Outdated, use <math.issng> instead).", ",", True))
            r(New ScriptCommand("system", "chrw", "str", {New ScriptArgument("charCodes", ScriptArgument.ArgumentTypes.IntArr)}.ToList(), "Converts Unicode CharCodes into a string.", ",", True))
            r(New ScriptCommand("system", "scriptlevel", "int", "Returns the current script level (call depth).", ",", True))
            r(New ScriptCommand("system", "language", "str", "Returns the current game language suffix.", ",", True))
            r(New ScriptCommand("system", "fileexists", "bool", {New ScriptArgument("filePath", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns if the specified file (including extension) exists (relative to the GameMode's ContentPath).", ",", True))
        End Sub

        Private Shared Sub DoEnvironment()
            ' Commands:
            r(New ScriptCommand("environment", "setweather", {New ScriptArgument("weatherType", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the weather type of the current map."))
            r(New ScriptCommand("environment", "setregionweather", {New ScriptArgument("weatherID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the weather of the current region."))
            r(New ScriptCommand("environment", "resetregionweather", "Resets the weather to be based on the current season."))
            r(New ScriptCommand("environment", "setseason", {New ScriptArgument("seasonID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the season. Use -1 as the argument to change back to the default season."))
            r(New ScriptCommand("environment", "setcanfly", {New ScriptArgument("canfly", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""CanFly"" parameter of the current map."))
            r(New ScriptCommand("environment", "setcandig", {New ScriptArgument("candig", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""CanDig"" parameter of the current map."))
            r(New ScriptCommand("environment", "setcanteleport", {New ScriptArgument("canteleport", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""CanTeleport"" parameter of the current map."))
            r(New ScriptCommand("environment", "setwildpokemongrass", {New ScriptArgument("canencounter", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""WildPokemonGrass"" parameter of the current map."))
            r(New ScriptCommand("environment", "setwildpokemonwater", {New ScriptArgument("canencounter", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""WildPokemonWater"" parameter of the current map."))
            r(New ScriptCommand("environment", "setwildpokemoneverywhere", {New ScriptArgument("canencounter", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""WildPokemonFloor"" parameter of the current map."))
            r(New ScriptCommand("environment", "setisdark", {New ScriptArgument("isDark", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""IsDark"" parameter of the current map."))
            r(New ScriptCommand("environment", "setrenderdistance", {New ScriptArgument("distance", ScriptArgument.ArgumentTypes.Str, {"0-4", "tiny", "small", "normal", "far", "extreme"})}.ToList(), "Sets the render distance."))
            r(New ScriptCommand("environment", "toggledarkness", "Toggles the ""IsDark"" parameter of the current map."))
            r(New ScriptCommand("environment", "setdaytime", {New ScriptArgument("daytime", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the daytime to use for the Outside EnvironmentType (0). Can be 1-4, any other number resets to the default daytime."))
            r(New ScriptCommand("environment", "setenvironmenttype", {New ScriptArgument("environmenttype", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the ""EnvironmentType"" parameter of the map, which also changes the sky texture and sometimes adds particles. Value can be 0-5."))
            ' Constructs:
            r(New ScriptCommand("environment", "daytime", "str", "Returns the current DayTime of the game.", ",", True))
            r(New ScriptCommand("environment", "daytimeid", "int", "Returns the current DayTimeID of the game.", ",", True))
            r(New ScriptCommand("environment", "season", "str", "Returns the current Season of the game.", ",", True))
            r(New ScriptCommand("environment", "seasonid", "int", "Returns the current SeasonID of the game.", ",", True))
            r(New ScriptCommand("environment", "day", "str", "Returns the current day of the week.", ",", True))
            r(New ScriptCommand("environment", "dayofyear", "int", "Returns the current day of the year.", ",", True))
            r(New ScriptCommand("environment", "dayinformation", "str", "Returns the current day of the week and DayTime of the game.", ",", True))
            r(New ScriptCommand("environment", "week", "str", "Returns the current week of the year.", ",", True))
            r(New ScriptCommand("environment", "hour", "str", "Returns the current hour in 24-hour time.", ",", True))
            r(New ScriptCommand("environment", "year", "str", "Returns the current year.", ",", True))
            r(New ScriptCommand("environment", "weather", "str", "Returns the Weather of the current map.", ",", True))
            r(New ScriptCommand("environment", "mapweather", "str", "Returns the Weather of the current map.", ",", True))
            r(New ScriptCommand("environment", "currentmapweather", "str", "Returns the Weather of the current map.", ",", True))
            r(New ScriptCommand("environment", "weatherid", "int", "Returns the WeatherID of the current map.", ",", True))
            r(New ScriptCommand("environment", "mapweatherid", "int", "Returns the WeatherID of the current map.", ",", True))
            r(New ScriptCommand("environment", "currentmapweatherid", "int", "Returns the WeatherID of the current map.", ",", True))
            r(New ScriptCommand("environment", "regionweather", "str", "Returns the Weather of the current region.", ",", True))
            r(New ScriptCommand("environment", "regionweatherid", "str", "Returns the WeatherID of the current region.", ",", True))
            r(New ScriptCommand("environment", "canfly", "bool", "Returns the ""CanFly"" parameter of the current map.", ",", True))
            r(New ScriptCommand("environment", "candig", "bool", "Returns the ""CanDig"" parameter of the current map.", ",", True))
            r(New ScriptCommand("environment", "canteleport", "bool", "Returns the ""CanTeleport"" parameter of the current map.", ",", True))
            r(New ScriptCommand("environment", "wildpokemongrass", "bool", "Returns the ""WildPokemonGrass"" parameter of the current map.", ",", True))
            r(New ScriptCommand("environment", "wildpokemonwater", "bool", "Returns the ""WildPokemonWater"" parameter of the current map.", ",", True))
            r(New ScriptCommand("environment", "wildpokemoneverywhere", "bool", "Returns the ""WildPokemonEverywhere"" parameter of the current map.", ",", True))
            r(New ScriptCommand("environment", "isdark", "bool", "Returns the ""IsDark"" parameter of the current map.", ",", True))
        End Sub

        Private Shared Sub DoPlayer()
            ' Commands:
            r(New ScriptCommand("player", "receivepokedex", "Makes the Pokédex accessible for the player."))
            r(New ScriptCommand("player", "receivepokegear", "Makes the Pokégear accessible for the player."))
            r(New ScriptCommand("player", "renamerival", "Opens the rival rename screen."))
            r(New ScriptCommand("player", "wearskin", {New ScriptArgument("skin", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the player skin temporarily."))
            r(New ScriptCommand("player", "setskin", {New ScriptArgument("skin", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the player skin permanently."))
            r(New ScriptCommand("player", "move", {New ScriptArgument("steps", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Starts the player movement."))
            r(New ScriptCommand("player", "moveasync", {New ScriptArgument("steps", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Starts the async player movement."))
            r(New ScriptCommand("player", "turn", {New ScriptArgument("turns", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds to the direction the player faces and starts the turning."))
            r(New ScriptCommand("player", "turnasync", {New ScriptArgument("turns", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds to the direction the player faces and starts the async turning."))
            r(New ScriptCommand("player", "turnto", {New ScriptArgument("facing", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the direction the player faces and starts the turning."))
            r(New ScriptCommand("player", "turntoasync", {New ScriptArgument("facing", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the direction the player faces and starts the async turning."))
            r(New ScriptCommand("player", "warp", {New ScriptArgument("MapPath", ScriptArgument.ArgumentTypes.Str, True, "Current map."),
                                               New ScriptArgument("X", ScriptArgument.ArgumentTypes.Sng, True, "~"),
                                               New ScriptArgument("Y", ScriptArgument.ArgumentTypes.Sng, True, "~"),
                                               New ScriptArgument("Z", ScriptArgument.ArgumentTypes.Sng, True, "~"),
                                               New ScriptArgument("Rotations", ScriptArgument.ArgumentTypes.Int, {"0-3"}, True, "0"),
                                               New ScriptArgument("WarpSound", ScriptArgument.ArgumentTypes.Int, {"0-3"}, True, "0")}.ToList(), "Warps the player to a new location on a new map and changes the facing afterwards. To get relative coordinates, enter a ""~""."))
            r(New ScriptCommand("player", "stopmovement", "Stops the player movement."))
            r(New ScriptCommand("player", "addmoney", {New ScriptArgument("amount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given amount to the player's money."))
            r(New ScriptCommand("player", "removemoney", {New ScriptArgument("amount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the given amount from the player's money."))
            r(New ScriptCommand("player", "addcoins", {New ScriptArgument("amount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given amount to the player's coins."))
            r(New ScriptCommand("player", "removecoins", {New ScriptArgument("amount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the given amount from the player's coins."))
            r(New ScriptCommand("player", "setspeed", {New ScriptArgument("speed", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the movement speed of the player. The default is ""1""."))
            r(New ScriptCommand("player", "resetspeed", "Resets the movement speed of the player to the default speed, which is ""1""."))
            r(New ScriptCommand("player", "setmovement", {New ScriptArgument("x", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("y", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("z", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the direction the player will move next regardless of facing."))
            r(New ScriptCommand("player", "resetmovement", "Resets the player movement to the default movement directions."))
            r(New ScriptCommand("player", "preventmovement", "Makes the player unable to move, while still keeping control over the menu, interactions etc."))
            r(New ScriptCommand("player", "allowmovement", "Gives the player back their ability to move after using @Player.PreventMovement."))
            r(New ScriptCommand("player", "getbadge", {New ScriptArgument("badgeID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given Badge to the player's Badges and displays a message."))
            r(New ScriptCommand("player", "removebadge", {New ScriptArgument("badgeID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the given Badge from the player's Badges."))
            r(New ScriptCommand("player", "addbadge", {New ScriptArgument("badgeID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given Badge from the player's Badges."))
            r(New ScriptCommand("player", "addfrontieremblem", {New ScriptArgument("frontierEmblemID", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("SilverOrGold", ScriptArgument.ArgumentTypes.Bool, True, "1")}.ToList(), "Adds a frontier emblem (silver or gold) to the player's emblems. Second argument can be 0 = silver, 1 = gold"))
            r(New ScriptCommand("player", "removefrontieremblem", {New ScriptArgument("frontierEmblemID", ScriptArgument.ArgumentTypes.Int), New ScriptArgument("SilverOrGold", ScriptArgument.ArgumentTypes.Bool, True, "")}.ToList(), "Removes a frontier emblem from the player's emblems. Second argument can be 0 = silver, 1 = gold. Without it, both silver and gold emblems will be removed."))
            r(New ScriptCommand("player", "achieveemblem", {New ScriptArgument("emblemName", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Makes the player achieve an emblem (GameJolt only)."))
            r(New ScriptCommand("player", "addbp", {New ScriptArgument("amount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given amount to the player's Battle Points."))
            r(New ScriptCommand("player", "removebp", {New ScriptArgument("amount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the given amount from the player's Battle Points."))
            r(New ScriptCommand("player", "showrod", {New ScriptArgument("rodID", ScriptArgument.ArgumentTypes.Int, {"0-2"})}.ToList(), "Displays a Fishing Rod on the screen."))
            r(New ScriptCommand("player", "hiderod", "Hides the Fishing Rod."))
            r(New ScriptCommand("player", "save", "Saves the game."))
            r(New ScriptCommand("player", "setrivalname", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the rival's name."))
            r(New ScriptCommand("player", "setrivalskin", {New ScriptArgument("skin", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the rival's skin."))
            r(New ScriptCommand("player", "setgender", {New ScriptArgument("gender", ScriptArgument.ArgumentTypes.Str, {"0-2, Male, Female, Other"})}.ToList(), "Sets the rival's skin."))
            r(New ScriptCommand("player", "setopacity", {New ScriptArgument("opacity", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the player entity's opacity."))
            r(New ScriptCommand("player", "setdifficulty", {New ScriptArgument("difficultyLevel", ScriptArgument.ArgumentTypes.Int, {"0-2"})}.ToList(), "Sets the difficulty level for the player."))
            r(New ScriptCommand("player", "quitgame", {New ScriptArgument("doFade", ScriptArgument.ArgumentTypes.Bool, True, "")}.ToList(), "Quits the game and goes back to the Main Menu (with optionally a fade out and in)."))
            r(New ScriptCommand("player", "dowalkanimation", {New ScriptArgument("walkAnimation", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Enables or disables the player's walking animation when walking or during a @player.move command."))
            r(New ScriptCommand("player", "removeitemdata", {New ScriptArgument("levelPath", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("itemIndex", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Makes the specified item index of the specified map spawn again after it has been found."))

            ' Constructs:
            r(New ScriptCommand("player", "position", "sngarr", {New ScriptArgument("coordinate", ScriptArgument.ArgumentTypes.StrArr, {"x", "y", "z"}, True, "")}.ToList(), "Returns the position of the player. The normal coordinate combination is ""X,Y,Z"".", ",", True))
            r(New ScriptCommand("player", "hasbadge", "bool", {New ScriptArgument("badgeID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the player owns a specific Badge.", ",", True))
            r(New ScriptCommand("player", "hasfrontieremblem", "bool", {New ScriptArgument("frontierEmblemID", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("SilverOrGold", ScriptArgument.ArgumentTypes.Bool, True, "")}.ToList(), "Returns if the player owns a specific frontier emblem. Without the second argument, returns true if the player owns either silver or gold.", ",", True))
            r(New ScriptCommand("player", "skin", "str", "Returns the current skin the player wears.", ",", True))
            r(New ScriptCommand("player", "velocity", "sng", "Returns the player's velocity (steps until the player movement ends).", ",", True))
            r(New ScriptCommand("player", "speed", "sng", "Returns the player's movement speed (divided by 0.04F).", ",", True))
            r(New ScriptCommand("player", "isrunning", "bool", "Returns if the player is currently running.", ",", True))
            r(New ScriptCommand("player", "ismoving", "bool", "Returns if the player is currently moving.", ",", True))
            r(New ScriptCommand("player", "facing", "int", "Returns the direction the player is facing.", ",", True))
            r(New ScriptCommand("player", "compass", "str", "Returns ""north"", ""east"", ""south"" or ""east"" depending on the direction the player is facing.", ",", True))
            r(New ScriptCommand("player", "money", "int", "Returns the player's money.", ",", True))
            r(New ScriptCommand("player", "coins", "int", "Returns the player's coins.", ",", True))
            r(New ScriptCommand("player", "name", "str", "Returns the player's name", ",", True))
            r(New ScriptCommand("player", "gender", "str", "Returns the player's gender (Male, Female, Other)", ",", True))
            r(New ScriptCommand("player", "bp", "int", "Returns the amount of Battle Points the player owns.", ",", True))
            r(New ScriptCommand("player", "badges", "int", "Returns the amount of Badges the player owns", ",", True))
            r(New ScriptCommand("player", "thirdperson", "bool", "Returns if the game is currently played in third person.", ",", True))
            r(New ScriptCommand("player", "rival", "str", "Returns the rival's name.", ",", True))
            r(New ScriptCommand("player", "rivalname", "str", "Returns the rival's name.", ",", True))
            r(New ScriptCommand("player", "ot", "str", "Returns the player's Original Trainer value.", ",", True))
            r(New ScriptCommand("player", "gamejoltid", "str", "Returns the player's GameJolt ID.", ",", True))
            r(New ScriptCommand("player", "haspokedex", "bool", "Returns if the player received the Pokédex.", ",", True))
            r(New ScriptCommand("player", "haspokegear", "bool", "Returns if the player received the Pokégear.", ",", True))
            r(New ScriptCommand("player", "lastrestplace", "bool", "Returns the last rest location the player has visited in the following format: ""map.dat,x,y,z"".", ",", True))
        End Sub

        Private Shared Sub DoNPC()
            ' Commands:
            r(New ScriptCommand("npc", "remove", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the selected NPC from the map."))
            r(New ScriptCommand("npc", "position", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                                New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                                New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Moves the selected NPC to a different place on the map. To get relative coordinates, enter a ""~""."))
            r(New ScriptCommand("npc", "warp", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                                New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                                New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Moves the selected NPC to a different place on the map. To get relative coordinates, enter a ""~""."))
            r(New ScriptCommand("npc", "addtoposition", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                        New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                                        New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                                        New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Adds the given coordinates to the position of the given NPC. To get relative coordinates, enter a ""~""."))
            r(New ScriptCommand("npc", "register", {New ScriptArgument("registerData", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Registers NPC data. Format: {MapFile|NPCID|Action(""position"",""remove"")|addition)"))
            r(New ScriptCommand("npc", "unregister", {New ScriptArgument("registerData", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Unregisters NPC data. Format: {MapFile|NPCID|Action(""position"",""remove"")|addition)"))
            r(New ScriptCommand("npc", "wearskin", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("skin", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the skin of the selected NPC."))
            r(New ScriptCommand("npc", "move", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("steps", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Starts NPC movement of the selected NPC."))
            r(New ScriptCommand("npc", "setmovey", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("distance", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the distance that the selected NPC should move in the Y direction."))
            r(New ScriptCommand("npc", "moveasync", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("steps", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Starts async NPC movement of the selected NPC."))
            r(New ScriptCommand("npc", "turn", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("facing", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the face direction of the selected NPC."))
            r(New ScriptCommand("npc", "spawn", {New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                             New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                             New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng),
                                             New ScriptArgument("actionValue", ScriptArgument.ArgumentTypes.Int, True, "0"),
                                             New ScriptArgument("additionalValue", ScriptArgument.ArgumentTypes.Str, True, ""),
                                             New ScriptArgument("TextureID", ScriptArgument.ArgumentTypes.Str, True, "0"),
                                             New ScriptArgument("AnimateIdle", ScriptArgument.ArgumentTypes.Bool, True, "false"),
                                             New ScriptArgument("Rotation", ScriptArgument.ArgumentTypes.Int, True, "0"),
                                             New ScriptArgument("Name", ScriptArgument.ArgumentTypes.Str, True, ""),
                                             New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int, True, "0"),
                                             New ScriptArgument("Movement", ScriptArgument.ArgumentTypes.Str, {"Pokeball", "Still", "Looking", "FacePlayer", "Walk", "Straight", "Turning"}, True, "Still")}.ToList(), "Spawns a new NPC with the given conditions."))
            r(New ScriptCommand("npc", "setspeed", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("speed", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the speed of an NPC. The default is ""1""."))
            r(New ScriptCommand("npc", "setscale", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                          New ScriptArgument("xS", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("yS", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("zS", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the Scale property of the selected NPC."))

            ' Constructs:
            r(New ScriptCommand("npc", "position", "sngArr", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the position of the selected NPC.", ",", True))
            r(New ScriptCommand("npc", "exists", "bool", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if an NPC with the given ID exists on the map.", ",", True))
            r(New ScriptCommand("npc", "ismoving", "bool", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the selected NPC is moving.", ",", True))
            r(New ScriptCommand("npc", "moved", "sng", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of steps the selected NPC still has to move.", ",", True))
            r(New ScriptCommand("npc", "skin", "str", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the skin of the selected NPC.", ",", True))
            r(New ScriptCommand("npc", "facing", "int", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the direction the selected NPC is facing.", ",", True))
            r(New ScriptCommand("npc", "ID", "int", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the NPC ID for the selected NPC.", ",", True))
            r(New ScriptCommand("npc", "name", "str", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the name of the selected NPC.", ",", True))
            r(New ScriptCommand("npc", "action", "str", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the action value of the selected NPC.", ",", True))
            r(New ScriptCommand("npc", "additionalvalue", "int", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the additional value of the selected NPC.", ",", True))
            r(New ScriptCommand("npc", "movement", "str", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the movement type of the selected NPC.", ",", True))
            r(New ScriptCommand("npc", "hasmoverectangles", "bool", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the selected NPC has any movement rectangles.", ",", True))
            r(New ScriptCommand("npc", "trainertexture", "str", {New ScriptArgument("trainerfile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the texture name of the given trainer. Trainer file starts at the ""Trainer\"" path and must not have the "".trainer"" extension.", ",", True))
        End Sub

        Private Shared Sub DoRadio()
            ' Commands:
            r(New ScriptCommand("radio", "allowchannel", {New ScriptArgument("channel", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Allows a Radio channel on the map."))
            r(New ScriptCommand("radio", "blockchannel", {New ScriptArgument("channel", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Blocks a Radio channel on the map."))
            ' Constructs:
            r(New ScriptCommand("radio", "currentchannel", "str", "Returns the name of the channel that is currently playing.", "", True))
        End Sub

        Private Shared Sub DoPokedex()
            ' Commands:
            r(New ScriptCommand("pokedex", "setautodetect", {New ScriptArgument("autodetect", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets if the Pokédex registers seen Pokémon in wild or trainer battles."))
            r(New ScriptCommand("pokedex", "changeentry", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("Type", ScriptArgument.ArgumentTypes.Int), New ScriptArgument("ForceChange", ScriptArgument.ArgumentTypes.Bool, True, "false")}.ToList(), "Changes a Pokédex Entry."))
            ' Constructs:
            r(New ScriptCommand("pokedex", "caught", "int", "Returns the amount of Pokémon registered as caught by the player.", "", True))
            r(New ScriptCommand("pokedex", "seen", "int", "Returns the amount of Pokémon registered as seen by the player.", "", True))
            r(New ScriptCommand("pokedex", "shiny", "int", "Returns the amount of Pokémon registered as Shiny by the player.", "", True))
            r(New ScriptCommand("pokedex", "dexcaught", "int", {New ScriptArgument("dexIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Pokémon registered as caught by the player for a specific Pokédex.", "", True))
            r(New ScriptCommand("pokedex", "dexseen", "int", {New ScriptArgument("dexIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Pokémon registered as seen by the player for a specific Pokédex.", "", True))
            r(New ScriptCommand("pokedex", "getheight", "sng", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the height of the Pokémon.", "", True))
            r(New ScriptCommand("pokedex", "getweight", "sng", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the weight of the Pokémon.", "", True))
            r(New ScriptCommand("pokedex", "getentry", "str", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the description of the Pokémon.", "", True))
            r(New ScriptCommand("pokedex", "getspecies", "str", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the species of the Pokémon.", "", True))
            r(New ScriptCommand("pokedex", "getname", "str", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the name of the Pokémon.", "", True))
            r(New ScriptCommand("pokedex", "getability", "int", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str), New ScriptArgument("requestType", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns one of the abilities of the Pokémon based on the requestType. 0 = random 1st or 2nd, 1 = first ability, 2 = second ability, 3 = hidden ability.", "", True))
            r(New ScriptCommand("pokedex", "pokemoncaught", "bool", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns if the specified Pokémon has been caught.", "", True))
            r(New ScriptCommand("pokedex", "pokemonseen", "bool", {New ScriptArgument("ID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns if the specified Pokémon has been seen.", "", True))

        End Sub

        Private Shared Sub DoMath()
            ' Constructs:
            r(New ScriptCommand("math", "int", "int", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Converts the argument to an integer.", "", True))
            r(New ScriptCommand("math", "sng", "sng", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Converts the argument to a single.", "", True))
            r(New ScriptCommand("math", "abs", "sng", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Returns the absolute value of a number.", "", True))
            r(New ScriptCommand("math", "ceiling", "int", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Rounds the value up to the next integer.", "", True))
            r(New ScriptCommand("math", "floor", "int", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Rounds the value down to the next integer.", "", True))
            r(New ScriptCommand("math", "isint", "bool", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if the expression is an integer.", "", True))
            r(New ScriptCommand("math", "issng", "bool", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if the expression is a single.", "", True))
            r(New ScriptCommand("math", "clamp", "sng", {New ScriptArgument("number", ScriptArgument.ArgumentTypes.Sng),
                                                     New ScriptArgument("min", ScriptArgument.ArgumentTypes.Sng),
                                                     New ScriptArgument("max", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Clamps a number.", "", True))
            r(New ScriptCommand("math", "rollover", "sng", {New ScriptArgument("number", ScriptArgument.ArgumentTypes.Sng),
                                                     New ScriptArgument("min", ScriptArgument.ArgumentTypes.Sng),
                                                     New ScriptArgument("max", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Rolls a number over with min and max properties.", "", True))
        End Sub

        Private Shared Sub DoRival()
            ' Constructs:
            r(New ScriptCommand("rival", "name", "str", "Returns the rival's name", "", True))
            r(New ScriptCommand("rival", "skin", "str", "Returns the rival's skin", "", True))
        End Sub

        Private Shared Sub DoDaycare()
            ' Commands:
            r(New ScriptCommand("Daycare", "Clean", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Cleans all data for the given Daycare. This doesn't remove the data, just rearranges it."))
            r(New ScriptCommand("Daycare", "ClearData", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Clears all the data for one Daycare. That includes the Pokémon stored there and a potential Egg."))
            r(New ScriptCommand("Daycare", "LeavePokémon", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                                  New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int),
                                                                  New ScriptArgument("PartyIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes a Pokémon from the player's party and fills the given Daycare's slot with that Pokémon."))
            r(New ScriptCommand("Daycare", "RemoveEgg", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the egg from the given Daycare permanently."))
            r(New ScriptCommand("Daycare", "TakeEgg", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the Egg from the Daycare and adds it to the player's party."))
            r(New ScriptCommand("Daycare", "TakePokémon", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                                 New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Takes the given Pokémon from the given Daycare to the player's party."))
            r(New ScriptCommand("Daycare", "Call", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Initializes a call with the Daycare. This checks if the Daycare is registered in the Pokégear."))

            ' Constructs:
            r(New ScriptCommand("Daycare", "PokémonID", "int", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Pokémon ID of a Pokémon in the Daycare.", ",", True))
            r(New ScriptCommand("Daycare", "PokémonName", "str", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the name of a Pokémon in the Daycare.", ",", True))
            r(New ScriptCommand("Daycare", "PokémonSprite", "str", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the sprite of a Pokémon in the Daycare.", ",", True))
            r(New ScriptCommand("Daycare", "ShinyIndicator", "str", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Shiny Indicator of a Pokémon in the Daycare (either ""N"" or ""S"").", ",", True))
            r(New ScriptCommand("Daycare", "CountPokémon", "int", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Pokémon in the Daycare.", ",", True))
            r(New ScriptCommand("Daycare", "HasPokémon", "bool", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns whether the given Daycare's slot is occupied.", ",", True))
            r(New ScriptCommand("Daycare", "CanSwim", "bool", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the Pokémon in the Daycare can swim.", ",", True))
            r(New ScriptCommand("Daycare", "HasEgg", "bool", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the Daycare has an Egg.", ",", True))
            r(New ScriptCommand("Daycare", "GrownLevels", "int", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of levels the Pokémon has grown in the Daycare.", ",", True))
            r(New ScriptCommand("Daycare", "CurrentLevel", "int", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the current level of the Pokémon in the Daycare.", ",", True))
            r(New ScriptCommand("Daycare", "CanBreed", "int", {New ScriptArgument("DaycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("DaycareSlot", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the chance the Pokémon in the Daycare can breed (in %).", ",", True))
        End Sub
        Private Shared Sub DoOverworldPokemon()
            ' Commands:
            r(New ScriptCommand("Player", "Show", "Shows the following Pokémon."))
            r(New ScriptCommand("Player", "Hide", "Hides the following Pokémon."))
            r(New ScriptCommand("Player", "Toggle", "Toggles the following Pokémon's visibility."))
        End Sub
        Private Shared Sub DoPokemon()
            ' Commands:
            r(New ScriptCommand("pokemon", "cry", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Plays the cry of the given Pokémon."))
            r(New ScriptCommand("pokemon", "remove", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the Pokémon at the given party index."))
            r(New ScriptCommand("pokemon", "add", {New ScriptArgument("pokemonData", ScriptArgument.ArgumentTypes.PokemonData)}.ToList(), "Adds the Pokémon to the player's party."))
            r(New ScriptCommand("pokemon", "add", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Int),
                                               New ScriptArgument("level", ScriptArgument.ArgumentTypes.Int),
                                               New ScriptArgument("method", ScriptArgument.ArgumentTypes.Str, True, "random reason"),
                                               New ScriptArgument("ballID", ScriptArgument.ArgumentTypes.Int, True, "5"),
                                               New ScriptArgument("location", ScriptArgument.ArgumentTypes.Str, True, "Current location"),
                                               New ScriptArgument("isEgg", ScriptArgument.ArgumentTypes.Bool, True, "false"),
                                               New ScriptArgument("trainerName", ScriptArgument.ArgumentTypes.Str, True, "Current TrainerName")}.ToList(), "Adds the Pokémon with the given arguments to the player's party."))
            r(New ScriptCommand("pokemon", "setadditionalvalue", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                              New ScriptArgument("data", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Set the additional data for a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setnickname", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("nickName", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Set the nickname for a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setstat", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                   New ScriptArgument("statName", ScriptArgument.ArgumentTypes.Str, {"maxhp", "hp", "chp", "atk", "attack", "def", "defense", "spatk", "specialattack", "spattack", "spdef", "specialdefense", "spdefense", "speed"}),
                                                   New ScriptArgument("statValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Set the value of a stat for a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "clear", "Clears the player's party."))
            r(New ScriptCommand("pokemon", "removeattack", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                        New ScriptArgument("attackIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the move at the given index from a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "removeattackid", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                        New ScriptArgument("attackID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes a move with the given ID from a Pokémon in the player's party if available."))
            r(New ScriptCommand("pokemon", "clearattacks", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Clears all moves from a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "addattack", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                     New ScriptArgument("attackID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the move to a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setshiny", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                    New ScriptArgument("shiny", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the Shiny value of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setshinyall", {New ScriptArgument("shiny", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the Shiny value of all Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "changelevel", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("newLevel", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the level of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "gainexp", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                   New ScriptArgument("expAmount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds Experience to the Experience value of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setnature", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                     New ScriptArgument("natureID", ScriptArgument.ArgumentTypes.Int, {"0-24"})}.ToList(), "Sets the Nature of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "npctrade", {New ScriptArgument("ownPokemonID", ScriptArgument.ArgumentTypes.Int),
                                                    New ScriptArgument("oppPokemonID", ScriptArgument.ArgumentTypes.Int),
                                                    New ScriptArgument("level", ScriptArgument.ArgumentTypes.Int),
                                                    New ScriptArgument("genderID", ScriptArgument.ArgumentTypes.Int, {"0-2"}),
                                                    New ScriptArgument("attackIDs", ScriptArgument.ArgumentTypes.IntArr),
                                                    New ScriptArgument("shiny", ScriptArgument.ArgumentTypes.Bool),
                                                    New ScriptArgument("OT", ScriptArgument.ArgumentTypes.Str),
                                                    New ScriptArgument("TrainerName", ScriptArgument.ArgumentTypes.Str),
                                                    New ScriptArgument("CatchBallID", ScriptArgument.ArgumentTypes.Int),
                                                    New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Int),
                                                    New ScriptArgument("location", ScriptArgument.ArgumentTypes.Str),
                                                    New ScriptArgument("method", ScriptArgument.ArgumentTypes.Str),
                                                    New ScriptArgument("nickname", ScriptArgument.ArgumentTypes.Str),
                                                    New ScriptArgument("message1", ScriptArgument.ArgumentTypes.Str),
                                                    New ScriptArgument("message2", ScriptArgument.ArgumentTypes.Str),
                                                    New ScriptArgument("register", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Trades a Pokémon with an NPC.", "|", False))
            r(New ScriptCommand("pokemon", "rename", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str, {"0-5", "last"}),
                                                  New ScriptArgument("OTcheck", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Opens the Name Rater rename feature."))
            r(New ScriptCommand("pokemon", "read", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str, {"[empty],0-5"})}.ToList(), "Displays the reader's dialogue."))
            r(New ScriptCommand("pokemon", "heal", {New ScriptArgument("pokemonIndicies", ScriptArgument.ArgumentTypes.IntArr)}.ToList(), "Heals the given Pokémon."))
            r(New ScriptCommand("pokemon", "setfriendship", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str),
                                                         New ScriptArgument("friendship", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the friendship value for a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "addfriendship", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str),
                                                         New ScriptArgument("friendship", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds to the frienship value of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "select", {New ScriptArgument("canExit", ScriptArgument.ArgumentTypes.Bool, True, "false"),
                                                         New ScriptArgument("canChooseEgg", ScriptArgument.ArgumentTypes.Bool, True, "true"),
                                                         New ScriptArgument("canChooseFainted", ScriptArgument.ArgumentTypes.Bool, True, "true"),
                                                         New ScriptArgument("canlearnAttack", ScriptArgument.ArgumentTypes.Int, True, "-1")}.ToList(), "Opens the Pokémon select screen. If canLearnAttack is set to an attack ID, it will be visible which Pokémon can learn that move."))
            r(New ScriptCommand("pokemon", "selectmove", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                         New ScriptArgument("canChooseHMMove", ScriptArgument.ArgumentTypes.Bool),
                                                         New ScriptArgument("canExit", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Opens the Move Selection screen."))
            r(New ScriptCommand("pokemon", "calcstats", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Recalculates the stats for the given Pokémon."))
            r(New ScriptCommand("pokemon", "learnattack", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("attackID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the move to the Pokémon's learnset."))
            r(New ScriptCommand("pokemon", "setgender", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("genderID", ScriptArgument.ArgumentTypes.Int, {"0-2"})}.ToList(), "Sets a Pokémon's gender."))
            r(New ScriptCommand("pokemon", "setability", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("abilityID", ScriptArgument.ArgumentTypes.Int, {"0-188"})}.ToList(), "Sets the Ability of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setev", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("evStat", ScriptArgument.ArgumentTypes.Str, {"hp", "atk", "attack", "def", "defense", "spatk", "specialattack", "spattack", "spdef", "specialdefense", "spdefense", "speed"}),
                                                       New ScriptArgument("evValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the value of the Effort Value stat of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setallevs", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("evValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the value of all Effort Value stats of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "addev", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("evStat", ScriptArgument.ArgumentTypes.Str, {"hp", "atk", "attack", "def", "defense", "spatk", "specialattack", "spattack", "spdef", "specialdefense", "spdefense", "speed"}),
                                                       New ScriptArgument("evValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the evValue argument to the Effort Value stat of a Pokémon in the player's party and makes sure it's legal."))
            r(New ScriptCommand("pokemon", "setiv", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("ivStat", ScriptArgument.ArgumentTypes.Str, {"hp", "atk", "attack", "def", "defense", "spatk", "specialattack", "spattack", "spdef", "specialdefense", "spdefense", "speed"}),
                                                       New ScriptArgument("ivValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the value of the Individual Value stat of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setallivs", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("ivStat", ScriptArgument.ArgumentTypes.Str, {"hp", "atk", "attack", "def", "defense", "spatk", "specialattack", "spattack", "spdef", "specialdefense", "spdefense", "speed"}),
                                                       New ScriptArgument("ivValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the value of all Individual Value stats of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "registerhalloffame", "Registers the current party as new Hall of Fame entry."))
            r(New ScriptCommand("pokemon", "setot", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("newOT", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the Original Trainer of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setitem", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("itemID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the item of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setitemData", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("itemData", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the data of the item of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setcatchtrainer", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("catchTrainer", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the Catch Trainer of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setcatchball", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("ballID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the Catch Ball of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setcatchmethod", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("method", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the Catch Method of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setcatchplace", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("location", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the Catch Location of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setcatchlocation", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("location", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the Catch Location of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setstatus", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("status", ScriptArgument.ArgumentTypes.Str, {"brn", "frz", "prz", "psn", "bpsn", "slp", "fnt"})}.ToList(), "Sets the status of a Pokémon in the player's party. Setting that to ""fnt"" (Fainted) will also set the Pokémon's HP to 0."))
            r(New ScriptCommand("pokemon", "newroaming", {New ScriptArgument("roamerID", ScriptArgument.ArgumentTypes.Str),
                                                      New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Str),
                                                      New ScriptArgument("level", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("regionID", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("startMap", ScriptArgument.ArgumentTypes.Str),
                                                      New ScriptArgument("shiny", ScriptArgument.ArgumentTypes.Bool, True, "-1"),
                                                      New ScriptArgument("scriptPath", ScriptArgument.ArgumentTypes.Str, True)}.ToList(), "Adds a new Roaming Pokémon to the list of Roaming Pokémon.", "|", False))
            r(New ScriptCommand("pokemon", "evolve", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                  New ScriptArgument("evolutionTrigger", ScriptArgument.ArgumentTypes.Str, {"level", "none", "item", "trade"}, True, "level"),
                                                  New ScriptArgument("evolutionArgument", ScriptArgument.ArgumentTypes.Str, True, "")}.ToList(), "Tries to evolve a Pokémon with the given conditions."))
            r(New ScriptCommand("pokemon", "levelup", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                  New ScriptArgument("levelAmount", ScriptArgument.ArgumentTypes.Int, {"[empty],0 - MaxLevel GameRule"}, True, "1")}.ToList(), "Raises a Pokémon's level by the given amount, checks for learnable level up moves."))
            r(New ScriptCommand("pokemon", "reload", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Reloads the data for a Pokémon in the player's party to apply changes."))
            r(New ScriptCommand("pokemon", "clone", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Clones the given Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "sendtostorage", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                  New ScriptArgument("boxIndex", ScriptArgument.ArgumentTypes.Int, True, "")}.ToList(), "Sends the given Pokémon to the storage system, in the specified box if given."))
            r(New ScriptCommand("pokemon", "addtostorage", {New ScriptArgument("boxIndex", ScriptArgument.ArgumentTypes.Int, True, ""),
                                                  New ScriptArgument("pokemonData", ScriptArgument.ArgumentTypes.PokemonData)}.ToList(), "Adds a Pokémon with the given Pokémon data to the storage system, in the specified box if given."))
            r(New ScriptCommand("pokemon", "addtostorage", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Int),
                                               New ScriptArgument("level", ScriptArgument.ArgumentTypes.Int),
                                               New ScriptArgument("method", ScriptArgument.ArgumentTypes.Str, True, "random reason"),
                                               New ScriptArgument("ballID", ScriptArgument.ArgumentTypes.Int, True, "5"),
                                               New ScriptArgument("location", ScriptArgument.ArgumentTypes.Str, True, "Current location"),
                                               New ScriptArgument("isEgg", ScriptArgument.ArgumentTypes.Bool, True, "false"),
                                               New ScriptArgument("trainerName", ScriptArgument.ArgumentTypes.Str, True, "Current TrainerName"),
                                               New ScriptArgument("heldItemID", ScriptArgument.ArgumentTypes.Int, True, "0"),
                                               New ScriptArgument("isShiny", ScriptArgument.ArgumentTypes.Bool, True, "false")}.ToList(), "Adds a Pokémon with the given Pokémon properties to the storage system."))
            r(New ScriptCommand("pokemon", "ride", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Int, True, "-1")}.ToList(), "Makes a Pokémon in the player's party use the field move Ride. If the argument is left empty, the first Pokémon who knows Ride gets selected."))


            ' Constructs:
            r(New ScriptCommand("pokemon", "id", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the ID of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "number", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the ID of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "data", "pokemonData", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the save data for a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "gender", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the gender for a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "genderchance", "int", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the Male/Female chance (1-100) of a Pokémon as defined by its Data file.", ",", True))
            r(New ScriptCommand("pokemon", "level", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the level of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "hasfullhp", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if a Pokémon in the player's party has a full Hit Point count.", ",", True))
            r(New ScriptCommand("pokemon", "hp", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Hit Points of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "atk", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Attack stat of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "def", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Defense stat of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "spatk", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Special Attack stat of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "spdef", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Special Defense stat of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "speed", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Speed stat of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "maxhp", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Maximum Hit Points of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "isegg", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Pokémon in the players party is an Egg.", ",", True))
            r(New ScriptCommand("pokemon", "additionaldata", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the additional data for the Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "mailsendername", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the name of the sender of a mail if there's one on the Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "mailsenderot", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the OT of the sender of a mail if there's one on the Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "nickname", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the nickname of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "hasnickname", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if a Pokémon in the player's party has a nickname.", ",", True))
            r(New ScriptCommand("pokemon", "name", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the name of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "ot", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Original Trainer of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "trainer", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the trainer of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "itemid", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the ID of the item of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "friendship", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the friendship value of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "itemname", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the item name of the item of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "catchball", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the ID of the Poké Ball the Pokémon was caught in.", ",", True))
            r(New ScriptCommand("pokemon", "catchmethod", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the method the Pokémon was caught.", ",", True))
            r(New ScriptCommand("pokemon", "catchlocation", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the location the Pokémon was caught in.", ",", True))
            r(New ScriptCommand("pokemon", "hasattack", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                             New ScriptArgument("attackID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the Pokémon in the player's party knows the specified move.", ",", True))
            r(New ScriptCommand("pokemon", "countattacks", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Counts the moves the Pokémon knows.", ",", True))
            r(New ScriptCommand("pokemon", "attackname", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                             New ScriptArgument("moveIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the name of the move of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "levelattacks", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                             New ScriptArgument("maxLevel", ScriptArgument.ArgumentTypes.Int, True, "-1")}.ToList(), "Returns a list of move IDs separated by commas that a Pokémon in the player's party can learn at or below its current level/the level specified by the maxLevel argument.", ",", True))
            r(New ScriptCommand("pokemon", "canlearnattack", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                                      New ScriptArgument("attackID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the Pokémon can learn the specified move.", ",", True))
            r(New ScriptCommand("pokemon", "isshiny", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the Pokémon is Shiny.", ",", True))
            r(New ScriptCommand("pokemon", "nature", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the nature of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "ownpokemon", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if a Pokémon in the player's party was caught by the player.", ",", True))
            r(New ScriptCommand("pokemon", "islegendary", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if a Pokémon in the player's party is a legendary Pokémon.", ",", True))
            r(New ScriptCommand("pokemon", "freeplaceinparty", "bool", "Checks if the player has a free place in their party.", ",", True))
            r(New ScriptCommand("pokemon", "nopokemon", "bool", "Checks if the player has no Pokémon in their party.", ",", True))
            r(New ScriptCommand("pokemon", "count", "int", "Returns the amount of Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "countbattle", "int", "Returns the amount Pokémon that can battle in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "has", "bool", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the player has the specified Pokémon in their party.", ",", True))
            r(New ScriptCommand("pokemon", "selected", "int", "Returns the index of the selector in the player's party. (Set with @pokemon.select)", ",", True))
            r(New ScriptCommand("pokemon", "selectedmove", "int", "Returns the index of the move selected. (Set with @pokemon.selectmove)", ",", True))
            r(New ScriptCommand("pokemon", "hasegg", "bool", "Returns if the player has an Egg in their party.", ",", True))
            r(New ScriptCommand("pokemon", "maxpartylevel", "int", "Returns the maximum level a Pokémon has in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "evhp", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Hit Point Effort Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "evatk", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Attack Effort Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "evdef", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Defense Effort Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "evspatk", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Special Attack Effort Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "evspdef", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Special Defense Effort Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "evspeed", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Speed Effort Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "ivhp", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Jit Point Individual Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "ivatk", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Attack Individual Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "ivdef", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Defense Individual Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "ivspatk", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Special Attack Individual Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "ivspdef", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Special Defense Individual Values of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "spawnwild", "pokemonData", "Returns the data for a Pokémon that can spawn in the current location.", ",", True))
            r(New ScriptCommand("pokemon", "itemdata", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the data of the item of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "countHallofFame", "int", "Counts the Hall of Fame entries.", ",", True))
            r(New ScriptCommand("pokemon", "learnedTutorMove", "bool", "Returns if a Pokémon just learned a tutor move (from @screen.teachmoves)", ",", True))
            r(New ScriptCommand("pokemon", "totalexp", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Experience a Pokémon received.", ",", True))
            r(New ScriptCommand("pokemon", "needexp", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Experience a Pokémon needs in order to level up.", ",", True))
            r(New ScriptCommand("pokemon", "currentexp", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Experience the Pokémon colleted for its current level.", ",", True))
            r(New ScriptCommand("pokemon", "generateFrontier", "pokemonData", {New ScriptArgument("level", ScriptArgument.ArgumentTypes.Int),
                                                                           New ScriptArgument("pokemonClass", ScriptArgument.ArgumentTypes.Int),
                                                                           New ScriptArgument("IDList", ScriptArgument.ArgumentTypes.IntArr, True, "")}.ToList(), "Generates a Frontier Pokémon within the set IDList (all Pokémon, if IDList is Nothing).", ",", True))
            r(New ScriptCommand("pokemon", "spawnwild", "pokemonData", "Returns the data for a Pokémon that can spawn in the current location.", ",", True))
            r(New ScriptCommand("pokemon", "spawn", "pokemonData", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Int),
                                                                New ScriptArgument("level", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the data for a Pokémon.", ",", True))
            r(New ScriptCommand("pokemon", "otmatch", "bool,int,str", {New ScriptArgument("checkOT", ScriptArgument.ArgumentTypes.Str),
                                                           New ScriptArgument("returnType", ScriptArgument.ArgumentTypes.Str, {"has", "id", "number", "name", "maxhits"})}.ToList(), "Returns if the player owns a Pokémon with the given Original Trainer.", ",", True))
            r(New ScriptCommand("pokemon", "randomot", "str", "Returns a random OT (5 digit number).", ",", True))
            r(New ScriptCommand("pokemon", "status", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the status condition of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "canevolve", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                  New ScriptArgument("evolutionTrigger", ScriptArgument.ArgumentTypes.Str, {"level", "none", "item", "trade"}, True, "level"),
                                                  New ScriptArgument("evolutionArgument", ScriptArgument.ArgumentTypes.Str, True, "")}.ToList(), "Returns if the Pokémon can be evolved via the given evolution method.", ",", True))
            r(New ScriptCommand("pokemon", "type1", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the first type of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "type2", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the second type of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "istype", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                          New ScriptArgument("type", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if a Pokémon in the player's party has a specific type.", ",", True))
            r(New ScriptCommand("pokemon", "isroaming", "bool", {New ScriptArgument("roamerID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if the given roaming Pokémon is still active.", ",", True))
            r(New ScriptCommand("pokemon", "fullyhealed", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str, True, "")}.ToList(), "Checks if a specific Pokémon or all Pokémon in the party are fully healed.", ",", True))

        End Sub

        Private Shared Sub r(ByVal s As ScriptCommand)
            Scripts.Add(s)
        End Sub

#End Region

        Private Class ScriptCommand

            Private _mainClass As String = ""
            Private _subClass As String = ""
            Private _argumentSeparator As String = ","
            Private _isConstruct As Boolean = False
            Private _description As String = ""
            Private _returnType As String = ""

            Private _arguments As New List(Of ScriptArgument)

            Public Sub New(ByVal mainClass As String, ByVal subClass As String, ByVal Description As String)
                Me.New(mainClass, subClass, "", New List(Of ScriptArgument), Description)
            End Sub

            Public Sub New(ByVal mainClass As String, ByVal subClass As String, ByVal Arguments As List(Of ScriptArgument), ByVal Description As String, Optional ByVal ArgumentSeparator As String = ",", Optional ByVal IsConstruct As Boolean = False)
                Me.New(mainClass, subClass, "", Arguments, Description, ArgumentSeparator, IsConstruct)
            End Sub

            Public Sub New(ByVal mainClass As String, ByVal subClass As String, Optional ByVal Description As String = "", Optional ByVal ArgumentSeparator As String = ",", Optional ByVal IsConstruct As Boolean = False)
                Me.New(mainClass, subClass, "", New List(Of ScriptArgument), Description, ArgumentSeparator, IsConstruct)
            End Sub

            Public Sub New(ByVal mainClass As String, ByVal subClass As String, ByVal returnType As String, Optional ByVal Description As String = "", Optional ByVal ArgumentSeparator As String = ",", Optional ByVal IsConstruct As Boolean = False)
                Me.New(mainClass, subClass, returnType, New List(Of ScriptArgument), Description, ArgumentSeparator, IsConstruct)
            End Sub

            Public Sub New(ByVal mainClass As String, ByVal subClass As String, ByVal returnType As String, ByVal Arguments As List(Of ScriptArgument), ByVal Description As String, Optional ByVal ArgumentSeparator As String = ",", Optional ByVal IsConstruct As Boolean = False)
                Me._mainClass = mainClass
                Me._subClass = subClass
                Me._argumentSeparator = ArgumentSeparator
                Me._isConstruct = IsConstruct
                Me._arguments = Arguments
                Me._description = Description
                Me._returnType = returnType
            End Sub

            Public ReadOnly Property MatchesClass(ByVal mainClass As String, ByVal subClass As String) As Boolean
                Get
                    If Me._mainClass.ToLower() = mainClass.ToLower() And Me._subClass.ToLower() = subClass.ToLower() Then
                        Return True
                    End If
                    Return False
                End Get
            End Property

            Public ReadOnly Property MainClass() As String
                Get
                    Return Me._mainClass
                End Get
            End Property

            Public ReadOnly Property SubClass() As String
                Get
                    Return Me._subClass
                End Get
            End Property

            Public ReadOnly Property ArgumentSeparator() As String
                Get
                    Return Me._argumentSeparator
                End Get
            End Property

            Public ReadOnly Property IsConstruct() As Boolean
                Get
                    Return Me._isConstruct
                End Get
            End Property

            Public Overrides Function ToString() As String
                Dim args As String = ""
                For Each arg As ScriptArgument In Me._arguments
                    If args <> "" Then
                        args &= Me._argumentSeparator
                    End If
                    args &= arg.ToString()
                Next

                Dim des As String = ""
                If Me._description <> "" Then
                    des = " " & Me._description
                End If

                If Me._isConstruct = True Then
                    Dim c As String = "<" & Me._mainClass & "." & Me._subClass & "(" & args & ")>" & des

                    If Me._returnType <> "" Then
                        c = "(" & _returnType.ToLower() & ") " & c
                    End If

                    Return c
                Else
                    Return "@" & Me._mainClass & "." & Me._subClass & "(" & args & ")" & des
                End If
            End Function

        End Class

        Private Class ScriptArgument

            Public Enum ArgumentTypes
                Str
                Int
                Sng
                ItemCollection
                Bool
                Rec
                IntArr
                StrArr
                SngArr
                BoolArr
                PokemonData
                Arr
            End Enum

            Private _type As ArgumentTypes = ArgumentTypes.Str
            Private _isOptional As Boolean = False
            Private _defaultValue As String = ""
            Private _name As String = "emptyName"
            Private _validArguments As String()

            Public Sub New(ByVal Name As String, ByVal ArgumentType As ArgumentTypes, Optional ByVal IsOptional As Boolean = False, Optional ByVal DefaultValue As String = "")
                Me.New(Name, ArgumentType, {}, IsOptional, DefaultValue)
            End Sub

            Public Sub New(ByVal Name As String, ByVal ArgumentType As ArgumentTypes, ByVal ValidArguments() As String, Optional ByVal IsOptional As Boolean = False, Optional ByVal DefaultValue As String = "")
                Me._name = Name
                Me._type = ArgumentType
                Me._isOptional = IsOptional
                Me._defaultValue = DefaultValue
                Me._validArguments = ValidArguments
            End Sub

            ''' <summary>
            ''' The data type needed for this argument.
            ''' </summary>
            Public ReadOnly Property ArgumentType() As ArgumentTypes
                Get
                    Return Me._type
                End Get
            End Property

            ''' <summary>
            ''' The name of this argument.
            ''' </summary>
            Public ReadOnly Property Name() As String
                Get
                    Return Me._name
                End Get
            End Property

            ''' <summary>
            ''' Specifies if this argument is optional for the command or construct.
            ''' </summary>
            Public ReadOnly Property IsOptional() As Boolean
                Get
                    Return Me._isOptional
                End Get
            End Property

            ''' <summary>
            ''' Returns the default value for the argument if it is optional.
            ''' </summary>
            Public ReadOnly Property DefaultValue() As String
                Get
                    Return Me._defaultValue
                End Get
            End Property

            Public ReadOnly Property ValidArguments() As String()
                Get
                    Return Me._validArguments
                End Get
            End Property

            Public Overrides Function ToString() As String
                If Me._isOptional = True Then
                    Dim s As String = Me._type.ToString().ToLower() & " " & Me._name
                    If Me._defaultValue <> "" Then
                        s &= "=""" & Me._defaultValue & """"
                    End If
                    Return "[" & s & "]"
                Else
                    Return Me._type.ToString().ToLower() & " " & Me._name
                End If
            End Function

        End Class

        Shared Scripts As New List(Of ScriptCommand)

        ''' <summary>
        ''' Gets the help content for a script command or construct.
        ''' </summary>
        ''' <param name="inputCommand">class.subclass</param>
        Public Shared Function GetHelpContent(ByVal inputCommand As String, ByVal pageSize As Integer) As String
            If inputCommand.ToLower().StartsWith("constructs") Then
                Dim list As New List(Of String)
                For Each ScriptCommand In Scripts
                    If ScriptCommand.IsConstruct = True Then
                        If list.Contains(ScriptCommand.MainClass.ToLower()) = False Then
                            list.Add(ScriptCommand.MainClass.ToLower())
                        End If
                    End If
                Next
                Dim str As String = ""
                Dim cList As New List(Of String)
                For Each l As String In list
                    If cList.Contains(l.ToLower()) = False Then
                        If str <> "" Then
                            str &= "; "
                        End If
                        str &= l
                        cList.Add(l.ToLower())
                    End If
                Next
                Return "Constructs: " & str
            ElseIf inputCommand.ToLower().StartsWith("commands") Then
                Dim list As New List(Of String)
                For Each ScriptCommand In Scripts
                    If ScriptCommand.IsConstruct = False Then
                        If list.Contains(ScriptCommand.MainClass.ToLower()) = False Then
                            list.Add(ScriptCommand.MainClass.ToLower())
                        End If
                    End If
                Next
                Dim str As String = ""
                Dim cList As New List(Of String)
                For Each l As String In list
                    If cList.Contains(l.ToLower()) = False Then
                        If str <> "" Then
                            str &= "; "
                        End If
                        str &= l
                        cList.Add(l.ToLower())
                    End If
                Next
                Return "Commands: " & str
            ElseIf inputCommand = "" Then
                Return "Type ""constructs"" or ""commands"" to view the list of main classes, or type a command or construct to view its details. Put a "","" afterwards, to view different pages."
            End If

            If inputCommand.Contains(".") = True Then
                Dim mainClass As String = inputCommand.Remove(inputCommand.IndexOf("."))
                Dim subClass As String = inputCommand.Remove(0, inputCommand.IndexOf(".") + 1)

                Dim count As Integer = 1
                Dim selected As Integer = 1

                If subClass.Contains(",") = True Then
                    If StringHelper.IsNumeric(subClass.GetSplit(1)) = True Then
                        selected = CInt(subClass.GetSplit(1))
                        subClass = subClass.GetSplit(0)
                    Else
                        Logger.Log(Logger.LogTypes.Warning, "ScriptLibrary.vb: The ""Selected"" argument has to be numeric.")
                    End If
                End If

                Dim validScriptCommands As New List(Of ScriptCommand)
                For Each ScriptCommand In Scripts
                    If ScriptCommand.MatchesClass(mainClass, subClass) = True Then
                        validScriptCommands.Add(ScriptCommand)
                    End If
                Next

                If validScriptCommands.Count > 0 Then
                    count = validScriptCommands.Count
                    selected = selected.Clamp(1, count)

                    Dim s As String = validScriptCommands(selected - 1).ToString()

                    If count > 1 Then
                        s = "(" & selected & "/" & count & ") " & s
                    End If
                    Return s
                Else
                    Return "No help content available for """ & mainClass & "." & subClass & """."
                End If
            Else
                Dim mainClass As String = inputCommand
                Dim page As Integer = 1

                If mainClass.Contains(",") = True Then
                    If StringHelper.IsNumeric(mainClass.GetSplit(1)) Then
                        page = CInt(mainClass.GetSplit(1))
                        mainClass = mainClass.GetSplit(0)
                    Else
                        Logger.Log(Logger.LogTypes.Warning, "ScriptLibrary.vb: The ""Page"" argument has to be numeric.")
                    End If
                End If

                Dim str As String = ""

                Dim validScriptCommands As New List(Of ScriptCommand)
                For Each ScriptCommand In Scripts
                    If ScriptCommand.MainClass.ToLower() = mainClass.ToLower() Then
                        validScriptCommands.Add(ScriptCommand)
                    End If
                Next
                Dim PageCount As Integer = CInt(Math.Ceiling(validScriptCommands.Count / pageSize))
                page = page.Clamp(1, PageCount)

                If validScriptCommands.Count > 0 Then
                    For i = (page - 1) * pageSize To page * pageSize
                        If i <= validScriptCommands.Count - 1 Then
                            Dim ScriptCommand As ScriptCommand = validScriptCommands(i)
                            Dim s As String = ""

                            If ScriptCommand.IsConstruct = True Then
                                s = "<" & ScriptCommand.MainClass & "." & ScriptCommand.SubClass & ">"
                            Else
                                s = "@" & ScriptCommand.MainClass & "." & ScriptCommand.SubClass
                            End If

                            If str.Contains(s) = False Then
                                If str <> "" Then
                                    str &= "; "
                                End If
                                str &= s
                            End If
                        End If
                    Next

                    If PageCount > 1 Then
                        Return mainClass & ": (" & page & "/" & PageCount & ") " & str
                    Else
                        Return mainClass & ": " & str
                    End If
                Else
                    Return "No Commands or Constructs available for """ & mainClass & """."
                End If
            End If
        End Function

    End Class

End Namespace