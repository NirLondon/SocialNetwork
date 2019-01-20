using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Client.ViewModels
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;




        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
