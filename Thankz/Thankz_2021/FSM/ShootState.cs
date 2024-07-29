using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    class ShootState : State
    {
        public override void Update()
        {
            //wait for shot bullet destroyed
            if(stateMachine.Owner.LastShotBullet == null || !stateMachine.Owner.LastShotBullet.IsActive)
            {
                stateMachine.GoTo(StateEnum.WAIT);
            }
        }
    }
}
