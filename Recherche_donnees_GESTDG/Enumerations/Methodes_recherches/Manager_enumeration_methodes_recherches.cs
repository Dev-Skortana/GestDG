using Recherche_donnees_GESTDG.enumeration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using System.Linq;

namespace Recherche_donnees_GESTDG{
    public static class Manager_enumeration_methodes_recherches
    {
        public static List<String> getmethodes_recherches<T>(SQLiteConnection connection_sqlite, String champ)
        {
            List<String> resultat = new List<string>();
            Type type_champ = connection_sqlite.GetMapping<T>().FindColumn(champ).ColumnType;
            if (type_champ == typeof(String))
            {
                resultat = Enumeration_methodes_recherches_string.get_liste_methodesrecherches();
            }
            else if (type_champ == typeof(int) || type_champ == typeof(DateTime) || type_champ == typeof(TimeSpan))
            {
                resultat = Enumeration_methodes_recherches_numbers.get_liste_methodesrecherches();
            }
            else
            {
                resultat = Enumerations_methodes_recherches.get_liste_methodesrecherches();
            }
            return resultat;
        }

        /*
        public static object InvokeGenericMethodWithRuntimeGenericArguments(Action methodDelegate, Type[] runtimeGenericArguments, params object[] parameters)
        {
            if (parameters == null)
            {
                parameters = new object[0];
            }
            if (runtimeGenericArguments == null)
            {
                runtimeGenericArguments = new Type[0];
            }

            var myMethod = methodDelegate.Target.GetType()
                         .GetMethods()
                         .Where(m => m.Name == methodDelegate.Method.Name)
                         .Select(m => new
                         {
                             Method = m,
                             Params = m.GetParameters(),
                             Args = m.GetGenericArguments()
                         })
                         .Where(x => x.Params.Length == parameters.Length
                                     && x.Args.Length == runtimeGenericArguments.Length
                         )
                         .Select(x => x.Method)
                         .First().MakeGenericMethod(runtimeGenericArguments);
            return myMethod.Invoke(methodDelegate.Target, parameters);
        }
        */
    }
}
