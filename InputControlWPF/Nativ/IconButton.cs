//-----------------------------------------------------------------------
// <copyright file="IconButton.cs" company="Lifeprojects.de">
//     Class: IconButton
//     Copyright © Gerhard Ahrens, 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>29.04.2023</date>
//
// <summary>Class for UI Control Button</summary>
//-----------------------------------------------------------------------

namespace InputControlWPF.InputControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using InputControlWPF.NativCore;

    public sealed class IconButton : Button
    {
        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }

        public IconButton()
        {
            string styleTextButton = new StyleText().Add("Button", this.ButtonFlatCoreStyle()).Value;
            //Style buttonStyle = XAMLBuilder<Style>.GetStyle(styleTextButton);
            //this.Style = buttonStyle;
        }

        #region PathData

        public static readonly DependencyProperty PathDataProperty =
            DependencyProperty.Register(nameof(PathData), typeof(Geometry), typeof(IconButton), new PropertyMetadata(Geometry.Empty));

        public Geometry PathData
        {
            get { return (Geometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        #endregion

        #region TextProperty
        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconButton),
                new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region Orientation

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(IconButton),
                new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        #endregion

        #region TextVisibility

        public static readonly DependencyProperty TextVisibilityProperty =
            DependencyProperty.Register(nameof(TextVisibility), typeof(Visibility), typeof(IconButton),
                new PropertyMetadata(default(Visibility)));

        public Visibility TextVisibility
        {
            get { return (Visibility)GetValue(TextVisibilityProperty); }
            set { SetValue(TextVisibilityProperty, value); }
        }

        #endregion

        #region IconVisibility

        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register(nameof(IconVisibility), typeof(Visibility), typeof(IconButton),
                new PropertyMetadata(default(Visibility)));

        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }

        #endregion

        private string ButtonFlatCoreStyle()
        {
            string result = string.Empty;

            result = "<Setter Property=\"Orientation\" Value=\"Horizontal\" />\r\n" +
                     "<Setter Property=\"Height\" Value=\"24\" />\r\n" +
                     "<Setter Property=\"Background\" Value=\"Transparent\" />\r\n" +
                     "<Setter Property=\"BorderThickness\" Value=\"0\" />\r\n" +
                     "<Setter Property=\"Cursor\" Value=\"Hand\" />\r\n" +
                     "<Setter Property=\"Template\">\r\n" +
                     "<Setter.Value>\r\n" +
                     "           <ControlTemplate TargetType=\"{x:Type Button}\">\r\n" +
                     "                    <Border\r\n" +
                     "                        Background=\"{TemplateBinding Property=Background}\"\r\n" +
                     "                        BorderBrush=\"{TemplateBinding Property=BorderBrush}\"\r\n" +
                     "                        BorderThickness=\"{TemplateBinding Property=BorderThickness}\">\r\n" +
                     "                        <Viewbox Stretch=\"Uniform\">\r\n" +
                     "                        <StackPanel\r\n" +
                     "                                Height=\"{TemplateBinding Property=Height}\"\r\n" +
                     "                                HorizontalAlignment=\"Center\"\r\n" +
                     "                                VerticalAlignment=\"Center\"\r\n" +
                     "                                Background=\"{TemplateBinding Property=Background}\"\r\n" +
                     "                                Orientation=\"{TemplateBinding Property=Orientation}\">\r\n" +
                     "                                <Viewbox\r\n" +
                     "                                    Margin=\"2,5\"\r\n" +
                     "                                    HorizontalAlignment=\"Left\"\r\n" +
                     "                                    VerticalAlignment=\"Bottom\"\r\n" +
                     "                                    Stretch=\"Uniform\"\r\n" +
                     "                                    Visibility=\"{TemplateBinding Property=IconVisibility}\">\r\n" +
                     "                                    <Path\r\n" +
                     "                                        Data=\"{TemplateBinding Property=PathData}\"\r\n" +
                     "                                        Fill=\"{TemplateBinding Property=Foreground}\"\r\n" +
                     "                                        Stretch=\"Uniform\" />\r\n" +
                     "                                </Viewbox>\r\n\r\n" +
                     "                                <ContentControl\r\n" +
                     "                                    Margin=\"2\"\r\n" +
                     "                                    HorizontalAlignment=\"Center\"\r\n" +
                     "                                    VerticalAlignment=\"Bottom\"\r\n" +
                     "                                    Content=\"{TemplateBinding Property=Text}\"\r\n" +
                     "                                    Visibility=\"{TemplateBinding Property=TextVisibility}\" />\r\n" +
                     "                            </StackPanel>\r\n" +
                     "                        </Viewbox>\r\n" +
                     "                    </Border>\r\n" +
                     "                </ControlTemplate>\r\n" +
                     "            </Setter.Value>\r\n" +
                     "        </Setter>\r\n" +
                     "        <Style.Triggers>\r\n" +
                     "            <Trigger Property=\"IsMouseOver\" Value=\"True\">\r\n" +
                     "                <Setter Property=\"BorderBrush\" Value=\"Transparent\" />\r\n" +
                     "                <Setter Property=\"Opacity\" Value=\"0.5\" />\r\n" +
                     "            </Trigger>\r\n" +
                     "            <Trigger Property=\"IsMouseOver\" Value=\"False\">\r\n" +
                     "                <Setter Property=\"BorderBrush\" Value=\"Transparent\" />\r\n" +
                     "            </Trigger>\r\n" +
                     "</Style.Triggers>";

            return result;
        }
    }
}
