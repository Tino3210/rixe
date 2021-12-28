using Rixe.ModelView;
using Rixe.Network.Client;
using Rixe.Tools;
using System;
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
            InitializeComponent();
            client.Click += btn1_Click;
            server.Click += btn2_Click;
            quit.Click += btn3_Click;
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Client");
            Client.GetInstance().Send("HELLO");
            Close();
            new Game().Show();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Server");
            Server.GetInstance();
            Close();
            new Game().Show();
        }
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            Close();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
