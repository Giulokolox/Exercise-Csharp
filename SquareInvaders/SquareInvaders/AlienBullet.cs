using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw;

namespace SquareInvaders
{
    class AlienBullet
    {
        private int halfWidth;
        private int halfHeight;

        public Vector2 Position;
        private Vector2 velocity;
        public bool IsAlive;

        private Sprite sprite;
        private Animation animation;

        public AlienBullet()
        {
            Position = new Vector2(0, 0);
            velocity = Position;

            // Animation Sprites settings
            string[] frames = new string[2];

            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = $"Assets/alienBullet_{i}.png";
            }

            animation = new Animation(frames, 20);
            sprite = animation.CurrentSprite;

            halfWidth = (int)(sprite.Width * 0.5f);
            halfHeight = (int)(sprite.Height * 0.5f);
        }

        public void Update()
        {
            Position.Y += velocity.Y * Gfx.Window.DeltaTime;

            if(Position.Y - halfHeight >= Gfx.Window.Height)
            {
                IsAlive = false;
            }
            else
            {
                animation.Update();
                sprite = animation.CurrentSprite;
            }
        }

        public bool Collides(Vector2 center, float ray)
        {
            Vector2 dist = center;
            dist.Sub(Position);

            return (dist.GetLength() <= halfWidth + ray);
        }

        public void Shoot(Vector2 startPos, Vector2 startVel)
        {
            // Add bullet halfHeight to shooting position
            startPos.Y += halfHeight;
            Position = startPos;
            velocity = startVel;
            IsAlive = true;
        }

        public void Draw()
        {
            Gfx.DrawSprite(sprite, (int)(Position.X - halfWidth), (int)(Position.Y - halfHeight));
        }
    }
}
