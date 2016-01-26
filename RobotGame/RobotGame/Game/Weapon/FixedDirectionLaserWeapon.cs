using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    class FixedDirectionLaserWeapon : LaserWeapon
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback LifetimeCallback;
        private double laserLifetime;
        
        // Properties ------------------------------------------------------------------------------------- Properties
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        public FixedDirectionLaserWeapon(int damage, DelayLogic delayLogic, double laserLifetime, bool collideWithMovers)
            : base(damage, delayLogic, null, collideWithMovers)
        {
            this.LifetimeCallback = new TimerCallback(lifetime_callback);

            this.laserLifetime = laserLifetime;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override int TryFire(int ammo)
        {
            if (this.enabled)
            {
                if (!TimerManager.GetInstance().IsTimerRegistered(this.LifetimeCallback))
                {
                    TimerManager.GetInstance().RegisterTimer(this.laserLifetime, this.LifetimeCallback, null);
                }

                if (this.laser == null)
                {
                    this.laser = (FixedDirectionLaser)this.projectileFactory.CreateProjectile(this.position, this.direction);
                }
                else
                {
                    this.laser.Enabled = true;
                }
            }

            return -1;
        }

        public override object Clone()
        {
            return new FixedDirectionLaserWeapon(((FixedDirectionLaserFactory)this.projectileFactory).Damage, this.delayLogic, this.laserLifetime, ((FixedDirectionLaserFactory)this.projectileFactory).CollideWithMovers);
        }
        
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void lifetime_callback(Object param)
        {
            this.enabled = false;

            if (this.laser != null)
            {
                this.laser.Enabled = false;

                this.delayLogic.Delay(this.TimerNotify);
            }
        }
    }
}
