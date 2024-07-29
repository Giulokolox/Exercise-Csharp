using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter
{
    enum EnemyType
    {
        Enemy01, EnemyBig, LAST
    }

    abstract class Enemy : Actor
    {
        protected float nextShoot;
        public EnemyType Type { get; protected set; }

        public int Points { get; protected set; }

        public Enemy(string textureName):base(textureName)
        {
            sprite.FlipX = true;
            maxSpeed = -200;
            RigidBody = new RigidBody(this);
            RigidBody.Velocity.X = maxSpeed;
            RigidBody.Type = RigidBodyType.Enemy;
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (sprite.position.X + HalfWidth < 0)
                {//outside screen
                    SpawnMngr.RestoreEnemy(this);
                }
                else
                {//shoot management
                    nextShoot -= Game.DeltaTime;

                    if (nextShoot <= 0)
                    {
                        nextShoot = RandomGenerator.GetRandomFloat() * 2 + 1;
                        Shoot();
                    }
                } 
            }
        }

        public override void OnDie()
        {
            SpawnMngr.RestoreEnemy(this);
        }
    }
}
