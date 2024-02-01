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
        public XamlProperty<int> ValueInt { get; set; } = XamlProperty.Set<int>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal valueInt = this.ValueInt.Value;
            decimal valueDec = this.ValueDecimal.Value;

            MessageBox.Show(valueInt.ToString());

        }
    }
}