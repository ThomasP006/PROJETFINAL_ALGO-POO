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
                    /** On initialise une variable string qui peut prendre la valeur null
                    d'où le "?" Cette variable va stocker les lignes de notre fichier
                    **/
                    int compteurLigne = 0; 
                    /**le compteur va nous permettre de changer de liste à chaque incrémentation
                    si compteurligne = 0 alors on est sur la liste de mots commencant en A puis B etc..**/
                    while ((ligne = lecteur_fichier.ReadLine()) != null && compteurLigne < 26)
                    /** Cette boucle while va nous permettre de lire toutes les lignes du 
                    fichier une une par une jusqu'à ce que les lignes du fichiers soit vides ou que on est arrivé à la fin de l'alphabet
                    ce qui signifique que tous les mots du fichier ont été rentrés dans les listes.
                    **/
                    {

                        mots[compteurLigne] = new List<string>();
                        /** On initialise chaque liste du tableau mots à chaque incrémentation de notre compteur**/
                        string motEnCours = ""; /** Cette variable permet de récuperer chaque lettre 
                        d'un mot de la ligne et de le construire jusqu'à arriver à un espace**/

                        foreach (char c in ligne) /**Notre boucle va permettre de construire les mots 
                        ou de les ajouters à notre liste selon si le caractère observé est une lettre 
                        ,et dans ce cas là il appartient encore à notre mot, ou si c'est un espace**/
                        {
                            if (c != ' ') // Dans le cas où le caractère n'est pas un espace
                            {
                                motEnCours += c; // On ajoute le caractère à notre mot 
                            }
                            else // Dans le cas où le caractère est un espace
                            {
                                if (!string.IsNullOrEmpty(motEnCours)) /**Si le mot que l'on a construit n'est pas 
                                un espace(dans le cas où il y aurait plusieurs espaces entre les mots dans le fichier)**/
                                {
                                    mots[compteurLigne].Add(motEnCours.ToUpper()); // On ajoute le mot à la liste
                                    motEnCours = ""; // On va reconstruire un mot après notre espace donc on remet la variable à vide.
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(motEnCours))
                        {
                            mots[compteurLigne].Add(motEnCours.ToUpper()); 
                        }

                        compteurLigne++; // on incrémente notre compteur pour passer à la lettre suivante de l'alphabet 
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la lecture : " + e.Message);
            }
        }

        public string toString()
        {
            char[] alphabet = new char[]{'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 
            'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            string resultat = "La langue de notre dictionnaire est en francais.\nVoici le nombre de mots par lettre :\n"; // On veut afficher le nombre de lettre par mot des listes et la langue.
            for (int i = 0; i < mots.Length; i++) // La boucle va permettre de passer par chaque mot du tableau donc du dictionnaire
            {
                char lettre = alphabet[i]; // on initialise une varibale qui récupère les lettres de l'alphabet
                int nombreMots; // Cette variable va prendre le nombre de mot contenus par liste donc par lettre de l'alphabet
                if (mots[i] != null) nombreMots = mots[i].Count; // Si la liste n'est pas vide on compte le nombre d'élements 
                else nombreMots = 0; //sinon comme la liste est vide cette lettre de l'alphabet ne contient aucun mot dans le dictionnaire
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
