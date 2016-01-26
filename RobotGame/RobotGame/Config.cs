using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using RobotGame.Game.Audio;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game;

namespace RobotGame
{
    class Config
    {
        public const int DEFAULT_BACK_BUFFER_WIDTH = 1280;
        public const int DEFAULT_BACK_BUFFER_HEIGHT = 720;

        public const int RENDER_TARGET_WIDTH = 1280;
        public const int RENDER_TARGET_HEIGHT = 720;

        public const int TITLE_SAFE_OFFSET_X = (int)(RENDER_TARGET_WIDTH * 0.1f / 2); // 64 pixels
        public const int TITLE_SAFE_OFFSET_Y = (int)(RENDER_TARGET_HEIGHT * 0.1f / 2); // 36 pixels


        // Error log
        public const string ERROR_LOG = "error.log";

        public const double LEVEL_START_TIME = 3000d;
        public const double LEVEL_END_TIME = 3000d;

        public static Color LEVEL_3_SKY_COLOR = new Color(75, 5, 5, 75);

        public const int NUMBER_OF_WORLDS = 3;
        public static LevelInfo[] LEVEL_ASSETS = new LevelInfo[]
        {
            //new LevelInfo(@"Maps\dev\background_test", new SongInfo(SongKey.Level_1_3_Intro, SongKey.Level_1_3_Loop), SpriteKey.Level1TileExplosion, new BackgroundFactory[]
            //    {
            //        new SkyBackgroundFactory(@"Backgrounds\level_1_3_sky", Color.White)
            //    }
            //),
            
            new LevelInfo(@"Maps\level_1_1", new LevelSongInfo(SongKey.Level_1_1_Intro, SongKey.Level_1_1_Loop), SpriteKey.Level1TileExplosion, new BackgroundFactory[]
                {
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_1_background_1", 0.30f, 4, 1, 1f, 0, 1000, new Color(200, 200, 200)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -0.25f, 0.40f, 3, 1f, 0, 500, new Color(255, 255, 255, 200)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_1_background_1", 0.50f, 4, 1, 0.70f, -500, 550, new Color(225, 225, 225)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -0.10f, 0.60f, 3, 0.75f, 0, 200, new Color(255, 255, 255, 200)),
                    new SkyBackgroundFactory(@"Backgrounds\level_1_1_sky", Color.White)
                }
            ),

            new LevelInfo(@"Maps\level_1_2", new LevelSongInfo(SongKey.Level_1_2_Intro, SongKey.Level_1_2_Loop), SpriteKey.Level1TileExplosion, new BackgroundFactory[]
                {
                    //new SimpleBackgroundFactory(@"Backgrounds\level_1_2_background_1", 0.20f, 8, 1, 0.20f, 0, 1750, new Color(130, 130, 130)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_2_background_1", 0.30f, 14, 1, 0.18f, -400, 1450, new Color(155, 155, 155)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -0.40f, 0.40f, 4, 0.80f, 0, 1000, new Color(255, 255, 255, 170)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_2_background_1", 0.40f, 13, 1, 0.16f, 0, 1050, new Color(180, 180, 180)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -0.30f, 0.50f, 4, 0.60f, -500, 800, new Color(255, 255, 255, 170)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_2_background_1", 0.50f, 13, 1, 0.14f, 0, 800, new Color(205, 205, 205)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -0.20f, 0.60f, 5, 0.40f, 0, 600, new Color(255, 255, 255, 170)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_2_background_1", 0.60f, 13, 1, 0.12f, -300, 550, new Color(230, 230, 230)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -0.10f, 0.70f, 7, 0.20f, -500, 400, new Color(255, 255, 255, 170)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_2_background_1", 0.70f, 12, 1, 0.10f, 0, 400, new Color(255, 255, 255)),
                    new SkyBackgroundFactory(@"Backgrounds\level_1_2_sky", Color.White),
                }
            ),

            new LevelInfo(@"Maps\level_1_3", new LevelSongInfo(SongKey.Level_1_3_Intro, SongKey.Level_1_3_Loop), SpriteKey.Level1TileExplosion, new BackgroundFactory[]
                {
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -1, 0f, 3, 2f, 0, 0, new Color(255, 255, 255, 100), MAP_CLOSE_BACKGOUND_DRAW_DEPTH),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -1, 0f, 3, 2f, -500, 1200, new Color(255, 255, 255, 100), MAP_CLOSE_BACKGOUND_DRAW_DEPTH),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_3_background_1", 0.20f, 5, 1, 0.40f, 0, 4870, new Color(180, 180, 180)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1",-0.30f, 0.20f, 3, 0.75f, 0, 4590, new Color(255, 255, 255, 200)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_3_background_1", 0.30f, 5, 1, 0.35f, 0, 3890, new Color(205, 205, 205)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -0.20f, 0.30f, 3, 0.60f, 0, 3590, new Color(255, 255, 255, 200)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_3_background_1", 0.40f, 7, 1, 0.30f, 0, 2990, new Color(230, 230, 230)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -0.10f, 0.40f, 4, 0.45f, 0, 2790, new Color(255, 255, 255, 200)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_1_3_background_1", 0.50f, 7, 1, 0.25f, 0, 2290, new Color(255, 255, 255)),
                    new SkyBackgroundFactory(@"Backgrounds\level_1_3_sky", Color.White),
                }
            ),
         
            new LevelInfo(@"Maps\level_2_1", new LevelSongInfo(SongKey.Level_2_1_Intro, SongKey.Level_2_1_Loop), SpriteKey.Level2TileExplosion, new BackgroundFactory[]
                {
                    new SimpleBackgroundFactory(@"Backgrounds\level_2_background_2", 0.20f, 5, 3, 1, 0, 0, Color.White),
                    new SimpleBackgroundFactory(@"Backgrounds\level_2_background_1", 0.40f, 5, 3, 1, 0, 0, new Color(190, 190, 190)),
                    new SkyBackgroundFactory(@"Backgrounds\level_2_sky_2", Color.White),
                }
            ),

            new LevelInfo(@"Maps\level_2_2", new LevelSongInfo(SongKey.Level_2_2_Intro, SongKey.Level_2_2_Loop), SpriteKey.Level2TileExplosion, new BackgroundFactory[]
                {
                    new SimpleBackgroundFactory(@"Backgrounds\level_2_background_3", 0.20f, 10, 3, 0.50f, 0, 0, Color.White),
                    new SimpleBackgroundFactory(@"Backgrounds\level_2_background_1", 0.40f, 4, 2, 1, 0, 0, new Color(190, 190, 190)),
                    new SkyBackgroundFactory(@"Backgrounds\level_2_sky_2", Color.White), 
                }
            ),

            new LevelInfo(@"Maps\level_2_3", new LevelSongInfo(SongKey.Level_2_3_Intro, SongKey.Level_2_3_Loop), SpriteKey.Level2TileExplosion, new BackgroundFactory[]
                {
                    new SimpleBackgroundFactory(@"Backgrounds\level_2_background_3", 0.40f, 13, 7, 0.30f, 0, 0, new Color(190, 190, 190)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_2_background_3", 0.60f, 14, 7, 0.20f, 0, 0, new Color(145, 145, 145)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_2_background_1", 0.80f, 4, 3, 0.50f, 0, 0, new Color(95, 95, 95)),                    
                    new SkyBackgroundFactory(@"Backgrounds\level_2_sky_2", Color.White),
                }
            ),
            
            new LevelInfo(@"Maps\level_3_1", new LevelSongInfo(SongKey.Level_3_1_Intro, SongKey.Level_3_1_Loop), SpriteKey.Level3TileExplosion, new BackgroundFactory[]
                {                    
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_3_animated_background_2", -16f, 0.20f, 4, 1.5f, 0, 2800, new Color(255, 255, 255, 175)),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_3_animated_background_1", -14f, 0.35f, 4, 1.5f, 0, 2000, new Color(255, 255, 255, 175)),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_3_animated_background_2", -12f, 0.50f, 4, 1.5f, 0, 1300, new Color(255, 255, 255, 175)),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_3_animated_background_1", -10f, 0.65f, 4, 1.5f, 0, 600, new Color(255, 255, 255, 175)),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_3_animated_background_2", -8, 0.80f, 4, 1.5f, 0, 0, new Color(255, 255, 255, 175)),
                }
            ),

            new LevelInfo(@"Maps\level_3_2", new LevelSongInfo(SongKey.Level_3_2_Intro, SongKey.Level_3_2_Loop), SpriteKey.Level3TileExplosion, new BackgroundFactory[]
                {
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_3_animated_background_1", -14f, 0.20f, 6, 1.5f, 0, 1800, new Color(255, 255, 255, 175)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_20", 0.40f, 6, 1, 1f, -1000, 1100, new Color(255, 255, 255)),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_3_animated_background_2", -10f, 0.40f, 5, 1.5f, 0, 800, new Color(255, 255, 255, 175)),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_20", 0.60f, 6, 1, 0.75f, 0, 600, new Color(255, 255, 255)),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_3_animated_background_1", -6f, 0.60f, 4, 1.5f, 0, 200, new Color(255, 255, 255, 175)),
                }
            ),

            new LevelInfo(@"Maps\level_3_3", new LevelSongInfo(SongKey.Level_3_3_Intro, SongKey.Level_3_3_Loop), SpriteKey.Level3TileExplosion, new BackgroundFactory[]
                {
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_20", 0.15f, 8, 1, 1.0f, 0, 6200, Color.White),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_20", 0.15f, 8, 1, 1.0f, -1000, 6200, Color.White),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_10", 0.30f, 8, 1, 0.9f, -450, 5000, Color.White),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_10", 0.30f, 8, 1, 0.9f, 450, 5000, Color.White),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_10", 0.45f, 8, 1, 0.8f, 0, 3800, Color.White),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_10", 0.45f, 8, 1, 0.8f, -800, 3800, Color.White),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_10", 0.60f, 8, 1, 0.7f, -350, 2700, Color.White),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_10", 0.60f, 8, 1, 0.7f, 350, 2700, Color.White),
                    new SkyBackgroundFactory(@"Backgrounds\level_3_sky_white", LEVEL_3_SKY_COLOR),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_10", 0.75f, 8, 1, 0.6f, 0, 1600, Color.White),
                    new SimpleBackgroundFactory(@"Backgrounds\level_3_castle_background_10", 0.75f, 8, 1, 0.6f, -600, 1600,  Color.White),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", -5f, 0.30f, 8, 1f, 0, 100, new Color(255, 255, 255, 200)),
                    new AnimatedBackgroundFactory(@"Backgrounds\level_1_1_animated_background_1", 5f, 0.30f, 8, 1f, 0, 50, new Color(255, 255, 255, 200)),
                }
            ),

        };


        public const int PRESS_START_MENU_CONTINUE = 0;

        public const int MAIN_MENU_START = 0;
        public const int MAIN_MENU_1985 = 1;
        public const int MAIN_MENU_HOW_TO_PLAY = 2;
        public const int MAIN_MENU_EXIT = 3;
        public const int MAIN_MENU_PURCHASE_GAME = 4;
        public const string MAIN_MENU_MESSAGE = "SUPER ROBOT WORLD";
        public static string[] MAIN_MENU_OPTIONS = new string[]
        {
            "Start",
            "1985 Mode",
            "How to Play",
            "Exit Game"
        };

        public const int PAUSE_MENU_CONTINUE = 0;
        public const int PAUSE_MENU_EXIT = 1;
        public const string PAUSE_MENU_MESSAGE = "PAUSE";
        public static string[] PAUSE_MENU_OPTIONS = new string[]
        {
            "Continue",
            "Exit to Main Menu"
        };

        public const int GAME_END_MENU_RESTART = 0;
        public const int GAME_END_MENU_EXIT = 1;
        public const string GAME_END_MENU_MESSAGE = "GAME COMPLETE!!";
        public static string[] GAME_END_MENU_OPTIONS = new string[]
        {
            "Main Menu",
            "Exit Game"
        };

        public const int GAME_OVER_MENU_CONTINUE = 0;
        public const int GAME_OVER_MENU_RESTART = 1;
        public const int GAME_OVER_MENU_EXIT = 2;
        public const string GAME_OVER_MENU_MESSAGE = "GAME OVER";
        public static string[] GAME_OVER_MENU_OPTIONS = new string[]
        {
            "Continue",
            "Main Menu",
            "Exit Game"
        };

        public const int CONFIRMATION_MENU_YES = 0;
        public const int CONFIRMATION_MENU_NO = 1;
        public const string CONFIRMATION_MENU_MESSAGE = "ARE YOU SURE?";
        public static string[] CONFIRMATION_MENU_OPTIONS = new string[]
        {
            "Yes",
            "No"
        };

        public static string[] CONTROLLER_CONTROLS = new string[]
        {
            "Left Thumbstick",
            "  Move Player Left or Right",

            "Right Thumbstick",
            "  Aim Nozzle",

            "Left Trigger",
            "  Jump",
            "  Use Jetpack",
            
            "Right Bumper",
            "  Fire Primary Weapon",

            "Right Trigger",
            "  Fire Secondary Weapon",

            "A",
            "  Change Secondary Weapon",
        };

        public static string[] KEYBOARD_CONTROLS = new string[]
        {
            "A",
            "  Move Left",

            "D",
            "  Move Right",

            "Mouse",
            "  Aim Nozzle",

            "Space",
            "  Jump",
            "  Use Jetapck",

            "Left Click",
            "  Fire Primary Weapon",

            "Right Click",
            "  Fire Secondary Weapon",

            "Tab",
            "  Change Secondary Weapon",
        };
        
        public const int MENU_LEFT_ALIGNMENT_OFFSET_X = 50;

        // Sprite draw depth
        public const float TITLE_SAFE_OVERLAY_DRAW_DEPTH = 0.05f;
        public const float CONTROLLER_INDEX_DRAW_DEPTH = 0.06f;
        public const float MENU_TEXT_DRAW_DEPTH = 0.11f; // front
        public const float MENU_DRAW_DEPTH = 0.12f;
        public const float HUD_METER_DRAW_DEPTH = 0.13f;
        public const float HUD_WEAPON_SELECTION_OVERLAY_DEPTH = 0.14f;
        public const float HUD_ICON_DRAW_DEPTH = 0.15f;
        public const float HUD_OVERLAY_DRAW_DEPTH = 0.16f;
        public const float HUD_CURSOR_DRAW_DEPTH = 0.17f;
        public const float MAP_CLOSE_BACKGOUND_DRAW_DEPTH = 0.18f;
        public const float ENEMY_EXPLOSION_DRAW_DEPTH = 0.19f;
        public const float PARTICLE_DRAW_DEPTH = 0.21f;
        public const float AERIAL_PAWN_DRAW_DEPTH = 0.21f;
        public const float ENVIRONEMNT_ACTOR_DRAW_DEPTH = 0.22f;
        public const float MAP_FOREGROUND_DRAW_DEPTH = 0.23f;
        public const float MOVER_DRAW_DEPTH = 0.24f;
        public const float LASER_DRAW_DEPTH = 0.25f;
        public const float POWERUP_DRAW_DEPTH = 0.26f;
        public const float PROJECTILE_DRAW_DEPTH = 0.27f;
        public const float NOZZLE_DRAW_DEPTH = 0.28f;
        public const float PLAYER_DRAW_DEPTH = 0.29f;
        public const float JETPACK_FLAME_DRAW_DEPTH = 0.30f;
        public const float ENEMY_DRAW_DEPTH = 0.31f;
        public const float MAP_BACKGROUND_DRAW_DEPTH = 0.32f;
        public const float LEVEL_BACKGROUND_DRAW_DEPTH = 0.33f; // back

        // Fonts
        public const string DEBUG_FONT_ASSET = @"Fonts\DebugInfo";
        public const string MENU_FONT_ASSET = @"Fonts\Menu";
        public const string MENU_SMALL_FONT_ASSET = @"Fonts\MenuSmall";
        public const string HUD_FONT_ASSET = @"Fonts\HUDFont";
        public const string PROFILER_FONT_ASSET = @"Fonts\ProfilerFont";

        // Temporary dev assets
        public const string TITLE_SAFE_OVERLAY = @"SpriteSheets\title_safe_rect";
        public const string DOUBLE_DAMAGE_POWERUP_ASSET = @"SpriteSheets\dev\powerup_double_damage";

        // Production assets
        public const string MENU_BLACK_ASSET = @"Screens\menu_background_black";
        public const string SPECIAL_THANKS_BACKGROUND_ASSET = @"Screens\special_thanks_background";

        public const string CROSSHAIR_ASSET = @"SpriteSheets\crosshair";

        public const string PLAYER_ASSET = @"SpriteSheets\player";
        public const string PLAYER_JETPACK_ASSET = @"SpriteSheets\player_jetpack";
        public const string PLAYER_JETPACK_FLAME = @"SpriteSheets\jetpack_flame";
        public const string NOZZLE_ASSET = @"SpriteSheets\nozzle";
        public const string SMOKE_ASSET = @"SpriteSheets\smoke";

        public const string EXPLOSION_ASSET = @"SpriteSheets\explosion";

        public const string EASY_PAWN_ASSET = @"SpriteSheets\easy_pawn";
        public const string GRENADIER_ASSET = @"SpriteSheets\grenadier";
        public const string HARD_GRENADIER_ASSET = @"SpriteSheets\grenadier_hard";
        public const string AERIAL_PAWN_ASSET = @"SpriteSheets\aerial_pawn";
        public const string AERIAL_BOMBER_ASSET = @"SpriteSheets\aerial_bomber";
        public const string HARD_PAWN_ASSET = @"SpriteSheets\hard_pawn";
        public const string WASP_ASSET = @"SpriteSheets\wasp";
        public const string CRAWLER_UP_ASSET = @"SpriteSheets\crawler_up";
        public const string CRAWLER_DOWN_ASSET = @"SpriteSheets\crawler_down";
        public const string CRAWLER_LEFT_ASSET = @"SpriteSheets\crawler_left";
        public const string CRAWLER_RIGHT_ASSET = @"SpriteSheets\crawler_right";
        public const string TURRET_STATIONARY_UP_ASSET = @"SpriteSheets\turret_stationary_up";
        public const string TURRET_STATIONARY_DOWN_ASSET = @"SpriteSheets\turret_stationary_down";
        public const string TURRET_STATIONARY_LEFT_ASSET = @"SpriteSheets\turret_stationary_left";
        public const string TURRET_STATIONARY_RIGHT_ASSET = @"SpriteSheets\turret_stationary_right";
        public const string TRACKING_TURRET_INVINCIBLE_UP_ASSET = @"SpriteSheets\tracking_turret_invincible_up";
        public const string TRACKING_TURRET_INVINCIBLE_DOWN_ASSET = @"SpriteSheets\tracking_turret_invincible_down";
        public const string TRACKING_TURRET_INVINCIBLE_LEFT_ASSET = @"SpriteSheets\tracking_turret_invincible_left";
        public const string TRACKING_TURRET_INVINCIBLE_RIGHT_ASSET = @"SpriteSheets\tracking_turret_invincible_right";
        public const string TURRET_STATIONARY_INVINCIBLE_UP_ASSET = @"SpriteSheets\turret_stationary_invincible_up";
        public const string TURRET_STATIONARY_INVINCIBLE_DOWN_ASSET = @"SpriteSheets\turret_stationary_invincible_down";
        public const string TURRET_STATIONARY_INVINCIBLE_LEFT_ASSET = @"SpriteSheets\turret_stationary_invincible_left";
        public const string TURRET_STATIONARY_INVINCIBLE_RIGHT_ASSET = @"SpriteSheets\turret_stationary_invincible_right";
        public const string TURRET_MOVING_UP_ASSET = @"SpriteSheets\turret_moving_up";
        public const string TURRET_MOVING_DOWN_ASSET = @"SpriteSheets\turret_moving_down";
        public const string TURRET_MOVING_LEFT_ASSET = @"SpriteSheets\turret_moving_left";
        public const string TURRET_MOVING_RIGHT_ASSET = @"SpriteSheets\turret_moving_right";
        public const string TURRET_LASER_NOZZLE_ASSET = @"SpriteSheets\turret_laser_nozzle";
        public const string TURRET_INVINCIBLE_LASER_NOZZLE_ASSET = @"SpriteSheets\turret_invincible_laser_nozzle";
        public const string TURRET_PROJECTILE_NOZZLE_ASSET = @"SpriteSheets\turret_projectile_nozzle";

        public const string CORE_ASSET = @"SpriteSheets\core";
        public const string CORE_EXPLOSION_ASSET = @"SpriteSheets\core_explosion";
        public const string CORE_SHIELD_ASSET = @"SpriteSheets\core_shield";
        public const string CORE_SHIELD_EXPLOSION_ASSET = @"SpriteSheets\core_shield_explosion";

        public const string PLAYER_EXPLOSION_ASSET = @"SpriteSheets\player_explosion";
        public const string EASY_PAWN_EXPLOSION_ASSET = @"SpriteSheets\easy_pawn_explosion";
        public const string GRENADIER_EXPLOSION_ASSET = @"SpriteSheets\grenadier_explosion";
        public const string HARD_GRENADIER_EXPLOSION_ASSET = @"SpriteSheets\grenadier_hard_explosion";
        public const string CRAWLER_EXPLOSION_ASSET = @"SpriteSheets\crawler_explosion";
        public const string AERIAL_PAWN_EXPLOSION_ASSET = @"SpriteSheets\aerial_pawn_explosion";
        public const string AERIAL_BOMBER_EXPLOSION_ASSET = @"SpriteSheets\aerial_bomber_explosion";
        public const string WASP_EXPLOSION_ASSET = @"SpriteSheets\wasp_explosion";
        public const string TURRET_EXPLOSION_ASSET = @"SpriteSheets\turret_stationary_explosion";
        public const string HARD_PAWN_EXPLOSION_ASSET = @"SpriteSheets\hard_pawn_explosion";

        public const string BULLET_PROJECTILE_COLLISION_PARTILE_ASSET = @"SpriteSheets\bullet_collision_particle";
        public const string ENEMY_BULLET_PROJECTILE_COLLISION_PARTICLE_ASSET = @"SpriteSheets\enemy_bullet_collision_particle";
        public const string EASY_PAWN_BULLET_PROJECTILE_ASSET = @"SpriteSheets\pawn_projectile";
        public const string AERIAL_PAWN_ORB_PROJECTILE_ASSET = @"SpriteSheets\orb_projectile";
        public const string AERIAL_PAWN_ORB_TAIL_ASSET = @"SpriteSheets\orb_tail";
        public const string GRENADIER_PROJECTILE_ASSET = @"SpriteSheets\grenadier_projectile";
        public const string CRAWLER_BOMB_PROJECTILE_ASSET = @"SpriteSheets\crawler_bomb_projectile";
        public const string CRAWLER_SPIRAL_PROJECTILE_ASSET = @"SpriteSheets\crawler_spiral_projectile";
        public const string AERIAL_BOMBER_PROJECTILE_ASSET = @"SpriteSheets\aerial_bomber_projectile";
        public const string HARD_PAWN_BULLET_PROJECTILE_ASSET = @"SpriteSheets\pawn_projectile";

        public const string HUD_BOTTOM_ASSET = @"SpriteSheets\hud_bottom";
        public const string HUD_TOP_ASSET = @"SpriteSheets\hud_top";
        public const string HUD_METER_CONTAINER_ASSET = @"SpriteSheets\hud_meter_container";
        public const string HUD_METER_ASSET = @"SpriteSheets\hud_meter";
        public const string HUD_AMMO_ASSET = @"SpriteSheets\hud_ammo";

        public const string TARGET_ARROW_ASSET = @"SpriteSheets\target_arrow";

        public const string HUD_GRENADE_UNAVAILABLE_ASSET = @"SpriteSheets\hud_grenade_unavailable";
        public const string HUD_GRENADE_AVAILABLE_ASSET = @"SpriteSheets\hud_grenade_available";
        public const string HUD_HOMING_MISSILE_UNAVAILABLE_ASSET = @"SpriteSheets\hud_homing_missile_unavailable";
        public const string HUD_HOMING_MISSILE_AVAILABLE_ASSET = @"SpriteSheets\hud_homing_missile_available";
        public const string HUD_WEAPON_SELECTED_OVERLAY_ASSET = @"SpriteSheets\hud_weapon_sl_overlay";
        public const string HUD_WEAPON_SELECTED_DOUBLE_DAMAGE_OVERLAY_ASSET = @"SpriteSheets\hud_weapon_sl_double_dmg_overlay";
        public const string PLAYER_LIVES_ICON_ASSET = @"SpriteSheets\hud_lives";

        public const string BULLET_PROJECTILE_ASSET = @"SpriteSheets\bullet";

        public const string HOMING_MISSILE_PROJECTILE_ASSET = @"SpriteSheets\homing_missile_projectile";
        public const string HOMING_MISSILE_PROJECTILE_SEEK_ASSET = @"SpriteSheets\homing_missile_projectile_seek";
        public const string GRENADE_PROJECTILE_ASSET = @"SpriteSheets\grenade_projectile";

        public const string LASER_LINE_ASSET = @"SpriteSheets\laser_line";
        public const string LASER_GLOW_ASSET = @"SpriteSheets\laser_glow";

        public const string POWERUP_HOMING_MISSILE_ASSET = @"SpriteSheets\powerup_homing_missile";
        public const string POWERUP_GRENADE_ASSET = @"SpriteSheets\powerup_grenade";
        public const string POWERUP_AMMO_ASSET = @"SpriteSheets\powerup_ammo";
        public const string POWERUP_HEALTH_ASSET = @"SpriteSheets\powerup_meter_health";
        public const string POWERUP_LIFE_ASSET = @"SpriteSheets\powerup_1up";
        public const string POWERUP_JETPACK_ASSET = @"SpriteSheets\powerup_jetpack";

        public const string LEVEL_1_MOVER_ASSET = @"SpriteSheets\level_1_mover";
        public const string LEVEL_2_MOVER_ASSET = @"SpriteSheets\level_2_mover";
        public const string LEVEL_3_MOVER_ASSET = @"SpriteSheets\level_3_mover";
        public const string LEVEL_3_FINAL_MOVER_ASSET = @"SpriteSheets\level_3_final_mover";
        public const string LEVEL_3_LONG_MOVER_ASSET = @"SpriteSheets\level_3_mover_long";
        public const string LEVEL_3_TALL_MOVER_ASSET = @"SpriteSheets\level_3_mover_tall";

        public const string LEVEL_1_TILE_EXPLOSION_ASSET = @"SpriteSheets\level_1_tile_explosion";
        public const string LEVEL_2_TILE_EXPLOSION_ASSET = @"SpriteSheets\level_2_tile_explosion";
        public const string LEVEL_3_TILE_EXPLOSION_ASSET = @"SpriteSheets\level_3_tile_explosion";

        public const string LEVEL_END_FLAG_ASSET = @"SpriteSheets\level_end_flag";
        public const string FAN_ASSET = @"SpriteSheets\fan";

        public const string NULL_SPRITESHEET_ASSET = @"SpriteSheets\null_spritesheet";
        

        // Audio assets
        public const string LEVEL_1_1_SONG_INTRO = null;
        public const string LEVEL_1_1_SONG_LOOP = @"Audio\Songs\Wrld1.1 - Daydream Trail";

        public const string LEVEL_1_2_SONG_INTRO = null;
        public const string LEVEL_1_2_SONG_LOOP = @"Audio\Songs\Wrld1.2 - Blue Skies";

        public const string LEVEL_1_3_SONG_INTRO = null;
        public const string LEVEL_1_3_SONG_LOOP = @"Audio\Songs\Wrld1.3 - Clarity";

        public const string LEVEL_2_1_SONG_INTRO = null;
        public const string LEVEL_2_1_SONG_LOOP = @"Audio\Songs\Wrld2.1 - Diamond Mine";

        public const string LEVEL_2_2_SONG_INTRO = null;
        public const string LEVEL_2_2_SONG_LOOP = @"Audio\Songs\Wrld2.2 - Urgent Matters";

        public const string LEVEL_2_3_SONG_INTRO = null;
        public const string LEVEL_2_3_SONG_LOOP = @"Audio\Songs\Wrld2.3 - Daybreak";

        public const string LEVEL_3_1_SONG_INTRO = null;
        public const string LEVEL_3_1_SONG_LOOP = @"Audio\Songs\Wrld3.1 - Cadence";

        public const string LEVEL_3_2_SONG_INTRO = null;
        public const string LEVEL_3_2_SONG_LOOP = @"Audio\Songs\Wrld3.2 - Voices";

        public const string LEVEL_3_3_SONG_INTRO = null;
        public const string LEVEL_3_3_SONG_LOOP = @"Audio\Songs\Wrld3.3 - Orilleugin";

        public const string BOSS_MUSIC_INTRO = null;
        public const string BOSS_MUSIC_LOOP = @"Audio\Songs\Wrld3.3 - Culmination";

        public const string GAME_END_SONG = @"Audio\Songs\Credits - Persistence";

        
        public const string PRIMARY_FIRE_SE = @"Audio\SoundEffects\MST Primary Fire";
        public const string SECONDARY_FIRE_SE = @"Audio\SoundEffects\MST Secondary Fire";
        public const string FIRE_NO_AMMO_SE = @"Audio\SoundEffects\MST OutOAmmo";
        public const string HOMING_MISSILE_SEEK_MODE_SE = @"Audio\SoundEffects\MST HOME MISL";

        public const string ENEMY_PRIMARY_FIRE_SE = @"Audio\SoundEffects\MST Primary Fire alt";

        public const string PROJECTILE_EXPLODE1_SE = @"Audio\SoundEffects\MST Projectile Explode";
        public const string PROJECTILE_EXPLODE2_SE = @"Audio\SoundEffects\MST Projectile Explode alt";

        public const string PLAYER_LAND_SE = @"Audio\SoundEffects\MST PLR LAND";
        public const string JETPACK_SE_LOOP = @"Audio\SoundEffects\MST JETPACK";

        public const string PLAYER_TAKE_DAMAGE1_SE = @"Audio\SoundEffects\MST PLR DMG A";
        public const string PLAYER_TAKE_DAMAGE2_SE = @"Audio\SoundEffects\MST PLR DMG B";
        public const string PLAYER_TAKE_DAMAGE3_SE = @"Audio\SoundEffects\MST PLR DMG C";
        public const string PLAYER_TAKE_DAMAGE4_SE = @"Audio\SoundEffects\MST PLR DMG D";
        public const string PLAYER_TAKE_DAMAGE5_SE = @"Audio\SoundEffects\MST PLR DMG E";
        public const string PLAYER_DEATH_SE = @"Audio\SoundEffects\MST PLR Death";
        public const string PLAYER_DEATH_JINGLE_SE = @"Audio\SoundEffects\MST PLR Death Jingle";
        
        public const string ENEMY_TAKE_DAMAGE1_SE = @"Audio\SoundEffects\MST ENMY DMG A";
        public const string ENEMY_TAKE_DAMAGE2_SE = @"Audio\SoundEffects\MST ENMY DMG B";
        public const string ENEMY_TAKE_DAMAGE3_SE = @"Audio\SoundEffects\MST ENMY DMG C";
        public const string ENEMY_DEATH_SE = @"Audio\SoundEffects\MST ENMY DEATH";
        
        public const string ACQUIRE_POWERUP_SE = @"Audio\SoundEffects\MST PLR PWR UP";

        public const string LEVEL_END_SE = @"Audio\SoundEffects\MST LVL COMP";
        public const string GAME_END_RUMBLE_SE = @"Audio\SoundEffects\MST GameEnd Rumble";

        public const string MENU_START_GAME_SE = @"Audio\SoundEffects\MST MENU START.2";
        public const string MENU_TOGGLE_SE = @"Audio\SoundEffects\MST MENU TOGGLE";
        public const string MENU_IN_SE = @"Audio\SoundEffects\MST MENU IN";
        public const string MENU_OUT_SE = @"Audio\SoundEffects\MST MENU OUT";


        // Screen configuration
        public const int CAMERA_SCREEN_X_OFFSET = 0;
        public const int CAMERA_SCREEN_Y_OFFSET = -100;


        // Collision configuration
        public const int ALPHA_THRESHOLD = 48;
        public const double DAMAGE_TEXTURE_DURATION = 200d;
        public const int ENEMY_COLLISION_DAMAGE = 25;

        public const float EXPLOSION_PARTICLE_LIFETIME = 1000f;
        public const float EXPLOSION_PARTICLE_FADE_AGE = 500f;
        public const float EXPLOSION_PARTICLE_EMITTER_STOP_AGE = 250f;
        public const float EXPLOSION_PARTICLE_ROTATION = 0.1f;
        public const float EXPLOSION_PARTICLE_EMITTER_SPAWN_FREQUENCY = 60f;
        public const float EXPLOSION_PARTICLE_EMITTER_ROTATION = 0.1f;
        public const int EXPLOSION_PARTICLE_EMITTER_FADE_START = 150;
        public const int EXPLOSION_PARTICLE_EMIITER_FADE_END = 0;
        public const float EXPLOSION_PARTICLE_EMITTER_SCALE_START = 0.1f;
        public const float EXPLOSION_PARTICLE_EMITTER_SCALE_END = 0.4f;
        
        public const int ENEMY_EXPLOSION_PARTICLE_MASS = 40;
        public const int ENEMY_EXPLOSION_PARTICLE_MASS_RANDOM_RANGE = 10;
        public const float ENEMY_EXPLOSION_PARTICLE_ROTATION = 0.1f;
        public const float ENEMY_EXPLOSION_PARTICLE_SPEED = 7f;
        public const int ENEMY_EXPLOSION_PARTICLE_SPEED_RANDOM_RANGE = 0;
        
        public const int TILE_EXPLOSION_PARTICLE_COUNT_X = 4;
        public const int TILE_EXPLOSION_PARTICLE_COUNT_Y = 4;
        public const int TILE_EXPLOSION_PARTICLE_WIDTH = 8;
        public const int TILE_EXPLOSION_PARTICLE_HEIGHT = 8;
        public const int TILE_EXPLOSION_PARTICLE_MASS = 50;
        public const int TILE_EXPLOSION_PARTICLE_MASS_RANDOM_RANGE = 5;
        public const float TILE_EXPLOSION_PARTICLE_ROTATION = 0.1f;
        public const float TILE_EXPLOSION_PARTICLE_SPEED = 4f;
        

        // Map configuration
        public const string VOLUME_LAYER = "Volumes";
        public const string CAMERA_BOUNDS_LAYER = "CameraBounds";
        public const string COLLISION_LAYER = "Collision";
        public const string FOREGROUND_LAYER = "Foreground";
        public const string BACKGROUND_LAYER = "Background";
        public const string DESTRUCTIBLE_LAYER = "Destructible";
        public const string OBJECTS_LAYER = "Objects";

        public const string SPAWN_DELAY = "SpawnDelay";

        public const string CAMERA_BOUNDS_TRANSITION = "Transition";
        public const string CAMERA_BOUNDS_OFFSET_X = "OffsetX";
        public const string CAMERA_BOUNDS_OFFSET_Y = "OffsetY";
        public const string CAMERA_BOUNDS_OFFSET_PRIORITY = "OffsetPriority";

        public const string LEVEL_END_VOLUME_TYPE = "LevelEnd";
        public const string DEATH_VOLUME_TYPE = "Death";
        public const string HEALTH_VOLUME_TYPE = "Health";
        public const string DAMAGE_VOLUME_TYPE = "Damage";
        public const string TRIGGER_VOLUME_TYPE = "Trigger";
        public const string CHECKPOINT_VOLUME_TYPE = "Checkpoint";
        public const string END_GAME_EXPLOSION_VOLUME_TYPE = "EndGameExplosion";
        public const string START_DELAY_PROPERTY = "StartDelay";
        public const string GAME_END_VOLUME_TYPE = "GameEnd";
        public const string INTERVAL_PROPERTY = "Interval";
        public const string AMOUNT_PROPERTY = "Amount";
        public const string FORCE_VOLUME_TYPE = "Force";
        public const string FORCE_X_PROPERTY = "ForceX";
        public const string FORCE_Y_PROPERTY = "ForceY";
        public const string ENEMY_SPAWN_VOLUME_TYPE = "EnemySpawn";
        public const string ENEMY_SPAWN_POINT_LIST_PROPERTY = "EnemySpawnList";
        public const string ENEMY_SPAWN_REF_PROPERTY = "EnemySpawnRef";
        public const string TIMED_ENEMY_SPAWN_VOLUME_TYPE = "TimedEnemySpawn";
        public const string SPAWN_POINT_TEMPLATE = "SpawnPointTemplate";
        public const string TRIGGER_KEY_PROPERTY = "TriggerKey";

        public const string TURRET_BODY_SPRITE_PROPERTY = "TurretBodySprite";
        public const string TURRET_NOZZLE_SPRITE_PROPERTY = "TurretNozzleSprite";
        public const string FIRE_DURATION_PROPERTY = "FireDuration";
        public const string FIRE_DELAY_PROPERTY = "FireDelay";
        public const string INVINCIBLE_PROPERTY = "Invincible";
        public const string COLLIDE_WITH_MOVERS = "CollideWithMovers";

        public const string PLAYER_SPAWN_MAP_TYPE = "PlayerSpawn";
        public const string CHECKPOINT_SPAWN_MAP_TYPE = "CheckpointSpawn";
        public const string ENEMY_SPAWN_MAP_TYPE = "EnemySpawn";
        public const string POWERUP_SPAWN_MAP_TYPE = "PowerupSpawn";
        public const string PARTICLE_EMITTER_SPAWN_MAP_TYPE = "ParticleEmitterSpawn";
        public const string MOVER_SPAWN_MAP_TYPE = "MoverSpawn";
        public const string ENVIRONMENT_ACTOR_SPAWN_MAP_TYPE = "EnvironmentActor";

        public const string ENVIRONMENT_ACTOR_SPRITE_KEY = "SpriteKey";
        public const string ROTATION = "Rotation";

        public const string OBJECT_TYPE_PROPERTY = "ObjectType";

        public const string DIFFICULTY = "Difficulty";

        public const string BASIC_MOVER = "BasicMover";
        public const string SUSPENDED_MOVER = "SuspendedMover";
        public const string MOVER_SPRITE_KEY = "MoverSpriteKey";
        public const string WAYPOINT = "Waypoint";
        public const string MAX_VELOCITY = "MaxVelocity";
        public const string ACCELERATION_PROPERTY = "Acceleration";
        public const string STOP_TIME = "StopTime";
        public const string WAYPOINT_ITERATOR = "WaypointIterator";
        public const string FORWARD_AND_BACKWARD = "ForwardAndBackward";
        public const string CIRCULAR = "Circular";
        public const string OFF_SCREEN_RESET = "OffScreenReset";

        public const string ENEMY_ORIENTATION = "EnemyOrientation";
        public const string SPEED = "Speed";
        public const string CRAWLER_FIRE_MODE = "CrawlerFireMode";
        
        public const string EASY_PAWN = "EasyPawn";
        public const string AERIAL_PAWN = "AerialPawn";
        public const string GRENADIER = "Grenadier";
        public const string CRAWLER = "Crawler";
        public const string AERIAL_BOMBER = "AerialBomber";
        public const string HARD_PAWN = "HardPawn";
        public const string WASP = "Wasp";
        public const string SIMPLE_TURRET = "SimpleTurret";
        public const string TRACKING_TURRET = "TrackingTurret";
        public const string CORE = "Core";
        public const string CORE_SHIELD = "CoreShield";

        public const string CORE_REF = "CoreRef";
        public const string ROTATION_ARM_LENGTH = "RotationArmLength";
        public const string ROTATION_INCREMENT = "RotationIncrement";

        public const string ENEMY_SPAWN_BOUNDARY_PROPERTY = "SpawnBoundary";
        public const string AERIAL_PAWN_RANGE_PROPERTY = "Range";

        public const string LEFT_WEAPON_ENABLED = "LeftWeaponEnabled";
        public const string RIGHT_WEAPON_ENABLED = "RightWeaponEnabled";

        public const string WAYPOINT_TYPE = "Waypoint";
        public const string WAYPOINT_LIST_PROPERTY = "WaypointList";
        public const string WAYPOINT_A_PROPERTY = "WaypointA";
        public const string WAYPOINT_B_PROPERTY = "WaypointB";
        public const string WAYPOINT_ORIENTATION_PROPERTY = "WaypointOrientation";
        public const string WAYPOINT_ORIENTATION_HORIZONTAL = "Horizontal";
        public const string WAYPOINT_ORIENTATION_VERTICAL = "Vertical";

        public const string ENEMY_POWERUP_SPAWN_TYPE = "PowerupType";
        public const string ENEMY_POWERUP_SPAWN_PHYSICS_MODE = "PowerupPhysicsMode";
        public const string PHYSICS_MODE_NONE = "None";
        public const string PHYSICS_MODE_GRAVITY = "Gravity";

        public const string INITIAL_HORIZONTAL_DIRECTION = "InitialDirectionX";

        public const string POSITION_CORRECTION_MODE = "PositionCorrectionMode";

        public const string POWERUP_AMMO = "Ammo";
        public const string POWERUP_LIFE = "Life";
        public const string POWERUP_HEALTH = "Health";
        public const string POWERUP_GRENADE = "Grenade";
        public const string POWERUP_HOMING_MISSILE = "HomingMissile";
        public const string POWERUP_DOUBLE_DAMAGE = "DoubleDamage";
        public const string POWERUP_JETPACK = "Jetpack";

        public const string PARTICLE_SPRITE_KEY = "ParticleSpriteKey";
        public const string PARTICLE_SPAWN_FREQUENCY_PROPERTY = "ParticleSpawnFrequency";
        public const string PARTICLE_VELOCITY_X_PROPERTY = "ParticleVelocityX";
        public const string PARTICLE_VELOCITY_Y_PROPERTY = "ParticleVelocityY";
        public const string PARTICLE_ACCELERATION_X_PROPERTY = "ParticleAccelerationX";
        public const string PARTICLE_ACCELERATION_Y_PROPERTY = "ParticleAccelerationY";
        public const string PARTICLE_MAX_VELOCITY_X_PROPERTY = "ParticleMaxVelocityX";
        public const string PARTICLE_MAX_VELOCITY_Y_PROPERTY = "ParticleMaxVelocityY";
        public const string PARTICLE_ROTATION_SPEED = "ParticleRotationSpeed";
        public const string PARTICLE_FADE_START = "ParticleFadeStart";
        public const string PARTICLE_FADE_END = "ParticleFadeEnd";
        public const string PARTICLE_SCALE_START = "ParticleScaleStart";
        public const string PARTICLE_SCALE_END = "ParticleScaleEnd";

        public const int ENEMY_OFFSCREEN_SPAWN_THRESHOLD = 100; // pixels
        

        // Keyboard configuration
        public const Keys KEYBOARD_MOVE_LEFT = Keys.A;
        public const Keys KEYBOARD_MOVE_RIGHT = Keys.D;
        public const Keys KEYBOARD_MOVE_UP = Keys.W;
        public const Keys KEYBOARD_MOVE_DOWN = Keys.S;
        public const Keys KEYBOARD_JUMP = Keys.Space;
        public const Keys KEYBOARD_WEAPON_SWITCH = Keys.Tab;
        public const Keys KEYBOARD_PAUSE = Keys.Enter;
        public const Keys KEYBOARD_MENU_SELECT = Keys.Space;
        public const Keys KEYBOARD_MENU_GO_BACK = Keys.Escape;


        // Gamepad configuration
        public const float THUMBSTICK_MOVE_THRESHOLD = 0.1f;
        public const Buttons GAMEPAD_PRIMARY_FIRE = Buttons.RightShoulder;
        public const Buttons GAMEPAD_SECONDARY_FIRE = Buttons.RightTrigger;
        public const Buttons GAMEPAD_JUMP = Buttons.LeftTrigger;
        public const Buttons GAMEPAD_WEAPON_SWITCH = Buttons.A;
        public const Buttons GAMEPAD_PAUSE = Buttons.Start;
        public const Buttons GAMEPAD_MENU_SELECT = Buttons.A;
        public const Buttons GAMEPAD_MENU_GO_BACK = Buttons.B;


        // Actor configuration
        public const double FLICKER_INTERVAL = 100d;
        public const float BLAST_SMOKE_POSITION_OFFSET = 5f;
        public const float BLAST_SMOKE_SPEED = 0.5f;
        public const float BLAST_SMOKE_SCALE_START = 0.25f;
        public const float BLAST_SMOKE_SCALE_END = 0.5f;
        public const float BLAST_SMOKE_ROTATION_INCREMENT = 0.1f;
        public const double SUSPENDED_MOVER_RESET_DELAY = 3000f;

        
        // Player configuration
        public const int PLAYER_START_LIVES = 3;
        public const float PLAYER_MASS = 150.0f;
        public const float PLAYER_GRAVITY_FORCE = 165.375f;
        public const float INPUT_FORCE_MULTIPLIER = 225f;
        public const float PLAYER_JUMP_VELOCITY = 12f;
        public const float PLAYER_JUMP_HOLD_FORCE = 1000.0f;
        public const float PLAYER_JUMP_HOLD_FORCE_VELOCITY_THRESHOLD = -0.0f;
        public const float JETPACK_FORCE = 2300f;
        public const float JETPACK_MAX_VERTICAL_SPEED = 7.0f; // pixels per game tick
        public const float JETPACK_Y_OFFSET = 25f;
        public const float JETPACK_PARTICLE_SPAWN_FREQUENCY = 75f;
        public const float JETPACK_PARTICLE_ROTATION = 0.05f;
        public const float JETPACK_PARTICLE_SCALE_START = 0.5f;
        public const float JETPACK_PARTICLE_SCALE_END = 1.5f;
        public const int PLAYER_START_HEALTH = 100;
        public const int PLAYER_HEALTH_MAX = 100;
        public const float PLAYER_PRIMARY_PROJECTILE_SPAWN_OFFSET = 32f;
        public const float PLAYER_SECONDARY_PROJECTILE_SPAWN_OFFSET = 45f;
        public const double INVINCIBILITY_DURATION = 1000d;
        public const float LAND_VIBRATION_VELOCITY_THRESHOLD = 20f;

        public const float PLAYER_GRENADE_PROJECTILE_MASS = 100.0f; // kg
        public const float PLAYER_GRENADE_GRAVITY_FORCE = 73.5f;
        public const float HOMING_MISSILE_PROJECTILE_MASS = 50.0f; // kg
        public const float HOMING_MISSILE_GRAVITY_FORCE = 18.375f;
        public const float PLAYER_PRIMARY_FIRE_DELAY = 167f;
        public const float PLAYER_SECONDARY_FIRE_DELAY = 1000f;
        public const int PLAYER_SECONDARY_AMMO_MAX = 20;
        //public const float PLAYER_BULLET_SPEED = 30f;
        public const float PLAYER_BULLET_SPEED = 21f;
        public const float PLAYER_GRENADE_SPEED = 20f;
        public const float PLAYER_HOMING_MISSILE_LAUNCH_SPEED = 17f;
        public const int PLAYER_BULLET_DAMAGE = 10;
        public const int PLAYER_BULLET_DOUBLE_DAMAGE = PLAYER_BULLET_DAMAGE * 2;
        public const int PLAYER_GRENADE_BLAST_DAMAGE = 60;
        public const int PLAYER_GRENADE_BLAST_RADIUS = 120;
        public const float PLAYER_GRENADE_ROTATION_AMOUNT = 0.15f;
        public const int HOMING_MISSILE_BLAST_DAMAGE = 50;
        public const int HOMING_MISSILE_BLAST_RADIUS = 75;
        public const int HOMING_MISSILE_TARGET_RADIUS = 400;
        public const float HOMING_MISSILE_TURN_INCREMENT = 0.35f;
        public const float HOMING_MISSILE_MAX_SEEK_SPEED = 15f;
        public const float HOMING_MISSILE_SEEK_ACCELERATION = 1f;
        public const float HOMING_MISSILE_LERP_AMOUNT = 0.15f;
        public const float HOMING_MISSILE_SEEK_EFFECT_INTERVAL = 100f;
        public const float HOMING_MISSILE_ROTATION_AMOUNT = 0.25f;
        public const float HOMING_MISSILE_PARTICLE_SPAWN_FREQUENCY = 17;
        public const float HOMING_MISSILE_PARTICLE_ROTATION = 0.1f;
        public const float HOMING_MISSILE_PARTICLE_SCALE_START = 0.25f;
        public const float HOMING_MISSILE_PARTICLE_SCALE_END = 0.75f;
        public const float HOMING_MISSILE_SELF_DESTRUCT_TIMEOUT = 5000f;

        public const double TAKE_DAMAGE_VIBRATION_DURATION = 275d;
        public const double LAND_VIBRATION_DURATION = 175d;

        public const float TAKE_DAMAGE_LF_VIBRATION = 1.0f;
        public const float TAKE_DAMAGE_HF_VIBRATION = 0.0f;
        public const float JETPACK_LF_VIBRATION = 0.0f;
        public const float JETPACK_HF_VIBRATION = 0.20f;
        public const float LAND_LF_VIBRATION = 0.4f;
        public const float LAND_HF_VIBRATION = 0.0f;

        public const float JETPACK_FUEL_MAX = 100f;
        public const float JETPACK_FUEL_DEPLETION_RATE = 1.25f;
        
        public const float JETPACK_FUEL_REPLENISH_RATE = 0.5f;

        public const double DOUBLE_DAMAGE_TEXTURE_FLICKER_FREQUENCY = 250d;


        // Enemy configuration
        public const float ENEMY_GRAVITY_FORCE = 165.375f;
        public const float ENEMY_PROJECTILE_GRAVITY_FORCE = 26.46f;

        public const int EASY_PAWN_START_HEALTH = 20;
        public const float EASY_PAWN_MASS = 150.0f; // kg
        //public const float EASY_PAWN_HORIZONTAL_SPEED = 5f; // pixels per game tick
        public const float EASY_PAWN_HORIZONTAL_SPEED = 4f;
        public const float EASY_PAWN_ATTACK_RANGE = 500f; // pixels
        public const float EASY_PAWN_FIRE_DELAY = 1500f; // milliseconds
        public const float EASY_PAWN_CHANGE_DIRECTION_DELAY = 1500f;
        //public const float EASY_PAWN_PROJECTILE_BULLET_SPEED = 12f; // pixels per game tick
        public const float EASY_PAWN_PROJECTILE_BULLET_SPEED = 10f;
        public const int EASY_PAWN_PROJECTILE_DAMAGE = 10;
        public const float PAWN_PROJECTILE_SPAWN_OFFSET = 25f;

        public const int AERIAL_PAWN_START_HEALTH = 20;
        public const float AERIAL_PAWN_HORIZONTAL_SPEED = 3f;
        public const float AERIAL_PAWN_VERTICAL_SPEED = 3.5f;
        public const int AERIAL_PAWN_VERTICAL_RANGE = 125;
        public const float AERIAL_PAWN_FIRE_DELAY = 1000f;
        public const int AERIAL_PAWN_PROJECTILE_DAMAGE = 10;
        public const float AERIAL_PAWN_PROJECTILE_SPEED = 7f;
        public const int AERIAL_PAWN_PROJECTILE_SPAWN_OFFSET = 20;
        public const int AERIAL_PAWN_HORIZONTAL_ATTACK_THRESHOLD = 600;
        public const int AERIAL_PAWN_VERTICAL_ATTACK_THRESHOLD = 500;
        public const float AERIAL_PAWN_PROJECTILE_PARTICLE_SPAWN_FREQUENCY = 100f;

        public const int WASP_START_HEALTH = 50;
        public const int WASP_ATTACK_RECTANGLE_WIDTH = 800;
        public const int WASP_ATTACK_RECTANGLE_HEIGHT = 200;
        public const int WASP_ATTACK_RECTANGLE_OFFSET_Y = -100;
        public const float WASP_ADDITIONAL_FIRE_DELAY = 500f;
        public const float WASP_BURST_DELAY = 200f;
        public static int WASP_BURST_AMOUNT = 3;
        public const int WASP_PROJECTILE_DAMAGE = 20;
        //public const float WASP_PROJECTILE_SPEED = 15;
        public const float WASP_PROJECTILE_SPEED = 10;
        public const int WASP_PROJECTILE_SPAWN_OFFSET = 20;
        public const float WASP_PARTICLE_SPAWN_FREQUENCY = 20;
        public const int WASP_TRANSITION_TICKS = 20;
        public const float WASP_MINIMUM_TRANSITION_DISTANCE = 250;

        public const int GRENADIER_START_HEALTH_NORMAL = 40;
        public const int GRENADIER_START_HEALTH_HARD = 60;
        public const float GRENADIER_MASS = 150.0f; //kg
        public const float GRENADIER_MAX_HORIZONTAL_SPEED = 3f; // pixels per game tick
        public const int GRENADIER_HORIZONTAL_ATTACK_THRESHOLD = 500; // pixels
        public const int GRENADIER_VERTICAL_ATTACK_THRESHOLD_NORMAL = 420; // pixels
        public const int GRENADIER_VERTICAL_ATTACK_THRESHOLD_HARD = 510; // pixels
        public const int GRENADIER_PROJECTILE_BLAST_DAMAGE = 40;
        public const int GRENADIER_PROJECTILE_BLAST_RADIUS = 80;
        public const float GRENADIER_PROJECTILE_LAUNCH_SPEED = 10f;
        public const float GRENADIER_PROJECTILE_MASS = 60f;
        public const float GRENADIER_PROJECTILE_ROTATION_INCREMENT = 0.30f;

        public const int AERIAL_BOMBER_START_HEALTH = 100;
        public const float AERIAL_BOMBER_ACCELERATION = 0.15f; // pixels per game tick
        public const float AERIAL_BOMBER_MAX_HORIZONTAL_SPEED = 5f; // pixels per game tick
        public const float AERIAL_BOMBER_HORIZONTAL_ATTACK_RANGE = 30f; // pixels
        public const float AERIAL_BOMBER_VERTICAL_ATTACK_RANGE = 700f;
        public const float AERIAL_BOMBER_FIRE_DELAY = 2000f; // milliseconds
        public const double AERIAL_BOMBER_CHANGE_DIRECTION_DELAY = 500d; // milliseconds
        public const int AERIAL_BOMBER_CHANGE_DIRECTION_RANDOM_RANGE = 400; // milliseconds
        public const int AERIAL_BOMBER_PROJECTILE_BLAST_DAMAGE = 60;
        public const int AERIAL_BOMBER_PROJECTILE_BLAST_RADIUS = 150;
        public const float AERIAL_BOMBER_PROJECTILE_LAUNCH_SPEED = 2f;
        //public const float AERIAL_BOMBER_PROJECTILE_MASS = 60f;
        public const float AERIAL_BOMBER_PROJECTILE_MASS = 40f;
        public const float AERIAL_BOMBER_PROJECTILE_ROTATION_INCREMENT = 0f;
        public const float AERIAL_BOMBER_PROJECTILE_SPAWN_OFFSET = 30f;

        public const int HARD_PAWN_START_HEALTH = 60;
        public const float HARD_PAWN_MASS = 150.0f; //kg
        public const float HARD_PAWN_GRAVITY_FORCE = 165.375f;
        public const float HARD_PAWN_JUMP_FORCE = 2800f;
        public const float HARD_PAWN_HORIZONTAL_SPEED = 5f; // pixels per game tick
        //public const float HARD_PAWN_HORIZONTAL_SPEED = 4f;
        public const float HARD_PAWN_ATTACK_RANGE = 800f; // pixels
        public const float HARD_PAWN_FIRE_DELAY = 2300f; // milliseconds
        //public const float HARD_PAWN_PROJECTILE_BULLET_SPEED = 12f; // pixels per game tick
        public const float HARD_PAWN_PROJECTILE_BULLET_SPEED = 10f;
        public const int HARD_PAWN_PROJECTILE_DAMAGE = 25;
        public const float HARD_PAWN_PROJECTILE_HORIZONAL_PROXIMITY_THRESHOLD = 150f; // pixels
        public const int HARD_PAWN_PROJECTILE_ANGLE_THRESHOLD = 30; // degrees
        public const float HARD_PAWN_JUMP_VELOCITY = 20f; // pixels per game tick
        public const float HARD_PAWN_DIRECTION_CHANGE_DELAY = 1250f; // milliseconds
        public const float HARD_PAWN_DIRECTION_CHANGE_DELAY_RANDOM_RANGE = 300f;
        public const float HARD_PAWN_JUMP_DISABLE_TIME = 1000f; // milliseconds
        //public const float HARD_PAWN_BURST_FIRE_DELAY = 350f; // milliseconds
        public const float HARD_PAWN_BURST_FIRE_DELAY = 450f; // milliseconds
        public const int HARD_PAWN_BURST_FIRE_AMOUNT = 3;

        public const int CRAWLER_START_HEALTH = 40;
        public const int CRAWLER_SPEED = 3;
        public const int CRAWLER_ATTACK_RANGE_SHORT = 30;
        public const int CRAWLER_ATTACK_RANGE_LONG = 800;
        public const float CRAWLER_FIRE_DELAY = 1100f;
        public const int CRAWLER_PROJECTILE_BLAST_DAMAGE = 40;
        public const int CRAWLER_PROJECTILE_BLAST_RADIUS = 80;
        public const int CRAWLER_PROJECTILE_BULLET_DAMAGE = 10;
        //public const float CRAWLER_PROJECTILE_BULLET_SPEED = 12f;
        //public const float CRAWLER_PROJECTILE_LAUNCH_SPEED = 10f;
        public const float CRAWLER_PROJECTILE_BULLET_SPEED = 10f;
        public const float CRAWLER_PROJECTILE_LAUNCH_SPEED = 8f;
        public const float CRAWLER_PROJECTILE_ROTATION_INCREMENT = 0.15f;
        public const float CRAWLER_PROJECTILE_MASS = 25f;


        public const int CORE_HEALTH = 500;
        public const int CORE_SHIELD_HEALTH = 60;


        // Laser configuration
        public const int LASER_LENGTH_INCREMENT = 32;
        public const int LASER_LINE_HEIGHT = 4;
        public const int LASER_GLOW_HEIGHT = 12;

        
        // Physics configuration
        public const float AIR_DRAG_FACTOR = 0.4f;

        public const float MIN_HORIZONTAL_SPEED = 1f; // pixels per game tick
        public const float MAX_HORIZONTAL_SPEED = 8.5f; // pixels per game tick
        public const float MAX_VERTICAL_SPEED = 20f; // pixels per game tick
        public const float MAX_INPUT_FORCE = 11000f;
        public const float POWERUP_MASS = 70f;
        public const float POWERUP_GRAVITY_FORCE = 36.015f;

        public const float PROJECTILE_MAX_HORIZONTAL_SPEED = 30f;
        public const float PROJECTILE_MAX_VERTICAL_SPEED = 30f;


        // Powerup configuration
        public const int POWERUP_POINT_VALUE = 25;
        public const int HEALTH_POWERUP_VALUE = 25;
        public const int AMMO_POWERUP_VALUE = 5;
        public const double DOUBLE_DAMAGE_DURATION = 5000d;
        public const float POWERUP_SPAWN_Y_VELOCITY = -8f;
        public const float POWERUP_SPAWN_X_VELOCITY = 0f;
        public const float POWERUP_MAX_Y_VELOCITY = 8f;


        // Sound configuration
        public const float SOUND_EFFECT_VOLUME = 0.5f;
        public const float MUSIC_VOLUME = 0.6f;

        //public const float SOUND_EFFECT_VOLUME = 0.0f;
        //public const float MUSIC_VOLUME = 0.0f;
    }
}
