using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    abstract class Actor : GameObject
    {
        // Variables
        protected BulletType bulletType;
        protected int energy;
        protected float maxSpeed;
        // Propertiex
        public bool IsAlive { get { return energy > 0; } }
        public int MaxEnergy { get; protected set; }
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, MaxEnergy); } }

        protected ProgressBar energyBar;

        public Bullet LastShotBullet { get; protected set; }

        public Actor(string texturePath, float w = 0, float h = 0) : base(texturePath, w: w, h: h)
        {
            float unitDist = Game.PixelsToUnits(4);
            energyBar = new ProgressBar("barFrame", "blueBar", new Vector2(unitDist));
            energyBar.Position = new Vector2(1, 1);
            energyBar.IsActive = true;
            MaxEnergy = 100;

            RigidBody = new RigidBody(this);
            RigidBody.IsGravityAffected = true;
        }

        protected virtual void Shoot(Vector2 velocity, Vector2 position)
        {
            Bullet b = BulletMngr.GetBullet(bulletType);

            if (b != null)
            {
                b.IsActive = true;
                position += velocity.Normalized() * (b.HalfWidth);
                b.Shoot(position, velocity);
            }

            LastShotBullet = b;
        }

        public override void Update()
        {
            if (IsActive)
            {
                PlayScene playScene = (PlayScene)Game.CurrentScene;

                if (Position.Y > playScene.GroundY)
                {
                    Y = ((PlayScene)Game.CurrentScene).GroundY;
                    RigidBody.Velocity.Y = 0;
                }

                X = MathHelper.Clamp(X, playScene.MinX, playScene.MaxX);
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            //SpawnMngr.RestoreEnemy(((Enemy)other));
            //AddDamage(30);
            //OnWallCollides(collisionInfo);
        }

        protected virtual void OnWallCollides(Collision collisionInfo)
        {
            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                // Horizontal Collision
                if (X < collisionInfo.Collider.X)
                {
                    // Collision from Left (inverse horizontal delta)
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                X += collisionInfo.Delta.X;
                RigidBody.Velocity.X = 0.0f;
            }
            else
            {
                // Vertical Collision
                if (Y < collisionInfo.Collider.Y)
                {
                    // Collision from Top
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    RigidBody.Velocity.Y = 0.0f;
                }
                else
                {
                    // Collision from Bottom
                    RigidBody.Velocity.Y = -RigidBody.Velocity.Y * 0.8f;
                }

                Y += collisionInfo.Delta.Y;
            }
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                OnDie();
            }
        }

        public virtual void AddEnergy(int amount)
        {
            Energy = Math.Min(Energy + amount, MaxEnergy);
        }

        public abstract void OnDie();

        public virtual void Reset()
        {
            Energy = MaxEnergy;
        }

    }
}

