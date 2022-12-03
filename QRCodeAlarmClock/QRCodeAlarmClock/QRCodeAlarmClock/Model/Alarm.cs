using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeAlarmClock.Model
{
    public class Alarm
    {
        public int ID { get; set; }
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
            }
        }
        public DateTime Time { get; set; }
        public string QRCode { get; set; }

        public string TimeString
        {
            get { return ConvertTimeToString(Time); }
        }

        /// <summary>
        /// Create a new alarm
        /// </summary>
        public Alarm()
        {
            Name = "Alarm";
            IsEnabled = true;
        }

        /// <summary>
        /// Load an alarm
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <param name="name"></param>
        /// <param name="time"></param>
        /// <param name="qRCode"></param>
        public Alarm(bool isEnabled, string name, DateTime time, string qRCode)
        {
            IsEnabled = isEnabled;
            Name = name;
            Time = time;
            QRCode = qRCode;
        }


        private string ConvertTimeToString(DateTime time)
        {
            if (time == null)
                return "Error";
            else
                return time.Hour.ToString("00") + ":" + time.Minute.ToString("00");
        }
    }
}
