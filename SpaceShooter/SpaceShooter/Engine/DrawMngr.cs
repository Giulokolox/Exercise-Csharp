using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    static class DrawMngr
    {
        private static List<IDrawable> items;

        static DrawMngr()
        {
            items = new List<IDrawable>();
        }

        public static void AddItem(IDrawable item)
        {
            items.Add(item);
        }

        public static void RemoveItem(IDrawable item)
        {
            items.Remove(item);
        }

        public static void ClearAll()
        {
            items.Clear();
        }

        public static void Draw()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw();
            }
        }
    }
}
