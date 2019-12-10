using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
namespace GestDG.Behaviors_Controls
{
    class Behavior_Datepiker:Behavior<DatePicker>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached(propertyName: "Command", returnType: typeof(ICommand), declaringType: typeof(Behavior_Datepiker), defaultValue: null);

        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }


        protected override void OnAttachedTo(DatePicker bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.DateSelected += datechange;
            bindable.BindingContextChanged += BindingContextEntry_changed;
        }

        private void datechange(Object sender, EventArgs e)
        {
            var objet_control = sender as DatePicker;
            Command.Execute(objet_control.Date);
        }

        public void BindingContextEntry_changed(Object sender, EventArgs e)
        {
            BindingContext = (sender as DatePicker).BindingContext;
        }

        protected override void OnDetachingFrom(DatePicker bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.DateSelected -= datechange;
            bindable.BindingContextChanged -= BindingContextEntry_changed;
        }

    }
}
