﻿namespace InputControlWPF
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

            this.DataContext = this;
        }

        public XamlProperty<decimal> ValueDecimal { get; set; } = XamlProperty.Set<decimal>();
        public XamlProperty<int> ValueInt { get; set; } = XamlProperty.Set<int>();
        public XamlProperty<string> ValueMath { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<List<string>> ValueSourceStrings { get; set; } = XamlProperty.Set<List<string>>();
        //public XamlProperty<string> ValueText { get; set; } = XamlProperty.Set<string>();

        private string _ValueText;
        public string ValueText
        {
            get { return _ValueText; }
            set { SetField(ref _ValueText, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int valueInt = this.ValueInt.Value;
            decimal valueDec = this.ValueDecimal.Value;
            string valueMath = this.ValueMath.Value;
            string valueText = this.ValueText;

            MessageBox.Show(valueInt.ToString());

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
    }
}