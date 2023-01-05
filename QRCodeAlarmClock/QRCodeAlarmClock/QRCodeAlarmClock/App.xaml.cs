using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRCodeAlarmClock
{
    public partial class App : Application
    {
        MainPage mainPage;
        public App()
        {
            InitializeComponent();

            mainPage = new MainPage();
            MainPage = mainPage;
        }

        public static void Test()
        {
           
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume() //CALL THIS WHEN AN ALARM RINGS OR IS OPENED!!!! ???
        {
            mainPage.CheckForRingingAlarm();
        }
    }
}
