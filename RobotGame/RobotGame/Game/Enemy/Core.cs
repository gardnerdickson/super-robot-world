using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame.Game.Enemy
{
    class Core : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        public const int START_HEALTH = Config.CORE_HEALTH;

        public const float START_SHAKE_DELAY = 2000f;
        public const float ACTIVATE_END_GAME_EXPLOSION_DELAY = 5000;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback StartShakeCallback;
        private TimerCallback ActivateEndGameExplosionsCallback;

        private Color tint;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Core(Vector2 position, string name)
            : base(position, PhysicsMode.None, name)
        {
            this.sprite = new Sprite(SpriteKey.Core);
            this.health = START_HEALTH;
            this.pointValue = 1000;

            this.StartShakeCallback = new TimerCallback(start_shake_callback);
            this.ActivateEndGameExplosionsCallback = new TimerCallback(activate_end_game_explosion_callback);

            this.collideWithEnvironment = false;

            tint = Color.White;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        protected override void OnDeath()
        {
            Explode(SpriteKey.CoreExplosion, 5, 5);

            Triggers.TriggerEvent(TriggerKey.CoreDestroyedDeactivateSpawners);

            RobotGame.HalfSpeed = true;
            RobotGame.RobotGameInputDevice.PauseEnabled = false;
            SoundManager.SoundEffectPitch = -1.0f;
            SoundManager.StopMusic();

            TimerManager.GetInstance().RegisterTimer(START_SHAKE_DELAY, this.StartShakeCallback, null);
            TimerManager.GetInstance().RegisterTimer(ACTIVATE_END_GAME_EXPLOSION_DELAY, this.ActivateEndGameExplosionsCallback, null);

            base.OnDeath();
        }

        public override void PerformAI(Player player)
        {
            // do nothing
        }

        public override void TakeDamage(int damage)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                OnDeath();
                this.Remove();

                SoundManager.PlaySoundEffect(SoundKey.EnemyDeath);
            }
            else
            {
                // change color slightly
                float tintValue = ((float)this.health / (float)START_HEALTH) * 255;
                this.tint.G = (byte)tintValue;
                this.tint.B = (byte)tintValue;

                SoundManager.PlayRandomSoundEffect(SoundKey.EnemyTakeDamage1, SoundKey.EnemyTakeDamage2, SoundKey.EnemyTakeDamage3);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.sprite.Draw(spriteBatch, this.position, 0f, this.drawDepth, SpriteEffects.None, this.tint);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void start_shake_callback(Object param)
        {
            Camera.GetInstance().Shake = true;
            SoundManager.PlayAndLoopSoundEffect(SoundKey.GameEndRumble);
        }

        private void activate_end_game_explosion_callback(Object param)
        {
            Triggers.TriggerEvent(TriggerKey.CoreDestroyedAddCameraBounds);
            Triggers.TriggerEvent(TriggerKey.ActivateEndGameExplosion);
            Triggers.TriggerEvent(TriggerKey.CoreOpenArena);
        }
    }
}
