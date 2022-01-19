using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Rixe.Tools;

namespace Rixe.Entity
{
    class Player
    {
        public Rect PlayerHitBox { get; set; }
        public Rectangle MyPlayer { get; }

        public int Health { get; set; }
        
        public Player(Rectangle _MyPlayer)
        {
            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));
            _MyPlayer.Fill = playerImage;
            MyPlayer = _MyPlayer;
            PlayerHitBox = new Rect(Canvas.GetLeft(MyPlayer), Canvas.GetTop(MyPlayer), MyPlayer.Width, MyPlayer.Height);
            Health = 3;
        }

        public void GoRight()
        {
            if (Canvas.GetLeft(MyPlayer) + MyPlayer.Width < Settings.APP_WIDTH)
            {
                Canvas.SetLeft(MyPlayer, Canvas.GetLeft(MyPlayer) + Settings.PLAYER_SPEED);
            }
        }

        public void GoLeft()
        {            
            if (Canvas.GetLeft(MyPlayer) > 0)
            {
                Canvas.SetLeft(MyPlayer, Canvas.GetLeft(MyPlayer) - Settings.PLAYER_SPEED);
            }
        }

        public void GoUp()
        {
            if(Canvas.GetTop(MyPlayer) > 0)
            {
                Canvas.SetTop(MyPlayer, Canvas.GetTop(MyPlayer) - Settings.PLAYER_SPEED);
            }
        }

        public void GoDown()
        {
            Console.WriteLine(Canvas.GetTop(MyPlayer) + MyPlayer.Height + " : " + Settings.APP_HEIGHT);            
            if ((Canvas.GetTop(MyPlayer) + MyPlayer.Height) < Settings.APP_HEIGHT)
            {
                Canvas.SetTop(MyPlayer, Canvas.GetTop(MyPlayer) + Settings.PLAYER_SPEED);
            }
        }
    }
}
