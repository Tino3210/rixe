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
    /// <summary>
    /// Projectile class 
    /// </summary>
    class Projectile
    {
        //Tag the user
        public string MyTag { get; set; }
        //Tag the name
        public string MyName { get; }
        //The rectangle
        public Rectangle MyProjectile { get; }

        /// <summary>
        /// Constructor for the player 
        /// </summary>
        /// <param name="_Tag">The tag</param> 
        /// <param name="_Player">Rectangle</param>        
        public Projectile(string _Tag, Rectangle _Player)
        {
            MyTag = _Tag;
            ImageBrush bg = new ImageBrush();
            BitmapImage molotov = new BitmapImage(new Uri("pack://application:,,,/images/molotov.png"));
            BitmapImage rock = new BitmapImage(new Uri("pack://application:,,,/images/rock.png"));
            var rand = new Random();
            
            //Rand to choose between the rock and the molotov
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
            
            //Create the rectangle
            MyProjectile = new Rectangle
            {
                Tag = _Tag,
                Name = MyName,
                Height = Settings.PROJECTILE_HEIGHT,
                Width = Settings.PROJECTILE_WIDHT,
                Fill = bg,
            };
            //Set the position
            Canvas.SetLeft(MyProjectile, (Canvas.GetLeft(_Player) + _Player.Width / 2) - Settings.PROJECTILE_WIDHT/2);
            Canvas.SetTop(MyProjectile, Canvas.GetTop(_Player) - MyProjectile.Height);
        }
    }
}
