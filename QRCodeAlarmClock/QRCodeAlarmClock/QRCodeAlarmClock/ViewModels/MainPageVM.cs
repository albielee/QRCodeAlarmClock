using QRCodeAlarmClock.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace QRCodeAlarmClock.ViewModels
{
    public class MainPageVM : INotifyPropertyChanged
    {
        private ObservableCollection<Alarm> alarmList;
        public ObservableCollection<Alarm> AlarmList
        {
            get { return alarmList; }
            set
            {
                alarmList = value;
                OnPropertyChanged(nameof(AlarmList));
            }
        }


        public Command<Alarm> AlarmItemSelectedCommand { get; set; }
        public MainPageVM()
        {
            AlarmItemSelectedCommand = new Command<Alarm>(AlarmItemSelected);

            AlarmList = new ObservableCollection<Alarm>();
            var a1 = new Alarm()
            {
                IsEnabled = true,
                Name = "First alarm",
                QRCode = "",
                Time = DateTime.Now,
            };
            AlarmList.Add(a1);
            var a2 = new Alarm()
            {
                IsEnabled = false,
                Name = "Second alarm",
                QRCode = "",
                Time = DateTime.Now,
            };
            AlarmList.Add(a2);
            var a3 = new Alarm()
            {
                IsEnabled = false,
                Name = "Third alarm",
                QRCode = "gggggggggg",
                Time = DateTime.Now,
            };
            AlarmList.Add(a3);
            AlarmList.Add(a1);
            AlarmList.Add(a2);
            AlarmList.Add(a3);
        }

        private void AlarmItemSelected(Alarm alarm)
        {

        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if(propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
