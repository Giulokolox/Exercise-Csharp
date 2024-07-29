using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    abstract class PowerUp : GameObject
    {
        protected Player attachedPlayer;

        public PowerUp(string textureName) : base(textureName)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.PowerUp;
            RigidBody.IsCollisionAffected = true;

            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Player);

            RigidBody.Velocity = new OpenTK.Vector2(-300.0f, 0.0f);
        }

        public virtual void OnAttach(Player p)
        {
            attachedPlayer = p;
            IsActive = false;
        }

        public virtual void OnDetach()
        {
            attachedPlayer = null;
            PowerUpsMngr.RestorePowerUp(this);
        }

        public override void Update()
        {
            if (IsActive)
            {
                if(Position.X + HalfWidth < 0)
                {
                    PowerUpsMngr.RestorePowerUp(this);
                }
            }
        }

        public override void OnCollide(GameObject other)
        {
            OnAttach((Player)other);
        }
    }
}
