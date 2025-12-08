using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJETFINAL_ALGO_POO
{
    public class Dictionnaire
    {
        private List<string>[] mots; //attribut privé qui contient la liste des mots du dictionnaire
        public Dictionnaire(string nomFichier) //constructeur qui initialise le dictionnaire à partir d'un fichier texte
        {
            this.mots = new List<string>[26];
            InitDictionnaire(nomFichier);
        }
        public void InitDictionnaire(string cheminFichier) // qui initialise le dictionnaire à partir d'un fichier texte
        {
            try
            {
                using (StreamReader sr = new StreamReader(cheminFichier))
                {
                    string ligne;
                    int compteurLigne = 0;
                    while ((ligne = sr.ReadLine()) != null && compteurLigne < 26)
                    {
                        string[] motsDecoupes = ligne.Split(' ');
                        mots[compteurLigne] = new List<string>(motsDecoupes);
                        compteurLigne++;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Le fichier spécifié est introuvable : " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue lors de la lecture du fichier : " + e.Message);
            }
        }
        public string toString() //qui retourne une chaîne de caractères qui décrit le dictionnaire à savoir ici le nombre de mots par lettre et la langue
        {
            // On commence avec une simple chaîne de caractères
            string resultat = "Dictionnaire : Français\n";
            resultat += "Nombre de mots par lettre :\n";

            for (int i = 0; i < mots.Length; i++)
            {
                char lettre = Convert.ToChar('A' + i); 
                
                // On s'assure que la liste n'est pas nulle
                int nombreMots = (mots[i] != null) ? mots[i].Count : 0;
                
                // On ajoute la ligne à la chaîne existante
                // \n permet de passer à la ligne suivante
                resultat += "Lettre " + lettre + " : " + nombreMots + " mots\n";
            }

            return resultat;
        }
        public bool RechDichoRecursif(string mot) // qui teste si le mot appartient bien au dictionnaire. Vous utiliserez cette méthode pour rechercher un mot dans le dictionnaire !
        public void Tri_XXX() //pour trier le dictionnaire (quick_sort ou tri-fusion)
    }
}
