using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RobotGame.Game;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;

namespace RobotGame.Engine
{
    class Debug
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const string FONT_ASSET = Config.DEBUG_FONT_ASSET;
        private const float TEXT_DRAW_DEPTH = Config.MENU_TEXT_DRAW_DEPTH;
        private const string TITLE_SAFE_OVERLAY = Config.TITLE_SAFE_OVERLAY;
        private const float TITLE_SAFE_OVERLAY_DRAW_DEPTH = Config.TITLE_SAFE_OVERLAY_DRAW_DEPTH;
        private const int TITLE_SAFE_OFFSET_X = Config.TITLE_SAFE_OFFSET_X;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static List<string> DebugInfoList = new List<string>();
        private static List<Color> ColorList = new List<Color>();
        private static List<string> PermanentDebugInfoList = new List<string>();

        private static SpriteFont font;
        private static Texture2D titleSafeOverlay;

        private static KeyboardState lastKeyboardState;

        private static int frameRate = 0;
        private static int frameCounter = 0;
        private static TimeSpan elapsedTime = TimeSpan.Zero;

        // Properties ------------------------------------------------------------------------------------- Properties
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static void LoadContent(RobotGameContentManager contentManager)
        {
            font = contentManager.Load<SpriteFont>(FONT_ASSET);
            titleSafeOverlay = contentManager.Load<Texture2D>(TITLE_SAFE_OVERLAY);
        }

        public static void AddDebugInfo(string info)
        {
            AddDebugInfo(info, Color.White);
        }

        public static void AddPermanentDebugInfo(string info)
        {
            PermanentDebugInfoList.Add(info);
        }

        public static void AddDebugInfo(string info, Color color)
        {
            DebugInfoList.Add(info);
            ColorList.Add(color);
        }

        public static void Update(GameTime gameTime, ref RobotGameState robotGameState)
        {
            if (Player.PlayerList.Count > 0)
            {
                Player player = (Player)Player.PlayerList[0];
                if (Keyboard.GetState().IsKeyDown(Keys.T))
                {
                    player.TakeDamage(25);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    player.TakeDamage(100);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.J))
                {
                    player.JetPackEnabled = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    player.SecondaryWeaponInventory.SetState(WeaponInventory.GRENADE_LAUNCHER_INDEX, InventoryState.Selected);
                    player.SecondaryWeaponInventory.SetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX, InventoryState.Available);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F))
                {
                    SoundManager.MusicFadeOut(2000f);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                {
                    player.SecondaryWeaponInventory.Ammo++;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.H))
                {
                    player.Health++;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.I))
                {
                    player.DisableInput = true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                SoundManager.SoundEffectPitch = -1.0f;
                SoundManager.PlayAndLoopSoundEffect(SoundKey.GameEndRumble);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                RobotGame.HalfSpeed = true;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                Camera.GetInstance().Shake = true;
            }


            // Jump to level
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                robotGameState.Level = -1;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                robotGameState.Level = 0;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                robotGameState.Level = 1;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                robotGameState.Level = 2;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                robotGameState.Level = 3;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D6))
            {
                robotGameState.Level = 4;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D7))
            {
                robotGameState.Level = 5;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D8))
            {
                robotGameState.Level = 6;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D9))
            {
                robotGameState.Level = 7;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }

            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y))
            {
                robotGameState.Level = 7;
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
            }

            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
            {
                Player player = (Player)Player.PlayerList[0];
                player.SecondaryWeaponInventory.SetState(WeaponInventory.GRENADE_LAUNCHER_INDEX, InventoryState.Selected);
                player.SecondaryWeaponInventory.SetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX, InventoryState.Available);
                player.JetPackEnabled = true;
                player.SecondaryWeaponInventory.Ammo = 20;
                player.Health = 100;
            }

            lastKeyboardState = Keyboard.GetState();

            // Framerate
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public static void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 drawPosition = new Vector2(TITLE_SAFE_OFFSET_X + offset.X, 75 + offset.Y);

            frameCounter++;
            string fps = String.Format("FPS: {0}", frameRate);
            spriteBatch.DrawString(font, fps, drawPosition, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, TEXT_DRAW_DEPTH + 0.001f);
            Color fpsColor = (frameRate < 60) ? Color.Red : Color.White;
            spriteBatch.DrawString(font, fps, drawPosition + new Vector2(-1f, -1f), fpsColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, TEXT_DRAW_DEPTH);

            for (int i = 0; i < DebugInfoList.Count; i++)
            {
                drawPosition.Y += 18;
                spriteBatch.DrawString(font, DebugInfoList[i], drawPosition, ColorList[i], 0f, Vector2.Zero, 1f, SpriteEffects.None, TEXT_DRAW_DEPTH);
                spriteBatch.DrawString(font, DebugInfoList[i], drawPosition + new Vector2(1f, 1f), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, TEXT_DRAW_DEPTH + 0.001f);
            }

            for (int i = 0; i < PermanentDebugInfoList.Count; i++)
            {
                drawPosition.Y += 18;
                spriteBatch.DrawString(font, PermanentDebugInfoList[i], drawPosition, Color.Green, 0f, Vector2.Zero, 1f, SpriteEffects.None, TEXT_DRAW_DEPTH);
                spriteBatch.DrawString(font, PermanentDebugInfoList[i], drawPosition + new Vector2(1f, 1f), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, TEXT_DRAW_DEPTH + 0.001f);
            }

            //spriteBatch.Draw(titleSafeOverlay, offset, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, TITLE_SAFE_OVERLAY_DRAW_DEPTH);

            DebugInfoList.Clear();
            ColorList.Clear();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
