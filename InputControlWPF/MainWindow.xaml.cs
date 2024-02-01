namespace InputControlWPF
{
    using System.Windows;

    using InputControlWPF.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public XamlProperty<decimal> ValueDecimal { get; set; } = XamlProperty.Set<decimal>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal value = this.ValueDecimal.Value;

            MessageBox.Show(value.ToString());

        }
    }
}