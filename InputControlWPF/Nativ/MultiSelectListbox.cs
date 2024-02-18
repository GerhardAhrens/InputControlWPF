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

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MultiSelectListbox), new PropertyMetadata(false, OnIsReadOnlyChanged));
        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(MultiSelectListbox), new PropertyMetadata(null, OnSelectedItemsListChange));

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectListbox"/> class.
        /// </summary>
        public MultiSelectListbox()
        {
            this.SelectionMode = SelectionMode.Extended;
            this.BorderBrush = Brushes.Green;
            this.BorderThickness = new Thickness(1);

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();

            WeakEventManager<MultiSelectListbox, RoutedEventArgs>.AddHandler(this, "Loaded", OnLoaded);
            WeakEventManager<MultiSelectListbox, SelectionChangedEventArgs>.AddHandler(this, "SelectionChanged", OnSelectionChanged);

        }

        static MultiSelectListbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectListbox), new FrameworkPropertyMetadata(typeof(ListBox)));
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

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
                {
                    castValue.Add(Convert.ChangeType(o, boundType));
                }

                obj.SetValue(SelectedItemsListProperty, castValue);
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
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
                        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(StackPanel))
                        { 
                            Name = "ItemTemplate" 
                        };

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

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MultiSelectListbox lb = sender as MultiSelectListbox;
            if (lb != null)
            {
                IList selectedItemsList = GetSelectedItemsList(lb);
                if (lb.SelectedItems == null)
                {
                    return;
                }
                if (selectedItemsList == null)
                {
                    selectedItemsList = new List<object>();
                }

                foreach (object addedItem in e.AddedItems)
                {
                    selectedItemsList.Add(addedItem);
                    ListBoxItem lbi = lb.ItemContainerGenerator.ContainerFromItem(addedItem) as ListBoxItem;
                    FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = true;
                }

                foreach (object removedItem in e.RemovedItems)
                {
                    selectedItemsList.Remove(removedItem);
                    ListBoxItem lbi = lb.ItemContainerGenerator.ContainerFromItem(removedItem) as ListBoxItem;
                    FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = false;
                }

                SetSelectedItemsList(lb, selectedItemsList);
            }
        }

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (MultiSelectListbox)d;
                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.IsEnabled = false;
                    }
                    else
                    {
                        control.IsEnabled = true;
                    }
                }
            }
        }

        private static void OnSelectedItemsListChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (MultiSelectListbox)d;
                if (e.NewValue.GetType().IsGenericType == true)
                {
                    foreach (string item in control.ItemsSource)
                    {
                        ListBoxItem lbi = control.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                        FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = true;
                    }
                }
            }
        }

        private static void CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListBoxItem lbi = GetParentOfType<ListBoxItem>(cb);
            lbi.IsSelected = cb.IsChecked.Value;
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
