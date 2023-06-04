using System.Drawing;

namespace RustFishingBot_GUI.Classes.Misc
{
    internal class Constants
    {
        public enum ItemsTypes
        {
            Food,
            Primanka,
            Drinks,
            Udochka
        }
        public class ItemInfo
        {
            public string Name;
            public ItemsTypes Types;
            public int Primanka;
            public ItemInfo(string name, ItemsTypes types, int primanka = -1)
            {
                Name = name;
                Types = types;
                Primanka = primanka;
            }
        }
        public static ItemInfo[] itemsInfo = new ItemInfo[]
        {
            new ItemInfo("УДОЧКА", ItemsTypes.Udochka),
            new ItemInfo("СЫРАЯ РЫБА", ItemsTypes.Primanka,5),
            new ItemInfo("ФОРЕЛЬ", ItemsTypes.Primanka,10)
        };
        public class Item
        {
            public ItemInfo Info;
            public Point position;
            public Item(ItemInfo itemInfo, Point point)
            {
                Info = itemInfo;
                position = point;
            }
        }

        public static Point firstSlot = new Point(700, 1000);
        public static Point newItemPixel = new Point(1857, 896);
        public static Color newItemColor = Color.FromArgb(88, 101, 66);

    }
}
