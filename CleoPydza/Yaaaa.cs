using System;
using System.Collections.Generic;
using System.Text;
using CleoPydza;
using System.Net.Sockets;
using System.IO;
using static CleoPydza.TypeCarte;
using static CleoPydza.TypeCepage;
using static CleoPydza.Connexion;

namespace CleoPydza
{
    class Yaaaa
    {
        #region attributes
        private Connexion _connect;
        private List<Carte> main;
        public List<Carte> Main { get => main; set => main = value; }
        public string Reception { get => reception; set => reception = value; }

        private Carte[] sommet = new Carte[2];
        private int nbJoueur;
        private string envoi;
        private string reception;
        private string[] receptionTab;
        #endregion

        #region methodes
        public Yaaaa(Connexion co)
        {
            _connect = co;
            _connect.ConnexionServeur();
            _connect.CreationFlux();
            envoi = "";
            reception = "";
            this.main = new List<Carte>();
            Inscription();
        }

        protected void Inscription() 
        {
            envoi = "INSCRIRE";
            EnvoiMessage();
            ReceptionMessage();
            int ok = Cherche("OK", receptionTab);
            if (ok == -1) { }
            else
            {
                nbJoueur = int.Parse(receptionTab[(ok + 1)]);
            }
        }

        protected int Cherche(string cible, string[] support)
        {
            int trouve = -1;
            for(int i = 0; i<support.Length; i++)
            {
                string s = support[i];
                if (s == cible)
                {
                    trouve = i; 
                }
                else { trouve = -1; }
            }
            return trouve;
        }

        public void ReceptionMessage()
        {
                reception = _connect.FluxEntrant.ReadLine();
                Console.WriteLine(reception);
         

            if ((!this.reception.StartsWith("NOK")) && (this.reception !=("FIN")))
            {
                receptionTab = reception.Split('|');
            }
            else if (Reception == "FIN")
            {
                _connect.Stop();
            }
 
            
        }

        private void EnvoiMessage()
        {
            _connect.FluxSortant.WriteLine(envoi);
            Console.WriteLine(envoi);
            if (Reception == "FIN")
            {
                _connect.Stop();
            }
        }

        protected void Sommet()
        {
            envoi = "SOMMET";
            EnvoiMessage();
            ReceptionMessage();
            ChercheSommet();
        }

        protected void Pioche(int numPioche)
        {
            envoi = ("PIOCHER|" + numPioche);

            EnvoiMessage();
            ReceptionMessage();

            main.Add(identifieCarte(receptionTab[1]));
        }

        /// <summary>
        /// effectue l'action saboter
        /// </summary>
        /// <param name="cible">joueur visé (1 ou -1)</param>
        protected void Sabote(int cible)
        {
            envoi = "SABOTER|";
            if (cible == 1)
            {
                envoi += "1";
            }
            else if (cible == -1)
            {
                envoi += "-1";
            }
            EnvoiMessage();
            MiseAjourMain();
        }

        protected void Pose(TypeCepage cep)
        {
            envoi = String.Format("POSER|{0}", cep);
            EnvoiMessage();
            MiseAjourMain();
        }


        /// <summary>
        /// Ne défausse rien
        /// </summary>
        protected void Defausse()
        {
            envoi = string.Format("DEFAUSSER");
            EnvoiMessage();
            MiseAjourMain();
        }
        /// <summary>
        /// Défausse une carte
        /// </summary>
        /// <param name="nbCartes"> carte demandée à défausser </param>
        protected void Defausse(int nbCartes)
        {
            envoi = string.Format("DEFAUSSER|{0}", nbCartes);
            EnvoiMessage();
            MiseAjourMain();
        }
        /// <summary>
        /// Défausse deux cartes
        /// </summary>
        /// <param name="nbCartes"> carte 1 à défausser </param>
        /// <param name="nbCarte"> carte 2 à défausser </param>
        protected void Defausse(int nbCartes, int nbCarte)
        {
            envoi = string.Format("DEFAUSSER|{0}|{1}", nbCartes, nbCarte);
            EnvoiMessage();
            MiseAjourMain();
        }

        private void MiseAjourMain()
        {
            ReceptionMessage();
            this.main = StringToCartes(receptionTab);
        }

        /// <summary>
        /// met à jour le tableau contenant les sommets des pioches
        /// </summary>
        /// <returns></returns>
        private Carte[] ChercheSommet()
        {
            Carte[] temp = new Carte[3];
            for (int i = 1; i>receptionTab.Length; i++)
            {
                temp[i-1] = identifieCarte(receptionTab[i]);
            }
            return temp;
        }

        /// <summary>
        /// analyse un chaine de caractères et renvoie une liste de cartes correspondant à celle-ci
        /// </summary>
        /// <param name="chaine">chaine dans laquelle chercher la liste</param>
        /// <returns></returns>
        private List<Carte> StringToCartes(string[] chaine)
        {
            List<Carte> temp = new List<Carte>();
            for (int i = 1; i >receptionTab.Length; i++)
            {
                temp[i - 1] = identifieCarte(receptionTab[i]);
            }
            return temp;
        }

        /// <summary>
        /// renvoie une carte selon une chaîne de caractères
        /// </summary>
        /// <param name="chaine"></param>
        /// <returns></returns>
        private Carte identifieCarte(string chaine)
        {
            Carte tempCarte;
            switch (chaine)
            {
                case "BOUTEILLE":
                    tempCarte = new Carte(TypeCarte.bouteille);
                    break;
                case "SABOTAGE":
                    tempCarte = new Carte(TypeCarte.sabotage);
                    break;
                default:
                    tempCarte = Decompose(chaine);
                    break;
            }
            return tempCarte;
        }

        /// <summary>
        /// retourne une carte resin contenue dans une chaîne
        /// </summary>
        /// <param name="laChaine">chaîne à étudier</param>
        /// <returns></returns>
        private Resin Decompose(string laChaine)
        {
            string[] temp = laChaine.Split(';');
            TypeCepage type = Aligote;
            switch(temp[1])
            {
                case "Chardonnay":
                    type = TypeCepage.Chardonnay;
                    break;

                case "Aligote":
                    type = TypeCepage.Aligote;
                    break;

                case "Gamay":
                    type = TypeCepage.Gamay;
                    break;

                case "Pinot":
                    type = TypeCepage.Pinot;
                    break;
            }
            return new Resin(type, int.Parse(temp[2]));
        }

        public TypeCepage PlusGrandCepage()
        {
            int aligote = 0;
            int chardonnay = 0;
            int gamay = 0;
            int pinot = 0;
            foreach (Resin card in main)
            {
                switch (card.Cepage)
                {
                    case (TypeCepage.Aligote):
                        aligote += card.Quantite;
                        break;
                    case TypeCepage.Chardonnay:
                        chardonnay += card.Quantite;
                        break;
                    case TypeCepage.Gamay:
                        gamay += card.Quantite;
                        break;
                    case TypeCepage.Pinot:
                        pinot += card.Quantite;
                        break;
                }
            }
            int test = Math.Max(Math.Max(gamay, pinot), Math.Max(chardonnay, aligote));
            if (aligote == test)
            {
                return TypeCepage.Aligote;
            }
            else if (chardonnay == test)
            {
                return TypeCepage.Chardonnay;
            }
            else if (gamay == test)
            {
                return TypeCepage.Gamay;
            }
            else
            {
                return TypeCepage.Pinot;
            }
        }

        public int CompteCarte(TypeCarte card)
        {
            int nb = 0;
            foreach (Carte c in main)
            {
                if (c.Nom == card)
                {
                    nb += 1;
                }
            }
            return nb;
        }

        public void PiocheResin(TypeCepage type)
        {
            Sommet();
            foreach (Carte t in sommet)
            {
                if (t.Nom == TypeCarte.raisin)
                {
                    if (t.Cepage)
                }
            }
        }
        #endregion
    }
}
