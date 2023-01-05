using Foundation;
using QRCodeAlarmClock.Interfaces;
using QRCodeAlarmClock.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace QRCodeAlarmClock.iOS
{
    public class NotificationDelegate : UNUserNotificationCenterDelegate
    {
        public event OnMessageCallbackHandler OnAlarmRangEvent;
        public delegate void OnMessageCallbackHandler();

        public NotificationDelegate()
        {
        }

        /// <summary>
        /// Handle notifications when the app is open
        /// </summary>
        /// <param name="center"></param>
        /// <param name="notification"></param>
        /// <param name="completionHandler"></param>
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            center.RemoveAllPendingNotificationRequests();
            center.RemoveAllDeliveredNotifications();
            OnAlarmRangEvent?.Invoke();
            

            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            center.RemoveAllDeliveredNotifications();

            // Take action based on Action ID
            switch (response.ActionIdentifier)
            {
                case "reply":
                    // Do something
                    break;
                default:
                    // Take action based on identifier
                    if (response.IsDefaultAction) //Triggers when the user opens the notification
                    {
                        OnAlarmRangEvent?.Invoke();
                        // Handle default action...
                    }
                    else if (response.IsDismissAction)
                    {
                        // Handle dismiss action
                    }
                    break;
            }

            // Inform caller it has been handled
            completionHandler();
        }

    }
}