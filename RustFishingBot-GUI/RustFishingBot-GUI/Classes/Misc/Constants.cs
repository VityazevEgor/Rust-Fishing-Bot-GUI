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
            Udochka,
            ToCut,
            Safe,
            Junk
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
            // приманки
            new ItemInfo("СЫРАЯ", ItemsTypes.Primanka,5), // всё сырое мясо это приманка
            new ItemInfo("СЫРОЕ", ItemsTypes.Primanka,5),
            new ItemInfo("ФОРЕЛЬ", ItemsTypes.Primanka,10), // ну и форель

            // рыбы
            new ItemInfo("ЛОСОСЬ", ItemsTypes.Safe),
            new ItemInfo("АКУЛА", ItemsTypes.Safe),
            new ItemInfo("СОМ", ItemsTypes.Primanka,10),
            new ItemInfo("АТЛАНТИЧЕСКИЙ", ItemsTypes.Primanka, 10),
            new ItemInfo("ОКУНЬ", ItemsTypes.Primanka, 10),
            new ItemInfo("САРДИНА", ItemsTypes.ToCut),
            new ItemInfo("СЕЛЬДЬ", ItemsTypes.ToCut),
            new ItemInfo("АНЧОУС", ItemsTypes.ToCut),

            // мусор
            new ItemInfo("ОБЛОМКИ", ItemsTypes.Junk),

            //еда
            new ItemInfo("ПРИГОТОВЛЕННАЯ", ItemsTypes.Food), // всё жаренное мясо
            new ItemInfo("ПРИГОТОВЛЕННОЕ", ItemsTypes.Food), // всё жаренное мясо

            new ItemInfo("ПЛИТКА", ItemsTypes.Food),
            new ItemInfo("КАРТОФЕЛЬ", ItemsTypes.Food),
            new ItemInfo("КУКУРУЗА", ItemsTypes.Food),
            new ItemInfo("ЯБЛОКО", ItemsTypes.Food),
            new ItemInfo("ТЫКВА", ItemsTypes.Food),
            new ItemInfo("ТУНЦА", ItemsTypes.Food),
            new ItemInfo("ФАСОЛИ", ItemsTypes.Food),
            new ItemInfo("БАТОНЧИК", ItemsTypes.Food),
            new ItemInfo("ЕЖЕВИКА", ItemsTypes.Food),

            // вода
            new ItemInfo("КАНИСТРА", ItemsTypes.Drinks),
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


        public static Rectangle[] buttonsPositions = new Rectangle[]
        {
            new Rectangle(1038,432,175,48),
            new Rectangle(1039, 490, 154, 37),
            new Rectangle(1040, 360, 128, 36),
            new Rectangle(1039, 412, 134, 42)
        };

        public static Point firstSlot = new Point(700, 1000);
        //public static Point newItemPixel = new Point(1857, 896);
        public static Color newItemColor = Color.FromArgb(88, 101, 66);

        // константы для плашек
        public static Point firstNotificationPixel = new Point(1608, 890);
        public static int notificationWidth = 288;
        public static int notificationHeight = 39;
        public static int notificationPadding = 3; // растояние между плашками
        public static Rectangle errorRectangel = new Rectangle(999, 999, 999,999);

        // константы для плашек об здоровье еде и воде
        public static Rectangle healthRect = new Rectangle(1645, 938, 67, 25);
        public static Rectangle waterRect = new Rectangle(1652, 984, 56, 32);
        public static Rectangle foodRect = new Rectangle(1647, 1022, 70, 30);

        // позиция текста с действием
        public static Rectangle actionRect = new Rectangle(916, 487,88,26);

    }
}
