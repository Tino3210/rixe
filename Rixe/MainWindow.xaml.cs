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
        DispatcherTimer gameTimer = new DispatcherTimer();
        List<Rectangle> itemRemover = new List<Rectangle>();
        bool moveLeft, moveRight;
        int speedPlayer = 10;
        Rect playerHitBox;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(15);            
            gameTimer.Start();

            MyCanevas.Focus();
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/purple.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0,0,.15,.15);
            bg.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanevas.Background = bg;

            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));
            Player.Fill = playerImage;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            playerHitBox = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);

            if (moveLeft && Canvas.GetLeft(Player) > 0)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) - speedPlayer);
            }

            if (moveRight && Canvas.GetLeft(Player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) + speedPlayer);
            }

            foreach (var x in MyCanevas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "Bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetBottom(x) < 0)
                    {

                        itemRemover.Add(x);
                    }
                }
            }

            foreach (Rectangle i in itemRemover)
            {
                MyCanevas.Children.Remove(i);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {               
                moveLeft = true;
            }
            if(e.Key == Key.Right)
            {               
                moveRight = true;
            }            
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "Bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red
                };

                Canvas.SetLeft(newBullet, Canvas.GetLeft(Player) + Player.Width / 2);
                Canvas.SetTop(newBullet, Canvas.GetTop(Player) - newBullet.Height);
                MyCanevas.Children.Add(newBullet);
            }
        }
    }
}
