using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter
{
    class GameObject : IUpdatable, IDrawable
    {
        protected Sprite sprite;
        protected Texture texture;

        public RigidBody RigidBody;
        public bool IsActive;

        public virtual Vector2 Position { get { return sprite.position; } set { sprite.position = value; } }

        public int HalfWidth { get { return (int)(sprite.Width * 0.5f); } protected set { } }
        public int HalfHeight { get { return (int)(sprite.Height * 0.5f); } protected set { } }

        public Vector2 Forward
        { 
            get
            {
                return new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation));
            }
            set
            {
                sprite.Rotation = (float)Math.Atan2(value.Y, value.X);
            }
        }

        public GameObject(string textureName, int w = 0, int h = 0)
        {
            texture = GfxMngr.GetTexture(textureName);

            int spriteW = w > 0 ? w : texture.Width;
            int spriteH = h > 0 ? h : texture.Height;

            sprite = new Sprite(spriteW, spriteH);

            //HalfWidth = (int)(sprite.Width * 0.5f);
            //HalfHeight = (int)(sprite.Height * 0.5f);

            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public virtual void Update()
        {
            //sprite.position += velocity * Game.DeltaTime;
        }

        public virtual void OnCollide(GameObject other)
        {

        }

        public virtual void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture);
            }
        }

        public virtual void Destroy()
        {
            sprite = null;
            texture = null;

            UpdateMngr.RemoveItem(this);
            DrawMngr.RemoveItem(this);

            if (RigidBody != null)
            {
                RigidBody.Destroy();
            }
        }

    }
}
