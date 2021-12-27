using Rixe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class MainWindow : Window
    {
        private Player player;

        private DispatcherTimer gameTimer = new DispatcherTimer();
        private List<Rectangle> itemRemover = new List<Rectangle>();
        private bool moveLeft, moveRight, moveUp, moveDown;        

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(15);            
            gameTimer.Start();

            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/purple.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0,0,.15,.15);
            bg.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = bg;

            player = new Player(MyPlayer);
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
                if (x is Rectangle && (string)x.Tag == "myProjectile")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetBottom(x) < 0)
                    {

                        itemRemover.Add(x);
                    }
                }
                if (x is Rectangle && (string)x.Tag == "Projectile")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 20);

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetBottom(x) < Application.Current.MainWindow.Height)
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

            if (e.Key == Key.Space)
            {
                Projectile newProjectile = new Projectile("myProjectile", MyPlayer);                
                MyCanvas.Children.Add(newProjectile.MyProjectile);
            }
        }
    }
}
