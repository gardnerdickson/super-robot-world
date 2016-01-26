using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Weapon
{
    public abstract class AbstractWeapon
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        public TimerCallback TimerNotify;

        protected WeaponFireCallback FireCallback;

        protected ProjectileFactory projectileFactory;

        protected SoundKey fireSoundEffect;
        protected SoundKey noAmmoSoundEffect;

        protected DelayLogic delayLogic;
        protected FireLogic fireLogic;

        protected Vector2 position;
        protected Vector2 direction;
        protected float launchSpeed;

        protected bool enabled;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        public ProjectileFactory ProjectileFactory
        {
            get { return this.projectileFactory; }
            set { this.projectileFactory = value; }
        }

        public DelayLogic DelayLogic
        {
            get { return this.delayLogic; }
            set { this.delayLogic = value; }
        }

        public FireLogic FireLogic
        {
            get { return this.fireLogic; }
            set { this.fireLogic = value; }
        }

        public virtual Vector2 Position
        {
            set { this.position = value; }
            get { return this.position; }
        }

        public virtual Vector2 Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        public float LaunchSpeed
        {
            get { return this.launchSpeed; }
            set { this.launchSpeed = value; }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors
        
        public AbstractWeapon(ProjectileFactory projectileFactory, DelayLogic delayLogic, FireLogic fireLogic, float launchSpeed)
        {
            this.projectileFactory = projectileFactory;
            this.delayLogic = delayLogic;
            this.fireLogic = fireLogic;
            this.launchSpeed = launchSpeed;

            this.enabled = true;

            this.TimerNotify = new TimerCallback(timer_enable);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public abstract int TryFire(int ammo);

        public double GetRemainingDelayTime()
        {
            return TimerManager.GetInstance().GetRemainingTime(this.TimerNotify);
        }

        public abstract object Clone();

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void timer_enable(Object param)
        {
            this.enabled = true;
        }
    }
}
