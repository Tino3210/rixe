using Rixe.Command;
using Rixe.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
            var address = new List<string>();
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && new Regex("^(?:[0-9]{1,3}\\.){3}[0-9]{1,3}$").Match(ip.ToString()).Success)
                {
                    address.Add(ip.ToString());
                }
            }
            _myIP = String.Join("\n", address.ToArray());
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
                    MyIP = "Server";
                    MyNetwork.GetInstance(MyNetwork.Type.Server, _inputIP);
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
                    MyIP = "Client";
                    MyNetwork.GetInstance(MyNetwork.Type.Client,_inputIP);
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