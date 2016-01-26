using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Waypoint;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Enemy
{
    public enum GrenadierDifficulty
    {
        Normal,
        Hard,
    }

    class Grenadier : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int START_HEALTH_NORMAL = Config.GRENADIER_START_HEALTH_NORMAL;
        private const int START_HEALTH_HARD = Config.GRENADIER_START_HEALTH_HARD;
        private const float MASS = Config.GRENADIER_MASS;
        private const float MAX_SPEED = Config.GRENADIER_MAX_HORIZONTAL_SPEED;
        private const int HORIZONTAL_ATTACK_THRESHOLD = Config.GRENADIER_HORIZONTAL_ATTACK_THRESHOLD;
        private const int VERTICAL_ATTACK_THRESHOLD_NORMAL = Config.GRENADIER_VERTICAL_ATTACK_THRESHOLD_NORMAL;
        private const int VERTICAL_ATTACK_THRESHOLD_HARD = Config.GRENADIER_VERTICAL_ATTACK_THRESHOLD_HARD;

        private const int PROJECTILE_BLAST_DAMAGE = Config.GRENADIER_PROJECTILE_BLAST_DAMAGE;
        private const int PROJECTILE_BLAST_RADIUS = Config.GRENADIER_PROJECTILE_BLAST_RADIUS;
        private const float PROJECTILE_LAUNCH_SPEED = Config.GRENADIER_PROJECTILE_LAUNCH_SPEED;
        private const float PROJECTILE_MASS = Config.GRENADIER_PROJECTILE_MASS;
        private const float PROJECTILE_ROTATION_INCREMENT = Config.GRENADIER_PROJECTILE_ROTATION_INCREMENT;
        private const float PROJECTILE_GRAVITY_FORCE = Config.ENEMY_PROJECTILE_GRAVITY_FORCE;

        private static readonly Vector2 LEFT_SIDE_FIRE_DIRECTION = new Vector2(-1f, -1.25f);
        private static readonly Vector2 RIGHT_SIDE_FIRE_DIRECTION = new Vector2(1f, -1.25f);

        private static readonly double[] LEFT_SIDE_WEAPON_DELAYS_NORMAL = new double[] { 4000 };
        private static readonly double[] RIGHT_SIDE_WEAPON_DELAYS_NORMAL = new double[] { 4000 };
        private static readonly int[] LEFT_SIDE_WEAPON_BURST_AMOUNTS_NORMAL = new int[] { 2 };
        private static readonly int[] RIGHT_SIDE_WEAPON_BURST_AMOUNTS_NORMAL = new int[] { 2 };
        //private static readonly int[] LEFT_SIDE_WEAPON_BURST_AMOUNTS_NORMAL = new int[] { 1 };
        //private static readonly int[] RIGHT_SIDE_WEAPON_BURST_AMOUNTS_NORMAL = new int[] { 1 };
        private const double BURST_DELAY_NORMAL = 500d;

        private static readonly double[] LEFT_SIDE_WEAPON_DELAYS_HARD = new double[] { 1000, 3100 };
        private static readonly double[] RIGHT_SIDE_WEAPON_DELAYS_HARD = new double[] { 1000, 3100 };
        private static readonly int[] LEFT_SIDE_WEAPON_BURST_AMOUNTS_HARD = new int[] { 2, 3 };
        private static readonly int[] RIGHT_SIDE_WEAPON_BURST_AMOUNTS_HARD = new int[] { 2, 3 };
        private const double BURST_DELAY_HARD = 200d;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private WaypointIterator waypointIterator;

        private AbstractWeapon leftSideWeapon;
        private AbstractWeapon rightSideWeapon;

        private SpriteKey explosionSpriteKey;

        private int verticalAttackThreshold;
        
        // Properties ------------------------------------------------------------------------------------- Properties
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Grenadier(Vector2 position, string name, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode, MapWaypoint[] waypoints, bool leftSideWeaponEnable, bool rightSideWeaponEnable, GrenadierDifficulty difficulty)
            : base(position, PhysicsMode.Gravity, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode)
        {
            this.mass = MASS;

            if (waypoints.Length != 2)
            {
                throw new InvalidOperationException("A Grenadier must have 2 waypoints.");
            }
            this.waypointIterator = new ForwardAndBackwardWaypointIterator(waypoints);
            MapWaypoint currentWaypoint = this.waypointIterator.Current();

            double[] leftSideWeaponDelays;
            double[] rightSideWeaponDelays;
            int[] leftSideBurstAmounts;
            int[] rightSideBurstAmounts;
            double burstDelay;

            if (difficulty == GrenadierDifficulty.Normal)
            {
                this.health = START_HEALTH_NORMAL;
                this.pointValue = START_HEALTH_NORMAL;
                this.sprite = new Sprite(SpriteKey.Grenadier);
                this.explosionSpriteKey = SpriteKey.GrenadierExplosion;

                this.verticalAttackThreshold = VERTICAL_ATTACK_THRESHOLD_NORMAL;

                leftSideWeaponDelays = LEFT_SIDE_WEAPON_DELAYS_NORMAL;
                rightSideWeaponDelays = RIGHT_SIDE_WEAPON_DELAYS_NORMAL;
                leftSideBurstAmounts = LEFT_SIDE_WEAPON_BURST_AMOUNTS_NORMAL;
                rightSideBurstAmounts = RIGHT_SIDE_WEAPON_BURST_AMOUNTS_NORMAL;
                burstDelay = BURST_DELAY_NORMAL;
            }
            else
            {
                this.health = START_HEALTH_HARD;
                this.pointValue = START_HEALTH_HARD;
                this.sprite = new Sprite(SpriteKey.HardGrenadier);
                this.explosionSpriteKey = SpriteKey.HardGrenadierExplosion;

                this.verticalAttackThreshold = VERTICAL_ATTACK_THRESHOLD_HARD;

                leftSideWeaponDelays = LEFT_SIDE_WEAPON_DELAYS_HARD;
                rightSideWeaponDelays = RIGHT_SIDE_WEAPON_DELAYS_HARD;
                leftSideBurstAmounts = LEFT_SIDE_WEAPON_BURST_AMOUNTS_HARD;
                rightSideBurstAmounts = RIGHT_SIDE_WEAPON_BURST_AMOUNTS_HARD;
                burstDelay = BURST_DELAY_HARD;
            }


            DelayLogic leftWeaponDelayLogic = new SequentialDelayLogic(leftSideWeaponDelays);
            ProjectileFactory leftWeaponGrenadeFactory = new ExplosiveProjectileFactory(PROJECTILE_BLAST_DAMAGE, PROJECTILE_BLAST_RADIUS, PROJECTILE_MASS, PROJECTILE_GRAVITY_FORCE,
                                                                                        PROJECTILE_ROTATION_INCREMENT, SpriteKey.GrenadierProjectile, ProjectileSource.Enemy);
            FireLogic leftWeaponFireLogic = new BurstFireLogic(false, burstDelay, leftSideBurstAmounts);
            this.leftSideWeapon = new ProjectileLauncher(leftWeaponGrenadeFactory, leftWeaponDelayLogic, leftWeaponFireLogic, PROJECTILE_LAUNCH_SPEED, SoundKey.EnemyFire, SoundKey.FireNoAmmo);

            DelayLogic rightWeaponDelayLogic = new SequentialDelayLogic(rightSideWeaponDelays);
            ProjectileFactory rightWeaponGrenadeFactory = new ExplosiveProjectileFactory(PROJECTILE_BLAST_DAMAGE, PROJECTILE_BLAST_RADIUS, PROJECTILE_MASS, PROJECTILE_GRAVITY_FORCE,
                                                                                         PROJECTILE_ROTATION_INCREMENT, SpriteKey.GrenadierProjectile, ProjectileSource.Enemy);
            FireLogic rightWeaponFireLogic = new BurstFireLogic(false, burstDelay, rightSideBurstAmounts);
            this.rightSideWeapon = new ProjectileLauncher(rightWeaponGrenadeFactory, rightWeaponDelayLogic, rightWeaponFireLogic, PROJECTILE_LAUNCH_SPEED, SoundKey.EnemyFire, SoundKey.FireNoAmmo);

            this.leftSideWeapon.Enabled = leftSideWeaponEnable;
            this.rightSideWeapon.Enabled = rightSideWeaponEnable;
            
            // Start moving toward first waypoint
            Vector2 direction = currentWaypoint.DirectionToWaypoint(this.position);
            this.velocity = direction * MAX_SPEED;
        }

        public Grenadier(Vector2 position, string name, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode, MapWaypoint[] waypoints)
            : this(position, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode, waypoints, true, true, GrenadierDifficulty.Normal)
        { }

        public Grenadier(Vector2 position, string name, MapWaypoint[] waypoints)
            : this(position, name, null, PhysicsMode.None, waypoints)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        public override void PerformAI(Player player)
        {
            // If we have passed the current waypoint, swich waypoints and direction.
            if (this.waypointIterator.Current().PassedWaypoint(this.position, Vector2.Normalize(this.velocity)))
            {
                this.waypointIterator.Next();
                this.velocity.X *= -1;

                DirectionOrientateHorizontal();
            }

            this.leftSideWeapon.Position = this.position + LEFT_SIDE_FIRE_DIRECTION * 40f;
            this.rightSideWeapon.Position = this.position + RIGHT_SIDE_FIRE_DIRECTION * 40f;

            // Figure out if we should fire at the player.
            float horizontalDifference = player.Position.X - this.position.X;
            float verticalDifference = player.Position.Y - this.position.Y;
                        
            if (Math.Abs(verticalDifference) < this.verticalAttackThreshold)
            {
                if (horizontalDifference < 0 && Math.Abs(horizontalDifference) < HORIZONTAL_ATTACK_THRESHOLD)
                {
                    FireWeapon(this.leftSideWeapon, LEFT_SIDE_FIRE_DIRECTION);
                }

                if (horizontalDifference > 0 && Math.Abs(horizontalDifference) < HORIZONTAL_ATTACK_THRESHOLD)
                {
                    FireWeapon(this.rightSideWeapon, RIGHT_SIDE_FIRE_DIRECTION);
                }
            }
        }

        public override void Remove()
        {
            this.leftSideWeapon.FireLogic.Interrupt();
            this.rightSideWeapon.FireLogic.Interrupt();
            base.Remove();
        }
        
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnDeath()
        {
            Explode(this.explosionSpriteKey, SpriteSheetFactory.GRENADIER_EXPLOSION_TILES_X, SpriteSheetFactory.GRENADIER_EXPLOSION_TILES_Y);
            base.OnDeath();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void FireWeapon(AbstractWeapon weapon, Vector2 fireDirection)
        {
            Vector2 normalizedFireDirection = Vector2.Normalize(fireDirection);

            weapon.Direction = normalizedFireDirection;
            weapon.Position = this.position + (normalizedFireDirection * 40);

            weapon.TryFire(-1);
        }
    }
}
