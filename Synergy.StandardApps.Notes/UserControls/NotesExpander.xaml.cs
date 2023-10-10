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

        #region RowLength

        public static DependencyProperty RowLengthProperty=
            DependencyProperty.Register("RowLength", typeof(int), typeof(NotesExpander), new PropertyMetadata(0, OnRowLengthChanged));

        public int RowLength
        {
            get => (int)GetValue(RowLengthProperty);
            set => SetValue(RowLengthProperty, value);
        }

        private static void OnRowLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var expander = (NotesExpander)d;
            if (expander != null)
                expander.OnRowLengthChanged((int)e.OldValue, (int)e.NewValue);
        }

        private void OnRowLengthChanged(int oldValue,  int newValue)
        {
            if (newValue < 0)
                throw new ArgumentException("Length can't be less than zero!");

            if(oldValue == newValue) return;

            if(oldValue > newValue)
            {
                for(int i = 0; i < oldValue - newValue; i++)
                {
                    Items.ColumnDefinitions.RemoveAt(Items.ColumnDefinitions.Count - 1);
                }
            }
            else
            {
                for (int i = 0; i < newValue - oldValue; i++)
                {
                    var column = new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    };

                    Items.ColumnDefinitions.Add(column);
                }
            }
        }

        #endregion

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

            item.SetValue(Grid.RowProperty, 0);
            item.SetValue(Grid.ColumnProperty, 0);

            // fix old children
            foreach(UIElement _item in Items.Children)
            {
                var r = (int)_item.GetValue(Grid.RowProperty);
                var c = (int)_item.GetValue(Grid.ColumnProperty);

                if(++c == RowLength)
                {
                    if (r + 1 == Items.RowDefinitions.Count)
                        Items.RowDefinitions.Add(new() { Height = GridLength.Auto });

                    c = 0;
                    r++;
                }

                _item.SetValue(Grid.RowProperty, r);
                _item.SetValue(Grid.ColumnProperty, c);
            }

            if(Items.RowDefinitions.Count == 0)
            {
                Items.RowDefinitions.Add(new() { Height = GridLength.Auto });
            }

            Items.Children.Add(item);

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

            var _r = (int)item.GetValue(Grid.RowProperty);
            var _c = (int)item.GetValue(Grid.ColumnProperty);

            Items.Children.Remove(item);

            // get items to fix
            var itemsToFix = new List<UIElement>();
            foreach (UIElement _item in Items.Children)
            {
                var r = (int)_item.GetValue(Grid.RowProperty);
                var c = (int)_item.GetValue(Grid.ColumnProperty);

                if (r > _r || (r == _r && c > _c))
                {
                    itemsToFix.Add(_item);
                }
            }

            int maxRow = 0;
            // fix items
            foreach (var _item in itemsToFix)
            {
                var r = (int)_item.GetValue(Grid.RowProperty);
                var c = (int)_item.GetValue(Grid.ColumnProperty);

                if (--c == -1)
                {
                    c = RowLength - 1;
                    r--;
                }

                _item.SetValue(Grid.RowProperty, r);
                _item.SetValue(Grid.ColumnProperty, c);

                if (r > maxRow)
                    maxRow = r;
            }

            // remove empty rows
            var countToRemove = Items.RowDefinitions.Count - (maxRow + 1);
            for (int i = 0; i < countToRemove; i++)
            {
                Items.RowDefinitions.RemoveAt(Items.RowDefinitions.Count - 1);
            }

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
            Items.RowDefinitions.Clear();
            Items.RowDefinitions.Add(new() { Height = GridLength.Auto });

            this.Visibility = Visibility.Collapsed;
        }

    }
}
