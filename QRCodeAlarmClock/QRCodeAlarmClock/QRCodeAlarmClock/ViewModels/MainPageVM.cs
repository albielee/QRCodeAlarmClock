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
        /// <summary>
        /// Repeat page properties
        /// </summary>
        /// 

        private bool repeatMonday;
        public bool RepeatMonday
        {
            get { return repeatMonday; }
            set
            {
                repeatMonday = value;
                OnPropertyChanged(nameof(RepeatMonday));
                OnPropertyChanged(nameof(RepeatDaysOfWeekLabel));
            }
        }

        private bool repeatTuesday;
        public bool RepeatTuesday
        {
            get { return repeatTuesday; }
            set
            {
                repeatTuesday = value;
                OnPropertyChanged(nameof(RepeatTuesday));
                OnPropertyChanged(nameof(RepeatDaysOfWeekLabel));
            }
        }

        private bool repeatWednesday;
        public bool RepeatWednesday
        {
            get { return repeatWednesday; }
            set
            {
                repeatWednesday = value;
                OnPropertyChanged(nameof(RepeatWednesday));
                OnPropertyChanged(nameof(RepeatDaysOfWeekLabel));
            }
        }

        private bool repeatThursday;
        public bool RepeatThursday
        {
            get { return repeatThursday; }
            set
            {
                repeatThursday = value;
                OnPropertyChanged(nameof(RepeatThursday));
                OnPropertyChanged(nameof(RepeatDaysOfWeekLabel));
            }
        }

        private bool repeatFriday;
        public bool RepeatFriday
        {
            get { return repeatFriday; }
            set
            {
                repeatFriday = value;
                OnPropertyChanged(nameof(RepeatFriday));
                OnPropertyChanged(nameof(RepeatDaysOfWeekLabel));
            }
        }

        private bool repeatSaturday;
        public bool RepeatSaturday
        {
            get { return repeatSaturday; }
            set
            {
                repeatSaturday = value;
                OnPropertyChanged(nameof(RepeatSaturday));
                OnPropertyChanged(nameof(RepeatDaysOfWeekLabel));
            }
        }

        private bool repeatSunday;
        public bool RepeatSunday
        {
            get { return repeatSunday; }
            set
            {
                repeatSunday = value;
                OnPropertyChanged(nameof(RepeatSunday));
                OnPropertyChanged(nameof(RepeatDaysOfWeekLabel));
            }
        }

        /// <summary>
        /// This is the label assigned in alarmInfoView to show which repeat days are selected
        /// </summary>
        public string RepeatDaysOfWeekLabel
        {
            get
            {
                return CreateDaysOfWeekString();
            }
        }

        private string CreateDaysOfWeekString()
        {
            List<string> labelList = new List<string>();
            if (RepeatMonday)
                labelList.Add("Mon");
            if (RepeatTuesday)
                labelList.Add("Tue");
            if (RepeatWednesday)
                labelList.Add("Wed");
            if (RepeatThursday)
                labelList.Add("Thu");
            if (RepeatFriday)
                labelList.Add("Fri");
            if (RepeatSaturday)
                labelList.Add("Sat");
            if (RepeatSunday)
                labelList.Add("Sun");

            if (labelList.Count == 0)
                return "Never ";
            else if (labelList.Count == 7)
                return "Every day ";
            else
            {
                string returnLabel = "";
                foreach (string dayLabel in labelList)
                {
                    returnLabel += dayLabel + " ";
                }
                return returnLabel;
            }
        }
        ///

        private string name = "";
        public string Name
        {
            get
            {
                if (name == null)
                    return "Alarm";
                else
                    return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


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

        /// <summary>
        /// Deal with saving loading and deleting alarms
        /// </summary>
        private AlarmManager alarmManager;


        /// <summary>
        /// Edit name commands
        /// </summary>
        public Command DeleteNameCommand { get; set; }

        //

        public Command AddAlarmCommand { get; set; }
        public Command CancelButtonPressed { get; set; }
        public Command SaveButtonPressed { get; set; }
        public Command<Alarm> AlarmItemSelectedCommand { get; set; }
        public MainPageVM()
        {
            DeleteNameCommand = new Command(DeleteName);
            AlarmItemSelectedCommand = new Command<Alarm>(AlarmItemSelected);
            AddAlarmCommand = new Command(AddAlarm);


            alarmManager = new AlarmManager();
            AlarmList = new ObservableCollection<Alarm>();
            List<Alarm> loadedAlarms = alarmManager.LoadAlarms();
            if(loadedAlarms != null)
                foreach (Alarm alarm in loadedAlarms)
                {
                    AlarmList.Add(alarm);
                }
        }

        private void AddAlarm()
        {

        }

        private void DeleteName()
        {
            Name = "";
        }

        private void AlarmItemSelected(Alarm alarm)
        {
            InitialiseRepeatView(alarm.IsMonday, alarm.IsTuesday, alarm.IsWednesday, alarm.IsThursday, alarm.IsFriday, alarm.IsSaturday, alarm.IsSunday);
        }
        private void InitialiseRepeatView(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun)
        {
            RepeatMonday = mon;
            RepeatTuesday = tue;
            RepeatWednesday = wed;
            RepeatThursday = thu;
            RepeatFriday = fri;
            RepeatSaturday = sat;
            RepeatSunday = sun;      
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if(propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
