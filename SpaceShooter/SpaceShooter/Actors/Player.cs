using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter
{
    class Player : Actor
    {
        private bool isFirePressed;
        protected ProgressBar nrgBar;
        protected TextObject playerName;
        protected int playerId;
        protected TextObject scoreText;
        protected int score;

        protected Controller controller;

        public override int Energy { get => base.Energy; set { base.Energy = value; nrgBar.Scale((float)value / (float)maxEnergy); } }

        public Player(Controller ctrl, int id = 0): base("player")
        {
            IsActive = true;
            maxSpeed = 250;
            shootOffset = new Vector2(sprite.pivot.X + 10.0f, sprite.pivot.Y - 10.0f);
            bulletType = BulletType.PlayerBullet;

            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy);

            nrgBar = new ProgressBar("barFrame", "blueBar", new Vector2(4.0f, 4.0f));
            nrgBar.Position = new Vector2(60.0f, 50.0f);

            Vector2 playerNamePos = nrgBar.Position + new Vector2(0, -20);
            playerName = new TextObject(playerNamePos, $"Player {playerId + 1}", FontMngr.GetFont(),5);
            playerName.IsActive = true;

            Vector2 scorePos = nrgBar.Position + new Vector2(0, 24);
            scoreText = new TextObject(scorePos, "", FontMngr.GetFont(),5);
            scoreText.IsActive = true;
            UpdateScore();

            shootVel = new Vector2(1000.0f, 0.0f);
            tripleShootAngle = MathHelper.DegreesToRadians(15.0f);

            Reset();

            controller = ctrl;

            Forward = new Vector2(0f, 0f);

            Console.WriteLine(Forward);
        }

        protected void UpdateScore()
        {
            scoreText.Text = score.ToString("00000000");
        }

        public void AddScore(int points)
        {
            score += points;
            UpdateScore();
        }

        public void Input()
        {
            Vector2 direction = new Vector2(controller.GetHorizontal(), controller.GetVertical());

            if(direction.Length > 1)
            {
                direction.Normalize();
            }

            RigidBody.Velocity = direction * maxSpeed;

            if(controller.IsFirePressed())
            {
                if(!isFirePressed)
                {
                    isFirePressed = true;
                    Shoot();
                }
            }
            else if(isFirePressed)
            {
                isFirePressed = false;
            }


            //// Horizontal Movement
            //if(Game.Window.GetKey(KeyCode.D) && Position.X + sprite.Width * 0.5f <= Game.Window.Width)
            //{
            //    RigidBody.Velocity.X = maxSpeed;
            //}
            //else if(Game.Window.GetKey(KeyCode.A) && Position.X - sprite.Width * 0.5f >= 0)
            //{
            //    RigidBody.Velocity.X = -maxSpeed;
            //}
            //else
            //{
            //    RigidBody.Velocity.X = 0.0f;
            //}

            //// Vertical Movement
            //if (Game.Window.GetKey(KeyCode.S) && Position.Y + sprite.Height * 0.5f <= Game.Window.Height)
            //{
            //    RigidBody.Velocity.Y = maxSpeed;
            //}
            //else if (Game.Window.GetKey(KeyCode.W) && Position.Y - sprite.Height * 0.5f >= 0)
            //{
            //    RigidBody.Velocity.Y = -maxSpeed;
            //}
            //else
            //{
            //    RigidBody.Velocity.Y = 0.0f;
            //}

            //// If direction is diagonal force velocity length to speed
            //if(RigidBody.Velocity.X != 0 && RigidBody.Velocity.Y != 0)
            //{
            //    RigidBody.Velocity = RigidBody.Velocity.Normalized() * maxSpeed;
            //}

            //if(Game.Window.GetKey(KeyCode.Space))
            //{
            //    if(!isFirePressed)
            //    {
            //        isFirePressed = true;
            //        Shoot();
            //    }
            //}
            //else if(isFirePressed)
            //{
            //    isFirePressed = false;
            //}
        }

        public override void OnCollide(GameObject other)
        {
            SpawnMngr.RestoreEnemy(((Enemy)other));
            AddDamage(30);
        }

        public override void ChangeWeapon(WeaponType weapon)
        {
            base.ChangeWeapon(weapon);

            if(weaponType == WeaponType.TripleShoot)
            {
                bulletType = BulletType.GreenGlobeBullet;
            }
            else
            {
                bulletType = BulletType.PlayerBullet;
            }
        }


    }
}
