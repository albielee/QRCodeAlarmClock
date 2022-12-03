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
    public partial class EditNameView : EditTemplateView
    {

        public EditNameView()
        {
            InitializeComponent();

            Animation slideDownEntry = new Animation(v => entryFrame.TranslationY = v, -entryFrame.Margin.Top, 0);
            slideDownEntry.Commit(this, "slideDownEntry", 4, 500, Easing.CubicInOut, (x, y) => entry.Focus());
        }

        private void ClearButton_Pressed(object sender, EventArgs e)
        {
            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }
            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }

            clearButton.Opacity = 0;
            clearButton.IsEnabled = false;

            Animation lightSplashAnim = new Animation(v => lightSplash.Opacity = v, 0.5, 0);
            lightSplashAnim.Commit(this, "lightSplashAnim", 4, 500, Easing.CubicInOut, (x, y) => entry.Focus());
        }

        private void entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(e.NewTextValue != "" && e.NewTextValue != null)
            {
                clearButton.Opacity = 1;
                clearButton.IsEnabled = true;
            }
        }
    }
}