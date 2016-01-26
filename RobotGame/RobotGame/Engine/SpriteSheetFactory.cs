using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame.Engine
{
    public enum SpriteKey
    {
        Player = 0,
        PlayerJetpack,
        PlayerJetpackFlame,
        Nozzle,
        JetpackExhaust,

        EasyPawn,
        AerialPawn,
        Grenadier,
        HardGrenadier,
        AerialBomber,
        HardPawn,
        Wasp,
        CrawlerUp,
        CrawlerDown,
        CrawlerLeft,
        CrawlerRight,
        StationaryTurretUp,
        StationaryTurretDown,
        StationaryTurretLeft,
        StationaryTurretRight,
        TrackingTurretInvincibleUp,
        TrackingTurretInvincibleDown,
        TrackingTurretInvincibleLeft,
        TrackingTurretInvincibleRight,
        StationaryTurretInvincibleUp,
        StationaryTurretInvincibleDown,
        StationaryTurretInvincibleLeft,
        StationaryTurretInvincibleRight,
        MovingTurretUp,
        MovingTurretDown,
        MovingTurretLeft,
        MovingTurretRight,
        TurretLaserNozzle,
        TurretInvincibleLaserNozzle,

        Core,
        CoreShield,

        PlayerExplosion,
        EasyPawnExplosion,
        GrenadierExplosion,
        HardGrenadierExplosion,
        CrawlerExplosion,
        AerialPawnExplosion,
        AerialBomberExplosion,
        WaspExplosion,
        TurretExplosion,
        HardPawnExplosion,
        CoreExplosion,
        CoreShieldExplosion,

        Bullet,
        Grenade,
        HomingMissile,
        HomingMissileSeek,
        LaserLine,
        LaserGlow,        

        BulletCollisionParticle,
        EnemyBulletCollisionParticle,
        EasyPawnProjectile,
        AerialPawnProjectile,
        AerialPawnProjectileParticle,
        GrenadierProjectile,
        AerialBomberProjectile,
        HardPawnProjectile,
        CrawlerBombProjectile,
        CrawlerSpiralProjectile,

        TargetArrow,

        HealthPowerup,
        LifePowerup,
        DoubleDamagePowerup,
        AmmoPowerup,
        GrenadePowerup,
        HomingMissilePowerup,
        JetpackPowerup,

        Explosion,
        ExplosionSmoke,

        Level1Mover,
        Level2Mover,
        Level3Mover,
        Level3TallMover,
        Level3LongMover,
        Level3FinalMover,

        Level1TileExplosion,
        Level2TileExplosion,
        Level3TileExplosion,

        LevelEndFlag,
        Fan,

        None
    }

    class SpriteSheetFactory
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const string PLAYER_ASSET = Config.PLAYER_ASSET;
        private const string PLAYER_JETPACK_ASSET = Config.PLAYER_JETPACK_ASSET;
        private const string PLAYER_JETPACK_FLAME = Config.PLAYER_JETPACK_FLAME;
        private const string NOZZLE_ASSET = Config.NOZZLE_ASSET;
        private const string JETPACK_EXHAUST_ASSET = Config.SMOKE_ASSET;
        private const string EASY_PAWN_ASSET = Config.EASY_PAWN_ASSET;
        private const string AERIAL_PAWN_ASSET = Config.AERIAL_PAWN_ASSET;
        private const string GRENADIER_ASSET = Config.GRENADIER_ASSET;
        private const string HARD_GRENADIER_ASSET = Config.HARD_GRENADIER_ASSET;
        private const string AERIAL_BOMBER_ASSET = Config.AERIAL_BOMBER_ASSET;
        private const string HARD_PAWN_ASSET = Config.HARD_PAWN_ASSET;
        private const string WASP_ASSET = Config.WASP_ASSET;
        private const string CRAWLER_UP_ASSET = Config.CRAWLER_UP_ASSET;
        private const string CRAWLER_DOWN_ASSET = Config.CRAWLER_DOWN_ASSET;
        private const string CRAWLER_LEFT_ASSET = Config.CRAWLER_LEFT_ASSET;
        private const string CRAWLER_RIGHT_ASSET = Config.CRAWLER_RIGHT_ASSET;
        private const string TURRET_STATIONARY_UP_ASSET = Config.TURRET_STATIONARY_UP_ASSET;
        private const string TURRET_STATIONARY_DOWN_ASSET = Config.TURRET_STATIONARY_DOWN_ASSET;
        private const string TURRET_STATIONARY_LEFT_ASSET = Config.TURRET_STATIONARY_LEFT_ASSET;
        private const string TURRET_STATIONARY_RIGHT_ASSET = Config.TURRET_STATIONARY_RIGHT_ASSET;
        private const string TRACKING_TURRET_INVINCIBLE_UP_ASSET = Config.TRACKING_TURRET_INVINCIBLE_UP_ASSET;
        private const string TRACKING_TURRET_INVINCIBLE_DOWN_ASSET = Config.TRACKING_TURRET_INVINCIBLE_DOWN_ASSET;
        private const string TRACKING_TURRET_INVINCIBLE_LEFT_ASSET = Config.TRACKING_TURRET_INVINCIBLE_LEFT_ASSET;
        private const string TRACKING_TURRET_INVINCIBLE_RIGHT_ASSET = Config.TRACKING_TURRET_INVINCIBLE_RIGHT_ASSET;
        private const string TURRET_STATIONARY_INVINCIBLE_UP_ASSET = Config.TURRET_STATIONARY_INVINCIBLE_UP_ASSET;
        private const string TURRET_STATIONARY_INVINCIBLE_DOWN_ASSET = Config.TURRET_STATIONARY_INVINCIBLE_DOWN_ASSET;
        private const string TURRET_STATIONARY_INVINCIBLE_LEFT_ASSET = Config.TURRET_STATIONARY_INVINCIBLE_LEFT_ASSET;
        private const string TURRET_STATIONARY_INVINCIBLE_RIGHT_ASSET = Config.TURRET_STATIONARY_INVINCIBLE_RIGHT_ASSET;
        private const string TURRET_MOVING_UP_ASSET = Config.TURRET_MOVING_UP_ASSET;
        private const string TURRET_MOVING_DOWN_ASSET = Config.TURRET_MOVING_DOWN_ASSET;
        private const string TURRET_MOVING_LEFT_ASSET = Config.TURRET_MOVING_LEFT_ASSET;
        private const string TURRET_MOVING_RIGHT_ASSET = Config.TURRET_MOVING_RIGHT_ASSET;
        private const string TURRET_LASER_NOZZLE_ASSET = Config.TURRET_LASER_NOZZLE_ASSET;
        private const string TURRET_INVINCIBLE_LASER_NOZZLE_ASSET = Config.TURRET_INVINCIBLE_LASER_NOZZLE_ASSET;
        private const string TURRET_PROJECTILE_NOZZLE_ASSET = Config.TURRET_PROJECTILE_NOZZLE_ASSET;

        private const string CORE_ASSET = Config.CORE_ASSET;
        private const string CORE_SHIELD_ASSET = Config.CORE_SHIELD_ASSET;
      
        private const string PLAYER_EXPLOSION_ASSET = Config.PLAYER_EXPLOSION_ASSET;
        private const string EASY_PAWN_EXPLOSION_ASSET = Config.EASY_PAWN_EXPLOSION_ASSET;
        private const string GRENADIER_EXPLOSION_ASSET = Config.GRENADIER_EXPLOSION_ASSET;
        private const string HARD_GRENADIER_EXPLOSION_ASSET = Config.HARD_GRENADIER_EXPLOSION_ASSET;
        private const string CRAWLER_EXPLOSION_ASSET = Config.CRAWLER_EXPLOSION_ASSET;
        private const string AERIAL_PAWN_EXPLOSION_ASSET = Config.AERIAL_PAWN_EXPLOSION_ASSET;
        private const string AERIAL_BOMBER_EXPLOSION_ASSET = Config.AERIAL_BOMBER_EXPLOSION_ASSET;
        private const string WASP_EXPLOSION_ASSET = Config.WASP_EXPLOSION_ASSET;
        private const string TURRET_EXPLOSION_ASSET = Config.TURRET_EXPLOSION_ASSET;
        private const string HARD_PAWN_EXPLOSION_ASSET = Config.HARD_PAWN_EXPLOSION_ASSET;
        private const string CORE_SHIELD_EXPLOSION_ASSET = Config.CORE_SHIELD_EXPLOSION_ASSET;
        private const string CORE_EXPLOSION_ASSET = Config.CORE_EXPLOSION_ASSET;

        private const string BULLET_PROJECTILE_COLLISION_PARTICLE_ASSET = Config.BULLET_PROJECTILE_COLLISION_PARTILE_ASSET;
        private const string ENEMY_BULLET_PROJECTILE_COLLISION_PARTICLE_ASSET = Config.ENEMY_BULLET_PROJECTILE_COLLISION_PARTICLE_ASSET;
        private const string EASY_PAWN_PROJECTILE_ASSET = Config.EASY_PAWN_BULLET_PROJECTILE_ASSET;
        private const string AERIAL_PAWN_PROJECTILE_ASSET = Config.AERIAL_PAWN_ORB_PROJECTILE_ASSET;
        private const string AERIAL_PAWN_PROJECTILE_PARTICLE = Config.AERIAL_PAWN_ORB_TAIL_ASSET;
        private const string AERIAL_BOMBER_PROJECTILE_ASSET = Config.AERIAL_BOMBER_PROJECTILE_ASSET;
        private const string GRENADIER_PROJECTILE_ASSET = Config.GRENADIER_PROJECTILE_ASSET;
        private const string CRAWLER_BOMB_PROJECTILE = Config.CRAWLER_BOMB_PROJECTILE_ASSET;
        private const string CRAWLER_SPIRAL_PROJECTILE = Config.CRAWLER_SPIRAL_PROJECTILE_ASSET;
        private const string HARD_PAWN_PROJECTILE_ASSET = Config.HARD_PAWN_BULLET_PROJECTILE_ASSET;
        private const string LASER_LINE_ASSET = Config.LASER_LINE_ASSET;
        private const string LASER_GLOW_ASSET = Config.LASER_GLOW_ASSET;

        private const string BULLET_PROJECTILE_ASSET = Config.BULLET_PROJECTILE_ASSET;
        private const string GRENADE_PROJECTILE_ASSET = Config.GRENADE_PROJECTILE_ASSET;
        private const string HOMING_MISSILE_PROJECTILE_ASSET = Config.HOMING_MISSILE_PROJECTILE_ASSET;
        private const string HOMING_MISSILE_PROJECTILE_SEEK_ASSET = Config.HOMING_MISSILE_PROJECTILE_SEEK_ASSET;
        private const string TARGET_ARROW_ASSET = Config.TARGET_ARROW_ASSET;

        private const string HEALTH_POWERUP_ASSET = Config.POWERUP_HEALTH_ASSET;
        private const string LIFE_POWERUP_ASSET = Config.POWERUP_LIFE_ASSET;
        private const string DOUBLE_DAMAGE_POWERUP_ASSET = Config.DOUBLE_DAMAGE_POWERUP_ASSET;
        private const string AMMO_POWERUP_ASSET = Config.POWERUP_AMMO_ASSET;
        private const string GRENADE_POWERUP_ASSET = Config.POWERUP_GRENADE_ASSET;
        private const string HOMING_MISSILE_POWERUP_ASSET = Config.POWERUP_HOMING_MISSILE_ASSET;
        private const string JETPACK_POWERUP_ASSET = Config.POWERUP_JETPACK_ASSET;

        private const string EXPLOSION_ASSET = Config.EXPLOSION_ASSET;
        private const string EXPLOSION_SMOKE_ASSET = Config.SMOKE_ASSET;
        
        private const string LEVEL_1_MOVER_ASSET = Config.LEVEL_1_MOVER_ASSET;
        private const string LEVEL_2_MOVER_ASSET = Config.LEVEL_2_MOVER_ASSET;
        private const string LEVEL_3_MOVER_ASSET = Config.LEVEL_3_MOVER_ASSET;
        private const string LEVEL_3_TALL_MOVER_ASSET = Config.LEVEL_3_TALL_MOVER_ASSET;
        private const string LEVEL_3_LONG_MOVER_ASSET = Config.LEVEL_3_LONG_MOVER_ASSET;
        private const string LEVEL_3_FINAL_MOVER_ASSET = Config.LEVEL_3_FINAL_MOVER_ASSET;

        private const string LEVEL_1_TILE_EXPLOSION_ASSET = Config.LEVEL_1_TILE_EXPLOSION_ASSET;
        private const string LEVEL_2_TILE_EXPLOSION_ASSET = Config.LEVEL_2_TILE_EXPLOSION_ASSET;
        private const string LEVEL_3_TILE_EXPLOSION_ASSET = Config.LEVEL_3_TILE_EXPLOSION_ASSET;

        private const string LEVEL_END_FLAG_ASSET = Config.LEVEL_END_FLAG_ASSET;
        private const string FAN_ASSET = Config.FAN_ASSET;
        
        private const string NULL_SPRITESHEET_ASSET = Config.NULL_SPRITESHEET_ASSET;

        public const int PLAYER_EXPLOSION_TILES_X = 7;
        public const int PLAYER_EXPLOSION_TILES_Y = 2;
        public const int EASY_PAWN_EXPLOSION_TILES_X = 5;
        public const int EASY_PAWN_EXPLOSION_TILES_Y = 2;
        public const int AERIAL_PAWN_EXPLOSION_TILES_X = 4;
        public const int AERIAL_PAWN_EXPLOSION_TILES_Y = 3;
        public const int GRENADIER_EXPLOSION_TILES_X = 4;
        public const int GRENADIER_EXPLOSION_TILES_Y = 3;
        public const int CRAWLER_EXPLOSION_TILES_X = 2;
        public const int CRAWLER_EXPLOSION_TILES_Y = 4;
        public const int AERIAL_BOMBER_EXPLOSION_TILES_X = 5;
        public const int AERIAL_BOMBER_EXPLOSION_TILES_Y = 2;
        public const int WASP_EXPLOSION_TILES_X = 4;
        public const int WASP_EXPLOSION_TILES_Y = 3;
        public const int LEVEL_EXPLOSION_TILES_X = 4;
        public const int LEVEL_EXPLOSION_TILES_Y = 4;
        public const int TURRET_EXPLOSION_TILES_X = 4;
        public const int TURRET_EXPLOSION_TILES_Y = 3;


        private const int SPRITESHEETS_LENGTH = 87;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static SpriteSheet[] spriteSheets = new SpriteSheet[SPRITESHEETS_LENGTH];
        
        private static RobotGameContentManager content;
        private static GraphicsDevice graphics;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private SpriteSheetFactory() { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static void Init(RobotGameContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            content = contentManager;
            graphics = graphicsDevice;
        }

        public static void PreLoadSprites()
        {
            for (int i = 0; i < SPRITESHEETS_LENGTH; i++)
            {
                GetSpriteSheet((SpriteKey)i);
            }
        }

        public static void UnloadSprites()
        {
            for (int i = 0; i < spriteSheets.Length; i++)
            {
                if (spriteSheets[i] != null)
                {
                    spriteSheets[i].Dispose();
                    spriteSheets[i] = null;
                }
            }
        }
        
        public static SpriteSheet GetSpriteSheet(SpriteKey key)
        {
            if (spriteSheets[(int)key] == null)
            {
                // Player/NPC sprites
                if (key == SpriteKey.Player)
                {
                    spriteSheets[(int)key] = new SpriteSheet(PLAYER_ASSET, 5, 1, 4, 200, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.PlayerJetpack)
                {
                    spriteSheets[(int)key] = new SpriteSheet(PLAYER_JETPACK_ASSET, 5, 1, 4, 200, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.PlayerJetpackFlame)
                {
                    spriteSheets[(int)key] = new SpriteSheet(PLAYER_JETPACK_FLAME, 3, 1, 2, 100, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Nozzle)
                {
                    spriteSheets[(int)key] = new SpriteSheet(NOZZLE_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.JetpackExhaust)
                {
                    spriteSheets[(int)key] = new SpriteSheet(JETPACK_EXHAUST_ASSET, 3, 2, 6, 100, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.EasyPawn)
                {
                    spriteSheets[(int)key] = new SpriteSheet(EASY_PAWN_ASSET, 5, 1, 4, 200, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.AerialPawn)
                {
                    spriteSheets[(int)key] = new SpriteSheet(AERIAL_PAWN_ASSET, 4, 2, 7, 100, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.Grenadier)
                {
                    spriteSheets[(int)key] = new SpriteSheet(GRENADIER_ASSET, 5, 1, 4, 200, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.HardGrenadier)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HARD_GRENADIER_ASSET, 5, 1, 4, 200, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.CrawlerUp)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CRAWLER_UP_ASSET, 5, 1, 4, 50, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.CrawlerDown)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CRAWLER_DOWN_ASSET, 5, 1, 4, 50, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.CrawlerLeft)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CRAWLER_LEFT_ASSET, 1, 5, 4, 50, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.CrawlerRight)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CRAWLER_RIGHT_ASSET, 1, 5, 4, 50, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.AerialBomber)
                {
                    spriteSheets[(int)key] = new SpriteSheet(AERIAL_BOMBER_ASSET, 2, 1, 2, 100, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.HardPawn)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HARD_PAWN_ASSET, 5, 1, 4, 200, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.Wasp)
                {
                    spriteSheets[(int)key] = new SpriteSheet(WASP_ASSET, 3, 3, 8, 15, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.StationaryTurretUp)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_STATIONARY_UP_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.StationaryTurretDown)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_STATIONARY_DOWN_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.StationaryTurretLeft)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_STATIONARY_LEFT_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.StationaryTurretRight)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_STATIONARY_RIGHT_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.TrackingTurretInvincibleDown)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TRACKING_TURRET_INVINCIBLE_DOWN_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.TrackingTurretInvincibleUp)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TRACKING_TURRET_INVINCIBLE_UP_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.TrackingTurretInvincibleLeft)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TRACKING_TURRET_INVINCIBLE_LEFT_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.TrackingTurretInvincibleRight)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TRACKING_TURRET_INVINCIBLE_RIGHT_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.StationaryTurretInvincibleUp)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_STATIONARY_INVINCIBLE_UP_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.StationaryTurretInvincibleDown)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_STATIONARY_INVINCIBLE_DOWN_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.StationaryTurretInvincibleLeft)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_STATIONARY_INVINCIBLE_LEFT_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.StationaryTurretInvincibleRight)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_STATIONARY_INVINCIBLE_RIGHT_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.MovingTurretUp)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_MOVING_UP_ASSET, 5, 1, 4, 200, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.MovingTurretDown)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_MOVING_DOWN_ASSET, 5, 1, 4, 200, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.MovingTurretLeft)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_MOVING_LEFT_ASSET, 1, 5, 4, 200, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.MovingTurretRight)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_MOVING_RIGHT_ASSET, 1, 5, 4, 200, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.TurretLaserNozzle)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_LASER_NOZZLE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.TurretInvincibleLaserNozzle)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_INVINCIBLE_LASER_NOZZLE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                //else if (key == SpriteKey.TurretProjectileNozzle)
                //{
                //    spriteSheets[(int)key] = new SpriteSheet(TURRET_PROJECTILE_NOZZLE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                //}
                else if (key == SpriteKey.PlayerExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(PLAYER_EXPLOSION_ASSET, PLAYER_EXPLOSION_TILES_X, PLAYER_EXPLOSION_TILES_Y, 14, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.EasyPawnExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(EASY_PAWN_EXPLOSION_ASSET, EASY_PAWN_EXPLOSION_TILES_X, EASY_PAWN_EXPLOSION_TILES_Y, 16, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.GrenadierExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(GRENADIER_EXPLOSION_ASSET, GRENADIER_EXPLOSION_TILES_X, GRENADIER_EXPLOSION_TILES_Y, 12, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.HardGrenadierExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HARD_GRENADIER_EXPLOSION_ASSET, GRENADIER_EXPLOSION_TILES_X, GRENADIER_EXPLOSION_TILES_Y, 12, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.HardPawnExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HARD_PAWN_EXPLOSION_ASSET, EASY_PAWN_EXPLOSION_TILES_X, EASY_PAWN_EXPLOSION_TILES_Y, 12, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.CrawlerExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CRAWLER_EXPLOSION_ASSET, CRAWLER_EXPLOSION_TILES_X, CRAWLER_EXPLOSION_TILES_Y, 12, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.AerialPawnExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(AERIAL_PAWN_EXPLOSION_ASSET, AERIAL_PAWN_EXPLOSION_TILES_X, AERIAL_PAWN_EXPLOSION_TILES_Y, 12, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.AerialBomberExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(AERIAL_BOMBER_EXPLOSION_ASSET, AERIAL_BOMBER_EXPLOSION_TILES_X, AERIAL_BOMBER_EXPLOSION_TILES_Y, 8, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.WaspExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(WASP_EXPLOSION_ASSET, WASP_EXPLOSION_TILES_X, WASP_EXPLOSION_TILES_Y, 12, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.TurretExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TURRET_EXPLOSION_ASSET, TURRET_EXPLOSION_TILES_X, TURRET_EXPLOSION_TILES_Y, 6, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.CoreExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CORE_EXPLOSION_ASSET, 5, 5, 25, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.CoreShieldExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CORE_SHIELD_EXPLOSION_ASSET, 2, 2, 4, 1, SpriteDamageMode.None, content, graphics);
                }
                // Projectile sprites
                else if (key == SpriteKey.Bullet)
                {
                    spriteSheets[(int)key] = new SpriteSheet(BULLET_PROJECTILE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Grenade)
                {
                    spriteSheets[(int)key] = new SpriteSheet(GRENADE_PROJECTILE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.CrawlerBombProjectile)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CRAWLER_BOMB_PROJECTILE, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.CrawlerSpiralProjectile)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CRAWLER_SPIRAL_PROJECTILE, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.HomingMissile)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HOMING_MISSILE_PROJECTILE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.HomingMissileSeek)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HOMING_MISSILE_PROJECTILE_SEEK_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.TargetArrow)
                {
                    spriteSheets[(int)key] = new SpriteSheet(TARGET_ARROW_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.BulletCollisionParticle)
                {
                    spriteSheets[(int)key] = new SpriteSheet(BULLET_PROJECTILE_COLLISION_PARTICLE_ASSET, 4, 1, 4, 50d, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.EnemyBulletCollisionParticle)
                {
                    spriteSheets[(int)key] = new SpriteSheet(ENEMY_BULLET_PROJECTILE_COLLISION_PARTICLE_ASSET, 4, 1, 4, 50d, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.EasyPawnProjectile)
                {
                    spriteSheets[(int)key] = new SpriteSheet(EASY_PAWN_PROJECTILE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.AerialPawnProjectile)
                {
                    spriteSheets[(int)key] = new SpriteSheet(AERIAL_PAWN_PROJECTILE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.AerialPawnProjectileParticle)
                {
                    spriteSheets[(int)key] = new SpriteSheet(AERIAL_PAWN_PROJECTILE_PARTICLE, 4, 1, 4, 100d, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.GrenadierProjectile)
                {
                    spriteSheets[(int)key] = new SpriteSheet(GRENADIER_PROJECTILE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.AerialBomberProjectile)
                {
                    spriteSheets[(int)key] = new SpriteSheet(AERIAL_BOMBER_PROJECTILE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.HardPawnProjectile)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HARD_PAWN_PROJECTILE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.LaserLine)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LASER_LINE_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.LaserGlow)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LASER_GLOW_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.CoreShield)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CORE_SHIELD_ASSET, 1, 1, 1, 1, SpriteDamageMode.DamageTexture, content, graphics);
                }
                else if (key == SpriteKey.Core)
                {
                    spriteSheets[(int)key] = new SpriteSheet(CORE_ASSET, 4, 1, 4, 250, SpriteDamageMode.DamageTexture, content, graphics);
                }
                // Powerup sprites
                else if (key == SpriteKey.HealthPowerup)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HEALTH_POWERUP_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.LifePowerup)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LIFE_POWERUP_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.DoubleDamagePowerup)
                {
                    spriteSheets[(int)key] = new SpriteSheet(DOUBLE_DAMAGE_POWERUP_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.AmmoPowerup)
                {
                    spriteSheets[(int)key] = new SpriteSheet(AMMO_POWERUP_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.GrenadePowerup)
                {
                    spriteSheets[(int)key] = new SpriteSheet(GRENADE_POWERUP_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.HomingMissilePowerup)
                {
                    spriteSheets[(int)key] = new SpriteSheet(HOMING_MISSILE_POWERUP_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.JetpackPowerup)
                {
                    spriteSheets[(int)key] = new SpriteSheet(JETPACK_POWERUP_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                // Sprite effects
                else if (key == SpriteKey.Explosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(EXPLOSION_ASSET, 4, 3, 9, 25d, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.ExplosionSmoke)
                {
                    spriteSheets[(int)key] = new SpriteSheet(EXPLOSION_SMOKE_ASSET, 3, 2, 6, 150d, SpriteDamageMode.None, content, graphics);
                }
                // Level stuff
                else if (key == SpriteKey.Level1Mover)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_1_MOVER_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Level2Mover)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_2_MOVER_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Level3Mover)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_3_MOVER_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Level3TallMover)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_3_TALL_MOVER_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Level3FinalMover)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_3_FINAL_MOVER_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Level3LongMover)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_3_LONG_MOVER_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Level1TileExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_1_TILE_EXPLOSION_ASSET, 4, 4, 16, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Level2TileExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_2_TILE_EXPLOSION_ASSET, 4, 4, 16, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Level3TileExplosion)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_3_TILE_EXPLOSION_ASSET, 4, 4, 16, 1, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.LevelEndFlag)
                {
                    spriteSheets[(int)key] = new SpriteSheet(LEVEL_END_FLAG_ASSET, 6, 1, 5, 150, SpriteDamageMode.None, content, graphics);
                }
                else if (key == SpriteKey.Fan)
                {
                    spriteSheets[(int)key] = new SpriteSheet(FAN_ASSET, 1, 5, 5, 50, SpriteDamageMode.None, content, graphics);
                }
                // Transparent Sprite
                else if (key == SpriteKey.None)
                {
                    spriteSheets[(int)key] = new SpriteSheet(NULL_SPRITESHEET_ASSET, 1, 1, 1, 1, SpriteDamageMode.None, content, graphics);
                }
            }
            return spriteSheets[(int)key];
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
