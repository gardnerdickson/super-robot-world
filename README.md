# Super Robot World

2D platformer released on Xbox Live Indie Games on May 6th, 2013.

## Project Setup (Visual Studio)

Resolve TiledLib reference:
- Under the Windows project
	- Delete existing TiledLib reference in References folder.
	- Right click References directory -> Add Reference...
	- Add ``TiledLib.dll`` from ``dependencies\TiledLib dll with Layer.LayerDepth\Windows`` folder.
- Repeat above for Xbox project, but with ``TiledLib.dll`` from ``Xbox 360`` folder.

Resolve TiledPipelineExtensions reference:
- Under RobotGameContent project
	- Delete existing TiledLib reference in References folder.
	- Right click References directory -> Add Reference...
	- Add ``TiledPipelineExtensions.dll`` from ``dependencies\TiledLib dll with Layer.LayerDepth\ContentPipeline`` folder.

Add ``Preferences.xml`` file from ``dependencies`` folder to ``RobotGame\RobotGame\bin\x86\Release`` and ``RobotGame\RobotGame\bin\x86\Debug`` folder.

## Links

[Gameplay Trailer](https://youtu.be/3lDOSaMp0Z8)

## PC Releases

### Release 1.2

[Super Robot World v1.2](https://github.com/gardnerdickson/super-robot-world/releases/download/v1.2/SuperRobotWorld_v1.2.zip)

#### Installation

- Exract ``SuperRobotWorld_v1.2.zip``.
- Run ``xnafx40_redist.msi`` to install XNA 4 runtime if it is not already installed.
- Run ``SuperRobotWorld/RobotGame.exe`` to play.
