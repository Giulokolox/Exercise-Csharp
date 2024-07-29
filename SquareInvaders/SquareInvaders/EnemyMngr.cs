using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    static class EnemyMngr
    {
        private static Alien[] aliens;
        private static int numAliens;
        private static int aliensPerRow;
        private static int aliensPerCols;
        private static int alienWidth;
        private static int alienHeight;

        public static void Init(int totAliens, int maxPerRow)
        {
            numAliens = totAliens;
            aliensPerRow = maxPerRow;
            aliensPerCols = totAliens / maxPerRow;

            aliens = new Alien[numAliens];

            alienWidth = 55;
            alienHeight = 40;
            int startX = 40;
            int startY = 40;
            int dist = 5;

            int lastRowIndex = numAliens - aliensPerCols;

            for (int i = 0; i < aliens.Length; i++)
            {
                aliens[i] = new Alien(new Vector2(startX + (i % aliensPerRow) * (alienWidth + dist),
                                                  startY + (i / aliensPerRow) * (alienHeight + dist)),
                                                  alienWidth,
                                                  alienHeight,
                                                  ColorsFactory.GetColor(ColorType.Green));

                if(i >= lastRowIndex)
                {
                    aliens[i].CanShoot = true;
                    aliens[i].ResetCounter();
                }
            }
        }

        public static void Update()
        {
            bool endReached = false;
            float overflowX = 0;

            for (int i = 0; i < aliens.Length; i++)
            {
                if((aliens[i].GetIsAlive() || aliens[i].IsVisible()) && aliens[i].Update(ref overflowX))
                {
                    endReached = true;
                }
            }

            if(endReached)
            {
                for (int i = 0; i < aliens.Length; i++)
                {
                    if (aliens[i].GetIsAlive())
                    {
                        aliens[i].ChangeDir(overflowX); 
                    }
                }
            }
        }

        public static bool CollideWithBullet(Bullet bullet)
        {
            for (int i = 0; i < aliens.Length; i++)
            {
                if(aliens[i].GetIsAlive())
                {
                    if(bullet.Collides(aliens[i].GetPosition(), aliens[i].GetHalfWidth()))
                    {
                        aliens[i].OnHit();

                        // Search for the next alien that can shoot now
                        int prevAlienindex = i - aliensPerRow;

                        while(prevAlienindex >= 0)
                        {
                            if(aliens[prevAlienindex].GetIsAlive())
                            {
                                aliens[prevAlienindex].CanShoot = true;
                                aliens[prevAlienindex].ResetCounter();
                                break;
                            }

                            prevAlienindex -= aliensPerRow;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        public static void Draw()
        {
            for (int i = 0; i < aliens.Length; i++)
            {
                aliens[i].Draw();
            }
        }
    }
}
