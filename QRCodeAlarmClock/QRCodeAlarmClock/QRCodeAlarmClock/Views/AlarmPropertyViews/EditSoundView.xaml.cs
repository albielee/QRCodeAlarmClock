using QRCodeAlarmClock.Views.AlarmPropertyViews;
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
    public partial class EditSoundView : EditTemplateView
    {
        public EditSoundView()
        {
            InitializeComponent();
        }
    }
}