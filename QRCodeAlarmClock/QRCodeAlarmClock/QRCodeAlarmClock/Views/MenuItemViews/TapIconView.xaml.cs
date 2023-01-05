using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRCodeAlarmClock.Views.MenuItemViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TapIconView : ContentView
    {
        public event ButtonPressedEventHandler ButtonPressed;
        public delegate void ButtonPressedEventHandler();

        uint opacityDelay = 500;
        uint secondRippleDelay = 200;
        uint growDelay = 1000;
        uint tapDelay = 300;
        uint tapWaitDelay = 1000;
        double minTapIconScale = 0.12;
        double maxTapIconScale = 0.17;
        double minCircleScale = 0.02;
        double maxCircleScale = 0.75;
        double rippleStartOpacity = 0.3;

        private bool stopped = false;
        double nullValue;

        Button b = null;
        public TapIconView()
        {
            InitializeComponent();

            StartTapIconAnim();

            b = new Button()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
            };
            b.Pressed += Button_Pressed;

            mainGrid.Children.Add(b);
        }

        void StartTapIconAnim()
        {
            Animation tapIconShrink = new Animation(v => tapIcon.Scale = v, maxTapIconScale, minTapIconScale);
            tapIconShrink.Commit(this, "tapIconShrink", 4, tapDelay, Easing.CubicIn, (x, y) =>
            {
                if(!stopped)
                {
                    //Tapped
                    TapIconGrow();

                    CreateRipples();
                }
            });
        }
        void TapIconGrow()
        {
            Animation tapIconGrow = new Animation(v => tapIcon.Scale = v, minTapIconScale, maxTapIconScale);
            tapIconGrow.Commit(this, "tapIconGrow", 4, tapDelay, Easing.CubicIn, (x, y) =>
            {
                Animation tapIconWait = new Animation(v => nullValue = v, 0, 1);
                tapIconWait.Commit(this, "tapIconWait", 4, tapWaitDelay, finished: (x, y) =>
                {
                    StartTapIconAnim();
                });
            });
        }

        void CreateRipples()
        {
            Animation startSecondEllipseDelay = new Animation(v => nullValue = v, 0, 1);
            startSecondEllipseDelay.Commit(this, "startSecondEllipseDelay", 4, secondRippleDelay, finished: (x, y) => {
                StartEllipse2Grow();
                StartEllipse2Appear();
            });

            StartEllipse1Appear();
            StartEllipse1Grow();
        }

        void StartEllipse2Grow()
        {
            Animation ellipse2Grow = new Animation(v => ellipse2.Scale = v, minCircleScale, maxCircleScale);
            ellipse2Grow.Commit(this, "ellipse2Grow", 4, growDelay, finished: (x, y) =>
            {
                
            });
        }
        void StartEllipse2Appear()
        {
            Animation elipse2Appear = new Animation(v => ellipse2.Opacity = v, rippleStartOpacity, 1);
            elipse2Appear.Commit(this, "elipse2Appear", 4, opacityDelay, Easing.CubicInOut, finished: (x, y) =>
            {
                StartEllipse2Disappear();
            });
        }
        void StartEllipse2Disappear()
        {
            Animation ellipse2Disappear = new Animation(v => ellipse2.Opacity = v, 1, 0);
            ellipse2Disappear.Commit(this, "ellipse2Disappear", 4, opacityDelay, Easing.CubicInOut, finished: (x, y) =>
            {

            });
        }

        void StartEllipse1Grow()
        {
            Animation ellipse1Grow = new Animation(v => ellipse1.Scale = v, minCircleScale, maxCircleScale);
            ellipse1Grow.Commit(this, "ellipse1Grow", 4, growDelay, finished: (x, y) =>
            {
                
            });
        }
        void StartEllipse1Appear()
        {
            Animation elipse1Appear = new Animation(v => ellipse1.Opacity = v, rippleStartOpacity, 1);
            elipse1Appear.Commit(this, "elipse1Appear", 4, opacityDelay, Easing.CubicInOut, finished: (x, y) =>
            {
                StartEllipse1Disappear();
            });
        }
        void StartEllipse1Disappear()
        {
            Animation ellipse1Disappear = new Animation(v => ellipse1.Opacity = v, 1, 0);
            ellipse1Disappear.Commit(this, "ellipse1Disappear", 4, opacityDelay, Easing.CubicInOut, finished: (x, y) =>
            {

            });
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            ButtonPressed?.Invoke();

            if (!stopped)
            {
                Animation fadeTapIcon = new Animation(v => tapIcon.Opacity = v, 1, 0);
                fadeTapIcon.Commit(this, "fadeTapIcon", 4, 250, Easing.CubicInOut);
                stopped = true;
            }
        }
    }
}