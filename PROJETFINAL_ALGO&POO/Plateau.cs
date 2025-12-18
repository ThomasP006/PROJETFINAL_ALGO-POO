using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    public class Plateau
    {
        private char[,] grille_de_lettres; // Matrice 2D qui stocke les lettres du plateau de jeu
        private int nombre_de_lignes; 
        private int nombre_de_colonnes; 
        private static Random generateur_aleatoire = new Random(); // Générateur de nombres aléatoires pour mélanger les lettres

        // On a deux constructeurs selon les manières de créer un plateau
        //Le premier constructeur consiste à charger un plateau depuis un fichier CSV existant
        public Plateau(string chemin_du_fichier_csv)
        {
            ChargerPlateauDepuisFichier(chemin_du_fichier_csv); // On charge le plateau depuis le fichier CSV passé en paramètre
        }

        //Pour le deuxième, on génère un plateau aléatoire en utilisant les lettres et fréquences d'un fichier
        public Plateau(string chemin_du_fichier_lettres, int lignes, int colonnes)
        {
            this.nombre_de_lignes = lignes; // On définit le nombre de lignes du plateau
            this.nombre_de_colonnes = colonnes; // On définit le nombre de colonnes du plateau
            this.grille_de_lettres = new char[lignes, colonnes]; // On initialise la grille avec les dimensions spécifiées
            GenererPlateauAleatoire(chemin_du_fichier_lettres); // On génère un plateau aléatoire en utilisant le fichier de lettres
        }

        // Génère un plateau aléatoire en respectant les fréquences des lettres définies dans un fichier
        private void GenererPlateauAleatoire(string chemin_fichier_lettre)
        {
            string[] lignes_du_fichier = File.ReadAllLines(chemin_fichier_lettre); /** La méthode
            FIle.ReadAllLines lit toutes les lignes du fichier et les renvoies dans les cases
            du tableau lignes_du_fichier**/
            Dictionary<char, int> frequences_max_par_lettre = new Dictionary<char, int>(); /** ON 
            créee un dictionnaire qui va stocker les lettres et les fréquences maximales pour chacune**/
            foreach (string ligne in lignes_du_fichier) /** On parcourt chaque ligne du 
            fichier puis on sépare les lettres de leur fréquence max en enlevant 
            la virgule et on les stocke dans le dictionnaire**/
            {
                string[] elements = ligne.Split(','); /** On crée un tableau qui sépare
                les éléments de chaque ligne **/
                char lettre = char.ToUpper(elements[0][0]); /**La lettre est placé en
                 premier élement de la sous chaine de elements donc on la place
                  dans la variable lettre**/
                int frequence_max = int.Parse(elements[1]); /** La fréquence maximale est 
                le deuxième élement de elements donc on la convertit en entier et on la
                 place dans la variable fréquence_max**/
                frequences_max_par_lettre[lettre] = frequence_max; // On ajoute la lettre et sa fréquence au dictionnaire
            }

            // On calcule le nombre total de lettres disponibles dans le fichier
            int total_lettres_disponibles = 0;
            foreach (int frequence in frequences_max_par_lettre.Values) // Pour chaque fréquence dans le dictionnaire
                total_lettres_disponibles += frequence; // On ajoute la fréquence au total

            // On vérifie si on a assez de lettres pour remplir le plateau
            if (total_lettres_disponibles < nombre_de_lignes * nombre_de_colonnes)
                throw new Exception("Pas assez de lettres pour remplir le plateau."); // Si non, on lance une exception

            // On crée une liste qui contiendra toutes les lettres, répétées selon leur fréquence
            List<char> reservoir_de_lettres = new List<char>();
            foreach (KeyValuePair<char, int> lettre_frequence in frequences_max_par_lettre) // Pour chaque lettre et sa fréquence
            {
                char lettre = lettre_frequence.Key; // On récupère la lettre
                int frequence = lettre_frequence.Value; // On récupère sa fréquence
                for (int i = 0; i < frequence; i++) // On répète la lettre autant de fois que sa fréquence
                    reservoir_de_lettres.Add(lettre); // On ajoute la lettre à la liste
            }

            // On mélange les lettres de manière aléatoire pour éviter les répétitions
            for (int i = reservoir_de_lettres.Count - 1; i > 0; i--) // On parcourt la liste à l'envers
            {
                int j = generateur_aleatoire.Next(i + 1); // On choisit un indice aléatoire entre 0 et i
                // On échange les lettres aux positions i et j pour les mélanger
                (reservoir_de_lettres[i], reservoir_de_lettres[j]) = (reservoir_de_lettres[j], reservoir_de_lettres[i]);
            }

            // On remplit la grille avec les lettres mélangées
            int index = 0; // Indice pour parcourir la liste des lettres
            for (int i = 0; i < nombre_de_lignes; i++) // Pour chaque ligne de la grille
            {
                for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne de la grille
                {
                    grille_de_lettres[i, j] = reservoir_de_lettres[index]; // On place la lettre dans la grille
                    index++; // On passe à la lettre suivante
                }
            }
        }

        public void ChargerPlateauDepuisFichier(string chemin_du_fichier)
        {
            string[] lignes_du_fichier = File.ReadAllLines(chemin_du_fichier); // On lit toutes les lignes du fichier CSV

            // On détermine les dimensions du plateau en fonction du fichier
            this.nombre_de_lignes = lignes_du_fichier.Length; // Le nombre de lignes est égal au nombre de lignes du fichier
            this.nombre_de_colonnes = lignes_du_fichier[0].Split(',').Length; // Le nombre de colonnes est égal au nombre de valeurs sur la première ligne

            // On initialise la grille avec les dimensions déterminées
            this.grille_de_lettres = new char[nombre_de_lignes, nombre_de_colonnes];

            // On remplit la grille avec les données du fichier
            for (int i = 0; i < nombre_de_lignes; i++) // Pour chaque ligne du fichier
            {
                string[] cases = lignes_du_fichier[i].Split(','); // On sépare les valeurs de la ligne par des virgules

                for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne
                {
                    if (j < cases.Length) // Si la colonne existe dans la ligne
                    {
                        if (string.IsNullOrEmpty(cases[j])) // Si la case est vide
                            grille_de_lettres[i, j] = ' '; // On met un espace
                        else
                            grille_de_lettres[i, j] = cases[j][0]; // Sinon, on prend le premier caractère de la case
                    }
                    else
                    {
                        grille_de_lettres[i, j] = ' '; // Si la ligne n'a pas assez de colonnes, on met un espace
                    }
                }
            }
        }

        // Affiche le plateau sous forme de chaîne de caractères avec des bordures
        public override string ToString()
        {
            string representation_plateau = ""; // Chaîne qui contiendra la représentation du plateau

            // On construit la bordure supérieure du plateau
            representation_plateau += "┌";
            for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne
            {
                representation_plateau += "───"; // On ajoute une ligne horizontale
                if (j < nombre_de_colonnes - 1) // Si ce n'est pas la dernière colonne
                    representation_plateau += "┬"; // On ajoute un connecteur
            }
            representation_plateau += "┐\n"; // On termine la bordure supérieure

            // On construit chaque ligne du plateau
            for (int i = 0; i < nombre_de_lignes; i++) // Pour chaque ligne
            {
                representation_plateau += "│"; // On commence par une bordure verticale
                for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne
                {
                    char lettre = grille_de_lettres[i, j]; // On récupère la lettre de la grille
                    // On ajoute la lettre (ou un espace si vide) entre deux bordures verticales
                    representation_plateau += $" {((lettre == ' ') ? ' ' : lettre)} │";
                }
                representation_plateau += "\n"; // On passe à la ligne suivante

                if (i < nombre_de_lignes - 1) // Si ce n'est pas la dernière ligne
                {
                    representation_plateau += "├"; // On commence la bordure intermédiaire
                    for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne
                    {
                        representation_plateau += "───"; // On ajoute une ligne horizontale
                        if (j < nombre_de_colonnes - 1) // Si ce n'est pas la dernière colonne
                            representation_plateau += "┼"; // On ajoute un connecteur
                    }
                    representation_plateau += "┤\n"; // On termine la bordure intermédiaire
                }
            }

            // On construit la bordure inférieure du plateau
            representation_plateau += "└";
            for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne
            {
                representation_plateau += "───"; // On ajoute une ligne horizontale
                if (j < nombre_de_colonnes - 1) // Si ce n'est pas la dernière colonne
                    representation_plateau += "┴"; // On ajoute un connecteur
            }
            representation_plateau += "┘\n"; // On termine la bordure inférieure

            return representation_plateau; // On retourne la représentation complète du plateau
        }

        // Recherche un mot sur le plateau en partant du bas
        public List<Position> RechercherMot(string mot)
        {
            mot = mot.ToUpper(); // On met le mot en majuscules pour uniformiser la recherche
            int ligne_de_depart = nombre_de_lignes - 1; // On commence la recherche depuis la dernière ligne

            for (int colonne = 0; colonne < nombre_de_colonnes; colonne++) // Pour chaque colonne
            {
                bool[,] cases_utilisees = new bool[nombre_de_lignes, nombre_de_colonnes]; // Matrice pour suivre les cases déjà utilisées
                List<Position> chemin_mot = new List<Position>(); // Liste pour stocker les positions des lettres du mot

                // On lance la recherche récursive à partir de la dernière ligne et de la colonne actuelle
                if (RechercherVoisins(ligne_de_depart, colonne, mot, 0, cases_utilisees, chemin_mot))
                    return chemin_mot; // Si le mot est trouvé, on retourne son chemin
            }

            return null; // Si le mot n'est pas trouvé, on retourne null
        }

        // Méthode récursive pour rechercher un mot en explorant les voisins
        private bool RechercherVoisins(int i, int j, string mot, int indice_lettre, bool[,] cases_utilisees, List<Position> chemin_mot)
        {
            // On vérifie si on est en dehors des limites du plateau
            if (i < 0 || i >= nombre_de_lignes || j < 0 || j >= nombre_de_colonnes)
                return false;

            // On vérifie si la case a déjà été utilisée
            if (cases_utilisees[i, j])
                return false;

            // On vérifie si la lettre de la case correspond à la lettre attendue du mot
            if (grille_de_lettres[i, j] != mot[indice_lettre])
                return false;

            // On marque la case comme utilisée et on ajoute sa position au chemin
            cases_utilisees[i, j] = true;
            chemin_mot.Add(new Position(i, j));

            // Si on a trouvé toutes les lettres du mot, on retourne vrai
            if (indice_lettre == mot.Length - 1)
                return true;

            // On explore récursivement les 4 directions possibles (haut, bas, gauche, droite)
            if (RechercherVoisins(i - 1, j, mot, indice_lettre + 1, cases_utilisees, chemin_mot) || // Haut
                RechercherVoisins(i + 1, j, mot, indice_lettre + 1, cases_utilisees, chemin_mot) || // Bas
                RechercherVoisins(i, j - 1, mot, indice_lettre + 1, cases_utilisees, chemin_mot) || // Gauche
                RechercherVoisins(i, j + 1, mot, indice_lettre + 1, cases_utilisees, chemin_mot))   // Droite
                return true;

            // Si le mot n'est pas trouvé dans cette branche, on annule le marquage de la case
            cases_utilisees[i, j] = false;
            chemin_mot.RemoveAt(chemin_mot.Count - 1);
            return false;
        }

        // Vérifie si le plateau est vide (toutes les cases sont vides)
        public bool EstVide()
        {
            for (int i = 0; i < nombre_de_lignes; i++) // Pour chaque ligne
            {
                for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne
                {
                    if (grille_de_lettres[i, j] != ' ') // Si une case n'est pas vide
                        return false; // Le plateau n'est pas vide
                }
            }
            return true; // Le plateau est vide
        }

        // Met à jour le plateau après qu'un mot a été trouvé (les lettres tombent vers le bas)
        public void MettreAJourPlateau(List<Position> positions_mot)
        {
            // On efface les lettres du mot trouvé en les remplaçant par des espaces
            foreach (Position position in positions_mot)
            {
                grille_de_lettres[position.Ligne, position.Colonne] = ' ';
            }

            // Pour chaque colonne, on fait descendre les lettres vers le bas
            for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne
            {
                List<char> lettres_colonne = new List<char>(); // Liste pour stocker les lettres de la colonne

                // On collecte les lettres non vides de la colonne, de bas en haut
                for (int i = nombre_de_lignes - 1; i >= 0; i--)
                {
                    if (grille_de_lettres[i, j] != ' ') // Si la case n'est pas vide
                    {
                        lettres_colonne.Add(grille_de_lettres[i, j]); // On ajoute la lettre à la liste
                    }
                }

                // On remplit la colonne avec les lettres collectées, en commençant par le bas
                int index = 0;
                for (int i = nombre_de_lignes - 1; i >= 0; i--) // De bas en haut
                {
                    if (index < lettres_colonne.Count) // Si on a encore des lettres à placer
                    {
                        grille_de_lettres[i, j] = lettres_colonne[index]; // On place la lettre
                        index++;
                    }
                    else
                    {
                        grille_de_lettres[i, j] = ' '; // Sinon, on met un espace
                    }
                }
            }
        }

        // Sauvegarde l'état actuel du plateau dans un fichier CSV
        public void SauvegarderDansFichier(string chemin_du_fichier)
        {
            using (StreamWriter ecrivain = new StreamWriter(chemin_du_fichier)) // On ouvre un flux d'écriture vers le fichier
            {
                for (int i = 0; i < nombre_de_lignes; i++) // Pour chaque ligne du plateau
                {
                    List<string> cellules = new List<string>(); // Liste pour stocker les valeurs de la ligne
                    for (int j = 0; j < nombre_de_colonnes; j++) // Pour chaque colonne
                    {
                        if (grille_de_lettres[i, j] == ' ') // Si la case est vide
                            cellules.Add(""); // On ajoute une chaîne vide
                        else
                            cellules.Add(grille_de_lettres[i, j].ToString()); // Sinon, on ajoute la lettre
                    }
                    ecrivain.WriteLine(string.Join(",", cellules)); // On écrit la ligne dans le fichier
                }
            }
        }
    }

    // Classe pour représenter une position (ligne, colonne) sur le plateau
    public class Position
    {
        public int Ligne { get; } // Propriété en lecture seule pour la ligne
        public int Colonne { get; } // Propriété en lecture seule pour la colonne

        public Position(int ligne, int colonne)
        {
            Ligne = ligne; // On initialise la ligne
            Colonne = colonne; // On initialise la colonne
        }
    }
}
