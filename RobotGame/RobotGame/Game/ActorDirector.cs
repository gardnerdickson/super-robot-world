using System;
using System.Collections.Generic;
using RobotGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TiledLib;
using RobotGame.Exceptions;
using RobotGame.Game.Enemy;
using RobotGame.Game.Powerup;
using RobotGame.Game.Volume;
using RobotGame.Game.Weapon;
using RobotGame.Game.Mover;
using System.Globalization;

namespace RobotGame.Game
{
    public class ActorDirector
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const string SPAWN_POINT_OBJECT_TYPE = Config.OBJECT_TYPE_PROPERTY;

        private const string SPAWN_DELAY = Config.SPAWN_DELAY;

        private const string POWERUP_TYPE_PROPERTY = Config.ENEMY_POWERUP_SPAWN_TYPE;
        private const string POWERUP_PHYSICS_MODE_PROPERTY = Config.ENEMY_POWERUP_SPAWN_PHYSICS_MODE;
        private const string INITIAL_HORIZONTAL_DIRECTION = Config.INITIAL_HORIZONTAL_DIRECTION;

        private const string POSITION_CORRECTION_MODE = Config.POSITION_CORRECTION_MODE;

        private const string WAYPOINT_A_PROPERTY = Config.WAYPOINT_A_PROPERTY;
        private const string WAYPOINT_B_PROPERTY = Config.WAYPOINT_B_PROPERTY;
        private const string WAYPOINT_ORIENTATION = Config.WAYPOINT_ORIENTATION_PROPERTY;

        private const string ENEMY_SPAWN_BOUNDARY_PROPERTY = Config.ENEMY_SPAWN_BOUNDARY_PROPERTY;
        private const int DEFAULT_ENEMY_OFFSCREEN_SPAWN_BOUNDARY = Config.ENEMY_OFFSCREEN_SPAWN_THRESHOLD;

        private const string INTERVAL_VOLUME_PROPERTY = Config.INTERVAL_PROPERTY;
        private const string AMOUNT_VOLUME_PROPERTY = Config.AMOUNT_PROPERTY;
        private const string FORCE_X_VOLUME_PROPERTY = Config.FORCE_X_PROPERTY;
        private const string FORCE_Y_VOLUME_PROPERTY = Config.FORCE_Y_PROPERTY;
        private const string ENEMY_SPAWN_POINT_LIST_PROPERTY = Config.ENEMY_SPAWN_POINT_LIST_PROPERTY;
        private const string ENEMY_SPAWN_REF_PROPERTY = Config.ENEMY_SPAWN_REF_PROPERTY;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback SpawnEnemyCallback;

        private static ActorDirector instance = null;

        private static Random random = new Random();

        private int screenWidth;
        private int screenHeight;
        
        private List<SpawnPoint> enemySpawnPoints;
        private Dictionary<string, bool> enemySpawnFlags;

        private List<SpawnPoint> particleEmitterSpawnPoints;
        private Dictionary<string, bool> particleEmitterSpawnFlags;
        
        private List<SpawnPoint> powerupSpawnPoints;
        private Dictionary<string, bool> powerupSpawnFlags;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors
        
        private ActorDirector() { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static ActorDirector GetInstance()
        {
            if (instance == null)
            {
                instance = new ActorDirector();
            }
            return instance;
        }

        public void Init(int screenWidth, int screenHeight)
        {
            this.SpawnEnemyCallback = new TimerCallback(spawn_enemy_callback);

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            this.enemySpawnPoints = Level.GetInstance().GetMapObjectSpawnPointsByType(Level.ENEMY_SPAWN_MAP_OBJECT_TYPE);
            this.enemySpawnFlags = new Dictionary<string, bool>();
            foreach (SpawnPoint spawnPoint in this.enemySpawnPoints)
            {
                this.enemySpawnFlags.Add(spawnPoint.Name, false);
            }

            this.particleEmitterSpawnPoints = Level.GetInstance().GetMapObjectSpawnPointsByType(Level.PARTICLE_EMITTER_SPAWN_MAP_OBJECT_TYPE);
            this.particleEmitterSpawnFlags = new Dictionary<string, bool>();
            foreach (SpawnPoint spawnPoint in this.particleEmitterSpawnPoints)
            {
                this.particleEmitterSpawnFlags.Add(spawnPoint.Name, false);
            }


            this.powerupSpawnPoints = Level.GetInstance().GetMapObjectSpawnPointsByType(Level.POWERUP_SPAWN_MAP_OBJECT_TYPE);
            this.powerupSpawnFlags = new Dictionary<string, bool>();
            foreach (SpawnPoint spawnPoint in this.powerupSpawnPoints)
            {
                this.powerupSpawnFlags.Add(spawnPoint.Name, false);
            }

            // Spawn movers
            List<SpawnPoint> moverSpawnPoints = Level.GetInstance().GetMapObjectSpawnPointsByType(Level.MOVER_SPAWN_MAP_OBJECT_TYPE);
            foreach (SpawnPoint spawnPoint in moverSpawnPoints)
            {
                CreateMover(spawnPoint);
            }

            // Spawn environment actors
            List<SpawnPoint> environmentActorSpawnPoints = Level.GetInstance().GetMapObjectSpawnPointsByType(Level.ENVIRONMENT_ACTOR_SPAWN_MAP_OBJECT_TYPE);
            foreach (SpawnPoint spawnPoint in environmentActorSpawnPoints)
            {
                CreateEnvironmentActor(spawnPoint);
            }
        }

        public bool IsEnemySpawned(string name)
        {
            return this.enemySpawnFlags[name];
        }

        public void Update(Vector2 cameraPosition)
        {
            Rectangle spawnBoundary = new Rectangle((int)cameraPosition.X - DEFAULT_ENEMY_OFFSCREEN_SPAWN_BOUNDARY,
                                                    (int)cameraPosition.Y - DEFAULT_ENEMY_OFFSCREEN_SPAWN_BOUNDARY,
                                                    this.screenWidth + DEFAULT_ENEMY_OFFSCREEN_SPAWN_BOUNDARY * 2,
                                                    this.screenHeight + DEFAULT_ENEMY_OFFSCREEN_SPAWN_BOUNDARY * 2);
            
            // Spawn particle emitters
            foreach (SpawnPoint spawnPoint in this.particleEmitterSpawnPoints)
            {
                if (spawnPoint.Position.X >= spawnBoundary.Left && spawnPoint.Position.X <= spawnBoundary.Right &&
                    spawnPoint.Position.Y >= spawnBoundary.Top && spawnPoint.Position.Y <= spawnBoundary.Bottom)
                {
                    if (!this.particleEmitterSpawnFlags[spawnPoint.Name])
                    {
                        CreateParticleEmitter(spawnPoint);
                        this.particleEmitterSpawnFlags[spawnPoint.Name] = true;
                    }
                }
            }

            // Spawn powerups
            foreach (SpawnPoint spawnPoint in this.powerupSpawnPoints)
            {
                if (spawnPoint.Position.X >= spawnBoundary.Left && spawnPoint.Position.X <= spawnBoundary.Right &&
                    spawnPoint.Position.Y >= spawnBoundary.Top && spawnPoint.Position.Y <= spawnBoundary.Bottom)
                {
                    if (!this.powerupSpawnFlags[spawnPoint.Name])
                    {
                        string powerupType = spawnPoint.Properties[Level.OBJECT_TYPE_PROPERTY].RawValue;
                        Property physicsModeProperty = spawnPoint.Properties[POWERUP_PHYSICS_MODE_PROPERTY];
                        PhysicsMode physicsMode;
                        if (physicsModeProperty != null && physicsModeProperty.RawValue == Level.PHYSICS_MODE_GRAVITY)
                        {
                            physicsMode = PhysicsMode.Gravity;
                        }
                        else
                        {
                            physicsMode = PhysicsMode.None;
                        }

                        SpawnPowerup(powerupType, physicsMode, spawnPoint.Position, Vector2.Zero);
                        this.powerupSpawnFlags[spawnPoint.Name] = true;
                    }
                }
            }


            // Calculate projectile boundary
            Rectangle projectileBoundary = new Rectangle((int)cameraPosition.X - this.screenWidth / 2, (int)cameraPosition.Y - this.screenHeight,
                                                         this.screenWidth * 2, (int)(this.screenHeight * 3f));

            // Destory projectiles that have gone too far off screen
            foreach (Projectile projectile in Projectile.PlayerProjectileList)
            {
                if (!(projectileBoundary.Intersects(projectile.Bounds)))
                {
                    projectile.Remove();
                }
            }
            foreach (Projectile projectile in Projectile.EnemyProjectileList)
            {
                if (!(projectileBoundary.Intersects(projectile.Bounds)))
                {
                    projectile.Remove();
                }
            }

            // Remove any actors that have been marked as dead
            RemoveDeadActors();
        }

        public void SpawnPowerup(string powerupType, PhysicsMode physicsMode, Vector2 position, Vector2 velocity)
        {
            Player player = (Player)Player.PlayerList[0];
            switch (powerupType)
            {
                case Level.POWERUP_AMMO:

                    if (player.SecondaryWeaponInventory.GetState(WeaponInventory.GRENADE_LAUNCHER_INDEX) == InventoryState.Unavailable &&
                        player.SecondaryWeaponInventory.GetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX) == InventoryState.Unavailable)
                    {
                        new PowerupGrenade(position, velocity, physicsMode);
                        break;
                    }

                    new PowerupAmmo(position, velocity, physicsMode);
                    break;

                case Level.POWERUP_GRENADE:

                    if (player.SecondaryWeaponInventory.GetState(WeaponInventory.GRENADE_LAUNCHER_INDEX) != InventoryState.Unavailable)
                    {
                        new PowerupAmmo(position, velocity, physicsMode);
                    }
                    else
                    {
                        new PowerupGrenade(position, velocity, physicsMode);
                    }
                    break;

                case Level.POWERUP_HOMING_MISSILE:

                    if (player.SecondaryWeaponInventory.GetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX) != InventoryState.Unavailable)
                    {
                        new PowerupAmmo(position, velocity, physicsMode);
                    }
                    else
                    {
                        new PowerupHomingMissile(position, velocity, physicsMode);
                    }
                    break;

                case Level.POWERUP_HEALTH:

                    new PowerupHealth(position, velocity, physicsMode);
                    break;

                case Level.POWERUP_LIFE:

                    if (RobotGame.GameMode == GameMode.NinteenEightyFive)
                    {
                        new PowerupLife(position, velocity, physicsMode);
                    }
                    else
                    {
                        if (random.Next(10) % 2 == 0)
                        {
                            new PowerupAmmo(position, velocity, physicsMode);
                        }
                        else
                        {
                            new PowerupHealth(position, velocity, physicsMode);
                        }
                    }

                    break;

                case Level.POWERUP_DOUBLE_DAMAGE:

                    new PowerupDoubleDamage(position, velocity, physicsMode);
                    break;

                case Level.POWERUP_JETPACK:

                    new PowerupJetPack(position, velocity, physicsMode);
                    break;

                default:
                    string message = String.Format("Invalid powerup type: {0}", powerupType);
                    this.LogError(message, typeof(ActorDirector));
                    throw new FatalLevelException(message); 
            }
        }

        public void SpawnMapVolumes()
        {
            List<VolumeInfo> mapVolumes = Level.GetInstance().GetVolumes();
            foreach (VolumeInfo volumeInfo in mapVolumes)
            {
                if (volumeInfo.Type == Level.LEVEL_END_VOLUME_TYPE)
                {
                    new LevelEndVolume(volumeInfo.Bounds);
                }
                else if (volumeInfo.Type == Level.DEATH_VOLUME_TYPE)
                {
                    new DeathVolume(volumeInfo.Bounds);
                }
                else if (volumeInfo.Type == Level.HEALTH_VOLUME_TYPE)
                {
                    int amount = int.Parse(volumeInfo.Properties[AMOUNT_VOLUME_PROPERTY].RawValue);
                    float interval = float.Parse(volumeInfo.Properties[INTERVAL_VOLUME_PROPERTY].RawValue, CultureInfo.InvariantCulture);

                    new HealthVolume(volumeInfo.Bounds, amount, interval);
                }
                else if (volumeInfo.Type == Level.DAMAGE_VOLUME_TYPE)
                {
                    int amount = int.Parse(volumeInfo.Properties[AMOUNT_VOLUME_PROPERTY].RawValue);

                    new DamageVolume(volumeInfo.Bounds, amount);
                }
                else if (volumeInfo.Type == Level.FORCE_VOLUME_TYPE)
                {
                    float forceX = float.Parse(volumeInfo.Properties[FORCE_X_VOLUME_PROPERTY].RawValue, CultureInfo.InvariantCulture);
                    float forceY = float.Parse(volumeInfo.Properties[FORCE_Y_VOLUME_PROPERTY].RawValue, CultureInfo.InvariantCulture);

                    new ForceVolume(volumeInfo.Bounds, new Vector2(forceX, forceY));
                }
                else if (volumeInfo.Type == Level.ENEMY_SPAWN_VOLUME_TYPE)
                {
                    string[] enemyNameArray;

                    Property enemySpawnRef = volumeInfo.Properties[ENEMY_SPAWN_REF_PROPERTY];
                    if (enemySpawnRef != null)
                    {
                        List<string> enemyNameList = new List<string>();
                        string[] enemySpawnRefNames = enemySpawnRef.RawValue.Split(',');
                        foreach (string enemySpawnRefName in enemySpawnRefNames)
                        {
                            VolumeInfo spawnRefVolume = Level.GetInstance().GetVolumeByNameAndType(enemySpawnRefName, Level.ENEMY_SPAWN_VOLUME_TYPE);
                            string[] enemySpawnNames = spawnRefVolume.Properties[ENEMY_SPAWN_POINT_LIST_PROPERTY].RawValue.Split(',');
                            foreach (string enemyName in enemySpawnNames)
                            {
                                enemyNameList.Add(enemyName);
                            }
                        }

                        enemyNameArray = enemyNameList.ToArray();
                    }
                    else
                    {
                        enemyNameArray = volumeInfo.Properties[ENEMY_SPAWN_POINT_LIST_PROPERTY].RawValue.Split(',');       
                    }

                    List<SpawnPoint> enemySpawnPoints = Level.GetInstance().GetMapObjectSpawnPointsByTypeAndName(Level.ENEMY_SPAWN_MAP_OBJECT_TYPE, enemyNameArray);
                    
                    new EnemySpawnVolume(volumeInfo.Bounds, enemySpawnPoints);
                }
                else if (volumeInfo.Type == Level.TIMED_ENEMY_SPAWN_VOLUME_TYPE)
                {
                    float startDelay = float.Parse(volumeInfo.Properties[Level.START_DELAY_PROPERTY].RawValue, CultureInfo.InvariantCulture);
                    float interval = float.Parse(volumeInfo.Properties[Level.INTERVAL_PROPERTY].RawValue, CultureInfo.InvariantCulture);
                    string spawnPointTemplate = volumeInfo.Properties[Level.SPAWN_POINT_TEMPLATE].RawValue;

                    List<SpawnPoint> spawnPoints = Level.GetInstance().GetMapObjectSpawnPointsByTypeAndName(Level.ENEMY_SPAWN_MAP_OBJECT_TYPE, spawnPointTemplate);

                    new TimedEnemySpawnVolume(volumeInfo.Bounds, spawnPoints[0], startDelay, interval);
                }
                else if (volumeInfo.Type == Level.TRIGGER_VOLUME_TYPE)
                {
                    TriggerKey triggerKey = (TriggerKey)Enum.Parse(typeof(TriggerKey), volumeInfo.Properties[Level.TRIGGER_KEY_PROPERTY].RawValue, false);

                    new EventTriggerVolume(volumeInfo.Bounds, triggerKey);
                }
                else if (volumeInfo.Type == Level.END_GAME_EXPLOSION_VOLUME_TYPE)
                {
                    Property startDelayProperty = volumeInfo.Properties[Level.START_DELAY_PROPERTY];
                    float startDelay = 0f;
                    if (startDelayProperty != null)
                    {
                        startDelay = float.Parse(startDelayProperty.RawValue, CultureInfo.InvariantCulture);
                    }

                    new EndGameExplosionVolume(volumeInfo.Bounds, startDelay);
                }
                else if (volumeInfo.Type == Level.CHECKPOINT_VOLUME_TYPE)
                {
                    string checkpointSpawnName = volumeInfo.Properties["checkpoint_spawn"].RawValue;
                    int priority = int.Parse(volumeInfo.Properties["priority"].RawValue);
                    SpawnPoint checkpointSpawn = Level.GetInstance().GetMapObjectSpawnPointsByTypeAndName(Level.CHECKPOINT_SPAWN_MAP_OBJECT_TYPE, checkpointSpawnName)[0];

                    new CheckpointVolume(volumeInfo.Bounds, checkpointSpawn.Position, priority);
                }
                else if (volumeInfo.Type == Level.GAME_END_VOLUME_TYPE)
                {
                    new GameEndVolume(volumeInfo.Bounds);
                }
            }
        }

        public void SpawnEnemy(SpawnPoint spawnPoint)
        {
            SpawnEnemy(spawnPoint, false);
        }

        public void SpawnEnemy(SpawnPoint spawnPoint, bool ignoreSpawnFlags)
        {
            if (ignoreSpawnFlags || !this.enemySpawnFlags[spawnPoint.Name])
            {
                Property spawnDelay = spawnPoint.Properties[SPAWN_DELAY];
                if (spawnDelay != null)
                {
                    spawnPoint.Properties.Remove(SPAWN_DELAY);
                    TimerManager.GetInstance().RegisterTimer(float.Parse(spawnDelay.RawValue, CultureInfo.InvariantCulture), this.SpawnEnemyCallback, spawnPoint);
                    return;
                }

                this.enemySpawnFlags[spawnPoint.Name] = true;

                Property positionCorrectionProperty = spawnPoint.Properties[POSITION_CORRECTION_MODE];
                Property powerupTypeProperty = spawnPoint.Properties[POWERUP_TYPE_PROPERTY];
                Property powerupPhysicsModeProperty = spawnPoint.Properties[POWERUP_PHYSICS_MODE_PROPERTY];
                Property enemyObjectTypeProperty = spawnPoint.Properties[SPAWN_POINT_OBJECT_TYPE];

                PhysicsMode powerupPhysicsMode = PhysicsMode.None;
                string powerupType = null;

                string enemySpawnType = enemyObjectTypeProperty.RawValue;

                GetEnemyOnDeathPowerupInfo(powerupTypeProperty, powerupPhysicsModeProperty, ref powerupType, ref powerupPhysicsMode);

                PositionCorrectionMode positionCorrectionMode;
                if (positionCorrectionProperty != null)
                {
                    positionCorrectionMode = (PositionCorrectionMode)Enum.Parse(typeof(PositionCorrectionMode), positionCorrectionProperty.RawValue, false);
                }
                else
                {
                    positionCorrectionMode = PositionCorrectionMode.None;
                }

                Vector2 spawnPosition = spawnPoint.Position;
                if (enemySpawnType == Config.EASY_PAWN)
                {
                    spawnPosition = CorrectPosition(spawnPoint.Position, SpriteSheetFactory.GetSpriteSheet(SpriteKey.EasyPawn), positionCorrectionMode);
                    int direction = int.Parse(spawnPoint.Properties[INITIAL_HORIZONTAL_DIRECTION].RawValue);

                    new EasyPawn(spawnPosition, spawnPoint.Name, direction, powerupType, powerupPhysicsMode);
                }
                else if (enemySpawnType == Config.GRENADIER)
                {
                    spawnPosition = CorrectPosition(spawnPoint.Position, SpriteSheetFactory.GetSpriteSheet(SpriteKey.Grenadier), positionCorrectionMode);

                    MapWaypoint[] waypoints = Level.GetInstance().GetWaypoints(spawnPoint);

                    Property leftSideWeaponEnabledProperty = spawnPoint.Properties[Level.LEFT_WEAPON_ENABLED];
                    Property rightSideWeaponEnabledProperty = spawnPoint.Properties[Level.RIGHT_WEAPON_ENABLED];

                    bool leftSideWeaponEnabled = true;
                    if (leftSideWeaponEnabledProperty != null)
                    {
                        leftSideWeaponEnabled = bool.Parse(leftSideWeaponEnabledProperty.RawValue);
                    }

                    bool rightSideWeaponEnabled = true;
                    if (rightSideWeaponEnabledProperty != null)
                    {
                        rightSideWeaponEnabled = bool.Parse(rightSideWeaponEnabledProperty.RawValue);
                    }

                    GrenadierDifficulty difficulty = GrenadierDifficulty.Normal;
                    Property difficultyProperty = spawnPoint.Properties[Level.DIFFICULTY];
                    if (difficultyProperty != null)
                    {
                        difficulty = (GrenadierDifficulty)Enum.Parse(typeof(GrenadierDifficulty), difficultyProperty.RawValue, false);
                    }

                    new Grenadier(spawnPosition, spawnPoint.Name, powerupType, powerupPhysicsMode, waypoints, leftSideWeaponEnabled, rightSideWeaponEnabled, difficulty);
                }
                else if (enemySpawnType == Config.CRAWLER)
                {
                    MapWaypoint[] waypoints = Level.GetInstance().GetWaypoints(spawnPoint);
                    EnemyOrientation orientation = (EnemyOrientation)Enum.Parse(typeof(EnemyOrientation), spawnPoint.Properties[Level.ENEMY_ORIENTATION].RawValue, false);
                    CrawlerFireMode fireMode = (CrawlerFireMode)Enum.Parse(typeof(CrawlerFireMode), spawnPoint.Properties[Level.CRAWLER_FIRE_MODE].RawValue, false);

                    SpriteKey spriteKey;
                    if (orientation == EnemyOrientation.Up)
                    {
                        spriteKey = SpriteKey.CrawlerUp;
                    }
                    else if (orientation == EnemyOrientation.Down)
                    {
                        spriteKey = SpriteKey.CrawlerDown;
                    }
                    else if (orientation == EnemyOrientation.Left)
                    {
                        spriteKey = SpriteKey.CrawlerLeft;
                    }
                    else
                    {
                        spriteKey = SpriteKey.CrawlerRight;
                    }

                    spawnPosition = CorrectPosition(spawnPoint.Position, SpriteSheetFactory.GetSpriteSheet(spriteKey), positionCorrectionMode);

                    float speed = Config.CRAWLER_SPEED;
                    Property speedProperty = spawnPoint.Properties[Level.SPEED];
                    if (speedProperty != null)
                    {
                        speed = float.Parse(speedProperty.RawValue, CultureInfo.InvariantCulture);
                    }
                    
                    new Crawler(spawnPosition, spawnPoint.Name, speed, powerupType, powerupPhysicsMode, waypoints, fireMode, orientation);
                }
                else if (enemySpawnType == Config.AERIAL_BOMBER)
                {
                    new AerialBomber(spawnPosition, spawnPoint.Name, powerupType, powerupPhysicsMode);
                }
                else if (enemySpawnType == Config.AERIAL_PAWN)
                {
                    int direction = int.Parse(spawnPoint.Properties[INITIAL_HORIZONTAL_DIRECTION].RawValue);
                    new AerialPawn(spawnPoint.Position, spawnPoint.Name, direction, powerupType, powerupPhysicsMode);
                }
                else if (enemySpawnType == Config.HARD_PAWN)
                {
                    spawnPosition = CorrectPosition(spawnPoint.Position, SpriteSheetFactory.GetSpriteSheet(SpriteKey.HardPawn), positionCorrectionMode);
                    int direction = int.Parse(spawnPoint.Properties[INITIAL_HORIZONTAL_DIRECTION].RawValue);

                    new HardPawn(spawnPosition, spawnPoint.Name, direction, powerupType, powerupPhysicsMode);
                }
                else if (enemySpawnType == Config.WASP)
                {
                    new Wasp(spawnPosition, spawnPoint.Name, powerupType, powerupPhysicsMode);
                }
                else if (enemySpawnType == Config.SIMPLE_TURRET)
                {
                    MapWaypoint[] waypoints = null;
                    if (spawnPoint.Properties[Level.WAYPOINT_LIST_PROPERTY] != null)
                    {
                        waypoints = Level.GetInstance().GetWaypoints(spawnPoint);
                    }

                    float fireDuration = float.Parse(spawnPoint.Properties[Level.FIRE_DURATION_PROPERTY].RawValue, CultureInfo.InvariantCulture);
                    float fireDelay = float.Parse(spawnPoint.Properties[Level.FIRE_DELAY_PROPERTY].RawValue, CultureInfo.InvariantCulture);
                    EnemyOrientation orientation = (EnemyOrientation)Enum.Parse(typeof(EnemyOrientation), spawnPoint.Properties[Level.ENEMY_ORIENTATION].RawValue, false);
                    SpriteKey bodySpriteKey = (SpriteKey)Enum.Parse(typeof(SpriteKey), spawnPoint.Properties[Level.TURRET_BODY_SPRITE_PROPERTY].RawValue, false);
                    SpriteKey nozzleSpriteKey = (SpriteKey)Enum.Parse(typeof(SpriteKey), spawnPoint.Properties[Level.TURRET_NOZZLE_SPRITE_PROPERTY].RawValue, false);
                    spawnPosition = CorrectPosition(spawnPoint.Position, SpriteSheetFactory.GetSpriteSheet(bodySpriteKey), positionCorrectionMode);

                    float speed = 0;
                    Property speedProperty = spawnPoint.Properties[Level.SPEED];
                    if (speedProperty != null)
                    {
                        speed = float.Parse(speedProperty.RawValue, CultureInfo.InvariantCulture);
                    }

                    bool invincible = false;
                    Property invincibleProperty = spawnPoint.Properties[Level.INVINCIBLE_PROPERTY];
                    if (invincibleProperty != null)
                    {
                        invincible = bool.Parse(invincibleProperty.RawValue);
                    }

                    bool collideWithMovers = true;
                    Property collideWithMoversProperty = spawnPoint.Properties[Level.COLLIDE_WITH_MOVERS];
                    if (collideWithMoversProperty != null)
                    {
                        collideWithMovers = bool.Parse(collideWithMoversProperty.RawValue);
                    }

                    new SimpleTurret(spawnPosition, orientation, speed, spawnPoint.Name, powerupType, powerupPhysicsMode, waypoints, fireDuration, fireDelay, invincible, collideWithMovers, bodySpriteKey, nozzleSpriteKey);
                }
                else if (enemySpawnType == Config.TRACKING_TURRET)
                {
                    MapWaypoint[] waypoints = null;
                    if (spawnPoint.Properties[Level.WAYPOINT_LIST_PROPERTY] != null)
                    {
                        waypoints = Level.GetInstance().GetWaypoints(spawnPoint);
                    }

                    bool invincible = false;
                    Property invincibleProperty = spawnPoint.Properties[Level.INVINCIBLE_PROPERTY];
                    if (invincibleProperty != null)
                    {
                        invincible = bool.Parse(invincibleProperty.RawValue);
                    }

                    float speed = 0f;
                    Property speedProperty = spawnPoint.Properties[Level.SPEED];
                    if (speedProperty != null)
                    {
                        speed = float.Parse(speedProperty.RawValue, CultureInfo.InvariantCulture);
                    }

                    EnemyOrientation orientation = (EnemyOrientation)Enum.Parse(typeof(EnemyOrientation), spawnPoint.Properties[Level.ENEMY_ORIENTATION].RawValue, false);
                    SpriteKey bodySpriteKey = (SpriteKey)Enum.Parse(typeof(SpriteKey), spawnPoint.Properties[Level.TURRET_BODY_SPRITE_PROPERTY].RawValue, false);
                    SpriteKey nozzleSpriteKey = (SpriteKey)Enum.Parse(typeof(SpriteKey), spawnPoint.Properties[Level.TURRET_NOZZLE_SPRITE_PROPERTY].RawValue, false);
                    spawnPosition = CorrectPosition(spawnPoint.Position, SpriteSheetFactory.GetSpriteSheet(bodySpriteKey), positionCorrectionMode);

                    new TrackingTurret(spawnPosition, orientation, speed, spawnPoint.Name, powerupType, invincible, powerupPhysicsMode, waypoints, bodySpriteKey, nozzleSpriteKey);
                }
                else if (enemySpawnType == Config.CORE)
                {
                    new Core(spawnPoint.Position, spawnPoint.Name);
                }
                else if (enemySpawnType == Config.CORE_SHIELD)
                {
                    Property coreRefProperty = spawnPoint.Properties[Level.CORE_REF];
                    Property rotationArmLengthProperty = spawnPoint.Properties[Level.ROTATION_ARM_LENGTH];
                    Property startRotationProperty = spawnPoint.Properties[Level.ROTATION];
                    Property rotationIncrementProperty = spawnPoint.Properties[Level.ROTATION_INCREMENT];
                    
                    Vector2 rotationArmPosition = Level.GetInstance().GetMapObjectSpawnPointsByTypeAndName(Level.ENEMY_SPAWN_MAP_OBJECT_TYPE, coreRefProperty.RawValue)[0].Position;
                    float rotationArmLength = float.Parse(rotationArmLengthProperty.RawValue, CultureInfo.InvariantCulture);
                    float startRotation = float.Parse(startRotationProperty.RawValue, CultureInfo.InvariantCulture);
                    float rotationIncrement = float.Parse(rotationIncrementProperty.RawValue, CultureInfo.InvariantCulture);

                    new CoreShield(spawnPoint.Name, rotationArmPosition, rotationArmLength, startRotation, rotationIncrement);
                }
                else if (enemySpawnType == Config.WAYPOINT)
                {
                    // Do nothing. Just avoiding exception.
                }
                else
                {
                    string message = String.Format("{0} is not a valid enemy type.", spawnPoint.Type);
                    this.LogError(message, typeof(ActorDirector));
                    throw new FatalLevelException(message);
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void CreateParticleEmitter(SpawnPoint spawnPoint)
        {
            Property particleSpriteKeyProperty = spawnPoint.Properties[Level.PARTICLE_SPRITE_KEY];
            Property particleSpawnFrequencyProperty = spawnPoint.Properties[Level.PARTICLE_SPAWN_FREQUENCY_PROPERTY];
            Property particleVelocityXProperty = spawnPoint.Properties[Level.PARTICLE_VELOCITY_X_PROPERTY];
            Property particleVelocityYProperty = spawnPoint.Properties[Level.PARTICLE_VELOCITY_Y_PROPERTY];
            Property particleAccelerationXProperty = spawnPoint.Properties[Level.PARTICLE_ACCELERATION_X_PROPERTY];
            Property particleAccelerationYProperty = spawnPoint.Properties[Level.PARTICLE_ACCELERATION_Y_PROPERTY];
            Property particleMaxVelocityXProperty = spawnPoint.Properties[Level.PARTICLE_MAX_VELOCITY_X_PROPERTY];
            Property particleMaxVelocityYProperty = spawnPoint.Properties[Level.PARTICLE_MAX_VELOCITY_Y_PROPERTY];
            Property particleRotationSpeedProperty = spawnPoint.Properties[Level.PARTICLE_ROTATION_SPEED];
            Property particleFadeStartProperty = spawnPoint.Properties[Level.PARTICLE_FADE_START];
            Property particleFadeEndProperty = spawnPoint.Properties[Level.PARTICLE_FADE_END];
            Property particleScaleStartProperty = spawnPoint.Properties[Level.PARTICLE_SCALE_START];
            Property particleScaleEndProperty = spawnPoint.Properties[Level.PARTICLE_SCALE_END];

            Vector2 position = spawnPoint.Position;

            // Optional properties
            float particleAccelerationX = 0.0f;
            float particleAccelerationY = 0.0f;
            if (particleAccelerationXProperty != null)
            {
                particleAccelerationX = float.Parse(particleAccelerationXProperty.RawValue, CultureInfo.InvariantCulture);
            }
            if (particleAccelerationYProperty != null)
            {
                particleAccelerationY = float.Parse(particleAccelerationYProperty.RawValue, CultureInfo.InvariantCulture);
            }
            Vector2 particleAcceleration = new Vector2(particleAccelerationX, particleAccelerationY);

            float particleRotationSpeed = 0.0f;
            if (particleRotationSpeedProperty != null)
            {
                particleRotationSpeed = float.Parse(particleRotationSpeedProperty.RawValue, CultureInfo.InvariantCulture);
            }
            
            int particleFadeStart = 255;
            if (particleFadeStartProperty != null)
            {
                particleFadeStart = int.Parse(particleFadeStartProperty.RawValue);
            }
            int particleFadeEnd = 0;
            if (particleFadeEndProperty != null)
            {
                particleFadeEnd = int.Parse(particleFadeEndProperty.RawValue);
            }

            float particleScaleStart = 1.0f;
            if (particleScaleStartProperty != null)
            {
                particleScaleStart = float.Parse(particleScaleStartProperty.RawValue, CultureInfo.InvariantCulture);
            }

            float particleScaleEnd = 1.0f;
            if (particleScaleEndProperty != null)
            {
                particleScaleEnd = float.Parse(particleScaleEndProperty.RawValue, CultureInfo.InvariantCulture);
            }
            
            SpriteKey particleSpriteKey = (SpriteKey)Enum.Parse(typeof(SpriteKey), particleSpriteKeyProperty.RawValue, false);
            float particleSpawnFrequency = float.Parse(particleSpawnFrequencyProperty.RawValue, CultureInfo.InvariantCulture);
            Vector2 particleVelocity = new Vector2(float.Parse(particleVelocityXProperty.RawValue, CultureInfo.InvariantCulture),
                                                   float.Parse(particleVelocityYProperty.RawValue, CultureInfo.InvariantCulture));

            float particleMaxVelocityX = float.Parse(particleMaxVelocityXProperty.RawValue, CultureInfo.InvariantCulture);
            float particleMaxVelocityY = float.Parse(particleMaxVelocityYProperty.RawValue, CultureInfo.InvariantCulture);
           
            ParticleEmitter particleEmitter = new ParticleEmitter(particleSpawnFrequency, position, particleVelocity, particleMaxVelocityX, particleMaxVelocityY, particleAcceleration,
                                                                  particleSpriteKey, particleRotationSpeed, particleFadeStart, particleFadeEnd, particleScaleStart, particleScaleEnd);
            particleEmitter.Start();
        }

        private void CreateMover(SpawnPoint spawnPoint)
        {
            string objectType = spawnPoint.Properties[Level.OBJECT_TYPE_PROPERTY].RawValue;
            if (objectType == Level.BASIC_MOVER || objectType == Level.SUSPENDED_MOVER)
            {
                float maxVelocity = float.Parse(spawnPoint.Properties[Level.MAX_VELOCITY_PROPERTY].RawValue, CultureInfo.InvariantCulture);
                float acceleration = float.Parse(spawnPoint.Properties[Level.ACCELERATION_PROPERTY].RawValue, CultureInfo.InvariantCulture);
                float stopTime = float.Parse(spawnPoint.Properties[Level.STOP_TIME].RawValue, CultureInfo.InvariantCulture);

                MapWaypoint[] waypoints = Level.GetInstance().GetWaypoints(spawnPoint);
                SpriteKey spriteKey = (SpriteKey)Enum.Parse(typeof(SpriteKey), spawnPoint.Properties[Level.MOVER_SPRITE_KEY_PROPERTY].RawValue, false);

                if (objectType == Level.BASIC_MOVER)
                {
                    string iterationMode = spawnPoint.Properties[Level.WAYPOINT_ITERATOR].RawValue;
                    new BasicMover(spawnPoint.Position, spriteKey, acceleration, maxVelocity, stopTime, iterationMode, waypoints);
                }
                else
                {
                    bool offScreenReset = false;
                    Property offScreenResetProperty = spawnPoint.Properties[Level.OFF_SCREEN_RESET];
                    if (offScreenResetProperty != null)
                    {
                        offScreenReset = bool.Parse(offScreenResetProperty.RawValue);
                    }

                    new SuspendedMover(spawnPoint.Position, spriteKey, acceleration, maxVelocity, stopTime, offScreenReset, waypoints);
                }
            }
        }

        private void CreateEnvironmentActor(SpawnPoint spawnPoint)
        {
            SpriteKey spriteKey = (SpriteKey)Enum.Parse(typeof(SpriteKey), spawnPoint.Properties[Level.ENVIRONEMNT_ACTOR_SPRITE_KEY_PROPERTY].RawValue, false);

            Vector2 spawnPosition = spawnPoint.Position;

            Property positionCorrectionProperty = spawnPoint.Properties[POSITION_CORRECTION_MODE];
            PositionCorrectionMode positionCorrection;
            if (positionCorrectionProperty != null)
            {
                positionCorrection = (PositionCorrectionMode)Enum.Parse(typeof(PositionCorrectionMode), positionCorrectionProperty.RawValue, false);
            }
            else
            {
                positionCorrection = PositionCorrectionMode.None;
            }

            float rotation = 0f;
            Property rotationProperty = spawnPoint.Properties[Level.ROTATION];
            if (rotationProperty != null)
            {
                rotation = float.Parse(rotationProperty.RawValue, CultureInfo.InvariantCulture);
            }

            spawnPosition = CorrectPosition(spawnPoint.Position, SpriteSheetFactory.GetSpriteSheet(spriteKey), positionCorrection);

            new EnvironmentActor(spawnPosition, spriteKey, rotation);
        }

        private void GetEnemyOnDeathPowerupInfo(Property powerupTypeProperty, Property powerupPhysicsModeProperty, ref string powerupType, ref PhysicsMode powerupPhysicsMode)
        {
            if (powerupTypeProperty != null && powerupPhysicsModeProperty != null)
            {
                powerupType = powerupTypeProperty.RawValue;

                if (powerupPhysicsModeProperty.RawValue == Level.PHYSICS_MODE_NONE)
                {
                    powerupPhysicsMode = PhysicsMode.None;
                }
                else if (powerupPhysicsModeProperty.RawValue == Level.PHYSICS_MODE_GRAVITY)
                {
                    powerupPhysicsMode = PhysicsMode.Gravity;
                }
                else
                {
                    string message = "Trying to spawn enemy with spawn powerup but no PhysicsMode for powerup";
                    this.LogError(message, typeof(ActorDirector));
                    throw new FatalLevelException(message);
                }
            }
        }

        private Vector2 CorrectPosition(Vector2 spawnPosition, SpriteSheet spriteSheet, PositionCorrectionMode positionCorrectionMode)
        {
            if (positionCorrectionMode == PositionCorrectionMode.None)
            {
                return spawnPosition;
            }

            Vector2 correctedPosition = Vector2.Zero;
            Point tile = Level.GetInstance().GetTile(spawnPosition.X, spawnPosition.Y);
            while (correctedPosition == Vector2.Zero)
            {
                if (Level.GetInstance().IsCollisionTile(tile.X, tile.Y) || Level.GetInstance().IsDestructibleTile(tile.X, tile.Y))
                {
                    if (positionCorrectionMode == PositionCorrectionMode.Up)
                    {
                        correctedPosition = new Vector2(spawnPosition.X, (tile.Y * Level.GetInstance().TileHeight) + spriteSheet.TileHeight);
                    }
                    else if (positionCorrectionMode == PositionCorrectionMode.Down)
                    {
                        correctedPosition = new Vector2(spawnPosition.X, (tile.Y * Level.GetInstance().TileHeight) - (spriteSheet.TileHeight / 2));
                    }
                    else if (positionCorrectionMode == PositionCorrectionMode.Left)
                    {
                        correctedPosition = new Vector2((tile.X * Level.GetInstance().TileWidth) + spriteSheet.TileWidth, spawnPosition.Y);
                    }
                    else if (positionCorrectionMode == PositionCorrectionMode.Right)
                    {
                        correctedPosition = new Vector2((tile.X * Level.GetInstance().TileWidth) - (spriteSheet.TileWidth / 2), spawnPosition.Y);
                    }
                }
                else
                {
                    if (positionCorrectionMode == PositionCorrectionMode.Up)
                    {
                        tile.Y--;
                    }
                    else if (positionCorrectionMode == PositionCorrectionMode.Down)
                    {
                        tile.Y++;
                    }
                    else if (positionCorrectionMode == PositionCorrectionMode.Left)
                    {
                        tile.X--;
                    }
                    else if (positionCorrectionMode == PositionCorrectionMode.Right)
                    {
                        tile.X++;
                    }
                }
            }

            return correctedPosition;
        }

        private void RemoveDeadActors()
        {
            // TODO: are there other lists of actors that need be removed?

            for (int i = Actor.ActorList.Count - 1; i >= 0; i--)
            {
                if (Actor.ActorList[i].Dead)
                {
                    Actor.ActorList.RemoveAt(i);
                }
            }

            for (int i = AbstractEnemy.EnemyList.Count - 1; i >= 0; i--)
            {
                if (AbstractEnemy.EnemyList[i].Dead)
                {
                    AbstractEnemy.EnemyList.RemoveAt(i);
                }
            }

            for (int i = Projectile.PlayerProjectileList.Count - 1; i >= 0; i--)
            {
                if (Projectile.PlayerProjectileList[i].Dead)
                {
                    Projectile.PlayerProjectileList.RemoveAt(i);
                }
            }

            for (int i = Projectile.EnemyProjectileList.Count - 1; i >= 0; i--)
            {
                if (Projectile.EnemyProjectileList[i].Dead)
                {
                    Projectile.EnemyProjectileList.RemoveAt(i);
                }
            }

            for (int i = AbstractPowerup.PowerupList.Count - 1; i >= 0; i--)
            {
                if (AbstractPowerup.PowerupList[i].Dead)
                {
                    AbstractPowerup.PowerupList.RemoveAt(i);
                }
            }

            for (int i = BasicMover.MoverList.Count - 1; i >= 0; i--)
            {
                if (BasicMover.MoverList[i].Dead)
                {
                    BasicMover.MoverList.RemoveAt(i);
                }
            }

            for (int i = TrackingLaser.LaserList.Count - 1; i >= 0; i--)
            {
                if (TrackingLaser.LaserList[i].Dead)
                {
                    TrackingLaser.LaserList.RemoveAt(i);
                }
            }
        }

        private void LogError(string message, Type clazz)
        {
#if WINDOWS
            Logging.LogError(message, clazz, Environment.StackTrace);
#endif
        }

        private void LogWarning(string message, Type clazz)
        {
#if WINDOWS
            Logging.LogWarning(message, clazz, Environment.StackTrace);
#endif
        }

        private void LogDebug(string message, Type clazz)
        {
#if WINDOWS
            Logging.LogDebug(message, clazz, Environment.StackTrace);
#endif
        }

        private void spawn_enemy_callback(Object param)
        {
            this.SpawnEnemy((SpawnPoint)param);
        }

    } // end of class
}
