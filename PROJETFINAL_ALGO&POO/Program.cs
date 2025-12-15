using System;

namespace PROJETFINAL_ALGO_POO
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Bienvenue dans le jeu des MOTS GLISSÉS ===\n");

            // 1. Créer les joueurs
            Console.Write("Nom du Joueur 1 : ");
            string nomJoueur1 = Console.ReadLine();
            Joueur joueur1 = new Joueur(nomJoueur1);

            Console.Write("Nom du Joueur 2 : ");
            string nomJoueur2 = Console.ReadLine();
            Joueur joueur2 = new Joueur(nomJoueur2);

            // 2. Charger le dictionnaire
            Dictionnaire dictionnaire = new Dictionnaire("MotsFrancais.txt");

            // 3. Demander à l'utilisateur s'il veut un plateau aléatoire ou depuis un fichier
            Console.WriteLine("\nChoisissez le mode de génération du plateau :");
            Console.WriteLine("1. Plateau généré aléatoirement");
            Console.WriteLine("2. Plateau chargé depuis un fichier CSV");
            Console.Write("Votre choix (1 ou 2) : ");
            string choixPlateau = Console.ReadLine();

            Plateau plateau;

            if (choixPlateau == "1")
            {
                // Générer un plateau aléatoire
                plateau = new Plateau("Lettres.txt", 8, 8);
                Console.WriteLine("\nPlateau généré aléatoirement :");
            }
            else if (choixPlateau == "2")
            {
                // Charger un plateau depuis un fichier CSV
                Console.Write("Chemin du fichier CSV du plateau : ");
                string cheminFichierPlateau = Console.ReadLine();
                plateau = new Plateau(cheminFichierPlateau);
                Console.WriteLine("\nPlateau chargé depuis le fichier :");
            }
            else
            {
                Console.WriteLine("Choix invalide. Génération d'un plateau aléatoire par défaut.");
                plateau = new Plateau("Lettres.txt", 8, 8);
            }

            // Afficher le plateau initial
            Console.WriteLine(plateau.ToString());

            // 4. Créer le jeu
            Jeu jeu = new Jeu(joueur1, joueur2, plateau, dictionnaire);

            // 5. Démarrer la partie
            Console.WriteLine("\nLa partie va commencer ! Appuyez sur une touche pour démarrer...");
            Console.ReadKey();
            jeu.DemarrerPartie();
        }
    }
}
