using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    enum ColorType { Black, Red, Green, Blue, White, LAST}

    static class ColorsFactory
    {
        private static Color[] colors;      

        static ColorsFactory()
        {
            Console.WriteLine("Static constructor called");

            colors = new Color[(int)ColorType.LAST];

            Color color;

            for (int i = 0; i < colors.Length; i++)
            {
                switch ((ColorType)i)
                {
                    case ColorType.Red:
                        color.R = 255;
                        color.G = 0;
                        color.B = 0;
                        break;
                    case ColorType.Green:
                        color.R = 0;
                        color.G = 255;
                        color.B = 0;
                        break;
                    case ColorType.Blue:
                        color.R = 0;
                        color.G = 0;
                        color.B = 255;
                        break;
                    case ColorType.White:
                        color.R = 255;
                        color.G = 255;
                        color.B = 255;
                        break;
                    default:
                        color.R = 0;
                        color.G = 0;
                        color.B = 0;
                        break;
                }

                colors[i] = color;
            }
        }

        public static Color GetColor(ColorType type)
        {
            return colors[(int)type];
        }
    }
}
