using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    abstract class Tank : Actor
    {
        protected Sprite body;
        protected Sprite cannon;

        protected Texture bodyTexture;
        protected Texture cannonTexture;

        protected Vector2 bodyOffset;
        protected Vector2 cannonOffset;

        protected float maxCannonAngle = (float)-Math.PI;

        public float CannonLength { get { return cannon.Width; } }

        protected Vector2 shakeOffset;
        protected float moveAccumulator;


        public Tank() : base("tracks")
        {
            bodyTexture = GfxMngr.GetTexture("body");
            cannonTexture = GfxMngr.GetTexture("cannon");

            body = new Sprite(Game.PixelsToUnits(bodyTexture.Width), Game.PixelsToUnits(bodyTexture.Height));
            cannon = new Sprite(Game.PixelsToUnits(cannonTexture.Width), Game.PixelsToUnits(cannonTexture.Height));

            body.pivot = new Vector2(body.Width * 0.5f, body.Height * 0.5f);
            cannon.pivot = new Vector2(0, cannon.Height * 0.5f);

            bodyOffset = new Vector2(Game.PixelsToUnits(2), -body.Height * 0.4f);
            cannonOffset = new Vector2(0, -body.Height * 0.4f);
        }

        protected virtual void Shoot(float speedPercentage=1)
        {
            Vector2 direction = new Vector2((float)Math.Cos(cannon.Rotation), (float)Math.Sin(cannon.Rotation));
            Vector2 bulletPosition = cannon.position + direction * CannonLength;

            Shoot(direction * speedPercentage, bulletPosition);
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();

                if (RigidBody.Velocity.X != 0)
                {//it's moving horizontally
                    moveAccumulator += Game.DeltaTime * 10f;

                    shakeOffset.Y = (float)Math.Sin(moveAccumulator)*0.01f;
                    shakeOffset.X = (float)Math.Cos(moveAccumulator)*0.01f;
                }

                body.position = sprite.position + bodyOffset + shakeOffset;//tracks + offset
                cannon.position = body.position + cannonOffset;
            }
        }

        public override void Draw()
        {
            if (IsActive)
            {
                cannon.DrawTexture(cannonTexture);
                sprite.DrawTexture(texture);//tracks
                body.DrawTexture(bodyTexture);
            }
        }
    }
}
