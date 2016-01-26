using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Weapon
{
    class ProjectileLauncher : AbstractWeapon
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int GRENADE_BLAST_RADIUS = Config.PLAYER_GRENADE_BLAST_RADIUS;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors
        
        public ProjectileLauncher(ProjectileFactory projectileFactory, DelayLogic delayLogic, FireLogic fireLogic, float launchSpeed, SoundKey fireSoundEffect, SoundKey noAmmoSoundEffect)
            : base(projectileFactory, delayLogic, fireLogic, launchSpeed)
        {
            this.FireCallback = new WeaponFireCallback(fire_callback);

            this.fireSoundEffect = fireSoundEffect;
            this.noAmmoSoundEffect = noAmmoSoundEffect;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override int TryFire(int ammo)
        {
            int ammoUsed = 0;

            if (this.enabled)
            {
                if (ammo == -1 || ammo > 0)
                {
                    this.enabled = false;

                    ammoUsed = this.fireLogic.Fire(this.FireCallback, ammo);
                    this.delayLogic.Delay(this.TimerNotify);
                }
                else
                {
                    SoundManager.PlaySoundEffect(this.noAmmoSoundEffect);
                }
            }

            return ammoUsed;
        }

        public override object Clone()
        {
            return new ProjectileLauncher(this.projectileFactory, this.delayLogic, this.fireLogic, this.launchSpeed, this.fireSoundEffect, this.noAmmoSoundEffect);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void fire_callback()
        {
            this.projectileFactory.CreateProjectile(this.position, this.direction * this.launchSpeed);
            SoundManager.PlaySoundEffect(this.fireSoundEffect);
        }
    }
}
