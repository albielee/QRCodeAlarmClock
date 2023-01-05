using QRCodeAlarmClock.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeAlarmClock.Interfaces
{
    public interface ISetAlarm
    {
        void Init(Alarm alarm);
        void StopAlarm();
        void StopAlarm(Alarm alarm);
    }
}
