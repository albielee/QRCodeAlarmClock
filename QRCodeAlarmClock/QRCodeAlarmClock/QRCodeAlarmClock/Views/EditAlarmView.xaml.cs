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
        const int topOffset = 80;

        //Editing views//
        EditInfoView infoView;
        EditRepeatView repeatView;
        //

        bool viewBeingOpened = false;
        bool isOpen;

        public ClosedDelegate Closed;
        public delegate void ClosedDelegate();

        public StartedClosingDelegate StartedClosing;
        public delegate void StartedClosingDelegate();
        public EditAlarmView()
        {
            InitializeComponent();

            CreateInfoView();

            Open();
        }

        private void CreateInfoView()
        {
            infoView = new EditInfoView() { };
            infoView.OpenQR += EditInfoView_OpenQR;
            infoView.OpenName += EditInfoView_OpenName;
            infoView.OpenRepeat += EditInfoView_OpenRepeat;
            infoView.OpenSound += EditInfoView_OpenSound;
            infoView.Closed += Close;

            AlarmEditingViews.Children.Add(infoView);
        }

        public void Open()
        {
            if (!isOpen)
            {
                isOpen = true;

                Animation animFlyIn = new Animation(v => editorViewFrame.TranslationY = v, Application.Current.MainPage.Height, topOffset);
                animFlyIn.Commit(this, "FlyIn", 4, 500, Easing.CubicInOut);

                Animation animFadeIn = new Animation(v => editorViewFrame.Opacity = v, 0, 1);
                animFadeIn.Commit(this, "FadeIn", 4, 500, Easing.CubicInOut);
            }
        }
            
        public void Close()
        {
            if (isOpen)
            {
                StartedClosing.Invoke();

                Animation animFlyOut = new Animation(v => editorViewFrame.TranslationY = v, topOffset, Application.Current.MainPage.Height);
                animFlyOut.Commit(this, "FlyOut", 4, 500, Easing.CubicInOut, (v, c) => FlyoutEnded());
            }
        }

        private void Close_Pressed(object sender, EventArgs e)
        {
            Close();
        }

        private void FlyoutEnded()
        {
            isOpen = false;

            Closed.Invoke();
        }

        /// <summary>
        /// Create the Repeat view and slide it into view. Remove the InfoView
        /// </summary>
        private void EditInfoView_OpenRepeat()
        {
            if (viewBeingOpened)
                return;
            viewBeingOpened = true;

            CreateRepeatView();
            OpenRightView(infoView, repeatView);

            viewBeingOpened = false;
        }

        private void RemoveView(View view)
        {
            if (AlarmEditingViews.Children.Contains(view))
            {
                AlarmEditingViews.Children.Remove(view);
            }
            view = null;
        }


        private void OpenRightView(View currentView, View newView)
        {
            double moveDist = Application.Current.MainPage.Width;

            Animation slideView1 = new Animation(v =>
            {
                currentView.TranslationX = v;
            }, 0, -moveDist/2);
            slideView1.Commit(this, "slideView1", 4, 400, Easing.SinInOut, (c, v) =>
            {
                RemoveView(currentView);
            });

            Animation slideView2 = new Animation(v =>
            {
                newView.TranslationX = v;
            }, moveDist, 0);
            slideView2.Commit(this, "slideView2", 4, 400, Easing.SinInOut);
        }

        private void OpenLeftView(View currentView, View newView)
        {
            AlarmEditingViews.RaiseChild(currentView);

            double moveDist = Application.Current.MainPage.Width;

            Animation slideView1 = new Animation(v =>
            {
                newView.TranslationX = v;
            }, -moveDist / 2, 0);
            slideView1.Commit(this, "slideView1", 4, 400, Easing.SinInOut);

            Animation slideView2 = new Animation(v =>
            {
                currentView.TranslationX = v;
            }, 0, moveDist);
            slideView2.Commit(this, "slideView2", 4, 400, Easing.SinInOut, (c, v) =>
            {
                RemoveView(currentView);
            });
        }

        private void CreateRepeatView()
        {
            repeatView = new EditRepeatView() { };
            repeatView.BackPressed += RepeatView_BackPressed;

            AlarmEditingViews.Children.Add(repeatView);
        }

        private void RepeatView_BackPressed()
        {
            if (viewBeingOpened)
                return;
            viewBeingOpened = true;

            CreateInfoView();
            OpenLeftView(repeatView, infoView);

            viewBeingOpened = false;
        }

        private void EditInfoView_OpenQR()
        {

        }

        private void EditInfoView_OpenSound()
        {

        }

        private void EditInfoView_OpenName()
        {

        }
    }
}