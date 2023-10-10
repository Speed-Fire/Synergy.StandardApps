using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.Alarms.Messages;
using Synergy.StandardApps.Alarms.ViewModels;
using Synergy.StandardApps.EntityForms.Notes;
using System;
using System.Collections.Generic;
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

namespace Synergy.StandardApps.Alarms
{
    /// <summary>
    /// Логика взаимодействия для AlarmsView.xaml
    /// </summary>
    public partial class AlarmsView : 
        UserControl,
        IRecipient<SetRightSidePanelVisibilityMessage>
    {
        private readonly List<DependencyObject> _hitResultList = new();

        #region Animations

        #region Alarms

        private readonly DoubleAnimation _alarmsAppearing;

        #endregion

        #region RightSideBar

        private readonly Storyboard _rightSidePanelAppearing;
        private readonly Storyboard _rightSidePanelDisappearing;

        private readonly DoubleAnimation _frameDisappearing;
        private readonly DoubleAnimation _frameAppearing;

        private const double _surfaceMaxOpacity = 0.8;

        #endregion

        #endregion

        #region Properties

        private volatile bool _isRightSidePanelSliding;

        private volatile bool _isRightSidePanelVisible;
        public bool IsRightSidePanelVisible
        {
            get => _isRightSidePanelVisible;

            set
            {
                if (_isRightSidePanelSliding) return;

                if (_isRightSidePanelVisible == value)
                    return;

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

        public AlarmsView()
        {
            InitializeComponent();

            #region Animations

            #region Notes

            _alarmsAppearing = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.7)
            };

            #endregion

            #region RightSidePanel

            #region Appearing

            _frameAppearing = new DoubleAnimation()
            {
                DecelerationRatio = 1,
                Duration = TimeSpan.FromSeconds(1.5)
            };

            var _surfaceTopAppearing = new DoubleAnimation()
            {
                From = 0,
                To = _surfaceMaxOpacity,
                Duration = TimeSpan.FromSeconds(1),
                DecelerationRatio = 1
            };

            var _surfaceBottomAppearing = new DoubleAnimation()
            {
                From = 0,
                To = _surfaceMaxOpacity,
                Duration = TimeSpan.FromSeconds(1),
                DecelerationRatio = 1
            };

            #region Storyboard

            _rightSidePanelAppearing = new Storyboard();

            Storyboard.SetTargetName(_frameAppearing, "TT");
            Storyboard.SetTarget(_surfaceTopAppearing, SurfaceBrd_Top);
            Storyboard.SetTarget(_surfaceBottomAppearing, SurfaceBrd_Bottom);

            Storyboard.SetTargetProperty(_frameAppearing, new PropertyPath(TranslateTransform.XProperty));
            Storyboard.SetTargetProperty(_surfaceTopAppearing, new PropertyPath(Border.OpacityProperty));
            Storyboard.SetTargetProperty(_surfaceBottomAppearing, new PropertyPath(Border.OpacityProperty));

            _rightSidePanelAppearing.Children.Add(_frameAppearing);
            _rightSidePanelAppearing.Children.Add(_surfaceTopAppearing);
            _rightSidePanelAppearing.Children.Add(_surfaceBottomAppearing);

            _rightSidePanelAppearing.Completed += (sender, e) =>
            {
                _isRightSidePanelSliding = false;
            };

            #endregion

            #endregion

            #region Disappearing

            _frameDisappearing = new DoubleAnimation()
            {
                DecelerationRatio = 1,
                Duration = TimeSpan.FromSeconds(1.5)
            };

            var _surfaceTopDisappearing = new DoubleAnimation()
            {
                From = _surfaceMaxOpacity,
                To = 0,
                Duration = TimeSpan.FromSeconds(1),
                DecelerationRatio = 1
            };

            var _surfaceBottomDisappearing = new DoubleAnimation()
            {
                From = _surfaceMaxOpacity,
                To = 0,
                Duration = TimeSpan.FromSeconds(1),
                DecelerationRatio = 1
            };

            _surfaceTopDisappearing.Completed += (sender, e) => { SurfaceBrd_Top.Visibility = Visibility.Collapsed; };
            _surfaceBottomDisappearing.Completed += (sender, e) => { SurfaceBrd_Bottom.Visibility = Visibility.Collapsed; };

            #region Storyboard

            _rightSidePanelDisappearing = new Storyboard();

            Storyboard.SetTargetName(_frameDisappearing, "TT");
            Storyboard.SetTarget(_surfaceTopDisappearing, SurfaceBrd_Top);
            Storyboard.SetTarget(_surfaceBottomDisappearing, SurfaceBrd_Bottom);

            Storyboard.SetTargetProperty(_frameDisappearing, new PropertyPath(TranslateTransform.XProperty));
            Storyboard.SetTargetProperty(_surfaceTopDisappearing, new PropertyPath(Border.OpacityProperty));
            Storyboard.SetTargetProperty(_surfaceBottomDisappearing, new PropertyPath(Border.OpacityProperty));

            _rightSidePanelDisappearing.Children.Add(_frameDisappearing);
            _rightSidePanelDisappearing.Children.Add(_surfaceTopDisappearing);
            _rightSidePanelDisappearing.Children.Add(_surfaceBottomDisappearing);

            _rightSidePanelDisappearing.Completed += (sender, e) =>
            {
                FrameBrd.Visibility = Visibility.Hidden;
                _isRightSidePanelSliding = false;

                WeakReferenceMessenger.Default
                    .Send(new RightSidePanelClosedMessage(null));
            };

            #endregion

            #endregion

            #endregion

            #endregion
        }

        #region Messages

        void IRecipient<SetRightSidePanelVisibilityMessage>.Receive(SetRightSidePanelVisibilityMessage message)
        {
            IsRightSidePanelVisible = message.Value;
        }

        #endregion

        #region Loading

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FrameBrd.Visibility = Visibility.Hidden;

            AddMouseHandler();

            IsRegistered = true;

            HideFrame();

            ImageBrd.BeginAnimation(UIElement.OpacityProperty, _alarmsAppearing);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveMouseHandler();

            IsRegistered = false;
        }

        #endregion

        #region Methods

        #region Notes initialization

        

        #endregion

        #region Right side panel visibility

        private void ShowFrame()
        {
            if (_isRightSidePanelSliding) return;
            _isRightSidePanelSliding = true;

            FrameBrd.Visibility = Visibility.Visible;
            SurfaceBrd_Top.Visibility = Visibility.Visible;
            SurfaceBrd_Bottom.Visibility = Visibility.Visible;

            _frameAppearing.From = FrameBrd.ActualWidth;
            _frameAppearing.To = 12;

            _rightSidePanelAppearing.Begin(this);
        }

        private void HideFrame()
        {
            if (_isRightSidePanelSliding) return;
            _isRightSidePanelSliding = true;

            _frameDisappearing.From = TT.X;
            _frameDisappearing.To = FrameBrd.ActualWidth + 12;

            _rightSidePanelDisappearing.Begin(this);
        }

        #endregion

        #region Handlers registrations

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

        #endregion

        #region Handlers

        private void HandleClickOutsideOfControl(object sender, MouseButtonEventArgs e)
        {
            if (!_isRightSidePanelVisible) return;

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
                    .Send(new CloseAlarmChangingMessage(null));
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
    }
}
