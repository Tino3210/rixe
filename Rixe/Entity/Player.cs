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
    /// <summary>
    /// Player save the rectangle and affect the mouvement of the player
    /// </summary>
    class Player
    {
        //The rectangle for the hit box
        public Rect PlayerHitBox { get; set; }
        //The rectangle of the player
        public Rectangle MyPlayer { get; }

        //The HP point of the user
        public int Health { get; set; }

        /// <summary>
        /// Constructor for the player 
        /// </summary>
        /// <param name="_MyPlayer">The rectangle form the game</param>
        public Player(Rectangle _MyPlayer)
        {
            ImageBrush playerImage = new ImageBrush();
            //Load the image for the player
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));
            _MyPlayer.Fill = playerImage;
            MyPlayer = _MyPlayer;
            //Create the hitbox
            PlayerHitBox = new Rect(Canvas.GetLeft(MyPlayer), Canvas.GetTop(MyPlayer), MyPlayer.Width, MyPlayer.Height);
            //Set the player with 3 lifes
            Health = 3;
        }

        /// <summary>
        /// Move the rectangle player to the right
        /// </summary>
        public void GoRight()
        {
            //Move the player only if is always on the screen
            if (Canvas.GetLeft(MyPlayer) + MyPlayer.Width < Settings.APP_WIDTH)
            {
                Canvas.SetLeft(MyPlayer, Canvas.GetLeft(MyPlayer) + Settings.PLAYER_SPEED);
            }
        }

        /// <summary>
        /// Move the rectangle player to the left
        /// </summary>
        public void GoLeft()
        {            
            //Move the player only if is always on the screen
            if (Canvas.GetLeft(MyPlayer) > 0)
            {
                Canvas.SetLeft(MyPlayer, Canvas.GetLeft(MyPlayer) - Settings.PLAYER_SPEED);
            }
        }

        /// <summary>
        /// Move the rectangle player to the top
        /// </summary>
        public void GoUp()
        {
            //Move the player only if always on the screen
            if(Canvas.GetTop(MyPlayer) > 0)
            {
                Canvas.SetTop(MyPlayer, Canvas.GetTop(MyPlayer) - Settings.PLAYER_SPEED);
            }
        }

        /// <summary>
        /// Move the rectangle player to the bottom
        /// </summary>
        public void GoDown()
        {
            //Move the player only if always on the screen
            if (Canvas.GetTop(MyPlayer) + MyPlayer.Height < Settings.APP_HEIGHT)
            {
                Canvas.SetTop(MyPlayer, Canvas.GetTop(MyPlayer) + Settings.PLAYER_SPEED);
            }
        }
    }
}
