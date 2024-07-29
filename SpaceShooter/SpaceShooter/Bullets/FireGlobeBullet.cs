using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class FireGlobeBullet : EnemyBullet
    {
        protected float rotationSpeed = -3.5f;
        protected float accumulator = 0;

        public FireGlobeBullet():base("fireGlobe")
        {
            Type = BulletType.FireglobeBullet;
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this, true);
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();
                
                if (IsActive)
                {
                    //sprite rotation
                    sprite.Rotation += rotationSpeed * Game.DeltaTime;

                    //sprite y movement
                    accumulator += Game.DeltaTime * 10;

                    RigidBody.Velocity.Y = (float)Math.Sin(accumulator) * 850;
                }

            }
        }

        public override void Reset()
        {
            base.Reset();
            accumulator = 0;
        }
    }
}
