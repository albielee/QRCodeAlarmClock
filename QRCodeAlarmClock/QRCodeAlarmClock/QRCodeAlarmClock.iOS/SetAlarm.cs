using Foundation;
using QRCodeAlarmClock.Interfaces;
using QRCodeAlarmClock.iOS;
using QRCodeAlarmClock.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(SetAlarm))]
namespace QRCodeAlarmClock.iOS
{
    public class SetAlarm : ISetAlarm
    {
        const int timeBetweenAlarms = 10;
        const int numberOfWakeupAlarms = 30;
        const int secondsInADay = 86400;
        const int daysInAWeek = 7;

        Dictionary<DayOfWeek, int> dayToIntDict = new Dictionary<DayOfWeek, int>()
        {
            {DayOfWeek.Monday, 0 },
            {DayOfWeek.Tuesday, 1 },
            {DayOfWeek.Wednesday, 2 },
            {DayOfWeek.Thursday, 3 },
            {DayOfWeek.Friday, 4 },
            {DayOfWeek.Saturday, 5 },
            {DayOfWeek.Sunday, 6 },
        };

        /// <summary>
        /// Sets the alarms for a specific alarm input
        /// </summary>
        /// <param name="alarm"></param>
        public void Init(Alarm alarm)
        {
            StopAlarm(alarm);

            string alarmSound = "defaultAlarmSound.caf";//alarm.Sound;


            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            double currentTimeSeconds = DateTime.Now.TimeOfDay.TotalSeconds;
            DateTime setTime = alarm.Time;
            double setTimeSeconds = setTime.TimeOfDay.TotalSeconds;

            bool notRepeating = !alarm.IsMonday && !alarm.IsTuesday && !alarm.IsWednesday && !alarm.IsThursday && !alarm.IsFriday && !alarm.IsSaturday && !alarm.IsSunday;
            if(notRepeating)
            {
                int addTime = 0;
                if (currentTimeSeconds > setTimeSeconds)
                    addTime = secondsInADay;

                double secondsTillNextAlarm = (setTimeSeconds + addTime) - currentTimeSeconds;
                string identifier = alarm.UniqueAlarmID + "-never";
                CreateAlarms(secondsTillNextAlarm, identifier, alarm.Name, alarmSound);
            }
            else
            {
                if (alarm.IsMonday)
                {
                    int offset = CalculateDayOffset(currentDay, DayOfWeek.Monday);
                    double secondsTillNextAlarm = CalculateDelay(setTimeSeconds, currentTimeSeconds, offset);
                    
                    string identifier = alarm.UniqueAlarmID + "-mon";
                    CreateAlarms(secondsTillNextAlarm, identifier, alarm.Name, alarmSound);
                }
                if (alarm.IsTuesday)
                {
                    int offset = CalculateDayOffset(currentDay, DayOfWeek.Tuesday);
                    double secondsTillNextAlarm = CalculateDelay(setTimeSeconds, currentTimeSeconds, offset);
                    string identifier = alarm.UniqueAlarmID + "-tue";
                    CreateAlarms(secondsTillNextAlarm, identifier, alarm.Name, alarmSound);
                }
                if (alarm.IsWednesday)
                {
                    int offset = CalculateDayOffset(currentDay, DayOfWeek.Wednesday);
                    double secondsTillNextAlarm = CalculateDelay(setTimeSeconds, currentTimeSeconds, offset);
                    string identifier = alarm.UniqueAlarmID + "-wed";
                    CreateAlarms(secondsTillNextAlarm, identifier, alarm.Name, alarmSound);
                }
                if (alarm.IsThursday)
                {
                    int offset = CalculateDayOffset(currentDay, DayOfWeek.Thursday);
                    double secondsTillNextAlarm = CalculateDelay(setTimeSeconds, currentTimeSeconds, offset);
                    string identifier = alarm.UniqueAlarmID + "-thu";
                    CreateAlarms(secondsTillNextAlarm, identifier, alarm.Name, alarmSound);
                }
                if (alarm.IsFriday)
                {
                    int offset = CalculateDayOffset(currentDay, DayOfWeek.Friday);
                    double secondsTillNextAlarm = CalculateDelay(setTimeSeconds, currentTimeSeconds, offset);
                    string identifier = alarm.UniqueAlarmID + "-fri";
                    CreateAlarms(secondsTillNextAlarm, identifier, alarm.Name, alarmSound);
                }
                if (alarm.IsSaturday)
                {
                    int offset = CalculateDayOffset(currentDay, DayOfWeek.Saturday);
                    double secondsTillNextAlarm = CalculateDelay(setTimeSeconds, currentTimeSeconds, offset);
                    string identifier = alarm.UniqueAlarmID + "-sat";
                    CreateAlarms(secondsTillNextAlarm, identifier, alarm.Name, alarmSound);
                }
                if (alarm.IsSunday)
                {
                    int offset = CalculateDayOffset(currentDay, DayOfWeek.Sunday);
                    double secondsTillNextAlarm = CalculateDelay(setTimeSeconds, currentTimeSeconds, offset);
                    string identifier = alarm.UniqueAlarmID + "-sun";
                    CreateAlarms(secondsTillNextAlarm, identifier, alarm.Name, alarmSound);
                }
            }
        }

        private UNNotificationContent CreateNotificationContent(string name, string sound)
        {
            UNMutableNotificationContent notificationContent = new UNMutableNotificationContent();
            notificationContent.Title = name;
            notificationContent.Body = "Tap to open";
            notificationContent.InterruptionLevel = UNNotificationInterruptionLevel.Critical;
            notificationContent.Sound = UNNotificationSound.GetSound(sound);

            return notificationContent;
        }

        private void CreateAlarms(double delay, string identifier, string name, string sound)
        {
            UNUserNotificationCenter center = UNUserNotificationCenter.Current;

            RemoveAlarm(identifier);

            var notificationContent = CreateNotificationContent(name, sound);

            
            for(int i = 0; i<numberOfWakeupAlarms; i++)
            {
                UNTimeIntervalNotificationTrigger trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(delay + i*timeBetweenAlarms, false);
                UNNotificationRequest request = UNNotificationRequest.FromIdentifier(identifier+$"-{i}", notificationContent, trigger);
                center.AddNotificationRequest(request, (NSError obj) => { });
            }
        }

        private void RemoveAlarm(string identifier)
        {
            UNUserNotificationCenter center = UNUserNotificationCenter.Current;
            string[] alarmToRemove = new string[] { identifier };
            center.RemovePendingNotificationRequests(alarmToRemove);
        }

        private double CalculateDelay(double setTimeSeconds, double currentTimeSeconds, int offset)
        {
            if (currentTimeSeconds > setTimeSeconds && offset == 0)
            {
                offset = daysInAWeek;
            }

            return (setTimeSeconds + secondsInADay * offset) - currentTimeSeconds;
        }
        private int CalculateDayOffset(DayOfWeek day1, DayOfWeek day2)
        {
            int setDayOffset = dayToIntDict[day2];
            int currentDayOffset = dayToIntDict[day1];
            int offset = setDayOffset - currentDayOffset;
            if (offset < 0)
                offset = daysInAWeek + offset;

            return offset;
        }

        public void StopAlarm()
        {
            UNUserNotificationCenter center = UNUserNotificationCenter.Current;
            center.RemoveAllDeliveredNotifications();

            string[] alarmsToRemove = new string[] { "Love", "Pain" };
            center.RemovePendingNotificationRequests(alarmsToRemove);
        }

        public void StopAlarm(Alarm alarm)
        {
            UNUserNotificationCenter center = UNUserNotificationCenter.Current;
            center.RemoveAllDeliveredNotifications();

            List<string> alarmsToRemove = new List<string> { "Love", "Pain" };
            if (alarm.IsMonday)
            {
                alarmsToRemove.Add(alarm.UniqueAlarmID + "-mon");
            }
            if (alarm.IsTuesday)
            {
                alarmsToRemove.Add(alarm.UniqueAlarmID + "-tue");
            }
            if (alarm.IsWednesday)
            {
                alarmsToRemove.Add(alarm.UniqueAlarmID + "-wed");
            }
            if (alarm.IsThursday)
            {
                alarmsToRemove.Add(alarm.UniqueAlarmID + "-thu");
            }
            if (alarm.IsFriday)
            {
                alarmsToRemove.Add(alarm.UniqueAlarmID + "-fri");
            }
            if (alarm.IsSaturday)
            {
                alarmsToRemove.Add(alarm.UniqueAlarmID + "-sat");
            }
            if (alarm.IsSunday)
            {
                alarmsToRemove.Add(alarm.UniqueAlarmID + "-sun");
            }

            center.RemovePendingNotificationRequests(alarmsToRemove.ToArray());
        }
    }
}