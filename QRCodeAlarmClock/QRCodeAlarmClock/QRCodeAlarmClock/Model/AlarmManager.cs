using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QRCodeAlarmClock.Model
{
    public class AlarmManager
    {
        string filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Alarms.txt");

        public AlarmManager()
        {
            CreateFile();
        }

        public bool DeleteAlarm(Alarm alarm)
        {
            try
            {
                List<Alarm> alarms = LoadAlarms();
                if (alarms == null)
                    return false;

                Alarm foundAlarm = alarms.FirstOrDefault(a => a.ID == alarm.ID);

                bool containsAlarm = foundAlarm != null;
                if (containsAlarm)
                {
                    alarms.Remove(foundAlarm);
                }

                string saveToFileText = Newtonsoft.Json.JsonConvert.SerializeObject(alarms);
                File.WriteAllText(filePath, saveToFileText);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Alarm> LoadAlarms()
        {
            try
            {
                string fileText = File.ReadAllText(filePath);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Alarm>>(fileText);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Saves an alarm to the phone. Updates the alarm if it already exists
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public bool SaveAlarm(Alarm alarm)
        {
            try
            {
                List<Alarm> alarms = LoadAlarms();
                if (alarms == null)
                    return false;

                Alarm foundAlarm = alarms.FirstOrDefault(a => a.ID == alarm.ID);

                bool containsAlarm = foundAlarm != null;
                if (containsAlarm)
                {
                    alarms.Remove(foundAlarm);
                    alarm.ID = foundAlarm.ID;
                    alarms.Add(alarm);
                }
                else
                {
                    int ID = alarms.Count;
                    alarm.ID = ID;
                    alarms.Add(alarm);
                }

                string saveToFileText = Newtonsoft.Json.JsonConvert.SerializeObject(alarms);
                File.WriteAllText(filePath, saveToFileText);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CreateFile()
        {
            if (!File.Exists(filePath))
                using (FileStream f = File.Create(filePath)) ;
        }
    }
}
