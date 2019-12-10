using System;
using System.Collections.Generic;
using System.Text;
using Recherche_donnees_GESTDG.enumeration;
namespace Recherche_donnees_GESTDG.Formats.Interface
{
   public interface ICreate_with_format
    {
        Boolean get_format(Object element);
        String Create_condition(String champ,Object valeur,Enumerations_recherches.methodes_recherches methode_recherche);
    }
}
