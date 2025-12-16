using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    public class Plateau
    {
        // Matrice de caractères représentant le plateau de jeu
        private char[,] grille_de_lettres;
        // Nombre de lignes et de colonnes du plateau
        private int nombre_de_lignes;
        private int nombre_de_colonnes;
        // Générateur de nombres aléatoires pour la création du plateau
        private static Random generateur_aleatoire = new Random();

        // Constructeur pour charger un plateau depuis un fichier CSV
        public Plateau(string chemin_du_fichier_csv)
        {
            ChargerPlateauDepuisFichier(chemin_du_fichier_csv);
        }

        // Constructeur pour générer un plateau aléatoire
        public Plateau(string chemin_du_fichier_lettres, int lignes, int colonnes)
        {
            this.nombre_de_lignes = lignes;
            this.nombre_de_colonnes = colonnes;
            this.grille_de_lettres = new char[lignes, colonnes];
            GenererPlateauAleatoire(chemin_du_fichier_lettres);
        }

        // Génère un plateau aléatoire à partir du fichier des lettres
        private void GenererPlateauAleatoire(string chemin_du_fichier_lettres)
        {
            // 1. Lire le fichier des lettres et leurs fréquences
            var lignes_du_fichier = File.ReadAllLines(chemin_du_fichier_lettres);
            Dictionary<char, int> frequences_max_par_lettre = new Dictionary<char, int>();

            foreach (var ligne in lignes_du_fichier)
            {
                var elements = ligne.Split(',');
                char lettre = char.ToUpper(elements[0][0]);
                int frequence_max = int.Parse(elements[1]);
                frequences_max_par_lettre[lettre] = frequence_max;
            }

            // 2. Vérifier qu'il y a assez de lettres pour remplir le plateau
            int total_lettres_disponibles = 0;
            foreach (var frequence in frequences_max_par_lettre.Values)
                total_lettres_disponibles += frequence;

            if (total_lettres_disponibles < nombre_de_lignes * nombre_de_colonnes)
                throw new Exception("Pas assez de lettres pour remplir le plateau.");

            // 3. Créer une liste de lettres en respectant les fréquences
            List<char> reservoir_de_lettres = new List<char>();
            foreach (var lettre_frequence in frequences_max_par_lettre)
            {
                char lettre = lettre_frequence.Key;
                int frequence = lettre_frequence.Value;
                for (int i = 0; i < frequence; i++)
                    reservoir_de_lettres.Add(lettre);
            }

            // 4. Mélanger les lettres aléatoirement
            for (int i = reservoir_de_lettres.Count - 1; i > 0; i--)
            {
                int j = generateur_aleatoire.Next(i + 1);
                (reservoir_de_lettres[i], reservoir_de_lettres[j]) = (reservoir_de_lettres[j], reservoir_de_lettres[i]);
            }

            // 5. Remplir la grille avec les lettres mélangées
            int index = 0;
            for (int i = 0; i < nombre_de_lignes; i++)
            {
                for (int j = 0; j < nombre_de_colonnes; j++)
                {
                    grille_de_lettres[i, j] = reservoir_de_lettres[index];
                    index++;
                }
            }
        }

        // Charge un plateau depuis un fichier CSV
        public void ChargerPlateauDepuisFichier(string chemin_du_fichier)
{
    var lignes_du_fichier = File.ReadAllLines(chemin_du_fichier);

    // Détermine le nombre de lignes et de colonnes
    this.nombre_de_lignes = lignes_du_fichier.Length;
    this.nombre_de_colonnes = lignes_du_fichier[0].Split(',').Length;

    // Initialise la grille
    this.grille_de_lettres = new char[nombre_de_lignes, nombre_de_colonnes];

    // Remplit la grille avec les données du fichier
    for (int i = 0; i < nombre_de_lignes; i++)
    {
        var cases = lignes_du_fichier[i].Split(',');

        for (int j = 0; j < nombre_de_colonnes; j++)
        {
            if (j < cases.Length)
            {
                if (string.IsNullOrEmpty(cases[j]))
                    grille_de_lettres[i, j] = ' '; // Case vide
                else
                    grille_de_lettres[i, j] = cases[j][0]; // Premier caractère
            }
            else
            {
                grille_de_lettres[i, j] = ' '; // Si la ligne n'a pas assez de colonnes, on met un espace
            }
        }
    }
}

        // Affiche le plateau sous forme de chaîne de caractères
       public override string ToString()
{
    string representation_plateau = "";

    // Ligne supérieure du plateau
    representation_plateau += "┌";
    for (int j = 0; j < nombre_de_colonnes; j++)
    {
        representation_plateau += "───";
        if (j < nombre_de_colonnes - 1)
            representation_plateau += "┬";
    }
    representation_plateau += "┐\n";

    // Lignes du plateau
    for (int i = 0; i < nombre_de_lignes; i++)
    {
        representation_plateau += "│";
        for (int j = 0; j < nombre_de_colonnes; j++)
        {
            char lettre = grille_de_lettres[i, j];
            representation_plateau += $" {((lettre == ' ') ? ' ' : lettre)} │"; // Affiche un espace si la case est vide
        }
        representation_plateau += "\n";

        if (i < nombre_de_lignes - 1)
        {
            representation_plateau += "├";
            for (int j = 0; j < nombre_de_colonnes; j++)
            {
                representation_plateau += "───";
                if (j < nombre_de_colonnes - 1)
                    representation_plateau += "┼";
            }
            representation_plateau += "┤\n";
        }
    }

    // Ligne inférieure du plateau
    representation_plateau += "└";
    for (int j = 0; j < nombre_de_colonnes; j++)
    {
        representation_plateau += "───";
        if (j < nombre_de_colonnes - 1)
            representation_plateau += "┴";
    }
    representation_plateau += "┘\n";

    return representation_plateau;
}



        // Recherche un mot sur le plateau
        public List<Position> RechercherMot(string mot)
        {
            mot = mot.ToUpper();
            int ligne_de_depart = nombre_de_lignes - 1; // On commence la recherche depuis la base du plateau

            for (int colonne = 0; colonne < nombre_de_colonnes; colonne++)
            {
                bool[,] cases_utilisees = new bool[nombre_de_lignes, nombre_de_colonnes];
                List<Position> chemin_mot = new List<Position>();

                if (RechercherVoisins(ligne_de_depart, colonne, mot, 0, cases_utilisees, chemin_mot))
                    return chemin_mot;
            }

            return null; // Mot introuvable
        }

        // Méthode récursive pour rechercher les voisins et former le mot
        private bool RechercherVoisins(int i, int j, string mot, int indice_lettre, bool[,] cases_utilisees, List<Position> chemin_mot)
        {
            // Vérifie si on est hors du plateau
            if (i < 0 || i >= nombre_de_lignes || j < 0 || j >= nombre_de_colonnes)
                return false;

            // Vérifie si la case est déjà utilisée
            if (cases_utilisees[i, j])
                return false;

            // Vérifie si la lettre correspond à celle attendue
            if (grille_de_lettres[i, j] != mot[indice_lettre])
                return false;

            // Marque la case comme utilisée et ajoute la position au chemin
            cases_utilisees[i, j] = true;
            chemin_mot.Add(new Position(i, j));

            // Si on a trouvé toutes les lettres du mot, retourne vrai
            if (indice_lettre == mot.Length - 1)
                return true;

            // Recherche récursive dans les 4 directions
            if (RechercherVoisins(i - 1, j, mot, indice_lettre + 1, cases_utilisees, chemin_mot) || // Haut
                RechercherVoisins(i + 1, j, mot, indice_lettre + 1, cases_utilisees, chemin_mot) || // Bas
                RechercherVoisins(i, j - 1, mot, indice_lettre + 1, cases_utilisees, chemin_mot) || // Gauche
                RechercherVoisins(i, j + 1, mot, indice_lettre + 1, cases_utilisees, chemin_mot))   // Droite
                return true;

            // Si le mot n'est pas trouvé, annule le marquage de la case et retire la position du chemin
            cases_utilisees[i, j] = false;
            chemin_mot.RemoveAt(chemin_mot.Count - 1);
            return false;
        }

        // Vérifie si le plateau est vide
        public bool EstVide()
        {
            for (int i = 0; i < nombre_de_lignes; i++)
            {
                for (int j = 0; j < nombre_de_colonnes; j++)
                {
                    if (grille_de_lettres[i, j] != '\0')
                        return false;
                }
            }
            return true;
        }

        // Met à jour le plateau après qu'un mot a été trouvé
        public void MettreAJourPlateau(List<Position> positions_mot)
{
    // 1. Efface les lettres du mot trouvé en les remplaçant par un espace vide
    foreach (var position in positions_mot)
    {
        grille_de_lettres[position.Ligne, position.Colonne] = ' ';
    }

    // 2. Fait glisser les lettres vers le bas pour chaque colonne
    for (int j = 0; j < nombre_de_colonnes; j++)
    {
        List<char> lettres_colonne = new List<char>();

        // Collecte toutes les lettres non vides de la colonne
        for (int i = nombre_de_lignes - 1; i >= 0; i--)
        {
            if (grille_de_lettres[i, j] != ' ')
            {
                lettres_colonne.Add(grille_de_lettres[i, j]);
            }
        }

        // Remplit la colonne de bas en haut avec les lettres collectées
        int index = 0;
        for (int i = nombre_de_lignes - 1; i >= 0; i--)
        {
            if (index < lettres_colonne.Count)
            {
                grille_de_lettres[i, j] = lettres_colonne[index];
                index++;
            }
            else
            {
                grille_de_lettres[i, j] = ' '; // Remplit les cases restantes par un espace
            }
        }
    }
}


        // Sauvegarde le plateau dans un fichier
        public void SauvegarderDansFichier(string chemin_du_fichier)
        {
            using (StreamWriter ecrivain = new StreamWriter(chemin_du_fichier))
            {
                for (int i = 0; i < nombre_de_lignes; i++)
                {
                    List<string> cellules = new List<string>();
                    for (int j = 0; j < nombre_de_colonnes; j++)
                    {
                        if (grille_de_lettres[i, j] == '\0')
                            cellules.Add("");
                        else
                            cellules.Add(grille_de_lettres[i, j].ToString());
                    }
                    ecrivain.WriteLine(string.Join(",", cellules));
                }
            }
        }
    }

    public class Position
    {
        public int Ligne { get; }
        public int Colonne { get; }

        public Position(int ligne, int colonne)
        {
            Ligne = ligne;
            Colonne = colonne;
        }
    }
}
