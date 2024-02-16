//-----------------------------------------------------------------------
// <copyright file="MultiSelectListbox.cs" company="Lifeprojects.de">
//     Class: MultiSelectListbox
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>16.02.2024 14:38:09</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace InputControlWPF.InputControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    public class MultiSelectListbox : ListBox
    {
        private Type boundType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectListbox"/> class.
        /// </summary>
        public MultiSelectListbox()
        {
            this.Loaded += MultiSelectListbox_Loaded;
            this.SelectionChanged += MultiSelectListbox_SelectionChanged;
        }

        static MultiSelectListbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectListbox), new FrameworkPropertyMetadata(typeof(ListBox)));
        }

        ~MultiSelectListbox()
        {
            this.Loaded -= MultiSelectListbox_Loaded;
            this.SelectionChanged -= MultiSelectListbox_SelectionChanged;
        }

        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(MultiSelectListbox));

        public static IList GetSelectedItemsList(DependencyObject obj)
        {
            return obj.GetValue(SelectedItemsListProperty) as IList;
        }

        public static void SetSelectedItemsList(DependencyObject obj, IList value)
        {
            Type boundType = (obj as MultiSelectListbox).boundType;
            if (boundType != null)
            {
                Type genericType = typeof(List<>);
                Type returnType = genericType.MakeGenericType(boundType);

                IList castValue = System.Activator.CreateInstance(returnType) as IList;
                foreach (object o in value)
                    castValue.Add(Convert.ChangeType(o, boundType));

                obj.SetValue(SelectedItemsListProperty, castValue);
            }
        }

        private static void MultiSelectListbox_Loaded(object sender, RoutedEventArgs e)
        {
            MultiSelectListbox lb = sender as MultiSelectListbox;
            if (lb != null)
            {
                BindingExpression be = lb.GetBindingExpression(SelectedItemsListProperty);
                if (be != null)
                {
                    FrameworkElement rs = be.ResolvedSource as FrameworkElement;
                    if (rs != null)
                    {
                        PropertyInfo lpi = rs.DataContext.GetType().GetProperty(be.ResolvedSourcePropertyName);
                        lb.boundType = lpi.PropertyType.GetGenericArguments().FirstOrDefault();

                        DataTemplate dt = new DataTemplate(typeof(MultiSelectListbox));
                        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(StackPanel)) { Name = "ItemTemplate" };
                        fef.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                        FrameworkElementFactory cb = new FrameworkElementFactory(typeof(CheckBox));
                        //CheckBox.MarginProperty = 
                        cb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(CheckedChanged));
                        cb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(CheckedChanged));
                        fef.AppendChild(cb);

                        FrameworkElementFactory tb = new FrameworkElementFactory(typeof(TextBlock));
                        tb.SetBinding(TextBlock.TextProperty, new Binding(lb.DisplayMemberPath));
                        fef.AppendChild(tb);

                        dt.VisualTree = fef;

                        lb.DisplayMemberPath = null;
                        lb.ItemTemplate = dt;

                        if (be.ParentBinding.Mode != BindingMode.TwoWay)
                        {
                            throw new NotSupportedException("SelectedItemsList Binding Mode must be TwoWay");
                        }
                    }
                }
            }
        }

        private static void MultiSelectListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MultiSelectListbox lb = sender as MultiSelectListbox;
            if (lb != null)
            {
                IList SelectedItemsList = GetSelectedItemsList(lb);
                if (lb.SelectedItems == null) return;
                if (SelectedItemsList == null) SelectedItemsList = new List<object>();

                foreach (object addedItem in e.AddedItems)
                {
                    SelectedItemsList.Add(addedItem);
                    ListBoxItem lbi = lb.ItemContainerGenerator.ContainerFromItem(addedItem) as ListBoxItem;
                    FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = true;
                }

                foreach (object removedItem in e.RemovedItems)
                {
                    SelectedItemsList.Remove(removedItem);
                    ListBoxItem lbi = lb.ItemContainerGenerator.ContainerFromItem(removedItem) as ListBoxItem;
                    FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = false;
                }

                SetSelectedItemsList(lb, SelectedItemsList);
            }
        }

        private static void CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListBoxItem lbi = GetParentOfType<ListBoxItem>(cb);
            lbi.IsSelected = cb.IsChecked.Value;
        }

        private static T GetParentOfType<T>(DependencyObject control) where T : System.Windows.DependencyObject
        {
            DependencyObject ParentControl = control;

            do
                ParentControl = VisualTreeHelper.GetParent(ParentControl);
            while (ParentControl != null && !(ParentControl is T));

            return ParentControl as T;
        }

        private static List<T> FindChildrenOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> Children = new List<T>();
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        Children.Add(child as T);
                    Children.AddRange(FindChildrenOfType<T>(child));
                }
            }
            return Children;
        }
    }
}
