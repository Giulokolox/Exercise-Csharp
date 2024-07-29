using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2021
{
    class StdBullet : Bullet
    {
        public StdBullet() : base("stdBullet")
        {
            Type = BulletType.StdBullet;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Tile );
            maxSpeed = 17;
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Player)
            {
                ((Player)collisionInfo.Collider).AddDamage(damage);
            }

            BulletMngr.RestoreBullet(this);
        }
    }
}
