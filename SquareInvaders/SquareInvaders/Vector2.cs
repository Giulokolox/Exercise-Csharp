using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Add(Vector2 b)
        {
            X += b.X;
            Y += b.Y;
        }
        public void Sub(Vector2 b)
        {
            X -= b.X;
            Y -= b.Y;
        }

        public void Mul(float s)
        {
            X *= s;
            Y *= s;
        }

        public float GetLength()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }
    }
}
