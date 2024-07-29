using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    class Tile : GameObject
    {
        public Tile(string textureName = "crate", DrawLayer layer = DrawLayer.Playground) : base(textureName, layer)
        {
            //sprite.pivot = OpenTK.Vector2.Zero;
            
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Tile;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            IsActive = true;


            DebugMngr.AddItem(RigidBody.Collider);
        }
    }
}
