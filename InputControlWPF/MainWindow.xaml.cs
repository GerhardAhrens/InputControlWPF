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
        public XamlProperty<string> ValueMath { get; set; } = XamlProperty.Set<string>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int valueInt = this.ValueInt.Value;
            decimal valueDec = this.ValueDecimal.Value;
            string valueMath = this.ValueMath.Value;

            MessageBox.Show(valueInt.ToString());

        }
    }
}