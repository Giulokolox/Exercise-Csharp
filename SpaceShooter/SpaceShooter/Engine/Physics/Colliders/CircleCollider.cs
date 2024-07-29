using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SpaceShooter
{
    class CircleCollider : Collider
    {
        public float Radius;

        public CircleCollider(RigidBody owner, float radius) : base(owner)
        {
            Radius = radius;

            DebugMngr.AddItem(this);
        }

        public override bool Collides(Collider other)
        {
            return other.Collides(this);
        }

        public override bool Collides(CircleCollider other)
        {
            Vector2 dist = other.Position - Position;
            return (dist.LengthSquared <= Math.Pow(other.Radius + Radius, 2));
        }

        public override bool Collides(BoxCollider collider)
        {
            return collider.Collides(this);
        }

        public override bool Contains(Vector2 point)
        {
            Vector2 distaFromCenter = point - Position;
            return distaFromCenter.LengthSquared <= (Radius * Radius);
        }

        public override bool Collides(CompoundCollider other)
        {
            return other.Collides(this);
        }
    }
}
