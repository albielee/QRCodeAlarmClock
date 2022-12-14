using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRCodeAlarmClock.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuNavButtonView : ContentView
	{
        private const double unfocusButtonDelay = 1000;
        private const double buttonFlashOpacity = 0.3;
        private const double buttonFlashReset = 0.1;

        bool buttonFocused = false;

        Timer _autoUnfocusTimer;
        Timer _autoUnfocusAfterTapTimer;

        public event ButtonPressedHandler ButtonPressed;
        public delegate void ButtonPressedHandler();


		public MenuNavButtonView ()
		{
			InitializeComponent();
		}

        private void Button_Pressed(object sender, EventArgs e)
        {
            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }


            DisposeTimer();
            CreateTimer();

            FocusButton();
            LightUp();
        }

        private void CreateUnfocusAfterTapTimer()
        {
            _autoUnfocusAfterTapTimer = new System.Timers.Timer();
            _autoUnfocusAfterTapTimer.Elapsed += new ElapsedEventHandler(UnfocusButtonTimer_Elapsed);
            _autoUnfocusAfterTapTimer.Interval = 500;
            _autoUnfocusAfterTapTimer.Enabled = true;
        }

        private void CreateTimer()
        {
            _autoUnfocusTimer = new System.Timers.Timer();
            _autoUnfocusTimer.Elapsed += new ElapsedEventHandler(UnfocusButtonTimer_Elapsed);
            _autoUnfocusTimer.Interval = unfocusButtonDelay;
            _autoUnfocusTimer.Enabled = true;
        }

        private void UnfocusButtonTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                LightOff();
                UnfocusButton();
            });

            DisposeAfterTapTimer();
            DisposeTimer();
        }

        private void FocusButton()
        {
            buttonFocused = true;
        }

        private void LightUp()
        {
            button.BackgroundColor = Color.White;
            button.Opacity = buttonFlashOpacity;
        }
        private void LightOff()
        {
            button.Opacity = buttonFlashReset;
            button.BackgroundColor = Color.Transparent;
        }

        private void DisposeAfterTapTimer()
        {
            if (_autoUnfocusAfterTapTimer != null)
            {
                _autoUnfocusAfterTapTimer.Enabled = false;
                _autoUnfocusAfterTapTimer.Stop();
                _autoUnfocusAfterTapTimer.Dispose();
            }
        }

        private void DisposeTimer()
        {
            if (_autoUnfocusTimer != null)
            {
                _autoUnfocusTimer.Enabled = false;
                _autoUnfocusTimer.Stop();
                _autoUnfocusTimer.Dispose();
            }
            
        }

        private void UnfocusButton()
        {
            buttonFocused = false;
        }

        private void Button_Released(object sender, EventArgs e)
        {

            if (buttonFocused)
            {
                try
                {
                    HapticFeedback.Perform(HapticFeedbackType.Click);
                }
                catch { }

                CreateUnfocusAfterTapTimer();

                UnfocusButton();
                ButtonPressed?.Invoke();
            }

            DisposeTimer();
        }
    }
}