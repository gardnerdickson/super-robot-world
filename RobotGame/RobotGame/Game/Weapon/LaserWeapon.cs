using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Weapon
{
    abstract class LaserWeapon : AbstractWeapon
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected Laser laser;

        // Properties ------------------------------------------------------------------------------------- Properties

        public override Vector2 Position
        {
            set
            {
                if (this.laser != null)
                {
                    this.laser.Position = value;
                }

                base.Position = value;
            }
        }

        public override Vector2 Direction
        {
            set
            {
                if (this.laser != null)
                {
                    this.laser.Direction = value;
                }

                base.Direction = value;
            }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public LaserWeapon(int damage, DelayLogic delayLogic, FireLogic fireLogic, bool collideWithMovers)
            : base(null, delayLogic, fireLogic, 0f)
        {
            if (this is TrackingLaserWeapon)
            {
                this.projectileFactory = new TrackingLaserFactory(damage, collideWithMovers);
            }
            else
            {
                this.projectileFactory = new FixedDirectionLaserFactory(damage, collideWithMovers);
            }
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Stop()
        {
            if (this.laser != null)
            {
                this.laser.Remove();
                this.laser = null;
            }
        }
        
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
