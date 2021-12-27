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

namespace Rixe.Entity
{
    class Player
    {
        public Rect PlayerHitBox { get; }
        public Rectangle MyPlayer { get; }
        
        public Player(Rectangle _MyPlayer)
        {
            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));
            _MyPlayer.Fill = playerImage;
            MyPlayer = _MyPlayer;
            PlayerHitBox = new Rect(Canvas.GetLeft(MyPlayer), Canvas.GetTop(MyPlayer), MyPlayer.Width, MyPlayer.Height);
        }

        public void GoRight()
        {
            if (Canvas.GetLeft(MyPlayer) + MyPlayer.Width < Application.Current.MainWindow.Width)
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
            if (Canvas.GetTop(MyPlayer) + MyPlayer.Height < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(MyPlayer, Canvas.GetTop(MyPlayer) + Settings.PLAYER_SPEED);
            }
        }
    }
}
