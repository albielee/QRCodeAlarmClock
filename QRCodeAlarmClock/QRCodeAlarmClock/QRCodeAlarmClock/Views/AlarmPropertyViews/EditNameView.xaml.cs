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
    public partial class EditNameView : ContentView
    {
        public event BackPressedEventHandler BackPressed;
        public delegate void BackPressedEventHandler();

        public EditNameView()
        {
            InitializeComponent();
        }

        private void Back_Pressed(object sender, EventArgs e)
        {
            BackPressed.Invoke();
        }
    }
}