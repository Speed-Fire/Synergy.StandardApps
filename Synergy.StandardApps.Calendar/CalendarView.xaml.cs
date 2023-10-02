using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Calendar.UserControls;
using Synergy.StandardApps.EntityForms.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Synergy.StandardApps.Calendar
{
    /// <summary>
    /// Логика взаимодействия для CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : 
        UserControl,
        IRecipient<MonthLoadedMessage>,
        IRecipient<SetRightSidePanelVisibilityMessage>
    {
        private readonly List<CalendarDay> _cards;
        private readonly List<DependencyObject> _hitResultList = new();

        #region Animations

        private readonly DoubleAnimation _calendarDisappearing;
        private readonly DoubleAnimation _calendarAppearing;

        #region Frame opened animations

        private readonly DoubleAnimation _frameDisappearing;
        private readonly DoubleAnimation _frameAppearing;

        private const double _surfaceMaxOpacity = 0.8;

        private readonly DoubleAnimation _surfaceTopDisappearing;
        private readonly DoubleAnimation _surfaceTopAppearing;

        private readonly DoubleAnimation _surfaceBottomDisappearing;
        private readonly DoubleAnimation _surfaceBottomAppearing;

        #endregion

        #endregion

        #region Constants

        private const double _cardHeight = 112.5;
        private const double _cardWidth = 75;

        public Thickness ItemMargin { get; } = new Thickness(15, 10, 15, 10);

        #endregion

        #region Properties

        private volatile bool _isRightSidePanelVisible;
        public bool IsRightSidePanelVisible
        {
            get => _isRightSidePanelVisible;

            set
            {
                _isRightSidePanelVisible = value;

                if (!value)
                {
                    HideFrame();
                }
                else
                {
                    ShowFrame();
                }
            }
        }

        private bool _isRegistered;
        private bool IsRegistered
        {
            get => _isRegistered;
            set
            {
                if (_isRegistered == value)
                    return;

                _isRegistered = value;

                if (value)
                {
                    WeakReferenceMessenger.Default
                        .RegisterAll(this);
                }
                else
                {
                    WeakReferenceMessenger.Default
                        .UnregisterAll(this);
                }
            }
        }

        #endregion

        public CalendarPage()
        {
            InitializeComponent();
            _cards = new();

            FrameBrd.Visibility = Visibility.Hidden;

            #region Animations

            _calendarDisappearing = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            _calendarAppearing = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.7)
            };

            _frameDisappearing = new DoubleAnimation()
            {
                DecelerationRatio = 1,
                Duration = TimeSpan.FromSeconds(1.5)
            };

            _frameAppearing = new DoubleAnimation()
            {
                DecelerationRatio = 1,
                Duration = TimeSpan.FromSeconds(1.5)
            };

            _surfaceTopAppearing = new DoubleAnimation()
            {
                From = 0,
                To = _surfaceMaxOpacity,
                Duration = TimeSpan.FromSeconds(1),
                DecelerationRatio = 1
            };

            _surfaceTopDisappearing = new DoubleAnimation()
            {
                From = _surfaceMaxOpacity,
                To = 0,
                Duration = TimeSpan.FromSeconds(1),
                DecelerationRatio = 1
            };

            _surfaceBottomAppearing = new DoubleAnimation()
            {
                From = 0,
                To = _surfaceMaxOpacity,
                Duration = TimeSpan.FromSeconds(1),
                DecelerationRatio = 1
            };

            _surfaceBottomDisappearing = new DoubleAnimation()
            {
                From = _surfaceMaxOpacity,
                To = 0,
                Duration = TimeSpan.FromSeconds(1),
                DecelerationRatio = 1
            };

            _frameDisappearing.Completed += (sender, e) => 
            {
                FrameBrd.Visibility = Visibility.Hidden;

                WeakReferenceMessenger.Default
                    .Send(new RightSidePanelClosedMessage(null));
            };

            _surfaceTopDisappearing.Completed += (sender, e) => { SurfaceBrd_Top.Visibility = Visibility.Collapsed; };
            _surfaceBottomDisappearing.Completed += (sender, e) => { SurfaceBrd_Bottom.Visibility = Visibility.Collapsed; };

            #endregion
        }

        #region Page loading handlers

        private void CalendarMain_Loaded(object sender, RoutedEventArgs e)
        {
            AddMouseHandler();

            IsRegistered = true;

            HideFrame();
        }

        private void CalendarMain_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveMouseHandler();

            IsRegistered = false;
        }

        #endregion

        #region Methods

        private void AddMouseHandler()
        {
            AddHandler(Mouse.PreviewMouseDownEvent,
                new MouseButtonEventHandler(HandleClickOutsideOfControl), true);
        }

        private void RemoveMouseHandler()
        {
            RemoveHandler(Mouse.PreviewMouseDownEvent,
                new MouseButtonEventHandler(HandleClickOutsideOfControl));
        }

        #endregion

        #region Messages

        void IRecipient<MonthLoadedMessage>.Receive(MonthLoadedMessage message)
        {
            LoadMonth(message.Value.Item1, message.Value.Item2);
        }

        void IRecipient<SetRightSidePanelVisibilityMessage>.Receive(SetRightSidePanelVisibilityMessage message)
        {
            IsRightSidePanelVisible = message.Value;
        }

        #endregion

        #region Month loading

        private void LoadMonth(IEnumerable<CalendarEventForm> events, DateTime currentDate)
        {
            // remove all cards from grid
            foreach (CalendarDay day in _cards)
            {
                CalendarGrid.Children.Remove(day);
            }
            _cards.Clear();


            // filling current month daycards
            var dt = new DateTime(currentDate.Year,
                                  currentDate.Month, 1);
            var month = dt.Month;
            var dtStart = dt;
            DateTime dtEnd = dt;

            var str = 0;
            for(int i = DayOfWeekToOffset(dt.DayOfWeek);
                dt.Month == month;
                dt = dt.AddDays(1), i++, str = i / 7)
            {
                var ev = events.FirstOrDefault(e => e.Day == dt.Day);
                var col = DayOfWeekToOffset(dt.DayOfWeek);

                CalendarDay? card = null;

                if(ev is null)
                {
                    card = new(dt.Day, dt.Month);
                }
                else
                {
                    card = new(ev);
                }

                card.SetValue(Grid.RowProperty, str);
                card.SetValue(Grid.ColumnProperty, col);
                card.SetValue(Control.MarginProperty, ItemMargin);
                card.SetValue(Control.MinHeightProperty, _cardHeight);
                card.SetValue(Control.MinWidthProperty, _cardWidth);

                _cards.Add(card);
                CalendarGrid.Children.Add(card);

                dtEnd = dt;
            }


            // adding blank daycard method
            void AddBlankDay(DateTime _dt, int str)
            {
                var col = DayOfWeekToOffset(_dt.DayOfWeek);
                var card = new CalendarDay(_dt.Day, _dt.Month, true);

                card.SetValue(Grid.RowProperty, str);
                card.SetValue(Grid.ColumnProperty, col);
                card.SetValue(Control.MarginProperty, ItemMargin);
                card.SetValue(Control.MinHeightProperty, _cardHeight);
                card.SetValue(Control.MinWidthProperty, _cardWidth);

                _cards.Add(card);
                CalendarGrid.Children.Add(card);
            }


            // filling days before the month
            if(dtStart.DayOfWeek != DayOfWeek.Monday)
            {
                do
                {
                    dtStart = dtStart.AddDays(-1);

                    AddBlankDay(dtStart, 0);
                }
                while (dtStart.DayOfWeek != DayOfWeek.Monday);
            }


            // filling days after the month
            if (dtEnd.DayOfWeek != DayOfWeek.Sunday)
            {
                do
                {
                    dtEnd = dtEnd.AddDays(1);

                    AddBlankDay(dtEnd, str);
                }
                while (dtEnd.DayOfWeek != DayOfWeek.Sunday);
            }

            LoadBackgroundImage(month);

            ImageBrd.BeginAnimation(Control.OpacityProperty, _calendarAppearing);
        }

        private void LoadBackgroundImage(int month)
        {
            switch (month)
            {
                case 12:
                case 1:
                case 2:
                    ImageBrd.Background = (Brush)FindResource("WinterBrush");
                    break;

                case 3:
                case 4:
                case 5:
                    ImageBrd.Background = (Brush)FindResource("SpringBrush");
                    break;

                case 6:
                case 7:
                case 8:
                    ImageBrd.Background = (Brush)FindResource("SummerBrush");
                    break;

                case 9:
                case 10:
                case 11:
                    ImageBrd.Background = (Brush)FindResource("AutumnBrush");
                    break;

                default:
                    ImageBrd.Background = Brushes.Transparent;
                    break;
            }
        }

        private int DayOfWeekToOffset(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 6;
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                default:
                    return 0;
            }
        }

        #endregion

        #region EventFrame animation

        private void ShowFrame()
        {
            FrameBrd.Visibility = Visibility.Visible;
            SurfaceBrd_Top.Visibility = Visibility.Visible;
            SurfaceBrd_Bottom.Visibility = Visibility.Visible;

            _frameAppearing.From = FrameBrd.ActualWidth;
            _frameAppearing.To = 12;

            TT.BeginAnimation(TranslateTransform.XProperty, _frameAppearing);
            SurfaceBrd_Top.BeginAnimation(Control.OpacityProperty, _surfaceTopAppearing);
            SurfaceBrd_Bottom.BeginAnimation(Control.OpacityProperty, _surfaceBottomAppearing); 
        }

        private void HideFrame()
        {
            _frameDisappearing.From = TT.X;
            _frameDisappearing.To = FrameBrd.ActualWidth + 12;

            TT.BeginAnimation(TranslateTransform.XProperty, _frameDisappearing);
            SurfaceBrd_Top.BeginAnimation(Control.OpacityProperty, _surfaceTopDisappearing);
            SurfaceBrd_Bottom.BeginAnimation(Control.OpacityProperty, _surfaceBottomDisappearing);
        }

        #endregion

        #region Handlers

        private void HandleClickOutsideOfControl(object sender, MouseButtonEventArgs e)
        {
            if(!_isRightSidePanelVisible) return;

            var pt = e.GetPosition((UIElement)sender);
            _hitResultList.Clear();

            //Retrieving all the elements under the cursor
            VisualTreeHelper.HitTest(this, null,
                new HitTestResultCallback(MyHitTestResultCallback),
                new PointHitTestParameters(pt));

            //Testing if the page is under the cursor
            //var contains = _hitResultList.Contains(this);
            if (!_hitResultList.Contains(FrameBrd))
            {
                WeakReferenceMessenger.Default
                    .Send(new CloseCalendarEventChangingMessage(null));
            }
        }

        #region Callbacks

        private HitTestResultBehavior MyHitTestResultCallback(HitTestResult result)
        {
            _hitResultList.Add(result.VisualHit);
            return HitTestResultBehavior.Continue;
        }

        #endregion

        #endregion

        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            ImageBrd.BeginAnimation(Control.OpacityProperty, _calendarDisappearing);
        }
    }
}
