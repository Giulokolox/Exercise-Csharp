using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Boids
{
     static class Map
    {
        public static Window window;

        public static void init()
        {
            window = new Window(800, 600, "Boids", false);
        }

        public static Vector2 Bord(Vector2 spriteposition)
        {
            if (spriteposition.X < 0)
            {
                spriteposition.X += window.Width;
            }
            if (spriteposition.X > window.Width)
            {
                spriteposition.X -= window.Height;
            }
            if (spriteposition.Y < 0)
            {
                spriteposition.Y += window.Height;
            }
            if (spriteposition.Y > window.Height)
            {
                spriteposition.Y -= window.Height;
            }
            return spriteposition;
        }
    }
}
