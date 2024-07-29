using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    abstract class TimedPowerUp : PowerUp
    {
        protected float duration;
        protected float counter;

        protected TimedPowerUp(string textureName) : base(textureName)
        {
            duration = 5.0f;
        }

        public override void OnAttach(Player p)
        {
            base.OnAttach(p);
            counter = duration;
        }

        public override void Update()
        {
            base.Update();

            if(attachedPlayer != null)
            {
                counter -= Game.Window.DeltaTime;

                if(counter <= 0)
                {
                    OnDetach();
                }
            }
        }
    }
}
