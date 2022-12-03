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

        private void RepeatButton_Pressed()
        {
            OpenRepeat.Invoke();
        }

        private void NameButton_Pressed()
        {
            OpenName.Invoke();
        }

        private void SoundButton_Pressed()
        {
            OpenSound.Invoke();
        }

        private void QRButton_Pressed()
        {
            OpenQR.Invoke();
        }

        private void Close_Pressed(object sender, EventArgs e)
        {
            Closed.Invoke();
        }
    }
}