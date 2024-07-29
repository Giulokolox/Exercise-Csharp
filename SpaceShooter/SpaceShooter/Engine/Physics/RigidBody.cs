using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SpaceShooter
{
    enum RigidBodyType { Player = 1, PlayerBullet = 2, Enemy = 4, EnemyBullet = 8, PowerUp = 16 }

    class RigidBody
    {
        public Vector2 Velocity;

        public GameObject GameObject;   // Owner
        //public bool IsGravityAffected;
        public bool IsCollisionAffected = true;

        public Collider Collider;

        public RigidBodyType Type;

        protected uint collisionMask;

        public bool IsActive { get { return GameObject.IsActive; } }

        public Vector2 Position { get { return GameObject.Position; } }

        public RigidBody(GameObject owner)
        {
            GameObject = owner;
            PhysicsMngr.AddItem(this);
        }

        public void Update()
        {
            GameObject.Position += Velocity * Game.DeltaTime;
        }

        public bool Collides(RigidBody other)
        {
            return Collider.Collides(other.Collider);
        }

        public void AddCollisionType(RigidBodyType type)
        {
            collisionMask |= (uint)type;
        }

        public void AddCollisionType(uint type)
        {
            collisionMask |= type;
        }

        public bool CollisionTypeMatches(RigidBodyType type)
        {
            return ((uint)type & collisionMask) != 0;
        }

        public void Destroy()
        {
            GameObject = null;
            Collider = null;
            //TODO: remove colliders
            PhysicsMngr.RemoveItem(this);
        }

    }
}
