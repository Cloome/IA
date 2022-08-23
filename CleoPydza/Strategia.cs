using System;
using System.Collections.Generic;
using System.Text;
using static CleoPydza.TypeCepage;

namespace CleoPydza
{
    /// <summary>
    /// Drunken IA
    /// </summary>
    class StrategIA : Yaaaa
    {
        private Random rand = new Random();
        public StrategIA(Connexion co) : base(co)
        {

            while (Reception != "FIN")
            {
                this.ReceptionMessage(); //DEBUT DU TOUR
                Phase1();
                Phase2();
                Phase3();
            }
        }

        private void Phase1()
        {
            Sommet();
            int parametrePiocher = 0;
            int parametreAlea = 0;

            parametreAlea = rand.Next(3); //Nombre aleatoire pour déterminer le parametre de la fonction
            switch (parametreAlea)
            {
                case (0): parametrePiocher = 0; break; //Pile 0
                case (1): parametrePiocher = 1; break; //Pile 1
                case (2): parametrePiocher = 2; break; //Pile 2
            }
            Pioche(parametrePiocher);


            if (CompteCarte(TypeCarte.bouteille) >2)
            {

            }
        }
        private void Phase2()
        {
            Sommet(); //Recupere les sommets des trois piles
            int action;
            int parametreAlea = 0;
            int parametrePiocher = 0;
            int parametreSaboter = 0;
            TypeCepage parametrePoser = Pinot;
            action = rand.Next(3); //Crée  un random qui représentera l'action à faire
            int nombreCarteRaisin = 0;
            int nombreCarteBouteille = 0;

            
            switch (action)
            {
                case (0): //Cas de la pioche
                    parametreAlea = rand.Next(3); //Nombre aleatoire pour déterminer le parametre de la fonction

                    switch (parametreAlea)
                    {
                        case (0): parametrePiocher = 0; break; //Pile 0
                        case (1): parametrePiocher = 1; break; //Pile 1
                        case (2): parametrePiocher = 2; break; //Pile 2
                    }
                    Pioche(parametrePiocher);
                    break;
                case (1): //Cas du sabotage
                    parametreAlea = rand.Next(2);
                    switch (parametreAlea)
                    {
                        case (0): parametreSaboter = -1; break; //Sabotage à gauche
                        case (1): parametreSaboter = 1; break; //Sabotage à droite 
                    }
                    Sabote(parametreSaboter);
                    break;
                case (2): //Cas de la pose
                    parametreAlea = rand.Next(4); //Nombre aleatoire pour déterminer le parametre de la fonction
                    switch (parametreAlea)
                    {
                        case (0): parametrePoser = Aligote; break; //Création d'une bouteille de Aligoté
                        case (1): parametrePoser = Chardonnay; break; //Création d'une bouteille de Chardonnay
                        case (2): parametrePoser = Gamay; break; //Création d'une bouteille de Gamay
                        case (3): parametrePoser = Pinot; break; //Création d'une bouteille de Pinot
                    }
                    Pose(parametrePoser);
                    break;
            }
        }
        private void Phase3()
        {
            if (Main.Count == 0) { Defausse(); }
            else
            {
                int parametreAlea = 0;
                int parametreDefausser1 = 0;
                int parametreDefausser2 = 0;
                parametreAlea = rand.Next(3); //Nombre aleatoire pour déterminer le parametre de la fonction
                switch (parametreAlea)
                {
                    case (0): Defausse(); break; //Defausse 0 carte
                    case (1):
                        parametreDefausser1 = rand.Next(Main.Count);
                        Defausse(parametreDefausser1); break;
                    case (2):
                        parametreDefausser1 = rand.Next(Main.Count);
                        parametreDefausser2 = rand.Next(Main.Count);
                        Defausse(parametreDefausser1); break;
                }
            }
        }
    }
}
