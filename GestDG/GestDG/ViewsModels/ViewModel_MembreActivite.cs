using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using GestDG.Models;
using GestDG.Services.Classes;
using GestDG.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace GestDG.ViewsModels
{
    class ViewModel_MembreActivite : BindableBase,INavigationAware
    {
        public string title { get; set; } = "Pages regroupent les activité des membres.";
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Activite service_activite;

        public ViewModel_MembreActivite(INavigationService _service_navigation, IService_Membre _service_membre,IService_Activite _service_activite)
        {
            this. service_membre = _service_membre;
            this.service_activite = _service_activite;
            this.service_navigation = _service_navigation;
            load();
        }

        public Command cmd 
        {
            get
            {
                return new Command<String>(async(el)=> {
                    await load(pseudo:el);
                });
            }
            
        }

        private List<Membre> _membres;
        public List<Membre> membres {
            get
            {
                return _membres;
            }
            set
            {
                SetProperty(ref _membres,value);
            }
        }

        private async Task load(String pseudo="",String libelle_activite="")
        {
            membres =(List<Membre>)await service_membre.GetList(pseudo);
            foreach (var item in membres)
            {
                item.liste_activites =(from el in  (List<Activite>)await service_activite.GetList(libelle_activite) where el.membre_pseudo==item.pseudo select el).ToList();
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
