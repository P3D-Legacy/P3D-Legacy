# P3D-Legacy

<p align="center">
<a href="https://github.com/P3D-Legacy/P3D-Legacy/blob/master/LICENSE"><img src="https://img.shields.io/github/license/P3D-Legacy/P3D-Legacy" alt="License"></a>
<a href="https://github.com/P3D-Legacy/P3D-Legacy/releases"><img src="https://img.shields.io/github/downloads/P3D-Legacy/P3D-Legacy/total" alt="Total Downloads"></a>
<a href="https://github.com/P3D-Legacy/P3D-Legacy/graphs/contributors"><img src="https://img.shields.io/github/contributors/P3D-Legacy/P3D-Legacy" alt="Contributors"></a>
<a href="https://github.com/P3D-Legacy/P3D-Legacy/releases"><img src="https://img.shields.io/github/v/release/P3D-Legacy/P3D-Legacy" alt="Version"></a>
<a href="https://github.com/P3D-Legacy/P3D-Legacy/releases"><img src="https://img.shields.io/github/release-date/P3D-Legacy/P3D-Legacy" alt="Release"></a>
<a href="https://discordapp.com/invite/EUhwdrq" target="_blank"><img src="https://img.shields.io/discord/299181628188524544" alt="Discord"></a>
</p>

The game is not in active development by the original developer [nilllzz](https://github.com/nilllzz) anymore, but a few people from the community are still developing and maintaining the game, hence the name P3D-Legacy.

You are free to fork and redistribute the code under the [GNU GPLv3 license](http://choosealicense.com/licenses/gpl-3.0/).

**Looking for documentation for the game?** Check out the [Wiki](https://wiki.pokemon3d.net/) we have for the game!

# Downloading the game

You will find the latest release **[here](https://github.com/P3D-Legacy/P3D-Legacy/releases)**.

# Instructions to play the game
* Go to the releases page (**[here](https://github.com/P3D-Legacy/P3D-Legacy/releases)**) and download the game (**you need the one that ends with "Release.zip"**) to a location where you can find it.
* After it has been downloaded, use an application like WinRAR or 7zip to extract the contents of the zip archive to a location of your preference.
* Navigate to the folder where you extracted the zip and double-click Pokemon3D.exe.
* Have fun playing Pokémon 3D!

If you want to play online on the official Pokémon 3D server, you need a GameJolt account and your game token (**[Click here if you don't know where to find your game token](https://gamejolt.com/f/how-to-find-your-user-token/291)**).
* Start the game, click on the GameJolt button in the Main Menu and enter your GameJolt username and your game token in the corresponding boxes.
* Click on the button that says "Log In" and when you're back in the Main Menu, select the new Savegame that appeared.
* If you want to play as a different character, check out the Skin Changer page on the Pokémon 3D homepage **[here](https://skin.pokemon3d.net/login)**.
* Log in with your GameJolt account

# Instructions to build the game from source

The game is written in Visual Basic/VB.NET and was compiled targeting the .NET Framework 4.6

It is built using the MonoGame framework as graphics middleware.

In order to build the game's solution, you need the following:
* Microsoft Windows operating system (XP and up)
* [Microsoft Visual Studio Community Edition](https://www.visualstudio.com/) (2019 and up)
* [MonoGame 3.7.1 for Visual Studio](https://community.monogame.net/t/monogame-3-7-1/11173)
* [Visual C++ Redistributable Packages for Visual Studio 2012 Update 4](http://www.microsoft.com/en-NZ/download/details.aspx?id=30679)
* [Visual C++ Redistributable Packages for Visual Studio 2013](https://www.microsoft.com/en-us/download/details.aspx?id=40784)

Make sure you've cloned the repository to a filepath without any spaces in it. It won't build otherwise.
To run the game after a successful build, you also need an applicable graphics card that supports DirectX (version 9 minimum).

The first build of the game will take a little longer due to the MonoGame Content Pipeline building all assets for the first time.

# Classified information

The game was configured to connect to several servers and internet APIs using private keys.
To keep the private keys private, they have been redacted from the source code, along with a few URLs.

If you want to you can add your own private keys/URLs back into the game to enable certain online features. To find these places search for these comments in the source code: `' CLASSIFIED`

Every line that has the "CLASSIFIED" comment at the end of it had some kind of string removed from it.

# File Validation

To ensure that the game has the original files, for fair online gaming, the game validates the files. It basically stores a hash for each map, script and data-file in a file called "meta". This is stored in the game's root directory.

Hardcoded into the game's code is the hash for the *meta file*, to ensure that it did not get altered.

The code responsible for this is located in *Security/FileValidation.vb*.

To generate a valid meta file for the current state of the files in the game, go to the aforementioned code file and set this:

        Const RUNVALIDATION As Boolean = True ' Instead of False

Also, be sure to set the Solution Configuration at the top of the screen to Release instead of Debug.

Then build and debug-run the game. The console output during the game's launch will output an expected size and metahash value.
It will also produce an updated "meta" file in the "P3D\bin\Release\" folder. Copy this to the main project folder (that's called P3D) and replace the file that's already there.

Stop the debugging of the game and copy these two values into the correct places at the top of the FileValidation code:

        Const EXPECTEDSIZE As Integer = <your expected size output here>
        Const METAHASH As String = <your meta hash here>

Once you have done this, disable the *RUNVALIDATION* variable again and build the game again to have it boot up like normal.

If you want to continue working on the code, it is recommended to set the Solution Configuration at the top of the screen back to Debug instead of Release.

# Development

The game has a Solution Configuration that enables debug mode which makes map development or general fooling around easier.

Set the Solution Configuration at the top of the screen to Debug (not Release), then rebuild the game.

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
