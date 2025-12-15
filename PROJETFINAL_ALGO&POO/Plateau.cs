using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    public class Plateau
    {
        private char[,] grilleDeLettres;
        private int nombreLignes;
        private int nombreColonnes;
        private static Random generateurAleatoire = new Random();

        // Constructeur pour charger un plateau depuis un fichier CSV
        public Plateau(string cheminFichierCSV)
        {
            ChargerDepuisFichier(cheminFichierCSV);
        }

        // Constructeur pour générer un plateau aléatoire
        public Plateau(string cheminFichierLettres, int lignes, int colonnes)
        {
            nombreLignes = lignes;
            nombreColonnes = colonnes;
            grilleDeLettres = new char[lignes, colonnes];
            GenererPlateauAleatoire(cheminFichierLettres);
        }

        private void GenererPlateauAleatoire(string cheminFichierLettres)
        {
            // 1. Lire les lettres et leurs fréquences maximales
            var lignesFichier = File.ReadAllLines(cheminFichierLettres);
            Dictionary<char, int> frequencesMaxLettres = new Dictionary<char, int>();

            foreach (var ligne in lignesFichier)
            {
                var elements = ligne.Split(',');
                char lettre = char.ToUpper(elements[0][0]);
                int frequenceMax = int.Parse(elements[1]);
                frequencesMaxLettres[lettre] = frequenceMax;
            }

            // 2. Vérifier qu'il y a assez de lettres
            int totalLettresDisponibles = 0;
            foreach (var frequence in frequencesMaxLettres.Values)
                totalLettresDisponibles += frequence;

            if (totalLettresDisponibles < nombreLignes * nombreColonnes)
                throw new Exception("Pas assez de lettres pour remplir le plateau.");

            // 3. Créer une liste de lettres en respectant les fréquences
            List<char> reservoirLettres = new List<char>();
            foreach (var lettreFrequence in frequencesMaxLettres)
            {
                char lettre = lettreFrequence.Key;
                int frequence = lettreFrequence.Value;
                for (int i = 0; i < frequence; i++)
                    reservoirLettres.Add(lettre);
            }

            // 4. Mélanger les lettres
            for (int i = reservoirLettres.Count - 1; i > 0; i--)
            {
                int j = generateurAleatoire.Next(i + 1);
                (reservoirLettres[i], reservoirLettres[j]) = (reservoirLettres[j], reservoirLettres[i]);
            }

            // 5. Remplir la grille
            int index = 0;
            for (int i = 0; i < nombreLignes; i++)
            {
                for (int j = 0; j < nombreColonnes; j++)
                {
                    grilleDeLettres[i, j] = reservoirLettres[index];
                    index++;
                }
            }
        }

        public override string ToString()
        {
            string representationPlateau = "";

            // Ligne supérieure
            representationPlateau += "┌";
            for (int j = 0; j < nombreColonnes; j++)
            {
                representationPlateau += "───";
                if (j < nombreColonnes - 1)
                    representationPlateau += "┬";
            }
            representationPlateau += "┐\n";

            // Lignes du plateau
            for (int i = 0; i < nombreLignes; i++)
            {
                representationPlateau += "│";
                for (int j = 0; j < nombreColonnes; j++)
                {
                    char lettre = grilleDeLettres[i, j];
                    representationPlateau += $" {lettre} │";
                }
                representationPlateau += "\n";

                if (i < nombreLignes - 1)
                {
                    representationPlateau += "├";
                    for (int j = 0; j < nombreColonnes; j++)
                    {
                        representationPlateau += "───";
                        if (j < nombreColonnes - 1)
                            representationPlateau += "┼";
                    }
                    representationPlateau += "┤\n";
                }
            }

            // Ligne inférieure
            representationPlateau += "└";
            for (int j = 0; j < nombreColonnes; j++)
            {
                representationPlateau += "───";
                if (j < nombreColonnes - 1)
                    representationPlateau += "┴";
            }
            representationPlateau += "┘\n";

            return representationPlateau;
        }

        public List<Position> Recherche_Mot(string mot)
        {
            mot = mot.ToUpper();
            int ligneDepart = nombreLignes - 1;

            for (int colonne = 0; colonne < nombreColonnes; colonne++)
            {
                bool[,] casesUtilisees = new bool[nombreLignes, nombreColonnes];
                List<Position> cheminMot = new List<Position>();

                if (RechercherVoisins(ligneDepart, colonne, mot, 0, casesUtilisees, cheminMot))
                    return cheminMot;
            }

            return null;
        }

        private bool RechercherVoisins(int i, int j, string mot, int indexLettre, bool[,] casesUtilisees, List<Position> cheminMot)
        {
            if (i < 0 || i >= nombreLignes || j < 0 || j >= nombreColonnes)
                return false;

            if (casesUtilisees[i, j])
                return false;

            if (grilleDeLettres[i, j] != mot[indexLettre])
                return false;

            casesUtilisees[i, j] = true;
            cheminMot.Add(new Position(i, j));

            if (indexLettre == mot.Length - 1)
                return true;

            if (RechercherVoisins(i - 1, j, mot, indexLettre + 1, casesUtilisees, cheminMot) ||
                RechercherVoisins(i + 1, j, mot, indexLettre + 1, casesUtilisees, cheminMot) ||
                RechercherVoisins(i, j - 1, mot, indexLettre + 1, casesUtilisees, cheminMot) ||
                RechercherVoisins(i, j + 1, mot, indexLettre + 1, casesUtilisees, cheminMot))
                return true;

            casesUtilisees[i, j] = false;
            cheminMot.RemoveAt(cheminMot.Count - 1);
            return false;
        }

        public bool EstVide()
        {
            for (int i = 0; i < nombreLignes; i++)
            {
                for (int j = 0; j < nombreColonnes; j++)
                {
                    if (grilleDeLettres[i, j] != '\0')
                        return false;
                }
            }
            return true;
        }

        public void MettreAJourPlateau(List<Position> positionsMot)
        {
            foreach (var position in positionsMot)
                grilleDeLettres[position.I, position.J] = '\0';

            for (int j = 0; j < nombreColonnes; j++)
            {
                List<char> lettresColonne = new List<char>();
                for (int i = nombreLignes - 1; i >= 0; i--)
                {
                    if (grilleDeLettres[i, j] != '\0')
                        lettresColonne.Add(grilleDeLettres[i, j]);
                }

                int index = 0;
                for (int i = nombreLignes - 1; i >= 0; i--)
                {
                    if (index < lettresColonne.Count)
                    {
                        grilleDeLettres[i, j] = lettresColonne[index];
                        index++;
                    }
                    else
                        grilleDeLettres[i, j] = '\0';
                }
            }
        }

        public void SauvegarderDansFichier(string cheminFichier)
        {
            using (StreamWriter ecrivain = new StreamWriter(cheminFichier))
            {
                for (int i = 0; i < nombreLignes; i++)
                {
                    List<string> cellules = new List<string>();
                    for (int j = 0; j < nombreColonnes; j++)
                    {
                        if (grilleDeLettres[i, j] == '\0')
                            cellules.Add("");
                        else
                            cellules.Add(grilleDeLettres[i, j].ToString());
                    }
                    ecrivain.WriteLine(string.Join(",", cellules));
                }
            }
        }

        public void ChargerDepuisFichier(string cheminFichier)
        {
            var lignesFichier = File.ReadAllLines(cheminFichier);
            nombreLignes = lignesFichier.Length;
            nombreColonnes = lignesFichier[0].Split(',').Length;
            grilleDeLettres = new char[nombreLignes, nombreColonnes];

            for (int i = 0; i < nombreLignes; i++)
            {
                var cellules = lignesFichier[i].Split(',');
                for (int j = 0; j < nombreColonnes; j++)
                {
                    if (string.IsNullOrEmpty(cellules[j]))
                        grilleDeLettres[i, j] = '\0';
                    else
                        grilleDeLettres[i, j] = cellules[j][0];
                }
            }
        }
    }

    public class Position
    {
        public int I { get; }
        public int J { get; }

        public Position(int i, int j)
        {
            I = i;
            J = j;
        }
    }
}
