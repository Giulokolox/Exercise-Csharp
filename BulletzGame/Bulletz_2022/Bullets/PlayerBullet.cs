using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Bulletz_2022
{
    class PlayerBullet : Bullet
    {
        public PlayerBullet(string bulletName = "playerBullet") : base(bulletName)
        {
            maxSpeed = 7;
            //RigidBody.Velocity.X = maxSpeed;

            Type = BulletType.PlayerBullet;
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.EnemyBullet);
            RigidBody.AddCollisionType(RigidBodyType.Enemy);

        }

        public override void OnCollide(GameObject other)
        {
            //if(other is EnemyBullet)
            //{
            //    BulletMngr.RestoreBullet(this);
            //}
            //else if(other is Enemy)
            //{
            //    Enemy enemy = ((Enemy)other);
            //    enemy.AddDamage(damage);
            //    if (!enemy.IsAlive)
            //    {
            //        ((PlayScene)Game.CurrentScene).Player.AddScore(enemy.Points);
            //    }
            //    BulletMngr.RestoreBullet(this);
            //}
        }
    }
}
