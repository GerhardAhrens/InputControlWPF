namespace InputControlWPF.InputControls
{
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TextBoxUpDown.xaml
    /// </summary>
    public partial class TextBoxUpDown : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TextBoxUpDown), new PropertyMetadata(null, OnItemsSourceChanged));
        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(TextBoxUpDown), new PropertyMetadata(string.Empty, OnDefaultValueChanged));
        public static readonly DependencyProperty ResultValueProperty = DependencyProperty.Register("ResultValue", typeof(string), typeof(TextBoxUpDown), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxUpDown), new PropertyMetadata(true, OnSetBorderChanged));

        private static ICollectionView itemSource { get; set; }
        private static string defaultValue { get; set; }

        public TextBoxUpDown()
        {
            this.InitializeComponent();
            this.TextBoxStringUpDown.BorderBrush = Brushes.Green;
            this.TextBoxStringUpDown.VerticalContentAlignment = VerticalAlignment.Center;

            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnUp, "Click", this.OnClickUp);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDown, "Click", this.OnClickDown);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string DefaultValue
        {
            get { return (string)GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        public string ResultValue
        {
            get { return (string)GetValue(ResultValueProperty); }
            set { SetValue(ResultValueProperty, value); }
        }

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = d as TextBoxUpDown;
                if (control != null)
                {
                    itemSource = CollectionViewSource.GetDefaultView(e.NewValue);
                    itemSource.MoveCurrentToPrevious();
                    itemSource.MoveCurrentToFirst();
                    if (string.IsNullOrEmpty(defaultValue) == false)
                    {
                        itemSource.MoveCurrentTo(defaultValue);
                    }

                    control.TextBoxStringUpDown.Text = itemSource.CurrentItem.ToString();
                    control.ResultValue = itemSource.CurrentItem.ToString();
                }
            }
        }

        private static void OnDefaultValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = d as TextBoxUpDown;
                if (control != null)
                {
                    defaultValue = (string)e.NewValue;
                }
            }
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxUpDown)d;

                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.BorderBrush = Brushes.Green;
                        control.BorderThickness = new Thickness(1);
                    }
                    else
                    {
                        control.BorderBrush = Brushes.Transparent;
                        control.BorderThickness = new Thickness(0);
                    }
                }
            }
        }

        private void OnClickUp(object sender, RoutedEventArgs e)
        {
            itemSource.MoveCurrentToPrevious();
            if (itemSource.IsCurrentBeforeFirst == false)
            {
                this.TextBoxStringUpDown.Text = itemSource.CurrentItem.ToString();
                this.ResultValue = itemSource.CurrentItem.ToString();
            }
        }

        private void OnClickDown(object sender, RoutedEventArgs e)
        {
            itemSource.MoveCurrentToNext();
            if (itemSource.IsCurrentAfterLast == false)
            {
                this.TextBoxStringUpDown.Text = itemSource.CurrentItem.ToString();
                this.ResultValue = itemSource.CurrentItem.ToString();
            }
        }
    }
}
