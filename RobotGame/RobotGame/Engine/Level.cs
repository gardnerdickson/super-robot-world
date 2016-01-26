using System;
using TiledLib;
using Microsoft.Xna.Framework.Content;
using RobotGame.Exceptions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RobotGame.Game;
using RobotGame.Game.Audio;

namespace RobotGame.Engine
{
    public enum PositionCorrectionMode
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public struct LevelInfo
    {
        public string MapAsset;
        public LevelSongInfo SongInfo;
        public BackgroundFactory[] Backgrounds;
        public SpriteKey TileExplosionSprite;

        public LevelInfo(string mapAsset, LevelSongInfo songKey, SpriteKey tileExplosionSprite, BackgroundFactory[] backgrounds)
        {
            this.MapAsset = mapAsset;
            this.SongInfo = songKey;
            this.Backgrounds = backgrounds;
            this.TileExplosionSprite = tileExplosionSprite;
        }
    }

    public struct SpawnPoint
    {
        public string Name; // unique identifier
        public string Type;
        public Vector2 Position;
        public PropertyCollection Properties;

        public SpawnPoint(string name, string type, Vector2 position, PropertyCollection properties)
        {
            this.Name = name;
            this.Type = type;
            this.Position = position;
            this.Properties = properties;
        }
    }

    struct WaypointInfo
    {
        public float WaypointA;
        public float WaypointB;
        public WaypointOrientation Orientation;

        public WaypointInfo(float waypointA, float waypointB, WaypointOrientation orientation)
        {
            this.WaypointA = waypointA;
            this.WaypointB = waypointB;
            this.Orientation = orientation;
        }
    }

    struct VolumeInfo
    {
        public Rectangle Bounds;
        public string Type;
        public PropertyCollection Properties;

        public VolumeInfo(Rectangle bounds, string type, PropertyCollection properties)
        {
            this.Bounds = bounds;
            this.Type = type;
            this.Properties = properties;
        }
    }

    struct CameraBoundsInfo
    {
        public Rectangle Bounds;
        public bool Transition;
        public int OffsetX;
        public int OffsetY;
        public int OffsetPriority;

        public CameraBoundsInfo(Rectangle bounds, bool transition, int offsetX, int offsetY, int offsetPriority)
        {
            this.Bounds = bounds;
            this.Transition = transition;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.OffsetPriority = offsetPriority;
        }
    }
    
    class Level
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        public static LevelInfo[] LEVEL_ASSETS = Config.LEVEL_ASSETS;

        public const string OBJECT_TYPE_PROPERTY = Config.OBJECT_TYPE_PROPERTY;

        public const string DIFFICULTY = Config.DIFFICULTY;

        public const string BASIC_MOVER = Config.BASIC_MOVER;
        public const string SUSPENDED_MOVER = Config.SUSPENDED_MOVER;
        public const string MOVER_SPRITE_KEY_PROPERTY = Config.MOVER_SPRITE_KEY;
        public const string WAYPOINT_PROPERTY = Config.WAYPOINT;
        public const string MAX_VELOCITY_PROPERTY = Config.MAX_VELOCITY;
        public const string ACCELERATION_PROPERTY = Config.ACCELERATION_PROPERTY;
        public const string STOP_TIME = Config.STOP_TIME;
        public const string WAYPOINT_ITERATOR = Config.WAYPOINT_ITERATOR;
        public const string CIRCULAR = Config.CIRCULAR;
        public const string FORWARD_AND_BACKWARD = Config.FORWARD_AND_BACKWARD;
        public const string OFF_SCREEN_RESET = Config.OFF_SCREEN_RESET;

        public const string ENVIRONEMNT_ACTOR_SPRITE_KEY_PROPERTY = Config.ENVIRONMENT_ACTOR_SPRITE_KEY;

        public const string PLAYER_SPAWN_MAP_OBJECT_TYPE = Config.PLAYER_SPAWN_MAP_TYPE;
        public const string CHECKPOINT_SPAWN_MAP_OBJECT_TYPE = Config.CHECKPOINT_SPAWN_MAP_TYPE;
        public const string ENEMY_SPAWN_MAP_OBJECT_TYPE = Config.ENEMY_SPAWN_MAP_TYPE;
        public const string POWERUP_SPAWN_MAP_OBJECT_TYPE = Config.POWERUP_SPAWN_MAP_TYPE;
        public const string PARTICLE_EMITTER_SPAWN_MAP_OBJECT_TYPE = Config.PARTICLE_EMITTER_SPAWN_MAP_TYPE;
        public const string MOVER_SPAWN_MAP_OBJECT_TYPE = Config.MOVER_SPAWN_MAP_TYPE;
        public const string ENVIRONMENT_ACTOR_SPAWN_MAP_OBJECT_TYPE = Config.ENVIRONMENT_ACTOR_SPAWN_MAP_TYPE;

        public const string PARTICLE_SPRITE_KEY = Config.PARTICLE_SPRITE_KEY;
        public const string PARTICLE_SPAWN_FREQUENCY_PROPERTY = Config.PARTICLE_SPAWN_FREQUENCY_PROPERTY;
        public const string PARTICLE_VELOCITY_X_PROPERTY = Config.PARTICLE_VELOCITY_X_PROPERTY;
        public const string PARTICLE_VELOCITY_Y_PROPERTY = Config.PARTICLE_VELOCITY_Y_PROPERTY;
        public const string PARTICLE_ACCELERATION_X_PROPERTY = Config.PARTICLE_ACCELERATION_X_PROPERTY;
        public const string PARTICLE_ACCELERATION_Y_PROPERTY = Config.PARTICLE_ACCELERATION_Y_PROPERTY;
        public const string PARTICLE_MAX_VELOCITY_X_PROPERTY = Config.PARTICLE_MAX_VELOCITY_X_PROPERTY;
        public const string PARTICLE_MAX_VELOCITY_Y_PROPERTY = Config.PARTICLE_MAX_VELOCITY_Y_PROPERTY;
        public const string PARTICLE_ROTATION_SPEED = Config.PARTICLE_ROTATION_SPEED;
        public const string PARTICLE_FADE_START = Config.PARTICLE_FADE_START;
        public const string PARTICLE_FADE_END = Config.PARTICLE_FADE_END;
        public const string PARTICLE_SCALE_START = Config.PARTICLE_SCALE_START;
        public const string PARTICLE_SCALE_END = Config.PARTICLE_SCALE_END;

        public const string VOLUME_LAYER = Config.VOLUME_LAYER;
        public const string CAMERA_BOUNDS_LAYER = Config.CAMERA_BOUNDS_LAYER;
        public const string COLLISION_LAYER = Config.COLLISION_LAYER;
        public const string DESTRUCTIBLE_LAYER = Config.DESTRUCTIBLE_LAYER;
        public const string OBJECTS_LAYER = Config.OBJECTS_LAYER;
        public const string FOREGROUND_LAYER = Config.FOREGROUND_LAYER;
        public const string BACKGROUND_LAYER = Config.BACKGROUND_LAYER;

        private const string CAMERA_BOUNDS_TRANSITION = Config.CAMERA_BOUNDS_TRANSITION;
        private const string CAMERA_BOUNDS_OFFSET_X = Config.CAMERA_BOUNDS_OFFSET_X;
        private const string CAMERA_BOUNDS_OFFSET_Y = Config.CAMERA_BOUNDS_OFFSET_Y;
        private const string CAMERA_BOUNDS_OFFSET_PRIORITY = Config.CAMERA_BOUNDS_OFFSET_PRIORITY;

        public const string LEVEL_END_VOLUME_TYPE = Config.LEVEL_END_VOLUME_TYPE;
        public const string DEATH_VOLUME_TYPE = Config.DEATH_VOLUME_TYPE;
        public const string HEALTH_VOLUME_TYPE = Config.HEALTH_VOLUME_TYPE;
        public const string DAMAGE_VOLUME_TYPE = Config.DAMAGE_VOLUME_TYPE;
        public const string FORCE_VOLUME_TYPE = Config.FORCE_VOLUME_TYPE;
        public const string ENEMY_SPAWN_VOLUME_TYPE = Config.ENEMY_SPAWN_VOLUME_TYPE;
        public const string CHECKPOINT_VOLUME_TYPE = Config.CHECKPOINT_VOLUME_TYPE;
        public const string TRIGGER_VOLUME_TYPE = Config.TRIGGER_VOLUME_TYPE;
        public const string END_GAME_EXPLOSION_VOLUME_TYPE = Config.END_GAME_EXPLOSION_VOLUME_TYPE;
        public const string START_DELAY_PROPERTY = Config.START_DELAY_PROPERTY;
        public const string GAME_END_VOLUME_TYPE = Config.GAME_END_VOLUME_TYPE;
        public const string TIMED_ENEMY_SPAWN_VOLUME_TYPE = Config.TIMED_ENEMY_SPAWN_VOLUME_TYPE;

        public const string INTERVAL_PROPERTY = Config.INTERVAL_PROPERTY;
        public const string SPAWN_POINT_TEMPLATE = Config.SPAWN_POINT_TEMPLATE;

        public const float MAP_FOREGROUND_DRAW_DEPTH = Config.MAP_FOREGROUND_DRAW_DEPTH;
        public const float MAP_BACKGROUND_DRAW_DEPTH = Config.MAP_BACKGROUND_DRAW_DEPTH;
        public const float LEVEL_BACKGROUND_DRAW_DEPTH = Config.LEVEL_BACKGROUND_DRAW_DEPTH;

        public const string TRIGGER_KEY_PROPERTY = Config.TRIGGER_KEY_PROPERTY;

        public const string LEFT_WEAPON_ENABLED = Config.LEFT_WEAPON_ENABLED;
        public const string RIGHT_WEAPON_ENABLED = Config.RIGHT_WEAPON_ENABLED;

        public const string WAYPOINT_LIST_PROPERTY = Config.WAYPOINT_LIST_PROPERTY;
        public const string WAYPOINT_A_PROPERTY = Config.WAYPOINT_A_PROPERTY;
        public const string WAYPOINT_B_PROPERTY = Config.WAYPOINT_B_PROPERTY;
        public const string WAYPOINT_ORIENTATION = Config.WAYPOINT_ORIENTATION_PROPERTY;
        public const string WAYPOINT_ORIENTATION_HORIZONTAL = Config.WAYPOINT_ORIENTATION_HORIZONTAL;
        public const string WAYPOINT_ORIENTATION_VERTICAL = Config.WAYPOINT_ORIENTATION_VERTICAL;
        
        public const string PHYSICS_MODE_NONE = Config.PHYSICS_MODE_NONE;
        public const string PHYSICS_MODE_GRAVITY = Config.PHYSICS_MODE_GRAVITY;

        public const string CORE_REF = Config.CORE_REF;
        public const string ROTATION_ARM_LENGTH = Config.ROTATION_ARM_LENGTH;
        public const string ROTATION_INCREMENT = Config.ROTATION_INCREMENT;

        public const string POWERUP_AMMO = Config.POWERUP_AMMO;
        public const string POWERUP_LIFE = Config.POWERUP_LIFE;
        public const string POWERUP_HEALTH = Config.POWERUP_HEALTH;
        public const string POWERUP_GRENADE = Config.POWERUP_GRENADE;
        public const string POWERUP_HOMING_MISSILE = Config.POWERUP_HOMING_MISSILE;
        public const string POWERUP_DOUBLE_DAMAGE = Config.POWERUP_DOUBLE_DAMAGE;
        public const string POWERUP_JETPACK = Config.POWERUP_JETPACK;

        public const string SPEED = Config.SPEED;
        public const string ENEMY_ORIENTATION = Config.ENEMY_ORIENTATION;
        public const string CRAWLER_FIRE_MODE = Config.CRAWLER_FIRE_MODE;

        public const string EASY_PAWN = Config.EASY_PAWN;
        public const string GRENADIER = Config.GRENADIER;
        public const string AERIAL_BOMBER = Config.AERIAL_BOMBER;
        public const string AERIAL_PAWN = Config.AERIAL_PAWN;
        public const string HARD_PAWN = Config.HARD_PAWN;
        public const string WASP = Config.WASP;
        public const string CRAWLER = Config.CRAWLER;
        public const string SIMPLE_TURRET = Config.SIMPLE_TURRET;
        public const string TRACKING_TURRET = Config.TRACKING_TURRET;
        public const string CORE = Config.CORE;
        public const string CORE_SHIELD = Config.CORE_SHIELD;
        public const string WAYPOINT_TYPE = Config.WAYPOINT_TYPE;

        public const string TURRET_BODY_SPRITE_PROPERTY = Config.TURRET_BODY_SPRITE_PROPERTY;
        public const string TURRET_NOZZLE_SPRITE_PROPERTY = Config.TURRET_NOZZLE_SPRITE_PROPERTY;
        public const string FIRE_DURATION_PROPERTY = Config.FIRE_DURATION_PROPERTY;
        public const string FIRE_DELAY_PROPERTY = Config.FIRE_DELAY_PROPERTY;
        public const string INVINCIBLE_PROPERTY = Config.INVINCIBLE_PROPERTY;
        public const string COLLIDE_WITH_MOVERS = Config.COLLIDE_WITH_MOVERS;

        public const string ROTATION = Config.ROTATION;

        private int DEFAULT_CAMERA_OFFSET_X = Config.CAMERA_SCREEN_X_OFFSET;
        private int DEFAULT_CAMERA_OFFSET_Y = Config.CAMERA_SCREEN_Y_OFFSET;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static Level instance = null;

        private Map map;
        private List<Background> backgrounds;
        private int levelNumber;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        public int Width
        {
            get { return this.map.Width * this.map.TileWidth; }
        }

        public int Height
        {
            get { return this.map.Height * this.map.TileHeight; }
        }

        public int TilesX
        {
            get { return this.map.Width; }
        }

        public int TilesY
        {
            get { return this.map.Height; }
        }

        public int TileWidth
        {
            get { return this.map.TileWidth; }
        }

        public int TileHeight
        {
            get { return this.map.TileHeight; }
        }

        public LevelInfo LevelInfo
        {
            get { return LEVEL_ASSETS[this.levelNumber]; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private Level() { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static Level GetInstance()
        {
            if (instance == null)
            {
                instance = new Level();
            }
            return instance;
        }

        public void Load(RobotGameContentManager contentManager, int levelNumber)
        {
            this.levelNumber = levelNumber;
            this.map = contentManager.Load<Map>(LEVEL_ASSETS[this.levelNumber].MapAsset, false);
            this.backgrounds = new List<Background>();
            foreach (BackgroundFactory factory in LEVEL_ASSETS[this.levelNumber].Backgrounds)
            {
                Background background = factory.CreateInstance();
                background.LoadTexture(contentManager);

                this.backgrounds.Add(background);
            }
            
            Validate(this.map);

            // Make everything but the foreground, background, and destructible layer invisible
            this.map.GetLayer(VOLUME_LAYER).Visible = false;
            this.map.GetLayer(CAMERA_BOUNDS_LAYER).Visible = false;
            this.map.GetLayer(COLLISION_LAYER).Visible = false;
            this.map.GetLayer(OBJECTS_LAYER).Visible = false;

            this.map.GetLayer(FOREGROUND_LAYER).LayerDepth = MAP_FOREGROUND_DRAW_DEPTH;
            this.map.GetLayer(DESTRUCTIBLE_LAYER).LayerDepth = MAP_FOREGROUND_DRAW_DEPTH;
            this.map.GetLayer(BACKGROUND_LAYER).LayerDepth = MAP_BACKGROUND_DRAW_DEPTH;
        }

        public Point GetTile(float xCoord, float yCoord)
        {
            int tileX = (int)Math.Ceiling(xCoord / this.TileWidth);
            int tileY = (int)Math.Ceiling(yCoord / this.TileHeight);

            return new Point(tileX, tileY);
        }

        public List<CameraBoundsInfo> GetCameraBounds(Vector2 position)
        {
            List<CameraBoundsInfo> cameraBoundsInfoList = new List<CameraBoundsInfo>();

            MapObjectLayer cameraBoundsLayer = this.map.GetLayer(CAMERA_BOUNDS_LAYER) as MapObjectLayer;

            int offsetX;
            int offsetY;
            bool transition;
            int offsetPriority;
            foreach (MapObject cameraBoundsObject in cameraBoundsLayer.Objects)
            {
                if ((int)position.Y > cameraBoundsObject.Bounds.Top &&
                    (int)position.Y < cameraBoundsObject.Bounds.Bottom &&
                    (int)position.X < cameraBoundsObject.Bounds.Right &&
                    (int)position.X > cameraBoundsObject.Bounds.Left)
                {
                    offsetX = DEFAULT_CAMERA_OFFSET_X;
                    offsetY = DEFAULT_CAMERA_OFFSET_Y;
                    transition = false;
                    offsetPriority = 99;

                    Property offsetXProperty = cameraBoundsObject.Properties[Level.CAMERA_BOUNDS_OFFSET_X];
                    Property offsetYProperty = cameraBoundsObject.Properties[Level.CAMERA_BOUNDS_OFFSET_Y];
                    Property transitionProperty = cameraBoundsObject.Properties[Level.CAMERA_BOUNDS_TRANSITION];
                    Property offsetPriorityProperty = cameraBoundsObject.Properties[Level.CAMERA_BOUNDS_OFFSET_PRIORITY];

                    if (offsetXProperty != null)
                    {
                        offsetX = int.Parse(offsetXProperty.RawValue);
                    }
                    if (offsetYProperty != null)
                    {
                        offsetY = int.Parse(offsetYProperty.RawValue);
                    }
                    if (transitionProperty != null)
                    {
                        transition = bool.Parse(transitionProperty.RawValue);
                    }
                    if (offsetPriorityProperty != null)
                    {
                        offsetPriority = int.Parse(offsetPriorityProperty.RawValue);
                    }

                    cameraBoundsInfoList.Add(new CameraBoundsInfo(cameraBoundsObject.Bounds, transition, offsetX, offsetY, offsetPriority));
                }
            }

            return cameraBoundsInfoList;
        }

        public void AddCameraBounds(string name, string type, Rectangle bounds, PropertyCollection properties)
        {
            MapObject cameraBounds = new MapObject(name, type, bounds, properties);

            MapObjectLayer cameraBoundsLayer = this.map.GetLayer(CAMERA_BOUNDS_LAYER) as MapObjectLayer;
            cameraBoundsLayer.AddObject(cameraBounds);
        }

        public List<VolumeInfo> GetVolumes()
        {
            MapObjectLayer volumeLayer = this.map.GetLayer(VOLUME_LAYER) as MapObjectLayer;

            List<VolumeInfo> volumeInfoList = new List<VolumeInfo>();
            foreach (MapObject volume in volumeLayer.Objects)
            {
                volumeInfoList.Add(new VolumeInfo(volume.Bounds, volume.Type, volume.Properties));
            }

            return volumeInfoList;
        }

        public VolumeInfo GetVolumeByNameAndType(string name, string type)
        {
            MapObjectLayer volumeLayer = this.map.GetLayer(VOLUME_LAYER) as MapObjectLayer;

            List<VolumeInfo> volumeInfoList = new List<VolumeInfo>();
            foreach (MapObject volume in volumeLayer.Objects)
            {
                if (volume.Name == name && volume.Type == type)
                {
                    return new VolumeInfo(volume.Bounds, volume.Type, volume.Properties);
                }
            }

            throw new LevelException(String.Format("Volume with name '{0}' and type '{1}' was not found.", name, type));
        }

        public void AddTile(string layer, int x, int y, Texture2D texture)
        {
            TileLayer tileLayer = this.map.GetLayer(layer) as TileLayer;
            tileLayer.Tiles[x, y] = new Tile(texture, Rectangle.Empty);
        }

        public void RemoveTile(string layer, int x, int y)
        {
            TileLayer tileLayer = this.map.GetLayer(layer) as TileLayer;
            tileLayer.Tiles[x, y] = null;
        }

        public Rectangle GetTileBounds(int x, int y)
        {
            return new Rectangle(x * this.map.TileWidth, y * this.map.TileHeight, this.map.TileWidth, this.map.TileHeight);
        }

        public bool IsCollisionTile(int x, int y)
        {
            return CheckTileType(x, y, COLLISION_LAYER);
        }

        public bool IsDestructibleTile(int x, int y)
        {
            return CheckTileType(x, y, DESTRUCTIBLE_LAYER);
        }
        
        public bool IsForegroundTile(int x, int y)
        {
            return CheckTileType(x, y, FOREGROUND_LAYER);
        }

        public bool DestroyTile(int x, int y)
        {
            TileLayer destructibleLayer = this.map.GetLayer(DESTRUCTIBLE_LAYER) as TileLayer;
            Tile tile = null;
            try
            {
                tile = destructibleLayer.Tiles[x, y];

                if (tile == null)
                {
                    return false;
                }

                // If no exception thrown, the tile is within map bounds so we can nullify it
                destructibleLayer.Tiles[x, y] = null;
                return true;
            }
            catch (IndexOutOfRangeException e)
            {
                throw new LevelException("Trying to reference a tile that is out of the map bounds.", e);
            }
        }

        public bool TileIsOnMap(int x, int y)
        {
            if (x > 0 && x < this.TilesX - 1 && y > 0 && y < this.TilesY - 1)
            {
                return true;
            }

            return false;
        }

        public bool IsBoundsOnMap(Rectangle bounds)
        {
            if (bounds.Left > 0 && bounds.Right < this.Width && bounds.Top > 0 && bounds.Bottom < this.Height)
            {
                return true;
            }

            return false;
        }

        public List<SpawnPoint> GetMapObjectSpawnPointsByType(string type)
        {
            if (type != PLAYER_SPAWN_MAP_OBJECT_TYPE && type != ENEMY_SPAWN_MAP_OBJECT_TYPE &&
                type != POWERUP_SPAWN_MAP_OBJECT_TYPE && type != PARTICLE_EMITTER_SPAWN_MAP_OBJECT_TYPE &&
                type != MOVER_SPAWN_MAP_OBJECT_TYPE && type != ENVIRONMENT_ACTOR_SPAWN_MAP_OBJECT_TYPE &&
                type != CHECKPOINT_SPAWN_MAP_OBJECT_TYPE)
            {
                String message = String.Format("Invalid map object type: {0}", type);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message);
            }

            List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

            MapObjectLayer objectsLayer = map.GetLayer(OBJECTS_LAYER) as MapObjectLayer;
            foreach (MapObject mapObject in objectsLayer.Objects)
            {
                if (mapObject.Type == type)
                {
                    spawnPoints.Add(new SpawnPoint(mapObject.Name, mapObject.Type, new Vector2(mapObject.Bounds.X, mapObject.Bounds.Y), mapObject.Properties));
                }
            }

            return spawnPoints;
        }

        public List<SpawnPoint> GetMapObjectSpawnPointsByTypeAndName(string type, params string[] names)
        {
            List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();
            List<SpawnPoint> spawnPoints = GetMapObjectSpawnPointsByType(type);
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                foreach (string name in names)
                {
                    if (name == spawnPoint.Name)
                    {
                        enemySpawnPoints.Add(spawnPoint);
                    }
                }
            }

            return enemySpawnPoints;
        }

        public WaypointInfo GetWaypointInfo(SpawnPoint spawnPoint)
        {
            string waypointNameA = spawnPoint.Properties[WAYPOINT_A_PROPERTY].RawValue;
            string waypointNameB = spawnPoint.Properties[WAYPOINT_B_PROPERTY].RawValue;
            string orientation = spawnPoint.Properties[WAYPOINT_ORIENTATION].RawValue;

            SpawnPoint waypointA = FindEnemySpawnPointByName(waypointNameA);
            SpawnPoint waypointB = FindEnemySpawnPointByName(waypointNameB);

            WaypointInfo waypointInfo = new WaypointInfo();
            if (orientation == WAYPOINT_ORIENTATION_HORIZONTAL)
            {
                waypointInfo.Orientation = WaypointOrientation.Horizontal;
                waypointInfo.WaypointA = waypointA.Position.X;
                waypointInfo.WaypointB = waypointB.Position.X;
            }
            else if (orientation == WAYPOINT_ORIENTATION_VERTICAL)
            {
                waypointInfo.Orientation = WaypointOrientation.Vertical;
                waypointInfo.WaypointA = waypointA.Position.Y;
                waypointInfo.WaypointB = waypointB.Position.Y;
            }
            else
            {
                String message = String.Format("Unrecognized waypoint orientation: {0}", orientation);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message);
            }

            return waypointInfo;
        }

        public MapWaypoint[] GetWaypoints(SpawnPoint spawnPoint)
        {
            string waypointListPropertyValue = spawnPoint.Properties[Level.WAYPOINT_LIST_PROPERTY].RawValue;
            string[] waypointNames = waypointListPropertyValue.Split(',');

            string orientation = spawnPoint.Properties[WAYPOINT_ORIENTATION].RawValue;
            
            if (waypointNames.Length < 2)
            {
                String message = String.Format("Actor '{0}' has less than 2 waypoints.", spawnPoint.Name);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message);
            }

            MapWaypoint[] waypoints = new MapWaypoint[waypointNames.Length];
            int waypointIndex = 0;

            MapObjectLayer objectLayer = this.map.GetLayer(OBJECTS_LAYER) as MapObjectLayer;
            foreach (string waypointName in waypointNames)
            {
                MapObject waypoint = objectLayer.GetObject(waypointName);
                waypoints[waypointIndex++] = new MapWaypoint(new Vector2(waypoint.Bounds.X, waypoint.Bounds.Y), (WaypointOrientation)Enum.Parse(typeof(WaypointOrientation), orientation, false));
            }

            return waypoints;
        }

        public void Update()
        {
            foreach (Background background in backgrounds)
            {
                background.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch, int cameraOffsetX, int cameraOffsetY, int screenWidth, int screenHeight)
        {
            if (this.map == null)
            {
                String message = "No instance of TiledLib.Map available. Use the Level.Reset method to instantiate the TiledLib.Map instance.";
                this.LogError(message, typeof(Level));
                throw new InvalidStateException(message);
            }

            float drawDepth = LEVEL_BACKGROUND_DRAW_DEPTH;
            foreach (Background background in backgrounds)
            {
                background.Draw(spriteBatch, cameraOffsetX, cameraOffsetY, drawDepth);
                drawDepth += 0.01f;
            }

            Rectangle worldArea = new Rectangle(cameraOffsetX, cameraOffsetY, screenWidth, screenHeight);
            this.map.Draw(spriteBatch, worldArea);
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private List<SpawnPoint> GetSpawnPoints(string mapLayer)
        {
            List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
            MapObjectLayer spawnLayer = map.GetLayer(mapLayer) as MapObjectLayer;
         
            foreach (MapObject spawnObject in spawnLayer.Objects)
            {
                spawnPoints.Add(new SpawnPoint(spawnObject.Name, spawnObject.Type, new Vector2(spawnObject.Bounds.X, spawnObject.Bounds.Y), spawnObject.Properties));
            }

            return spawnPoints;
        }

        private SpawnPoint FindEnemySpawnPointByName(string name)
        {
            List<SpawnPoint> spawnPoints = GetMapObjectSpawnPointsByType(Level.ENEMY_SPAWN_MAP_OBJECT_TYPE);
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if (spawnPoint.Name == name)
                {
                    return spawnPoint;
                }
            }

            String message = String.Format("SpawnPoint with name '{0}' not found in map.", name);
            this.LogError(message, typeof(Level));
            throw new FatalLevelException(message);
        }

        private bool CheckTileType(int xPos, int yPos, string layer)
        {
            TileLayer tileLayer = this.map.GetLayer(layer) as TileLayer;
            Tile tile = null;
            try
            {
                tile = tileLayer.Tiles[xPos, yPos];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new LevelException("Trying to reference a tile that is out of the map bounds.", e);
            }

            if (tile != null)
            {
                return true;
            }
            return false;
        }

        private void Validate(Map map)
        {
            ValidateVolumesLayer(map);
            ValidateObjectsLayer(map);
            ValidateForegroundLayer(map);
            ValidateBackgroundLayer(map);
            ValidateDestructibleLayer(map);
            ValidateCollisionLayer(map);
            ValidateCameraBoundsLayer(map);
        }

        private void ValidateVolumesLayer(Map map)
        {
            try
            {
                MapObjectLayer volumeLayer = this.map.GetLayer(VOLUME_LAYER) as MapObjectLayer;
                if (volumeLayer == null)
                {
                    string message = String.Format("The '{0}' layer must be an object layer.", VOLUME_LAYER);
                    this.LogError(message, typeof(Level));
                    throw new FatalLevelException(message);
                }

                // Make sure all of the volumes in the 'Volumes' layer are valid
                foreach (MapObject volumeObject in volumeLayer.Objects)
                {
                    if (!(volumeObject.Type == LEVEL_END_VOLUME_TYPE || volumeObject.Type == DEATH_VOLUME_TYPE || volumeObject.Type == HEALTH_VOLUME_TYPE ||
                          volumeObject.Type == DAMAGE_VOLUME_TYPE || volumeObject.Type == FORCE_VOLUME_TYPE || volumeObject.Type == ENEMY_SPAWN_VOLUME_TYPE ||
                          volumeObject.Type == TRIGGER_VOLUME_TYPE || volumeObject.Type == END_GAME_EXPLOSION_VOLUME_TYPE || volumeObject.Type == GAME_END_VOLUME_TYPE ||
                          volumeObject.Type == TIMED_ENEMY_SPAWN_VOLUME_TYPE || volumeObject.Type == CHECKPOINT_VOLUME_TYPE))
                    {
                        string message = String.Format("Volume '{0}' has invalid type: {1}", volumeObject.Name, volumeObject.Type);
                        this.LogError(message, typeof(Level));
                        throw new FatalLevelException(message);
                    }
                }
            }
            catch (KeyNotFoundException e)
            {
                string message = String.Format("Level is missing required layer: {0}", VOLUME_LAYER);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message, e);
            }
        }

        private void ValidateCameraBoundsLayer(Map map)
        {
            try
            {
                MapObjectLayer cameraBoundsLayer = this.map.GetLayer(CAMERA_BOUNDS_LAYER) as MapObjectLayer;
                if (cameraBoundsLayer == null)
                {
                    string message = String.Format("The '{0}' layer must be an object layer.", CAMERA_BOUNDS_LAYER);
                    this.LogError(message, typeof(Level));
                    throw new FatalLevelException(message);
                }
            }
            catch (KeyNotFoundException e)
            {
                string message = String.Format("Level is missing required layer: {0}", CAMERA_BOUNDS_LAYER);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message, e);
            }
        }

        private void ValidateObjectsLayer(Map map)
        {
            MapObjectLayer objectsLayer;
            try
            {
                objectsLayer = this.map.GetLayer(OBJECTS_LAYER) as MapObjectLayer;
                if (objectsLayer == null)
                {
                    new LevelException("The '" + OBJECTS_LAYER + "' layer must be an object layer.");
                }

                // Make sure there is a player spawn and that each object in the 'Objects' layer has a valid 'ObjectType'
                bool foundPlayerSpawn = false;
                foreach (MapObject mapObject in objectsLayer.Objects)
                {
                    if (mapObject.Type == PLAYER_SPAWN_MAP_OBJECT_TYPE)
                    {
                        foundPlayerSpawn = true;
                    }
                    else if (mapObject.Type == CHECKPOINT_SPAWN_MAP_OBJECT_TYPE)
                    {
                        // don't need to do anything here.
                    }
                    else if (mapObject.Type == ENEMY_SPAWN_MAP_OBJECT_TYPE)
                    {
                        Property objectType = mapObject.Properties[OBJECT_TYPE_PROPERTY];
                        if (objectType == null)
                        {
                            string message = String.Format("EnemySpawn '{0}' does not contain an 'ObjectType' property", mapObject.Name);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        string objectTypeValue = objectType.RawValue;
                        if (!(objectTypeValue == EASY_PAWN || objectTypeValue == AERIAL_PAWN || objectTypeValue == GRENADIER || objectTypeValue == AERIAL_BOMBER ||
                              objectTypeValue == HARD_PAWN || objectTypeValue == WASP || objectTypeValue == CRAWLER || objectTypeValue == SIMPLE_TURRET ||
                              objectTypeValue == TRACKING_TURRET || objectTypeValue == CORE || objectTypeValue == CORE_SHIELD || objectTypeValue == WAYPOINT_TYPE))
                        {
                            string message = String.Format("EnemySpawn '{0}' contains an invalid 'ObjectType': {1}", mapObject.Name, objectTypeValue);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                    }
                    else if (mapObject.Type == POWERUP_SPAWN_MAP_OBJECT_TYPE)
                    {
                        Property objectType = mapObject.Properties[OBJECT_TYPE_PROPERTY];
                        if (objectType == null)
                        {
                            string message = String.Format("PowerupSpawn '{0}' does not contain an 'ObjectType' property", mapObject.Name);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        string objectTypeValue = objectType.RawValue;
                        if (!(objectTypeValue == POWERUP_AMMO || objectTypeValue == POWERUP_HEALTH || objectTypeValue == POWERUP_LIFE || objectTypeValue == POWERUP_GRENADE ||
                            objectTypeValue == POWERUP_HOMING_MISSILE || objectTypeValue == POWERUP_DOUBLE_DAMAGE || objectTypeValue == POWERUP_JETPACK))
                        {
                            string message = String.Format("PowerupSpawn '{0}' contains an invalid 'ObjectType': {1}", mapObject.Name, objectTypeValue);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                    }
                    else if (mapObject.Type == PARTICLE_EMITTER_SPAWN_MAP_OBJECT_TYPE)
                    {
                        Property particleSpriteKeyProperty = mapObject.Properties[PARTICLE_SPRITE_KEY];
                        Property particleSpawnFrequencyProperty = mapObject.Properties[PARTICLE_SPAWN_FREQUENCY_PROPERTY];
                        Property particleVelocityXProperty = mapObject.Properties[PARTICLE_VELOCITY_X_PROPERTY];
                        Property particleVelocityYProperty = mapObject.Properties[PARTICLE_VELOCITY_Y_PROPERTY];
                        Property particleAccelerationXProperty = mapObject.Properties[PARTICLE_ACCELERATION_X_PROPERTY];
                        Property particleAccelerationYProperty = mapObject.Properties[PARTICLE_ACCELERATION_Y_PROPERTY];
                        Property particleMaxVelocityXProperty = mapObject.Properties[PARTICLE_MAX_VELOCITY_X_PROPERTY];
                        Property particleMaxVelocityYProperty = mapObject.Properties[PARTICLE_MAX_VELOCITY_Y_PROPERTY];
                        Property particleRotationSpeedProperty = mapObject.Properties[PARTICLE_ROTATION_SPEED];
                        Property particleFadeStartProperty = mapObject.Properties[PARTICLE_FADE_START];
                        Property particleFadeEndProperty = mapObject.Properties[PARTICLE_FADE_END];
                        Property particleScaleStartProperty = mapObject.Properties[PARTICLE_SCALE_START];
                        Property particleScaleEndProperty = mapObject.Properties[PARTICLE_SCALE_END];

                        if (particleSpriteKeyProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_SPRITE_KEY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleSpriteKeyProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_SPRITE_KEY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleSpawnFrequencyProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_SPAWN_FREQUENCY_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleSpawnFrequencyProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_SPAWN_FREQUENCY_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleVelocityXProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_VELOCITY_X_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleVelocityXProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_VELOCITY_X_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleVelocityYProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_VELOCITY_Y_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleVelocityYProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_VELOCITY_Y_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleAccelerationXProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_ACCELERATION_X_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleAccelerationXProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_ACCELERATION_X_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleAccelerationYProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_ACCELERATION_Y_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleAccelerationYProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_ACCELERATION_Y_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleMaxVelocityXProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_MAX_VELOCITY_X_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleMaxVelocityXProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_MAX_VELOCITY_X_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleMaxVelocityYProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_MAX_VELOCITY_Y_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleMaxVelocityYProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_MAX_VELOCITY_Y_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleRotationSpeedProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_ROTATION_SPEED);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleRotationSpeedProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_ROTATION_SPEED);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleFadeStartProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_FADE_START);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleFadeStartProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_FADE_START);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleFadeEndProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_FADE_END);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleFadeEndProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_FADE_END);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleScaleStartProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_SCALE_START);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleScaleStartProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_SCALE_START);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }

                        if (particleScaleEndProperty == null)
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing required property: {1}", mapObject.Name, PARTICLE_SCALE_END);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        if (particleScaleEndProperty.RawValue == "")
                        {
                            string message = String.Format("ParticleEmitter '{0}' is missing value for property: {1}", mapObject.Name, PARTICLE_SCALE_END);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                    }
                    else if (mapObject.Type == MOVER_SPAWN_MAP_OBJECT_TYPE)
                    {
                        Property objectType = mapObject.Properties[OBJECT_TYPE_PROPERTY];
                        if (objectType == null)
                        {
                            string message = String.Format("MoverSpawn '{0}' does not contain an 'ObjectType' property", mapObject.Name);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        string objectTypeValue = objectType.RawValue;
                        if (objectTypeValue == BASIC_MOVER || objectTypeValue == SUSPENDED_MOVER)
                        {
                            Property waypointListProperty = mapObject.Properties[WAYPOINT_LIST_PROPERTY];
                            if (waypointListProperty == null)
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' property", mapObject.Name, WAYPOINT_LIST_PROPERTY);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }
                            string waypointListValue = waypointListProperty.RawValue;
                            if (waypointListValue == "")
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' value", mapObject.Name, WAYPOINT_LIST_PROPERTY);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }

                            Property spriteKeyProperty = mapObject.Properties[MOVER_SPRITE_KEY_PROPERTY];
                            if (spriteKeyProperty == null)
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' property", mapObject.Name, MOVER_SPRITE_KEY_PROPERTY);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }
                            string spriteKeyValue = spriteKeyProperty.RawValue;
                            if (spriteKeyValue == "")
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' value", mapObject.Name, MOVER_SPRITE_KEY_PROPERTY);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }

                            Property maxVelocityProperty = mapObject.Properties[MAX_VELOCITY_PROPERTY];
                            if (maxVelocityProperty == null)
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' property", mapObject.Name, MAX_VELOCITY_PROPERTY);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }
                            string maxVelocityValue = maxVelocityProperty.RawValue;
                            if (maxVelocityValue == "")
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' value", mapObject.Name, MAX_VELOCITY_PROPERTY);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }

                            Property stopTime = mapObject.Properties[STOP_TIME];
                            if (stopTime == null)
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' property", mapObject.Name, STOP_TIME);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }
                            string stopTimeValue = stopTime.RawValue;
                            if (stopTimeValue == "")
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' value", mapObject.Name, STOP_TIME);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }

                            Property iterationModeProperty = mapObject.Properties[WAYPOINT_ITERATOR];
                            if (iterationModeProperty == null)
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' property", mapObject.Name, WAYPOINT_ITERATOR);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }
                            string iterationModeValue = iterationModeProperty.RawValue;
                            if (stopTimeValue == "")
                            {
                                string message = String.Format("MoverSpawn '{0}' does not contain an '{1}' value", mapObject.Name, WAYPOINT_ITERATOR);
                                this.LogError(message, typeof(Level));
                                throw new FatalLevelException(message);
                            }
                        }
                        else if (objectTypeValue != WAYPOINT_PROPERTY)
                        {
                            string message = String.Format("MoverSpawn '{0}' contains an invalid 'ObjectType' value: {1}", mapObject.Name, objectTypeValue);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                    }
                    else if (mapObject.Type == ENVIRONMENT_ACTOR_SPAWN_MAP_OBJECT_TYPE)
                    {
                        Property spriteKeyProperty = mapObject.Properties[ENVIRONEMNT_ACTOR_SPRITE_KEY_PROPERTY];
                        if (spriteKeyProperty == null)
                        {
                            string message = String.Format("EnvironemntActor '{0}' does not contain an '{1}' property", mapObject.Name, ENVIRONEMNT_ACTOR_SPRITE_KEY_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                        string spriteKeyValue = spriteKeyProperty.RawValue;
                        if (spriteKeyValue == "")
                        {
                            string message = String.Format("EnvironemntActor '{0}' does not contain an '{1}' value", mapObject.Name, ENVIRONEMNT_ACTOR_SPRITE_KEY_PROPERTY);
                            this.LogError(message, typeof(Level));
                            throw new FatalLevelException(message);
                        }
                    }
                    else
                    {
                        string message = String.Format("'{0}' has an invalid type: {1}", mapObject.Name, mapObject.Type);
                        this.LogError(message, typeof(Level));
                        throw new FatalLevelException(message);
                    }
                }

                if (!foundPlayerSpawn)
                {
                    string message = String.Format("There is no player spawn point in the '{0}' layer.", OBJECTS_LAYER);
                    this.LogError(message, typeof(Level));
                    throw new FatalLevelException(message);
                }
            }
            catch (KeyNotFoundException e)
            {
                string message = String.Format("Level is missing required layer: {0}", OBJECTS_LAYER);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message, e);
            }
        }

        private void ValidateCollisionLayer(Map map)
        {
            try
            {
                TileLayer collisionLayer = this.map.GetLayer(COLLISION_LAYER) as TileLayer;
                if (collisionLayer == null)
                {
                    string message = String.Format("The '{0}' layer must be a tile layer.", COLLISION_LAYER);
                    this.LogError(message, typeof(Level));
                    throw new FatalLevelException(message);
                }
            }
            catch (KeyNotFoundException e)
            {
                string message = String.Format("Level is missing required layer: {0}", COLLISION_LAYER);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message, e);
            }
        }

        private void ValidateForegroundLayer(Map map)
        {
            try
            {
                TileLayer foregroundLayer = this.map.GetLayer(FOREGROUND_LAYER) as TileLayer;
                if (foregroundLayer == null)
                {
                    string message = String.Format("The '{0}' layer must be a tile layer.", FOREGROUND_LAYER);
                    this.LogError(message, typeof(Level));
                    throw new FatalLevelException(message);
                }
            }
            catch (KeyNotFoundException e)
            {
                string message = String.Format("Level is missing required layer: {0}", FOREGROUND_LAYER);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message, e);
            }
        }

        private void ValidateBackgroundLayer(Map map)
        {
            try
            {
                TileLayer foregroundLayer = this.map.GetLayer(BACKGROUND_LAYER) as TileLayer;
                if (foregroundLayer == null)
                {
                    string message = String.Format("The '{0}' layer must be a tile layer.", BACKGROUND_LAYER);
                    this.LogError(message, typeof(Level));
                    throw new FatalLevelException(message);
                }
            }
            catch (KeyNotFoundException e)
            {
                string message = String.Format("Level is missing required layer: {0}", BACKGROUND_LAYER);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message, e);
            }
        }

        private void ValidateDestructibleLayer(Map map)
        {
            try
            {
                TileLayer destructibleLayer = this.map.GetLayer(DESTRUCTIBLE_LAYER) as TileLayer;
                if (destructibleLayer == null)
                {
                    string message = String.Format("The '{0}' layer must be a tile layer.", DESTRUCTIBLE_LAYER);
                    this.LogError(message, typeof(Level));
                    throw new FatalLevelException(message);
                }
            }
            catch (KeyNotFoundException e)
            {
                string message = String.Format("Level is missing required layer: {0}", DESTRUCTIBLE_LAYER);
                this.LogError(message, typeof(Level));
                throw new FatalLevelException(message, e);
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

    }
}
