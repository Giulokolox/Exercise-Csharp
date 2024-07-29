using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet(string textureName, int w=0, int h=0) : base(textureName,w,h)
        {
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            maxSpeed = -600.0f;
            RigidBody.Velocity.X = maxSpeed;
            RigidBody.Type = RigidBodyType.EnemyBullet;
            RigidBody.AddCollisionType(RigidBodyType.PlayerBullet | RigidBodyType.Player);
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (sprite.position.X + sprite.pivot.X < 0)
                {
                    BulletMngr.RestoreBullet(this);
                } 
            }
        }

        public override void OnCollide(GameObject other)
        {
            if (other is PlayerBullet)
            {
                BulletMngr.RestoreBullet(this);
            }
            else if (other is Player)
            {
                ((Player)other).AddDamage(damage);
                BulletMngr.RestoreBullet(this);
            }
        }
    }
}
