using System;
using System.Collections.Generic;
using System.Linq;
using AVFoundation;
using CoreFoundation;
using CoreMedia;
using CoreVideo;
using Foundation;
using ObjCRuntime;
using QRCodeAlarmClock.Interfaces;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace QRCodeAlarmClock.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            DependencyService.Register<SetAlarm>();
            DependencyService.Register<RingAlarm>();
            DependencyService.Register<QRCodeDetector>();

            UNUserNotificationCenter center = UNUserNotificationCenter.Current;
            center.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge, (bool arg1, NSError arg2) =>
            {

            });
            var notifDelegate = new NotificationDelegate();
            notifDelegate.OnAlarmRangEvent += DependencyService.Get<IRingAlarm>().Ring;
            center.Delegate = notifDelegate;

            UIColor tintColor = UIColor.White;
            UINavigationBar.Appearance.TintColor = tintColor;
            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            UINavigationBar.Appearance.Translucent = true;

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

    }
}
