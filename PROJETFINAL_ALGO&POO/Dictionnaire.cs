using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    public class Dictionnaire
    {
        // -----on initialise d'abord les attributs-----
        private List<string>[] mots;
        /**Cet attribut est un tableau de listes.
         Chaque liste correspondera plus tard à une lettre de l'alphabet
         Il y aura donc 26 listes au total**/
       
        // -----on fait le constructeur-----
        public Dictionnaire(string nomdufichier)
        {
            this.mots = new List<string>[26]; // Comme dit plus haut on veut un tableau de 26 listes pour chaque lettre de l'alphabet
            InitialiserLeDictionnaire(nomdufichier); // On utilise la fonction que l'on a créer plus bas pour charger chaque mot du fichier dans notre tableau de liste
            TriParQuickSort(); // On tri les mots de chaque liste de notre tableau pour que ca soit déjà fait pour les futures fonctions.
        }


        public void InitialiserLeDictionnaire(string chemindufichier)
        {
            try /** On peut souvent avoir des erreurs lorsque l'on cherche à lire un fichier donc on
            utilise un try/catch comme ca s'il y a une erreur de lecture, on affiche le problème 
            et le programme continue.**/
            {
                using (StreamReader lecteur_fichier = new StreamReader(chemindufichier))
                /** Le Streamreader va permettre d'ouvrir le fichier dont le chemin d'accès est donné en
                paramètre pour ensuite extirper du fichier les mots pour les mettre dans les listes
                **/
                {
                    string? ligne; 
                    int compteurLigne = 0; 
                    /**le compteur va nous permettre de changer de liste à chaque incrémentation
                    si compteurligne = 0 alors on est sur la liste de mots commencant en A puis B etc..**/
                    while ((ligne = lecteur_fichier.ReadLine()) != null && compteurLigne < 26)
                    {

                        mots[compteurLigne] = new List<string>();
                        string motEnCours = "";

                        // Parcourt chaque caractère de la ligne
                        foreach (char c in ligne)
                        {
                            if (c != ' ') // Si le caractère n'est pas un espace
                            {
                                motEnCours += c; // Ajoute le caractère au mot en cours
                            }
                            else // Si c'est un espace
                            {
                                if (!string.IsNullOrEmpty(motEnCours)) // Si le mot n'est pas vide
                                {
                                    mots[compteurLigne].Add(motEnCours.ToUpper()); // Ajoute le mot à la liste
                                    motEnCours = ""; // Réinitialise pour le prochain mot
                                }
                            }
                        }
                        // Ajoute le dernier mot (s'il y en a un) après la fin de la ligne
                        if (!string.IsNullOrEmpty(motEnCours))
                        {
                            mots[compteurLigne].Add(motEnCours.ToUpper());
                        }

                        compteurLigne++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la lecture : " + e.Message);
            }
        }

        // Affiche le nombre de mots par lettre
        public string toString()
        {
            string resultat = "Dictionnaire : Français\nNombre de mots par lettre :\n";
            for (int i = 0; i < mots.Length; i++)
            {
                char lettre = (char)('A' + i);
                // On vérifie que la liste n'est pas vide
                int nombreMots = (mots[i] != null) ? mots[i].Count : 0;
                resultat += "Lettre " + lettre + " : " + nombreMots + " mots\n";
            }
            return resultat;
        }

        // Recherche récursive
        public bool RechDichoRecursif(string mot)
        {
            if (string.IsNullOrEmpty(mot)) return false;

            string motMaj = mot.ToUpper(); // On s'assure que c'est bien en majuscule
            int index = motMaj[0] - 'A';

            // Vérif si c'est bien une lettre valide
            if (index < 0 || index > 25) return false;

            List<string> maListe = mots[index];
            if (maListe == null || maListe.Count == 0) return false;

            // On lance la vraie recherche
            return RechDichoInterne(maListe, motMaj, 0, maListe.Count - 1);
        }

        // Logique dichotomique
        private bool RechDichoInterne(List<string> liste, string motCherche, int debut, int fin)
        {
            if (debut > fin) return false; // Zone de recherche vide

            int milieu = (debut + fin) / 2;

            int comparaison = string.Compare(motCherche, liste[milieu]);

            if (comparaison == 0) return true;

            // Si le mot est avant dans l'alphabet, on cherche à gauche
            if (comparaison < 0) return RechDichoInterne(liste, motCherche, debut, milieu - 1);

            // Sinon on cherche à droite
            else return RechDichoInterne(liste, motCherche, milieu + 1, fin);
        }

        // Lance le tri sur toutes les lettres
        public void TriParQuickSort()
        {
            for (int i = 0; i < mots.Length; i++)
            {
                // On trie seulement s'il y a au moins 2 mots
                if (mots[i] != null && mots[i].Count > 1)
                {
                    QuickSortInterne(mots[i], 0, mots[i].Count - 1);
                }
            }
        }

        private void QuickSortInterne(List<string> liste, int gauche, int droite)
        {
            // Optimisation : si la liste est toute petite, le tri par insertion est plus rapide que la récursion
            if (droite - gauche <= 15)
            {
                if (gauche < droite) TriInsertion(liste, gauche, droite);
                return;
            }

            int pivotIndex = gauche;
            string pivot = liste[droite]; // On prend le dernier comme pivot
            string temp;

            // Partitionnement
            for (int i = gauche; i < droite; i++)
            {
                if (string.Compare(liste[i], pivot) <= 0)
                {
                    // Echange
                    temp = liste[pivotIndex];
                    liste[pivotIndex] = liste[i];
                    liste[i] = temp;
                    pivotIndex++;
                }
            }

            // On met le pivot à sa place définitive
            temp = liste[pivotIndex];
            liste[pivotIndex] = liste[droite];
            liste[droite] = temp;

            // Appels récursifs
            QuickSortInterne(liste, gauche, pivotIndex - 1);
            QuickSortInterne(liste, pivotIndex + 1, droite);
        }

        // Tri simple utilisé quand les sous-listes deviennent petites
        private void TriInsertion(List<string> liste, int debut, int fin)
        {
            for (int i = debut + 1; i <= fin; i++)
            {
                string cle = liste[i];
                int j = i - 1;

                while (j >= debut && string.Compare(liste[j], cle) > 0)
                {
                    liste[j + 1] = liste[j];
                    j--;
                }
                liste[j + 1] = cle;
            }
        }
    }
}
