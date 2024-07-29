using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    class PlayState : State
    {
        public override void OnEnter()
        {
            ((PlayScene)Game.CurrentScene).ResetTimer();
        }

        public override void OnExit()
        {
            stateMachine.Owner.RigidBody.Velocity.X = 0;
        }

        public override void Update()
        {
            if(((PlayScene)Game.CurrentScene).PlayerTimer < 0)
            {
                stateMachine.GoTo(StateEnum.WAIT);
                return;
            }
            stateMachine.Owner.Input();
        }
    }
}
