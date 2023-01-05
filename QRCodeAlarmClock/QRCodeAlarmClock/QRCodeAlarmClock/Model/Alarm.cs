using QRCodeAlarmClock.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeAlarmClock.Model
{
    public class Alarm
    {
        
        public int ID { get; set; }
        public string UniqueAlarmID { get; set; }
        ///For updating the alarm views
        public bool IsChanged { get; set; }

        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public bool IsSunday { get; set; }
        public bool IsEnabled { get; set; }
        private string name;
        public string Name
        {
            get
            {
                if (name == null || name == "")
                    return "Alarm";
                else
                    return name;
            }
            set
            {
                name = value;
                
                if(value == "")
                {
                    name = "Alarm";
                }
            }
        }
        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set
            {
                time = value;

                TimeString = ConvertTimeToString(value);
            }
        }
        public string QRCode { get; set; }
        public string RepeatDaysString
        {
            get
            {
                string dayOfWeek = Alarm.CreateDaysOfWeekString(IsMonday, IsTuesday, IsWednesday, IsThursday, IsFriday, IsSaturday, IsSunday);
                if(dayOfWeek.Trim() == "Never")
                {
                    return "";
                }
                else
                {
                    if (Name.Length+dayOfWeek.Length > 25)
                        return "\n" + dayOfWeek;
                    else
                        return dayOfWeek;
                }
            }
        }

        public string TimeString { get; set; }

        /// <summary>
        /// Create a new alarm
        /// </summary>
        public Alarm()
        {
            Name = "Alarm";
            IsEnabled = true;
            UniqueAlarmID = AlarmMethods.GenerateRandomString(DateTime.Now);
        }


        private string ConvertTimeToString(DateTime time)
        {
            if (time == null)
                return "Error";
            else
                return time.Hour.ToString("00") + ":" + time.Minute.ToString("00");
        }


        public static string CreateDaysOfWeekString(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun)
        {
            List<string> labelList = new List<string>();
            if (mon)
                labelList.Add("Mon");
            if (tue)
                labelList.Add("Tue");
            if (wed)
                labelList.Add("Wed");
            if (thu)
                labelList.Add("Thu");
            if (fri)
                labelList.Add("Fri");
            if (sat)
                labelList.Add("Sat");
            if (sun)
                labelList.Add("Sun");

            if (labelList.Count == 0)
                return "Never";
            else if (labelList.Count == 7)
                return "Every day";
            else
            {
                string returnLabel = "";
                foreach (string dayLabel in labelList)
                {
                    returnLabel += dayLabel + " ";
                }
                returnLabel = returnLabel.Trim();

                return returnLabel;
            }
        }
    }
}
