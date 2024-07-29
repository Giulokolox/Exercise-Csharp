using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    static class BulletMngr
    {
        private static Bullet[] bullets;
        private static AlienBullet[] alienBullets;

        private static int totBullets;
        private static int totAlienBullets;

        // Init method in a static class allows us to decide when
        // the class itself is going to be initialized
        public static void Init(int maxBullets, int maxAlienBullets)
        {
            totBullets = maxBullets;
            bullets = new Bullet[totBullets];

            for (int i = 0; i < totBullets; i++)
            {
                bullets[i] = new Bullet();
            }

            totAlienBullets = maxAlienBullets;
            alienBullets = new AlienBullet[totAlienBullets];
            
            for (int i = 0; i < totAlienBullets; i++)
            {
                alienBullets[i] = new AlienBullet();
            }
        }

        public static Bullet GetBullet()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].IsAlive)
                {
                    return bullets[i];
                }
            }

            return null;
        }

        public static AlienBullet GetAlienBullet()
        {
            for (int i = 0; i < totAlienBullets; i++)
            {
                if (!alienBullets[i].IsAlive)
                {
                    return alienBullets[i];
                }
            }

            return null;
        }

        public static void Update()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].IsAlive)
                {
                    bullets[i].Update();

                    if(EnemyMngr.CollideWithBullet(bullets[i]))
                    {
                        bullets[i].IsAlive = false;
                    }
                }
            }

            for (int i = 0; i < alienBullets.Length; i++)
            {
                if (alienBullets[i].IsAlive)
                {
                    alienBullets[i].Update();

                    // We need a Ref to the Player to check if any AlienBullet collided with it
                    if (alienBullets[i].Collides(Game.Player.GetPosition(), Game.Player.GetHalfWidth()))
                    {
                        alienBullets[i].IsAlive = false;
                        Game.Player.AddDamage();
                    }
                }
            }
        }

        public static void Draw()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].IsAlive)
                {
                    bullets[i].Draw();
                }
            }

            for (int i = 0; i < alienBullets.Length; i++)
            {
                if (alienBullets[i].IsAlive)
                {
                    alienBullets[i].Draw();
                }
            }
        }
    }
}
