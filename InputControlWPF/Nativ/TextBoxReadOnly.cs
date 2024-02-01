//-----------------------------------------------------------------------
// <copyright file="TextBoxReadOnly.cs" company="NRM Netzdienste Rhein-Main GmbH">
//     Class: TextBoxReadOnly
//     Copyright © NRM Netzdienste Rhein-Main GmbH 2023
// </copyright>
//
// <author>DeveloperName - NRM Netzdienste Rhein-Main GmbH</author>
// <email>DeveloperName@nrm-netzdienste.de</email>
// <date>29.11.2023 14:31:11</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace InputControlWPF.InputControls
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Shapes = System.Windows.Shapes;

    public class TextBoxReadOnly : TextBox
    {
        /* Definition der Path Geometry Icon für Kontextmenü */
        private const string ICON_COPY = "M19,21H8V7H19M19,5H8A2,2 0 0,0 6,7V21A2,2 0 0,0 8,23H19A2,2 0 0,0 21,21V7A2,2 0 0,0 19,5M16,1H4A2,2 0 0,0 2,3V17H4V3H16V1Z";
        private const string ICON_DELETE = "M9,3V4H4V6H5V19A2,2 0 0,0 7,21H17A2,2 0 0,0 19,19V6H20V4H15V3H9M7,6H17V19H7V6M9,8V17H11V8H9M13,8V17H15V8H13Z";
        private const string ICON_CLOCK = "M12,20A8,8 0 0,0 20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22C6.47,22 2,17.5 2,12A10,10 0 0,1 12,2M12.5,7V12.25L17,14.92L16.25,16.15L11,13V7H12.5Z";

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxReadOnly"/> class.
        /// </summary>
        public TextBoxReadOnly()
        {
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 25;
            this.FontSize = 14;
            this.FontFamily = new FontFamily("Arial");
            this.IsReadOnly = false;
            this.Focusable = true;
            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
        }

        public static readonly DependencyProperty ReadOnlyColorProperty =
            DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TextBoxReadOnly), new PropertyMetadata(Brushes.LightYellow));

        /*
        private static void ReadOnlyColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uIElement)
            {
                var uiElement = (TextBox)uIElement;
                //uiElement.Background = ;
            }
        }
        */

        public Brush ReadOnlyColor
        {
            get { return (Brush)GetValue(ReadOnlyColorProperty); }
            set { SetValue(ReadOnlyColorProperty, value); }
        }

        public static readonly DependencyProperty SetBorderProperty =
            DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxReadOnly), new PropertyMetadata(true));

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
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

        private Style SetTriggerFunction()
        {
            Style textBoxStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            textBoxStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsMouseOver = false */
            triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = false;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyColor });

            /* Trigger zum Style hinzufügen */
            textBoxStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            textBoxStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = false */
            triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = false;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyColor });
            textBoxStyle.Triggers.Add(triggerIsFocused);

            return textBoxStyle;
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

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
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
        private Shapes.Path GetPathGeometry(string iconString, Color iconColor)
        {
            var path = new Shapes.Path
            {
                Height = 24,
                Width = 24,
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
        private Shapes.Path GetPathGeometry(string iconString)
        {
            return GetPathGeometry(iconString,Colors.Blue);
        }

        /// <summary>
        /// Convert Geometry to ImageSource, Draws the Geometry on a bitmap surface and centers it.
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="TargetSize"></param>
        /// <returns></returns>
        private ImageSource GeometryToImageSource(Geometry geometry, int TargetSize)
        {
            var rect = geometry.GetRenderBounds(new Pen(Brushes.Black, 0));

            var bigger = rect.Width > rect.Height ? rect.Width : rect.Height;
            var scale = TargetSize / bigger;

            Geometry scaledGeometry = Geometry.Combine(geometry, geometry, GeometryCombineMode.Intersect, new ScaleTransform(scale, scale));
            rect = scaledGeometry.GetRenderBounds(new Pen(Brushes.Black, 0));

            Geometry transformedGeometry = Geometry.Combine(scaledGeometry, scaledGeometry, GeometryCombineMode.Intersect, new TranslateTransform(((TargetSize - rect.Width) / 2) - rect.Left, ((TargetSize - rect.Height) / 2) - rect.Top));

            RenderTargetBitmap bmp = new RenderTargetBitmap(TargetSize, TargetSize, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual viz = new DrawingVisual();
            using (DrawingContext dc = viz.RenderOpen())
            {
                dc.DrawGeometry(Brushes.Black, null, transformedGeometry);
            }

            bmp.Render(viz);

            var mem = new MemoryStream();
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(bmp));
            pngEncoder.Save(mem);
            var itm = GetImg(mem);
            return itm;
        }

        private BitmapImage GetImg(MemoryStream ms)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = ms;
            bmp.EndInit();

            return bmp;
        }
    }
}
