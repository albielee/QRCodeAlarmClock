using Foundation;
using QRCodeAlarmClock.Interfaces;
using QRCodeAlarmClock.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(RingAlarm))]
namespace QRCodeAlarmClock.iOS
{
    public class RingAlarm : IRingAlarm
    {
        public event IRingAlarm.AlarmRangHandler AlarmRang;
        public void Ring()
        {
            AlarmRang?.Invoke();
        }
    }
}