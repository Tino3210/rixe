using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rixe.Tools
{
    class Serializable
    {
        public static string RectToString(Rectangle _rectangle)
        {
            return Canvas.GetLeft(_rectangle) + ";" + _rectangle.Name;
        }

        public static Rectangle StringToRect(string msg)
        {
            string[] subs = msg.Split(';');
            ImageBrush bg = new ImageBrush();
            BitmapImage molotov = new BitmapImage(new Uri("pack://application:,,,/images/"+ subs[1] +".png"));
            bg.ImageSource = molotov;
            
            return new Rectangle
            {
                Tag = "Projectille",
                Height = Settings.PROJECTILE_HEIGHT,
                Width = Settings.PROJECTILE_WIDHT,
                Fill = bg,
            };
        }
    }
}
