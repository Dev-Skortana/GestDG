using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Gest.Behaviors_Controls
{
    class Behavior_EntryTextChanged:Behavior<Entry>
    {
        /* Attention tres important !!! ->  le nom de la proprieter Bindable et la proprieter icommand doivent respecter des regles de convention pour faire fonctionner le behavior.*/
        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached(propertyName: "Command",returnType: typeof(ICommand),declaringType: typeof(Behavior_EntryTextChanged),defaultValue:null);

        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }


        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged +=textchange;
            bindable.BindingContextChanged += BindingContextEntry_changed;
        }

        private void textchange(Object sender,EventArgs e)
        {      
            var objet_control = sender as Entry;
            Command.Execute(objet_control.Text);
        }

        public void BindingContextEntry_changed(Object sender, EventArgs e)
        {
            BindingContext = (sender as BindableObject).BindingContext;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -=textchange;
            bindable.BindingContextChanged -= BindingContextEntry_changed;
        }

       
    }
}
