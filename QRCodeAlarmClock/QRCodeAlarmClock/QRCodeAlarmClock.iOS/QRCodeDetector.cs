using CoreImage;
using Foundation;
using QRCodeAlarmClock.Interfaces;
using QRCodeAlarmClock.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(QRCodeDetector))]
namespace QRCodeAlarmClock.iOS
{
    public class QRCodeDetector : IQRCodeDetector
    {
        public QRCodeDetector()
        {

        }

        public string ScanImage(byte[] image)
        {
            CIContext context = new CIContext();
            CIDetectorOptions options = new CIDetectorOptions()
            {
                Smile = false,
                EyeBlink = false,
            };

            var detector = CIDetector.CreateQRDetector(context, options);

            CIImage im = new CIImage(NSData.FromArray(image));
            CIFeature[] features = detector.FeaturesInImage(im);
            foreach (CIFeature f in features)
            {
                if (f.Type == CIFeature.TypeQRCode)
                {
                    var qrcode = (CIQRCodeFeature)f;
                    var x = qrcode.MessageString;
                    return x;
                }
            }

            return null;
        }
    }
}