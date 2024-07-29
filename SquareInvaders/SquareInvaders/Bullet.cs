using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    class Bullet
    {
        private int halfWidth;
        private int halfHeight;
        private Color color;

        public Vector2 Position;
        private Vector2 velocity;
        public bool IsAlive;

        public Bullet()
        {
            halfWidth = 4;
            halfHeight = 12;
            velocity.Y = -950;

            color = ColorsFactory.GetColor(ColorType.Red);
        }

        public void Update()
        {
            Position.Y += velocity.Y * Gfx.Window.DeltaTime;

            // If the whole Bullet goes up the screen, set it back as not alive
            if(Position.Y + halfHeight * 2 < 0)
            {
                IsAlive = false;
            }
        }

        public bool Collides(Vector2 center, float ray)
        {
            Vector2 dist = center;
            dist.Sub(Position);

            return (dist.GetLength() <= halfWidth + ray);
        }

        public void Draw()
        {
            Gfx.DrawRect((int)Position.X - halfWidth, (int)Position.Y - halfHeight, halfWidth * 2, halfHeight * 2, color);
        }

    }
}
