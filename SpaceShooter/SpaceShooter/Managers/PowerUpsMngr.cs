using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SpaceShooter
{
    static class PowerUpsMngr
    {
        static List<PowerUp> items;

        static float nextSpawn;

        public static void Init()
        {
            items = new List<PowerUp>();

            items.Add(new EnergyPowerUp());
            items.Add(new TripleBulletPowerUp());
        }

        public static void Update()
        {
            nextSpawn -= Game.Window.DeltaTime;

            if(nextSpawn <= 0)
            {
                SpawnPowerUp();
                nextSpawn = RandomGenerator.GetRandomFloat() * 8 + 2;
            }
        }

        public static void SpawnPowerUp()
        {
            if(items.Count > 0)
            {
                int randomIndex = RandomGenerator.GetRandomInt(0, items.Count);

                PowerUp randPowerUp = items[randomIndex];
                items.RemoveAt(randomIndex);

                randPowerUp.Position = new Vector2(Game.Window.Width + randPowerUp.HalfWidth,
                                                   RandomGenerator.GetRandomInt(randPowerUp.HalfHeight, Game.Window.Height - randPowerUp.HalfHeight));

                randPowerUp.IsActive = true;
            }
        }

        public static void RestorePowerUp(PowerUp p)
        {
            p.IsActive = false;
            items.Add(p);
        }

        public static void Clear()
        {
            items.Clear();
        }
    }
}
