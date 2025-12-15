using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PROJETFINAL_ALGO_POO
{
    public class Jeu
    {
        private Joueur joueurUn;
        private Joueur joueurDeux;
        private Plateau plateauDeJeu;
        private Dictionnaire dictionnaireDeMots;
        private DateTime heureDebutPartie;
        private TimeSpan dureeTotalePartie = TimeSpan.FromMinutes(2); // Durée totale de la partie
        private TimeSpan dureeParTour = TimeSpan.FromSeconds(20); // Durée par tour
        private Dictionary<char, int> poidsDesLettres;

        public Jeu(Joueur joueur1, Joueur joueur2, Plateau plateau, Dictionnaire dictionnaire)
        {
            joueurUn = joueur1;
            joueurDeux = joueur2;
            plateauDeJeu = plateau;
            dictionnaireDeMots = dictionnaire;
            ChargerPoidsDesLettres("Lettres.txt");
        }

        public void DemarrerPartie()
        {
            Console.WriteLine("Début de la partie !");
            heureDebutPartie = DateTime.Now;
            Joueur joueurActif = joueurUn;

            while (DateTime.Now - heureDebutPartie < dureeTotalePartie && !plateauDeJeu.EstVide())
            {
                bool tourValide = JouerUnTour(joueurActif);
                if (!tourValide)
                    break;

                joueurActif = (joueurActif == joueurUn) ? joueurDeux : joueurUn;
            }

            Console.WriteLine("Fin de la partie !");
            AfficherScoresFinaux();
        }

        private bool JouerUnTour(Joueur joueurActif)
        {
            while (true)
            {
                TimeSpan tempsRestantPartie = dureeTotalePartie - (DateTime.Now - heureDebutPartie);
                if (tempsRestantPartie <= TimeSpan.Zero)
                    return false;

                Console.Clear();
                Console.WriteLine(plateauDeJeu.ToString());
                Console.WriteLine($"Tour de {joueurActif.Nom}");
                Console.WriteLine($"Temps restant : {(int)tempsRestantPartie.TotalSeconds} secondes");

                if (tempsRestantPartie.TotalSeconds <= 10)
                    Console.WriteLine("⚠️ Il reste moins de 10 secondes !");

                Console.Write("Proposez un mot : ");
                string motPropose = Console.ReadLine().ToUpper();

                if (motPropose.Length < 2)
                {
                    Console.WriteLine("Le mot est trop court !");
                    Console.ReadKey();
                    continue;
                }

                if (joueurActif.Contient(motPropose))
                {
                    Console.WriteLine("Ce mot a déjà été trouvé !");
                    Console.ReadKey();
                    continue;
                }

                if (!dictionnaireDeMots.RechDichoRecursif(motPropose))
                {
                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire !");
                    Console.ReadKey();
                    continue;
                }

                var positionsMot = plateauDeJeu.Recherche_Mot(motPropose);
                if (positionsMot == null)
                {
                    Console.WriteLine("Ce mot n'est pas présent sur le plateau !");
                    Console.ReadKey();
                    continue;
                }

                plateauDeJeu.MettreAJourPlateau(positionsMot);
                int scoreMot = CalculerScoreMot(motPropose);
                joueurActif.Add_Mot(motPropose);
                joueurActif.Add_Score(scoreMot);

                Console.WriteLine($"Mot validé ! Score : +{scoreMot}");
                Console.ReadKey();
                return true;
            }
        }

        private void ChargerPoidsDesLettres(string cheminFichier)
        {
            poidsDesLettres = new Dictionary<char, int>();
            string[] lignesFichier = File.ReadAllLines(cheminFichier);
            foreach (string ligne in lignesFichier)
            {
                string[] elementsLigne = ligne.Split(',');
                char lettre = elementsLigne[0][0];
                int poids = int.Parse(elementsLigne[2]);
                poidsDesLettres[lettre] = poids;
            }
        }

        private int CalculerScoreMot(string mot)
        {
            int scoreTotal = 0;
            foreach (char lettre in mot)
            {
                if (poidsDesLettres.ContainsKey(lettre))
                    scoreTotal += poidsDesLettres[lettre];
            }
            return scoreTotal * mot.Length;
        }

        private void AfficherScoresFinaux()
        {
            Console.Clear();
            Console.WriteLine("=== Résultats finaux ===\n");
            Console.WriteLine($"{joueurUn.Nom} : {joueurUn.Scores_plateau} points");
            Console.WriteLine($"{joueurDeux.Nom} : {joueurDeux.Scores_plateau} points\n");

            if (joueurUn.Scores_plateau > joueurDeux.Scores_plateau)
                Console.WriteLine($"🏆 Le gagnant est {joueurUn.Nom} !");
            else if (joueurDeux.Scores_plateau > joueurUn.Scores_plateau)
                Console.WriteLine($"🏆 Le gagnant est {joueurDeux.Nom} !");
            else
                Console.WriteLine("🤝 Match nul !");

            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}
