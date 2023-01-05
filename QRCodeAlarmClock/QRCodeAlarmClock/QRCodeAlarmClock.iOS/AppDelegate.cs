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
        #region Private Variables
        private NSError Error;
        #endregion

        #region Computed Properties
        public override UIWindow Window { get; set; }
        public bool CameraAvailable { get; set; }
        public AVCaptureSession Session { get; set; }
        public AVCaptureDevice CaptureDevice { get; set; }
        public OutputRecorder Recorder { get; set; }
        public DispatchQueue Queue { get; set; }
        public AVCaptureDeviceInput Input { get; set; }
        #endregion

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

            #region Camera
            StartCamera();
            #endregion


            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        void StartCamera()
        {
            // Create a new capture session
            Session = new AVCaptureSession();
            Session.SessionPreset = AVCaptureSession.PresetMedium;

            // Create a device input
            CaptureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            if (CaptureDevice == null)
            {
                // Video capture not supported, abort
                Console.WriteLine("Video recording not supported on this device");
                CameraAvailable = false;
                return;
            }

            // Prepare device for configuration
            CaptureDevice.LockForConfiguration(out Error);
            if (Error != null)
            {
                // There has been an issue, abort
                Console.WriteLine("Error: {0}", Error.LocalizedDescription);
                CaptureDevice.UnlockForConfiguration();
                return;
            }

            // Configure stream for 15 frames per second (fps)
            CaptureDevice.ActiveVideoMinFrameDuration = new CMTime(1, 15);

            // Unlock configuration
            CaptureDevice.UnlockForConfiguration();

            // Get input from capture device
            Input = AVCaptureDeviceInput.FromDevice(CaptureDevice);
            if (Input == null)
            {
                // Error, report and abort
                Console.WriteLine("Unable to gain input from capture device.");
                CameraAvailable = false;
                return;
            }

            // Attach input to session
            Session.AddInput(Input);

            // Create a new output
            var output = new AVCaptureVideoDataOutput();
            var settings = new AVVideoSettingsUncompressed();
            settings.PixelFormatType = CVPixelFormatType.CV32BGRA;
            output.WeakVideoSettings = settings.Dictionary;

            // Configure and attach to the output to the session
            Queue = new DispatchQueue("ManCamQueue");
            Recorder = new OutputRecorder();
            output.SetSampleBufferDelegate(Recorder, Queue);
            Session.AddOutput(output);

            // Let tabs know that a camera is available
            CameraAvailable = true;
        }
    }
}
