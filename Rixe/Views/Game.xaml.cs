﻿using Rixe.Entity;
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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class Game : Window
    {
        private Player player;

        private DispatcherTimer gameTimer = new DispatcherTimer();
        private List<Rectangle> itemRemover = new List<Rectangle>();
        private List<Rectangle> hearts = new List<Rectangle>();
        private bool moveLeft, moveRight, moveUp, moveDown, canShoot;
        private Timer cooldownProjectile;

        public Game()
        {
            InitializeComponent();

            
            MyNetwork.GetInstance().eventSendProjectile += Receive;

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(15);            
            gameTimer.Start();

            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.png"));            
            MyCanvas.Background = bg;

            player = new Player(MyPlayer);

            hearts.Add(MyHeart1);
            hearts.Add(MyHeart2);
            hearts.Add(MyHeart3);

            cooldownProjectile = new Timer(Settings.COOLDOWN_MS);
            cooldownProjectile.Elapsed += CoolDownEvent;

            canShoot = true;
        }

        private void CoolDownEvent(Object source, ElapsedEventArgs e)
        {
            canShoot = true;
        }

        private void GameLoop(object sender, EventArgs e)
        {        

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

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                player.PlayerHitBox = new Rect(Canvas.GetLeft(MyPlayer), Canvas.GetTop(MyPlayer), MyPlayer.Width, MyPlayer.Height);
                if (x is Rectangle && (string)x.Tag == "myProjectile")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - Settings.PROJECTILE_SPEED);

                    if (Canvas.GetTop(x) + Settings.PROJECTILE_HEIGHT < 0)
                    {
                        itemRemover.Add(x);
                        MyNetwork.GetInstance().Send(Serializable.RectangleToString(x));
                    }
                }
                if (x is Rectangle && (string)x.Tag == "Projectile")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + Settings.PROJECTILE_SPEED);
                    Rect projectilleHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    
                    if (player.PlayerHitBox.IntersectsWith(projectilleHitBox))
                    {                   
                        itemRemover.Add(x);
                        player.Health -= 1;                        
                        if(player.Health == 0)
                        {
                            hearts[player.Health].Visibility = Visibility.Hidden;
                            MessageBoxResult result = MessageBox.Show("Vous avez perdu !\n Voulez-vous faire une nouvelle partie", "Rixe", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    MessageBox.Show("Hello to you too!", "My App");
                                    break;
                                case MessageBoxResult.No:
                                    Application.Current.Shutdown();
                                    break;                                
                            }
                        }
                        else
                        {
                            hearts[player.Health].Visibility = Visibility.Hidden;
                        }                        
                    }                    
                    if ((Canvas.GetTop(x) + x.Height) > Settings.APP_HEIGHT)
                    {
                        itemRemover.Add(x);
                    }
                }
            }            

            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);
            }                   
        }

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

        public void Receive(object source, ReceiveProjectileEvent e)
        {
            Dispatcher.Invoke(() =>
            {
                Rectangle rect = Serializable.StringToRectangle(e.Rectangle);
                MyCanvas.Children.Add(rect);
            });
        }
    }
}
