using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Calendar.Misc;
using Synergy.StandardApps.EntityForms.Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Synergy.StandardApps.Calendar.UserControls
{
    /// <summary>
    /// Логика взаимодействия для CalendarDay.xaml
    /// </summary>
    public partial class CalendarDay : 
        UserControl,
        INotifyPropertyChanged,
        IRecipient<CalendarEventCreatedMessage>,
        IRecipient<CalendarEventUpdatedMessage>,
        IRecipient<CalendarEventDeletedMessage>
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region Animations

        private readonly DoubleAnimation _appearing = new()
        {
            Duration = TimeSpan.FromSeconds(0.5),
            DecelerationRatio = 0.8,
            From = 0,
            To = 1,
            BeginTime = TimeSpan.FromSeconds(0.5)
        };

        private readonly DoubleAnimation _disappearing = new()
        {
            Duration = TimeSpan.FromSeconds(0.5),
            DecelerationRatio = 0.8,
            From = 1,
            To = 0,
        };

        #endregion

        private Action? _visualUpdater;

        private bool _outOfMonth;
        private int _month;

        private int day;
        public int Day
        {
            get
            {
                return day;
            }
            set
            {
                day = value;
                OnPropertyChanged(nameof(Day));
            }
        }

        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private string? eventName;
        public string EventName
        {
            get
            {
                return eventName;
            }
            set
            {
                eventName = value;
                OnPropertyChanged(nameof(EventName));
            }
        }

        private Color seasonColor;
        public Color SeasonColor
        {
            get
            {
                return seasonColor;
            }
            set
            {
                seasonColor = value;
                OnPropertyChanged(nameof(SeasonColor));
            }
        }

        public CalendarDay(int day, int month, bool outOfMonth = false)
        {
            InitializeComponent();

            _disappearing.Completed += UpdateVisualHandler;

            _outOfMonth = outOfMonth;

            InitAppearance(day, month);
        }

        public CalendarDay(CalendarEventForm form)
        {
            InitializeComponent();

            InitAppearance(form);
        }

#if DEBUG

        public CalendarDay()
        {
            InitializeComponent();

            SetSeasonColor(1);

            Day = 31;
            Color = Colors.DarkRed;
            EventName = "New Year";
        }

#endif

        #region Messages

        void IRecipient<CalendarEventCreatedMessage>.Receive(CalendarEventCreatedMessage message)
        {
            if (_outOfMonth || message.Value.Day != Day) return;

            UpdateVisual(message.Value);
        }

        void IRecipient<CalendarEventUpdatedMessage>.Receive(CalendarEventUpdatedMessage message)
        {
            if (_outOfMonth || message.Value.Day != Day) return;

            UpdateVisual(message.Value);
        }

        void IRecipient<CalendarEventDeletedMessage>.Receive(CalendarEventDeletedMessage message)
        {
            if (_outOfMonth || message.Value != Day) return;

            UpdateVisual(Day, _month);
        }

        #endregion

        #region Methods

        #region Update visual

        private void UpdateVisual(CalendarEventForm form)
        {
            _visualUpdater = () =>
            {
                InitAppearance(form);
            };

            BeginAnimation(Control.OpacityProperty, _disappearing);
        }

        private void UpdateVisual(int day, int month)
        {
            _visualUpdater = () =>
            {
                InitAppearance(day, month);
            };

            BeginAnimation(Control.OpacityProperty, _disappearing);
        }

        private void UpdateVisualHandler(object? sender, EventArgs e)
        {
            _visualUpdater?.Invoke();

            BeginAnimation(Control.OpacityProperty, _appearing);
        }

        #endregion

        private void InitAppearance(int day, int month)
        {
            Day = day;
            _month = month;
            EventName = "";

            if (!_outOfMonth)
            {
                SetSeasonColor(_month);
                Color = Colors.OrangeRed;
            }
            else
            {
                SetSeasonColor(13);
                Color = Colors.Brown;
                IsEnabled = false;
            }
        }

        private void InitAppearance(CalendarEventForm form)
        {
            SetSeasonColor(form.Month);

            Day = form.Day;
            _month = form.Month;
            EventName = form.Title;
            Color = form.GetColor();
        }

        private void SetSeasonColor(int month)
        {
            SeasonColor = Misc.SeasonColor.Get(month);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Event handlers

        private void CalendarDayCard_Loaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default
                .RegisterAll(this);
        }

        private void CalendarDayCard_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default
                .UnregisterAll(this);
        }

        #endregion
    }
}
