using Rixe.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rixe.Entity
{
    class Projectile
    {
        public string MyTag { get; set; }
        public string MyName { get; }
        public Rectangle MyProjectile { get; }
        
        public Projectile(string _Tag, Rectangle _Player)
        {
            MyTag = _Tag;
            ImageBrush bg = new ImageBrush();
            BitmapImage molotov = new BitmapImage(new Uri("pack://application:,,,/images/molotov.png"));
            BitmapImage rock = new BitmapImage(new Uri("pack://application:,,,/images/rock.png"));
            var rand = new Random();
            
            if (rand.Next(0, 5) == 0)
            {
                bg.ImageSource = molotov;
                MyName = "molotov";
            }
            else
            {
                bg.ImageSource = rock;
                MyName = "rock";
            }            
            
            MyProjectile = new Rectangle
            {
                Tag = _Tag,
                Name = MyName,
                Height = Settings.PROJECTILE_HEIGHT,
                Width = Settings.PROJECTILE_WIDHT,
                Fill = bg,
            };
            Canvas.SetLeft(MyProjectile, Canvas.GetLeft(_Player) + _Player.Width / 2);
            Canvas.SetTop(MyProjectile, Canvas.GetTop(_Player) - MyProjectile.Height);
        }
    }
}
