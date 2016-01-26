using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using System.Collections.Generic;
using RobotGame.Game.Enemy;
using RobotGame.Game.Mover;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame.Game.Weapon
{
    abstract class Laser : GameActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        protected const float LASER_DRAW_DEPTH = Config.LASER_DRAW_DEPTH;
        protected const int LENGTH_INCREMENT = Config.LASER_LENGTH_INCREMENT;
        protected const int LASER_LINE_HEIGHT = Config.LASER_LINE_HEIGHT;
        protected const int LASER_GLOW_HEIGHT = Config.LASER_GLOW_HEIGHT;

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected static List<GameActor> laserList = new List<GameActor>();

        protected int damage;
        protected Vector2 direction;
        protected Line laserLine;

        private Sprite lineSprite;
        private Sprite glowSprite;

        protected bool enabled;

        // Properties ------------------------------------------------------------------------------------- Properties

        public static List<GameActor> LaserList
        {
            get { return laserList; }
        }

        public Line LaserLine
        {
            get { return this.laserLine; }
        }

        public Vector2 Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        public int Damage
        {
            get { return this.damage; }
            set { this.damage = value; }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Laser(int damage, Vector2 position, Vector2 direction)
            : base(position, Vector2.Zero, PhysicsMode.None)
        {
            this.damage = damage;
            this.direction = direction;

            this.laserLine = new Line();

            this.lineSprite = new Sprite(SpriteKey.LaserLine);
            this.glowSprite = new Sprite(SpriteKey.LaserGlow);

            this.enabled = true;

            laserList.Add(this);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Collide(GameActor actor)
        {
            // do nothing
        }

        public override void Update(GameTime gameTime)
        {
            if (enabled)
            {
                HandleActorCollisions();
                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (enabled)
            {
                int width = (int)Vector2.Distance(this.laserLine.Point1, this.laserLine.Point2);

                float angle = (float)Math.Atan2(this.direction.Y, this.direction.X);
                Rectangle lineRectangle = new Rectangle((int)this.laserLine.Point1.X, (int)this.laserLine.Point1.Y, width, LASER_LINE_HEIGHT);
                Rectangle glowRectangle = new Rectangle((int)this.laserLine.Point1.X, (int)this.laserLine.Point1.Y, width, LASER_GLOW_HEIGHT);

                spriteBatch.Draw(this.lineSprite.Texture, this.position, glowRectangle, new Color(255, 255, 255, 100), angle, new Vector2(0, LASER_GLOW_HEIGHT / 2), 1f, SpriteEffects.None, LASER_DRAW_DEPTH);
                spriteBatch.Draw(this.lineSprite.Texture, this.position, lineRectangle, Color.White, angle, new Vector2(0, LASER_LINE_HEIGHT / 2), 1f, SpriteEffects.None, LASER_DRAW_DEPTH);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected Line CollideLineWithEnvironment(Line line)
        {
            Point dummy = new Point();
            return CollideLineWithEnvironment(line, false, false, ref dummy);
        }

        protected virtual Line CollideLineWithEnvironment(Line line, bool ignoreLevel, bool ignoreMovers, ref Point collidingTile)
        {
            int i = 0;
            while (true)
            {
                Vector2 point2 = this.position + (this.direction * LENGTH_INCREMENT * (i + 1));

                if (Vector2.Distance(line.Point1, point2) > 4096f)
                {
                    return line;
                }

                line.Point2.X = point2.X;
                line.Point2.Y = point2.Y;

                if (!ignoreLevel)
                {
                    Rectangle collisionRectangle = new Rectangle((int)line.Point2.X - (int)(Level.GetInstance().TileWidth / 2),
                                                                 (int)line.Point2.Y - (int)(Level.GetInstance().TileHeight / 2),
                                                                 Level.GetInstance().TileWidth, Level.GetInstance().TileHeight);

                    int leftTile = (int)Math.Floor((float)collisionRectangle.Left / Level.GetInstance().TileWidth);
                    int rightTile = (int)Math.Ceiling((float)collisionRectangle.Right / Level.GetInstance().TileWidth) - 1;
                    int topTile = (int)Math.Floor((float)collisionRectangle.Top / Level.GetInstance().TileHeight);
                    int bottomTile = (int)Math.Ceiling((float)collisionRectangle.Bottom / Level.GetInstance().TileHeight) - 1;

                    for (int y = topTile; y <= bottomTile; y++)
                    {
                        for (int x = leftTile; x <= rightTile; x++)
                        {
                            if (!Level.GetInstance().TileIsOnMap(x, y))
                            {
                                return line;
                            }

                            if (Level.GetInstance().IsCollisionTile(x, y) || Level.GetInstance().IsDestructibleTile(x, y))
                            {
                                collidingTile = new Point(x, y);

                                Rectangle tileBounds = Level.GetInstance().GetTileBounds(x, y);
                                if (CollisionUtil.CheckIntersectionCollision(line, tileBounds))
                                {
                                    return line;
                                }
                            }
                        }
                    }
                }


                if (!ignoreMovers)
                {
                    List<GameActor> moverList = AbstractMover.MoverList;
                    foreach (AbstractMover mover in moverList)
                    {
                        if (CollisionUtil.CheckIntersectionCollision(line, mover.Bounds))
                        {
                            return line;
                        }
                    }
                }

                i++;
            }
        }

        protected Line CollideLineWithMovers(Line line)
        {
            AbstractMover dummy = null;
            return CollideLineWithMovers(line, ref dummy);
        }

        protected Line CollideLineWithMovers(Line line, ref AbstractMover collidingMover)
        {
            List<GameActor> moverList = AbstractMover.MoverList;
            foreach (AbstractMover mover in moverList)
            {
                if (CollisionUtil.CheckIntersectionCollision(line, mover.Bounds))
                {
                    collidingMover = mover;

                    if (Math.Abs(this.direction.Y) > Math.Abs(this.direction.X))
                    {
                        if (this.direction.Y > 0)
                        {
                            line.Point2.Y = mover.Bounds.Top;
                        }
                        else
                        {
                            line.Point2.Y = mover.Bounds.Bottom;
                        }
                    }
                    else
                    {
                        if (this.direction.X > 0)
                        {
                            line.Point2.X = mover.Bounds.Left;
                        }
                        else
                        {
                            line.Point2.X = mover.Bounds.Right;
                        }
                    }
                }
            }

            return line;
        }

        protected bool CheckMoverCollision(Line line)
        {
            List<GameActor> moverList = AbstractMover.MoverList;
            foreach (AbstractMover mover in moverList)
            {
                if (CollisionUtil.CheckIntersectionCollision(line, mover.Bounds))
                {
                    return true;
                }
            }
            return false;
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void HandleActorCollisions()
        {
            // Handle player collisions
            GameActor player = Player.PlayerList[0];
            if (!player.Dead)
            {
                if (CollisionUtil.CheckIntersectionCollision(this.laserLine, player.Bounds))
                {
                    ((Player)player).TakeDamage(this.damage);
                    this.Collide(player);
                }
            }

            // Handle enemy collisions
            foreach (GameActor enemy in AbstractEnemy.EnemyList)
            {
                if (!enemy.Dead)
                {
                    if (CollisionUtil.CheckIntersectionCollision(this.laserLine, enemy.Bounds))
                    {
                        ((AbstractEnemy)enemy).TakeDamage(this.damage);
                        this.Collide(enemy);
                    }
                }
            }

            // Handle projectile collisions
            foreach (GameActor projectile in Projectile.PlayerProjectileList)
            {
                if (!projectile.Dead)
                {
                    ExplosiveProjectile explosiveProjectle = projectile as ExplosiveProjectile;
                    if (explosiveProjectle != null)
                    {
                        if (CollisionUtil.CheckIntersectionCollision(this.laserLine, explosiveProjectle.Bounds))
                        {
                            projectile.Collide(this);
                            this.Collide(projectile);
                        }
                    }
                }
            }
            foreach (GameActor projectile in Projectile.EnemyProjectileList)
            {
                if (!projectile.Dead)
                {
                    ExplosiveProjectile explosiveProjectle = projectile as ExplosiveProjectile;
                    if (explosiveProjectle != null)
                    {
                        if (CollisionUtil.CheckIntersectionCollision(this.laserLine, explosiveProjectle.Bounds))
                        {
                            projectile.Collide(this);
                            this.Collide(projectile);
                        }
                    }
                }
            }
        }
    }
}
