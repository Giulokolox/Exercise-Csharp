using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class PurpleBullet : EnemyBullet
    {
        public PurpleBullet() : base("purpleLaser", 74, 46)
        {
            Type = BulletType.PurpleBullet;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture, 156, 227, (int)sprite.Width, (int)sprite.Height);
            }
        }
    }
}
