using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SpaceShooter
{
    class Enemy_01 : Enemy
    {
        public Enemy_01() : base("enemy01")
        {
            Type = EnemyType.Enemy01;

            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);

            shootOffset = new Vector2(-sprite.pivot.X, sprite.pivot.Y * 0.5f);
            nextShoot = RandomGenerator.GetRandomInt(1, 2);
            bulletType = BulletType.PurpleBullet;
            shootVel = new Vector2(-1000.0f, 0.0f);

            Points = 25;
        }
    }
}
