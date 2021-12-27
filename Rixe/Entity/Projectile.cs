using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rixe.Entity
{
    class Projectile
    {
        public string MyTag { get; set; }
        public Rectangle MyProjectile { get; }
        
        public Projectile(string _Tag, Rectangle _Player)
        {
            MyTag = _Tag;
            MyProjectile = new Rectangle
            {
                Tag = _Tag,
                Height = Settings.PROJECTILE_HEIGHT,
                Width = Settings.PROJECTILE_WIDHT,
                Fill = Brushes.White,
                Stroke = Brushes.Red
            };
            Canvas.SetLeft(MyProjectile, Canvas.GetLeft(_Player) + _Player.Width / 2);
            Canvas.SetTop(MyProjectile, Canvas.GetTop(_Player) - MyProjectile.Height);
        }
    }
}
