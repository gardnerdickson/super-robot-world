using System;
using RobotGame.Game.Waypoint;
using RobotGame.Game.Weapon;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame.Game.Enemy
{
    abstract class Turret : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        protected const float NOZZLE_DRAW_DEPTH = Config.NOZZLE_DRAW_DEPTH;

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected Sprite nozzleSprite;

        protected WaypointIterator waypointIterator;

        protected AbstractWeapon laserWeapon;

        protected EnemyOrientation orientation;
        protected float speed;
        protected bool invincible;

        protected Vector2 fireDirection;

        protected Vector2 laserOffset;

        // Properties ------------------------------------------------------------------------------------- Properties

        public virtual EnemyOrientation Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                this.orientation = value;
                if (orientation == EnemyOrientation.Up)
                {
                    this.fireDirection = new Vector2(0, -1);
                }
                else if (orientation == EnemyOrientation.Down)
                {
                    this.fireDirection = new Vector2(0, 1);
                }
                else if (orientation == EnemyOrientation.Left)
                {
                    this.fireDirection = new Vector2(-1, 0);
                }
                else
                {
                    this.fireDirection = new Vector2(1, 0);
                }
            }
        }

        public bool Invincible
        {
            get { return this.invincible; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Turret(Vector2 position, EnemyOrientation orientation, float speed, string name, string onDeathPowerupSpawnType, bool invincible, PhysicsMode onDeathPowerupPhysicsMode, MapWaypoint[] waypoints, SpriteKey bodySprite, SpriteKey nozzleSprite)
            : base(position, PhysicsMode.None, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode)
        {
            this.sprite = new Sprite(bodySprite);
            this.nozzleSprite = new Sprite(nozzleSprite);

            this.speed = speed;
            this.Orientation = orientation;
            this.invincible = invincible;

            if (waypoints != null && waypoints.Length != 2)
            {
                throw new InvalidOperationException("A LaserTurret must have 2 waypoints. Or waypoint parameter may be null");
            }

            if (waypoints != null)
            {
                this.waypointIterator = new ForwardAndBackwardWaypointIterator(waypoints);
                Vector2 direction = this.waypointIterator.Current().DirectionToWaypoint(this.position);
                this.velocity = direction * this.speed;
            }
            else
            {
                this.waypointIterator = null;
                this.velocity = Vector2.Zero;
            }
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Remove()
        {
            ((LaserWeapon)this.laserWeapon).Stop();
            base.Remove();
        }

        public override void PerformAI(Player player)
        {
            if (this.waypointIterator != null)
            {
                // If we have passed the current waypoint, swich waypoints and direction.
                if (this.waypointIterator.Current().PassedWaypoint(this.position, Vector2.Normalize(this.velocity)))
                {
                    this.waypointIterator.Next();
                    this.velocity.X *= -1;

                    DirectionOrientateHorizontal();
                }
            }

            if (!this.dead)
            {
                AttackPlayer(player);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected abstract void AttackPlayer(Player player);

        protected override void OnDeath()
        {
            Explode(SpriteKey.TurretExplosion, SpriteSheetFactory.TURRET_EXPLOSION_TILES_X, SpriteSheetFactory.TURRET_EXPLOSION_TILES_Y);
            base.OnDeath();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
