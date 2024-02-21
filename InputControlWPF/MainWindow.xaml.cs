/*
 * https://stackoverflow.com/questions/3227462/how-to-add-string-with-the-style-add-into-resourcedictionary-in-wpf
 * https://stackoverflow.com/questions/910814/loading-xaml-at-runtime
 */

namespace InputControlWPF
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using InputControlWPF.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.ValueSourceStrings.Value = new List<string> { "Affe", "Bär","Elefant","Hund","Zebra" };
            this.ValueSourceYears.Value = Enumerable.Range(DateTime.Today.Year-5, 30).Select(x => (x - 1) + 1); 
            this.ValueIntUpDown.Value = 1;
            this.ValueStringUpDown.Value = "Bär";

            Style aa = LoadXaml<Style>(StyleString());

            this.DataContext = this;
        }

        public XamlProperty<string> ValueTextAll { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<decimal> ValueDecimal { get; set; } = XamlProperty.Set<decimal>();
        public XamlProperty<int> ValueInt { get; set; } = XamlProperty.Set<int>();
        public XamlProperty<string> ValueMath { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<List<string>> ValueSourceStrings { get; set; } = XamlProperty.Set<List<string>>();
        public XamlProperty<IEnumerable<int>> ValueSourceYears { get; set; } = XamlProperty.Set<IEnumerable<int>>();
        public XamlProperty<string> ValueStringUpDown { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<int> ValueIntUpDown { get; set; } = XamlProperty.Set<int>();

        private string _ValueText;
        public string ValueText
        {
            get { return _ValueText; }
            set { SetField(ref _ValueText, value); }
        }

        private List<string> _MultiSelecteds;
        public List<string> MultiSelecteds
        {
            get { return _MultiSelecteds; }
            set
            { 
                SetField(ref _MultiSelecteds, value);
                SelectedsString = string.Join(",", MultiSelecteds.Select(s => s));
            }
        }

        private string _SelectedsString;
        public string SelectedsString
        {
            get { return _SelectedsString; }
            set
            {
                SetField(ref _SelectedsString, value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string valueTextAll = this.ValueTextAll.Value;
            int valueInt = this.ValueInt.Value;
            decimal valueDec = this.ValueDecimal.Value;
            string valueMath = this.ValueMath.Value;
            string valueStringUpDown = this.ValueStringUpDown.Value;
            int valueIntUpDown = this.ValueIntUpDown.Value;

            string msg = $"TextBoxAll={valueTextAll}\nTextBoxInt={valueInt}\nTextBoxStringUpDown={valueStringUpDown}\nTextBoxIntegerUpDown={valueIntUpDown}";

            MessageBox.Show(msg);

        }

        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung

        private void GroupBox2_Click(object sender, RoutedEventArgs e)
        {
            string msg = $"{this.SelectedsString}";
            MessageBox.Show(msg);
        }

        private void GroupBox3_Click(object sender, RoutedEventArgs e)
        {
            this.MultiSelecteds = new List<string> { "Elefant", "Hund" };
        }


        private string StyleString()
        {
            string styleText = @" <Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""> " +
                                " <Style TargetType=\"Button\"> " +
                                "<Setter Property=\"Background\" Value=\"Yellow\" />" +
                            "</Style>";

            return styleText;
        }

        public T LoadXaml<T>(string xaml)
        {
            using (var stringReader = new System.IO.StringReader(xaml))
            using (var xmlReader = System.Xml.XmlReader.Create(stringReader))
                return (T)System.Windows.Markup.XamlReader.Load(xmlReader);
        }
    }
}