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
    /// <summary>
    /// Serializable used to transform the rectangle to string or a rectangle to string
    /// </summary>
    class Serializable
    {
        /// <summary>
        /// Return a string that contains the pos x and the image name
        /// </summary>
        /// <param name="_rectangle">the rectangle to save</param>
        public static string RectangleToString(Rectangle _rectangle)
        {
            return Canvas.GetLeft(_rectangle) + ";" + _rectangle.Name;
        }

        /// <summary>
        /// Convert a string to a rectangle
        /// </summary>
        /// <param name="msg">the rectangle to save</param>
        public static Rectangle StringToRectangle(string msg)
        {
            string[] subs = msg.Split(';');
            ImageBrush bg = new ImageBrush();
            //Create the image
            BitmapImage molotov = new BitmapImage(new Uri("pack://application:,,,/images/"+ subs[1] +".png"));
            bg.ImageSource = molotov;
            
            //Create the rectangle
            Rectangle rectangle = new Rectangle
            {
                Tag = "Projectile",
                Height = Settings.PROJECTILE_HEIGHT,
                Width = Settings.PROJECTILE_WIDHT,
                Fill = bg,
            };

            //Set the position
            Canvas.SetLeft(rectangle, float.Parse(subs[0]));
            Canvas.SetTop(rectangle, 0);

            return rectangle;
        }
    }
}
