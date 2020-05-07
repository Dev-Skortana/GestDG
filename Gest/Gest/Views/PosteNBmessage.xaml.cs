using Gest.ViewModels;
using Gest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using System.ComponentModel;
using Gest.Behaviors_Controls;

namespace Gest.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PosteNBmessage : ContentPage
	{
		public PosteNBmessage ()
		{
			InitializeComponent ();
            picker_controlessaisies.SelectedIndex = 0;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);
        }

        private void CarouselViewControl_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            var dictionnaire =(IDictionary) (sender as CarouselView.FormsPlugin.Abstractions.CarouselViewControl)?.ItemsSource;
            var liste_membres = ((IEnumerable)dictionnaire.Keys).Cast<Gest.Models.Membre>().ToList();
            if ((liste_membres!=null) & (liste_membres.Count != 0))
            {
                ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_sync.Execute(liste_membres[e.NewValue]);
            }
        }

        private void Picker_switchsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_switch_source.Execute(null);
        }

        private void Picker_switchcontrole_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker controle_picker = sender as Picker;
            if (controle_picker.SelectedItem.ToString() == "Text")
            {
                zone_saisi_text.IsVisible = true;

                zone_saisi_date.IsVisible = false;
                zone_saisi_time.IsVisible = false;
                button_search_datewithtime.IsVisible = false;

            }
            else if (controle_picker.SelectedItem.ToString() == "Date")
            {
                zone_saisi_date.DateSelected += (zone_saisi_date.Behaviors[0] as Behavior_Datepiker).Pickerdate_SelectedIndexChanged;
                zone_saisi_date.DateSelected -= this.Pickerdate_SelectedIndexChanged;
                zone_saisi_date.IsVisible = true;

                zone_saisi_text.IsVisible = false;
                zone_saisi_time.IsVisible = false;
                button_search_datewithtime.IsVisible = false;
            }
            else if (controle_picker.SelectedItem.ToString() == "Heure")
            {
                zone_saisi_time.PropertyChanged += (zone_saisi_time.Behaviors[0] as Behavior_Pikertime).Pickertime_SelectedIndexChanged;
                zone_saisi_time.PropertyChanged -= this.Pickertime_SelectedIndexChanged;
                zone_saisi_time.IsVisible = true;

                zone_saisi_text.IsVisible = false;
                zone_saisi_date.IsVisible = false;
                button_search_datewithtime.IsVisible = false;

            }
            else if (controle_picker.SelectedItem.ToString() == "Date et heure")
            {
                zone_saisi_date.DateSelected -= (zone_saisi_date.Behaviors[0] as Behavior_Datepiker).Pickerdate_SelectedIndexChanged;
                zone_saisi_date.DateSelected += this.Pickerdate_SelectedIndexChanged;
                zone_saisi_date.IsVisible = true;
                zone_saisi_time.PropertyChanged -= (zone_saisi_time.Behaviors[0] as Behavior_Pikertime).Pickertime_SelectedIndexChanged;
                zone_saisi_time.PropertyChanged += this.Pickertime_SelectedIndexChanged;
                zone_saisi_time.IsVisible = true;
                button_search_datewithtime.IsVisible = true;

                zone_saisi_text.IsVisible = false;
            }

        }

        private void Pickerdate_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (BindingContext as PosteNBmessageViewModel != null)
            {
                ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_update_dateandtime.Execute(((DatePicker)sender).Date);
            }
        }

        private void Pickertime_SelectedIndexChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                if (BindingContext as PosteNBmessageViewModel != null)
                {
                    ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_update_dateandtime.Execute(((TimePicker)sender).Time);
                }
            }
        }

    }
}