﻿// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectBindablePicker.cs" company="CodigoEdulis">
//    Código Edulis 2017
//    http://www.codigoedulis.es
//  </copyright>
//  <summary>
//     This implementation is a group of the offers of several persons along the network;
//     because of this, it is under Creative Common By License:
//     
//     You are free to:
// 
//     Share — copy and redistribute the material in any medium or format
//     Adapt — remix, transform, and build upon the material for any purpose, even commercially.
//     
//     The licensor cannot revoke these freedoms as long as you follow the license terms.
//     
//     Under the following terms:
//     
//     Attribution — You must give appropriate credit, provide a link to the license, and indicate if changes were made. You may do so in any reasonable manner, but not in any way that suggests the licensor endorses you or your use.
//     No additional restrictions — You may not apply legal terms or technological measures that legally restrict others from doing anything the license permits.
//  
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

namespace XamarinFormsDemo.CustomControls
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    using Xamarin.Forms;

    public class ObjectBindablePicker : Picker
    {
        public ObjectBindablePicker()
        {
            this.SelectedIndexChanged += this.OnSelectedIndexChanged;
        }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                return (string)this.GetValue(DisplayNameProperty);
            }

            set
            {
                this.SetValue(DisplayNameProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the items source.
        /// </summary>
        /// <value>
        ///     The items source.
        /// </value>
        public IList OriginalItemsSource
        {
            get
            {
                return (IList)this.GetValue(OriginalItemsSourceProperty);
            }

            set
            {
                this.SetValue(OriginalItemsSourceProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the selected item.
        /// </summary>
        /// <value>
        ///     The selected item.
        /// </value>
        public object SelectedObject
        {
            get
            {
                return this.GetValue(SelectedObjectProperty);
            }

            set
            {
                this.SetValue(SelectedObjectProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the selected value. The value of the property of your model object you want to save i.e in the View
        ///     Model.
        /// </summary>
        /// <value>
        ///     The selected value.
        /// </value>
        public object SelectedValue
        {
            get
            {
                return this.GetValue(SelectedValueProperty);
            }

            set
            {
                this.SetValue(SelectedValueProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the selected value path. The Property´s Name of uour model object
        /// </summary>
        /// <value>
        ///     The selected value path.
        /// </value>
        public string SelectedValuePath
        {
            get
            {
                return (string)this.GetValue(SelectedValuePathProperty);
            }

            set
            {
                this.SetValue(SelectedValuePathProperty, value);
            }
        }
        
        /// <summary>
        ///     Called when [items source changed].
        /// </summary>
        /// <param name="bindableObject">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemsSourceChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var picker = bindableObject as ObjectBindablePicker;

            if (picker == null)
            {
                return;
            }

            picker.Items.Clear();

            var list = newValue as IList;
            if (list == null)
            {
                return;
            }

            RefreshItems(list, picker);
        }

        private static void RefreshItems(IList list, ObjectBindablePicker picker)
        {
            foreach(var item in list)
            {
                if(string.IsNullOrEmpty(picker.DisplayName))
                {
                    picker.Items.Add(item.ToString());
                }
                else
                {
                    var prop = item.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, picker.DisplayName, StringComparison.OrdinalIgnoreCase));
                    if(prop != null)
                    {
                        picker.Items.Add(prop.GetValue(item).ToString());
                    }
                }
            }
        }

        /// <summary>
        ///     Called when [selected item changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldvalue">The oldvalue.</param>
        /// <param name="newvalue">The newvalue.</param>
        private static void OnSelectedObjectChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as ObjectBindablePicker;

            if (picker?.SelectedObject == null)
                return;

            if (picker.OriginalItemsSource.Contains(picker.SelectedObject))
            {
                picker.SelectedIndex = picker.OriginalItemsSource.IndexOf(picker.SelectedObject);
            }
            else
            {
                picker.SelectedObject = null;
            }
        }

        /// <summary>
        ///     Called when [selected index changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (this.SelectedIndex < 0 || this.SelectedIndex > this.Items.Count - 1)
            {
                this.SelectedObject = null;
            }
            else
            {
                var picker = sender as ObjectBindablePicker;
                if (picker == null)
                {
                    return;
                }

                this.SelectedObject = this.OriginalItemsSource[this.SelectedIndex];

                if (string.IsNullOrEmpty(this.SelectedValuePath))
                {
                    return;
                }

                var prop = this.SelectedObject.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, picker.SelectedValuePath, StringComparison.OrdinalIgnoreCase));
                if (prop != null)
                {
                    this.SelectedValue = prop.GetValue(this.SelectedObject);
                }
            }
        }

        public static BindableProperty OriginalItemsSourceProperty = BindableProperty.Create(nameof(OriginalItemsSource), typeof(IList), typeof(ObjectBindablePicker), default(IList), BindingMode.OneWay, null, OnItemsSourceChanged);

        public static BindableProperty SelectedObjectProperty = BindableProperty.Create(nameof(SelectedObject), typeof(object), typeof(ObjectBindablePicker), default(object), BindingMode.TwoWay, null, OnSelectedObjectChanged);

        public static BindableProperty SelectedValueProperty = BindableProperty.Create(nameof(SelectedValue), typeof(object), typeof(ObjectBindablePicker), default(object), BindingMode.OneWayToSource);

        public static BindableProperty SelectedValuePathProperty = BindableProperty.Create(nameof(SelectedValuePath), typeof(string), typeof(ObjectBindablePicker), string.Empty);

        public static BindableProperty DisplayNameProperty = BindableProperty.Create(nameof(DisplayName), typeof(string), typeof(ObjectBindablePicker), string.Empty, BindingMode.Default);
    }
}