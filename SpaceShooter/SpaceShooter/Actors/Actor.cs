using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    enum WeaponType
    {
        Default,
        TripleShoot
    }

    abstract class Actor : GameObject
    {
        protected Vector2 shootOffset;
        protected BulletType bulletType;

        protected Vector2 shootVel;

        protected WeaponType weaponType;
        protected float tripleShootAngle = 0.0f;

        protected int energy;
        protected int maxEnergy;

        protected float maxSpeed;

        public bool IsAlive { get { return energy > 0; } }
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }

        public Actor(string texturePath): base(texturePath)
        {
            //RigidBody = new RigidBody(this);
            //RigidBody.Collider = ColliderFactory.CreateBoxFor(this);

            maxEnergy = 100;

            //Reset();
        }

        public virtual void ChangeWeapon(WeaponType weapon)
        {
            weaponType = weapon;
        }

        protected virtual void Shoot()
        {
            //Bullet bullet = BulletMngr.GetBullet(bulletType);

            //if (bullet != null)
            //{
            //    bullet.Shoot(sprite.position + shootOffset);
            //}

            Bullet b;

            switch(weaponType)
            {
                case WeaponType.Default:
                    b = BulletMngr.GetBullet(bulletType);

                    if(b != null)
                    {
                        b.Shoot(sprite.position + shootOffset, shootVel);
                    }
                    break;

                case WeaponType.TripleShoot:
                    float x = (float)Math.Cos(tripleShootAngle);
                    float y = (float)Math.Sin(tripleShootAngle);

                    Vector2 bulletDirection = new Vector2(x, y);

                    for (int i = 0; i < 3; i++)
                    {
                        b = BulletMngr.GetBullet(bulletType);

                        if(b != null)
                        {
                            b.Shoot(Position + shootOffset, bulletDirection.Normalized() * 1000.0f);
                            bulletDirection.Y -= y;
                        }
                    }
                    break;
            }
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                OnDie();
            }
        }

        public virtual void OnDie()
        {

        }

        public virtual void Reset()
        {
            Energy = maxEnergy;
        }
    }
}
