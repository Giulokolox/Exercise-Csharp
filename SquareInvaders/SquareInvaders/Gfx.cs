using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw;

namespace SquareInvaders
{
    struct Color
    {
        public byte R;
        public byte G;
        public byte B;
    }

    static class Gfx
    {
        public static Window Window;

        public static void Init(string name, int width = 800, int height = 600)
        {
            Window = new Window(width, height, name, PixelFormat.RGB);
        }

        public static void PutPixel(int x, int y, byte r, byte g, byte b)
        {
            if (x >= 0 && x < Window.Width && y >= 0 && y < Window.Height)
            {
                int pixelIndex = (Window.Width * y + x) * 3;

                Window.Bitmap[pixelIndex] = r;
                Window.Bitmap[pixelIndex + 1] = g;
                Window.Bitmap[pixelIndex + 2] = b;
            }
        }

        public static void DrawHorizontalLine(int x, int y, int width, byte r, byte g, byte b)
        {
            for (int i = 0; i < width; i++)
            {
                PutPixel(x + i, y, r, g, b);
            }
        }

        public static void DrawHorizontalLine(int x, int y, int width, Color color)
        {
            DrawHorizontalLine(x, y, width, color.R, color.G, color.B);
        }

        public static void DrawRect(int x, int y, int width, int height, byte r, byte g, byte b)
        {
            for (int i = 0; i < height; i++)
            {
                DrawHorizontalLine(x, y + i, width, r, g, b);
            }
        }

        public static void DrawRect(int x, int y, int width, int height, Color color)
        {
            DrawRect(x, y, width, height, color.R, color.G, color.B);
        }

        public static void DrawVerticalLine(int x, int y, int height, byte r, byte g, byte b)
        {
            for (int i = 0; i < height; i++)
            {
                PutPixel(x, y + i, r, g, b);
            }
        }
        public static void DrawVerticalLine(int x, int y, int height, Color color)
        {
            DrawVerticalLine(x, y, height, color.R, color.G, color.B);
        }

        public static void DrawSprite(Sprite sprite, int spriteX, int spriteY)
        {
            // ALPHA BLENDING
            // A Sprite has opaque pixels as well as transparent ones. To be able to do this it uses a fourth channel in its Bitmap
            // array called the Alpha Channel. Values in the Alpha Channel do not control a color value but a transparency value and
            // are used to control the transparency of the pixels.
            // To draw a Sprite then, we need a way to draw only the opaque pixels of the Sprite while the transparent ones need to
            // let the background being seen without obstructing it.
            // The technique used is called "Alpha Blending": we get values for the Sprite Bitmap arrays as well as the Window's Bitmap
            // array in the same position, then we blend those values together based on the alpha values of the sprite and draw the
            // whole area using the blended values we've found. This way the sprite's opaque pixels will be drawn while in place of the
            // transparent ones the window pixels (background) will be drawn.

            // Window Coordinates
            int x;
            int y;

            // r -> rows (vertical);
            for (int r = 0; r < sprite.Height; r++)
            {
                // c -> columns (horizontal);
                for (int c = 0; c < sprite.Width; c++)
                {
                    // Calculate Window Coordinates for every pixel of the sprite
                    x = spriteX + c;
                    y = spriteY + r;

                    // If a sprite's pixel happens to be outside of the Window, ignore it and go to the next one
                    if(x < 0 || x >= Window.Width || y < 0 || y >= Window.Height)
                    {
                        continue;
                    }

                    // Find indices for the Window (canvas) and Sprite Bitmaps (pixels arrays)
                    int canvasIndex = (y * Window.Width + x) * 3;   // Like the normal PutPixel function
                    int spriteIndex = (r * sprite.Width + c) * 4;   // 4 because in this case we're counting the Alpha channel too

                    // Get the Sprite's Bitmap values (RGBA)
                    byte spriteR = sprite.Bitmap[spriteIndex];
                    byte spriteB = sprite.Bitmap[spriteIndex + 1];
                    byte spriteG = sprite.Bitmap[spriteIndex + 2];
                    byte spriteA = sprite.Bitmap[spriteIndex + 3];
                    
                    // Calculate an alpha value to be used in the blending formula in order to obtain only sprite's info if pixel is opaque
                    // and only Window's info if pixel is transparent
                    // (if spriteA = 255 fully opaque, draw only sprite values -> alpha = 1)
                    // (if spriteA = 0 fully transparent, draw only window values -> alpha = 0)
                    float alpha = spriteA / 255.0f;

                    // Get the Window's Bitmap values (RGB)
                    byte winR = Window.Bitmap[canvasIndex];
                    byte winG = Window.Bitmap[canvasIndex + 1];
                    byte winB = Window.Bitmap[canvasIndex + 2];

                    // Blend the two values together for each channel (RGB)
                    // NOTE: if alpha = 1 window's values will be ignored
                    //       if alpha = 0 sprite's values will be ignored
                    byte blendedR = (byte)(spriteR * alpha + winR * (1 - alpha));
                    byte blendedG = (byte)(spriteG * alpha + winG * (1 - alpha));
                    byte blendedB = (byte)(spriteB * alpha + winB * (1 - alpha));

                    // Set the Window's Bitmap values as the blended values we've found
                    Window.Bitmap[canvasIndex] = blendedR;
                    Window.Bitmap[canvasIndex + 1] = blendedG;
                    Window.Bitmap[canvasIndex + 2] = blendedB;
                }
            }
        }


        public static void ClearScreen()
        {
            for (int i = 0; i < Window.Bitmap.Length; i++)
            {
                Window.Bitmap[i] = 0;
            }
        }
    }
}
