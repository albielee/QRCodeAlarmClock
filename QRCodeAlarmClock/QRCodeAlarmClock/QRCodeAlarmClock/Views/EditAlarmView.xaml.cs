using QRCodeAlarmClock.ViewModels;
using QRCodeAlarmClock.Views.AlarmPropertyViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        EditSoundView soundView;
        EditNameView nameView;
        EditQRView qrView;
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
        }

        public void OpenAlarmInfo()
        {
            CreateInfoView();

            Open();
        }

        private void CreateInfoView()
        {
            infoView = new EditInfoView() { BindingContext = mainGrid.BindingContext };
            infoView.OpenQR += EditInfoView_OpenQR;
            infoView.OpenName += EditInfoView_OpenName;
            infoView.OpenRepeat += EditInfoView_OpenRepeat;
            infoView.OpenSound += EditInfoView_OpenSound;
            infoView.Closed += Close;
            infoView.Deleted += Deleted;

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

        public void Deleted()
        {
            if (isOpen)
            {
                ((MainPageVM)mainGrid.BindingContext).DeleteAlarm();

                StartedClosing.Invoke();

                Animation animFlyOut = new Animation(v => editorViewFrame.TranslationY = v, topOffset, Application.Current.MainPage.Height);
                animFlyOut.Commit(this, "FlyOut", 4, 500, Easing.CubicInOut, (v, c) => FlyoutEnded());
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

            CreateRepeatView();
            OpenRightView(infoView, repeatView);

            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }
        }

        private void EditInfoView_OpenSound()
        {
            if (viewBeingOpened)
                return;

            CreateSoundView();
            OpenRightView(infoView, soundView);

            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }
        }

        private void EditInfoView_OpenName()
        {
            if (viewBeingOpened)
                return;

            CreateNameView();
            OpenRightView(infoView, nameView);

            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }
        }

        private void EditInfoView_OpenQR()
        {
            if (viewBeingOpened)
                return;

            CreateQRView();
            OpenRightView(infoView, qrView);

            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }
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
            viewBeingOpened = true;

            double moveDist = Application.Current.MainPage.Width;
            Animation slideView1 = new Animation(v =>
            {
                currentView.TranslationX = v;
            }, 0, -moveDist/2);
            slideView1.Commit(this, "slideView1", 4, 400, Easing.SinInOut, (c, v) =>
            {
                RemoveView(currentView);
                viewBeingOpened = false;
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

            viewBeingOpened = true;

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
                viewBeingOpened = false;
            });
        }

        private void CreateRepeatView()
        {
            repeatView = new EditRepeatView() { BindingContext = this.BindingContext };
            repeatView.BackPressed += RepeatView_BackPressed;

            AlarmEditingViews.Children.Add(repeatView);
        }

        private void CreateSoundView()
        {
            soundView = new EditSoundView() { BindingContext = this.BindingContext };
            soundView.BackPressed += SoundView_BackPressed;

            AlarmEditingViews.Children.Add(soundView);
        }

        private void CreateNameView()
        {
            nameView = new EditNameView() { BindingContext = this.BindingContext };
            nameView.BackPressed += NameView_BackPressed;

            AlarmEditingViews.Children.Add(nameView);
        }

        private void CreateQRView()
        {
            qrView = new EditQRView() { BindingContext = this.BindingContext };
            qrView.BackPressed += QRView_BackPressed;

            AlarmEditingViews.Children.Add(qrView);
        }

        private void RepeatView_BackPressed()
        {
            if (viewBeingOpened)
                return;

            CreateInfoView();
            OpenLeftView(repeatView, infoView);
        }

        private void NameView_BackPressed()
        {
            if (viewBeingOpened)
                return;

            CreateInfoView();
            OpenLeftView(nameView, infoView);
        }

        private void SoundView_BackPressed()
        {
            if (viewBeingOpened)
                return;

            CreateInfoView();
            OpenLeftView(soundView, infoView);
        }

        private void QRView_BackPressed()
        {
            if (viewBeingOpened)
                return;

            CreateInfoView();
            OpenLeftView(qrView, infoView);
        }
    }
}