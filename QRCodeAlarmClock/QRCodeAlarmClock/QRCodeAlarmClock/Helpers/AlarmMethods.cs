using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRCodeAlarmClock.Helpers
{
    public class AlarmMethods
    {
        public static string GenerateRandomString(DateTime currentTime)
        {
            // Get the ticks (the number of milliseconds since January 1, 0001)
            // from the current time
            long ticks = currentTime.Ticks;

            // Use the ticks as a seed for the random number generator
            var random = new Random((int)(ticks & 0xffffffffL) | (int)(ticks >> 32));

            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(characters, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
