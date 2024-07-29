using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Boids
{
    static class RandomGenerator
    {
        private static Random rand;

        static RandomGenerator()
        {
            rand = new Random();
        }

        public static int RandomNumber()
        {
            return rand.Next(-90, 90);
        }

        public static Vector2 RandomForward()
        {
            return new Vector2(RandomNumber(), RandomNumber());
        }
    }
}
