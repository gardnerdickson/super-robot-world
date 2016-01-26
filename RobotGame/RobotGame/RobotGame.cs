using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using RobotGame.Engine;
using RobotGame.Game;
using RobotGame.Exceptions;
using RobotGame.Game.Enemy;
using RobotGame.Game.Input;
using RobotGame.Game.Powerup;
using RobotGame.Game.Volume;
using RobotGame.Game.Weapon;
using RobotGame.Game.Mover;
using RobotGame.Game.Audio;
using System.Globalization;
//using Indiefreaks.Xna.Profiler;
//using Indiefreaks.AOP.Profiler;

namespace RobotGame
{
    public enum ExecutionState
    {
        PressStart,
        PurchaseGame,
        MainMenu,
        ShowControls,
        Playing,
        Died,
        Pause,
        LevelEnd,
        LevelStart,
        GameOver,
        GameEnd,
        ShowScore,
        Credits,
        CreditsEnd,
    }

    public enum GameMode
    {
        Normal,
        NinteenEightyFive
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RobotGame : Microsoft.Xna.Framework.Game
    {
        // Constants ---------------------------------------------------------------------------------------- Contants
        
        private const int RENDER_TARGET_WIDTH = Config.RENDER_TARGET_WIDTH;
        private const int RENDER_TARGET_HEIGHT = Config.RENDER_TARGET_HEIGHT;

        private const string ERROR_LOG = Config.ERROR_LOG;

        private const string MENU_FONT_ASSET = Config.MENU_FONT_ASSET;
        private const string MENU_SMALL_FONT_ASSET = Config.MENU_SMALL_FONT_ASSET;
        private const string PROFILER_FONT_ASSET = Config.PROFILER_FONT_ASSET;

        private const double LEVEL_START_TIME = Config.LEVEL_START_TIME;
        private const double LEVEL_END_TIME = Config.LEVEL_END_TIME;

        private const int NUMBER_OF_WORLDS = Config.NUMBER_OF_WORLDS;

        private const int PLAYER_START_LIVES = Config.PLAYER_START_LIVES;
        private const int PLAYER_START_HEALTH = Config.PLAYER_START_HEALTH;
        private const float PLAYER_JETPACK_FUEL_MAX = Config.JETPACK_FUEL_MAX;
        private const float SECONDARY_FIRE_DELAY = Config.PLAYER_SECONDARY_FIRE_DELAY;
        private const float PLAYER_GRENADE_SPEED = Config.PLAYER_GRENADE_SPEED;
        private const float PLAYER_GRENADE_GRAVITY_FORCE = Config.PLAYER_GRENADE_GRAVITY_FORCE;
        private const int PLAYER_GRENADE_BLAST_DAMAGE = Config.PLAYER_GRENADE_BLAST_DAMAGE;
        private const int PLAYER_GRENADE_BLAST_RADIUS = Config.PLAYER_GRENADE_BLAST_RADIUS;
        private const float PLAYER_GRENADE_MASS = Config.PLAYER_GRENADE_PROJECTILE_MASS;
        private const float PLAYER_GRENADE_ROTATION_INCREMENT = Config.PLAYER_GRENADE_ROTATION_AMOUNT;
        private const float PROJECTILE_HOMING_MISSILE_LAUNCH_SPEED = Config.PLAYER_HOMING_MISSILE_LAUNCH_SPEED;
        private const int HOMING_MISSILE_BLAST_DAMAGE = Config.HOMING_MISSILE_BLAST_DAMAGE;
        private const int HOMING_MISSILE_BLAST_RADIUS = Config.HOMING_MISSILE_BLAST_RADIUS;
        private const int HOMING_MISSILE_TARGET_RADIUS = Config.HOMING_MISSILE_TARGET_RADIUS;

        private const string MENU_BLACK_ASSET = Config.MENU_BLACK_ASSET;

        private const int PRESS_START_MENU_CONTINUE = Config.PRESS_START_MENU_CONTINUE;

        private static string[] MAIN_MENU_OPTIONS = Config.MAIN_MENU_OPTIONS;
        private const string MAIN_MENU_MESSAGE = Config.MAIN_MENU_MESSAGE;
        private const int MAIN_MENU_START = Config.MAIN_MENU_START;
        private const int MAIN_MENU_1985 = Config.MAIN_MENU_1985;
        private const int MAIN_MENU_HOW_TO_PLAY = Config.MAIN_MENU_HOW_TO_PLAY;
        private const int MAIN_MENU_EXIT = Config.MAIN_MENU_EXIT;
        private const int MAIN_MENU_PURCHASE_GAME = Config.MAIN_MENU_PURCHASE_GAME;

        private static string[] PAUSE_MENU_OPTIONS = Config.PAUSE_MENU_OPTIONS;
        private const string PAUSE_MENU_MESSAGE = Config.PAUSE_MENU_MESSAGE;
        private const int PAUSE_MENU_CONTINUE = Config.PAUSE_MENU_CONTINUE;
        private const int PAUSE_MENU_EXIT = Config.PAUSE_MENU_EXIT;

        private static string[] GAME_END_MENU_OPTIONS = Config.GAME_END_MENU_OPTIONS;
        private const string GAME_END_MENU_MESSAGE = Config.GAME_END_MENU_MESSAGE;
        private const int GAME_END_MENU_RESTART = Config.GAME_END_MENU_RESTART;
        private const int GAME_END_MENU_EXIT = Config.GAME_END_MENU_EXIT;

        private static string[] GAME_OVER_MENU_OPTIONS = Config.GAME_OVER_MENU_OPTIONS;
        private const string GAME_OVER_MENU_MESSAGE = Config.GAME_OVER_MENU_MESSAGE;
        private const int GAME_OVER_MENU_CONTINUE = Config.GAME_OVER_MENU_CONTINUE;
        private const int GAME_OVER_MENU_RESTART = Config.GAME_OVER_MENU_RESTART;
        private const int GAME_OVER_MENU_EXIT = Config.GAME_OVER_MENU_EXIT;

        private static string[] CONTROLLER_CONTROLS = Config.CONTROLLER_CONTROLS;
        private static string[] KEYBOARD_CONTROLS = Config.KEYBOARD_CONTROLS;

        private const float SCREEN_OVERLAY_DRAW_DEPTH = Config.MENU_DRAW_DEPTH;

        private const double GAME_END_DELAY = 4375d;
        private const double CREDITS_END_DELAY = 6000d;
        private const int GAME_COMPLETE_STR_LENGTH = 25;
        
        // Data Members --------------------------------------------------------------------------------- Data Members

        public static GameMode GameMode;

        public static ExecutionState RobotGameExecutionState;
        public static InputDevice RobotGameInputDevice;
        public static RobotGameState GameState;

        public static bool HalfSpeed = false;

        private static bool WindowsTrial;

        private bool playFrame = true;

        private PlayTimer playTimer;
        private int numberOfLevels;

        private Menu pressStartMenu;
        private Menu mainMenu;
        private Menu pauseMenu;
        private Menu levelStartMenu;
        private Menu levelEndMenu;
        private Menu playerDiedMenu;
        private Menu gameOverMenu;
        private Menu gameEndMenu;
        private Menu creditsEndMenu;
        private Menu showScoreMenu;
        private Menu confirmationMenu;
        private Menu controlsMenu;
        
        private RobotGameContentManager robotGameContentManager;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;

        private Player player;

        private Credits credits;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public RobotGame(int backBufferWidth, int backBufferHeight, bool fullScreen, InputDeviceType inputDeviceType)
        {
            //Guide.SimulateTrialMode = true;
            //WindowsTrial = true;
            
            graphics = new GraphicsDeviceManager(this);
            
            graphics.PreferredBackBufferWidth = backBufferWidth;
            graphics.PreferredBackBufferHeight = backBufferHeight;
            graphics.IsFullScreen = fullScreen;
            graphics.PreferMultiSampling = true;

            if (inputDeviceType == InputDeviceType.Controller)
            {
                RobotGameInputDevice = new Controller();
            }
            else
            {
                RobotGameInputDevice = new KeyboardAndMouse();
            }
            RobotGameInputDevice.Update();

            robotGameContentManager = new RobotGameContentManager(Services, "Content");

            // Indiefreaks profiler
            //ProfilerGameComponent profilerGameComponent = new ProfilerGameComponent(this, @"Content\" + PROFILER_FONT_ASSET);
            //ProfilingManager.Run = true;
            //Components.Add(profilerGameComponent);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Logging.Init(ERROR_LOG);

            // SpriteSheetFactory needs a ContentManager and a GraphicsDevice to load in assets.
            SpriteSheetFactory.Init(this.robotGameContentManager, GraphicsDevice);

            // SoundManager needs a ContentManager to load in assets.
            SoundManager.Init(this.robotGameContentManager);

            Triggers.Init(GraphicsDevice);

            // Menus

            pressStartMenu = new PressStartMenu(MENU_BLACK_ASSET, RobotGameInputDevice, MENU_FONT_ASSET, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            pressStartMenu.AddMessage(MAIN_MENU_MESSAGE);
            pressStartMenu.AddMessage("");
            pressStartMenu.AddMessage("[Press " + ((RobotGame.RobotGameInputDevice is Controller) ? "Start" : "Enter") + "]");
            pressStartMenu.AddMessage("");

            mainMenu = new OptionMenu(MENU_BLACK_ASSET, RobotGameInputDevice, MENU_FONT_ASSET, MENU_FONT_ASSET, MENU_SMALL_FONT_ASSET, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            mainMenu.AddMessage(MAIN_MENU_MESSAGE);
            ((OptionMenu)mainMenu).AddOption(MAIN_MENU_OPTIONS[MAIN_MENU_START]);
            ((OptionMenu)mainMenu).AddOption(MAIN_MENU_OPTIONS[MAIN_MENU_1985], "1985 Mode: Start with 3 lives. Game over when lives run out.");
            ((OptionMenu)mainMenu).AddOption(MAIN_MENU_OPTIONS[MAIN_MENU_HOW_TO_PLAY]);
            ((OptionMenu)mainMenu).AddOption(MAIN_MENU_OPTIONS[MAIN_MENU_EXIT]);


            pauseMenu = new PauseMenu(MENU_BLACK_ASSET, RobotGameInputDevice, MENU_FONT_ASSET, MENU_FONT_ASSET, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            ((OptionMenu)pauseMenu).AddMessage(PAUSE_MENU_MESSAGE);
            ((OptionMenu)pauseMenu).AddOption(PAUSE_MENU_OPTIONS[PAUSE_MENU_CONTINUE]);
            ((OptionMenu)pauseMenu).AddOption(PAUSE_MENU_OPTIONS[PAUSE_MENU_EXIT]);
            pauseMenu.BackgroundAlpha = 100;

            levelStartMenu = new TimedMenu(MENU_BLACK_ASSET, LEVEL_START_TIME, MENU_FONT_ASSET, false, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);

            levelEndMenu = new TimedMenu(MENU_BLACK_ASSET, LEVEL_END_TIME, MENU_FONT_ASSET, false, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            levelEndMenu.BackgroundAlpha = 100;
            levelEndMenu.AddMessage("LEVEL COMPLETE!");

            playerDiedMenu = new TimedMenu(MENU_BLACK_ASSET, LEVEL_END_TIME);
            playerDiedMenu.BackgroundAlpha = 255;

            gameEndMenu = new TimedMenu(MENU_BLACK_ASSET, GAME_END_DELAY, MENU_FONT_ASSET, false, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            gameEndMenu.BackgroundAlpha = 0;

            creditsEndMenu = new TimedMenu(MENU_BLACK_ASSET, CREDITS_END_DELAY, MENU_FONT_ASSET, false, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            creditsEndMenu.BackgroundAlpha = 0;
            creditsEndMenu.DrawDepth -= 0.02f;

            showScoreMenu = new InteractiveMenu(MENU_BLACK_ASSET, RobotGameInputDevice, MENU_FONT_ASSET, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);

            gameOverMenu = new OptionMenu(MENU_BLACK_ASSET, RobotGameInputDevice, MENU_FONT_ASSET, MENU_FONT_ASSET, MENU_FONT_ASSET, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            gameOverMenu.AddMessage(GAME_OVER_MENU_MESSAGE);
            ((OptionMenu)gameOverMenu).AddOption(GAME_OVER_MENU_OPTIONS[GAME_OVER_MENU_CONTINUE]);
            ((OptionMenu)gameOverMenu).AddOption(GAME_OVER_MENU_OPTIONS[GAME_OVER_MENU_RESTART]);
            ((OptionMenu)gameOverMenu).AddOption(GAME_OVER_MENU_OPTIONS[GAME_OVER_MENU_EXIT]);

            confirmationMenu = new ConfirmationMenu(MENU_BLACK_ASSET, RobotGameInputDevice, MENU_FONT_ASSET, MENU_FONT_ASSET, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            confirmationMenu.BackgroundAlpha = 100;
            ((OptionMenu)pauseMenu).InsertConfirmationMenu(PAUSE_MENU_EXIT, (ConfirmationMenu)confirmationMenu);
            ((OptionMenu)gameOverMenu).InsertConfirmationMenu(GAME_OVER_MENU_EXIT, (ConfirmationMenu)confirmationMenu);
            ((OptionMenu)gameOverMenu).InsertConfirmationMenu(GAME_OVER_MENU_RESTART, (ConfirmationMenu)confirmationMenu);
            ((OptionMenu)mainMenu).InsertConfirmationMenu(MAIN_MENU_EXIT, (ConfirmationMenu)confirmationMenu);

            controlsMenu = new InteractiveMenu(MENU_BLACK_ASSET, RobotGameInputDevice, MENU_SMALL_FONT_ASSET, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            string[] controlsMessages;
            if (RobotGameInputDevice is Controller)
            {
                controlsMessages = CONTROLLER_CONTROLS;
            }
            else
            {
                controlsMessages = KEYBOARD_CONTROLS;
            }
            foreach (string message in controlsMessages)
            {
                controlsMenu.AddMessage(message);
            }
            controlsMenu.Alignment = Menu.TextAlignment.Left;

            this.credits = new Credits(RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);

            numberOfLevels = Level.LEVEL_ASSETS.Length;

            ResetGame();
            RobotGameExecutionState = ExecutionState.PressStart;

#if XBOX
            // This is to make sure that Guide.IsTrailMode property is read accurately. Without this, Guide.IsTrialMode will alway read 'true'
            Components.Add(new GamerServicesComponent(this));
#endif

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if DEBUG
            Debug.LoadContent(this.robotGameContentManager);
#endif

            pressStartMenu.LoadContent(this.robotGameContentManager);
            mainMenu.LoadContent(this.robotGameContentManager);
            pauseMenu.LoadContent(this.robotGameContentManager);
            levelStartMenu.LoadContent(this.robotGameContentManager);
            levelEndMenu.LoadContent(this.robotGameContentManager);
            playerDiedMenu.LoadContent(this.robotGameContentManager);
            gameEndMenu.LoadContent(this.robotGameContentManager);
            creditsEndMenu.LoadContent(this.robotGameContentManager);
            showScoreMenu.LoadContent(this.robotGameContentManager);
            gameOverMenu.LoadContent(this.robotGameContentManager);
            controlsMenu.LoadContent(this.robotGameContentManager);

            this.credits.LoadContent(this.robotGameContentManager);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderTarget = new RenderTarget2D(GraphicsDevice, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);

            SpriteSheetFactory.PreLoadSprites();
            SoundManager.PreLoadAllSoundEffects(SoundKey.None, SoundKey.GameEndSong);

            // Tell the HUD to load it's textures.
            HUD.GetInstance().LoadContent(this.robotGameContentManager, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!HalfSpeed || (HalfSpeed && playFrame))
            {
                if (RobotGameInputDevice != null)
                {
                    RobotGameInputDevice.Update();
                }
            }

#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                System.Diagnostics.Debugger.Break();

            //Debug.AddDebugInfo("Total play time: " + this.playTimer.GetPrettyString());
            //Debug.AddDebugInfo("Number of Actors: " + Actor.ActorList.Count);
            //Debug.AddDebugInfo("Number of Enemies: " + AbstractEnemy.EnemyList.Count);
            //Debug.AddDebugInfo("Number of Enemy Projectiles: " + Projectile.EnemyProjectileList.Count);
            //Debug.AddDebugInfo("Number of Player Projectiles: " + Projectile.PlayerProjectileList.Count);
            //Debug.AddDebugInfo("Level number: " + this.robotGameState.Level);

            Debug.AddDebugInfo("Trail mode: " + Guide.IsTrialMode, Color.DarkOrange);
            Debug.AddDebugInfo("Game Mode: " + RobotGame.GameMode);
            Debug.AddDebugInfo("Checkpoint priority: " + GameState.LevelCheckpointPriority);
            Debug.AddDebugInfo("Player Deaths: " + GameState.PlayerDeaths);
#endif

            switch (RobotGameExecutionState)
            {
                case ExecutionState.PressStart:

                    if (gameTime.TotalGameTime.TotalMilliseconds > 100)
                    {
                        int pressStartValue = this.pressStartMenu.Update();
                        if (pressStartValue == PRESS_START_MENU_CONTINUE)
                        {
                            SoundManager.PlaySoundEffect(SoundKey.MenuIn);
                            RobotGameExecutionState = ExecutionState.MainMenu;

#if XBOX
                            if (Guide.IsTrialMode)
                            {
                                ((OptionMenu)mainMenu).AddOption("Purchase Full Game");
                            }
#endif
                        }
                    }

                    break;

                case ExecutionState.MainMenu:

                    int mainMenuValue = this.mainMenu.Update();
                    if (mainMenuValue == MAIN_MENU_START || mainMenuValue == MAIN_MENU_1985)
                    {
                        RobotGame.GameMode = (mainMenuValue == MAIN_MENU_START) ? GameMode.Normal : GameMode.NinteenEightyFive;
                        
                        SoundManager.PlaySoundEffect(SoundKey.MenuStartGame);

                        ((OptionMenu)mainMenu).Reset();

                        RobotGameExecutionState = ExecutionState.LevelStart;
                        this.levelStartMenu.Messages.Clear();
                        this.levelStartMenu.AddMessage(String.Format("WORLD {0}-{1}", (GameState.Level / NUMBER_OF_WORLDS) + 1, (GameState.Level % NUMBER_OF_WORLDS) + 1));
                    }
                    else if (mainMenuValue == MAIN_MENU_HOW_TO_PLAY)
                    {
                        RobotGameExecutionState = ExecutionState.ShowControls;
                        SoundManager.PlaySoundEffect(SoundKey.MenuIn);
                    }
                    else if (mainMenuValue == MAIN_MENU_EXIT)
                    {
                        this.Exit();
                    }
                    else if (mainMenuValue == MAIN_MENU_PURCHASE_GAME)
                    {
                        PlayerIndex index = ((Controller)RobotGameInputDevice).ActivePlayerIndex;
                        if (GuideUtils.UserCanPurchaseGame(index))
                        {
                            if (!Guide.IsVisible)
                            {
                                Guide.ShowMarketplace(index);

                                RobotGameExecutionState = ExecutionState.PurchaseGame;
                            }
                        }
                    }

                    TimerManager.GetInstance().Update(gameTime);

                    break;

                case ExecutionState.PurchaseGame:

                    if (!Guide.IsVisible)
                    {
                        if (!Guide.IsTrialMode)
                        {
                            ((OptionMenu)mainMenu).RemoveOptionAt(MAIN_MENU_PURCHASE_GAME);
                            ((OptionMenu)mainMenu).CurrentOption = 0;
                        }

                        RobotGameExecutionState = ExecutionState.MainMenu;
                    }

                    break;

                case ExecutionState.ShowControls:

                    if (this.controlsMenu.Update() != -1)
                    {
                        RobotGameExecutionState = ExecutionState.MainMenu;
                        SoundManager.PlaySoundEffect(SoundKey.MenuOut);
                    }

                    break;

                case ExecutionState.Playing:
                case ExecutionState.Died:
                case ExecutionState.GameEnd:

                    if (RobotGameExecutionState == ExecutionState.Died)
                    {
                        if (this.playerDiedMenu.Update() != -1)
                        {
                            if (this.player.Lives <= 0)
                            {
                                RobotGameExecutionState = ExecutionState.GameOver;
                            }
                            else
                            {
                                ((TimedMenu)this.playerDiedMenu).Reset();
                                RobotGameExecutionState = ExecutionState.LevelStart;
                                
                                GameState.PlayerLives = this.player.Lives;
                                GameState.PlayerHealth = Config.PLAYER_START_HEALTH;

                                HalfSpeed = false;
                                this.playFrame = true;
                                RobotGameInputDevice.PauseEnabled = true;
                                SoundManager.SoundEffectPitch = 0.0f;
                            }
                        }
                    }
                    else if (RobotGameExecutionState == ExecutionState.GameEnd)
                    {
                        if (gameEndMenu.Update() != -1)
                        {
                            ((TimedMenu)gameEndMenu).Reset();

                            OnGameEnd();
                        }
                        else
                        {
                            double timeLeft = ((TimedMenu)gameEndMenu).TimeLeft();
                            double lifeTime = ((TimedMenu)gameEndMenu).LifeTime;

                            double fadeToBlackPercentage = ((lifeTime - timeLeft) / lifeTime);

                            gameEndMenu.BackgroundAlpha = (int)(255 * fadeToBlackPercentage);
                        }
                    }

                    if (!this.IsActive)
                    {
                        RobotGameExecutionState = ExecutionState.Pause;
                    }

                    if (playFrame)
                    {
                        if (HalfSpeed)
                        {
                            playFrame = false;
                        }

                        // Update all actors
                        for (int i = Actor.ActorList.Count - 1; i >= 0; i--)
                        {
                            Actor.ActorList[i].Update(gameTime);
                        }

                        // Update map volumes
                        foreach (AbstractVolume volume in AbstractVolume.VolumeList)
                        {
                            volume.Update();
                        }

                        // Update camera position.
                        Camera.GetInstance().Update(this.player);

                        Level.GetInstance().Update();

                        // Spawn and unspawn enemies
                        ActorDirector.GetInstance().Update(Camera.GetInstance().Position);

                        // Check for change to Pause state
                        if (RobotGameExecutionState == ExecutionState.Playing)
                        {
                            if (RobotGameInputDevice.GetPause())
                            {
                                RobotGameExecutionState = ExecutionState.Pause;
                                RobotGameInputDevice.DisableLandFeedback();
                                RobotGameInputDevice.DisableJetpackFeedback();
                                RobotGameInputDevice.DisableTakeDamageFeedback();
                            }
                        }

                        TimerManager.GetInstance().Update(gameTime);

                        if (RobotGameExecutionState == ExecutionState.Playing)
                        {
                            this.playTimer.Update(gameTime);
                        }
                    }
                    else if (HalfSpeed)
                    {
                        playFrame = true;
                    }

#if DEBUG
                    Debug.Update(gameTime, ref GameState);
#endif

                    break;

                case ExecutionState.Pause:

                    int pauseMenuValue = this.pauseMenu.Update();

                    Controller controller = RobotGameInputDevice as Controller;
                    if (controller != null && controller.PlayerIndexChanged)
                    {
                        break;
                    }

                    if (pauseMenuValue == PAUSE_MENU_EXIT)
                    {
                        ((OptionMenu)pauseMenu).Reset();

                        OnLevelEnd();
                        ResetGame();
                        SoundManager.Reset();
                    }
                    else if (pauseMenuValue == PAUSE_MENU_CONTINUE)
                    {
                        ((OptionMenu)pauseMenu).Reset();

                        RobotGameExecutionState = ExecutionState.Playing;
                        SoundManager.PlaySoundEffect(SoundKey.MenuIn);
                    }

                    break;

                case ExecutionState.LevelEnd:

                    if (this.levelEndMenu.Update() != -1)
                    {
#if XBOX
                        if (Guide.IsTrialMode && GameState.Level >= 2)
                        {
                            OnGameEnd();
                        }
                        else
                        {
#elif WINDOWS
                        if (WindowsTrial && GameState.Level >= 2)
                        {
                            OnGameEnd();
                        }
                        else
                        {
#endif
                            ((TimedMenu)this.levelEndMenu).Reset();
                            RobotGameExecutionState = ExecutionState.LevelStart;
                            SaveGameState(GameState.Level + 1);
                            GameState.LevelCheckpointPriority = -1;
                            GameState.CheckpointRespawnPosition = Vector2.Zero;
                            this.levelStartMenu.Messages.Clear();
                            this.levelStartMenu.AddMessage(String.Format("WORLD {0}-{1}", (GameState.Level / NUMBER_OF_WORLDS) + 1, (GameState.Level % NUMBER_OF_WORLDS) + 1));
                        }
                    }

                    TimerManager.GetInstance().Update(gameTime);

                    break;

                case ExecutionState.LevelStart:

                    if (this.levelStartMenu.Update() != -1)
                    {
                        ((TimedMenu)this.levelStartMenu).Reset();
                        OnLevelEnd();
                        RobotGameExecutionState = ExecutionState.Playing;
                        OnLevelStart();
                    }

                    TimerManager.GetInstance().Update(gameTime);
                    
                    break;

                case ExecutionState.GameOver:

                    int gameOverMenuValue = this.gameOverMenu.Update();
                    if (gameOverMenuValue == GAME_OVER_MENU_CONTINUE)
                    {
                        gameOverMenu.Reset();

                        int startLevel;
                        int playerAmmo;
                        bool jetpackEnabled;
                        InventoryState grenadeState;
                        InventoryState homingMissileState;
                        if (GameState.Level < 3)
                        {
                            startLevel = 0;
                            playerAmmo = 0;
                            jetpackEnabled = false;
                            grenadeState = InventoryState.Unavailable;
                            homingMissileState = InventoryState.Unavailable;
                        }
                        else if (GameState.Level < 6)
                        {
                            startLevel = 3;
                            playerAmmo = 5;
                            jetpackEnabled = true;
                            grenadeState = InventoryState.Selected;
                            homingMissileState = InventoryState.Unavailable;
                        }
                        else
                        {
                            startLevel = 6;
                            playerAmmo = 10;
                            jetpackEnabled = true;
                            grenadeState = InventoryState.Selected;
                            homingMissileState = InventoryState.Available;
                        }
                        int playerPoints = this.player.Points;
                        int continuesUsed = GameState.ContinuesUsed + 1;
                        TimeSpan totalPlayTime = this.playTimer.TotalPlayTime;


                        OnLevelEnd();
                        ResetGame();
                        SoundManager.Reset();

                        GameState.Level = startLevel;
                        GameState.SecondaryWeaponAmmo = playerAmmo;
                        GameState.PlayerJetpackEnabled = jetpackEnabled;
                        GameState.GrenadeLauncherInventoryState = grenadeState;
                        GameState.HomingMissileLauncherInventoryState = homingMissileState;
                        GameState.PlayerPoints = playerPoints;
                        GameState.ContinuesUsed = continuesUsed;
                        this.playTimer = new PlayTimer(totalPlayTime.TotalMilliseconds);

                        RobotGameExecutionState = ExecutionState.LevelStart;
                        this.levelStartMenu.Messages.Clear();
                        this.levelStartMenu.AddMessage(String.Format("WORLD {0}-{1}", (GameState.Level / NUMBER_OF_WORLDS) + 1, (GameState.Level % NUMBER_OF_WORLDS) + 1));
                    }
                    else if (gameOverMenuValue == GAME_OVER_MENU_RESTART)
                    {
                        gameOverMenu.Reset();

                        OnLevelEnd();
                        ResetGame();
                        SoundManager.Reset();
                    }
                    else if (gameOverMenuValue == GAME_OVER_MENU_EXIT)
                    {
                        this.Exit();
                    }

                    TimerManager.GetInstance().Update(gameTime);

                    break;

                case ExecutionState.ShowScore:

                    if (showScoreMenu.Update() != -1)
                    {
                        showScoreMenu.Reset();

#if XBOX
                        if (Guide.IsTrialMode)
                        {
                            RobotGameExecutionState = ExecutionState.MainMenu;
                        }
                        else
                        {
#elif WINDOWS
                        if (WindowsTrial)
                        {
                            RobotGameExecutionState = ExecutionState.MainMenu;
                        }
                        else
                        {
#endif
                            RobotGameExecutionState = ExecutionState.Credits;
                        }
                    }

                    break;

                case ExecutionState.Credits:

                    if (credits.Update() == 0)
                    {
                        //SoundManager.StopMusic();
                        SoundManager.StopAllLoopingSoundEffects();
                        SoundManager.SoundEffectVolume = Config.SOUND_EFFECT_VOLUME;
                        SoundManager.MusicVolume = Config.MUSIC_VOLUME;
                        RobotGameExecutionState = ExecutionState.CreditsEnd;
                    }

                    TimerManager.GetInstance().Update(gameTime);

                    break;

                case ExecutionState.CreditsEnd:

                    if (creditsEndMenu.Update() != -1)
                    {
                        ((TimedMenu)creditsEndMenu).Reset();
                        this.credits.Reset();
                        SoundManager.StopMusic();
                        SoundManager.MusicVolume = Config.MUSIC_VOLUME;
                        SoundManager.StopSoundEffect(SoundKey.GameEndSong);
                        RobotGameExecutionState = ExecutionState.MainMenu;
                    }
                    else
                    {
                        double timeLeft = ((TimedMenu)creditsEndMenu).TimeLeft();
                        double lifeTime = ((TimedMenu)creditsEndMenu).LifeTime;
                        timeLeft = MathHelper.Clamp((float)timeLeft - 2000f, 0f, (float)timeLeft);
                        lifeTime = MathHelper.Clamp((float)lifeTime - 2000f, 0f, (float)lifeTime);

                        double fadePercentage = ((lifeTime - timeLeft) / lifeTime);

                        creditsEndMenu.BackgroundAlpha = (int)(255 * fadePercentage);
                        SoundManager.MusicVolume = Config.MUSIC_VOLUME - (float)fadePercentage;
                        SoundManager.ChangeSoundEffectInstanceVolume(SoundKey.GameEndSong, Math.Max(0.0f, Config.MUSIC_VOLUME - (float)fadePercentage));
                    }

                    TimerManager.GetInstance().Update(gameTime);

                    break;
            }

            HUD.GetInstance().Update(gameTime, this.player);

            SoundManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Render scene to a 1280x720 target, even though back buffer is 1080p
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.Black);

            // Set the spritebatch to draw everything with a screen offset
            Matrix translationMatrix = Camera.GetInstance().GetMatrix();
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, translationMatrix);

            switch (RobotGameExecutionState)
            {
                case ExecutionState.PressStart:

                    this.pressStartMenu.Draw(spriteBatch);

                    break;

                case ExecutionState.MainMenu:

                    this.mainMenu.Draw(spriteBatch);

                    break;

                case ExecutionState.ShowControls:

                    this.controlsMenu.Draw(spriteBatch);

                    break;

                case ExecutionState.Playing:
                case ExecutionState.Pause:
                case ExecutionState.Died:
                case ExecutionState.LevelEnd:
                case ExecutionState.GameEnd:
                case ExecutionState.ShowScore:
                    
                    if (RobotGameExecutionState == ExecutionState.Pause)
                    {
                        this.pauseMenu.Draw(spriteBatch);
                    }
                    else if (RobotGameExecutionState == ExecutionState.LevelEnd)
                    {
                        this.levelEndMenu.Draw(spriteBatch);
                    }
                    else if (RobotGameExecutionState == ExecutionState.GameEnd)
                    {
                        this.gameEndMenu.Draw(spriteBatch);
                    }
                    else if (RobotGameExecutionState == ExecutionState.ShowScore)
                    {
                        this.showScoreMenu.Draw(spriteBatch);
                    }

                    // Draw the level
                    Level.GetInstance().Draw(spriteBatch, (int)Camera.GetInstance().Position.X, (int)Camera.GetInstance().Position.Y, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);

                    // Draw all of the actors
                    foreach (Actor actor in Actor.ActorList)
                    {
                        actor.Draw(spriteBatch);
                    }

                    break;

                case ExecutionState.LevelStart:

                    this.levelStartMenu.Draw(spriteBatch);

                    break;

                case ExecutionState.GameOver:

                    this.gameOverMenu.Draw(spriteBatch);

                    break;

                case ExecutionState.Credits:
                case ExecutionState.CreditsEnd:

                    this.credits.Draw(spriteBatch);

                    if (RobotGameExecutionState == ExecutionState.CreditsEnd)
                    {
                        this.creditsEndMenu.Draw(spriteBatch);
                    }

                    break;
            }

#if DEBUG
            Debug.Draw(spriteBatch, Camera.GetInstance().Position);
#endif

            HUD.GetInstance().Draw(spriteBatch, Camera.GetInstance().Position);
                        
            spriteBatch.End();


            // Set render target back to the back buffer and render the 720p scene to back buffer (whatever back buffer resolution may be)
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(this.renderTarget, new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void ResetGame()
        {
            //SoundManager.StopMusic();

            RobotGameExecutionState = ExecutionState.MainMenu;

            ProjectileFactory grenadeFactory = new ExplosiveProjectileFactory(PLAYER_GRENADE_BLAST_DAMAGE, PLAYER_GRENADE_BLAST_RADIUS, PLAYER_GRENADE_MASS, PLAYER_GRENADE_GRAVITY_FORCE, PLAYER_GRENADE_ROTATION_INCREMENT, SpriteKey.Grenade, ProjectileSource.Player);
            ProjectileFactory homingMissileFactory = new HomingMissileFactory(HOMING_MISSILE_BLAST_DAMAGE, HOMING_MISSILE_BLAST_RADIUS, HOMING_MISSILE_TARGET_RADIUS, ProjectileSource.Player);
            ProjectileLauncher grenadeLauncherPrototype = new ProjectileLauncher(grenadeFactory, new SimpleDelayLogic(SECONDARY_FIRE_DELAY), new SimpleFireLogic(), PLAYER_GRENADE_SPEED, SoundKey.SecondaryFire, SoundKey.FireNoAmmo);
            HomingMissileLauncher homingMissileLauncherPrototype = new HomingMissileLauncher(new SimpleDelayLogic(SECONDARY_FIRE_DELAY), homingMissileFactory, PROJECTILE_HOMING_MISSILE_LAUNCH_SPEED, SoundKey.SecondaryFire, SoundKey.FireNoAmmo, SoundKey.HomingMissileSeekMode);

            GameState = new RobotGameState(0, -1, Vector2.Zero, 0, PLAYER_START_LIVES, 0, PLAYER_START_HEALTH, false, grenadeLauncherPrototype, homingMissileLauncherPrototype, InventoryState.Unavailable, InventoryState.Unavailable, 0, 0);

            this.playTimer = new PlayTimer();
        }

        private void SaveGameState(int level)
        {
            GameState.Level = level;
            GameState.PlayerPoints = this.player.Points;
            GameState.PlayerLives = this.player.Lives;
            
            if (this.player.Health <= 0)
            {
                GameState.PlayerHealth = PLAYER_START_HEALTH / 2;
            }
            else
            {
                GameState.PlayerHealth = this.player.Health;
            }
            
            GameState.PlayerJetpackEnabled = this.player.JetPackEnabled;
            GameState.SecondaryWeaponAmmo = this.player.SecondaryWeaponInventory.Ammo;
            GameState.GrenadeLauncherInventoryState = this.player.SecondaryWeaponInventory.GetState(WeaponInventory.GRENADE_LAUNCHER_INDEX);
            GameState.HomingMissileLauncherInventoryState = this.player.SecondaryWeaponInventory.GetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX);
        }

        private void OnLevelStart()
        {
            // Load the appropriate level according to the provided RobotGameState
            Level.GetInstance().Load(this.robotGameContentManager, GameState.Level);

            ActorDirector.GetInstance().Init(RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            ActorDirector.GetInstance().SpawnMapVolumes();

            Camera.GetInstance().Init(new Vector2(0f, Level.GetInstance().Height), Level.GetInstance().Width, Level.GetInstance().Height, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);

            // Load the player state
            Vector2 position;
            if (GameState.CheckpointRespawnPosition != Vector2.Zero)
            {
                position = GameState.CheckpointRespawnPosition;
            }
            else
            {
                position = Level.GetInstance().GetMapObjectSpawnPointsByType(Level.PLAYER_SPAWN_MAP_OBJECT_TYPE)[0].Position;
            }
            this.player = new Player(position, PhysicsMode.Gravity);
            this.player.Lives = GameState.PlayerLives;
            this.player.Points = GameState.PlayerPoints;
            this.player.Health = Config.PLAYER_HEALTH_MAX;

            WeaponInventory weaponInventory = new WeaponInventory();
            weaponInventory.Ammo = GameState.SecondaryWeaponAmmo;
            weaponInventory.SetWeapon(WeaponInventory.GRENADE_LAUNCHER_INDEX, (AbstractWeapon)GameState.GrenadeLauncherPrototype.Clone());
            weaponInventory.SetWeapon(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX, (AbstractWeapon)GameState.HomingMissileLauncherPrototype.Clone());
            weaponInventory.SetState(WeaponInventory.GRENADE_LAUNCHER_INDEX, GameState.GrenadeLauncherInventoryState);
            weaponInventory.SetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX, GameState.HomingMissileLauncherInventoryState);
            this.player.SecondaryWeaponInventory = weaponInventory;
            
            this.player.JetPackEnabled = GameState.PlayerJetpackEnabled;
            if (GameState.PlayerJetpackEnabled)
            {
                this.player.JetPackFuel = PLAYER_JETPACK_FUEL_MAX;
            }
            
            SaveGameState(GameState.Level);

            SoundManager.UnloadSongs();
            SoundManager.PreLoadSongs(Level.GetInstance().LevelInfo.SongInfo.Intro, Level.GetInstance().LevelInfo.SongInfo.Loop);
            SoundManager.StartLevelMusic(Level.GetInstance().LevelInfo.SongInfo);

            if (GameState.Level == 8)
            {
                SoundManager.LoadSoundEffect(SoundKey.GameEndSong);
                SoundManager.PreLoadSongs(SongKey.CoreBossMusicIntro, SongKey.CoreBossMusicLoop);
            }
        }

        private void OnLevelEnd()
        {
            // Remove all actors from the game
            AbstractEnemy.EnemyList.Clear();
            AbstractPowerup.PowerupList.Clear();
            Projectile.PlayerProjectileList.Clear();
            Projectile.EnemyProjectileList.Clear();
            AbstractMover.MoverList.Clear();
            Actor.ActorList.Clear();
            Player.PlayerList.Clear();
            AbstractVolume.VolumeList.Clear();

            TimerManager.GetInstance().RemoveAllTimers();
            
            SoundManager.StopAllLoopingSoundEffects();
        }

        private void OnGameEnd()
        {
            showScoreMenu.Messages.Clear();
            showScoreMenu.AddMessage("");

#if XBOX
            if (Guide.IsTrialMode)
            {
                showScoreMenu.AddMessage("End of trial. Download full");
                showScoreMenu.AddMessage("version to play all 9 levels.");
            }
            else
            {
#elif WINDOWS
            if (WindowsTrial)
            {
                showScoreMenu.AddMessage("End of trial. Download full");
                showScoreMenu.AddMessage("version to play all 9 levels.");
            }
            else
            {
#endif
                showScoreMenu.AddMessage("GAME COMPLETE!");
            }

            showScoreMenu.AddMessage("");
            
            int numSpaces;
            string playTimerLabelStr = "Play Time:";
            string playTimerValueStr = playTimer.GetPrettyString();
            numSpaces = GAME_COMPLETE_STR_LENGTH - playTimerLabelStr.Length - playTimerValueStr.Length;
            showScoreMenu.AddMessage(playTimerLabelStr + new String(' ', numSpaces) + playTimerValueStr);

            string scoreLabelStr = "Score:";
            string scoreValueStr = this.player.Points.ToString("000000", CultureInfo.InvariantCulture);
            numSpaces = GAME_COMPLETE_STR_LENGTH - scoreLabelStr.Length - scoreValueStr.Length;
            showScoreMenu.AddMessage(scoreLabelStr + new String(' ', numSpaces) + scoreValueStr);

            string deathLabelStr = "Deaths:";
            string deathValueStr = GameState.PlayerDeaths.ToString(CultureInfo.InvariantCulture);
            numSpaces = GAME_COMPLETE_STR_LENGTH - deathLabelStr.Length - deathValueStr.Length;
            showScoreMenu.AddMessage(deathLabelStr + new String(' ', numSpaces) + deathValueStr);

            if (RobotGame.GameMode == GameMode.NinteenEightyFive)
            {
                string continuesLabelStr = "Continues:";
                string continuesValueStr = GameState.ContinuesUsed.ToString(CultureInfo.InvariantCulture);
                numSpaces = GAME_COMPLETE_STR_LENGTH - continuesLabelStr.Length - continuesValueStr.Length;
                showScoreMenu.AddMessage(continuesLabelStr + new String(' ', numSpaces) + continuesValueStr);
            }
            else
            {
                showScoreMenu.AddMessage("");
            }

            showScoreMenu.AddMessage("");
            showScoreMenu.AddMessage("[Press " + ((RobotGame.RobotGameInputDevice is Controller) ? "A]" : "Space]"));
            showScoreMenu.AddMessage("");

            RobotGame.HalfSpeed = false;
            this.playFrame = true;
            RobotGameInputDevice.PauseEnabled = true;
            SoundManager.SoundEffectPitch = 0.0f;
            Camera.GetInstance().Shake = false;

            OnLevelEnd();
            ResetGame();

            RobotGameExecutionState = ExecutionState.ShowScore;
        }
    }
}
