using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    interface IDrawable
    {
        DrawLayer Layer { get; }
        void Draw();
    }
}
