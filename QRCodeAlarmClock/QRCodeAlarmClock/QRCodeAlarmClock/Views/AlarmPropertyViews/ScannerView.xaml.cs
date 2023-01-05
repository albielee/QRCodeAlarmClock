using QRCodeAlarmClock.Interfaces;
using QRCodeAlarmClock.Model;
using QRCodeAlarmClock.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRCodeAlarmClock.Views.AlarmPropertyViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerView : ContentView
    {
        public event ScanSuccessfulEventHandler ScanSuccessful;
        public delegate void ScanSuccessfulEventHandler();

        SemaphoreSlim imageBeingCaptured = new SemaphoreSlim(1);

        bool setNewCode = false;
        Alarm alarm;

        uint maxHeight = (uint)(Application.Current.MainPage.Height / 2);
        uint openGridDelay = 100;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setNewQrCode">If true, the scanner sets the QR code. Else, just verify the QR is the same as the alarm</param>
        public ScannerView(bool _setNewQrCode, Alarm _alarm)
        {
            InitializeComponent();

            setNewCode = _setNewQrCode;
            alarm = _alarm;

            OpenScanGrid();
        }

        void OpenScanGrid()
        {
            Animation openMainGrid = new Animation(v => mainGrid.HeightRequest = v, 1, maxHeight);
            openMainGrid.Commit(this, "openMainGrid", 4, openGridDelay, Easing.CubicInOut);
        }

        void CloseScanGrid()
        {
            Animation closeMainGrid = new Animation(v => mainGrid.HeightRequest = v, maxHeight, 0);
            closeMainGrid.Commit(this, "closeMainGrid", 4, openGridDelay, Easing.CubicInOut);
        }

        private void cameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            imageBeingCaptured.Wait();

            MainPageVM viewModel = ((MainPageVM)BindingContext);

            byte[] image = e.ImageData;
            IQRCodeDetector detector = DependencyService.Get<IQRCodeDetector>();
            string scanResult = detector.ScanImage(image);

            //If a new code is being set
            if (setNewCode)
            {
                //viewModel.SetNewCode(scanResult, image);
            }
            //else, verify the code with the current alarm
            else
            {
                if (VerifyCode(scanResult))
                {
                    CloseScanGrid();
                    ScanSuccessful?.Invoke();
                }
            }

            imageBeingCaptured.Release();
        }

        bool VerifyCode(string result)
        {
            return result == alarm.QRCode;
        }

        private async void TapIconView_ButtonPressed()
        {
            //var fileresult = await MediaPicker.CapturePhotoAsync();
            Shutter();
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            Shutter();
        }

        private void Shutter()
        {
            flashOverlay.Opacity = 1;
            flashOverlay.FadeTo(0, 250, Easing.CubicOut);
            if (imageBeingCaptured.CurrentCount == 0)
                return;

            cameraView.Shutter();
        }
    }
}