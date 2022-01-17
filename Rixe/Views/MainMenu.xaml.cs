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
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            DataContext = new MainMenuViewModel();
            InitializeComponent();
        }
    }
}
