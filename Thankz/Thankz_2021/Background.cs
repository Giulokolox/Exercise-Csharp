using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    class Background : IDrawable
    {
        protected Sprite playground;
        //protected Sprite playground2;
        protected Texture playgroundTexture;

        protected Texture[] textures;
        protected Sprite[] sprites;

        protected int textureRepeat = 4;

        public DrawLayer Layer { get; protected set; }

        public Background(int numTextures)
        {
            Layer = DrawLayer.Background;

            playgroundTexture = new Texture("Assets/bg_2.png");
            playgroundTexture.SetRepeatX(true);
            playground = new Sprite(Game.PixelsToUnits(playgroundTexture.Width * textureRepeat), Game.PixelsToUnits(playgroundTexture.Height));
            playground.position.Y = 4.6f;

            //playground2 = new Sprite(playground.Width, playground.Height);
            //playground2.position.Y = playground.position.Y;
            //playground2.position.X = playground.Width;

            textures = new Texture[numTextures];
            sprites = new Sprite[numTextures];

            float[] positions = new float[] { -2, 3.8f };

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = new Texture($"Assets/bg_{i}.png");
                textures[i].SetRepeatX(true);
                sprites[i] = new Sprite(Game.PixelsToUnits(textures[i].Width * textureRepeat), Game.PixelsToUnits(textures[i].Height));
                sprites[i].position.Y = positions[i];

                //int cloneIndex = i + numTextures;

                //sprites[cloneIndex] = new Sprite(sprites[i].Width, sprites[i].Height);
                //sprites[cloneIndex].position.Y = sprites[i].position.Y;
                //sprites[cloneIndex].position.X = sprites[i].Width;

                //setting cameras
                sprites[i].Camera = CameraMngr.GetCamera($"Bg_{i}");
                //sprites[cloneIndex].Camera = sprites[i].Camera;

            }

            DrawMngr.AddItem(this);
        }


        public void Draw()
        {

            for (int i = 0; i < textures.Length; i++)
            {
                sprites[i].DrawTexture(textures[i],0,0, textures[i].Width*textureRepeat, textures[i].Height);
                //sprites[i + textures.Length].DrawTexture(textures[i]);
            }

            playground.DrawTexture(playgroundTexture,0,0,playgroundTexture.Width*textureRepeat, playgroundTexture.Height);
            //playground2.DrawTexture(playgroundTexture);
        }
    }
}
