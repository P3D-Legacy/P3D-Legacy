The game is not in active development by [nilllzz](https://github.com/nilllzz) anymore, but few people from the community are maintaining the game and porting it to MonoGame platform. 

You are free to fork and redistribute the code under the [GNU GPLv3 license](http://choosealicense.com/licenses/gpl-3.0/).

# Build instructions (game)

The game is written in Visual Basic and was compiled targeting the .NET Framework 4.6

It is built using the MonoGame framework as graphics middleware.

In order to build the game's solution, you need the following:
* Microsoft Windows operating system (XP and up)
* [Microsoft Visual Studio](https://www.visualstudio.com/) (2010 and up)
* [MonoGame 3.7](http://www.monogame.net/downloads/)

To run the game after a successful build, you also need an applicable graphics card that supports DirectX (version 9 minimum).

The first build of the game will take a little longer due to the MonoGame Content Pipeline building all assets for the first time.

# Running the game

In order to run the game, you will need the following:
* [OpenAL](https://www.openal.org/downloads/oalinst.zip)

# Classified information

The game was configured to connect to several servers and internet APIs using private keys.
To keep the private keys private, they have been redacted from the source code, along with a few URLs.

If you want to you can add your own private keys/URLs back into the game to enable certain online features. To find these places search for these comments in the source code:

    ' CLASSIFIED

Every line that has the "CLASSIFIED" comment at the end of it had some kind of string removed from it.

# File Validation

To ensure that the game has the original files, for fair online gaming, the game validates the files. It basically stores a hash for each map, script and data-file in a file called "meta". This is stored in the game's root directory.

Hardcoded into the game's code is the hash for the *meta file*, to ensure that it did not get altered.

The code responsible for this is located in *Security/FileValidation.vb*.

To generate a valid meta file for the current state of the files in the game, go to the aforementioned code file and set this:

        Const RUNVALIDATION As Boolean = True ' Instead of False

Also, be sure to turn off the *IS_DEBUG_ACTIVE* in the *Core/GameController.vb* file.

Then build and debug-run the game. The console output of during the game's launch will output an expected size and metahash value.

It will also produce an updated "meta" file.

Stop the debugging of the game and copy these two values into the correct places at the top of the FileValidation code:

        Const EXPECTEDSIZE As Integer = <your expected size output here>
        Const METAHASH As String = <your meta hash here>

Once you have done this, disable the *RUNVALIDATION* variable again and build the game again to have it boot up like normal.

# Development

The game includes a switch to enable debug mode that makes map development or general fooling around easier.

Locate the file *Core/GameController.vb* and set the *IS_DEBUG_ACTIVE* const to *True*, then rebuild the game.

This is basically the Sandbox Mode that can be enabled in the game's save files plus these features:

* Display last build time in the F3 menu.
* Trade with yourself in the Game Jolt GTS.
* Bypass current version checks in the Game Jolt login screen.
* Update the game's current version on Game Jolt. Set *UPDATEONLINEVERSION* to *True* in *Core/GameController.vb* and rebuild the game.
* Move the 3rd person camera around freely with arrow keys. Hold CTRL and up/down to move the camera on the Y axis.
* Open the Pokégear even though the player has not received it yet and use its modules without receiving them.
* Legacy diagonal movement support.
* Reduce Kolben Splash Screen delay as much as possible.
* All HM moves are present in every Pokémon's menu.
* Ignore FileValidation warning.
