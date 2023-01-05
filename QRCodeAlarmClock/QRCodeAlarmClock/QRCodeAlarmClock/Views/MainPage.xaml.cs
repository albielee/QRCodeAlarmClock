using QRCodeAlarmClock.Interfaces;
using QRCodeAlarmClock.Model;
using QRCodeAlarmClock.ViewModels;
using QRCodeAlarmClock.Views;
using QRCodeAlarmClock.Views.MenuItemViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRCodeAlarmClock
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class MainPage : ContentPage
    {
        const int alarmFadeDist = 80;

        EditAlarmView alarmView;
        AlarmRingingView alarmRingingView;

        double startScrollY;

        MainPageVM vm;

        SemaphoreSlim listGate;


        public static readonly BindableProperty ResumedProperty =
        BindableProperty.Create(nameof(Resumed), typeof(bool), typeof(MainPage), null, propertyChanged: OnEventNameChanged);
        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MainPage view = (MainPage)bindable;
            view.CheckForRingingAlarm();
        }

        public bool Resumed
        {
            get { return (bool)GetValue(ResumedProperty); }
            set { SetValue(ResumedProperty, value); }
        }

        public MainPage()
        {
            InitializeComponent();
            vm = ((MainPageVM)MainView.BindingContext);
            vm.AlarmListChanged += AlarmListChanged;

            listGate = new SemaphoreSlim(1);

            PopulateAlarmList();

            DependencyService.Get<IRingAlarm>().AlarmRang += CheckForRingingAlarm;
        }

        public void CheckForRingingAlarm()
        {
            TimeSpan alarmRingingTime = new TimeSpan(0, 0, 300);
            DateTime currentTime = DateTime.Now;
            //Check for any alarms that should be ringing

            List<Alarm> alarmList = new List<Alarm>();
            foreach (Alarm alarm in vm.AlarmList)
            {
                DayOfWeek day = currentTime.DayOfWeek;

                TimeSpan alarmTime = alarm.Time.TimeOfDay;
                TimeSpan currentDayTime = currentTime.TimeOfDay;
                bool inRange = Math.Abs((alarmTime - currentDayTime).Ticks) < alarmRingingTime.Ticks;

                if (alarm.IsMonday && day == DayOfWeek.Monday)
                {
                    if (inRange)
                    {
                        alarmList.Add(alarm);
                    }
                }
                else if (alarm.IsTuesday && day == DayOfWeek.Tuesday)
                {
                    if (inRange)
                    {
                        alarmList.Add(alarm);
                    }
                }
                else if (alarm.IsWednesday && day == DayOfWeek.Wednesday)
                {
                    if (inRange)
                    {
                        alarmList.Add(alarm);
                    }
                }
                else if (alarm.IsThursday && day == DayOfWeek.Thursday)
                {
                    if (inRange)
                    {
                        alarmList.Add(alarm);
                    }
                }
                else if (alarm.IsFriday && day == DayOfWeek.Friday)
                {
                    if (inRange)
                    {
                        alarmList.Add(alarm);
                    }
                }
                else if (alarm.IsSaturday && day == DayOfWeek.Saturday)
                {
                    if (inRange)
                    {
                        alarmList.Add(alarm);
                    }
                }
                else if (alarm.IsSunday && day == DayOfWeek.Sunday)
                {
                    if (inRange)
                    {
                        alarmList.Add(alarm);
                    }
                }
                else
                {
                    if (alarm.IsEnabled)
                    {
                        if (inRange)
                        {
                            alarmList.Add(alarm);
                        }
                    }
                }
            }

            if(alarmList.Count > 0)
                CreateAlarmRingingView(alarmList);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            CheckForRingingAlarm();
        }

        private void CreateAlarmRingingView(List<Alarm> alarmList)
        {
            if(alarmRingingView == null)
            {
                alarmRingingView = new AlarmRingingView(alarmList);
                MainView.Children.Add(alarmRingingView);
                alarmRingingView.Tapped += AlarmRingingView_Tapped;
            }
            else
            {
                alarmRingingView.AddAlarms(alarmList);
            }
        }

        private void AlarmRingingView_Tapped(List<Alarm> alarmList)
        {


            alarmRingingView.Tapped -= AlarmRingingView_Tapped;
            MainView.Children.Remove(alarmRingingView);
            alarmRingingView = null;
        }


        /// <summary>
        /// Update alarm list to update any changes.
        /// To prevent O(n^2) the alarm list and AlarmList.Children are always in the same order.
        /// </summary>
        private void AlarmListChanged()
        {
            listGate.Wait();

            List<MenuAlarmListItemView> alarmsToDelete = new List<MenuAlarmListItemView>();
            List<(Alarm, MenuAlarmListItemView)> alarmsToChange = new List<(Alarm, MenuAlarmListItemView)>();
            foreach(MenuAlarmListItemView alarmItem in AlarmList.Children)
            {
                int itemId = alarmItem.Alarm.ID;
                Alarm viewModelAlarm = vm.AlarmList.FirstOrDefault(a => a.ID == itemId);

                //If alarm is null, delete
                if(viewModelAlarm == null)
                {
                    alarmsToDelete.Add(alarmItem);
                }
                else if (viewModelAlarm.IsChanged)
                {
                    alarmsToChange.Add((viewModelAlarm, alarmItem));
                    viewModelAlarm.IsChanged = false;
                }
            }

            //Delete alarms
            foreach(var alarmView in alarmsToDelete)
            {
                RemoveSingleAlarm(alarmView, true);
            }

            //Change alarms
            foreach((Alarm alarm,MenuAlarmListItemView view) tuple in alarmsToChange)
            {
                int viewStackPos = AlarmList.Children.IndexOf(tuple.view);
                RemoveSingleAlarm(tuple.view, false);
                AddSingleAlarm(tuple.alarm, viewStackPos);
            }

            //Add any new alarms
            int alarmCount = 0;
            foreach (Alarm alarm in vm.AlarmList)
            {
                //Try get the alarm view item
                View alarmItemView = null;
                try
                {
                    alarmItemView = AlarmList.Children[alarmCount];
                }
                catch { }

                if(alarmItemView == null) //Add the new alarm
                {
                    AddSingleAlarm(alarm);
                }

                alarmCount++;
            }

            listGate.Release();
        }

        private async void RemoveSingleAlarm(View alarmItem, bool animate)
        {
            ((MenuAlarmListItemView)alarmItem).Dispose();
            ((MenuAlarmListItemView)alarmItem).SelectButtonClicked -= AlarmItem_SelectButtonClicked;
            ((MenuAlarmListItemView)alarmItem).SwitchToggled -= AlarmItem_SwitchToggled;

            if (animate)
            {
                double startHeight = alarmItem.Height;

                Animation slideAlarmItemAway = new Animation(v => alarmItem.HeightRequest = v, startHeight, 0);
                slideAlarmItemAway.Commit(this, "slideAlarmItemAway", 4, 500, Easing.CubicInOut, (x, y) =>
                {
                    AlarmList.Children.Remove(alarmItem);
                });
                await Task.Delay(501);
            }
            else
            {
                AlarmList.Children.Remove(alarmItem);
            }
        }

        /// <summary>
        /// Adds a single alarm to the bottom of the view stack. Animates the action.
        /// </summary>
        /// <param name="alarm"></param>
        private void AddSingleAlarm(Alarm alarm)
        {
            Grid alarmItem = CreateAlarmView(alarm);
            AlarmList.Children.Add(alarmItem);

            double screenHeight = Application.Current.MainPage.Height;

            Animation slideAlarmItemUp = new Animation(v => alarmItem.TranslationY = v, screenHeight, 0);
            slideAlarmItemUp.Commit(this, "slideAlarmItemUp", 4, 250, Easing.CubicInOut);
        }

        /// <summary>
        /// Add a single alarm to the view stack at a position. Does not animate.
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="position"></param>
        private void AddSingleAlarm(Alarm alarm, int position)
        {
            Grid alarmItem = CreateAlarmView(alarm);
            AlarmList.Children.Insert(position, alarmItem);
        }

        private void PopulateAlarmList()
        {
            int alarmCount = 0;
            foreach(Alarm alarm in vm.AlarmList)
            {
                AlarmList.Children.Add(CreateAlarmView(alarm));
                alarmCount++;
            }
        }

        private MenuAlarmListItemView CreateAlarmView(Alarm alarm)
        {
            MenuAlarmListItemView alarmItem = new MenuAlarmListItemView(alarm, AlarmList.Children.Count);

            alarmItem.SelectButtonClicked += AlarmItem_SelectButtonClicked;
            alarmItem.SwitchToggled += AlarmItem_SwitchToggled;

            return alarmItem;
        }

        private void AlarmItem_SwitchToggled(Alarm alarm, bool toggled)
        {
            vm.UpdateAlarmToggle(alarm, toggled);
        }

        private void AlarmItem_SelectButtonClicked(Alarm alarm)
        {
            vm.AlarmItemSelected(alarm);

            OpenAlarm();
        }

        private void ShrinkAlarmListFrame()
        {
            int horizontalThickness = 20;

            Animation shrinkAlarmListFrameH = new Animation(v => {

                double verticalMargin = AlarmListFrame.Margin.VerticalThickness;
                AlarmListFrame.Margin = new Thickness(v, v * 2);

            }, 0, horizontalThickness);
            shrinkAlarmListFrameH.Commit(this, "shrinkAlarmListFrameH", 4, 400, Easing.CubicInOut);
        }

        private void AlarmViewStartedClosing()
        {
            FlashMainView();
            GrowAlarmListFrame();
        }

        private void FlashMainView()
        {
            ForegroundFlashFrame.Margin = new Thickness(100);
            ForegroundFlashFrame.CornerRadius = 40;
            ForegroundFlashFrame.Opacity = 0.1;

            Animation flashGrow = new Animation(v => ForegroundFlashFrame.Margin = new Thickness(v), 100, 0);
            flashGrow.Commit(this, "flashGrow", 4, 500, Easing.SinOut);

            Animation flashDown = new Animation(v => ForegroundFlashFrame.Opacity = v, 0.6, 0);
            flashDown.Commit(this, "flashDown", 4, 500, Easing.SinOut);

            Animation flashCorners = new Animation(v => ForegroundFlashFrame.CornerRadius = (float)v, 40, 0);
            flashDown.Commit(this, "flashCorners", 4, 500, Easing.SinOut, (x,y) =>
            {
                ForegroundFlashFrame.BackgroundColor = Color.Transparent;
            });

            ForegroundFlashFrame.BackgroundColor = Color.White;
        }

        private void GrowAlarmListFrame()
        {
            int horizontalThickness = 20;
            int verticalThickness = 40;

            Animation growAlarmListFrameH = new Animation(v => {

                AlarmListFrame.Margin = new Thickness(v, verticalThickness - (horizontalThickness-v)*2);

            }, horizontalThickness, 0);
            growAlarmListFrameH.Commit(this, "growAlarmListFrameH", 4, 400, Easing.CubicInOut);
        }

        private void AlarmViewClosed()
        {
            if (alarmView != null)
            {
                MainView.Children.Remove(alarmView);
                alarmView = null;
            }
        }
        
        private void ResetSelectButtons()
        {
            foreach(var child in AlarmList.Children)
            {
                if(child.GetType() == typeof(Button))
                {
                    child.Opacity = 0.1;
                }
            }
        }

        private void AddAlarm_Pressed(object sender, EventArgs e)
        {
            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }

            OpenAlarm();
        }

        private void OpenAlarm()
        {
            if (alarmView == null)
            {
                alarmView = new EditAlarmView() { BindingContext = MainView.BindingContext };
                alarmView.Closed += AlarmViewClosed;
                alarmView.StartedClosing += AlarmViewStartedClosing;
                MainView.Children.Add(alarmView);

                alarmView.OpenAlarmInfo();

                ShrinkAlarmListFrame();
            }
        }

        private SwipeItems CreateSwipeItems()
        {
            SwipeItems SwipeItems = new SwipeItems();
            SwipeItem SwipeItem = new SwipeItem();
            SwipeItem.BackgroundColor = (Color)Application.Current.Resources["Red"];
            SwipeItem.Text = "Remove";

            SwipeItems.Add(SwipeItem);

            return SwipeItems;
        }

        private void scrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (startScrollY == 0)
                startScrollY = e.ScrollY;

            var adjustedScrollY = e.ScrollY - startScrollY;

            TitleFrame.Opacity = (adjustedScrollY < alarmFadeDist) ? 1 - (adjustedScrollY / alarmFadeDist) : 0;

            Title.ScaleY = (adjustedScrollY < 0) ? (1 - adjustedScrollY/200) : 1;
        }
    }
}
