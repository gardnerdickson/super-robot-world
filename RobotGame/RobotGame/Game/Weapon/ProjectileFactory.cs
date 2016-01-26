using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Weapon
{
    public interface ProjectileFactory
    {
        GameActor CreateProjectile(Vector2 position, Vector2 velocity);
    }
}
