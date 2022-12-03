using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRCodeAlarmClock.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuButtonView : ContentView
	{
        private const double unfocusButtonDelay = 1000;
        private const double buttonFlashOpacity = 0.3;
        private const double buttonFlashReset = 0.1;

        bool buttonFocused = false;

        Timer _autoUnfocusTimer;

        public event ButtonPressedHandler ButtonPressed;
        public delegate void ButtonPressedHandler();

		public MenuButtonView ()
		{
			InitializeComponent();
		}

        private void Button_Pressed(object sender, EventArgs e)
        {
            DisposeTimer();
            CreateTimer();

            FocusButton();
            LightUp();
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
                if (_autoUnfocusTimer != null)
                {
                    if (buttonFocused)
                    {
                        LightOff();
                    }

                    UnfocusButton();
                }
            });

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

        private void LightSplash()
        {
            button.BackgroundColor = Color.White;

            Animation fadeButton = new Animation(v =>
            {
                button.Opacity = v;
            }, buttonFlashOpacity, 0);
            fadeButton.Commit(this, "fadeButton", 4, 500, Easing.SinOut, (x, y) =>
            {
                LightOff();
            });
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
                LightSplash();

                UnfocusButton();
                ButtonPressed?.Invoke();
            }

            DisposeTimer();
        }
    }
}