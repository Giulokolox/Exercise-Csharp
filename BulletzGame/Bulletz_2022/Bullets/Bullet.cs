using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz_2022
{
    enum BulletType { PlayerBullet, LAST }

    abstract class Bullet : GameObject
    {
        protected int damage = 25;

        public BulletType Type { get; protected set; }

        protected float maxSpeed;

        public Bullet(string texturePath, int spriteWidth=0, int spriteHeight=0) : base(texturePath)
        {
            //texture = GfxMngr.GetTexture(texturePath);

            //int spriteW = spriteWidth != 0 ? spriteWidth : texture.Width;
            //int spriteH = spriteHeight != 0 ? spriteHeight : texture.Height;

            //sprite = new Sprite(spriteW, spriteH);
            //sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            
            RigidBody = new RigidBody(this);
        }

        public void Shoot(Vector2 shootPos, Vector2 shootVel)
        {
            sprite.position = shootPos;

            RigidBody.Velocity = shootVel;
        }

        public virtual void Reset()
        {
            IsActive = false;

        }

        public override void Update()
        {
            Vector2 cameraDist = Position - CameraMngr.MainCamera.position;//vector from camera to bullet

            if(cameraDist.LengthSquared > CameraMngr.HalfDiagonalSquared)
            {
                BulletMngr.RestoreBullet(this);
            }

        }
    }
}
