using QRCodeAlarmClock.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace QRCodeAlarmClock.Views
{
    public class SwitchBehaviour : Behavior<Switch>
    {
        public BindableProperty AssignedAlarmProperty = BindableProperty.Create(nameof(AssignedAlarm), typeof(Alarm), typeof(SwitchBehaviour), null);
        public Alarm AssignedAlarm
        {
            get { return (Alarm)GetValue(AssignedAlarmProperty); }
            set
            {
                SetValue(AssignedAlarmProperty, value);
            }
        }

        public BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SwitchBehaviour), null);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        public Switch Bindable { get; private set; }

        protected override void OnAttachedTo(Switch bindable)
        {
            base.OnAttachedTo(bindable);
            Bindable = bindable;
            Bindable.BindingContextChanged += OnBindingContextChanged;
            Bindable.Toggled += OnSwitchToggled;
        }

        protected override void OnDetachingFrom(Switch bindable)
        {
            base.OnDetachingFrom(bindable);
            Bindable.BindingContextChanged -= OnBindingContextChanged;
            Bindable.Toggled -= OnSwitchToggled;
            Bindable = null;
            AssignedAlarm = null;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
            BindingContext = Bindable.BindingContext;
        }

        private void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            Command?.Execute(AssignedAlarm);
        }
    }
}
