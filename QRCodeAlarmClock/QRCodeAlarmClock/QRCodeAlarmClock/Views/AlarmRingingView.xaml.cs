using QRCodeAlarmClock.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.UI.Views;
using System.Threading;
using QRCodeAlarmClock.Interfaces;
using QRCodeAlarmClock.Views.AlarmPropertyViews;

namespace QRCodeAlarmClock.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlarmRingingView : ContentView
    {
        public event TappedEventDelegate Tapped;
        public delegate void TappedEventDelegate(List<Alarm> alarmList);

        ScannerView scannerView;

        private Alarm currentAlarm;
        private List<Alarm> alarmList;
        private bool stopped = false;
        public AlarmRingingView(List<Alarm> aList)
        {
            InitializeComponent();

            alarmList = aList;
            SetCurrentAlarm();

            Start();
        }

        public void AddAlarms(List<Alarm> aList)
        {
            if (alarmList == null)
                alarmList = new List<Alarm>();

            alarmList.AddRange(aList);
            SetCurrentAlarm();
        }

        private void SetCurrentAlarm()
        {
            if(alarmList.Count > 0)
            {
                currentAlarm = alarmList.OrderBy(a => a.Time).First();
                alarmLabel.Text = currentAlarm.Name;
            }
        }

        public void Start()
        {
            mainView.Opacity = 1;
            mainView.InputTransparent = false;

            BeginFlashAnimations();
        }

        private void BeginFlashAnimations()
        {
            flashFrame.Opacity = 1;

            Animation fadeFlashFrame = new Animation(v => flashFrame.Opacity = v, 0.4, 0);
            fadeFlashFrame.Commit(this, "fadeFlashFrame", 4, 1000, Easing.CubicInOut);

            Animation growYFlashFrame = new Animation(v => flashFrame.ScaleY = v, 0.1, 1);
            growYFlashFrame.Commit(this, "growYFlashFrame", 4, 1000, Easing.CubicInOut);

            Animation growXFlashFrame = new Animation(v => flashFrame.ScaleX = v, 0.4, 1);
            growXFlashFrame.Commit(this, "growFlashFrame", 4, 1000, Easing.CubicInOut, (x, y) =>
            {
                Animation delayAnim = new Animation(v => flashFrame.ScaleX = v, 1, 0.1);
                delayAnim.Commit(this, "delayAnim", 4, 250, Easing.Linear, (a, b) =>
                {
                    if(!stopped)
                        BeginFlashAnimations();
                });
            });
        }

        public void Stop()
        {
            if(!stopped) //Make sure we dont create multiple scanner instances...
            {
                stopped = true;
                if (currentAlarm.QRCode != null)
                {
                    alarmButton.InputTransparent = true;
                    scanStackView.InputTransparent = false;

                    scannerView = new(false, currentAlarm);
                    scannerView.ScanSuccessful += ScannerView_ScanSuccessful;
                    scanStackView.Children.Insert(0, scannerView);
                }
                else
                {
                    //Reset the alarm
                    //hide the alarm
                    HideAlarm();
                }
            }
        }

        private void ScannerView_ScanSuccessful()
        {
            HideAlarm();
        }

        void DisposeObjects()
        {
            if (scannerView != null)
            {
                scannerView.ScanSuccessful -= ScannerView_ScanSuccessful;
                scannerView = null;
            }  
        }

        private void HideAlarm()
        {
            DisposeObjects();

            Animation fadeMainView = new Animation(v => mainView.Opacity = v, 1, 0);
            fadeMainView.Commit(this, "fadeMainView", 4, 1000, Easing.CubicInOut, (x, y) =>
            {
                Tapped?.Invoke(alarmList);
            });
        }


        private void Button_Pressed(object sender, EventArgs e)
        {
            Stop();
        }
    }
}