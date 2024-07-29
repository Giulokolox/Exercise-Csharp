using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class GreenGlobeBullet : PlayerBullet
    {
        public GreenGlobeBullet() : base("greenGlobe")
        {
            Type = BulletType.GreenGlobeBullet;
        }
    }
}
