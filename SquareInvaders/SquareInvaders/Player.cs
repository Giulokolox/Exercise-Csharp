using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw;

namespace SquareInvaders
{
    class Player
    {
        private Vector2 position;
        private Vector2 velocity;
        private float speed = 850;
        private float minScreenX;
        private float maxScreenX;
        private float screenOffset = 10;

        Sprite sprite;
        private int halfWidth;
        private int halfHeight;

        private Color color;

        // Boolean used to check the fire key's being pressed once
        private bool isFirePressed;

        private int energy = 3;


        public Player(int baseW, int baseH, int cannonW, int cannonH, Vector2 startPos, Color c)
        {
            sprite = new Sprite("Assets/player.png");

            halfWidth = (int)(sprite.Width * 0.5f);
            halfHeight = (int)(sprite.Height * 0.5f);


            position = startPos;
            color = c;

            minScreenX = screenOffset;
            maxScreenX = Gfx.Window.Width - 1 - screenOffset;

            // Set it to false at the start (not pressed yet)
            isFirePressed = false;
        }

        public void Input()
        {
            if (Gfx.Window.GetKey(KeyCode.D))
            {
                velocity.X = speed;
            }
            else if (Gfx.Window.GetKey(KeyCode.A))
            {
                velocity.X = -speed;
            }
            else
            {
                velocity.X = 0;
            }

            if (Gfx.Window.GetKey(KeyCode.Space))
            {
                // After Space has been pressed, if not set to true yet,
                // set isFirePressed to true, then fire a Bullet
                if(!isFirePressed)
                {
                    isFirePressed = true;

                    Bullet b = BulletMngr.GetBullet();

                    if (b != null)
                    {
                        b.Position = position;

                        b.Position.Y -= 20;
                        b.IsAlive = true;
                    }
                }
            }
            // It is important to set it back to false here,
            // when we know Space key is not pressed anymore
            else
            {
                isFirePressed = false;
            }
        }

        public void Update()
        {
            if (velocity.X != 0)
            {//he's moving
                float deltaX = velocity.X * Gfx.Window.DeltaTime;
                position.X += deltaX;

                if (velocity.X > 0)
                {//moving to right
                    int maxX = (int)position.X + halfWidth;
                    if(maxX > maxScreenX)
                    {
                        position.X = maxScreenX - halfWidth;
                    }
                }
                else
                {//moving to left
                    int minX = (int)position.X - halfWidth;
                    if (minX < minScreenX)
                    {
                        position.X = minScreenX + halfWidth;
                    }
                }
            }
        }

        public void AddDamage(int dmg = 1)
        {
            energy -= dmg;
        }

        public bool IsAlive()
        {
            return energy > 0;
        }

        public void Draw()
        {
            Gfx.DrawSprite(sprite, (int)(position.X - halfWidth), (int)(position.Y - halfHeight));
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public int GetHalfWidth()
        {
            return halfWidth;
        }
    }
}
