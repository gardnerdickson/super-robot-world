SUPER ROBOT WORLD
=================

2D platformer released on Xbox Live Indie Games on May 5th, 2013.

**Project setup**

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
