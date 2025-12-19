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
    /// Cette class sert à représenter un participant au jeu
    /// Elle contient tout ce qui le concerne : son nom, son score actuel et la liste des mots qu'il a réussi à trouver
    public class Joueur
    {
        /// Ici, on déclare les attributs en privé pour qu'ils ne soient pas modifiés directement de l'extérieur
        private string nom;
        private List<string> mots_trouvés;
        private int scores_plateau;

        /// C'est le constructeur qui permet de créer un nouveau joueur
        /// On vérifie d'abord si le nom donné est valide. Si ce n'est pas le cas, on lui donne un nom par défaut
        /// Ensuite, on initialise son score à 0 et sa liste de mots à null pour commencer proprement
        public Joueur(string nom)
        {
            if (nom == null || nom == "" || nom == " ")
            {
                Console.WriteLine("Il y a une erreur, le nom du joueur est vide. On l'initialise donc par défaut");
                nom = "Joueur_par_défaut";
            }
            this.nom = nom;
            this.mots_trouvés = null;
            this.scores_plateau = 0;
        }

        /// Permet de lire le nom du joueur depuis l'extérieur de la classe en lecture seule
        public string Nom
        {
            get { return nom; }
        }

        /// Permet d'accéder à la liste des mots trouvés par le joueur en lecture seule
        public List<string> Mots_trouvés
        {
            get { return mots_trouvés; }
        }

        /// Permet de consulter le score actuel du joueur en lecture seule
        public int Scores_plateau
        {
            get { return scores_plateau; }
        }

        /// Cette méthode sert à ajouter un mot trouvé par le joueur dans sa liste personnelle
        /// Si c'est le tout premier mot ,donc que la liste est vide/null, on prend soin de créer la liste avant d'ajouter le mot dedans
        public void Add_Mot(string mot)
        {
            if (this.mots_trouvés == null) this.mots_trouvés = new List<string>();
            this.mots_trouvés.Add(mot);
        }

        /// Cette fonction génère une phrase complète pour décrire le joueur
        /// Elle affiche son nom, son score, et fait la liste de tous les mots trouvés s'il y en a
        public string toString()
        {
            string phrase = "Le joueur s'appelle : " + this.nom + "\n";
            phrase += "Son score actuel est de : " + this.scores_plateau + " points\n";
            phrase += "Il a trouvé les mots suivants : \n";
            if (this.mots_trouvés != null && this.mots_trouvés.Count > 0)
            {
                for (int i = 0; i < this.mots_trouvés.Count; i++)
                {
                    phrase += "- " + this.mots_trouvés[i] + "\n";
                }
            }
            else
            {
                phrase += $"{this.nom} n'a pas encore trouvé de mots.\n";
            }
            return phrase;
        }

        /// Méthode simple pour augmenter le score du joueur
        /// On prend la valeur donnée en paramètre et on l'ajoute au score total
        public void Add_Score(int val)
        {
            this.scores_plateau += val;
        }

        /// Cette méthode vérifie si le joueur a déjà trouvé un mot spécifique
        /// On parcourt toute sa liste de mots trouvés. Si on le trouve, on renvoie vrai, sinon on renvoie faux
        public bool Contient(string mot)
        {
            if (this.mots_trouvés == null) return false;
            for (int i = 0; i < this.mots_trouvés.Count; i++)
            {
                if (this.mots_trouvés[i] == mot) return true;
            }
            return false;
        }
    }
}