using System;
using System.Collections.Generic;
using System.Text;
using static CleoPydza.TypeCepage;
using static CleoPydza.TypeCarte;

namespace CleoPydza
{
    public class Resin : Carte
    {
        private TypeCepage cepage;
        private int quantite;

        #region properties
        public int Quantite { get => quantite; }
        public TypeCepage Cepage { get => cepage; }
        #endregion

        public Resin(TypeCepage cepage, int quantite) : base(TypeCarte.raisin)
        {
            this.cepage = cepage;
            this.quantite = quantite;
        }
    }
}
