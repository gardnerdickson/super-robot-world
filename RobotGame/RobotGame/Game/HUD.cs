using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RobotGame.Engine;
using RobotGame.Game.Input;
using RobotGame.Game.Weapon;
using System.Globalization;

namespace RobotGame.Game
{
    class HUD
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int TITLE_SAFE_OFFSET_X = Config.TITLE_SAFE_OFFSET_X;
        private const int TITLE_SAFE_OFFSET_Y = Config.TITLE_SAFE_OFFSET_Y;

        private const double CONTROLLER_INDEX_CHANGED_DRAW_TIME = 3000d;
        private const float CONTROLLER_INDEX_CHANGED_DRAW_DEPTH = Config.CONTROLLER_INDEX_DRAW_DEPTH;
        
        private const float HUD_SCALE = 1f;

        private const int PLAYER_HEALTH_MAX = Config.PLAYER_HEALTH_MAX;
        private const float JETPACK_FUEL_MAX = Config.JETPACK_FUEL_MAX;

        private const string CROSSHAIR_ASSET = Config.CROSSHAIR_ASSET;
        private const string BOTTOM_ASSET = Config.HUD_BOTTOM_ASSET;
        private const string TOP_ASSET = Config.HUD_TOP_ASSET;

        private const string FONT_ASSET = Config.HUD_FONT_ASSET;

        private const string GRENADE_UNAVAILABLE_ASSET = Config.HUD_GRENADE_UNAVAILABLE_ASSET;
        private const string GRENADE_AVAILABLE_ASSET = Config.HUD_GRENADE_AVAILABLE_ASSET;
        private const string HOMING_MISSILE_UNAVAILABLE_ASSET = Config.HUD_HOMING_MISSILE_UNAVAILABLE_ASSET;
        private const string HOMING_MISSILE_AVAILABLE_ASSET = Config.HUD_HOMING_MISSILE_AVAILABLE_ASSET;
        private const string WEAPON_SELECTED_OVERLAY_ASSET = Config.HUD_WEAPON_SELECTED_OVERLAY_ASSET;
        private const string WEAPON_SELECTED_DOUBLE_DAMAGE_OVERLAY_ASSET = Config.HUD_WEAPON_SELECTED_DOUBLE_DAMAGE_OVERLAY_ASSET;
        private const string PLAYER_LIVES_ICON_ASSET = Config.PLAYER_LIVES_ICON_ASSET;

        private const string METER_CONTAINER_ASSET = Config.HUD_METER_CONTAINER_ASSET;
        private const string METER_ASSET = Config.HUD_METER_ASSET;
        private const string AMMO_ASSET = Config.HUD_AMMO_ASSET;

        private const float OVERLAY_DRAW_DEPTH = Config.HUD_OVERLAY_DRAW_DEPTH;
        private const float ICON_DRAW_DEPTH = Config.HUD_ICON_DRAW_DEPTH;
        private const float METER_DRAW_DEPTH = Config.HUD_METER_DRAW_DEPTH;
        private const float WEAPON_SELECTION_DRAW_DEPTH = Config.HUD_WEAPON_SELECTION_OVERLAY_DEPTH;

        private const int PLAYER_LIVES_ICON_PADDING_X = 5;
        private const int PLAYER_LIVES_ICON_PADDING_Y = 5;

        private const int OVERLAY_ALPHA = 150;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private bool drawControllerIndexChanged;
        private double drawControllerIndexTimeLeft;

        private static HUD instance;
        private int screenWidth;
        private int screenHeight;


        private int playerLives;
        private int playerPoints;
        private int playerHealth;
        private float jetPackFuel;
        private bool jetPackEnabled;
        private int playerAmmo;

        private Vector2 mouseCrosshairPosition;

        private InventoryState grenadeSelectionState;
        private InventoryState homingMissileSelectionState;

        private Texture2D mouseCrosshair;

        private Texture2D bottom;
        private Texture2D top;

        private Texture2D meterContainer;
        private Texture2D meter;
        private Texture2D ammo;

        private Texture2D grenadeUnavailable;
        private Texture2D grenadeAvailable;
        private Texture2D homingMissileUnavailable;
        private Texture2D homingMissileAvailable;
        private Texture2D weaponSelectedOverlay;
        private Texture2D playerLivesIcon;

        private SpriteFont font;

        // Properties ------------------------------------------------------------------------------------- Properties

        public Vector2 CrosshairPosition
        {
            get { return this.mouseCrosshairPosition; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private HUD() { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static HUD GetInstance()
        {
            if (instance == null)
            {
                instance = new HUD();
            }
            return instance;
        }

        public void LoadContent(RobotGameContentManager contentManager, int screenWidth, int screenHeight)
        {
            this.mouseCrosshair = contentManager.Load<Texture2D>(CROSSHAIR_ASSET);
            this.bottom = contentManager.Load<Texture2D>(BOTTOM_ASSET);
            this.top = contentManager.Load<Texture2D>(TOP_ASSET);

            this.grenadeUnavailable = contentManager.Load<Texture2D>(GRENADE_UNAVAILABLE_ASSET);
            this.grenadeAvailable = contentManager.Load<Texture2D>(GRENADE_AVAILABLE_ASSET);
            this.homingMissileUnavailable = contentManager.Load<Texture2D>(HOMING_MISSILE_UNAVAILABLE_ASSET);
            this.homingMissileAvailable = contentManager.Load<Texture2D>(HOMING_MISSILE_AVAILABLE_ASSET);
            this.weaponSelectedOverlay = contentManager.Load<Texture2D>(WEAPON_SELECTED_OVERLAY_ASSET);
            this.playerLivesIcon = contentManager.Load<Texture2D>(PLAYER_LIVES_ICON_ASSET);

            this.meterContainer = contentManager.Load<Texture2D>(METER_CONTAINER_ASSET);
            this.meter = contentManager.Load<Texture2D>(METER_ASSET);
            this.ammo = contentManager.Load<Texture2D>(AMMO_ASSET);

            this.font = contentManager.Load<SpriteFont>(FONT_ASSET);

            this.screenWidth = screenWidth - (TITLE_SAFE_OFFSET_X * 2);
            this.screenHeight = screenHeight - (TITLE_SAFE_OFFSET_Y * 2);
        }

        public void UpdateCrosshairPosition(Vector2 cameraPosition)
        {
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            this.mouseCrosshairPosition = mousePosition + cameraPosition;
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (player != null)
            {
                this.playerLives = player.Lives;
                this.playerPoints = player.Points;
                this.playerHealth = player.Health;
                this.jetPackFuel = player.JetPackFuel;
                this.jetPackEnabled = player.JetPackEnabled;
                this.playerAmmo = player.SecondaryWeaponInventory.Ammo;
                this.grenadeSelectionState = player.SecondaryWeaponInventory.GetState(WeaponInventory.GRENADE_LAUNCHER_INDEX);
                this.homingMissileSelectionState = player.SecondaryWeaponInventory.GetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX);
            }

            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            this.mouseCrosshairPosition = mousePosition + Camera.GetInstance().Position;

            Controller controller = RobotGame.RobotGameInputDevice as Controller;
            if (controller != null && controller.PlayerIndexChanged)
            {
                drawControllerIndexChanged = true;
                drawControllerIndexTimeLeft = CONTROLLER_INDEX_CHANGED_DRAW_TIME;
            }

            if (drawControllerIndexChanged)
            {
                drawControllerIndexTimeLeft -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (drawControllerIndexTimeLeft <= 0)
                {
                    drawControllerIndexChanged = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraOffset)
        {
            switch (RobotGame.RobotGameExecutionState)
            {
                case ExecutionState.Playing:
                case ExecutionState.Pause:
                case ExecutionState.Died:
                case ExecutionState.LevelEnd:
                case ExecutionState.GameEnd:
                case ExecutionState.ShowScore:

                    cameraOffset += new Vector2(TITLE_SAFE_OFFSET_X, TITLE_SAFE_OFFSET_Y);

                    Vector2 origin;
                    if (RobotGame.RobotGameInputDevice is KeyboardAndMouse)
                    {
                        origin = new Vector2(mouseCrosshair.Width / 2, mouseCrosshair.Height / 2);
                        spriteBatch.Draw(this.mouseCrosshair, this.mouseCrosshairPosition, null, new Color(255, 255, 255, 190), 0f, origin, 1f, SpriteEffects.None, OVERLAY_DRAW_DEPTH);
                    }

                    // Draw the blue overlay
                    origin = new Vector2(this.bottom.Width / 2, this.bottom.Height / 2);

                    Vector2 textureOffset = new Vector2(this.screenWidth / 2, (this.screenHeight - (this.bottom.Height * HUD_SCALE / 2)));
                    spriteBatch.Draw(this.bottom, cameraOffset + textureOffset, null, new Color(255, 255, 255, OVERLAY_ALPHA), 0f, origin, HUD_SCALE, SpriteEffects.None, OVERLAY_DRAW_DEPTH);
                    spriteBatch.Draw(this.top, cameraOffset, null, new Color(255, 255, 255, OVERLAY_ALPHA), 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, OVERLAY_DRAW_DEPTH);
                    spriteBatch.Draw(this.top, cameraOffset + new Vector2(this.screenWidth - this.top.Width * HUD_SCALE, 0f), null, new Color(255, 255, 255, OVERLAY_ALPHA), 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.FlipHorizontally, OVERLAY_DRAW_DEPTH);

                    // Draw the player lives
                    if (RobotGame.GameMode == GameMode.NinteenEightyFive)
                    {
                        Vector2 playerLivesOffset = cameraOffset + new Vector2(this.top.Width * HUD_SCALE + PLAYER_LIVES_ICON_PADDING_X, (float)PLAYER_LIVES_ICON_PADDING_Y);
                        spriteBatch.Draw(this.playerLivesIcon, playerLivesOffset + new Vector2(0f, -4f), null, Color.White, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, OVERLAY_DRAW_DEPTH);
                        spriteBatch.DrawString(this.font, String.Format("{0}{1}", "x", this.playerLives), playerLivesOffset + new Vector2(25, 5), Color.White, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, OVERLAY_DRAW_DEPTH);
                        spriteBatch.DrawString(this.font, String.Format("{0}{1}", "x", this.playerLives), playerLivesOffset + new Vector2(26, 6), Color.Black, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, OVERLAY_DRAW_DEPTH + 0.001f);
                    }

                    // Draw the player's points
                    String playerPointsStr = this.playerPoints.ToString("000000", CultureInfo.InvariantCulture);
                    Vector2 playerPointsOffset = cameraOffset + new Vector2(this.screenWidth / 2, 10);
                    Vector2 playerPointsOrigin = new Vector2(this.font.MeasureString(playerPointsStr).X / 2, 0f);
                    spriteBatch.DrawString(this.font, playerPointsStr, playerPointsOffset, Color.White, 0f, playerPointsOrigin, HUD_SCALE, SpriteEffects.None, OVERLAY_DRAW_DEPTH);
                    spriteBatch.DrawString(this.font, playerPointsStr, playerPointsOffset + new Vector2(1f, 1f), Color.Black, 0f, playerPointsOrigin, HUD_SCALE, SpriteEffects.None, OVERLAY_DRAW_DEPTH + 0.001f);


                    // Draw the green health meter
                    spriteBatch.Draw(this.meterContainer, cameraOffset + new Vector2(11f, 8f) * HUD_SCALE, null, Color.White, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, ICON_DRAW_DEPTH);
                    int sourceRectangleWidth = (int)(this.meter.Width * ((float)playerHealth / PLAYER_HEALTH_MAX));
                    spriteBatch.Draw(this.meter, cameraOffset + new Vector2(11f, 8f) * HUD_SCALE, new Rectangle(0, 0, sourceRectangleWidth, this.meter.Height),
                                     Color.Lime, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, METER_DRAW_DEPTH);

                    // Draw the purple jet pack meter 
                    spriteBatch.Draw(this.meterContainer, cameraOffset + new Vector2(this.screenWidth - this.meterContainer.Width * HUD_SCALE - 11f, 8f * HUD_SCALE), null, Color.White, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, ICON_DRAW_DEPTH);
                    if (this.jetPackEnabled)
                    {
                        sourceRectangleWidth = (int)((this.meter.Width * ((float)jetPackFuel / JETPACK_FUEL_MAX)));
                        int meterXPos = (int)((this.meter.Width - sourceRectangleWidth) * HUD_SCALE);
                        spriteBatch.Draw(this.meter, cameraOffset + new Vector2(this.screenWidth - this.meter.Width * HUD_SCALE - 11f + meterXPos, 8f * HUD_SCALE), new Rectangle(0, 0, sourceRectangleWidth, this.meter.Height),
                                         Color.Fuchsia, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.FlipHorizontally, METER_DRAW_DEPTH);
                    }

                    // Draw the ammo indicator
                    int leftSourceRectangleWidth = (int)((4f + (18f * playerAmmo) + 8f * (playerAmmo / 5)));
                    spriteBatch.Draw(this.ammo, cameraOffset + new Vector2(this.screenWidth / 2 - this.ammo.Width * HUD_SCALE / 2 - 147f, this.screenHeight - this.ammo.Height * HUD_SCALE - 20f),
                                     new Rectangle(0, 0, leftSourceRectangleWidth, (int)(this.ammo.Height * HUD_SCALE)), Color.White, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, ICON_DRAW_DEPTH);
                    if (playerAmmo > 10)
                    {
                        int tempAmmo = playerAmmo - 10;
                        int rightSourceRectangleWidth = (int)((4f + 18f * tempAmmo + 8f * (tempAmmo / 5)));
                        spriteBatch.Draw(this.ammo, cameraOffset + new Vector2(this.screenWidth / 2 - this.ammo.Width / 2 + 147f, this.screenHeight - this.ammo.Height - 20f),
                                         new Rectangle(0, 0, rightSourceRectangleWidth, this.ammo.Height), Color.White, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, ICON_DRAW_DEPTH);
                    }

                    // Draw the grenade selection icon
                    if (this.grenadeSelectionState == InventoryState.Unavailable)
                    {
                        spriteBatch.Draw(this.grenadeUnavailable, cameraOffset + new Vector2(this.screenWidth / 2 - this.grenadeUnavailable.Width * 0.8f, this.screenHeight - this.grenadeUnavailable.Height * 0.8f - 11f),
                                         null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, ICON_DRAW_DEPTH);
                    }
                    else
                    {
                        spriteBatch.Draw(this.grenadeAvailable, cameraOffset + new Vector2(this.screenWidth / 2 - this.grenadeAvailable.Width * 0.8f, this.screenHeight - this.grenadeAvailable.Height * 0.8f - 11f),
                                         null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, ICON_DRAW_DEPTH);

                        if (this.grenadeSelectionState == InventoryState.Selected)
                        {
                            spriteBatch.Draw(this.weaponSelectedOverlay, cameraOffset + new Vector2(this.screenWidth / 2 - this.weaponSelectedOverlay.Width * 0.8f, this.screenHeight - this.weaponSelectedOverlay.Height * 0.8f - 11f),
                                                null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, WEAPON_SELECTION_DRAW_DEPTH);
                        }
                    }

                    // Draw the homing missile weapon selection icon
                    if (this.homingMissileSelectionState == InventoryState.Unavailable)
                    {
                        spriteBatch.Draw(this.homingMissileUnavailable, cameraOffset + new Vector2(this.screenWidth / 2, this.screenHeight - this.homingMissileUnavailable.Height * 0.8f - 11f),
                                         null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, ICON_DRAW_DEPTH);
                    }
                    else
                    {
                        spriteBatch.Draw(this.homingMissileAvailable, cameraOffset + new Vector2(this.screenWidth / 2, this.screenHeight - this.homingMissileAvailable.Height * 0.8f - 11f),
                                         null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, ICON_DRAW_DEPTH);

                        if (this.homingMissileSelectionState == InventoryState.Selected)
                        {
                            spriteBatch.Draw(this.weaponSelectedOverlay, cameraOffset + new Vector2(this.screenWidth / 2, this.screenHeight - this.weaponSelectedOverlay.Height * 0.8f - 11f),
                                                null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.FlipHorizontally, WEAPON_SELECTION_DRAW_DEPTH);
                        }
                    }

                    DrawControllerIndexChangedString(spriteBatch, cameraOffset);
                    
                    break;

                default:

                    DrawControllerIndexChangedString(spriteBatch, new Vector2(TITLE_SAFE_OFFSET_X, TITLE_SAFE_OFFSET_Y));

                    break;
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void DrawControllerIndexChangedString(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (drawControllerIndexChanged)
            {
                Controller controller = (Controller)RobotGame.RobotGameInputDevice;
                spriteBatch.DrawString(this.font, String.Format("Controller: {0}", ((int)controller.ActivePlayerIndex + 1)), offset + new Vector2(0f, 40f), Color.White, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, CONTROLLER_INDEX_CHANGED_DRAW_DEPTH);
                spriteBatch.DrawString(this.font, String.Format("Controller: {0}", ((int)controller.ActivePlayerIndex + 1)), offset + new Vector2(1f, 41f), Color.Black, 0f, Vector2.Zero, HUD_SCALE, SpriteEffects.None, CONTROLLER_INDEX_CHANGED_DRAW_DEPTH + 0.001f);
            }
        }
    }
}
