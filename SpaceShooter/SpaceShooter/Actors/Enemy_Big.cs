using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SpaceShooter
{
    class Enemy_Big : Enemy
    {
        public Enemy_Big() : base("enemyBig")
        {
            Type = EnemyType.EnemyBig;

            // Compound Collider
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this, false);
            CompoundCollider compound = new CompoundCollider(RigidBody, RigidBody.Collider);

            BoxCollider box01 = new BoxCollider(RigidBody, (HalfWidth + 100), (HalfHeight * 2 - 40));
            box01.Offset = new Vector2(40,0);
            compound.AddCollider(box01);

            BoxCollider box02 = new BoxCollider(RigidBody, (HalfWidth * 2), 25);
            box02.Offset = new Vector2(0.0f, 75.0f);
            compound.AddCollider(box02);

            BoxCollider box03 = new BoxCollider(RigidBody, 80, 20);
            box03.Offset = new Vector2(68.0f, 90.0f);
            compound.AddCollider(box03);

            RigidBody.Collider.Offset = compound.Offset;
            RigidBody.Collider = compound;

            shootOffset = new Vector2(-sprite.pivot.X, sprite.pivot.Y * 0.5f);
            nextShoot = RandomGenerator.GetRandomInt(1, 2);
            bulletType = BulletType.FireglobeBullet;
            shootVel = new Vector2(-1000.0f, 0.0f);

            Points = 50;
        }
    }
}
