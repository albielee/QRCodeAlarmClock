using QRCodeAlarmClock.Interfaces;
using QRCodeAlarmClock.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace QRCodeAlarmClock.ViewModels
{
    public class MainPageVM : INotifyPropertyChanged
    {
        public string SetQRCode { get; set; }

        private ImageSource setImage { get; set; }
        public ImageSource SetImage
        {
            get { return setImage; }
            set
            {
                setImage = value;
                OnPropertyChanged(nameof(SetImage));
            }
        }

        private DateTime setTime;
        /// <summary>
        /// The time set for currently selected alarm, or new alarm
        /// </summary>
        public DateTime SetTime
        {
            get { return setTime; }
            set
            {
                setTime = value;
                OnPropertyChanged(nameof(SetTime));
            }
        }

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
                return Alarm.CreateDaysOfWeekString(RepeatMonday, RepeatTuesday, RepeatWednesday, RepeatThursday, RepeatFriday, RepeatSaturday, RepeatSunday);
            }
        }

        
        ///

        private string setName = "";
        /// <summary>
        /// The name of the alarm selected or being created. Reset on adding an alarm or deleting / deselecting an alarm
        /// </summary>
        public string SetName
        {
            get
            {
                return setName;
            }
            set
            {
                if (value == null)
                    value = "Alarm";

                setName = value;

                OnPropertyChanged(nameof(SetName));
            }
        }


        private bool isAlarmSelected;
        public bool IsAlarmSelected
        {
            get { return isAlarmSelected; }
            set
            {
                isAlarmSelected = value;
                OnPropertyChanged(nameof(IsAlarmSelected));
            }
        }

        private Alarm alarmSelected;

        /// <summary>
        /// Required so the view behind is notified of any changes to the alarm list.
        /// I much prefer making the view objects in the code behind so an event is used as a binding to update the alarm list.
        /// Also, binding the list to a stacklayout provides less freedom for animations.. grumble grumble
        /// </summary>
        public event AlarmListChangedEventHandler AlarmListChanged;
        public delegate void AlarmListChangedEventHandler();

        private ObservableCollection<Alarm> alarmList;
        public ObservableCollection<Alarm> AlarmList
        {
            get { return alarmList; }
            set
            {
                alarmList = value;
                OnPropertyChanged(nameof(AlarmList));

                AlarmListChanged?.Invoke(); 
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
        public MainPageVM()
        {
            DeleteNameCommand = new Command(DeleteName);
            AddAlarmCommand = new Command(ResetAlarmInfo);
            SaveButtonPressed = new Command(SaveAlarm);
            CancelButtonPressed = new Command(ResetAlarmInfo);

            alarmManager = new AlarmManager();
            AlarmList = new ObservableCollection<Alarm>();

            InitAlarms();
        }

        private void InitAlarms()
        {
            List<Alarm> loadedAlarms = alarmManager.LoadAlarms();
            if (loadedAlarms != null)
                foreach (Alarm alarm in loadedAlarms)
                {
                    AlarmList.Add(alarm);
                }
        }

        public void UpdateAlarmToggle(Alarm alarm, bool toggled)
        {
            foreach(Alarm a in AlarmList)
            {
                if(a.ID == alarm.ID)
                {
                    a.IsEnabled = toggled;
                    alarmManager.SaveAlarm(a);

                    if (toggled)
                    {
                        DependencyService.Get<ISetAlarm>().Init(alarm);
                    }
                    else
                    {
                        DependencyService.Get<ISetAlarm>().StopAlarm(alarm);
                    }
                    break;
                }
            }
        }

        private void SaveAlarm()
        {
            Alarm alarm = new Alarm()
            {
                IsMonday = RepeatMonday,
                IsTuesday = RepeatTuesday,
                IsWednesday = RepeatWednesday,
                IsThursday = RepeatThursday,
                IsFriday = RepeatFriday,
                IsSaturday = RepeatSaturday,
                IsSunday = RepeatSunday,
                Time = SetTime,
                Name = SetName,
                QRCode = "TEST"//SetQRCode,
            };

            if(alarmSelected != null)
            {
                bool alarmFound = false;

                int index = 0;
                foreach(Alarm a in AlarmList)
                {
                    if(a.ID == alarmSelected.ID)
                    {
                        alarm.ID = alarmSelected.ID;
                        alarm.Time = alarmSelected.Time; //remove once I have time setup
                        alarm.IsEnabled = alarmSelected.IsEnabled;
                        alarm.IsChanged = true;

                        alarmFound = true;
                        break;
                    }

                    index++;
                }
                if (!alarmFound)
                    return;

                AlarmList[index] = alarm;

                alarmSelected = null;
            }
            else
            {
                if(alarmList.Count > 0)
                {
                    alarm.ID = alarmList.OrderBy(x => x.ID).Last().ID + 1;
                }
                else
                {
                    alarm.ID = 0;
                }
                
                alarm.IsChanged = false;

                AddAlarm(alarm); 
            }

            DependencyService.Get<ISetAlarm>().Init(alarm);

            alarmManager.SaveAlarm(alarm);
            AlarmListChanged?.Invoke();
            ResetAlarmInfo();
        }

        public void DeleteAlarm()
        {
            List<Alarm> alarms = AlarmList.Where(x => x.ID == alarmSelected.ID).ToList();
            foreach(Alarm a in alarms)
            {
                AlarmList.Remove(a);
            }
            
            AlarmListChanged?.Invoke();

            alarmManager.DeleteAlarm(alarmSelected);

            DependencyService.Get<ISetAlarm>().StopAlarm(alarmSelected);

            ResetAlarmInfo();
        }

        private void AddAlarm(Alarm alarm)
        {
            AlarmList.Add(alarm);
            OnPropertyChanged(nameof(AlarmList));
        }

        private void ResetAlarmInfo()
        {
            SetQRCode = null;

            RepeatMonday = false;
            RepeatTuesday = false;
            RepeatWednesday = false;
            RepeatThursday = false;
            RepeatFriday = false;
            RepeatSaturday = false;
            RepeatSunday = false;
            SetName = "";
            SetTime = DateTime.Now.AddSeconds(10);

            alarmSelected = null;
            IsAlarmSelected = false;
        }

        private void DeleteName()
        {
            SetName = "";
        }

        public void AlarmItemSelected(Alarm alarm)
        {
            ResetAlarmInfo();

            IsAlarmSelected = true;
            alarmSelected = alarm;
            InitialiseRepeatView(alarm.IsMonday, alarm.IsTuesday, alarm.IsWednesday, alarm.IsThursday, alarm.IsFriday, alarm.IsSaturday, alarm.IsSunday);
            InitialiseNameView(alarm.Name);
        }
        private void InitialiseNameView(string name)
        {
            SetName = name;
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
