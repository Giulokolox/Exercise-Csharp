using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Bulletz_2022
{
    class Player : Actor
    {
        private bool isFirePressed;
        protected ProgressBar nrgBar;
        protected TextObject playerName;
        protected int playerId;
        protected TextObject scoreText;
        protected int score;

        protected float jumpSpeed = -8;
        protected bool isJumpPressed;

        protected Controller controller;

        public bool IsJumping { get { return RigidBody.Velocity.Y < 0; } }
        public bool IsFalling { get { return RigidBody.Velocity.Y > 0; } }
        public bool IsGrounded { get { return RigidBody.Velocity.Y == 0; } }

        public override int Energy { get => base.Energy; set { base.Energy = value; nrgBar.Scale((float)value / (float)maxEnergy); } }

        public Player(Controller ctrl, int id = 0) : base("player", Game.PixelsToUnits(58), Game.PixelsToUnits(58))
        {
            IsActive = true;
            maxSpeed = 6;
            shootOffset = Vector2.Zero;//new Vector2(sprite.pivot.X + 10.0f, sprite.pivot.Y - 10.0f);
            bulletType = BulletType.PlayerBullet;

            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy);
            RigidBody.IsGravityAffected = true;

            nrgBar = new ProgressBar("barFrame", "blueBar", new Vector2(Game.PixelsToUnits(4.0f), Game.PixelsToUnits(4.0f)));
            nrgBar.Position = new Vector2(1.0f, 1.5f);

            Vector2 playerNamePos = nrgBar.Position + new Vector2(0, -20);
            playerName = new TextObject(playerNamePos, $"Player {playerId + 1}", FontMngr.GetFont(), 5);
            playerName.IsActive = true;

            Vector2 scorePos = nrgBar.Position + new Vector2(0, 24);
            scoreText = new TextObject(scorePos, "", FontMngr.GetFont(), 5);
            scoreText.IsActive = true;
            UpdateScore();

            shootVel = new Vector2(1000.0f, 0.0f);
            tripleShootAngle = MathHelper.DegreesToRadians(15.0f);

            Reset();

            controller = ctrl;

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

        public virtual void Jump()
        {
            RigidBody.Velocity.Y = jumpSpeed;
        }

        public override void Update()
        {
            float groundY = ((PlayScene)Game.CurrentScene).GroundY;

            if (Position.Y + HalfHeight > groundY)
            {
                sprite.position.Y = groundY - HalfHeight;
                RigidBody.Velocity.Y = 0;
            }
        }

        public void Input()
        {
            //Vector2 direction = new Vector2(controller.GetHorizontal(), controller.GetVertical());

            //if(direction.Length > 1)
            //{
            //    direction.Normalize();
            //}

            RigidBody.Velocity.X = controller.GetHorizontal() * maxSpeed;

            if (controller.IsFirePressed())
            {
                if (!isFirePressed)
                {
                    isFirePressed = true;
                    Vector2 mouseAbsolutePosition = CameraMngr.MainCamera.position - CameraMngr.MainCamera.pivot + Game.Window.MousePosition;
                    Vector2 direction = (mouseAbsolutePosition - Position).Normalized();
                    Shoot(direction);
                    spriteOffsetX = frameWidth;
                }
            }
            else if (isFirePressed)
            {
                isFirePressed = false;
                spriteOffsetX = 0;
            }

            if (controller.IsJumpPressed())
            {
                if (!isJumpPressed)
                {
                    isJumpPressed = true;
                    if (IsGrounded)
                    {
                        Jump();
                    }
                }
            }
            else if (isJumpPressed)
            {
                isJumpPressed = false;
            }
        }

        public override void OnCollide(GameObject other)
        {
            //SpawnMngr.RestoreEnemy(((Enemy)other));
            //AddDamage(30);
        }

        public override void ChangeWeapon(WeaponType weapon)
        {
            base.ChangeWeapon(weapon);

            if (weaponType == WeaponType.TripleShoot)
            {
                bulletType = BulletType.PlayerBullet;
            }
            else
            {
                bulletType = BulletType.PlayerBullet;
            }
        }

    }
}
