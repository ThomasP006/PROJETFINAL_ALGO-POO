/**using System;

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
}**/
using System;

namespace PROJETFINAL_ALGO_POO
{
    class Program
    {
        static void Main(string[] args)
        {
            // Affichage du titre du jeu
            Console.WriteLine("============================================");
            Console.WriteLine("=== Bienvenue dans le jeu des MOTS GLISSÉS ===");
            Console.WriteLine("============================================");
            Console.WriteLine();

            // 1. Créer les joueurs
            Console.Write("Veuillez entrer le nom du Joueur 1 : ");
            string nom_joueur_un = Console.ReadLine();
            Joueur joueur_un = new Joueur(nom_joueur_un);

            Console.Write("Veuillez entrer le nom du Joueur 2 : ");
            string nom_joueur_deux = Console.ReadLine();
            Joueur joueur_deux = new Joueur(nom_joueur_deux);

            // Affichage des informations des joueurs
            Console.WriteLine(joueur_un.toString());
            Console.WriteLine(joueur_deux.toString());

            // 2. Charger le dictionnaire
            Dictionnaire dictionnaire = new Dictionnaire("Mots_Français.txt");

            // Affichage des informations du dictionnaire
            Console.WriteLine(dictionnaire.toString());

            // 3. Demander à l'utilisateur s'il veut un plateau aléatoire ou depuis un fichier
            Console.WriteLine("\nChoisissez le mode de génération du plateau :");
            Console.WriteLine("1. Plateau généré aléatoirement");
            Console.WriteLine("2. Plateau chargé depuis un fichier CSV");
            Console.Write("Votre choix (1 ou 2) : ");
            string choix_plateau = Console.ReadLine();

            Plateau plateau;

            if (choix_plateau == "1")
            {
                // Générer un plateau aléatoire
                plateau = new Plateau("Lettres.txt", 8, 8);
                Console.WriteLine("\nPlateau généré aléatoirement :");
            }
            else if (choix_plateau == "2")
            {
                // Charger un plateau depuis un fichier CSV
                Console.Write("Veuillez entrer le chemin du fichier CSV du plateau : ");
                string chemin_fichier_plateau = Console.ReadLine();
                plateau = new Plateau(chemin_fichier_plateau);
                Console.WriteLine("\nPlateau chargé depuis le fichier :");
            }
            else
            {
                Console.WriteLine("Choix invalide. Génération d'un plateau aléatoire par défaut.");
                plateau = new Plateau("Lettres.txt", 8, 8);
            }

            // Affichage du plateau initial
            Console.WriteLine("\nVoici le plateau de jeu :");
            Console.WriteLine(plateau.ToString());

            // 4. Créer le jeu
            Jeu jeu = new Jeu(joueur_un, joueur_deux, plateau, dictionnaire);

            // 5. Démarrer la partie
            Console.WriteLine("\nLa partie va commencer ! Appuyez sur une touche pour démarrer...");
            Console.ReadKey();
            jeu.DemarrerPartie();
        }
    }
}
