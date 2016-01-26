using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;

namespace RobotGame.Game.Volume
{
    class EndGameExplosionVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float EXPLOSION_INTERVAL = 250f;
        private const int WIDTH_RANDOM_RANGE = 50;
        private const int BLAST_DAMAGE = 1000;
        private const int BLAST_RADIUS = 200;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback EnabledCallback;

        private static Random random = new Random();

        private bool enabled;
        private int xPos;
        private float startDelay;

        // Properties ------------------------------------------------------------------------------------- Properties
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        public EndGameExplosionVolume(Rectangle bounds, float startDelay)
            : base(bounds)
        {
            this.enabled = false;
            this.xPos = this.bounds.Right;
            this.startDelay = startDelay;

            this.EnabledCallback = new TimerCallback(enabled_callback);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Activate()
        {
            if (this.startDelay > 0)
            {
                TimerManager.GetInstance().RegisterTimer(this.startDelay, this.EnabledCallback, null);
            }
            else
            {
                this.enabled = true;
            }
        }

        public override void Update()
        {
            if (this.enabled)
            {
                if (this.xPos < this.bounds.Left)
                {
                    TimerManager.GetInstance().UnregisterTimer(this.EnabledCallback);
                    this.enabled = false;
                }

                int x = random.Next(xPos - WIDTH_RANDOM_RANGE, xPos);
                int y = random.Next(bounds.Top, bounds.Bottom);

                Vector2 position = new Vector2(x, y);

                new Blast(position, Rectangle.Empty, SpriteKey.Explosion, SpriteKey.ExplosionSmoke, BLAST_DAMAGE, new Circle(position, BLAST_RADIUS));

                this.enabled = false;

                TimerManager.GetInstance().RegisterTimer(EXPLOSION_INTERVAL, this.EnabledCallback, null);

                this.xPos -= 75;
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void enabled_callback(Object param)
        {
            this.enabled = true;
        }
    }
}
