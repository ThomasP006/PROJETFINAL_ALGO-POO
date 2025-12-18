using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    public class Jeu
    {
        // On met nos attributs privés de la classe
        private Joueur joueur_un; // Il y a déjà deux joueurs pour jouer
        private Joueur joueur_deux;
        private Plateau plateau_de_jeu; // Un plateau que l'on a déjà créé dans la classe Plateau
        private Dictionnaire dictionnaire_de_mots; // Un dictionnaire qui nous permet de vérifier les mots entrés
        private DateTime heure_de_debut_partie; // L'heure de début de la partie
        private TimeSpan duree_totale_partie = TimeSpan.FromMinutes(2); // Une durée de jeu totale de 2 minutes
        private TimeSpan duree_par_tour = TimeSpan.FromSeconds(20); // Une durée de tour par joueur de 20 secondes
        private Dictionary<char, int> poids_des_lettres; // Dictionnaire pour stocker le poids (score) de chaque lettre

        // Pour jouer au jeu, il faut deux joueurs, un plateau et un dictionnaire, c'est pour ça qu'on fait le constructeur de la classe à partir de ces attributs
        public Jeu(Joueur joueur1, Joueur joueur2, Plateau plateau, Dictionnaire dictionnaire)
        {
            joueur_un = joueur1; // On assigne le premier joueur
            joueur_deux = joueur2; // On assigne le deuxième joueur
            plateau_de_jeu = plateau; // On assigne le plateau de jeu
            dictionnaire_de_mots = dictionnaire; // On assigne le dictionnaire de mots
            ChargerPoidsDesLettres("Lettres.txt"); // On charge les poids des lettres depuis un fichier
        }

        // Méthode pour charger les poids des lettres depuis un fichier
        private void ChargerPoidsDesLettres(string chemin_du_fichier)
        {
            poids_des_lettres = new Dictionary<char, int>(); // On initialise le dictionnaire des poids des lettres
            string[] lignes_du_fichier = File.ReadAllLines(chemin_du_fichier); // On lit toutes les lignes du fichier
            foreach (string ligne in lignes_du_fichier) // On parcourt chaque ligne du fichier
            {
                string[] elements_ligne = ligne.Split(','); // On sépare chaque ligne en éléments en utilisant la virgule comme séparateur
                char lettre = elements_ligne[0][0]; // On récupère la lettre (premier élément de la première sous-chaîne)
                int poids = int.Parse(elements_ligne[2]); // On récupère le poids de la lettre (troisième élément)
                poids_des_lettres[lettre] = poids; // On ajoute la lettre et son poids au dictionnaire
            }
        }

        // Méthode pour démarrer la partie
        public void DemarrerPartie()
        {
            Console.WriteLine("La partie va commencer!!!"); // On annonce le début de la partie
            heure_de_debut_partie = DateTime.Now; // On enregistre l'heure de début de la partie
            Joueur joueur_actif = joueur_un; // On commence avec le premier joueur

            // Tant que la durée totale de la partie n'est pas écoulée et que le plateau n'est pas vide
            while (DateTime.Now - heure_de_debut_partie < duree_totale_partie && !plateau_de_jeu.EstVide())
            {
                bool tour_valide = JouerUnTour(joueur_actif); // On joue un tour avec le joueur actif
                if (!tour_valide) // Si le tour n'est pas valide (temps écoulé)
                    break; // On sort de la boucle

                // On alterne entre les joueurs
                joueur_actif = (joueur_actif == joueur_un) ? joueur_deux : joueur_un;
            }

            Console.WriteLine("La partie est terminée!!! Bravo aux deux joueurs!"); // On annonce la fin de la partie
            AfficherScoresFinaux(); // On affiche les scores finaux
        }

        // Méthode pour jouer un tour
        private bool JouerUnTour(Joueur joueur_actif)
        {
            while (true) // Boucle infinie jusqu'à ce qu'un tour valide soit joué
            {
                TimeSpan temps_restant_partie = duree_totale_partie - (DateTime.Now - heure_de_debut_partie); // On calcule le temps restant pour la partie
                if (temps_restant_partie <= TimeSpan.Zero) // Si le temps de la partie est écoulé
                    return false; // On retourne false pour indiquer que la partie est terminée

                // Réinitialise la console à chaque tour
                Console.Clear();

                // Affiche le plateau et les informations du tour
                Console.WriteLine(plateau_de_jeu.ToString()); // On affiche le plateau de jeu
                Console.WriteLine($"Au tour de {joueur_actif.Nom}"); // On indique le joueur actif
                Console.WriteLine($"Il reste {(int)temps_restant_partie.TotalSeconds} secondes"); // On affiche le temps restant

                if (temps_restant_partie.TotalSeconds <= 10) // Si le temps restant est inférieur ou égal à 10 secondes
                    Console.WriteLine("Attention au temps, moins de 10 secondes restantes!"); // On avertit le joueur

                Console.Write("Tente un mot ! "); // On demande au joueur de proposer un mot
                string mot_propose = Console.ReadLine().ToUpper(); // On lit le mot proposé et on le met en majuscules

                // Vérifie si le mot est valide
                if (mot_propose.Length < 2) // Si le mot est trop court
                {
                    Console.WriteLine("Le mot est trop court !"); // On informe le joueur
                    Console.ReadKey(); // On attend que le joueur appuie sur une touche
                    continue; // On recommence le tour
                }

                if (joueur_actif.Contient(mot_propose)) // Si le joueur a déjà trouvé ce mot
                {
                    Console.WriteLine("Ce mot a déjà été trouvé !"); // On informe le joueur
                    Console.ReadKey(); // On attend que le joueur appuie sur une touche
                    continue; // On recommence le tour
                }

                if (!dictionnaire_de_mots.RechDichoRecursif(mot_propose)) // Si le mot n'est pas dans le dictionnaire
                {
                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire !"); // On informe le joueur
                    Console.ReadKey(); // On attend que le joueur appuie sur une touche
                    continue; // On recommence le tour
                }

                var positions_mot = plateau_de_jeu.RechercherMot(mot_propose); // On recherche le mot sur le plateau
                if (positions_mot == null) // Si le mot n'est pas sur le plateau
                {
                    Console.WriteLine("On dirait bien que le mot n'est pas sur le plateau !"); // On informe le joueur
                    Console.ReadKey(); // On attend que le joueur appuie sur une touche
                    continue; // On recommence le tour
                }

                // Met à jour le plateau et le score
                plateau_de_jeu.MettreAJourPlateau(positions_mot); // On met à jour le plateau en supprimant les lettres du mot trouvé
                int score_mot = CalculerScoreMot(mot_propose); // On calcule le score du mot
                joueur_actif.Add_Mot(mot_propose); // On ajoute le mot à la liste des mots trouvés par le joueur
                joueur_actif.Add_Score(score_mot); // On ajoute le score du mot au score total du joueur

                // Réinitialise la console pour afficher le message de félicitations
                Console.Clear();
                Console.WriteLine(plateau_de_jeu.ToString()); // On affiche le plateau mis à jour
                Console.WriteLine($"Bravo tu as trouvé un mot, tu gagnes {score_mot} points !"); // On félicite le joueur
                Console.ReadKey(); // On attend que le joueur appuie sur une touche
                return true; // On retourne true pour indiquer que le tour s'est bien passé
            }
        }

        // Méthode pour calculer le score d'un mot
        private int CalculerScoreMot(string mot)
        {
            int score_total = 0; // On initialise le score total à 0
            foreach (char lettre in mot) // Pour chaque lettre du mot
            {
                if (poids_des_lettres.ContainsKey(lettre)) // Si la lettre a un poids défini
                    score_total += poids_des_lettres[lettre]; // On ajoute le poids de la lettre au score total
            }
            return score_total * mot.Length; // On retourne le score total multiplié par la longueur du mot
        }

        // Méthode pour afficher les scores finaux
        private void AfficherScoresFinaux()
        {
            Console.Clear(); // On nettoie la console
            Console.WriteLine("Les résultats viennent de tomber\n"); // On annonce les résultats
            Console.WriteLine($"{joueur_un.Nom} : {joueur_un.Scores_plateau} points"); // On affiche le score du premier joueur
            Console.WriteLine($"{joueur_deux.Nom} : {joueur_deux.Scores_plateau} points\n"); // On affiche le score du deuxième joueur

            // On détermine le gagnant
            if (joueur_un.Scores_plateau > joueur_deux.Scores_plateau)
                Console.WriteLine($"Bravo à notre champion du jour : {joueur_un.Nom} !"); // Le premier joueur a gagné
            else if (joueur_deux.Scores_plateau > joueur_un.Scores_plateau)
                Console.WriteLine($"Bravo à notre champion du jour : {joueur_deux.Nom} !"); // Le deuxième joueur a gagné
            else
                Console.WriteLine("C'est triste.... Personne n'a gagné"); // match  nul
            Console.WriteLine("\nAppuyez sur  une touche pour quitter..."); // On demande à l'utilisateur d'appuyer sur une touche pour quitter
            Console.ReadKey(); // On va attendre    que l'utilisateur appuie sur une touche
        }
    }
}
