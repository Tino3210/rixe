using Rixe.Command;
using Rixe.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rixe.ModelView
{
    class MainMenuViewModel : INotifyPropertyChanged
    {
        private string _myIP;
        private string _inputIP;

        public ICommand _startHost;
        public ICommand _startGuest;
        public ICommand _quit;

        public MainMenuViewModel()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    _myIP = ip.ToString();
                }
            }
            InputIP = "hello";
        }

        public string MyIP
        {
            get { return _myIP; }
            set
            {
                if (value.Equals(_myIP)) return;
                _myIP = value;
                OnPropertyChanged("MyIP");
            }
        }

        public string InputIP
        {
            get { return _inputIP; }
            set
            {
                if (value.Equals(_inputIP)) return;
                _inputIP = value;
                OnPropertyChanged("InputIP");
            }
        }

        public ICommand StartHost
        {
            get
            {
                return _startHost ?? (_startHost = new RelayCommand(x =>
                {
                    Console.WriteLine("Server");
                    MyNetwork.GetInstance(MyNetwork.Type.Server);
                    new Game().Show();
                    ((Window)x).Close();

                }));
            }
        }

        public ICommand StartGuest
        {
            get
            {
                return _startGuest ?? (_startGuest = new RelayCommand(x =>
                {
                    Console.WriteLine("Client");
                    MyNetwork.GetInstance(MyNetwork.Type.Client);
                    new Game().Show();
                    ((Window)x).Close();
                }));
            }
        }
        public ICommand Quit
        {
            get
            {
                return _quit ?? (_quit = new RelayCommand(x =>
                {
                    Console.WriteLine("CLOSING");
                    ((Window)x).Close();
                    Application.Current.Shutdown();
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new ArgumentNullException(GetType().Name + " does not contain property: " + propertyName);
        }
    }
}