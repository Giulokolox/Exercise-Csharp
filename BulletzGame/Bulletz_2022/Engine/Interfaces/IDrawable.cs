﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz_2022
{
    interface IDrawable
    {
        DrawLayer Layer { get; }
        void Draw();
    }
}