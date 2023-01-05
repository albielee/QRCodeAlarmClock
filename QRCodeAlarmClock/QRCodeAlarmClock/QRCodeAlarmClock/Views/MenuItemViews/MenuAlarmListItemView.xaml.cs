using QRCodeAlarmClock.Model;
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
    public partial class MenuAlarmListItemView : Grid
    {
        public Alarm Alarm = null;

        MenuNavAlarmListButtonView selectButton;

        public event SwitchHandler SwitchToggled;
        public delegate void SwitchHandler(Alarm alarm, bool toggled);

        public event SelectButtonHandler SelectButtonClicked;
        public delegate void SelectButtonHandler(Alarm alarm);

        public MenuAlarmListItemView(Alarm a, int alarmCount)
        {
            InitializeComponent();

            Alarm = a;

            Children.Add(CreateAlarmInfo(Alarm, alarmCount));
            Children.Add(CreateSelectButton(Alarm));
            Children.Add(CreateAlarmSwitch(Alarm));
        }

        public void Dispose()
        {
            selectButton.ButtonPressed -= SelectButton_Pressed;
            SwitchToggled = null;
            SelectButtonClicked = null;
        }

        private MenuNavAlarmListButtonView CreateSelectButton(Alarm alarm)
        {
            selectButton = new MenuNavAlarmListButtonView()
            {
                AssignedAlarm = alarm,
            };

            selectButton.Margin = new Thickness(-20, 0);
            selectButton.ButtonPressed += SelectButton_Pressed;
            selectButton.Unfocused += ShakeAnimation;

            return selectButton;
        }

        private void ShakeAnimation()
        {
            int translationDist = 16;

            Animation shakeAnimStart = new Animation(v => TranslationX = v, 0, translationDist);
            Animation shakeAnimLeft = new Animation(v => TranslationX = v, translationDist, -translationDist);
            Animation shakeAnimRight = new Animation(v => TranslationX = v, -translationDist, translationDist);
            Animation shakeAnimEnd = new Animation(v => TranslationX = v, TranslationX, 0);
            shakeAnimStart.Commit(this, "shakeAnimStart", 4, 100, Easing.CubicInOut, (a, b) =>
            {
                translationDist -= 2;

                shakeAnimLeft.Commit(this, "shakeAnimLeft", 4, 100, Easing.CubicInOut, (c, d) =>
                {
                    translationDist -= 2;

                    shakeAnimRight.Commit(this, "shakeAnimRight", 4, 100, Easing.CubicInOut, (e, f) =>
                    {
                        translationDist -= 2;

                        shakeAnimLeft.Commit(this, "shakeAnimLeft", 4, 100, Easing.CubicInOut, (g, h) =>
                        {
                            translationDist -= 2;

                            shakeAnimRight.Commit(this, "shakeAnimRight", 4, 100, Easing.CubicInOut, (i, j) =>
                            {
                                shakeAnimEnd.Commit(this, "shakeAnimEnd", 4, 100, Easing.BounceIn);
                            });
                        });
                    });
                });
            });
        }

        private void SelectButton_Pressed(Alarm alarm)
        {
            SelectButtonClicked?.Invoke(alarm);
        }

        private Switch CreateAlarmSwitch(Alarm alarm)
        {
            Switch alarmSwitch = new Switch()
            {
                IsToggled = alarm.IsEnabled,
                Margin = new Thickness(0, 0, 20, 10),
                
            };
            alarmSwitch.Toggled += AlarmSwitch_Toggled;

            alarmSwitch.VerticalOptions = LayoutOptions.CenterAndExpand;
            alarmSwitch.HorizontalOptions = LayoutOptions.EndAndExpand;

            return alarmSwitch;
        }

        private void AlarmSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            SwitchToggled?.Invoke(Alarm, ((Switch)sender).IsToggled);
        }

        private StackLayout CreateAlarmInfo(Alarm alarm, int alarmCount)
        {
            StackLayout alarmInfo = new StackLayout()
            {
                Spacing = 0,
                BackgroundColor = (Color)Application.Current.Resources["Background"],

            };

            if (alarmCount != 0)
                alarmInfo.Children.Add(CreateSeperatorLine());
            alarmInfo.Children.Add(CreateLeftSideInfo(alarm));

            return alarmInfo;
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


        private Grid CreateLeftSideInfo(Alarm alarm)
        {
            Grid alarmGridLeft = new Grid()
            {
                InputTransparent = true,
            };

            ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
            columnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            columnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            RowDefinitionCollection rowDefinitions = new RowDefinitionCollection();
            rowDefinitions.Add(new RowDefinition() { Height = 40 });
            rowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Grid timeAndQr = new Grid()
            {
                Margin = new Thickness(20, 10, 20, 0),
                ColumnDefinitions = columnDefinitions,
                RowDefinitions = rowDefinitions,
            };
            Label alarmTime = new Label()
            {
                Text = alarm.TimeString,
                Style = Application.Current.Resources["alarmTitle"] as Style,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(0, 0, 0, 0)
            };
            timeAndQr.Children.Add(alarmTime, 0, 0);
            if (alarm.QRCode != "" && alarm.QRCode != null)
                timeAndQr.Children.Add(CreateQRButton(alarm), 1, 0);

            alarmGridLeft.Children.Add(timeAndQr, 0, 0);

            string repeatDaysString = "";
            if (alarm.RepeatDaysString != "")
            {
                repeatDaysString = ", " + alarm.RepeatDaysString;
            }
            Label alarmNameAndDays = new Label()
            {
                Text = alarm.Name + repeatDaysString,
                Style = Application.Current.Resources["alarmNormal"] as Style,
                Margin = new Thickness(20, 0, 10, 0),
                VerticalOptions = LayoutOptions.Center,
            };
            alarmGridLeft.Children.Add(alarmNameAndDays, 0, 1);

            alarmGridLeft.HorizontalOptions = LayoutOptions.StartAndExpand;

            return alarmGridLeft;
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
    }
}