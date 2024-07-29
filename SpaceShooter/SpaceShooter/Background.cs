using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter
{
    class Background
    {
        private Sprite head;
        private Sprite tail;
        private Texture texture;

        private Vector2 velocity;

        public Background()
        {
            texture = new Texture("Assets/spaceBg.png");
            head = new Sprite(texture.Width, texture.Height);
            tail = new Sprite(texture.Width, texture.Height);

            velocity.X = -800.0f;
        }

        public void Update()
        {
            head.position.X += velocity.X * Game.DeltaTime;

            if(head.position.X <= -head.Width)
            {
                head.position.X += head.Width;
            }

            tail.position.X = head.position.X + head.Width;
        }

        public void Draw()
        {
            head.DrawTexture(texture);
            tail.DrawTexture(texture);
        }
    }
}
