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
    public partial class MenuToggleButtonView : Grid
    {
        public event ButtonPressedEventHandler ButtonPressed;
        public delegate void ButtonPressedEventHandler();



        public static readonly BindableProperty ToggleProperty =
        BindableProperty.Create(nameof(Toggle), typeof(bool), typeof(MenuToggleButtonView), null, BindingMode.TwoWay);
        public bool Toggle
        {
            get { return (bool)GetValue(ToggleProperty); }
            set { SetValue(ToggleProperty, value); }
        }


        public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(MenuToggleButtonView), null);
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public MenuToggleButtonView()
        {
            InitializeComponent();
        }

        private void MenuButtonView_ButtonPressed()
        {
            AnimateDiamond();

            ButtonPressed?.Invoke();
        }

        private void AnimateDiamond()
        {
            if (Toggle)
            {
                Toggle = false;
                orangeDiamond.FadeTo(0, 250, Easing.SinIn);
                orangeDiamond.RotateTo(0, 250, Easing.SinInOut);
            }
            else
            {
                Toggle = true;
                orangeDiamond.FadeTo(1, 250, Easing.SinIn);
                orangeDiamond.RotateTo(360, 250, Easing.SinInOut);
            }
           
        }
    }
}