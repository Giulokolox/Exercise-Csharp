using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_2021
{
    class Enemy : Actor
    {
        public float VisionRadius { get; protected set; }
        protected float shootDistance;

        protected float halfConeAngle = MathHelper.DegreesToRadians(40);
        protected Vector2 pointToReach;

        protected StateMachine fsm;

        public float followSpeed;
        public float walkSpeed;

        public Player Rival;
        public GameObject Target;

        public override int Energy { get => base.Energy; set { base.Energy = value; energyBar.Scale((float)value / (float)base.MaxEnergy); } }

        

        public Enemy() : base("enemy_0")
        {
            // Set RB
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Enemy;
            bulletType = BulletType.EnemyBullet;

            // Pathfinding
            Agent = new Agent(this);

            // FSM Set
            VisionRadius = 5.0f;
            walkSpeed = 1.5f;
            followSpeed = walkSpeed * 2.0f;
            shootDistance = 2.0f;

            fsm = new StateMachine();
            fsm.AddState(StateEnum.WALK, new WalkState(this));
            fsm.AddState(StateEnum.FOLLOW, new FollowState(this));
            fsm.AddState(StateEnum.SHOOT, new ShootState(this));
            fsm.AddState(StateEnum.RECHARGE, new RechargeState(this));
            fsm.GoTo(StateEnum.WALK);

            Reset();
            IsActive = true;

            
        }

        public void ComputeRandomPoint()
        {
            float randX = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoWidth - 2) + 1;
            float randY = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoHeight - 2) + 1;

            pointToReach = new Vector2(randX, randY);
        }

        public void HeadToPoint()
        {
            Vector2 distVect = pointToReach - Position;

            if (distVect.LengthSquared <= 0.01f)
            {
                //ComputeRandomPoint();
                Agent.Target = null;
            }

            //RigidBody.Velocity = distVect.Normalized() * walkSpeed;

            // Pathfinding
            if (Agent.Target == null)
            {
                Node randomNode = ((PlayScene)Game.CurrentScene).map.GetRandomNode();
                List<Node> path = ((PlayScene)Game.CurrentScene).map.GetPath((int)Position.X, (int)Position.Y, (int)randomNode.X, (int)randomNode.Y);
                Agent.SetPath(path);
            }

            Agent.Update(walkSpeed);
        }

        public bool CanAttackPlayer()
        {
            if (Rival == null || !Rival.IsAlive)
            {
                return false;
            }

            Vector2 distVect = Rival.Position - Position;

            return distVect.LengthSquared < shootDistance * shootDistance;
        }

        public void HeadToPlayer()
        {
            //if (Rival !=null)
            //{
            //    Vector2 distVect = Rival.Position - Position;
            //    RigidBody.Velocity = distVect.Normalized() * followSpeed; 
            //}

            // Pathfinding
            if(Agent.Target == null)
            {
                List<Node> path = ((PlayScene)Game.CurrentScene).map.GetPath((int)Position.X, (int)Position.Y, (int)Rival.Position.X, (int)Rival.Position.Y);
                Agent.SetPath(path);
            }
            
            Agent.Update(followSpeed);
            
        }

        public List<Player> GetVisiblePlayers()
        {
            List<Player> players = ((PlayScene)Game.CurrentScene).Players;
            List<Player> visiblePlayers = new List<Player>();

            for (int i = 0; i < players.Count; i++)
            {
                if(!players[i].IsAlive)
                {
                    continue;
                }

                //Vector2 distVect = players[i].Position - Position;

                //if (distVect.LengthSquared < VisionRadius * VisionRadius)
                //{
                //    // Player is inside vision radius
                //    // Check for cone angle
                //    double angleCos = MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1, 1);
                //    float playerAngle = (float)Math.Acos(angleCos);

                //    if (playerAngle < halfConeAngle)
                //    {
                //        visiblePlayers.Add(players[i]);
                //    }
                //}

                foreach(Node n in Agent.SightCone)
                {
                    Node playerNode = ((PlayScene)Game.CurrentScene).map.GetNode((int)(players[i].Position.X + 0.5f), (int)(players[i].Position.Y + 0.5f));
                    
                    if(playerNode == n)
                    {
                        visiblePlayers.Add(players[i]);
                    }
                }
            }

            return visiblePlayers;
        }

        public Player GetBestPlayerToFight()
        {
            Player bestPlayer = null;

            List<Player> visiblePlayers = GetVisiblePlayers();

            if(visiblePlayers.Count > 0)
            {
                // We need to decide only if we currently have 2 Players
                if(visiblePlayers.Count > 1)
                {
                    // Init the FuzzySum variable to -1 (our min value)
                    float maxFuzzy = -1;

                    for (int i = 0; i < visiblePlayers.Count; i++)
                    {
                        // Distance
                        Vector2 distanceFromPlayer = Position - visiblePlayers[i].Position;
                        float fuzzyDistance = 1 - distanceFromPlayer.LengthSquared / (VisionRadius * VisionRadius);

                        // Energy
                        float fuzzyEnergy = 1 - visiblePlayers[i].Energy / visiblePlayers[i].MaxEnergy;

                        // Angle
                        float playerAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(visiblePlayers[i].Forward, distanceFromPlayer.Normalized()), -1, 1));
                        float fuzzyAngle = 1 - (playerAngle / (float)Math.PI);

                        // Sum
                        float fuzzySum = fuzzyDistance + fuzzyEnergy + fuzzyAngle;

                        // Check for best result
                        if(fuzzySum > maxFuzzy)
                        {
                            maxFuzzy = fuzzySum;
                            bestPlayer = visiblePlayers[i];
                        }
                    }
                }
                else
                {
                    // We only have 1 Player
                    bestPlayer = visiblePlayers[0];
                }
            }

            return bestPlayer;
        }

        public virtual PowerUp GetNearestPowerUp()
        {
            PowerUp nearest = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < PowerUpsMngr.PowerUps.Count; i++)
            {
                Vector2 distanceVector;

                if(IsPointVisible(PowerUpsMngr.PowerUps[i].Position, out distanceVector))
                {
                    if(distanceVector.LengthSquared < minDistance)
                    {
                        nearest = PowerUpsMngr.PowerUps[i];
                        minDistance = distanceVector.LengthSquared;
                    }
                }
            }

            return nearest;
        }

        public bool IsPointVisible(Vector2 point, out Vector2 distanceVector)
        {
            distanceVector = point - Position;

            if(distanceVector.LengthSquared <= VisionRadius * VisionRadius)
            {
                float pointAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(Forward, distanceVector.Normalized()), -1.0f, 1.0f));

                if(pointAngle <= halfConeAngle)
                {
                    return true;
                }
            }

            return false;
        }

        public void ShootPlayer()
        {
            //Player player = ((PlayScene)Game.CurrentScene).Player;
            //Vector2 direction = player.Position - Position;
            Shoot(Forward);
        }

        public void LookAtPlayer()
        {
            if (Rival != null)
            {
                Vector2 direction = Rival.Position - Position;
                Forward = direction;
            }
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (RigidBody.Velocity != Vector2.Zero)
                {
                    Forward = RigidBody.Velocity;
                }

                fsm.Update();

                base.Update();
            }
        }

        public override void OnDie()
        {
            IsActive = false;
            energyBar.IsActive = false;
        }

        public override void Draw()
        {
            if(IsActive)
            {
                Agent.Draw();

                base.Draw();
            }
        }
    }
}
