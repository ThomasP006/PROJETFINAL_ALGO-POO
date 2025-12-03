using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PROJETFINAL_ALGO_POO
{
    public class Joueur // on commence par créer la classe Joueur
    {
        // on initialise ses attributs en privés
        private string nom; // énoncé : "Un joueur est caractérisé par son nom, par les mots trouvés et les scores par plateau au cours de la partie"
        private List <string> mots_trouvés;
        private int scores_plateau;


        //La création d’un joueur n’est possible que si celui-ci a un nom au départ du jeu
        public Joueur(string nom)
        {
            this.nom = nom;  // on initalise le nom du joueur avec le nom donné en paramètre
            this.mots_trouvés = null; // la liste doit être intialisée à null
            this.scores_plateau = 0; // le score doit etre intialisé à 0 d'après l'énoncé
        }

        // on met les propriétés en lecture des attributs pour que celle ci soit observable depuis d'autres classes car ls attributs de la classe sont en privés
        public string Nom
        {
            get { return nom; } 
        }
        public List <string> Mots_trouvés
        {
            get { return mots_trouvés; }    
        }
        public int Scores_plateau
        {
            get { return scores_plateau; }
        }
        public void Add_Mot (string mot)
        {
            if (this.mots_trouvés==null) this.mots_trouvés=new List<string>(); // la liste est initialisée à null mais on ne peut pas ajouter de mot dans ce cas. 
            //ON crée donc une nouvelle liste auquel on peut attribuer des valeurs
            this.mots_trouvés.Add(mot); // j'utilise la propriété .Add des listes pur ajouter le mot entré en paramètre
        }
        public string toString()
        {
            string phrase = "Le joueur s'appelle : " + this.nom + "\nIl a trouvés les mots suivants:\n"; // je créée une chaine de carctère phrase dans laquelle je met le texte qui va présenter chaque valeur d'attribut 
            for (int i = 0; i < this.mots_trouvés.Count; i++) // ce for va permettre d'énumérer chaque valeur de mot qui est compris dans le tableau.
            {
                phrase += this.mots_trouvés[i] + "\n";
            }
            phrase += "Il a pour l'instant " + this.scores_plateau + " points."; // j'ajoute dans le texte le nombre de points que le joueur a.
            return phrase; // je retourne ensuite la chaine de caractère qui contient le texte.
        }
        public void Add_Score(int val)
        {
            this.scores_plateau += val; //j'ajoute simplement le nombre de points entré en paramètre à mon score total
        }
        public bool Contient(string mot)
        {
            for( int i = 0; i< this.mots_trouvés.Count; i++) //le for va vérifier chaque valeur de la liste pour voir si elle correspond au mot recherché
            {
                if (this.mots_trouvés[i] == mot) return true;  // elle  retourne vrai si on on observe que le mot est trouvé une fois dans liste 
            }
            return false; //elle retourne faux si arrivé à la fin du tableau on a pas tro
        }

    }
}
