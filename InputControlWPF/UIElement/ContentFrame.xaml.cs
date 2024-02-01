namespace InputControlWPF.InputControls
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für ContentFrame.xaml
    /// </summary>
    public partial class ContentFrame : UserControl
    {
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ContentFrame), new PropertyMetadata("<TODO>"));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty ContentElementProperty =
            DependencyProperty.Register("ContentElement", typeof(Control), typeof(ContentFrame), new PropertyMetadata(null));

        public Control ContentElement
        {
            get { return (Control)GetValue(ContentElementProperty); }
            set { SetValue(ContentElementProperty, value); }
        }

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(ContentFrame), new PropertyMetadata(Brushes.Black));

        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly DependencyProperty BorderWidthProperty =
            DependencyProperty.Register("BorderWidth", typeof(Thickness), typeof(ContentFrame), new PropertyMetadata(new Thickness(1)));

        public Thickness BorderWidth
        {
            get { return (Thickness)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentColorProperty =
            DependencyProperty.Register("ContentColor", typeof(Brush), typeof(ContentFrame), new PropertyMetadata(Brushes.Transparent));

        public Brush ContentColor
        {
            get { return (Brush)GetValue(ContentColorProperty); }
            set { SetValue(ContentColorProperty, value); }
        }

        public static readonly DependencyProperty FrameWidthProperty =
            DependencyProperty.Register("FrameWidth", typeof(double), typeof(ContentFrame), new PropertyMetadata(100.00));

        public double FrameWidth
        {
            get { return (double)GetValue(FrameWidthProperty); }
            set { SetValue(FrameWidthProperty, value); }
        }

        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(ContentFrame), new PropertyMetadata(110.00));

        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        public ContentFrame()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
    }
}
