namespace InputControlWPF.InputControls
{
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TextBoxStringUpDown.xaml
    /// </summary>
    public partial class TextBoxStringUpDown : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TextBoxStringUpDown), new PropertyMetadata(null, OnItemsSourceChanged));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(TextBoxStringUpDown), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnValuePropertyChanged)));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxStringUpDown), new PropertyMetadata(true, OnSetBorderChanged));

        private static ICollectionView itemSource { get; set; }

        public TextBoxStringUpDown()
        {
            this.InitializeComponent();
            this.TxTBoxStringUpDown.BorderBrush = Brushes.Green;
            this.TxTBoxStringUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            this.TxTBoxStringUpDown.VerticalAlignment = VerticalAlignment.Center;
            this.TxTBoxStringUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            this.TxTBoxStringUpDown.FontSize = 12.0;
            this.TxTBoxStringUpDown.FontFamily = new FontFamily("Arial");
            this.TxTBoxStringUpDown.BorderBrush = Brushes.Green;
            this.TxTBoxStringUpDown.Padding = new Thickness(0);
            this.TxTBoxStringUpDown.Margin = new Thickness(2);
            this.TxTBoxStringUpDown.IsReadOnly = false;
            this.TxTBoxStringUpDown.Focusable = true;

            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnUp, "Click", this.OnClickUp);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDown, "Click", this.OnClickDown);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set 
            {
                this.TxTBoxStringUpDown.Text = value.ToString();
                SetValue(ValueProperty, value); 
            }
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
                var control = d as TextBoxStringUpDown;
                if (control != null)
                {
                    itemSource = CollectionViewSource.GetDefaultView(e.NewValue);
                    itemSource.MoveCurrentToFirst();
                }
            }
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxStringUpDown control = d as TextBoxStringUpDown;
            control.TxTBoxStringUpDown.Text = e.NewValue.ToString();
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxStringUpDown)d;

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
                this.TxTBoxStringUpDown.Text = itemSource.CurrentItem.ToString();
                this.Value = itemSource.CurrentItem.ToString();
            }
        }

        private void OnClickDown(object sender, RoutedEventArgs e)
        {
            itemSource.MoveCurrentToNext();
            if (itemSource.IsCurrentAfterLast == false)
            {
                this.TxTBoxStringUpDown.Text = itemSource.CurrentItem.ToString();
                this.Value = itemSource.CurrentItem.ToString();
            }
        }
    }
}
