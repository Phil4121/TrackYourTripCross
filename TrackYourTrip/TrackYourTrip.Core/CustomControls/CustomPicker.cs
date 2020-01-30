using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using Xamarin.Forms;

namespace TrackYourTrip.Core.CustomControls
{
    public class CustomPicker : Picker
    {
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(CustomPicker), true, BindingMode.TwoWay);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        Boolean _disableNestedCalls;

        public static new readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(CustomPicker),
                null, propertyChanged: OnItemsSourceChanged);

        public static new readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create("SelectedItem", typeof(Object), typeof(CustomPicker),
                null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        public static readonly BindableProperty SelectedValueProperty =
            BindableProperty.Create("SelectedValue", typeof(Object), typeof(CustomPicker),
                null, BindingMode.TwoWay, propertyChanged: OnSelectedValueChanged);

        public String DisplayMemberPath { get; set; }

        public new IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public new Object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set
            {
                if (SelectedItem != value)
                {
                    SetValue(SelectedItemProperty, value);
                    InternalSelectedItemChanged();
                }
            }
        }

        public Object SelectedValue
        {
            get => GetValue(SelectedValueProperty);
            set
            {
                SetValue(SelectedValueProperty, value);
                InternalSelectedValueChanged();
            }
        }

        public String SelectedValuePath { get; set; }

        public CustomPicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        void InstanceOnItemsSourceChanged(Object oldValue, Object newValue)
        {
            _disableNestedCalls = true;
            Items.Clear();

            INotifyCollectionChanged oldCollectionINotifyCollectionChanged = oldValue as INotifyCollectionChanged;
            if (oldCollectionINotifyCollectionChanged != null)
            {
                oldCollectionINotifyCollectionChanged.CollectionChanged -= ItemsSource_CollectionChanged;
            }

            INotifyCollectionChanged newCollectionINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (newCollectionINotifyCollectionChanged != null)
            {
                newCollectionINotifyCollectionChanged.CollectionChanged += ItemsSource_CollectionChanged;
            }

            if (!Equals(newValue, null))
            {
                bool hasDisplayMemberPath = !String.IsNullOrWhiteSpace(DisplayMemberPath);

                foreach (object item in (IEnumerable)newValue)
                {
                    if (hasDisplayMemberPath)
                    {
                        Type type = item.GetType();
                        PropertyInfo prop = type.GetRuntimeProperty(DisplayMemberPath);
                        Items.Add(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        Items.Add(item.ToString());
                    }
                }

                SelectedIndex = -1;
                _disableNestedCalls = false;

                if (SelectedItem != null)
                {
                    InternalSelectedItemChanged();
                }
                else if (hasDisplayMemberPath && SelectedValue != null)
                {
                    InternalSelectedValueChanged();
                }
            }
            else
            {
                _disableNestedCalls = true;
                SelectedIndex = -1;
                SelectedItem = null;
                SelectedValue = null;
                _disableNestedCalls = false;
            }
        }

        void InternalSelectedItemChanged()
        {
            if (_disableNestedCalls)
            {
                return;
            }

            int selectedIndex = -1;
            Object selectedValue = null;
            if (ItemsSource != null)
            {
                int index = 0;
                bool hasSelectedValuePath = !String.IsNullOrWhiteSpace(SelectedValuePath);
                foreach (object item in ItemsSource)
                {
                    if (item != null && item.Equals(SelectedItem))
                    {
                        selectedIndex = index;
                        if (hasSelectedValuePath)
                        {
                            Type type = item.GetType();
                            PropertyInfo prop = type.GetRuntimeProperty(SelectedValuePath);
                            selectedValue = prop.GetValue(item);
                        }
                        break;
                    }
                    index++;
                }
            }
            _disableNestedCalls = true;
            SelectedValue = selectedValue;
            SelectedIndex = selectedIndex;
            _disableNestedCalls = false;
        }

        void InternalSelectedValueChanged()
        {
            if (_disableNestedCalls)
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(SelectedValuePath))
            {
                IsValid = false;
                return;
            }

            int selectedIndex = -1;
            IsValid = false;

            Object selectedItem = null;
            bool hasSelectedValuePath = !String.IsNullOrWhiteSpace(SelectedValuePath);
            if (ItemsSource != null && hasSelectedValuePath)
            {
                int index = 0;
                foreach (object item in ItemsSource)
                {
                    if (item != null)
                    {
                        Type type = item.GetType();
                        PropertyInfo prop = type.GetRuntimeProperty(SelectedValuePath);
                        if (Object.Equals(prop.GetValue(item), SelectedValue))
                        {
                            selectedIndex = index;
                            selectedItem = item;
                            IsValid = true;
                            break;
                        }
                    }

                    index++;
                }
            }
            _disableNestedCalls = true;
            SelectedItem = selectedItem;
            SelectedIndex = selectedIndex;
            _disableNestedCalls = false;
        }

        void ItemsSource_CollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            bool hasDisplayMemberPath = !String.IsNullOrWhiteSpace(DisplayMemberPath);
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (hasDisplayMemberPath)
                    {
                        Type type = item.GetType();
                        PropertyInfo prop = type.GetRuntimeProperty(DisplayMemberPath);
                        Items.Add(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        Items.Add(item.ToString());
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.NewItems)
                {
                    if (hasDisplayMemberPath)
                    {
                        Type type = item.GetType();
                        PropertyInfo prop = type.GetRuntimeProperty(DisplayMemberPath);
                        Items.Remove(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        Items.Remove(item.ToString());
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (object item in e.NewItems)
                {
                    if (hasDisplayMemberPath)
                    {
                        Type type = item.GetType();
                        PropertyInfo prop = type.GetRuntimeProperty(DisplayMemberPath);
                        Items.Remove(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        int index = Items.IndexOf(item.ToString());
                        if (index > -1)
                        {
                            Items[index] = item.ToString();
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Items.Clear();
                if (e.NewItems != null)
                {
                    foreach (object item in e.NewItems)
                    {
                        if (hasDisplayMemberPath)
                        {
                            Type type = item.GetType();
                            PropertyInfo prop = type.GetRuntimeProperty(DisplayMemberPath);
                            Items.Remove(prop.GetValue(item).ToString());
                        }
                        else
                        {
                            int index = Items.IndexOf(item.ToString());
                            if (index > -1)
                            {
                                Items[index] = item.ToString();
                            }
                        }
                    }
                }
                else
                {
                    _disableNestedCalls = true;
                    SelectedItem = null;
                    SelectedIndex = -1;
                    SelectedValue = null;
                    _disableNestedCalls = false;
                }
            }
        }

        static void OnItemsSourceChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (Equals(newValue, null) && Equals(oldValue, null))
            {
                return;
            }

            CustomPicker picker = (CustomPicker)bindable;
            picker.InstanceOnItemsSourceChanged(oldValue, newValue);
        }

        void OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (_disableNestedCalls)
            {
                return;
            }

            if (SelectedIndex < 0 || ItemsSource == null || !ItemsSource.GetEnumerator().MoveNext())
            {
                _disableNestedCalls = true;
                if (SelectedIndex != -1)
                {
                    SelectedIndex = -1;
                }
                SelectedItem = null;
                SelectedValue = null;
                _disableNestedCalls = false;
                return;
            }

            _disableNestedCalls = true;

            int index = 0;
            bool hasSelectedValuePath = !String.IsNullOrWhiteSpace(SelectedValuePath);
            foreach (object item in ItemsSource)
            {
                if (index == SelectedIndex)
                {
                    SelectedItem = item;
                    if (hasSelectedValuePath)
                    {
                        Type type = item.GetType();
                        PropertyInfo prop = type.GetRuntimeProperty(SelectedValuePath);
                        SelectedValue = prop.GetValue(item);
                    }

                    break;
                }
                index++;
            }

            _disableNestedCalls = false;
        }

        static void OnSelectedItemChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            CustomPicker boundPicker = (CustomPicker)bindable;
            boundPicker.ItemSelected?.Invoke(boundPicker, new SelectedItemChangedEventArgs(newValue));
            boundPicker.InternalSelectedItemChanged();
        }

        static void OnSelectedValueChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            CustomPicker boundPicker = (CustomPicker)bindable;
            boundPicker.InternalSelectedValueChanged();
        }
    }
}
