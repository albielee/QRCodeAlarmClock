using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRCodeAlarmClock.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditInfoView : ContentView
    {
        const double buttonFlashOpacity = 0.3;
        const double buttonFlashReset = 0.1;

        bool isNavButtonPressed = false;


        public event ClosedEventHandler Closed;
        public delegate void ClosedEventHandler();

        public event OpenRepeatEventHandler OpenRepeat;
        public delegate void OpenRepeatEventHandler();

        public event OpenNameEventHandler OpenName;
        public delegate void OpenNameEventHandler();

        public event OpenSoundEventHandler OpenSound;
        public delegate void OpenSoundEventHandler();

        public event OpenQREventHandler OpenQR;
        public delegate void OpenQREventHandler();

        public EditInfoView()
        {
            InitializeComponent();
        }

        private void RepeatButton_Pressed(object sender, EventArgs e)
        {
            if (isNavButtonPressed)
                return;
            isNavButtonPressed = true;

            RepeatButton.BackgroundColor = Color.White;
            RepeatButtonFlashExtra.BackgroundColor = Color.White;

            OpenRepeat.Invoke();

            Animation fadeButton = new Animation(v =>
            {
                RepeatButton.Opacity = v;
                RepeatButtonFlashExtra.Opacity = v;
            }, buttonFlashOpacity, 0);
            fadeButton.Commit(this, "fadeButton", 4, 250, Easing.SinOut, (x, y) =>
            {
                RepeatButton.BackgroundColor = Color.Transparent;
                RepeatButton.Opacity = buttonFlashReset;
                RepeatButtonFlashExtra.BackgroundColor = Color.Transparent;
                RepeatButtonFlashExtra.Opacity = buttonFlashReset;
            });

            isNavButtonPressed = false;
        }

        private void NameButton_Pressed(object sender, EventArgs e)
        {
            if (isNavButtonPressed)
                return;
            isNavButtonPressed = true;

            NameButton.BackgroundColor = Color.White;

            OpenName.Invoke();

            Animation fadeButton = new Animation(v =>
            {
                NameButton.Opacity = v;
            }, buttonFlashOpacity, 0);

            fadeButton.Commit(this, "fadeButton", 4, 250, Easing.SinOut, (x, y) =>
            {
                NameButton.BackgroundColor = Color.Transparent;
                NameButton.Opacity = buttonFlashReset;
            });

            isNavButtonPressed = false;
        }

        private void SoundButton_Pressed(object sender, EventArgs e)
        {
            if (isNavButtonPressed)
                return;
            isNavButtonPressed = true;

            SoundButton.BackgroundColor = Color.White;

            OpenSound.Invoke();

            Animation fadeButton = new Animation(v =>
            {
                SoundButton.Opacity = v;
            }, buttonFlashOpacity, 0);

            fadeButton.Commit(this, "fadeButton", 4, 250, Easing.SinOut, (x, y) =>
            {
                SoundButton.BackgroundColor = Color.Transparent;
                SoundButton.Opacity = buttonFlashReset;
            });

            isNavButtonPressed = false;
        }

        private void QRButton_Pressed(object sender, EventArgs e)
        {
            if (isNavButtonPressed)
                return;
            isNavButtonPressed = true;

            QRButton.BackgroundColor = Color.White;
            QRButtonFlashExtra.BackgroundColor = Color.White;

            OpenQR.Invoke();

            Animation fadeButton = new Animation(v =>
            {

                QRButton.Opacity = v;
                QRButtonFlashExtra.Opacity = v;

            }, buttonFlashOpacity, 0);

            fadeButton.Commit(this, "fadeButton", 4, 250, Easing.SinOut, (x, y) => {

                QRButton.BackgroundColor = Color.Transparent;
                QRButton.Opacity = buttonFlashReset;
                QRButtonFlashExtra.BackgroundColor = Color.Transparent;
                QRButtonFlashExtra.Opacity = buttonFlashReset;

            });

            isNavButtonPressed = false;
        }

        private void Close_Pressed(object sender, EventArgs e)
        {
            Closed.Invoke();
        }
    }
}