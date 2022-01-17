﻿using Rixe.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rixe.ModelView
{
    class MainMenuViewModel : INotifyPropertyChanged
    {
        private string _captured;

        public ICommand _test;
        public MainMenuViewModel()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    _captured = ip.ToString();
                }
            }
        }

        public string Capture
        {
            get { return _captured; }
            set
            {
                if (value.Equals(_captured)) return;
                _captured = value;
                OnPropertyChanged("Capture");
            }
        }

        public ICommand Test
        {
            get
            {
                return _test ?? (_test = new RelayCommand(x =>
                {
                    
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