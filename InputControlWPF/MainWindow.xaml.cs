namespace InputControlWPF
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;

    using InputControlWPF.Core;
    using InputControlWPF.NativCore;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        string styleString = string.Empty;

        public MainWindow()
        {
            this.InitializeComponent();

            this.ValueSourceStringsCB.Value = new List<string> { "Affe", "Bär", "Elefant", "Hund", "Zebra" };
            this.ValueSourceStrings.Value = new List<string> { "Affe", "Bär","Elefant","Hund","Zebra" };
            this.ValueSourceYears.Value = Enumerable.Range(DateTime.Today.Year-5, 30).Select(x => (x - 1) + 1); 
            this.ValueIntUpDown.Value = 1;
            this.ValueStringUpDown.Value = "Bär";
            this.ValueDate.Value = DateTime.Now;

            /*
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"<Setter Property=\"Background\" Value=\"Yellow\" />").Append(" ");
            */

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<Setter Property=\"HorizontalContentAlignment\" Value=\"Center\"/>");
            stringBuilder.AppendLine("<Setter Property=\"VerticalContentAlignment\" Value=\"Center\"/>");
            stringBuilder.AppendLine("<Setter Property=\"Padding\" Value=\"1\"/>");
            stringBuilder.AppendLine("<Setter Property=\"Template\">");
            stringBuilder.AppendLine("<Setter.Value>");
            stringBuilder.AppendLine("<ControlTemplate TargetType=\"{x:Type Button}\">");
            stringBuilder.AppendLine("<Grid x:Name=\"grid\">");
            stringBuilder.AppendLine("<Border x:Name=\"border\" CornerRadius=\"8\" BorderBrush=\"Black\" BorderThickness=\"2\">");
            stringBuilder.AppendLine("<Border.Background>");
            stringBuilder.AppendLine("<RadialGradientBrush GradientOrigin=\"0.496,1.052\">");
            stringBuilder.AppendLine("<RadialGradientBrush.RelativeTransform>");
            stringBuilder.AppendLine("<TransformGroup>");
            stringBuilder.AppendLine("<ScaleTransform CenterX=\"0.5\" CenterY=\"0.5\" \r\nScaleX=\"1.5\" ScaleY=\"1.5\"/>");
            stringBuilder.AppendLine("<TranslateTransform X=\"0.02\" Y=\"0.3\"/>");
            stringBuilder.AppendLine("</TransformGroup>");
            stringBuilder.AppendLine("</RadialGradientBrush.RelativeTransform>");
            stringBuilder.AppendLine("<GradientStop Offset=\"1\" Color=\"#00000000\"/>");
            stringBuilder.AppendLine("<GradientStop Offset=\"0.3\" Color=\"#FFFFFFFF\"/>");
            stringBuilder.AppendLine("</RadialGradientBrush>");
            stringBuilder.AppendLine("</Border.Background>");
            stringBuilder.AppendLine("<ContentPresenter HorizontalAlignment=\"Center\"\r\nVerticalAlignment=\"Center\"\r\nTextElement.FontWeight=\"Bold\">");
            stringBuilder.AppendLine("</ContentPresenter>");
            stringBuilder.AppendLine("</Border>");
            stringBuilder.AppendLine("</Grid>");
            stringBuilder.AppendLine("<ControlTemplate.Triggers>");
            stringBuilder.AppendLine("<Trigger Property=\"IsPressed\" Value=\"True\">");
            stringBuilder.AppendLine("<Setter Property=\"Background\" TargetName=\"border\">");
            stringBuilder.AppendLine("<Setter.Value>");
            stringBuilder.AppendLine("<RadialGradientBrush GradientOrigin=\"0.496,1.052\">");
            stringBuilder.AppendLine("<RadialGradientBrush.RelativeTransform>");
            stringBuilder.AppendLine("<TransformGroup>");
            stringBuilder.AppendLine("<ScaleTransform CenterX=\"0.5\" CenterY=\"0.5\" ScaleX=\"1.5\" ScaleY=\"1.5\"/>");
            stringBuilder.AppendLine("<TranslateTransform X=\"0.02\" Y=\"0.3\"/>");
            stringBuilder.AppendLine("</TransformGroup>");
            stringBuilder.AppendLine("</RadialGradientBrush.RelativeTransform>");
            stringBuilder.AppendLine(" <GradientStop Color=\"#00000000\" Offset=\"1\"/>");
            stringBuilder.AppendLine("<GradientStop Color=\"#FF303030\" Offset=\"0.3\"/>");
            stringBuilder.AppendLine("</RadialGradientBrush>");
            stringBuilder.AppendLine("</Setter.Value>");
            stringBuilder.AppendLine("</Setter>");
            stringBuilder.AppendLine("</Trigger>");
            stringBuilder.AppendLine("<Trigger Property=\"IsMouseOver\" Value=\"True\">");
            stringBuilder.AppendLine("<Setter Property=\"BorderBrush\" TargetName=\"border\" Value=\"#FF33962B\"/>");
            stringBuilder.AppendLine("</Trigger>");
            stringBuilder.AppendLine("<Trigger Property=\"IsEnabled\" Value=\"False\">");
            stringBuilder.AppendLine("<Setter Property=\"Opacity\" TargetName=\"grid\" Value=\"0.25\"/>");
            stringBuilder.AppendLine("</Trigger>");
            stringBuilder.AppendLine("</ControlTemplate.Triggers>");
            stringBuilder.AppendLine("</ControlTemplate>");
            stringBuilder.AppendLine("</Setter.Value>");
            stringBuilder.AppendLine("</Setter>");

            /*
            stringBuilder.AppendLine($"<Setter Property=\"Background\" Value=\"Yellow\" />").Append(" ");
            */

            /*
            string insertContent = $"<Setter Property=\"Background\" Value=\"Yellow\" />";
            */
            string styleText = new StyleText().Add("Button", stringBuilder).Value;
            Style buttonStyle = XAMLBuilder<Style>.GetStyle(styleText);
            this.BtnGetValueTxt.Style = buttonStyle;

            this.SelectedItemCB.Value = this.ValueSourceStrings.Value.FirstOrDefault();

            this.DataContext = this;
        }

        public XamlProperty<string> ValueTextAll { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<decimal> ValueDecimal { get; set; } = XamlProperty.Set<decimal>();
        public XamlProperty<int> ValueInt { get; set; } = XamlProperty.Set<int>();
        public XamlProperty<string> ValueMath { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<List<string>> ValueSourceStrings { get; set; } = XamlProperty.Set<List<string>>();
        public XamlProperty<List<string>> ValueSourceStringsCB { get; set; } = XamlProperty.Set<List<string>>();
        public XamlProperty<IEnumerable<int>> ValueSourceYears { get; set; } = XamlProperty.Set<IEnumerable<int>>();
        public XamlProperty<string> ValueStringUpDown { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<int> ValueIntUpDown { get; set; } = XamlProperty.Set<int>();
        public XamlProperty<DateTime?> ValueDate { get; set; } = XamlProperty.Set<DateTime?>();
        public XamlProperty<string> SelectedItemCB { get; set; } = XamlProperty.Set<string>();

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
            DateTime? valueDate = this.ValueDate.Value;
            string selectedItemCB = this.SelectedItemCB.Value;

            string msg = $"TextBoxAll={valueTextAll}\nTextBoxInt={valueInt}\nTextBoxDecimal={valueDec}\nTextBoxStringUpDown={valueStringUpDown}\nTextBoxIntegerUpDown={valueIntUpDown}\n{((DateTime)valueDate).ToShortDateString()}\nSelectedItem CM={selectedItemCB}";

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
    }
}