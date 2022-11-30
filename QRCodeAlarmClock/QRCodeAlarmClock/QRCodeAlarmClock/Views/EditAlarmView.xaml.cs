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
    public partial class EditAlarmView : ContentView
    {
        const int topOffset = 90;

        bool isOpen;

        public ClosedDelegate Closed;
        public delegate void ClosedDelegate();
        public EditAlarmView()
        {
            InitializeComponent();

            Open();
        }

        public void Open()
        {
            if (!isOpen)
            {
                isOpen = true;

                Animation animFlyIn = new Animation(v => editorViewFrame.TranslationY = v, Application.Current.MainPage.Height, topOffset);
                animFlyIn.Commit(this, "FlyIn", 16, 250, Easing.CubicInOut);

                Animation animFadeIn = new Animation(v => editorViewFrame.Opacity = v, 0, 1);
                animFadeIn.Commit(this, "FadeIn", 16, 250, Easing.CubicInOut);
            }
        }
            
        public void Close()
        {
            if (isOpen)
            {
                Animation animFlyOut = new Animation(v => editorViewFrame.TranslationY = v, topOffset, Application.Current.MainPage.Height);
                animFlyOut.Commit(this, "FlyOut", 16, 250, Easing.CubicInOut, (v, c) => FlyoutEnded());
            }
        }

        private void DeselectButton_Pressed(object sender, EventArgs e)
        {
            Close();
        }

        private void FlyoutEnded()
        {
            isOpen = false;

            Closed.Invoke();
        }

        private void FrameScrollAwayView_Scrolled(object sender, ScrolledEventArgs e)
        {
            editorViewFrame.TranslationY = -e.ScrollY+300;
        }
    }
}