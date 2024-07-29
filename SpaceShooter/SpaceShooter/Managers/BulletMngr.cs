using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    static class BulletMngr
    {
        private static Queue<Bullet>[] bullets;
        //private static List<Bullet>[] activeBullets;

        public static void Init()
        {
            int queueSize = 16;

            bullets = new Queue<Bullet>[(int)BulletType.LAST];

            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Queue<Bullet>(queueSize);

                switch ((BulletType)i)
                {
                    case BulletType.PlayerBullet:

                        for (int q = 0; q < queueSize; q++)
                        {
                            bullets[i].Enqueue(new PlayerBullet());
                        }

                        break;

                    case BulletType.PurpleBullet:

                        for (int q = 0; q < queueSize; q++)
                        {
                            bullets[i].Enqueue(new PurpleBullet());
                        }

                        break;

                    case BulletType.FireglobeBullet:

                        for (int q = 0; q < queueSize; q++)
                        {
                            bullets[i].Enqueue(new FireGlobeBullet());
                        }

                        break;
                    case BulletType.GreenGlobeBullet:

                        for (int q = 0; q < queueSize; q++)
                        {
                            bullets[i].Enqueue(new GreenGlobeBullet());
                        }

                        break;
                }
            }
        }

        public static Bullet GetBullet(BulletType type)
        {
            int index = (int)type;

            if (bullets[index].Count > 0)
            {
                Bullet bullet = bullets[index].Dequeue();
                bullet.IsActive = true;

                //UpdateMngr.AddItem(bullet);
                //DrawMngr.AddItem(bullet);

                return bullet;
            }

            return null;
        }

        public static void RestoreBullet(Bullet bullet)
        {
            bullet.Reset();
            bullets[(int)bullet.Type].Enqueue(bullet);

            //UpdateMngr.RemoveItem(bullet);
            //DrawMngr.RemoveItem(bullet);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Clear();
            }
        }
    }
}
