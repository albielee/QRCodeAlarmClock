using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRCodeAlarmClock.Views.AlarmPropertyViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTemplateView : ContentView
    {
        public event BackPressedEventHandler BackPressed;
        public delegate void BackPressedEventHandler();


        public static readonly BindableProperty BackPressedProperty =
        BindableProperty.Create(nameof(TriggerBackButton), typeof(bool), typeof(EditTemplateView), null, propertyChanged: OnEventNameChanged);
        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            EditTemplateView view = (EditTemplateView)bindable;
            view.BackPressed?.Invoke();
        }

        public bool TriggerBackButton
        {
            get { return (bool)GetValue(BackPressedProperty); }
            set { SetValue(BackPressedProperty, value); }
        }

        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(EditTemplateView), null);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public EditTemplateView()
        {
            InitializeComponent();
        }

        private void Back_Pressed(object sender, EventArgs e)
        {
            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }

            BackPressed.Invoke();
        }
    }
}