using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeAlarmClock.Interfaces
{
    public interface IRingAlarm
    {
        event AlarmRangHandler AlarmRang;
        delegate void AlarmRangHandler();

        void Ring();
    }
}
