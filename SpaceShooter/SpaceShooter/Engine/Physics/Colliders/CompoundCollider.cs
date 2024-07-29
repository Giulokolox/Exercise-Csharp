using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SpaceShooter
{
    class CompoundCollider : Collider
    {
        public Collider BoundingCollider;
        protected List<Collider> colliders;

        public CompoundCollider(RigidBody owner, Collider boundingCollider) : base(owner)
        {
            BoundingCollider = boundingCollider;
            colliders = new List<Collider>();

            if(BoundingCollider is CircleCollider)
            {
                Offset = Vector2.Zero;
            }

            if (BoundingCollider is BoxCollider)
            {
                Offset = new Vector2(((BoxCollider)BoundingCollider).Width * -0.5f, ((BoxCollider)BoundingCollider).Height * -0.5f);
            }
        }

        public virtual void AddCollider(Collider collider)
        {
            colliders.Add(collider);
        }

        public virtual bool InnerCollidersCollide(Collider collider)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if(collider.Collides(colliders[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Collides(Collider other)
        {
            return other.Collides(this);
        }

        public override bool Collides(BoxCollider other)
        {
            return (other.Collides(BoundingCollider) && InnerCollidersCollide(other));
        }

        public override bool Collides(CircleCollider other)
        {
            return (other.Collides(BoundingCollider) && InnerCollidersCollide(other));
        }

        public override bool Collides(CompoundCollider other)
        {
            if(BoundingCollider.Collides(other.BoundingCollider))
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    for (int j = 0; j < other.colliders.Count; j++)
                    {
                        if(colliders[i].Collides(other.colliders[j]))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override bool Contains(Vector2 point)
        {
            throw new NotImplementedException();
        }
    }
}
