using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RobotGame.Game.Audio;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    public abstract class FireLogic
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected bool consumeAmmo = true;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public FireLogic(/*ProjectileFactory projectileFactory,*/ bool consumeAmmo)
        {
            this.consumeAmmo = consumeAmmo;
        }

        public FireLogic()
            : this(true)
        { }
        
        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public abstract int Fire(WeaponFireCallback fireCallback , int ammo);

        public abstract int Fire(WeaponFireCallback fireCallback);

        public virtual void Interrupt() { }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

    }
}
