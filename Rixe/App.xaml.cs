using Rixe.ModelView;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Rixe
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainMenu app = new MainMenu();
            AppViewModel context = new AppViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
