using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    class WaitState : State
    {
        public override void OnEnter()
        {
            stateMachine.Owner.StopLoading();
            ((PlayScene)Game.CurrentScene).NextPlayer();
        }

        public override void Update()
        {
            
        }
    }
}
