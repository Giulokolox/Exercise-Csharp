using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz_2022
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
        protected int spriteOffsetX;
        protected int frameWidth = 58;

        protected Vector2 shootVel;

        protected WeaponType weaponType;
        protected float tripleShootAngle = 0.0f;

        protected int energy;
        protected int maxEnergy;

        protected float maxSpeed;
        protected float shootSpeed = 8f;

        public bool IsAlive { get { return energy > 0; } }
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }

        public Actor(string texturePath, float w = 0, float h = 0) : base(texturePath, w:w ,h:h)
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

        protected virtual void Shoot(Vector2 direction)
        {
            Bullet b;

            switch (weaponType)
            {
                case WeaponType.Default:
                    b = BulletMngr.GetBullet(bulletType);

                    if (b != null)
                    {
                        b.Shoot(sprite.position + shootOffset, direction * shootSpeed);
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

        public override void Draw()
        {
            sprite.DrawTexture(texture, spriteOffsetX, 0, frameWidth, frameWidth);
        }
    }
}
