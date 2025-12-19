using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    /// La classe "Jeu" est la classe principale
    /// Elle va faire le lien entre les joueurs, le plateau et le dictionnaire pour que la partie se déroule correctement
    public class Jeu
    {
        /// On déclare ici tous les éléments nécessaires au bon fonctionnement de la partie
        private Joueur joueur_un;
        private Joueur joueur_deux;
        private Plateau plateau_de_jeu;
        private Dictionnaire dictionnaireMots;

        /// Gestion du temps : début, durée totale et temps par tour
        private DateTime heure_de_debut_partie;
        private TimeSpan duree_totale_partie = TimeSpan.FromMinutes(2);
        private TimeSpan duree_par_tour = TimeSpan.FromSeconds(20);
        private Dictionary<char, int> poids_lettres;

        /// Le constructeur initialise la partie
        /// On a besoin des deux joueurs, du plateau préparé et du dictionnaire chargé
        /// On en profite aussi pour charger la valeur des points de chaque lettre
        public Jeu(Joueur joueur1, Joueur joueur2, Plateau plateau, Dictionnaire dictionnaire)
        {
            joueur_un = joueur1;
            joueur_deux = joueur2;
            plateau_de_jeu = plateau;
            dictionnaireMots = dictionnaire;
            ChargerPoidsDesLettres("Lettres.txt");
        }

        /// Cette méthode va lire le fichier "Lettres.txt"
        /// Elle sert à savoir combien de points rapporte chaque lettre pour le calcul des scores
        private void ChargerPoidsDesLettres(string chemin_du_fichier)
        {
            poids_lettres = new Dictionary<char, int>();
            string[] lignes_du_fichier = File.ReadAllLines(chemin_du_fichier);
            foreach (string ligne in lignes_du_fichier)
            {
                string[] elements_ligne = ligne.Split(',');
                char lettre = elements_ligne[0][0];
                int poids = int.Parse(elements_ligne[2]);
                poids_lettres[lettre] = poids;
            }
        }

        /// Cette méthode lance la boucle principale du jeu
        /// Elle gère le chronomètre global et l'alternance entre les joueurs tant que la partie n'est pas finie
        public void DemarrerPartie()
        {
            Console.WriteLine("La partie va commencer!!!");
            heure_de_debut_partie = DateTime.Now;
            Joueur joueur_actif = joueur_un;

            /// On boucle tant qu'il reste du temps et qu'il reste des lettres sur le plateau
            while (DateTime.Now - heure_de_debut_partie < duree_totale_partie && !plateau_de_jeu.EstVide())
            {
                bool tour_valide = JouerUnTour(joueur_actif);
                /// Si le temps s'est écoulé pendant un tour, on arrête tout de suite
                if (!tour_valide)
                    break;

                /// On passe la main à l'autre joueur
                joueur_actif = (joueur_actif == joueur_un) ? joueur_deux : joueur_un;
            }

            Console.WriteLine("La partie est terminée!!! Bravo aux deux joueurs!");
            AfficherScoresFinaux();
        }

        /// Cette méthode gère toute la logique d'un tour de jeu pour un joueur donné
        /// Affichage, saisie du mot, vérifications et mise à jour des scores
        private bool JouerUnTour(Joueur joueur_actif)
        {
            /// On utilise une boucle pour redemander un mot tant que le joueur se trompe ou ne trouve pas
            while (true)
            {
                /// On vérifie d'abord combien de temps il reste au total
                TimeSpan temps_restant_partie = duree_totale_partie - (DateTime.Now - heure_de_debut_partie);
                if (temps_restant_partie <= TimeSpan.Zero)
                    return false;

                Console.Clear();

                /// On affiche l'état actuel du jeu à l'utilisateur
                Console.WriteLine(plateau_de_jeu.ToString());
                Console.WriteLine($"Au tour de {joueur_actif.Nom}");
                Console.WriteLine($"Il reste {(int)temps_restant_partie.TotalSeconds} secondes");

                /// Si la fin approche, on prévient le joueur
                if (temps_restant_partie.TotalSeconds <= 10)
                    Console.WriteLine("Attention au temps, moins de 10 secondes restantes!");

                Console.Write("Tente un mot ! ");
                string mot_propose = Console.ReadLine().ToUpper();

                /// 1ère vérif : Le mot est-il assez long ?
                if (mot_propose.Length < 2)
                {
                    Console.WriteLine("Le mot est trop court !");
                    Console.ReadKey();
                    continue;
                }

                /// 2ème vérif : Le joueur a-t-il déjà trouvé ce mot ?
                if (joueur_actif.Contient(mot_propose))
                {
                    Console.WriteLine("Ce mot a déjà été trouvé !");
                    Console.ReadKey();
                    continue;
                }

                /// 3ème vérif : Le mot existe-t-il dans le dictionnaire français ?
                if (!dictionnaireMots.RechDichoRecursif(mot_propose))
                {
                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire !");
                    Console.ReadKey();
                    continue;
                }

                /// 4ème vérif : Le mot est-il présent sur le plateau en partant du bas ?
                var positions_mot = plateau_de_jeu.RechercherMot(mot_propose);
                if (positions_mot == null)
                {
                    Console.WriteLine("On dirait bien que le mot n'est pas sur le plateau !");
                    Console.ReadKey();
                    continue;
                }

                /// Une fois qu'on a tout vérifié, on met à jour le jeu : on enlève les lettres, on compte les points
                plateau_de_jeu.MettreAJourPlateau(positions_mot);
                int score_mot = CalculerScoreMot(mot_propose);
                joueur_actif.Add_Mot(mot_propose);
                joueur_actif.Add_Score(score_mot);

                Console.Clear();
                Console.WriteLine(plateau_de_jeu.ToString());
                Console.WriteLine($"Bravo tu as trouvé un mot, tu gagnes {score_mot} points !");
                Console.ReadKey();
                return true;
            }
        }

        /// Calcule le score d'un mot en fonction du poids de ses lettres et de sa longueur
        private int CalculerScoreMot(string mot)
        {
            int score_total = 0;
            foreach (char lettre in mot)
            {
                if (poids_lettres.ContainsKey(lettre))
                    score_total += poids_lettres[lettre];
            }
            /// On multiplie le total par la longueur du mot pour privilégier les mots longs
            return score_total * mot.Length;
        }

        /// Méthode de fin de partie. Elle affiche le bilan, compare les scores et désigne le vainqueur
        private void AfficherScoresFinaux()
        {
            Console.Clear();
            Console.WriteLine("Les résultats viennent de tomber\n");
            Console.WriteLine($"{joueur_un.Nom} : {joueur_un.Scores_plateau} points");
            Console.WriteLine($"{joueur_deux.Nom} : {joueur_deux.Scores_plateau} points\n");

            /// On regarde qui a fait le meilleur score
            if (joueur_un.Scores_plateau > joueur_deux.Scores_plateau)
                Console.WriteLine($"Bravo à notre champion du jour : {joueur_un.Nom} !");
            else if (joueur_deux.Scores_plateau > joueur_un.Scores_plateau)
                Console.WriteLine($"Bravo à notre champion du jour : {joueur_deux.Nom} !");
            else
                Console.WriteLine("C'est triste.... Personne n'a gagné");

            Console.WriteLine("\nAppuyez sur  une touche pour quitter...");
            Console.ReadKey();
        }
    }
}