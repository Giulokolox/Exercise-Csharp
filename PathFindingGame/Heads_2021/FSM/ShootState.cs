using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads_2021
{
    class ShootState : State
    {
        private Enemy enemy;

        private float shootTimeLimit = 0.25f;
        private float shootCoolDown = 0.0f;

        private RandomTimer checkForNewPlayer;
        private RandomTimer checkForPowerUp;

        public ShootState(Enemy enemy)
        {
            this.enemy = enemy;
            checkForNewPlayer = new RandomTimer(0.2f, 1.2f);
            checkForPowerUp = new RandomTimer(0.4f, 1.35f);
        }

        public override void OnEnter()
        {
            this.enemy.RigidBody.Velocity = Vector2.Zero;
        }

        protected virtual bool ContinueAttack(PowerUp nearestPowerUp)
        {
            //float rechargeDistFuzzy = 1 - (nearestPowerUp.Position - enemy.Position).LengthSquared / (enemy.VisionRadius * enemy.VisionRadius);
            float rechargeNrgFuzzy = 1 - (float)enemy.Energy / (float)enemy.MaxEnergy;
            float rechargeSum = rechargeNrgFuzzy;

            //float attackDistFuzzy = 1 - (enemy.Rival.Position - enemy.Position).LengthSquared / (enemy.VisionRadius * enemy.VisionRadius);
            float attackNrgFuzzy = Math.Min((float)enemy.Energy / (float)enemy.Rival.Energy, 1);
            float attackSum = attackNrgFuzzy;

            return attackSum > rechargeSum;
        }

        public override void Update()
        {
            checkForNewPlayer.Tick();

            if(checkForNewPlayer.IsOver())
            {
                if(enemy.Rival == null)
                {
                    enemy.Rival = enemy.GetBestPlayerToFight();
                    checkForNewPlayer.Reset();
                }
            }

            checkForPowerUp.Tick();

            if(checkForPowerUp.IsOver())
            {
                PowerUp p = enemy.GetNearestPowerUp();

                if(p != null)
                {
                    if(enemy.Rival == null || !ContinueAttack(p))
                    {
                        enemy.Target = p;
                        stateMachine.GoTo(StateEnum.RECHARGE);
                        checkForPowerUp.Reset();
                        return;
                    }
                }
            }

            shootCoolDown -= Game.Window.DeltaTime;

            if(enemy.Rival == null || !enemy.CanAttackPlayer())
            {
                stateMachine.GoTo(StateEnum.WALK);
            }
            else
            {
                enemy.LookAtPlayer();

                if(shootCoolDown <= 0.0f)
                {
                    shootCoolDown = shootTimeLimit;
                    enemy.ShootPlayer();
                }
            }
        }
    }
}
