using Rixe.ModelView;
using Rixe.Network;
using Rixe.Tools;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Rixe
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainMenu : Window, IPageViewModel
    {
        public MainMenu()
        {
            DataContext = new MainMenuViewModel();
            InitializeComponent();
            client.Click += btn1_Click;
            server.Click += btn2_Click;
            quit.Click += btn3_Click;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Client");
            MyNetwork.GetInstance(MyNetwork.Type.Client);
            new Game().Show();
            Close();

        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Server");
            MyNetwork.GetInstance(MyNetwork.Type.Server);
            new Game().Show();
            Close();

        }
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            //Close();
            //System.Windows.Application.Current.Shutdown();
        }
    }
}
