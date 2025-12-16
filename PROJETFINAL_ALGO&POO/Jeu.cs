using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    public class Jeu
    {
        // Attributs de la classe Jeu
        private Joueur joueur_un;
        private Joueur joueur_deux;
        private Plateau plateau_de_jeu;
        private Dictionnaire dictionnaire_de_mots;
        private DateTime heure_de_debut_partie;
        private TimeSpan duree_totale_partie = TimeSpan.FromMinutes(2); // Durée totale de la partie
        private TimeSpan duree_par_tour = TimeSpan.FromSeconds(20); // Durée par tour
        private Dictionary<char, int> poids_des_lettres;

        // Constructeur : initialise le jeu avec deux joueurs, un plateau et un dictionnaire
        public Jeu(Joueur joueur1, Joueur joueur2, Plateau plateau, Dictionnaire dictionnaire)
        {
            joueur_un = joueur1;
            joueur_deux = joueur2;
            plateau_de_jeu = plateau;
            dictionnaire_de_mots = dictionnaire;
            ChargerPoidsDesLettres("Lettres.txt");
        }

        // Charge les poids des lettres depuis le fichier
        private void ChargerPoidsDesLettres(string chemin_du_fichier)
        {
            poids_des_lettres = new Dictionary<char, int>();
            string[] lignes_du_fichier = File.ReadAllLines(chemin_du_fichier);
            foreach (string ligne in lignes_du_fichier)
            {
                string[] elements_ligne = ligne.Split(',');
                char lettre = elements_ligne[0][0];
                int poids = int.Parse(elements_ligne[2]);
                poids_des_lettres[lettre] = poids;
            }
        }

        // Démarre la partie
        public void DemarrerPartie()
        {
            Console.WriteLine("Début de la partie !");
            heure_de_debut_partie = DateTime.Now;
            Joueur joueur_actif = joueur_un;

            // Boucle principale du jeu
            while (DateTime.Now - heure_de_debut_partie < duree_totale_partie && !plateau_de_jeu.EstVide())
            {
                bool tour_valide = JouerUnTour(joueur_actif);
                if (!tour_valide)
                    break;

                // Change de joueur
                joueur_actif = (joueur_actif == joueur_un) ? joueur_deux : joueur_un;
            }

            Console.WriteLine("Fin de la partie !");
            AfficherScoresFinaux();
        }

        // Gère un tour de jeu pour un joueur
        private bool JouerUnTour(Joueur joueur_actif)
        {
            while (true)
            {
                TimeSpan temps_restant_partie = duree_totale_partie - (DateTime.Now - heure_de_debut_partie);
                if (temps_restant_partie <= TimeSpan.Zero)
                    return false;

                Console.Clear();
                Console.WriteLine(plateau_de_jeu.ToString());
                Console.WriteLine($"Tour de {joueur_actif.Nom}");
                Console.WriteLine($"Temps restant : {(int)temps_restant_partie.TotalSeconds} secondes");

                if (temps_restant_partie.TotalSeconds <= 10)
                    Console.WriteLine("⚠️ Il reste moins de 10 secondes !");

                Console.Write("Proposez un mot : ");
                string mot_propose = Console.ReadLine().ToUpper();

                // Vérifie si le mot est valide
                if (mot_propose.Length < 2)
                {
                    Console.WriteLine("Le mot est trop court !");
                    Console.ReadKey();
                    continue;
                }

                if (joueur_actif.Contient(mot_propose))
                {
                    Console.WriteLine("Ce mot a déjà été trouvé !");
                    Console.ReadKey();
                    continue;
                }

                if (!dictionnaire_de_mots.RechDichoRecursif(mot_propose))
                {
                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire !");
                    Console.ReadKey();
                    continue;
                }

                var positions_mot = plateau_de_jeu.RechercherMot(mot_propose);
                if (positions_mot == null)
                {
                    Console.WriteLine("Ce mot n'est pas présent sur le plateau !");
                    Console.ReadKey();
                    continue;
                }

                // Met à jour le plateau et le score
                plateau_de_jeu.MettreAJourPlateau(positions_mot);
                int score_mot = CalculerScoreMot(mot_propose);
                joueur_actif.Add_Mot(mot_propose);
                joueur_actif.Add_Score(score_mot);

                Console.WriteLine($"Mot validé ! Score : +{score_mot}");
                Console.ReadKey();
                return true;
            }
        }

        // Calcule le score d'un mot
        private int CalculerScoreMot(string mot)
        {
            int score_total = 0;
            foreach (char lettre in mot)
            {
                if (poids_des_lettres.ContainsKey(lettre))
                    score_total += poids_des_lettres[lettre];
            }
            return score_total * mot.Length;
        }

        // Affiche les scores finaux
        private void AfficherScoresFinaux()
        {
            Console.Clear();
            Console.WriteLine("=== Résultats finaux ===\n");
            Console.WriteLine($"{joueur_un.Nom} : {joueur_un.Scores_plateau} points");
            Console.WriteLine($"{joueur_deux.Nom} : {joueur_deux.Scores_plateau} points\n");

            if (joueur_un.Scores_plateau > joueur_deux.Scores_plateau)
                Console.WriteLine($"🏆 Le gagnant est {joueur_un.Nom} !");
            else if (joueur_deux.Scores_plateau > joueur_un.Scores_plateau)
                Console.WriteLine($"🏆 Le gagnant est {joueur_deux.Nom} !");
            else
                Console.WriteLine("🤝 Match nul !");

            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}
