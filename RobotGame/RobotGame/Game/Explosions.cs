using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game
{
    class Explosions
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float EXPLOSION_PARTICLE_ROTATION = Config.EXPLOSION_PARTICLE_ROTATION;
        private const float PARTICLE_EMITTER_SPAWN_FREQUENCY = Config.EXPLOSION_PARTICLE_EMITTER_SPAWN_FREQUENCY;
        private const float PARTICLE_EMITTER_ROTATION = Config.EXPLOSION_PARTICLE_EMITTER_ROTATION;
        private const int PARTICLE_EMITTER_FADE_START = Config.EXPLOSION_PARTICLE_EMITTER_FADE_START;
        private const int PARTICLE_EMITTER_FADE_END = Config.EXPLOSION_PARTICLE_EMIITER_FADE_END;
        private const float PARTICLE_EMITTER_SCALE_START = Config.EXPLOSION_PARTICLE_EMITTER_SCALE_START;
        private const float PARTICLE_EMITTER_SCALE_END = Config.EXPLOSION_PARTICLE_EMITTER_SCALE_END;

        // Data Members --------------------------------------------------------------------------------- Data Members
        
        private static Random random = new Random();

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private Explosions() { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static void CreateExplosion(Vector2 position, SpriteKey explosionSpriteSheet, int explosionTilesX, int explosionTilesY,
                                           SpriteKey particleSpriteKey, int particleMass, int particleMassRandomRange, float particleSpeed)
        {
            float randomMass;
            Vector2 randomDirection, randomVelocity;
            Sprite explosionSprite;
            int frame = 0;

            for (int i = 0; i < explosionTilesX; i++)
            {
                for (int j = 0; j < explosionTilesY; j++)
                {

                    randomMass = particleMass + random.Next(0, particleMassRandomRange + 1);
                    
                    int randomDirectionY = random.Next(-100, 1);
                    int randomDirectionX = random.Next(-Math.Abs(randomDirectionY), Math.Abs(randomDirectionY));

                    randomDirection = Vector2.Normalize(new Vector2(randomDirectionX, randomDirectionY));
                    randomVelocity = randomDirection * particleSpeed;

                    int rotationSign = random.Next(0, 10) < 5 ? -1 : 1;

                    explosionSprite = new Sprite(explosionSpriteSheet, false);
                    ParticleEmitter particleEmitter = new ParticleEmitter(PARTICLE_EMITTER_SPAWN_FREQUENCY, position, Vector2.Zero, 0f, 0f, Vector2.Zero, particleSpriteKey,
                                                                          0f, PARTICLE_EMITTER_FADE_START, PARTICLE_EMITTER_FADE_END, PARTICLE_EMITTER_SCALE_START, PARTICLE_EMITTER_SCALE_END);
                    new ExplosionParticle(position, randomVelocity, randomMass, explosionSprite, frame, EXPLOSION_PARTICLE_ROTATION * rotationSign, particleEmitter);

                    frame++;
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
