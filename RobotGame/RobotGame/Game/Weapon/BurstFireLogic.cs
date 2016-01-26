using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Weapon
{
    public class BurstFireLogic : FireLogic
    {
        struct BurstFireCallbackParams
        {
            public WeaponFireCallback FireCallback;
            public int Ammo;
            public int BurstAmount;

            public BurstFireCallbackParams(WeaponFireCallback fireCallback, int ammo, int burstAmount)
            {
                this.FireCallback = fireCallback;
                this.Ammo = ammo;
                this.BurstAmount = burstAmount;
            }
        }

        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback FireDelayCallback;

        private double burstDelay;
        private int burstAmountIndex = 0;
        private int[] burstAmounts;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public BurstFireLogic(bool consumeAmmo, double burstDelay, int[] burstAmounts)
            :  base(consumeAmmo)
        {
            this.burstDelay = burstDelay;
            this.burstAmounts = burstAmounts;

            this.FireDelayCallback += new TimerCallback(fire_callback);
        }

        public BurstFireLogic(double burstDelay, int[] burstAmounts)            
            : this(true, burstDelay, burstAmounts)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override int Fire(WeaponFireCallback fireCallback, int ammo)
        {
            if (this.burstAmountIndex >= this.burstAmounts.Length)
            {
                this.burstAmountIndex = 0;
            }

            TimerManager.GetInstance().RegisterTimer(0.0d, this.FireDelayCallback, new BurstFireCallbackParams(fireCallback, ammo, this.burstAmounts[this.burstAmountIndex]));

            int ammoUsed;
            if (this.burstAmounts[this.burstAmountIndex] > ammo)
            {
                ammoUsed = ammo;
            }
            else
            {
                ammoUsed = this.burstAmounts[this.burstAmountIndex];
            }

            this.burstAmountIndex++;
            return ammoUsed;
        }

        public override int Fire(WeaponFireCallback fireCallback)
        {
            return this.Fire(fireCallback, -1);
        }

        public override void Interrupt()
        {
            TimerManager.GetInstance().UnregisterTimer(this.FireDelayCallback);
            this.burstAmountIndex = 0;
            base.Interrupt();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void fire_callback(Object param)
        {
            WeaponFireCallback fireCallback = ((BurstFireCallbackParams)param).FireCallback;
            int ammo = ((BurstFireCallbackParams)param).Ammo;
            int localBurstAmount = ((BurstFireCallbackParams)param).BurstAmount;

            --localBurstAmount;
            --ammo;
            if (localBurstAmount >= 0)
            {
                if (!consumeAmmo || (consumeAmmo && ammo >= 0))
                {
                    fireCallback();
                    TimerManager.GetInstance().RegisterTimer(this.burstDelay, this.FireDelayCallback, new BurstFireCallbackParams(fireCallback, ammo, localBurstAmount));
                }
            }
        }
    }
}
