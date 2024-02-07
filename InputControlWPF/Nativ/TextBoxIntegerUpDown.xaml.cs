/*
 * https://www.codeproject.com/Articles/139629/A-Numeric-Up-Down-Control-for-WPF
 */

namespace InputControlWPF.InputControls
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TextBoxIntegerUpDown.xaml
    /// </summary>
    public partial class TextBoxIntegerUpDown : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(TextBoxIntegerUpDown), new PropertyMetadata(0, new PropertyChangedCallback(OnValuePropertyChanged)));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(TextBoxIntegerUpDown), new UIPropertyMetadata(100));
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(int), typeof(TextBoxIntegerUpDown), new UIPropertyMetadata(0));
        private static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxIntegerUpDown));
        private static readonly RoutedEvent IncreaseClickedEvent = EventManager.RegisterRoutedEvent("IncreaseClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxIntegerUpDown));
        private static readonly RoutedEvent DecreaseClickedEvent = EventManager.RegisterRoutedEvent("DecreaseClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxIntegerUpDown));
        private readonly Regex _numMatch;

        public TextBoxIntegerUpDown()
        {
            this.InitializeComponent();

            _numMatch = new Regex(@"^-?\d+$");
            this.Maximum = int.MaxValue;
            this.Minimum = 0;
            this.TxtIntegerUpDown.Text = "0";
            this.TxtIntegerUpDown.HorizontalContentAlignment = HorizontalAlignment.Right;
            this.TxtIntegerUpDown.VerticalAlignment = VerticalAlignment.Center;
            this.TxtIntegerUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            this.TxtIntegerUpDown.FontSize = 12.0;
            this.TxtIntegerUpDown.FontFamily = new FontFamily("Arial");
            this.TxtIntegerUpDown.BorderBrush = Brushes.Green;
            this.TxtIntegerUpDown.Padding = new Thickness(0);
            this.TxtIntegerUpDown.Margin = new Thickness(2);

            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnUp, "Click", this.OnClickUp);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDown, "Click", this.OnClickDown);
            WeakEventManager<TextBox, TextCompositionEventArgs>.AddHandler(this.TxtIntegerUpDown, "PreviewTextInput", this.OnPreviewTextInput);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(this.TxtIntegerUpDown, "TextChanged", this.OnTextChanged);
            WeakEventManager<TextBox, KeyEventArgs>.AddHandler(this.TxtIntegerUpDown, "PreviewKeyDown", this.OnPreviewKeyDown);
        }

        public int Value
        {
            get
            {
                return (int)this.GetValue(ValueProperty);
            }
            set
            {
                this.TxtIntegerUpDown.Text = value.ToString();
                this.SetValue(ValueProperty, value);
            }
        }

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }


        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public event RoutedEventHandler IncreaseClicked
        {
            add { AddHandler(IncreaseClickedEvent, value); }
            remove { RemoveHandler(IncreaseClickedEvent, value); }
        }

        public event RoutedEventHandler DecreaseClicked
        {
            add { AddHandler(DecreaseClickedEvent, value); }
            remove { RemoveHandler(DecreaseClickedEvent, value); }
        }

        private static void OnValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            TextBoxIntegerUpDown control = target as TextBoxIntegerUpDown;
            control.TxtIntegerUpDown.Text = e.NewValue.ToString();
        }

        private void OnClickDown(object sender, RoutedEventArgs e)
        {
            if (this.Value < this.Maximum)
            {
                this.Value++;
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
        }

        private void OnClickUp(object sender, RoutedEventArgs e)
        {
            if (this.Value > this.Minimum)
            {
                this.Value--;
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            var text = tb.Text.Insert(tb.CaretIndex, e.Text);

            e.Handled = !_numMatch.IsMatch(text);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (!_numMatch.IsMatch(tb.Text))
            {
                ResetText(tb);
            }

            this.Value = Convert.ToInt32(tb.Text);
            if (this.Value < this.Minimum)
            {
                this.Value = this.Minimum;
            }

            if (this.Value > this.Maximum)
            {
                this.Value = this.Maximum;
            }

            RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Up && Value < Maximum)
            {
                this.Value++;
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
            else if (e.IsDown && e.Key == Key.Down && Value > Minimum)
            {
                this.Value--;
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));

            }
        }

        private void ResetText(TextBox tb)
        {
            tb.Text = 0 < Minimum ? Minimum.ToString() : "0";
            tb.SelectAll();
        }
    }
}
