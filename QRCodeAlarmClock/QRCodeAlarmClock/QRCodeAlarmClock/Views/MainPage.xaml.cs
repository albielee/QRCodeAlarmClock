using QRCodeAlarmClock.Model;
using QRCodeAlarmClock.ViewModels;
using QRCodeAlarmClock.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QRCodeAlarmClock
{
    public partial class MainPage : ContentPage
    {
        const int alarmFadeDist = 80;

        EditAlarmView alarmView;
        double startScrollY;

        public MainPage()
        {
            InitializeComponent();

            PopulateAlarmList();
        }

        private void PopulateAlarmList()
        {
            MainPageVM vm = ((MainPageVM)AlarmListView.BindingContext);
            int alarmCount = 0;
            foreach(Alarm alarm in vm.AlarmList)
            {
                AlarmList.Children.Add(CreateAlarmView(alarm, alarmCount));
                alarmCount++;
            }
        }

        private Grid CreateAlarmView(Alarm alarm, int alarmCount)
        {
            MainPageVM vm = ((MainPageVM)AlarmListView.BindingContext);

            Grid alarmItem = new Grid();
            alarmItem.Children.Add(CreateAlarmInfo(alarm, alarmCount));
            alarmItem.Children.Add(CreateSelectButton(vm, alarm));
            alarmItem.Children.Add(CreateAlarmSwitch(alarm));

            return alarmItem;
        }

        private Button CreateQRButton(Alarm alarm)
        {
            Button button = new Button()
            {
                ImageSource = "QrCodeIcon.png",
                WidthRequest = 25,
                HeightRequest = 25,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0),
            };
            return button;
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            if(alarmView == null)
            {
                alarmView = new EditAlarmView();
                alarmView.Closed += AlarmViewClosed;
                alarmView.StartedClosing += AlarmViewStartedClosing;
                MainView.Children.Add(alarmView);

                ShrinkAlarmListFrame();
            }
        }

        private void ShrinkAlarmListFrame()
        {
            int horizontalThickness = 20;
            int verticalThickness = 40;

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
            ForegroundFlashFrame.Opacity = 0;

            Animation flashGrow = new Animation(v => ForegroundFlashFrame.Margin = new Thickness(v), 100, 0);
            flashGrow.Commit(this, "flashGrow", 4, 500, Easing.SinOut);

            Animation flashDown = new Animation(v => ForegroundFlashFrame.Opacity = v, 0.6, 0);
            flashDown.Commit(this, "flashDown", 4, 500, Easing.SinOut);

            Animation flashCorners = new Animation(v => ForegroundFlashFrame.CornerRadius = (float)v, 40, 0);
            flashDown.Commit(this, "flashCorners", 4, 500, Easing.SinOut);
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
            if(alarmView != null)
            {
                MainView.Children.Remove(alarmView);
                alarmView = null;
            }       
        }

        private Switch CreateAlarmSwitch(Alarm alarm)
        {
            Switch alarmSwitch = new Switch()
            {
                IsToggled = alarm.IsEnabled,
                Margin = new Thickness(20),
            };
            alarmSwitch.VerticalOptions = LayoutOptions.CenterAndExpand;
            alarmSwitch.HorizontalOptions = LayoutOptions.EndAndExpand;
            
            return alarmSwitch;
        }

        private Grid CreateLeftSideInfo(Alarm alarm)
        {
            Grid alarmGridLeft = new Grid()
            {
                InputTransparent = true,
            };

            ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
            columnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            columnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            Grid timeAndQr = new Grid()
            {
                Margin = new Thickness(20, 20, 20, 0),
                ColumnDefinitions = columnDefinitions,
            };
            Label alarmTime = new Label()
            {
                Text = alarm.TimeString,
                Style = Application.Current.Resources["title"] as Style,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            timeAndQr.Children.Add(alarmTime, 0, 0);
            if (alarm.QRCode != "" || alarm.QRCode == null)
                timeAndQr.Children.Add(CreateQRButton(alarm), 1, 0);

            alarmGridLeft.Children.Add(timeAndQr, 0, 0);

            Label alarmName = new Label()
            {
                Text = alarm.Name,
                Style = Application.Current.Resources["alarmNormal"] as Style,
                Margin = new Thickness(20, 0),
                VerticalOptions = LayoutOptions.Center,
            };
            alarmGridLeft.Children.Add(alarmName, 0, 1);
            alarmGridLeft.HorizontalOptions = LayoutOptions.StartAndExpand;

            return alarmGridLeft;
        }

        private StackLayout CreateAlarmInfo(Alarm alarm, int alarmCount)
        {
            StackLayout alarmInfo = new StackLayout()
            {
                Spacing = 0,
                BackgroundColor = (Color)Application.Current.Resources["AlarmItemBackground"]
            };

            if(alarmCount != 0)
                alarmInfo.Children.Add(CreateSeperatorLine());
            alarmInfo.Children.Add(CreateLeftSideInfo(alarm));

            return alarmInfo;
        }

        private Button CreateSelectButton(MainPageVM vm, Alarm alarm)
        {
            Button selectButton = new Button()
            {
                BackgroundColor = (Color)Application.Current.Resources["Transparent"],
                Command = vm.AlarmItemSelectedCommand,
                CommandParameter = alarm,
            };
            selectButton.Released += Button_Pressed;

            return selectButton;
        }

        private BoxView CreateSeperatorLine()
        {
            BoxView line = new BoxView()
            {
                HeightRequest = 1,
                BackgroundColor = (Color)Application.Current.Resources["Grey"],
                Margin = new Thickness(0),
            };

            return line;
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
