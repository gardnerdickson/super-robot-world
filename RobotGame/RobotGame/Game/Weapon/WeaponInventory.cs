using System;
using System.Collections.Generic;
using RobotGame.Engine;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Weapon
{
    public enum InventoryState
    {
        Unavailable,
        Available,
        Selected
    }

    public class WeaponInventory
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        public static readonly int GRENADE_LAUNCHER_INDEX = 0;
        public static readonly int HOMING_MISSILE_LAUNCHER_INDEX = 1;

        private static readonly int NUM_WEAPONS = 2;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private int ammo;
        private InventoryWeapon[] weaponList;

        // Properties ------------------------------------------------------------------------------------- Properties

        public int Ammo
        {
            get { return this.ammo; }
            set { this.ammo = Math.Min(value, Config.PLAYER_SECONDARY_AMMO_MAX); }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public WeaponInventory()
        {
            this.weaponList = new InventoryWeapon[NUM_WEAPONS];
            this.ammo = 0;
            for (int i = 0; i < NUM_WEAPONS; i++)
            {
                this.weaponList[i] = new InventoryWeapon(null, InventoryState.Unavailable);
            }
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public InventoryState GetState(int weaponIndex)
        {
            return weaponList[weaponIndex].InventoryState;
        }

        public void SetWeapon(int weaponIndex, AbstractWeapon weapon)
        {
            this.weaponList[weaponIndex].Weapon = weapon;
        }

        public void SetState(int weaponIndex, InventoryState weaponState)
        {
            if (weaponState == InventoryState.Selected)
            {
                foreach (InventoryWeapon inventoryWeapon in weaponList)
                {
                    if (inventoryWeapon.InventoryState == InventoryState.Selected)
                    {
                        inventoryWeapon.InventoryState = InventoryState.Available;
                    }
                }
            }

            weaponList[weaponIndex].InventoryState = weaponState;
        }

        public void CycleSelectedWeapon()
        {
            int selectedIndex = GetSelectedWeaponIndex();
            if (selectedIndex == -1)
            {
                return;
            }

            SetState(selectedIndex, InventoryState.Available);
            int newIndex = selectedIndex;
            bool foundAvailableWeapon = false;
            while (!foundAvailableWeapon)
            {
                newIndex = (newIndex + 1) % NUM_WEAPONS;
                if (this.weaponList[newIndex].InventoryState == InventoryState.Available)
                {
                    // If the current selected weapon is disabled, transfer the remaining timer delay information to the new selected weapon
                    if (!(this.weaponList[selectedIndex].Weapon.Enabled))
                    {
                        this.weaponList[newIndex].Weapon.Enabled = false;
                        TimerManager.GetInstance().RegisterTimer(this.weaponList[selectedIndex].Weapon.GetRemainingDelayTime(), this.weaponList[newIndex].Weapon.TimerNotify, null);
                    }
                    SetState(newIndex, InventoryState.Selected);
                    foundAvailableWeapon = true;
                }
            }
        }

        public AbstractWeapon GetSelectedWeapon()
        {
            foreach (InventoryWeapon inventoryWeapon in weaponList)
            {
                if (inventoryWeapon.InventoryState == InventoryState.Selected)
                {
                    return inventoryWeapon.Weapon;
                }
            }
            return null;
        }

        public AbstractWeapon GetWeapon(int weaponIndex)
        {
            return this.weaponList[weaponIndex].Weapon;
        }

        public int GetSelectedWeaponIndex()
        {
            AbstractWeapon selectedWeapon = GetSelectedWeapon();
            foreach (InventoryWeapon inventoryWeapon in this.weaponList)
            {
                if (inventoryWeapon.Weapon != null && inventoryWeapon.Weapon.Equals(selectedWeapon))
                {
                    return Array.IndexOf<InventoryWeapon>(this.weaponList, inventoryWeapon);
                }
            }
            return -1;
        }

        public void FireSelectedWeapon(Vector2 launchPosition, Vector2 launchDirection)
        {
            AbstractWeapon weapon = GetSelectedWeapon();
            if (weapon != null)
            {
                weapon.Position = launchPosition;
                weapon.Direction = launchDirection;

                int ammoUsed = weapon.TryFire(this.ammo);
                this.ammo -= ammoUsed;
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        // Inner Classes ------------------------------------------------------------------------------- Inner Classes

        class InventoryWeapon
        {
            private AbstractWeapon weapon;
            private InventoryState inventoryState;

            public AbstractWeapon Weapon
            {
                get { return this.weapon; }
                set { this.weapon = value; }
            }

            public InventoryState InventoryState
            {
                get { return this.inventoryState; }
                set { this.inventoryState = value; }
            }

            public InventoryWeapon(AbstractWeapon weapon, InventoryState inventoryState)
            {
                this.weapon = weapon;
                this.inventoryState = inventoryState;
            }
        }

    } // end of class
}
