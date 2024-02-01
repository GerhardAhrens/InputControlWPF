﻿namespace InputControlWPF.InputControls
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    public class TextBoxDecimal : TextBox
    {
        public static readonly DependencyProperty IsNegativeProperty = DependencyProperty.Register("IsNegative", typeof(bool), typeof(TextBoxDecimal), new PropertyMetadata(false));
        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(TextBoxDecimal), new FrameworkPropertyMetadata(2));
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(decimal), typeof(TextBoxDecimal), new FrameworkPropertyMetadata(0M, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private char decimalSeparator = ',';

        public TextBoxDecimal()
        {
            string separator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            this.decimalSeparator = Convert.ToChar(separator);

            this.FontSize = 12.0;
            this.BorderBrush = Brushes.Green;
            this.HorizontalContentAlignment = HorizontalAlignment.Right;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 20;
            this.ClipToBounds = false;
            this.Focusable = true;

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
        }

        public bool IsNegative
        {
            get { return (bool)GetValue(IsNegativeProperty); }
            set { SetValue(IsNegativeProperty, value); }
        }

        public int DecimalPlaces
        {
            get {return (int)GetValue(DecimalPlacesProperty);}
            set {SetValue(DecimalPlacesProperty, value); }
        }

        public decimal Number
        {
            get { return (decimal)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CaretIndex = this.Text.Length;
            this.SelectAll();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (string.IsNullOrEmpty( this.Text)  == true )
            {
                this.Text = string.Empty;
                this.Number = 0;
            }
            else
            {
                this.Number = Convert.ToDecimal(string.IsNullOrEmpty(this.Text) == true ? "0" : this.Text);
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            this.SelectAll();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (this.DecimalPlaces == 0)
            {
                this.Number = Convert.ToDecimal(this.Text);
            }
            else
            {
                e.Handled = false;
                this.Number = Convert.ToDecimal(string.IsNullOrEmpty(this.Text) == true ? "0" : this.Text);
                int c = GetDecimalPlaces(this.Number);
                if (this.DecimalPlaces != c)
                {
                    this.Text = $"{this.Text}{this.decimalSeparator}{new string('0', this.DecimalPlaces - c)}";
                    this.Number = Convert.ToDecimal(this.Text);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.ClickCount == 2)
            {
                this.SelectAll();
            }

        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (this.IsNegative == true)
            {
                if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
                {
                    if (this.Text.Count(c => c == '-') >= 1)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        int cursorPos = ((TextBox)e.Source).CaretIndex;
                        if (cursorPos == 0)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }

                    return;
                }
            }

            if (e.Key == Key.OemComma)
            {
                if (this.Text.Count(c => c == decimalSeparator) >= 1)
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }

                return;
            }

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key == 2 || key == 32 || key == 21 || key == 22 || key == 23 || key == 25 || key == 9);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                if (e.Key == Key.Tab)
                {
                    return;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Up:
                        {
                            this.MoveFocus(FocusNavigationDirection.Previous);
                            return;
                        }
                    case Key.Down:
                        {
                            this.MoveFocus(FocusNavigationDirection.Next);
                            return;
                        }
                    case Key.Left:
                        {
                            return;
                        }
                    case Key.Right:
                        {
                            return;
                        }
                    case Key.Pa1:
                        {
                            return;
                        }
                    case Key.End:
                        {
                            return;
                        }
                    case Key.Back:
                        {
                            return;
                        }
                    case Key.Delete:
                        {
                            return;
                        }
                    case Key.Return:
                        {
                            this.MoveFocus(FocusNavigationDirection.Next);
                            return;
                        }

                    case Key.Tab:
                        {
                            this.MoveFocus(FocusNavigationDirection.Next);
                            return;
                        }
                }
            }

            if (this.Text.IndexOf(decimalSeparator) > 0)
            {
                int decPlaces = this.Text.Substring(this.Text.IndexOf(decimalSeparator)).Length;
                if (decPlaces > this.DecimalPlaces)
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void MoveFocus(FocusNavigationDirection direction)
        {
            UIElement focusedElement = Keyboard.FocusedElement as UIElement;

            if (focusedElement != null)
            {
                if (focusedElement is TextBox)
                {
                    focusedElement.MoveFocus(new TraversalRequest(direction));
                }
            }
        }

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightYellow });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            return inputControlStyle;
        }

        private int GetDecimalPlaces(decimal @this)
        {
            return BitConverter.GetBytes(decimal.GetBits(@this)[3])[2];
        }
    }
}
