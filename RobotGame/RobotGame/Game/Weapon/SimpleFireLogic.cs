using System;
using Microsoft.Xna.Framework;
using RobotGame.Game.Audio;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    class SimpleFireLogic : FireLogic
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SimpleFireLogic(bool consumeAmmo)
            : base(consumeAmmo)
        { }

        public SimpleFireLogic()
            : base(true)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override int Fire(WeaponFireCallback fireCallback, int ammo)
        {
            fireCallback();
            return 1;
        }

        public override int Fire(WeaponFireCallback fireCallback)
        {
            return this.Fire(fireCallback, -1);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
