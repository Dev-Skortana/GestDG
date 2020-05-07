using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Gest.Behaviors_Controls
{
    class Behavior_Pikertime:Behavior<TimePicker>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached(propertyName: "Command", returnType: typeof(ICommand), declaringType: typeof(Behavior_Pikertime), defaultValue: null);

        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }


        protected override void OnAttachedTo(TimePicker bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.PropertyChanged += Pickertime_SelectedIndexChanged;
            bindable.BindingContextChanged += BindingContextEntry_changed;
        }

        public void Pickertime_SelectedIndexChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="Time")
            {
                var objet_control = sender as TimePicker;
                Command.Execute(objet_control.Time);
            }
        }


        public void BindingContextEntry_changed(Object sender, EventArgs e)
        {
            BindingContext = (sender as BindableObject).BindingContext;
        }

        protected override void OnDetachingFrom(TimePicker bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.PropertyChanged -= Pickertime_SelectedIndexChanged;
            bindable.BindingContextChanged -= BindingContextEntry_changed;
        }


    }
}
