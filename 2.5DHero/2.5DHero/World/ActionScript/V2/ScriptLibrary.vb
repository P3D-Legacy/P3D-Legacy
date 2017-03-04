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
            r(New ScriptCommand("filesystem", "pathsplit", "str", {New ScriptArgument("index", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("path", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the part of the path that is at the position of index.", ",", True))
            r(New ScriptCommand("filesystem", "pathsplitcount", "int", {New ScriptArgument("path", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the amount of parts in the given path.", ",", True))
            r(New ScriptCommand("filesystem", "pathup", "str", {New ScriptArgument("path", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the parent path to the given path if possible.", ",", True))
        End Sub

        Private Shared Sub DoTitle()
            ' Commands:
            r(New ScriptCommand("title", "add", {New ScriptArgument("text", ScriptArgument.ArgumentTypes.Str, True, "Sample Text"),
                                              New ScriptArgument("delay", ScriptArgument.ArgumentTypes.Sng, True, "20.0"),
                                              New ScriptArgument("R", ScriptArgument.ArgumentTypes.Int, True, "255"),
                                              New ScriptArgument("G", ScriptArgument.ArgumentTypes.Int, True, "255"),
                                              New ScriptArgument("B", ScriptArgument.ArgumentTypes.Int, True, "255"),
                                              New ScriptArgument("scale", ScriptArgument.ArgumentTypes.Sng, True, "10.0"),
                                              New ScriptArgument("isCentered", ScriptArgument.ArgumentTypes.Bool, True, "true"),
                                              New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng, True, "0.0"),
                                              New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng, True, "0.0")}.ToList(), "Adds a new title for the game to display during gameplay."))
            r(New ScriptCommand("title", "clear", "Clears all titles that are currently being displayed."))
        End Sub

        Private Shared Sub DoCamera()
            ' Commands:
            r(New ScriptCommand("camera", "set", {New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("yaw", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("pitch", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the given properties of the camera."))
            r(New ScriptCommand("camera", "reset", "Resets the camera to the default setting."))
            r(New ScriptCommand("camera", "setyaw", {New ScriptArgument("yaw", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the Yaw property of the camera."))
            r(New ScriptCommand("camera", "setpitch", {New ScriptArgument("pitch", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the Pitch property of the camera."))
            r(New ScriptCommand("camera", "setposition", {New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                              New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the Position property of the camera."))
            r(New ScriptCommand("camera", "setx", {New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the X Position property of the camera."))
            r(New ScriptCommand("camera", "sety", {New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the Y Position property of the camera."))
            r(New ScriptCommand("camera", "setz", {New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the Z Position property of the camera."))
            r(New ScriptCommand("camera", "togglethirdperson", {New ScriptArgument("doUpdate", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Toggles the third person camera."))
            r(New ScriptCommand("camera", "activatethirdperson", {New ScriptArgument("doUpdate", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Activates the third person camera."))
            r(New ScriptCommand("camera", "deactivatethirdperson", {New ScriptArgument("doUpdate", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Deactivates the third person camera."))
            r(New ScriptCommand("camera", "setthirdperson", {New ScriptArgument("thirdPerson", ScriptArgument.ArgumentTypes.Bool),
                                                         New ScriptArgument("doUpdate", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the camera to the desired status."))
            r(New ScriptCommand("camera", "fix", "Fixes the camera to the current position."))
            r(New ScriptCommand("camera", "defix", "Defixes the camera so that it clips behind the player again."))
            r(New ScriptCommand("camera", "togglefix", "Sets the fix state of the camera to the opposite of the current state."))
            r(New ScriptCommand("camera", "update", "Updates the camera."))
            r(New ScriptCommand("camera", "setfocus", {New ScriptArgument("focusType", ScriptArgument.ArgumentTypes.Str, {"player", "npc", "entity"}),
                                                   New ScriptArgument("focusID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Focuses the camera on an object. "))
            r(New ScriptCommand("camera", "setfocustype", {New ScriptArgument("focusType", ScriptArgument.ArgumentTypes.Str, {"player", "npc", "entity"})}.ToList(), "Sets the focus type for the camera."))
            r(New ScriptCommand("camera", "setfocusid", {New ScriptArgument("focusID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the ID of the focus target for the camera."))
            r(New ScriptCommand("camera", "settoplayerfacing", "Sets the Yaw of the camera to accommodate the player's facing."))

            ' Constructs:
            r(New ScriptCommand("camera", "isfixed", "bool", "Returns if the camera is fixed to a specific position.", ",", True))
            r(New ScriptCommand("camera", "x", "sng", "Returns the current X position of the camera.", ",", True))
            r(New ScriptCommand("camera", "y", "sng", "Returns the current Y position of the camera.", ",", True))
            r(New ScriptCommand("camera", "z", "sng", "Returns the current Z position of the camera.", ",", True))
            r(New ScriptCommand("camera", "yaw", "sng", "Returns the current Yaw rotation of the camera.", ",", True))
            r(New ScriptCommand("camera", "pitch", "sng", "Returns the current Pitch rotation of the camera.", ",", True))
            r(New ScriptCommand("camera", "thirdperson", "bool", "Returns if the camera is in third person mode.", ",", True))
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
            r(New ScriptCommand("text", "debug", {New ScriptArgument("text", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Prints the ""text"" argument to the immediate window console."))
            r(New ScriptCommand("text", "log", {New ScriptArgument("text", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Logs the ""text"" argument into the log.dat file."))
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
            r(New ScriptCommand("music", "play", {New ScriptArgument("musicFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the currently playing music to a new one."))
            r(New ScriptCommand("music", "setmusicloop", {New ScriptArgument("musicFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the map musicloop to a new musicfile."))
            r(New ScriptCommand("music", "stop", "Stops the music playback."))
            r(New ScriptCommand("music", "pause", "Pauses the music playback."))
            r(New ScriptCommand("music", "resume", "Resumes the music playback."))
        End Sub

        Private Shared Sub DoBattle()
            ' Commands:
            r(New ScriptCommand("battle", "starttrainer", {New ScriptArgument("trainerFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Initializes a trainer interaction and checks if the player has already beaten that trainer."))
            r(New ScriptCommand("battle", "trainer", {New ScriptArgument("trainerFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Initializes a trainer battle."))
            r(New ScriptCommand("battle", "wild", {New ScriptArgument("pokemonData", ScriptArgument.ArgumentTypes.PokemonData),
                                               New ScriptArgument("musicloop", ScriptArgument.ArgumentTypes.Str, True, "")}.ToList(), "Initializes the battle with a wild Pokémon."))
            r(New ScriptCommand("battle", "wild", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Int),
                                               New ScriptArgument("level", ScriptArgument.ArgumentTypes.Int),
                                               New ScriptArgument("shiny", ScriptArgument.ArgumentTypes.Int, True, "-1"),
                                               New ScriptArgument("musicloop", ScriptArgument.ArgumentTypes.Str, True, ""),
                                               New ScriptArgument("introtype", ScriptArgument.ArgumentTypes.Int, True, "0-10")}.ToList(), "Initializes the battle with a wild Pokémon."))
            r(New ScriptCommand("battle", "setvar", {New ScriptArgument("varName", ScriptArgument.ArgumentTypes.Str, {"canrun", "cancatch", "canblackout", "canreceiveexp", "canuseitems", "frontiertrainer", "divebattle", "inversebattle, custombattlemusic, hiddenabilitychance"}),
                                                 New ScriptArgument("varValue", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets a battle value."))
            ' Constructs:
            r(New ScriptCommand("battle", "defeatmessage", "str", {New ScriptArgument("trainerFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the defeat message of the trainer loaded from the given ""trainerFile"".", ",", True))
            r(New ScriptCommand("battle", "intromessage", "str", {New ScriptArgument("trainerFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the intro message of the trainer loaded from the given ""trainerFile"".", ",", True))
            r(New ScriptCommand("battle", "outromessage", "str", {New ScriptArgument("trainerFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the outro message of the trainer loaded from the given ""trainerFile"".", ",", True))
            r(New ScriptCommand("battle", "won", "bool", "Returns ""true"" if the player won the last battle. Returns ""false"" otherwise.", ",", True))
        End Sub

        Private Shared Sub DoLevel()
            ' Commands:
            r(New ScriptCommand("level", "wait", {New ScriptArgument("ticks", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Makes the level idle for the duration of the given ticks."))
            r(New ScriptCommand("level", "update", "Updates the level and all entities once."))
            r(New ScriptCommand("level", "waitforevents", "Makes the level idle until every NPC movement is done."))
            r(New ScriptCommand("level", "waitforsave", "Makes the level idle until the current saving of an GameJolt save is done."))
            r(New ScriptCommand("level", "reload", "Reloads the current map."))
            r(New ScriptCommand("level", "setsafari", {New ScriptArgument("safari", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets if the current map is a Safari Zone (influences battle style)."))
            ' Constructs:
            r(New ScriptCommand("level", "mapfile", "str", "Returns the mapfile of the currently loaded map.", ",", True))
            r(New ScriptCommand("level", "levelfile", "str", "Returns the mapfile of the currently loaded map.", ",", True))
            r(New ScriptCommand("level", "filename", "str", "Returns only the name of the current map file, without path and extension.", ",", True))
            r(New ScriptCommand("level", "riding", "bool", "Returns if the player is Riding a Pokémon right now.", ",", True))
            r(New ScriptCommand("level", "surfing", "bool", "Returns if the player is Suring on a Pokémon right now.", ",", True))
        End Sub

        Private Shared Sub DoEntity()
            ' Commands:
            r(New ScriptCommand("entity", "showmessagebulb", {New ScriptArgument("bulbID", ScriptArgument.ArgumentTypes.Int, {"0-11"}),
                                                          New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Displays a message bulb in the world.", "|"))
            r(New ScriptCommand("entity", "warp", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                          New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Warps the entity to a new location on the map."))
            r(New ScriptCommand("entity", "scale", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                          New ScriptArgument("xS", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("yS", ScriptArgument.ArgumentTypes.Sng),
                                                          New ScriptArgument("zS", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Changes the Scale property of the selected entity."))
            r(New ScriptCommand("entity", "remove", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the entity from the map once it updates."))
            r(New ScriptCommand("entity", "setid", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("newID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the entity ID of the selected entity to a new ID."))
            r(New ScriptCommand("entity", "setopacity", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                     New ScriptArgument("opacity", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the Opacity property of the selected entity. ""opacity"" in %."))
            r(New ScriptCommand("entity", "setvisible", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("visible", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the Visible property of the selected entity."))
            r(New ScriptCommand("entity", "setadditionalvalue", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("additionalValue", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the AdditionalValue property of the selected entity."))
            r(New ScriptCommand("entity", "setcollision", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("collision", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the Collision property of the selected entity."))
            r(New ScriptCommand("entity", "settetxure", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("textureIndex", ScriptArgument.ArgumentTypes.Str),
                                                New ScriptArgument("textureName", ScriptArgument.ArgumentTypes.Str),
                                                New ScriptArgument("rX", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("rY", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("rWidth", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("rHeight", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the texture in the selected entity's texture array. Argument example: 0,0,[nilllzz,0,10,32,32]"))
            r(New ScriptCommand("entity", "addtoposition", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int),
                                                         New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                                         New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                                         New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Adds the given coordinates to the position of the given entity."))
            ' Constructs:
            r(New ScriptCommand("entity", "visible", "bool", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the selected entity is visible.", ",", True))
            r(New ScriptCommand("entity", "opacity", "int", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the opacity property of the selected entity.", ",", True))
            r(New ScriptCommand("entity", "position", "sngArr", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the position of the selected entity in the pattern ""x,y,z"".", ",", True))
            r(New ScriptCommand("entity", "positionx", "sng", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the X position of the selected entity.", ",", True))
            r(New ScriptCommand("entity", "positiony", "sng", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Y position of the selected entity.", ",", True))
            r(New ScriptCommand("entity", "positionz", "sng", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Z position of the selected entity.", ",", True))
            r(New ScriptCommand("entity", "scale", "sngArr", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the scale of the selected entity in the pattern ""x,y,z"".", ",", True))
            r(New ScriptCommand("entity", "additionalvalue", "str", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the additional value of the selected entity.", ",", True))
            r(New ScriptCommand("entity", "collision", "bool", {New ScriptArgument("entityID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the collision property of the selected entity.", ",", True))
        End Sub

        Private Shared Sub DoPhone()
            ' Constructs:
            r(New ScriptCommand("phone", "callflag", "str", "Returns, if the Pokégear is calling or getting called. Values: ""calling"", ""receiving""", ",", True))
            r(New ScriptCommand("phone", "got", "bool", "Returns if the player got the Pokégear.", ",", True))
        End Sub

        Private Shared Sub DoItem()
            ' Commands:
            r(New ScriptCommand("item", "give", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Int),
                                             New ScriptArgument("Amount", ScriptArgument.ArgumentTypes.Int, True, "1")}.ToList(), "Adds the given amount of items to the player's inventory."))
            r(New ScriptCommand("item", "remove", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Int),
                                             New ScriptArgument("Amount", ScriptArgument.ArgumentTypes.Int, True, "1"),
                                               New ScriptArgument("showMessage", ScriptArgument.ArgumentTypes.Bool, True, "true")}.ToList(), "Removes the given amount of items from the player's inventory. Displays a message afterwards, if ""showMessage"" is true."))
            r(New ScriptCommand("item", "clearitem", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Int, True, "")}.ToList(), "Clears all items with the given ID from the player's inventory. Clears the whole inventory if ItemID is empty."))
            r(New ScriptCommand("item", "messagegive", {New ScriptArgument("ItemID", ScriptArgument.ArgumentTypes.Int),
                                             New ScriptArgument("Amount", ScriptArgument.ArgumentTypes.Int, True, "1")}.ToList(), "Displays a message for getting the specified amount of items."))
            r(New ScriptCommand("item", "repel", {New ScriptArgument("RepelItemID", ScriptArgument.ArgumentTypes.Int, {"20", "42", "43"})}.ToList(), "Adds the steps of the Repel to the Repel steps of the player."))
        End Sub

        Private Shared Sub DoInventory()
            ' Constructs:
            r(New ScriptCommand("inventory", "countitem", "int", {New ScriptArgument("itemID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of the Item with the given ID in the player's inventory.", ",", True))
            r(New ScriptCommand("inventory", "countitems", "int", "Counts all items in the player's inventory.", ",", True))
            r(New ScriptCommand("inventory", "name", "str", {New ScriptArgument("itemID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the name of an Item by its ItemID.", ",", True))
            r(New ScriptCommand("inventory", "ID", "int", {New ScriptArgument("itemName", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Returns the ID of an Item by its Name.", ",", True))
        End Sub

        Private Shared Sub DoChat()
            ' Commands:
            r(New ScriptCommand("chat", "clear", "Clears the chat."))
        End Sub

        Private Shared Sub DoScreen()
            ' Commands:
            r(New ScriptCommand("screen", "storagesystem", "Opens the storage system."))
            r(New ScriptCommand("screen", "apricornkurt", "Opens the Apricorn Screen."))
            r(New ScriptCommand("screen", "trade", {New ScriptArgument("tradeItems", ScriptArgument.ArgumentTypes.ItemCollection),
                                                New ScriptArgument("canBuy", ScriptArgument.ArgumentTypes.Bool),
                                                New ScriptArgument("canSell", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Opens a new trade screen with the given items in stock. Item collection: {itemID|amount|price}{...}..., amount and price are default for -1", ","))
            r(New ScriptCommand("screen", "townmap", {New ScriptArgument("regionList", ScriptArgument.ArgumentTypes.StrArr)}.ToList(), "Opens the map screen with the given regions."))
            r(New ScriptCommand("screen", "donation", "Opens the donation screen."))
            r(New ScriptCommand("screen", "blackout", "Opens the blackout screen and warps the player back to the last saveplace."))
            r(New ScriptCommand("screen", "fadein", {New ScriptArgument("fadeSpeed", ScriptArgument.ArgumentTypes.Int, True, "5")}.ToList(), "Fades the screen back in."))
            r(New ScriptCommand("screen", "fadeout", {New ScriptArgument("fadeSpeed", ScriptArgument.ArgumentTypes.Int, True, "5")}.ToList(), "Fades the screen to black."))
            r(New ScriptCommand("screen", "setfade", {New ScriptArgument("fadeValue", ScriptArgument.ArgumentTypes.Int, {"0-255"})}.ToList(), "Sets the alpha value of the screen fade."))
            r(New ScriptCommand("screen", "credits", {New ScriptArgument("ending", ScriptArgument.ArgumentTypes.Str, True, "Johto")}.ToList(), "Displays the credits scene."))
            r(New ScriptCommand("screen", "halloffame", {New ScriptArgument("displayEntryIndex", ScriptArgument.ArgumentTypes.Int, True, "")}.ToList(), "Displays the Hall of Fame. If the argument ""displayEntryIndex"" is not empty, it displays only that entry."))
            r(New ScriptCommand("screen", "teachmoves", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                     New ScriptArgument("moveIDs", ScriptArgument.ArgumentTypes.IntArr, True, "")}.ToList(), "Displays a move learn screen. If the argument ""moveIDs"" is left empty, it defaults to the Pokémon's tutor moves."))
            r(New ScriptCommand("screen", "mailsystem", "Opens the PC Inbox screen."))
            r(New ScriptCommand("screen", "pvp", "Opens the PvP lobby screen (not finished yet, don't use)."))
            r(New ScriptCommand("screen", "input", {New ScriptArgument("defaultName", ScriptArgument.ArgumentTypes.Str, True, ""),
                                                New ScriptArgument("inputMode", ScriptArgument.ArgumentTypes.Str, {"0-2", "name", "text", "numbers"}, True, "0"),
                                                New ScriptArgument("currentText", ScriptArgument.ArgumentTypes.Str, True, ""),
                                                New ScriptArgument("maxChars", ScriptArgument.ArgumentTypes.Int, True, "14")}.ToList(), "Displays the Input screen. The input can be retrieved with <system.lastinput>."))
            r(New ScriptCommand("screen", "mysteryevent", "Opens the Mystery Event screen."))
            r(New ScriptCommand("screen", "secretbase", "Opens the Secret Base screen."))
        End Sub

        Private Shared Sub DoScript()
            ' Commands:
            r(New ScriptCommand("script", "start", {New ScriptArgument("scriptFile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Starts a script with the given filename (without file extension)."))
            r(New ScriptCommand("script", "text", {New ScriptArgument("text", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Starts a script with a simple text to display."))
            r(New ScriptCommand("script", "run", {New ScriptArgument("scriptContent", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Runs script content. New lines are represented with ""^""."))
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
            r(New ScriptCommand("system", "isinsightscript", "bool", "Returns if the running script was triggred by the inSight function of an NPC.", ",", True))
            r(New ScriptCommand("system", "lastinput", "str", "Returns the last input received from the input screen (@screen.input).", ",", True))
            r(New ScriptCommand("system", "return", "str", "Returns the value set with the "":return"" switch.", ",", True))
            r(New ScriptCommand("system", "isint", "bool", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if the expression is an integer (Outdated, use <math.isint> instead).", ",", True))
            r(New ScriptCommand("system", "issng", "bool", {New ScriptArgument("expression", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if the expression is a single (Outdated, use <math.issng> instead).", ",", True))
            r(New ScriptCommand("system", "chrw", "str", {New ScriptArgument("charCodes", ScriptArgument.ArgumentTypes.IntArr)}.ToList(), "Converts Unicode CharCodes into a string.", ",", True))
            r(New ScriptCommand("system", "scriptlevel", "int", "Returns the current script level (call depth).", ",", True))
        End Sub

        Private Shared Sub DoEnvironment()
            ' Commands:
            r(New ScriptCommand("environment", "setweather", {New ScriptArgument("weatherType", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the weather type of the current map."))
            r(New ScriptCommand("environment", "setregionweather", {New ScriptArgument("weatherID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the weather of the current region."))
            r(New ScriptCommand("environment", "setcanfly", {New ScriptArgument("canfly", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""CanFly"" parameter of the current map."))
            r(New ScriptCommand("environment", "setcandig", {New ScriptArgument("candig", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""CanDig"" parameter of the current map."))
            r(New ScriptCommand("environment", "setcanteleport", {New ScriptArgument("canteleport", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""CanTeleport"" parameter of the current map."))
            r(New ScriptCommand("environment", "setwildpokemongrass", {New ScriptArgument("canencounter", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""WildPokemonGrass"" parameter of the current map."))
            r(New ScriptCommand("environment", "setwildpokemonwater", {New ScriptArgument("canencounter", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""WildPokemonWater"" parameter of the current map."))
            r(New ScriptCommand("environment", "setwildpokemoneverywhere", {New ScriptArgument("canencounter", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""WildPokemonEverywhere"" parameter of the current map."))
            r(New ScriptCommand("environment", "setisdark", {New ScriptArgument("isDark", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the ""IsDark"" parameter of the current map."))
            r(New ScriptCommand("environment", "setrenderdistance", {New ScriptArgument("distance", ScriptArgument.ArgumentTypes.Str, {"0-4", "tiny", "small", "normal", "far", "extreme"})}.ToList(), "Sets the render distance."))
            r(New ScriptCommand("environment", "toggledarkness", "Toggles the ""IsDark"" parameter of the current map."))

            ' Constructs:
            r(New ScriptCommand("environment", "daytime", "str", "Returns the current DayTime of the game.", ",", True))
            r(New ScriptCommand("environment", "daytimeID", "int", "Returns the current DayTimeID of the game.", ",", True))
            r(New ScriptCommand("environment", "season", "str", "Returns the current Season of the game.", ",", True))
            r(New ScriptCommand("environment", "seasonID", "int", "Returns the current SeasonID of the game.", ",", True))
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
            r(New ScriptCommand("player", "wearskin", {New ScriptArgument("skin", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the player skin."))
            r(New ScriptCommand("player", "setonlineskin", {New ScriptArgument("gamejoltID", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Changes the player skin to a skin downloaded for the GameJoltID."))
            r(New ScriptCommand("player", "move", {New ScriptArgument("steps", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Starts the player movement."))
            r(New ScriptCommand("player", "moveasync", {New ScriptArgument("steps", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Starts the async player movement."))
            r(New ScriptCommand("player", "turn", {New ScriptArgument("turns", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds to the direction the player faces and starts the turning."))
            r(New ScriptCommand("player", "turnasync", {New ScriptArgument("turns", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds to the direction the player faces and starts the async turning."))
            r(New ScriptCommand("player", "turnto", {New ScriptArgument("facing", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the direction the player faces and starts the turning."))
            r(New ScriptCommand("player", "turntoasync", {New ScriptArgument("facing", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Changes the direction the player faces and starts the async turning."))
            r(New ScriptCommand("player", "warp", {New ScriptArgument("mapfile", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Warps the player to a new map file."))
            r(New ScriptCommand("player", "warp", {New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                               New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                               New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Warps the player to a new location on the map. To get relative coordinates, enter a ""~""."))
            r(New ScriptCommand("player", "warp", {New ScriptArgument("mapfile", ScriptArgument.ArgumentTypes.Str),
                                               New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                               New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                               New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Warps the player to a new location on a new map. To get relative coordinates, enter a ""~""."))
            r(New ScriptCommand("player", "warp", {New ScriptArgument("mapfile", ScriptArgument.ArgumentTypes.Str),
                                               New ScriptArgument("x", ScriptArgument.ArgumentTypes.Sng),
                                               New ScriptArgument("y", ScriptArgument.ArgumentTypes.Sng),
                                               New ScriptArgument("z", ScriptArgument.ArgumentTypes.Sng),
                                               New ScriptArgument("facing", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Warps the player to a new location on a new map and changes the facing afterwards. To get relative coordinates, enter a ""~""."))
            r(New ScriptCommand("player", "stopmovement", "Stops the player movement."))
            r(New ScriptCommand("player", "addmoney", {New ScriptArgument("amount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given amount to the player's money."))
            r(New ScriptCommand("player", "setmovement", {New ScriptArgument("x", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("y", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("z", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the direction the player will move next regardless of facing."))
            r(New ScriptCommand("player", "resetmovement", "Resets the player movement to the default movement directions."))
            r(New ScriptCommand("player", "getbadge", {New ScriptArgument("badgeID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given Badge to the player's Badges and displays a message."))
            r(New ScriptCommand("player", "removebadge", {New ScriptArgument("badgeID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the given Badge from the player's Badges."))
            r(New ScriptCommand("player", "addbadge", {New ScriptArgument("badgeID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given Badge from the player's Badges."))
            r(New ScriptCommand("player", "achieveemblem", {New ScriptArgument("emblemName", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Makes the player achieve an emblem (GameJolt only)."))
            r(New ScriptCommand("player", "addbp", {New ScriptArgument("amount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the given amount to the player's Battle Points."))
            r(New ScriptCommand("player", "showrod", {New ScriptArgument("rodID", ScriptArgument.ArgumentTypes.Int, {"0-2"})}.ToList(), "Displays a Fishing Rod on the screen."))
            r(New ScriptCommand("player", "hiderod", "Hides the Fishing Rod."))
            r(New ScriptCommand("player", "showpokemonfollow", "Shows the following Pokémon."))
            r(New ScriptCommand("player", "hidepokemonfollow", "Hides the following Pokémon."))
            r(New ScriptCommand("player", "togglepokemonfollow", "Toggles the following Pokémon's visibility."))
            r(New ScriptCommand("player", "save", "Saves the game."))
            r(New ScriptCommand("player", "setrivalname", {New ScriptArgument("name", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the rival's name."))
            r(New ScriptCommand("player", "setopacity", {New ScriptArgument("opacity", ScriptArgument.ArgumentTypes.Sng)}.ToList(), "Sets the player entity's opacity."))

            ' Constructs:
            r(New ScriptCommand("player", "position", "sngarr", {New ScriptArgument("coordinate", ScriptArgument.ArgumentTypes.StrArr, {"x", "y", "z"}, True, "")}.ToList(), "Returns the position of the player. The normal coordinate combination is ""X,Y,Z"".", ",", True))
            r(New ScriptCommand("player", "hasbadge", "bool", {New ScriptArgument("badgeID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the player owns a specific Badge.", ",", True))
            r(New ScriptCommand("player", "skin", "str", "Returns the current skin the player wears.", ",", True))
            r(New ScriptCommand("player", "velocity", "sng", "Returns the player's velocity (steps until the player movement ends).", ",", True))
            r(New ScriptCommand("player", "ismoving", "bool", "Returns if the player moves.", ",", True))
            r(New ScriptCommand("player", "facing", "int", "Returns the direction the player is facing.", ",", True))
            r(New ScriptCommand("player", "compass", "str", "Returns ""north"", ""east"", ""south"" or ""east"" depending on the direction the player is facing.", ",", True))
            r(New ScriptCommand("player", "money", "int", "Returns the player's money.", ",", True))
            r(New ScriptCommand("player", "name", "str", "Returns the player's name", ",", True))
            r(New ScriptCommand("player", "gender", "int", "Returns the player's gender (0=male, 1=female)", ",", True))
            r(New ScriptCommand("player", "bp", "int", "Returns the amount of Battle Points the player owns.", ",", True))
            r(New ScriptCommand("player", "badges", "int", "Returns the amount of Badges the player owns", ",", True))
            r(New ScriptCommand("player", "thirdperson", "bool", "Returns if the game is currently played in third person.", ",", True))
            r(New ScriptCommand("player", "rival", "str", "Returns the rival's name.", ",", True))
            r(New ScriptCommand("player", "rivalname", "str", "Returns the rival's name.", ",", True))
            r(New ScriptCommand("player", "ot", "str", "Returns the player's Original Trainer value.", ",", True))
            r(New ScriptCommand("player", "gamejoltid", "str", "Returns the player's GameJolt ID.", ",", True))
            r(New ScriptCommand("player", "haspokedex", "bool", "Returns if the player received the Pokédex.", ",", True))
            r(New ScriptCommand("player", "haspokegear", "bool", "Returns if the player received the Pokégear.", ",", True))
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
            r(New ScriptCommand("npc", "setMoveY", {New ScriptArgument("npcID", ScriptArgument.ArgumentTypes.Int),
                                                New ScriptArgument("steps", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the steps the selected NPC should walk in the Y direction."))
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
            ' Constructs:
            r(New ScriptCommand("pokedex", "caught", "int", "Returns the amount of Pokémon registered as caught by the player.", "", True))
            r(New ScriptCommand("pokedex", "seen", "int", "Returns the amount of Pokémon registered as seen by the player.", "", True))
            r(New ScriptCommand("pokedex", "shiny", "int", "Returns the amount of Pokémon registered as Shiny by the player.", "", True))
            r(New ScriptCommand("pokedex", "dexcaught", "int", {New ScriptArgument("dexIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Pokémon registered as caught by the player for a specific Pokédex.", "", True))
            r(New ScriptCommand("pokedex", "dexseen", "int", {New ScriptArgument("dexIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Pokémon registered as seen by the player for a specific Pokédex.", "", True))
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
        End Sub

        Private Shared Sub DoDaycare()
            ' Commands:
            r(New ScriptCommand("daycare", "takeegg", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the Egg from the Day Care and adds it to the player's party."))
            r(New ScriptCommand("daycare", "takepokemon", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                                 New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Takes a Pokémon from the Day Care to the player's party."))
            r(New ScriptCommand("daycare", "leavepokemon", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                                  New ScriptArgument("PokemonDaycareIndex", ScriptArgument.ArgumentTypes.Int),
                                                                  New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes a Pokémon from the player's party and fills the given PokemonDaycareIndex with that Pokémon."))
            r(New ScriptCommand("daycare", "removeegg", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the Egg from the Day Care."))
            r(New ScriptCommand("daycare", "clean", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Cleans all data for the given Day Care. This doesn't remove the data, just rearranges it."))
            r(New ScriptCommand("daycare", "call", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Initializes a call with the Day Care. This checks if the Day Care is registered in the Pokégear."))
            r(New ScriptCommand("daycare", "cleardata", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Clears all the data for one Day Care. That includes the Pokémon stored there and a potential Egg."))

            ' Constructs:
            r(New ScriptCommand("daycare", "pokemonID", "int", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Pokémon ID of a Pokémon in the Day Care.", ",", True))
            r(New ScriptCommand("daycare", "pokemonName", "str", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the name of a Pokémon in the Day Care.", ",", True))
            r(New ScriptCommand("daycare", "pokemonSprite", "str", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the sprite of a Pokémon in the Day Care.", ",", True))
            r(New ScriptCommand("daycare", "shinyIndicator", "str", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Shiny Indicator of a Pokémon in the Day Care (either ""N"" or ""S"").", ",", True))
            r(New ScriptCommand("daycare", "countpokemon", "int", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of Pokémon in the Day Care.", ",", True))
            r(New ScriptCommand("daycare", "haspokemon", "bool", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the Pokémon ID of a Pokémon in the Day Care.", ",", True))
            r(New ScriptCommand("daycare", "canswim", "bool", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the Pokémon in the Day Care can swim.", ",", True))
            r(New ScriptCommand("daycare", "hasegg", "bool", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the Day Care has an Egg.", ",", True))
            r(New ScriptCommand("daycare", "grownlevels", "int", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the amount of levels the Pokémon has grown in the Day Care.", ",", True))
            r(New ScriptCommand("daycare", "currentlevel", "int", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the current level of the Pokémon in the Day Care.", ",", True))
            r(New ScriptCommand("daycare", "canbreed", "int", {New ScriptArgument("daycareID", ScriptArgument.ArgumentTypes.Int),
                                                               New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the chance the Pokémon in the Day Care can breed (in %).", ",", True))
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
            r(New ScriptCommand("pokemon", "setadditionaldata", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                              New ScriptArgument("data", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Set the additional data for a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setnickname", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("nickName", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Set the nickname for a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setstat", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                   New ScriptArgument("statName", ScriptArgument.ArgumentTypes.Str, {"maxhp", "hp", "chp", "atk", "attack", "def", "defense", "spatk", "specialattack", "spattack", "spdef", "specialdefense", "spdefense", "speed"}),
                                                   New ScriptArgument("statValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Set the value of a stat for a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "clear", "Clears the player's party."))
            r(New ScriptCommand("pokemon", "removeattack", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                        New ScriptArgument("attackIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Removes the move at the given index from a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "clearattacks", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Clears all moves from a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "addattack", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                     New ScriptArgument("attackID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the move to a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setshiny", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                    New ScriptArgument("shiny", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Sets the Shiny value of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "changelevel", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("newLevel", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the level of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "gainexp", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                   New ScriptArgument("expAmount", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds Experience to the Experience value of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setnature", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                     New ScriptArgument("natureID", ScriptArgument.ArgumentTypes.Int, {"0-24"})}.ToList(), "Sets the Nature of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "npcTrade", {New ScriptArgument("ownPokemonID", ScriptArgument.ArgumentTypes.Int),
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
            r(New ScriptCommand("pokemon", "hide", "Hides the following Pokémon."))
            r(New ScriptCommand("pokemon", "rename", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str, {"0-5", "last"}),
                                                  New ScriptArgument("OTcheck", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Opens the Name Rater rename feature."))
            r(New ScriptCommand("pokemon", "read", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str, {"[empty],0-5"})}.ToList(), "Displays the reader's dialogue."))
            r(New ScriptCommand("pokemon", "heal", {New ScriptArgument("pokemonIndicies", ScriptArgument.ArgumentTypes.IntArr)}.ToList(), "Heals the given Pokémon."))
            r(New ScriptCommand("pokemon", "setfriendship", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str),
                                                         New ScriptArgument("friendship", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the friendship value for a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "addfriendship", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Str),
                                                         New ScriptArgument("friendship", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds to the frienship value of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "select", {New ScriptArgument("canExit", ScriptArgument.ArgumentTypes.Bool),
                                                         New ScriptArgument("canChooseEgg", ScriptArgument.ArgumentTypes.Bool),
                                                         New ScriptArgument("canChooseFainted", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Opens the Pokémon select screen."))
            r(New ScriptCommand("pokemon", "selectmove", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                         New ScriptArgument("canChooseHMMove", ScriptArgument.ArgumentTypes.Bool),
                                                         New ScriptArgument("canExit", ScriptArgument.ArgumentTypes.Bool)}.ToList(), "Opens the Move Selection screen."))
            r(New ScriptCommand("pokemon", "calcStats", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Recalculates the stats for the given Pokémon."))
            r(New ScriptCommand("pokemon", "learnAttack", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("attackID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Adds the move to the Pokémon's learnset."))
            r(New ScriptCommand("pokemon", "setgender", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("genderID", ScriptArgument.ArgumentTypes.Int, {"0-2"})}.ToList(), "Sets a Pokémon's gender."))
            r(New ScriptCommand("pokemon", "setability", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("abilityID", ScriptArgument.ArgumentTypes.Int, {"0-188"})}.ToList(), "Sets the Ability of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setev", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("evStat", ScriptArgument.ArgumentTypes.Str, {"hp", "atk", "attack", "def", "defense", "spatk", "specialattack", "spattack", "spdef", "specialdefense", "spdefense", "speed"}),
                                                       New ScriptArgument("evValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the value of the Effort Value stat of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setiv", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("ivStat", ScriptArgument.ArgumentTypes.Str, {"hp", "atk", "attack", "def", "defense", "spatk", "specialattack", "spattack", "spdef", "specialdefense", "spdefense", "speed"}),
                                                       New ScriptArgument("ivValue", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the value of the Individual Value stat of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "registerhalloffame", "Registers the current party as new Hall of Fame entry."))
            r(New ScriptCommand("pokemon", "setOT", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("newOT", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Sets the Original Trainer of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setItem", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                       New ScriptArgument("itemID", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Sets the item of a Pokémon in the player's party."))
            r(New ScriptCommand("pokemon", "setItemData", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
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
            r(New ScriptCommand("pokemon", "newroaming", {New ScriptArgument("pokemonID", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("level", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("regionID", ScriptArgument.ArgumentTypes.Int),
                                                      New ScriptArgument("startMap", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Adds a new Roaming Pokémon to the list of Roaming Pokémon.", "|", False))
            r(New ScriptCommand("pokemon", "evolve", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                  New ScriptArgument("evolutionTrigger", ScriptArgument.ArgumentTypes.Str, {"level", "none", "item", "trade"}, True, "level"),
                                                  New ScriptArgument("evolutionArgument", ScriptArgument.ArgumentTypes.Str, True, "")}.ToList(), "Tries to evolve a Pokémon with the given conditions."))
            r(New ScriptCommand("pokemon", "reload", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Reloads the data for a Pokémon in the player's party to apply changes."))
            r(New ScriptCommand("pokemon", "clone", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Clones the given Pokémon in the player's party."))


            ' Constructs:
            r(New ScriptCommand("pokemon", "id", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the ID of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "number", "int", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the ID of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "data", "pokemonData", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the save data for a Pokémon in the player's party.", ",", True))
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
            r(New ScriptCommand("pokemon", "isShiny", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns if the Pokémon is Shiny.", ",", True))
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
            r(New ScriptCommand("pokemon", "OTmatch", "bool,int,str", {New ScriptArgument("checkOT", ScriptArgument.ArgumentTypes.Str),
                                                           New ScriptArgument("returnType", ScriptArgument.ArgumentTypes.Str, {"has", "id", "number", "name", "maxhits"})}.ToList(), "Returns if the player owns a Pokémon with the given Original Trainer.", ",", True))
            r(New ScriptCommand("pokemon", "randomOT", "str", "Returns a random OT (5 digit number).", ",", True))
            r(New ScriptCommand("pokemon", "status", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the status condition of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "canevolve", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                  New ScriptArgument("evolutionTrigger", ScriptArgument.ArgumentTypes.Str, {"level", "none", "item", "trade"}, True, "level"),
                                                  New ScriptArgument("evolutionArgument", ScriptArgument.ArgumentTypes.Str, True, "")}.ToList(), "Returns if the Pokémon can be evolved via the given evolution method.", ",", True))
            r(New ScriptCommand("pokemon", "type1", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the first type of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "type2", "str", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int)}.ToList(), "Returns the second type of a Pokémon in the player's party.", ",", True))
            r(New ScriptCommand("pokemon", "istype", "bool", {New ScriptArgument("pokemonIndex", ScriptArgument.ArgumentTypes.Int),
                                                          New ScriptArgument("type", ScriptArgument.ArgumentTypes.Str)}.ToList(), "Checks if a Pokémon in the player's party has a specific type.", ",", True))
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
                    If IsNumeric(subClass.GetSplit(1)) = True Then
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
                    If IsNumeric(mainClass.GetSplit(1)) = True Then
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