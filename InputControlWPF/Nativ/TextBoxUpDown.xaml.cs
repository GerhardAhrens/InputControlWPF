/*
 * https://stackoverflow.com/questions/9460034/custom-itemssource-property-for-a-usercontrol
 * https://www.codeproject.com/Articles/267601/Csharp-WPF-NET-ArrowRepeatButton-NumericUpDown
 * 
 */

namespace InputControlWPF.InputControls
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für TextBoxUpDown.xaml
    /// </summary>
    public partial class TextBoxUpDown : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TextBoxUpDown), new PropertyMetadata(null, OnItemsSourceChanged));

        public TextBoxUpDown()
        {
            this.InitializeComponent();
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxUpDown)d;
            }
        }
    }
}
