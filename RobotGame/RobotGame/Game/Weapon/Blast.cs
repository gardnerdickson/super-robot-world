using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using RobotGame.Engine;
using RobotGame.Game.Enemy;
using RobotGame.Game.Audio;
using RobotGame.Game.Mover;

namespace RobotGame.Game.Weapon
{
    class Blast : GameActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float POSITION_OFFSET = Config.BLAST_SMOKE_POSITION_OFFSET;
        private const float SMOKE_SPEED = Config.BLAST_SMOKE_SPEED;
        private const float SMOKE_SCALE_START = Config.BLAST_SMOKE_SCALE_START;
        private const float SMOKE_SCALE_END = Config.BLAST_SMOKE_SCALE_END;
        private const float SMOKE_ROTATION_INCREMENT = Config.BLAST_SMOKE_ROTATION_INCREMENT;

        private const int TILE_EXPLOSION_PARTICLE_COUNT_X = Config.TILE_EXPLOSION_PARTICLE_COUNT_X;
        private const int TILE_EXPLOSION_PARTICLE_COUNT_Y = Config.TILE_EXPLOSION_PARTICLE_COUNT_Y;
        private const int TILE_EXPLOSION_PARTICLE_WIDTH = Config.TILE_EXPLOSION_PARTICLE_WIDTH;
        private const int TILE_EXPLOSION_PARTICLE_HEIGHT = Config.TILE_EXPLOSION_PARTICLE_HEIGHT;
        private const int TILE_EXPLOSION_PARTICLE_MASS = Config.TILE_EXPLOSION_PARTICLE_MASS;
        private const int TILE_EXPLOSION_PARTICLE_MASS_RANDOM_RANGE = Config.TILE_EXPLOSION_PARTICLE_MASS_RANDOM_RANGE;
        private const float TILE_EXPLOSION_PARTICLE_ROTATION = Config.TILE_EXPLOSION_PARTICLE_ROTATION;
        private const float TILE_EXPLOSION_PARTICLE_SPEED = Config.TILE_EXPLOSION_PARTICLE_SPEED;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private Circle blastArea;

        private int damage;
        private Rectangle projectileBounds;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Blast(Vector2 position, Rectangle projectileBounds, SpriteKey explosionParticleSpriteKey, SpriteKey smokeParticleSpriteKey, int damage, Circle blastArea)
            : base(position)
        {
            this.projectileBounds = projectileBounds;

            this.damage = damage;
            this.blastArea = blastArea;

            float explosionScale = (this.blastArea.Radius * 2) / 100;
            float smokeScaleStart = SMOKE_SCALE_START * explosionScale;
            float smokeScaleEnd = SMOKE_SCALE_END * explosionScale;
            float smokeSpeed = SMOKE_SPEED * explosionScale;
            
            SoundManager.PlayRandomSoundEffect(SoundKey.ProjectileExplosion2, SoundKey.ProjectileExplosion1);

            new Particle(position, explosionParticleSpriteKey, 0f, 255, 255, explosionScale, explosionScale);

            new Particle(position + (Vector2.Normalize(new Vector2(0f, -1f)) * POSITION_OFFSET), Vector2.Normalize(new Vector2(0f, -1f)) * smokeSpeed, 100f, 100f, Vector2.Zero, smokeParticleSpriteKey, SMOKE_ROTATION_INCREMENT, smokeScaleStart, smokeScaleEnd);
            new Particle(position + (Vector2.Normalize(new Vector2(1f, 0f)) * POSITION_OFFSET), Vector2.Normalize(new Vector2(1f, 0f)) * smokeSpeed, 100f, 100f, Vector2.Zero, smokeParticleSpriteKey, SMOKE_ROTATION_INCREMENT, smokeScaleStart, smokeScaleEnd);
            new Particle(position + (Vector2.Normalize(new Vector2(-1f, 0f)) * POSITION_OFFSET), Vector2.Normalize(new Vector2(-1f, 0f)) * smokeSpeed, 100f, 100f, Vector2.Zero, smokeParticleSpriteKey, SMOKE_ROTATION_INCREMENT, smokeScaleStart, smokeScaleEnd);
            new Particle(position + (Vector2.Normalize(new Vector2(0f, 1f)) * POSITION_OFFSET), Vector2.Normalize(new Vector2(0f, 1f)) * smokeSpeed, 100f, 100f, Vector2.Zero, smokeParticleSpriteKey, SMOKE_ROTATION_INCREMENT, smokeScaleStart, smokeScaleEnd);
            new Particle(position, (new Vector2(-1f, -1f)) * smokeSpeed, 100f, 100f, Vector2.Zero, smokeParticleSpriteKey, SMOKE_ROTATION_INCREMENT, smokeScaleStart, smokeScaleEnd);
            new Particle(position, (new Vector2(1f, 1f)) * smokeSpeed, 100f, 100f, Vector2.Zero, smokeParticleSpriteKey, SMOKE_ROTATION_INCREMENT, smokeScaleStart, smokeScaleEnd);
            new Particle(position, (new Vector2(-1f, 1f)) * smokeSpeed, 100f, 100f, Vector2.Zero, smokeParticleSpriteKey, SMOKE_ROTATION_INCREMENT, smokeScaleStart, smokeScaleEnd);
            new Particle(position, (new Vector2(1f, -1f)) * smokeSpeed, 100f, 100f, Vector2.Zero, smokeParticleSpriteKey, SMOKE_ROTATION_INCREMENT, smokeScaleStart, smokeScaleEnd);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update(GameTime gameTime)
        {

            CheckActorCollisions();
            HandleDesctructibleTileCollisions();

            this.Remove();
            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void CheckActorCollisions()
        {
            // Player collisions
            foreach (GameActor gameActor in Player.PlayerList)
            {
                HandleCollision((Player)gameActor);
            }

            // Enemy collisions
            foreach (GameActor gameActor in AbstractEnemy.EnemyList)
            {
                HandleCollision((AbstractEnemy)gameActor);
            }
        }

        private void HandleCollision(SentientActor actor)
        {
            if (CollisionUtil.CheckIntersectionCollision(this.projectileBounds, actor.Bounds) != Rectangle.Empty)
            {
                actor.TakeDamage(this.damage);
            }
            else
            {
                float damagePercentage = CollisionUtil.CheckIntersectionCollision(this.blastArea, actor.Bounds);
                int totalDamage = (int)(this.damage * damagePercentage);

                if (totalDamage > 0)
                {
                    // Get the range of tiles for which we need to check for intersections
                    Rectangle rectArea = new Rectangle((int)(this.blastArea.Center.X - this.blastArea.Radius),
                                                       (int)(this.blastArea.Center.Y - this.blastArea.Radius),
                                                       (int)(this.blastArea.Radius * 2),
                                                       (int)(this.blastArea.Radius * 2));

                    int leftTile = (int)Math.Floor((float)rectArea.Left / Level.GetInstance().TileWidth);
                    int rightTile = (int)Math.Ceiling((float)rectArea.Right / Level.GetInstance().TileWidth) - 1;
                    int topTile = (int)Math.Floor((float)rectArea.Top / Level.GetInstance().TileHeight);
                    int bottomTile = (int)Math.Ceiling((float)rectArea.Bottom / Level.GetInstance().TileHeight) - 1;

                    // Iterate through all the tiles in the blast area to see if there is a collision tile between
                    // center of the blast and the center of the actor colliding with the blast
                    int levelWidth = Level.GetInstance().TilesX;
                    int levelHeight = Level.GetInstance().TilesY;
                    for (int y = topTile; y <= bottomTile; ++y)
                    {
                        for (int x = leftTile; x <= rightTile; ++x)
                        {
                            if (y > 0 && y < levelHeight && x > 0 && x < levelWidth)
                            {
                                if (Level.GetInstance().IsCollisionTile(x, y) || Level.GetInstance().IsDestructibleTile(x, y))
                                {
                                    // If the line segment between the blast area and the actor intersect a collision tile,
                                    // no damage shoud be dealt to the  actor
                                    bool collisionOccurred = CollisionUtil.CheckIntersectionCollision(new Line(this.position, actor.Position), Level.GetInstance().GetTileBounds(x, y));
                                    if (collisionOccurred)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }


                    // Now check if the mover is interfering with the blast
                    foreach (AbstractMover mover in AbstractMover.MoverList)
                    {
                        float intersectionDepth = CollisionUtil.CheckIntersectionCollision(this.blastArea, mover.Bounds);
                        if (intersectionDepth > 0)
                        {
                            bool collisionOccurred = CollisionUtil.CheckIntersectionCollision(new Line(this.position, actor.Position), mover.Bounds);
                            if (collisionOccurred)
                            {
                                return;
                            }
                        }
                    }

                    // If we've gotten this far, the level is not interfering with the blast collision, so deal damage to the actor
                    actor.TakeDamage(totalDamage);
                }
            }
        }

        private void HandleDesctructibleTileCollisions()
        {
            Rectangle rectArea = new Rectangle((int)(this.blastArea.Center.X - this.blastArea.Radius / 2),
                                               (int)(this.blastArea.Center.Y - this.blastArea.Radius / 2),
                                               (int)(this.blastArea.Radius),
                                               (int)(this.blastArea.Radius));

            int leftTile = (int)Math.Floor((float)rectArea.Left / Level.GetInstance().TileWidth);
            int rightTile = (int)Math.Ceiling((float)rectArea.Right / Level.GetInstance().TileWidth) - 1;
            int topTile = (int)Math.Floor((float)rectArea.Top / Level.GetInstance().TileHeight);
            int bottomTile = (int)Math.Ceiling((float)rectArea.Bottom / Level.GetInstance().TileHeight) - 1;

            int levelWidth = Level.GetInstance().TilesX;
            int levelHeight = Level.GetInstance().TilesY;
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    if (y > 0 && y < levelHeight && x > 0 && x < levelWidth)
                    {
                        if (Level.GetInstance().IsDestructibleTile(x, y))
                        {
                            float intersectionDepth = CollisionUtil.CheckIntersectionCollision(this.blastArea, Level.GetInstance().GetTileBounds(x, y));
                            if (intersectionDepth > 0.75f)
                            {
                                bool destroyedTile = Level.GetInstance().DestroyTile(x, y);
                                if (destroyedTile)
                                {
                                    TileExplode(x, y);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void TileExplode(int x, int y)
        {
            Rectangle tileBounds = Level.GetInstance().GetTileBounds(x, y);
            Vector2 position = new Vector2(tileBounds.X + tileBounds.Width / 2, tileBounds.Y + tileBounds.Height / 2);

            Explosions.CreateExplosion(position, Level.GetInstance().LevelInfo.TileExplosionSprite, SpriteSheetFactory.LEVEL_EXPLOSION_TILES_X, SpriteSheetFactory.LEVEL_EXPLOSION_TILES_Y,
                                       SpriteKey.None, TILE_EXPLOSION_PARTICLE_MASS, TILE_EXPLOSION_PARTICLE_MASS_RANDOM_RANGE, TILE_EXPLOSION_PARTICLE_SPEED);
        }

    } // End class
}
