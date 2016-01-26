using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TiledLib;
using RobotGame.Game.Volume;
using RobotGame.Game.Audio;

namespace RobotGame.Game
{
    public enum TriggerKey
    {
        CoreCloseArena,
        CoreOpenArena,
        CoreDestroyedAddCameraBounds,
        CoreDestroyedDeactivateSpawners,
        CoreLevelMusicFadeOut,
        ActivateEndGameExplosion,
    }

    class Triggers
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int CORE_ARENA_LEFT_COLUMN = 601;
        private const int CORE_ARENA_RIGHT_COLUMN = 647;
        private const int CORE_ARENA_TOP_ROW = 0;
        private const int CORE_ARENA_BOTTOM_ROW = 35;

        private const int CORE_DESTROYED_CAMERA_BOUNDS_X = 534;
        private const int CORE_DESTROYED_CAMERA_BOUNDS_Y = 2;
        private const int CORE_DESTROYED_CAMERA_BOUNDS_WIDTH = 113;
        private const int CORE_DESTROYED_CAMERA_BOUNDS_HEIGHT = 50;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static GraphicsDevice graphics;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private Triggers()
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static void Init(GraphicsDevice graphicsDevice)
        {
            graphics = graphicsDevice;
        }

        public static void TriggerEvent(TriggerKey key)
        {
            if (key == TriggerKey.CoreCloseArena)
            {
                core_close_arena();
            }
            else if (key == TriggerKey.CoreOpenArena)
            {
                core_open_arena();
            }
            else if (key == TriggerKey.CoreDestroyedAddCameraBounds)
            {
                core_destroyed_add_camera_bounds();
            }
            else if (key == TriggerKey.CoreDestroyedDeactivateSpawners)
            {
                core_destroyed_deactivate_spawners();
            }
            else if (key == TriggerKey.CoreLevelMusicFadeOut)
            {
                core_level_music_fade_out();
            }
            else if (key == TriggerKey.ActivateEndGameExplosion)
            {
                activate_end_game_explosion();
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private static void core_close_arena()
        {
            Texture2D texture = new Texture2D(graphics, 32, 32);

            for (int y = CORE_ARENA_TOP_ROW; y <= CORE_ARENA_BOTTOM_ROW; y++)
            {
                Level.GetInstance().AddTile(Level.COLLISION_LAYER, CORE_ARENA_LEFT_COLUMN, y, texture);
                Level.GetInstance().AddTile(Level.COLLISION_LAYER, CORE_ARENA_RIGHT_COLUMN, y, texture);

                Player player = (Player)Player.PlayerList[0];
                Rectangle tileBounds = Level.GetInstance().GetTileBounds(CORE_ARENA_RIGHT_COLUMN, y);
                if (CollisionUtil.CheckIntersectionCollision(player.Bounds, tileBounds) != Rectangle.Empty)
                {
                    player.Position = new Vector2(tileBounds.Left - player.Bounds.Width / 2, player.Position.Y);
                }
            }

            SoundManager.MusicVolume = 0.75f;
            SoundManager.StartLevelMusic(new LevelSongInfo(SongKey.CoreBossMusicIntro, SongKey.CoreBossMusicLoop));
        }

        private static void core_open_arena()
        {
            for (int y = CORE_ARENA_TOP_ROW; y <= CORE_ARENA_BOTTOM_ROW; y++)
            {
                Level.GetInstance().RemoveTile(Level.COLLISION_LAYER, CORE_ARENA_LEFT_COLUMN, y);
                Level.GetInstance().RemoveTile(Level.COLLISION_LAYER, CORE_ARENA_RIGHT_COLUMN, y);
            }
        }

        private static void core_destroyed_add_camera_bounds()
        {
            PropertyCollection properties = new PropertyCollection();
            properties.Add(new Property("Transition", "true"));
            properties.Add(new Property("OffsetX", "0"));
            properties.Add(new Property("OffsetY", "-150"));
            properties.Add(new Property("OffsetPriority", "1"));

            int tileWidth = Level.GetInstance().TileWidth;
            int tileHeight = Level.GetInstance().TileHeight;
            Rectangle bounds = new Rectangle(CORE_DESTROYED_CAMERA_BOUNDS_X * tileWidth, CORE_DESTROYED_CAMERA_BOUNDS_Y * tileHeight,
                                             CORE_DESTROYED_CAMERA_BOUNDS_WIDTH * tileWidth, CORE_DESTROYED_CAMERA_BOUNDS_HEIGHT * tileHeight);

            Level.GetInstance().AddCameraBounds("core_destroyed_camera_bounds", "", bounds, properties);
        }

        private static void core_destroyed_deactivate_spawners()
        {
            foreach (AbstractVolume volume in AbstractVolume.VolumeList)
            {
                TimedEnemySpawnVolume spawnVolume = volume as TimedEnemySpawnVolume;
                if (spawnVolume != null)
                {
                    spawnVolume.Enabled = false;
                }
            }
        }

        private static void core_level_music_fade_out()
        {
            SoundManager.MusicFadeOut(4000f);
        }

        private static void activate_end_game_explosion()
        {
            foreach (AbstractVolume volume in AbstractVolume.VolumeList)
            {
                EndGameExplosionVolume endGameExplosionVolume = volume as EndGameExplosionVolume;
                if (endGameExplosionVolume != null)
                {
                    endGameExplosionVolume.Activate();
                }
            }
        }
    }
}