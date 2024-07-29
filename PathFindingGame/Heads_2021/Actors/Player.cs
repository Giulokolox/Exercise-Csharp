using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_2021
{
    class Player : Actor
    {
        // References
        protected Controller controller;
        //protected ProgressBar nrgBar;
        //protected TextObject playerName;
        // Variables
        protected int playerId;
        private bool isFirePressed;
        // Properties
        public override int Energy { get => base.Energy; set { base.Energy = value; energyBar.Scale((float)value / (float)base.MaxEnergy); } }
        //public int MaxEnergy { get { return base.MaxEnergy; } }
        public bool IsGrounded { get { return RigidBody.Velocity.Y == 0; } }



        public Player(Controller ctrl, int id = 0) : base("player_1")
        {
            // Misc
            controller = ctrl;
            maxSpeed = 4;
            bulletType = BulletType.PlayerBullet;
            IsActive = true;
            // RB
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.Tile | RigidBodyType.Player);
            RigidBody.Friction = 40;
            // Player ID and Name
            playerId = id;
            //Vector2 playerNamePos = new Vector2(1, 1);
            //playerName = new TextObject(playerNamePos, $"Player {playerId + 1}", FontMngr.GetFont(), 5);
            //playerName.IsActive = true;

            Reset();
        }

        public void Input()
        {
            Vector2 direction = new Vector2(controller.GetHorizontal(), controller.GetVertical());

            if(direction != Vector2.Zero)
            {
                RigidBody.Velocity = direction.Normalized() * maxSpeed;
            }

            // SHOOT
            if (controller.IsFirePressed())
            {
                if (!isFirePressed)
                {
                    isFirePressed = true;
                    Shoot(Forward);
                }
            }
            else if (isFirePressed)
            {
                isFirePressed = false;
            }
        }

        public override void OnDie()
        {
            IsActive = false;
            energyBar.IsActive = false;
            ((PlayScene)Game.CurrentScene).OnPlayerDies();
        }
    }
}
