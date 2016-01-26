using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;
using RobotGame.Game.Waypoint;

namespace RobotGame.Game.Enemy
{
    public enum CrawlerFireMode
    {
        DropBomb,
        TargetBomb,
        FireBullet,
        TargetBullet,
    }

    class Crawler : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int START_HEALTH = Config.CRAWLER_START_HEALTH;
        private const int SPEED = Config.CRAWLER_SPEED;
        private const int ATTACK_RANGE_SHORT = Config.CRAWLER_ATTACK_RANGE_SHORT;
        private const int ATTACK_RANGE_LONG = Config.CRAWLER_ATTACK_RANGE_LONG;

        private const float FIRE_DELAY = Config.CRAWLER_FIRE_DELAY;
        private const int PROJECTILE_BLAST_DAMAGE = Config.CRAWLER_PROJECTILE_BLAST_DAMAGE;
        private const int PROJECTILE_BLAST_RADIUS = Config.CRAWLER_PROJECTILE_BLAST_RADIUS;
        private const int PROJECTILE_BULLET_DAMAGE = Config.CRAWLER_PROJECTILE_BULLET_DAMAGE;
        private const float PROJECTILE_BULLET_SPEED = Config.CRAWLER_PROJECTILE_BULLET_SPEED;
        private const float PROJECTILE_LAUNCH_SPEED = Config.CRAWLER_PROJECTILE_LAUNCH_SPEED;
        private const float PROJECTILE_ROTATION_INCREMENT = Config.CRAWLER_PROJECTILE_ROTATION_INCREMENT;
        private const float PROJECTILE_MASS = Config.CRAWLER_PROJECTILE_MASS;
        private const float PROJECTILE_GRAVITY_FORCE = Config.ENEMY_PROJECTILE_GRAVITY_FORCE;

        private const float NOZZLE_RANGE = 150f;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback EnabledCallback;

        private bool enabled;

        private float speed;

        private WaypointIterator waypointIterator;

        private AbstractWeapon weapon;
        private CrawlerFireMode fireMode;
        private EnemyOrientation orientation;

        private int attackRangeX;
        private int attackRangeY;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Crawler(Vector2 position, string name, float speed, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode, MapWaypoint[] waypoints, CrawlerFireMode fireMode, EnemyOrientation orientation)
            : base(position, PhysicsMode.None, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode)
        {
            this.health = START_HEALTH;
            this.pointValue = START_HEALTH;
            this.fireMode = fireMode;
            this.orientation = orientation;
            this.speed = speed;
            this.enabled = false;

            if (waypoints.Length != 2)
            {
                throw new InvalidOperationException("A Crawler must have 2 waypoints.");
            }
            this.waypointIterator = new ForwardAndBackwardWaypointIterator(waypoints);

            FireLogic simpleFireLogic = new SimpleFireLogic(false);
            if (this.fireMode == CrawlerFireMode.DropBomb)
            {
                DelayLogic simpleDelayLogic = new SimpleDelayLogic(FIRE_DELAY);
                ProjectileFactory projectileFactory = new ExplosiveProjectileFactory(PROJECTILE_BLAST_DAMAGE, PROJECTILE_BLAST_RADIUS, PROJECTILE_MASS, PROJECTILE_GRAVITY_FORCE, PROJECTILE_ROTATION_INCREMENT, SpriteKey.CrawlerBombProjectile, ProjectileSource.Enemy);
                this.weapon = new ProjectileLauncher(projectileFactory, simpleDelayLogic, simpleFireLogic, 0f, SoundKey.EnemyFire, SoundKey.None);
            }
            else if (this.fireMode == CrawlerFireMode.TargetBomb)
            {
                DelayLogic simpleDelayLogic = new SimpleDelayLogic(2000);
                ProjectileFactory projectileFactory = new ExplosiveProjectileFactory(PROJECTILE_BLAST_DAMAGE, PROJECTILE_BLAST_RADIUS, 0f, 0f, PROJECTILE_ROTATION_INCREMENT, SpriteKey.CrawlerSpiralProjectile, ProjectileSource.Enemy);
                this.weapon = new ProjectileLauncher(projectileFactory, simpleDelayLogic, simpleFireLogic, PROJECTILE_LAUNCH_SPEED, SoundKey.EnemyFire, SoundKey.None);
            }
            else
            {
                FireLogic burstFireLogic = new BurstFireLogic(false, 200d, new int[] { 2 });
                float fireDelay;
                if (this.fireMode == CrawlerFireMode.FireBullet)
                {
                    fireDelay = 1000f;
                }
                else
                {
                    fireDelay = 2000f;
                }
                DelayLogic simpleDelayLogic = new SimpleDelayLogic(fireDelay);
                ProjectileFactory projectileFactory = new BulletFactory(PROJECTILE_BULLET_DAMAGE, SpriteKey.EasyPawnProjectile, SpriteKey.EnemyBulletCollisionParticle, null, ProjectileSource.Enemy);
                this.weapon = new ProjectileLauncher(projectileFactory, simpleDelayLogic, burstFireLogic, PROJECTILE_BULLET_SPEED, SoundKey.EnemyFire, SoundKey.FireNoAmmo);
            }


            if (this.orientation == EnemyOrientation.Up)
            {
                this.sprite = new Sprite(SpriteKey.CrawlerUp);
                this.weapon.Direction = new Vector2(0, -1);

                if (this.fireMode == CrawlerFireMode.DropBomb || this.fireMode == CrawlerFireMode.FireBullet)
                {
                    this.attackRangeX = ATTACK_RANGE_SHORT;
                }
                else
                {
                    this.attackRangeX = ATTACK_RANGE_LONG;
                }
                this.attackRangeY = ATTACK_RANGE_LONG;
            }
            else if (this.orientation == EnemyOrientation.Down)
            { 
                this.sprite = new Sprite(SpriteKey.CrawlerDown);
                this.weapon.Direction = new Vector2(0, 1);

                if (this.fireMode == CrawlerFireMode.DropBomb || this.fireMode == CrawlerFireMode.FireBullet)
                {
                    this.attackRangeX = ATTACK_RANGE_SHORT;
                }
                else
                {
                    this.attackRangeX = ATTACK_RANGE_LONG;
                }
                this.attackRangeY = ATTACK_RANGE_LONG;
            }
            else if (this.orientation == EnemyOrientation.Left)
            {
                this.sprite = new Sprite(SpriteKey.CrawlerLeft);
                this.weapon.Direction = new Vector2(-1, 0);

                this.attackRangeX = ATTACK_RANGE_LONG;
                if (this.fireMode == CrawlerFireMode.DropBomb || this.fireMode == CrawlerFireMode.FireBullet)
                {
                    this.attackRangeY = ATTACK_RANGE_SHORT;
                }
                else
                {
                    this.attackRangeY = ATTACK_RANGE_LONG;
                }
            }
            else
            {
                this.sprite = new Sprite(SpriteKey.CrawlerRight);
                this.weapon.Direction = new Vector2(1, 0);

                this.attackRangeX = ATTACK_RANGE_LONG;
                if (this.fireMode == CrawlerFireMode.DropBomb || this.fireMode == CrawlerFireMode.FireBullet)
                {
                    this.attackRangeY = ATTACK_RANGE_SHORT;
                }
                else
                {
                    this.attackRangeY = ATTACK_RANGE_LONG;
                }
            }

            Vector2 direction = this.waypointIterator.Current().DirectionToWaypoint(this.position);
            this.velocity = direction * this.speed;

            this.EnabledCallback = new TimerCallback(enabled_callback);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void PerformAI(Player player)
        {
            if (this.enabled)
            {

                // If we have passed the current waypoint, swich waypoints and direction.
                if (this.waypointIterator.Current().PassedWaypoint(this.position, Vector2.Normalize(this.velocity)))
                {
                    this.waypointIterator.Next();
                    if (this.waypointIterator.Current().Orientation == WaypointOrientation.Horizontal)
                    {
                        this.velocity.X *= -1;
                        DirectionOrientateHorizontal();
                    }
                    else if (this.waypointIterator.Current().Orientation == WaypointOrientation.Vertical)
                    {
                        this.velocity.Y *= -1;
                        DirectionOrientateVertical();
                    }
                }


                // Figure out if we should fire at the player
                Vector2 vectorToTarget = player.Position - this.position;
                if (Math.Abs(vectorToTarget.Y) < attackRangeY && Math.Abs(vectorToTarget.X) < attackRangeX)
                {
                    if (this.fireMode == CrawlerFireMode.DropBomb || this.fireMode == CrawlerFireMode.FireBullet)
                    {
                        FireWeapon();
                    }
                    else
                    {
                        Vector2 directionVector = Vector2.Normalize(vectorToTarget);
                        if (IsVectorInRange(directionVector, NOZZLE_RANGE, this.orientation))
                        {
                            this.weapon.Direction = directionVector;
                            FireWeapon();
                        }
                    }
                }
            }
            else
            {
                TimerManager.GetInstance().RegisterTimer(500, this.EnabledCallback, null);
            }
        }

        public override void Remove()
        {
            this.weapon.FireLogic.Interrupt();
            base.Remove();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnDeath()
        {
            Explode(SpriteKey.CrawlerExplosion, SpriteSheetFactory.CRAWLER_EXPLOSION_TILES_X, SpriteSheetFactory.CRAWLER_EXPLOSION_TILES_Y);
            base.OnDeath();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void FireWeapon()
        {
            this.weapon.Position = this.position + (this.weapon.Direction * 30);
            this.weapon.TryFire(-1);
        }

        private void enabled_callback(Object param)
        {
            this.enabled = true;
        }
    }
}
