using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    public class Dictionnaire
    {
        // Tableau de listes
        private List<string>[] mots;
        
        // On déclare le séparateur ici pour ne pas le recréer à chaque ligne lue
        private static readonly char[] separateurs = { ' ' }; 

        // Constructeur
        public Dictionnaire(string nomFichier)
        {
            this.mots = new List<string>[26]; // 26 cases pour l'alphabet
            InitialiserLeDictionnaire(nomFichier);
            
            // On trie tout de suite pour que la recherche dichotomique fonctionne
            TriParQuickSort();
        }

        // Lecture et chargement du fichier
        public void InitialiserLeDictionnaire(string chemindufichier)
        {
            try // La fonction try/catch va nous permettre d'executer le code si celui ci n'a pas d'erreur au moment de la recherche ou de la lecture du fichier. Si il y a une erreur alors cela affichera l'erreur directement.
            {
                using (StreamReader lecteur_fichier = new StreamReader(chemindufichier))
                {
                    string? ligne; // le point d'interogation permet d'accepter de mettre une valeur nulle pour pas que le prgroamme me mette un erreur "nullabl" au moment de compiler
                    int compteurLigne = 0;
                    
                    // On lit ligne par ligne
                    while ((ligne = lecteur_fichier.ReadLine()) != null && compteurLigne < 26)
                    {
                        // On découpe la ligne et on vire les entrées vides s'il y en a
                        string[] motsséparés = ligne.Split(separateurs, StringSplitOptions.RemoveEmptyEntries); // cette ligne a été faite à partir de l'ia pour enlever tous les espaces que l'on peut trouver dans le fichier
                        
                        // Optimisation : on donne la taille direct pour éviter que la liste s'agrandisse toute seule
                        mots[compteurLigne] = new List<string>(motsséparés.Length);
                        
                        foreach(var m in motsséparés)
                        {
                            // On stocke tout en majuscules pour simplifier les comparaisons plus tard
                            mots[compteurLigne].Add(m.ToUpper());
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