using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads_2021
{
    static class PowerUpsMngr
    {
        public static List<PowerUp> PowerUps { get; private set; }
        private static int listSize = 6;
        private static float nextSpawn;

        public static void Init()
        {
            PowerUps = new List<PowerUp>(listSize);

            for (int i = 0; i < listSize; i++)
            {
                PowerUps.Add(new PowerUp());
            }

            nextSpawn = RandomGenerator.GetRandomFloat() * 2 + 3;
        }

        public static void Update()
        {
            nextSpawn -= Game.DeltaTime;

            if(nextSpawn <= 0)
            {
                SpawnPowerUp();
                nextSpawn = RandomGenerator.GetRandomFloat() * 3 + 3;
            }
        }

        public static void SpawnPowerUp()
        {
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if(!PowerUps[i].IsActive)
                {
                    PowerUps[i].Position = GetRandomPoint();
                    PowerUps[i].IsActive = true;
                    break;
                }
            }
        }

        private static Vector2 GetRandomPoint()
        {
            //float randX = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoWidth - 2) + 1;
            //float randY = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoHeight - 2) + 1;
            Node rand = null;

            do
            {
                rand = ((PlayScene)Game.CurrentScene).map.GetRandomNode();
            } while (rand.Cost == int.MaxValue || rand.X < 1 || rand.X > Game.Window.OrthoWidth - 2 || rand.Y < 1 || rand.Y > Game.Window.OrthoHeight - 2);

            return new Vector2(rand.X, rand.Y);
        }
    }
}
