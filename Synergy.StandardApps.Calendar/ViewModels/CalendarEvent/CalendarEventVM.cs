using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Calendar.Misc;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Synergy.StandardApps.Calendar.ViewModels.CalendarEvent
{
    public class CalendarEventVM :
        ViewModel,
        IRecipient<OpenCalendarEventMessage>
    {
        #region Properties

        private int _day;
        public int Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }

        private string _month;
        public string Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        private int _monthNum;
        public int MonthNum
        {
            get => _monthNum;
            private set => SetProperty(ref _monthNum, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private Color _color;
        public Color Color
        {
            get => _color; 
            set => SetProperty(ref _color, value);
        }

        private Color _seasonColor;
        public Color SeasonColor
        {
            get => _seasonColor;
            set => SetProperty(ref _seasonColor, value);
        }

        #endregion

        public CalendarEventVM()
        {
            _day = 0;
            MonthNum = 0;
            _month = string.Empty;
            _title = string.Empty;
            _description = string.Empty;
            _color = Colors.White;
            _seasonColor = Misc.SeasonColor.Get(MonthNum);
        }

        #region Messages

        void IRecipient<OpenCalendarEventMessage>.Receive(OpenCalendarEventMessage message)
        {
            if(message.Value is null)
            {
                _day = 0;
                MonthNum = 0;
                _month = string.Empty;
                Title = string.Empty;
                Description = string.Empty;
                Color = Colors.White;
            }
            else
            {
                Day = message.Value.Day;
                SetMonth(message.Value.Month);
                Title = message.Value.Title;
                Description = message.Value.Description;
                Color = message.Value.GetColor();
            }

            SeasonColor = Misc.SeasonColor.Get(MonthNum);
        }

        #endregion

        #region Commands

        private RelayCommand? viewLoaded;
        public ICommand ViewLoaded => viewLoaded ??
            (viewLoaded = new RelayCommand(() =>
            {
                IsActive = true;
            }));

        private RelayCommand? viewUnloaded;
        public ICommand ViewUnloaded => viewUnloaded ??
            (viewUnloaded = new RelayCommand(() =>
            {
                IsActive = false;
            }));

        #endregion

        #region Methods

        private void SetMonth(int month)
        {
            var dt = new DateTime(2000, month, 1);
            Month = dt.ToString("MMMM");
            MonthNum = month;
        }

        #endregion
    }
}
