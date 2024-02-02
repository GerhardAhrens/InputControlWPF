//-----------------------------------------------------------------------
// <copyright file="TextBoxAll.cs" company="Lifeprojects.de">
//     Class: TextBoxAll
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>02.02.2024</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------


namespace InputControlWPF.InputControls
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Shapes = System.Windows.Shapes;

    public class TextBoxMath : TextBox
    {
        /* Definition der Path Geometry Icon für Kontextmenü */
        private const string ICON_COPY = "M19,21H8V7H19M19,5H8A2,2 0 0,0 6,7V21A2,2 0 0,0 8,23H19A2,2 0 0,0 21,21V7A2,2 0 0,0 19,5M16,1H4A2,2 0 0,0 2,3V17H4V3H16V1Z";
        private const string ICON_PASTE = "M19,20H5V4H7V7H17V4H19M12,2A1,1 0 0,1 13,3A1,1 0 0,1 12,4A1,1 0 0,1 11,3A1,1 0 0,1 12,2M19,2H14.82C14.4,0.84 13.3,0 12,0C10.7,0 9.6,0.84 9.18,2H5A2,2 0 0,0 3,4V20A2,2 0 0,0 5,22H19A2,2 0 0,0 21,20V4A2,2 0 0,0 19,2Z";
        private const string ICON_DELETE = "M9,3V4H4V6H5V19A2,2 0 0,0 7,21H17A2,2 0 0,0 19,19V6H20V4H15V3H9M7,6H17V19H7V6M9,8V17H11V8H9M13,8V17H15V8H13Z";
        private const string ICON_CLOCK = "M12,20A8,8 0 0,0 20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22C6.47,22 2,17.5 2,12A10,10 0 0,1 12,2M12.5,7V12.25L17,14.92L16.25,16.15L11,13V7H12.5Z";
        private const string ICON_ADD = "M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z";
        private const string ICON_EQUALS = "M19,10H5V8H19V10M19,16H5V14H19V16Z";
        private const string ICON_MINUS = "M19,13H5V11H19V13Z";
        private const string ICON_MULT = "M11,3H13V10.27L19.29,6.64L20.29,8.37L14,12L20.3,15.64L19.3,17.37L13,13.72V21H11V13.73L4.69,17.36L3.69,15.63L10,12L3.72,8.36L4.72,6.63L11,10.26V3Z";
        private const string ICON_DIV = "M7 21L14.9 3H17L9.1 21H7Z";

        private static Regex regexDisallowedInteger = new Regex(@"[^0-9-]+");  // matches disallowed text
        private static Regex regexDisallowedFloat = new Regex(@"[^0-9-+.,e]+");  // matches disallowed text

        public static readonly DependencyProperty ReadOnlyColorProperty =
            DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TextBoxMath), new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty SetBorderProperty =
            DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxMath), new PropertyMetadata(true, OnSetBorderChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxMath"/> class.
        /// </summary>
        public TextBoxMath()
        {
            this.FontSize = 12.0;
            this.FontFamily = new FontFamily("Arial");
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.FontSize = 14;
            this.IsReadOnly = false;
            this.Focusable = true;

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();

            WeakEventManager<TextBoxMath, TextCompositionEventArgs>.AddHandler(this,"PreviewTextInput", this.OnPreviewTextInput);
        }

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
        }

        public Brush ReadOnlyColor
        {
            get { return (Brush)GetValue(ReadOnlyColorProperty); }
            set { SetValue(ReadOnlyColorProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.CaretIndex = this.Text.Length;
            this.SelectAll();

            /* Spezifisches Kontextmenü für Control übergeben */
            this.ContextMenu = this.BuildContextMenu();

            /* Rahmen für Control festlegen */
            if (SetBorder == true)
            {
                this.BorderBrush = Brushes.Green;
                this.BorderThickness = new Thickness(1);
            }
            else
            {
                this.BorderBrush = Brushes.Transparent;
                this.BorderThickness = new Thickness(0);
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Text.Length > 0 && OpenMathMenu(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = regexDisallowedFloat.IsMatch(e.Text);  // or regexDisallowedInteger
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
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
                        this.MoveFocus(FocusNavigationDirection.Previous);
                        break;
                    case Key.Down:
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
                    case Key.Left:
                        return;
                    case Key.Right:
                        return;
                    case Key.Pa1:
                        return;
                    case Key.End:
                        return;
                    case Key.Delete:
                        return;
                    case Key.Return:
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
                    case Key.Tab:
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
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

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxMath)d;

                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.BorderBrush = Brushes.Green;
                        control.BorderThickness = new Thickness(1);
                    }
                    else
                    {
                        control.BorderBrush = Brushes.Transparent;
                        control.BorderThickness = new Thickness(0);
                    }
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

        /// <summary>
        /// Spezifisches Kontextmenü erstellen
        /// </summary>
        /// <returns></returns>
        private ContextMenu BuildContextMenu()
        {
            ContextMenu textBoxContextMenu = new ContextMenu();
            MenuItem copyMenu = new MenuItem();
            copyMenu.Header = "Kopiere Inhalt";
            copyMenu.Icon = this.GetPathGeometry(ICON_COPY);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            if (this.IsReadOnly == false)
            {
                MenuItem pasteMenu = new MenuItem();
                pasteMenu.Header = "Einfügen Inhalt";
                pasteMenu.Icon = this.GetPathGeometry(ICON_PASTE);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(pasteMenu, "Click", this.OnPasteMenu);
                textBoxContextMenu.Items.Add(pasteMenu);

                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Header = "Lösche Inhalt";
                deleteMenu.Icon = this.GetPathGeometry(ICON_DELETE);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(deleteMenu, "Click", this.OnDeleteMenu);
                textBoxContextMenu.Items.Add(deleteMenu);

                MenuItem setDateMenu = new MenuItem();
                setDateMenu.Header = "Setze Datum";
                setDateMenu.Icon = this.GetPathGeometry(ICON_CLOCK);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(setDateMenu, "Click", this.OnSetDateMenu);
                textBoxContextMenu.Items.Add(setDateMenu);
            }

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
        }

        private void OnPasteMenu(object sender, RoutedEventArgs e)
        {
            this.Text = Clipboard.GetText();
        }

        private void OnDeleteMenu(object sender, RoutedEventArgs e)
        {
            this.Text = string.Empty;
        }

        private void OnSetDateMenu(object sender, RoutedEventArgs e)
        {
            this.Text = DateTime.Now.ToShortDateString();
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <param name="iconColor">Icon Farbe</param>
        /// <returns></returns>
        private Shapes.Path GetPathGeometry(string iconString, Color iconColor, int size = 24)
        {
            var path = new Shapes.Path
            {
                Height = size,
                Width = size,
                Fill = new SolidColorBrush(iconColor),
                Data = Geometry.Parse(iconString)
            };

            return path;
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <returns></returns>
        private Shapes.Path GetPathGeometry(string iconString, int size = 24)
        {
            return GetPathGeometry(iconString, Colors.Blue, size);
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <returns></returns>
        private Shapes.Path GetPathGeometry(string iconString)
        {
            return GetPathGeometry(iconString,Colors.Blue);
        }


        #region Math
        private enum EOperation { Add, Subtract, Multiply, Divide }
        private EOperation operation = EOperation.Add;
        private ContextMenu menuMath = null;
        private MenuItem miOperand = null;
        private MenuItem miResult = null;

        private bool OpenMathMenu(string text)
        {
            if (menuMath != null)
                return false;

            if (text == "+" || text == "-" || text == "*" || text == "/")
            {
                if (text == "-" && CaretIndex == 0)  // negative sign in front of number
                    return false;

                miOperand = new MenuItem();
                miOperand.Header = "";
                miOperand.FontSize = 18;
                miOperand.FontWeight = FontWeights.Medium;
                miOperand.IsEnabled = false;

                if (text == "+")
                {
                    operation = EOperation.Add;
                    miOperand.Icon = this.GetPathGeometry(ICON_ADD);
                }
                else if (text == "-")
                {
                    operation = EOperation.Subtract;
                    miOperand.Icon = this.GetPathGeometry(ICON_MINUS);
                }
                else if (text == "*")
                {
                    operation = EOperation.Multiply;
                    miOperand.Icon = this.GetPathGeometry(ICON_MULT);
                }
                else if (text == "/")
                {
                    operation = EOperation.Divide;
                    miOperand.Icon = this.GetPathGeometry(ICON_DIV);
                }

                miResult = new MenuItem();
                miResult.Header = "";
                miResult.Icon = this.GetPathGeometry(ICON_EQUALS);
                miResult.FontSize = 18;
                miResult.FontWeight = FontWeights.Medium;
                miResult.IsEnabled = false;
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(miResult, "Click", this.OnResultClick);

                menuMath = new ContextMenu();
                menuMath.Items.Add(miOperand);
                menuMath.Items.Add(new Separator());
                menuMath.Items.Add(miResult);
                menuMath.PlacementTarget = this;
                menuMath.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

                WeakEventManager<ContextMenu, KeyEventArgs>.AddHandler(menuMath, "PreviewKeyDown", this.OnMathPreviewKeyDown);
                WeakEventManager<ContextMenu, RoutedEventArgs>.AddHandler(menuMath, "Closed", this.OnMathClosed);
                menuMath.IsOpen = true;
                return true;
            }

            return false;
        }

        private void OnMathPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                return;
            }

            string operand = miOperand.Header.ToString();
            switch (e.Key)
            {
                case Key.Cancel:
                case Key.Clear:
                case Key.Escape:
                case Key.OemClear:
                    miResult.Header = "";
                    OnResultClick(this, null);
                    break;

                case Key.Back:
                case Key.Delete:
                    if (operand.Length > 0)
                    {
                        UpdateResult(operand.Substring(0, operand.Length - 1));
                    }
                    else
                    {
                        OnResultClick(this, null);
                    }
                    break;

                case Key.LineFeed:
                case Key.Enter:
                    OnResultClick(this, null);
                    break;

                case Key.OemPlus:  // '='
                    OnResultClick(this, null);
                    break;

                case Key.Subtract:
                case Key.OemMinus:
                    if (operand.Length == 0)
                        miOperand.Header = "-";
                    break;

                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                    char key1 = (char)((e.Key - Key.D0) + '0');
                    if (char.IsDigit(key1))
                        UpdateResult(operand + key1);
                    break;

                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    char key2 = (char)((e.Key - Key.NumPad0) + '0');
                    if (char.IsDigit(key2))
                        UpdateResult(operand + key2);
                    break;

                case Key.OemComma:
                    if (!operand.Contains(","))
                        miOperand.Header = operand + ",";
                    break;

                case Key.Decimal:
                case Key.OemPeriod:
                    if (!operand.Contains("."))
                        miOperand.Header = operand + ".";
                    break;

                    //case Key.Multiply:
                    //case Key.Add:
                    //case Key.Divide:
            }
        }

        private void UpdateResult(string operand)
        {
            miOperand.Header = operand;
            double dop1, dop2, result;
            if (double.TryParse(Text, out dop1) && double.TryParse(operand, out dop2))
            {
                switch (operation)
                {
                    case EOperation.Add:
                        result = dop1 + dop2;
                        miResult.Header = result.ToString();
                        miResult.IsEnabled = true;
                        break;

                    case EOperation.Subtract:
                        result = dop1 - dop2;
                        miResult.Header = result.ToString();
                        miResult.IsEnabled = true;
                        break;

                    case EOperation.Multiply:
                        result = dop1 * dop2;
                        miResult.Header = result.ToString();
                        miResult.IsEnabled = true;
                        break;

                    case EOperation.Divide:
                        if (dop2 == 0.0)
                        {
                            miResult.Header = "";
                            miResult.IsEnabled = false;
                        }
                        else
                        {
                            result = dop1 / dop2;
                            if (double.IsInfinity(result) || double.IsNaN(result))
                            {
                                miResult.Header = "";
                                miResult.IsEnabled = false;
                            }
                            else
                            {
                                miResult.Header = result.ToString();
                                miResult.IsEnabled = true;
                            }
                        }
                        break;
                }
            }
            else
            {
                miResult.Header = "";
                miResult.IsEnabled = false;
            }
        }

        private void OnResultClick(object sender, RoutedEventArgs e)
        {
            string result = miResult.Header.ToString();
            if (result.Length > 0)
            {
                Text = result;
                CaretIndex = Text.Length;
            }

            menuMath.IsOpen = false;
        }

        private void OnMathClosed(object sender, RoutedEventArgs e)
        {
            WeakEventManager<ContextMenu, KeyEventArgs>.RemoveHandler(menuMath, "PreviewKeyDown", this.OnMathPreviewKeyDown);
            WeakEventManager<ContextMenu, RoutedEventArgs>.RemoveHandler(menuMath, "Closed", this.OnMathClosed);
            WeakEventManager<MenuItem, RoutedEventArgs>.RemoveHandler(miResult, "Click", this.OnResultClick);

            menuMath = null;
            miOperand = null;
            miResult = null;
        }
        #endregion Math
    }
}
