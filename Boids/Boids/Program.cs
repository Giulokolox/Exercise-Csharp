using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Boids
{
     class Program
    {
        static void Main(string[] args)
        {
            Map.init();
            Boids boid = new Boids();
            int i = 0;
            bool mousePressed = false;
            while (Map.window.IsOpened)
            {
                if (Map.window.MouseLeft)
                {
                    if (!mousePressed)
                    {
                        i++;
                        boid.AddBoids(i, boid);
                        mousePressed = true;
                    }
                }
                else
                {
                    mousePressed = false;
                }
                boid.Update();
                boid.Draw();
                Map.window.Update();

            }
        }
    }
}
