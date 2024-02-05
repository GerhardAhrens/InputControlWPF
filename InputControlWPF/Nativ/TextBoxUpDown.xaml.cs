/*
 * https://stackoverflow.com/questions/9460034/custom-itemssource-property-for-a-usercontrol
 * https://www.codeproject.com/Articles/267601/Csharp-WPF-NET-ArrowRepeatButton-NumericUpDown
 * 
 */

namespace InputControlWPF.InputControls
{
    using System.Collections;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TextBoxUpDown.xaml
    /// </summary>
    public partial class TextBoxUpDown : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TextBoxUpDown), new PropertyMetadata(null, OnItemsSourceChanged));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxUpDown), new PropertyMetadata(true, OnSetBorderChanged));

        public TextBoxUpDown()
        {
            this.InitializeComponent();
            this.TextBoxStringUpDown.BorderBrush = Brushes.Green;
            this.TextBoxStringUpDown.VerticalContentAlignment = VerticalAlignment.Center;

            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnUp, "Click", this.OnClickUp);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDown, "Click", this.OnClickDown);
        }

        private void OnClickUp(object sender, RoutedEventArgs e)
        {
        }

        private void OnClickDown(object sender, RoutedEventArgs e)
        {
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
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
                    control.TextBoxStringUpDown.Text = ((List<string>)e.NewValue).First();
                }
            }
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxAll)d;

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
    }
}
