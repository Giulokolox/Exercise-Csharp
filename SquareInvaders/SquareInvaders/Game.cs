using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    static class Game
    {
        public static Player Player;
        public static float Gravity;

        public static void Init()
        {
            Gfx.Init("Square Invaders");
            EnemyMngr.Init(24, 8);
            BulletMngr.Init(10, 30);

            Gravity = 1500f;

            Player = new Player(50, 15, 8, 18, new Vector2(Gfx.Window.Width * 0.5f, Gfx.Window.Height - 100), ColorsFactory.GetColor(ColorType.White));
        }

        public static void Play()
        {
            while (Gfx.Window.IsOpened && Player.IsAlive())
            {

                //Input
                Player.Input();

                //Update
                Player.Update();
                EnemyMngr.Update();
                BulletMngr.Update();

                //Draw
                Gfx.ClearScreen();

                Player.Draw();

                EnemyMngr.Draw();
                BulletMngr.Draw();

                Gfx.Window.Blit();
            }
        }
    }
}
