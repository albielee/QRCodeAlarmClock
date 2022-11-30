using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QRCodeAlarmClock.ViewModels
{
    public class EditInfoViewVM
    {
        public Command CancelButtonPressed { get; set; }
        public Command SaveButtonPressed { get; set; }
        public EditInfoViewVM()
        {
            CancelButtonPressed = new Command(Cancel);
            SaveButtonPressed = new Command(Save);
        }

        private void Cancel()
        {

        }

        private void Save()
        {

        }
    }
}
