using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
namespace Gest.Behaviors_Controls
{
    class Behavior_Picker:Behavior<Picker>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached(propertyName: "Command", returnType: typeof(ICommand), declaringType: typeof(Behavior_Picker), defaultValue: null);

        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }


        protected override void OnAttachedTo(Picker bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SelectedIndexChanged += OnSelectedIndexChanged;
            bindable.BindingContextChanged += BindingContextEntry_changed;
        }

        public void OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            var objet_control = sender as Picker;
            Command.Execute(null);
        }


        public void BindingContextEntry_changed(Object sender, EventArgs e)
        {
            BindingContext = (sender as BindableObject).BindingContext;
        }

        protected override void OnDetachingFrom(Picker bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SelectedIndexChanged -= OnSelectedIndexChanged;
            bindable.BindingContextChanged -= BindingContextEntry_changed;
        }

    }
}
