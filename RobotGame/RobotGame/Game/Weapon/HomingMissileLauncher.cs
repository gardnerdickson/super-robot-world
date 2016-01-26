using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Weapon
{
    class HomingMissileLauncher : AbstractWeapon
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int HOMING_MISSILE_BLAST_RADIUS = Config.HOMING_MISSILE_BLAST_RADIUS;
        private const int HOMING_MISSILE_TARGET_RADIUS = Config.HOMING_MISSILE_TARGET_RADIUS;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private HomingMissile homingMissile;

        private SoundKey seekModeSoundEffect;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public HomingMissileLauncher(DelayLogic fireDelayLogic, ProjectileFactory projectileFactory, float launchSpeed, SoundKey fireSoundEffect, SoundKey noAmmoSoundEffect, SoundKey seekModeSoundEffect)
            : base(projectileFactory, fireDelayLogic, null, launchSpeed)
        {
            this.projectileFactory = projectileFactory;
            this.fireSoundEffect = fireSoundEffect;
            this.noAmmoSoundEffect = noAmmoSoundEffect;
            this.seekModeSoundEffect = seekModeSoundEffect;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override int TryFire(int ammo)
        {
            if (this.homingMissile != null && !this.homingMissile.Dead && this.homingMissile.HomingMissilePhase == HomingMissilePhase.Launch)
            {
                if (this.homingMissile.HasTarget())
                {
                    this.homingMissile.HomingMissilePhase = HomingMissilePhase.Seek;
                    SoundManager.PlayAndLoopSoundEffect(seekModeSoundEffect);
                }

                //Return ammo used
                return 0;
            }


            if (this.enabled && ammo > 0)
            {
                this.enabled = false;
                this.delayLogic.Delay(this.TimerNotify);

                if (this.homingMissile == null || this.homingMissile.Dead || this.homingMissile.HomingMissilePhase == HomingMissilePhase.Seek)
                {
                    this.homingMissile = (HomingMissile)this.projectileFactory.CreateProjectile(this.position, this.direction * this.launchSpeed);
                    SoundManager.PlaySoundEffect(fireSoundEffect);
                }

                // Return ammo used
                return 1;
            }

            SoundManager.PlaySoundEffect(noAmmoSoundEffect);

            // Return ammo used
            return 0;
        }

        public override object Clone()
        {
            return new HomingMissileLauncher(this.delayLogic, this.projectileFactory, this.launchSpeed, this.fireSoundEffect, this.noAmmoSoundEffect, this.seekModeSoundEffect);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
