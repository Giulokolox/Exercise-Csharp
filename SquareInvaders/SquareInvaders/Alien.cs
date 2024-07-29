using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    class Alien
    {
        private Vector2 position;
        private Vector2 velocity;
        private int halfWidth;
        private int halfHeight;
        private Color color;
        private int visiblePixels;

        private bool isAlive;
        private bool isVisible;

        private float minScreenX;
        private float maxScreenX;
        private float screenOffset = 10;

        private Pixel[] sprite;
        int pixelSize;

        public bool CanShoot;
        private float nextShoot;

        public Alien(Vector2 pos, int w, int h, Color col)
        {
            this.position = pos;
            this.halfWidth = (int)(w * 0.5f);
            this.halfHeight = (int)(h * 0.5f);
            this.color = col;

            velocity.X = 200;

            isAlive = true;
            isVisible = true;

            minScreenX = screenOffset;
            maxScreenX = Gfx.Window.Width - 1 -screenOffset;

            // CREATE ALIEN SPRITE

            // This is the byte array we'll use as our Alien's Pixels Texture (1 = there is a pixel; 0 = no pixel)
            byte[] pixels = { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0,
                              0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
                              0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                              0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0,
                              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                              1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1,
                              1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1,
                              0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0
            };

            // Calculate the total number of pixels we'll use
            int numPixels = 0;

            for (int i = 0; i < pixels.Length; i++)
            {
                if(pixels[i] == 1)
                {
                    numPixels++;
                }
            }

            // Create the array of Pixels
            sprite = new Pixel[numPixels];
            visiblePixels = numPixels;

            // Useful variables
            int verticalPixels = 8;
            int horizontalPixels = 11;
            pixelSize = (halfHeight * 2) / verticalPixels;
            halfWidth = (int)((horizontalPixels * pixelSize) * 0.5f);

            float startX = position.X - halfWidth;
            float startY = position.Y - halfHeight;

            // Start adding new pixels to the array, calculating each one's position
            int index = 0;

            for (int i = 0; i < pixels.Length; i++)
            {
                if(i != 0 && i % horizontalPixels == 0)
                {
                    startY += pixelSize;
                }

                if(pixels[i] != 0)
                {
                    float x = startX + (i % horizontalPixels) * pixelSize;
                    sprite[index] = new Pixel(new Vector2(x, startY), pixelSize, color);
                    index++;
                }
            }
        }

        public bool Update(ref float overflowX)
        {
            //bool endReached = false;
            
            if(isAlive)
            {
                return UpdateMovement(ref overflowX);
            }
            else if (isVisible)
            {
                for (int i = 0; i < sprite.Length; i++)
                {
                    if (sprite[i].IsAlive)
                    {
                        sprite[i].Update();

                        if (!sprite[i].IsAlive)
                        {
                            visiblePixels--;
                            if (visiblePixels <= 0)
                            {
                                isVisible = false;
                            }
                        }
                    }
                }
            }

            return false;

            //return endReached;
        }

        private bool UpdateMovement(ref float overflowX)
        {
            bool endReached = false;

            float deltaX = velocity.X * Gfx.Window.DeltaTime;
            float deltaY = velocity.Y * Gfx.Window.DeltaTime;

            position.X += deltaX;
            position.Y += deltaY;

            float maxX = position.X + halfWidth;
            float minX = position.X - halfWidth;

            if (maxX > maxScreenX)
            {
                overflowX = maxX - maxScreenX;
                endReached = true;
            }
            else if (minX < minScreenX)
            {
                overflowX = minX - minScreenX;
                endReached = true;
            }

            UpdateSprite(new Vector2(deltaX, deltaY));

            if (CanShoot)
            {
                nextShoot -= Gfx.Window.DeltaTime;

                if (nextShoot <= 0)
                {
                    Shoot();
                    ResetCounter();
                }
            }

            return endReached;
        }

        public void ResetCounter()
        {
            nextShoot = RandomGenerator.GetRandomFloat() * 1 + 3;
        }

        public void UpdateSprite(Vector2 deltaVector)
        {
            // Move all the pixels of the sprite
            for (int i = 0; i < sprite.Length; i++)
            {
                sprite[i].Translate(deltaVector.X, deltaVector.Y);
            }
        }

        public void ChangeDir(float overflowX)
        {
            position.X -= overflowX;
            position.Y += halfHeight + pixelSize;
            
            UpdateSprite(new Vector2(-overflowX, halfHeight + pixelSize));

            velocity.X = -velocity.X;
        }

        private void Shoot()
        {
            AlienBullet b = BulletMngr.GetAlienBullet();

            if(b != null)
            {
                // Create a startPos V2 and set it as the Alien fire position (Middle - Bottom spot)
                Vector2 shootPos = position;
                shootPos.Y += halfHeight;   // Add alien halfHeight to reach the bottom
                b.Shoot(shootPos, new Vector2(0.0f, 450.0f));
            }
        }

        public bool IsVisible()
        {
            return isVisible;
        }

        public void OnHit()
        {
            isAlive = false;
            Explode();
        }

        private void Explode()
        {
            for (int i = 0; i < sprite.Length; i++)
            {
                //v = pixel position - Alien position
                Vector2 pixelVel = sprite[i].GetPosition();
                pixelVel.Sub(position);

                pixelVel.X *= RandomGenerator.GetRandomInt(4, 15);
                pixelVel.Y *= RandomGenerator.GetRandomInt(4, 23);
                pixelVel.Mul(4);
                sprite[i].Velocity = pixelVel;
            }
        }

        public void Draw()
        {
            if(isVisible)
            {
                //Gfx.DrawRect((int)position.X - halfWidth, (int)position.Y - halfHeight, halfWidth * 2, halfHeight * 2, color);

                for (int i = 0; i < sprite.Length; i++)
                {
                    sprite[i].Draw();
                }
            }
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public int GetHalfWidth()
        {
            return halfWidth;
        }

        public bool GetIsAlive()
        {
            return isAlive;
        }
    }
}
