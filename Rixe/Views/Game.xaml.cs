using Rixe.Entity;
using Rixe.ModelView;
using Rixe.Network;
using Rixe.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Rixe
{
    /// <summary>
    /// Game is the logic part
    /// </summary>
    public partial class Game : Window
    {
        //The player
        private Player player;

        //Game timer
        private DispatcherTimer gameTimer = new DispatcherTimer();
        //List of rectangle to remove
        private List<Rectangle> itemRemover = new List<Rectangle>();
        //List of hearts
        private List<Rectangle> hearts = new List<Rectangle>();
        //boolean of mouvements
        private bool moveLeft, moveRight, moveUp, moveDown, canShoot;
        //Time before re shoot projectille
        private Timer cooldownProjectile;

        /// <summary>
        /// Constructor
        /// </summary>
        public Game()
        {
            InitializeComponent();

            //Link the events with the methods
            MyNetwork.GetInstance().eventSendProjectile += Receive;
            MyNetwork.GetInstance().eventSendWin += Victory;

            //Every 15 ms the GameLoop method will be call
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(15);
            //Start the timer
            gameTimer.Start();

            //Set the focus to the canevas
            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush();
            //Set the background image
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.png"));            
            MyCanvas.Background = bg;

            player = new Player(MyPlayer);

            //Set the hearts
            hearts.Add(MyHeart1);
            hearts.Add(MyHeart2);
            hearts.Add(MyHeart3);

            //Set the timer for the cooldown for the projectille
            cooldownProjectile = new Timer(Settings.COOLDOWN_MS);
            cooldownProjectile.Elapsed += CoolDownEvent;

            //The user shoot
            canShoot = true;
        }

        /// <summary>
        /// Call after the end of the cooldown and the player is allow to shoot
        /// </summary>
        private void CoolDownEvent(Object source, ElapsedEventArgs e)
        {
            canShoot = true;
        }

        /// <summary>
        /// The method call every 15ms
        /// </summary>
        private void GameLoop(object sender, EventArgs e)
        {        

            //Mouvement gestion
            if (moveLeft)
            {
                player.GoLeft();
            }

            if (moveRight)
            {
                player.GoRight();
            }

            if(moveUp)
            {
                player.GoUp();
            }

            if (moveDown)
            {
                player.GoDown();
            }

            //Travel all rectangle of the canevas
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                //Calculate the new hitbox
                player.PlayerHitBox = new Rect(Canvas.GetLeft(MyPlayer), Canvas.GetTop(MyPlayer), MyPlayer.Width, MyPlayer.Height);

                //"myProjectille" is the projectille of the player
                if (x is Rectangle && (string)x.Tag == "myProjectile")
                {
                    //Move the projectille to the top
                    Canvas.SetTop(x, Canvas.GetTop(x) - Settings.PROJECTILE_SPEED);

                    //If the projectille go out the top of the screen
                    if (Canvas.GetTop(x) + Settings.PROJECTILE_HEIGHT < 0)
                    {
                        //Add to the remove list
                        itemRemover.Add(x);
                        //Send the projectille
                        MyNetwork.GetInstance().Send(Serializable.RectangleToString(x));
                    }
                }

                //"Projectille" is the projectille of the enemy
                if (x is Rectangle && (string)x.Tag == "Projectile")
                {
                    //Move the projectille to the bottom
                    Canvas.SetTop(x, Canvas.GetTop(x) + Settings.PROJECTILE_SPEED);
                    //Calculate the new hitbox of the projectille
                    Rect projectilleHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    
                    //If the projectille touch the player
                    if (player.PlayerHitBox.IntersectsWith(projectilleHitBox))
                    {           
                        //Add the projectille to the remove list        
                        itemRemover.Add(x);                        
                        player.Health -= 1;   
                        //If the play have no life left                  
                        if(player.Health == 0)
                        {
                            //Remove the hearts
                            hearts[player.Health].Visibility = Visibility.Hidden;
                            //Send to the enemy he win
                            MyNetwork.GetInstance().Send("win");
                            //Display a message box
                            MessageBoxResult result = MessageBox.Show("Vous avez perdu !\n Voulez-vous faire une nouvelle partie", "Rixe", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    MyNetwork.Disconnect();
                                    new MainMenu().Show();
                                    Close();
                                    break;
                                case MessageBoxResult.No:
                                    Application.Current.Shutdown();
                                    break;
                            }
                        }
                        else
                        {
                            //Remove heart
                            hearts[player.Health].Visibility = Visibility.Hidden;
                        }                        
                    }   
                    //If the projectille is out the bottom of the screen                 
                    if ((Canvas.GetTop(x) + x.Height) > Settings.APP_HEIGHT)
                    {
                        //Add the projectille to the remove list
                        itemRemover.Add(x);
                    }
                }
            }            

            //Travel the remove list and remove all rectangle
            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);
            }                   
        }

        /// <summary>
        /// Function call by a Key press when the state is DOWN
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                moveRight = true;
            }
            if (e.Key == Key.Up || e.Key == Key.W)
            {
                moveUp = true;
            }
            if (e.Key == Key.Down || e.Key == Key.S)
            {
                moveDown = true;
            }
        }

        /// <summary>
        /// Function call by a Key press when the state is UP
        /// </summary>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                moveRight = false;
            }
            if (e.Key == Key.Up || e.Key == Key.W)
            {
                moveUp = false;
            }
            if (e.Key == Key.Down || e.Key == Key.S)
            {
                moveDown = false;
            }

            if (e.Key == Key.Space && canShoot)
            {
                Projectile newProjectile = new Projectile("myProjectile", MyPlayer);                
                MyCanvas.Children.Add(newProjectile.MyProjectile);
                cooldownProjectile.Start();
                canShoot = false;
            }
        }

        /// <summary>
        /// Create a projectill when the opponent send one
        /// </summary>
        public void Receive(object source, ReceiveProjectileEvent e)
        {
            Dispatcher.Invoke(() =>
            {
                Rectangle rect = Serializable.StringToRectangle(e.Rectangle);
                MyCanvas.Children.Add(rect);
            });
        }

        /// <summary>
        /// End the game when the opponent send that we win
        /// </summary>
        public void Victory(object source, ReceiveWinEvent e)
        {
            MessageBoxResult result = MessageBox.Show("Vous avez Gagné !\n Voulez-vous faire une nouvelle partie", "Rixe", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MyNetwork.Disconnect();
                    new MainMenu().Show();
                    Close();
                    break;
                case MessageBoxResult.No:
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}
