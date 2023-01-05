using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeAlarmClock.Interfaces
{
    public interface IQRCodeDetector
    {
        string ScanImage(byte[] image);
    }
}
