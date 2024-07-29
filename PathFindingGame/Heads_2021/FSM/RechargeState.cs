using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    class RechargeState : State
    {
        private Enemy enemy;

        public RechargeState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void OnEnter()
        {
            if(enemy.Target != null && enemy.Target.IsActive)
            {
                //Vector2 distance = enemy.Target.Position - enemy.Position;
                //// we just need to set RB velocity towards the powerup on state enter
                //enemy.RigidBody.Velocity = distance.Normalized() * enemy.followSpeed;

                List<Node> path = ((PlayScene)Game.CurrentScene).map.GetPath((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Target.Position.X, (int)enemy.Target.Position.Y);
                enemy.Agent.SetPath(path);
            }
        }

        public override void Update()
        {
            if(enemy.Target == null || !enemy.Target.IsActive)
            {
                enemy.Target = null;

                if(enemy.Rival != null && enemy.Rival.IsActive)
                {
                    stateMachine.GoTo(StateEnum.FOLLOW);
                }
                else
                {
                    stateMachine.GoTo(StateEnum.WALK);
                }
            }
            else
            {
                enemy.Agent.Update(enemy.followSpeed);
            }
        }
    }
}
