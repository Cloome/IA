using System;
using System.Collections.Generic;
using System.Text;
using static CleoPydza.TypeCarte;

namespace CleoPydza
{
    public class Carte
    {
        #region attributes
        private TypeCarte nom;
        #endregion

        public TypeCarte Nom { get => nom; }

        /// <summary>
        /// Constructeur de carte
        /// </summary>
        /// <param name="nom">nom de la carte de type typeCarte</param>
        public Carte(TypeCarte nom)
        {
            this.nom = nom;
        }
    }
}
