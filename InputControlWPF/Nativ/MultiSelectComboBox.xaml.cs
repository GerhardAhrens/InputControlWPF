namespace InputControlWPF.InputControls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using InputControlWPF.NativCore;

    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private ObservableCollection<Node> _nodeList = null;
        private string allText = string.Empty;
        private string noneText = string.Empty;

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(Dictionary<string, object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
                new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(Dictionary<string, object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
                new PropertyChangedCallback(MultiSelectComboBox.OnSelectedItemsChanged)));

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));


        public static readonly DependencyProperty DropDownHeightProperty =
            DependencyProperty.Register("DropDownHeight", typeof(double), typeof(MultiSelectComboBox), new UIPropertyMetadata(100.00));

        public static readonly DependencyProperty DropDownBackgroundProperty =
            DependencyProperty.Register("DropDownBackground", typeof(Brush), typeof(MultiSelectComboBox), new UIPropertyMetadata(Brushes.WhiteSmoke));

        public Dictionary<string, object> ItemsSource
        {
            get { return (Dictionary<string, object>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public Dictionary<string, object> SelectedItems
        {
            get { return (Dictionary<string, object>)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        [Bindable(true)]
        [Category("Layout")]
        [TypeConverter(typeof(LengthConverter))]
        public double DropDownHeight
        {
            get
            {
                return (double)GetValue(DropDownHeightProperty);
            }
            set
            {
                SetValue(DropDownHeightProperty, value);
            }
        }

        [Bindable(true)]
        [Category("Layout")]
        //[TypeConverter(typeof(BrushConverter))]
        public Brush DropDownBackground
        {
            get
            {
                return (Brush)GetValue(DropDownBackgroundProperty);
            }
            set
            {
                SetValue(DropDownBackgroundProperty, value);
            }
        }
        #endregion

        public MultiSelectComboBox()
        {
            InitializeComponent();

            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.Height = ControlBase.DefaultHeight;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(2);
            this.ClipToBounds = false;
            this.Focusable = true;

            CultureInfo ci = CultureInfo.CurrentCulture;
            if (ci.Parent.Name == "de")
            {
                this.allText = "Alle";
                this.noneText = "Keiner";
            }
            else
            {
                this.allText = "All";
                this.noneText = "None";
            }

            this._nodeList = new ObservableCollection<Node>();

            /* Spezifisches Kontextmenü für Control übergeben */
            this.MultiSelectCombo.ContextMenu = this.BuildContextMenu();

        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.DisplayInControl();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        private void OnCheckBoxClick(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;

            if (clickedBox.Content.ToString().ToLower().Contains("all") == true)
            {
                int? countSelected = this._nodeList?.Count(c => c.IsSelected == true);
                int? countAll = this._nodeList?.Count() - 1;

                if (countSelected == countAll)
                {
                    foreach (Node node in this._nodeList)
                    {
                        node.IsSelected = false;
                    }
                }
                else
                {
                    foreach (Node node in this._nodeList)
                    {
                        node.IsSelected = true;
                    }
                }
            }
            else
            {
                int _selectedCount = 0;
                foreach (Node s in this._nodeList)
                {
                    if (s.IsSelected && s.Title.ToLower().Contains("all") == false)
                    {
                        _selectedCount++;
                    }
                }
                if (_selectedCount == this._nodeList.Count - 1)
                {
                    this._nodeList.FirstOrDefault(i => i.Title.ToLower().Contains("all") == true).IsSelected = true;
                }
                else
                {
                    this._nodeList.FirstOrDefault(i => i.Title.ToLower().Contains("all") == true).IsSelected = false;
                }
            }

            this.SetSelectedItems();
            this.SetText();

        }

        private void SelectNodes()
        {
            foreach (KeyValuePair<string, object> keyValue in this.SelectedItems)
            {
                Node node = _nodeList.FirstOrDefault(i => i.Title == keyValue.Key);
                if (node != null)
                {
                    node.IsSelected = true;
                }
            }
        }

        private void SetSelectedItems()
        {
            if (this.SelectedItems == null)
            {
                this.SelectedItems = new Dictionary<string, object>();
            }

            this.SelectedItems.Clear();
            foreach (Node node in _nodeList)
            {
                if (node.IsSelected && node.Title.ToLower().Contains("all") == false)
                {
                    if (this.ItemsSource.Count > 0)
                    {
                        this.SelectedItems.Add(node.Title, this.ItemsSource[node.Title]);
                    }
                }
            }
        }

        private void DisplayInControl()
        {
            _nodeList.Clear();
            if (this.ItemsSource.Count > 0)
            {
                _nodeList.Add(new Node(this.allText));
            }

            foreach (KeyValuePair<string, object> keyValue in this.ItemsSource)
            {
                Node node = new Node(keyValue.Key);
                _nodeList.Add(node);
            }

            this.MultiSelectCombo.ItemsSource = this._nodeList;
        }

        private void SetText()
        {
            if (this.SelectedItems != null)
            {
                StringBuilder displayText = new StringBuilder();
                bool? isSelected = this._nodeList?.Any(c => c.IsSelected == true);
                if (isSelected == false)
                {
                    displayText.Append(this.noneText);
                }
                else
                {
                    foreach (Node s in this._nodeList)
                    {
                        if (s.IsSelected == true && s.Title.ToLower().Contains("all") == true)
                        {
                            displayText = new StringBuilder();
                            displayText.Append(this.allText);
                            break;
                        }
                        else if (s.IsSelected == true && s.Title.ToLower().Contains("all") == false)
                        {
                            displayText.Append(s.Title);
                            displayText.Append(',');
                        }
                    }
                }

                this.Text = displayText.ToString().TrimEnd(new char[] { ',' });
            }

            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.DefaultText;
            }
        }

        /// <summary>
        /// Spezifisches Kontextmenü erstellen
        /// </summary>
        /// <returns></returns>
        private ContextMenu BuildContextMenu()
        {
            ContextMenu textBoxContextMenu = new ContextMenu();

            MenuItem copyMenu = new MenuItem();
            copyMenu.Header = "Alle Kopieren";
            copyMenu.Icon = Icons.GetPathGeometry(Icons.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopy);
            textBoxContextMenu.Items.Add(copyMenu);

            MenuItem copyCheckMenu = new MenuItem();
            copyCheckMenu.Header = "Markierte Kopieren";
            copyCheckMenu.Icon = Icons.GetPathGeometry(Icons.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyCheckMenu, "Click", this.OnCheckCopy);
            textBoxContextMenu.Items.Add(copyCheckMenu);

            return textBoxContextMenu;
        }

        private void OnCopy(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Node keyValue in this._nodeList)
            {
            }

            Clipboard.SetText(sb.ToString());
        }

        private void OnCheckCopy(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Node keyValue in this._nodeList)
            {
            }

            Clipboard.SetText(sb.ToString());
        }
    }

    [DebuggerDisplay("Title: {this.Title}, IsSelected: {this.IsSelected}")]
    public class Node : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _title;
        private bool _isSelected;

        public Node(string title)
        {
            this.Title = title;
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
                this.NotifyPropertyChanged();
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
