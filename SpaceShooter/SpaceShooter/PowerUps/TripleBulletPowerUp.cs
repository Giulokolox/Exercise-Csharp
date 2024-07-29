using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class TripleBulletPowerUp : TimedPowerUp
    {
        public TripleBulletPowerUp() : base("tripleShootPowerUp")
        {

        }

        public override void OnAttach(Player p)
        {
            base.OnAttach(p);

            attachedPlayer.ChangeWeapon(WeaponType.TripleShoot);
        }

        public override void OnDetach()
        {
            attachedPlayer.ChangeWeapon(WeaponType.Default);
            base.OnDetach();
        }
    }
}
