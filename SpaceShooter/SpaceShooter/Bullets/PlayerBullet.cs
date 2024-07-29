using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter
{
    class PlayerBullet : Bullet
    {
        public PlayerBullet(string bulletName = "blueLaser") : base(bulletName)
        {
            maxSpeed = 600.0f;
            RigidBody.Velocity.X = maxSpeed;

            Type = BulletType.PlayerBullet;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.EnemyBullet);
            RigidBody.AddCollisionType(RigidBodyType.Enemy);

        }

        public override void Update()
        {
            if (IsActive)
            {
                if (sprite.position.X - sprite.pivot.X >= Game.Window.Width)
                {
                    BulletMngr.RestoreBullet(this);
                } 
            }
        }

        public override void OnCollide(GameObject other)
        {
            if(other is EnemyBullet)
            {
                BulletMngr.RestoreBullet(this);
            }
            else if(other is Enemy)
            {
                Enemy enemy = ((Enemy)other);
                enemy.AddDamage(damage);
                if (!enemy.IsAlive)
                {
                    ((PlayScene)Game.CurrentScene).Player.AddScore(enemy.Points);
                }
                BulletMngr.RestoreBullet(this);
            }
        }
    }
}
