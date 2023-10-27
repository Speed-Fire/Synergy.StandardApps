using Synergy.WPF.Common.Controls;
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

namespace Synergy.StandardApps.Notes.UserControls
{
    /// <summary>
    /// Логика взаимодействия для NotesExpander.xaml
    /// </summary>
    public partial class NotesExpander : UserControl
    {
        #region Properties

        #region Title

        public static DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NotesExpander), new PropertyMetadata(""));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion

        #region IsExpanded

        public static DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(NotesExpander), new PropertyMetadata(true));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        #endregion

        #endregion

        private DoubleAnimation _gridUpdatedAnimation;

        public NotesExpander()
        {
            InitializeComponent();

            _gridUpdatedAnimation = new()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5),
            };
        }

        public void Add(UIElement item)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));

            Items.Opacity = 0;

            Items.Children.Insert(0, item);

            if(this.Visibility != Visibility.Visible)
            {
                this.Visibility = Visibility.Visible;
            }

            Items.BeginAnimation(UIElement.OpacityProperty, _gridUpdatedAnimation);
        }

        public void Remove(UIElement item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var xDisappear = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
            };
            var yDisappear = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
            };
            yDisappear.Completed += (sender, e) => { Remove_impl(item); };

            var st = item.RenderTransform as ScaleTransform;
            st.BeginAnimation(ScaleTransform.ScaleXProperty, xDisappear);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, yDisappear);
        }

        private void Remove_impl(UIElement item)
        {
            Items.Opacity = 0;

            Items.Children.Remove(item);

            if (Items.Children.Count == 0)
            {
                this.Visibility = Visibility.Collapsed;
            }

            Items.BeginAnimation(UIElement.OpacityProperty, _gridUpdatedAnimation);
        }

        public bool Remove(Func<UIElement, bool> predicate)
        {
            foreach(UIElement item in Items.Children)
            {
                if (predicate(item))
                {
                    Remove(item);
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            Items.Children.Clear();

            this.Visibility = Visibility.Collapsed;
        }

    }
}
