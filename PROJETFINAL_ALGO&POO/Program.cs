using System;

namespace PROJETFINAL_ALGO_POO
{
    class Program
    {
        static void Main(string[] args)
        {
            // On nettoie la console pour commencer avec une page vierge
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkRed; // On met le texte en rouge foncé
            Console.BackgroundColor = ConsoleColor.DarkYellow; // On met le fond en jaune foncé
            Console.WriteLine(@" __  __                    ____       _            _             _
|  \/  | ___ _ __  _   _  |  _ \ _ __(_)_ __   ___(_)_ __   __ _| |
| |\/| |/ _ \ '_ \| | | | | |_) | '__| | '_ \ / __| | '_ \ / _` | |
| |  | |  __/ | | | |_| | |  __/| |  | | | | | (__| | |_) | (_| | |
|_|  |_|\___|_| |_|\__,_| |_|   |_|  |_|_| |_|\___|_| .__/ \__,_|_|
                                                    |_|            ");


            // 1. On demande à l'utilisateur de créer le premier joueur
            Console.Write("Quel sera bien le nom de notre premier concurrent ? Je le laisse saisir son nom : ");
            string nom_joueur_un = Console.ReadLine(); // On lit le nom du premier joueur
            Joueur joueur_un = new Joueur(nom_joueur_un); // On crée un objet Joueur pour le premier joueur

            // On demande à l'utilisateur de créer le deuxième joueur
            Console.Write("Qui sera donc l'adversaire du féroce " + nom_joueur_un + " ? Qu'il saisisse son nom : ");
            string nom_joueur_deux = Console.ReadLine(); // On lit le nom du deuxième joueur
            Joueur joueur_deux = new Joueur(nom_joueur_deux); // On crée un objet Joueur pour le deuxième joueur

            // 2. On charge le dictionnaire de mots pour vérifier les mots trouvés
            Dictionnaire dictionnaire = new Dictionnaire("Mots_Français.txt");

            // 3. On demande à l'utilisateur comment il veut générer le plateau de jeu
            Console.WriteLine("\nDe quelle manière les deux adversaires vont-ils vouloir jouer ? Ont-ils apporté leur plateau CSV en main propre ou veulent-ils jouer en terrain inconnu ?");
            Console.WriteLine("\nQu'ils tapent 1 s'ils veulent un plateau généré aléatoirement");
            Console.WriteLine("\nOu 2 s'ils veulent charger un plateau depuis un fichier CSV");
            Console.Write("C'est votre choix: ");
            string choix_plateau = Console.ReadLine(); // On lit le choix de l'utilisateur

            Plateau plateau; // On déclare une variable pour stocker le plateau

            if (choix_plateau == "1")
            {
                // Si l'utilisateur choisit 1, on génère un plateau aléatoire
                plateau = new Plateau("Lettres.txt", 8, 8);
                Console.WriteLine("\nPlateau généré aléatoirement :");
            }
            else if (choix_plateau == "2")
            {
                // Si l'utilisateur choisit 2, on charge un plateau depuis un fichier CSV
                Console.Write("Quel est donc le nom de votre fichier ? Je vous laisse le saisir : ");
                string chemin_fichier_plateau = Console.ReadLine(); // On lit le chemin du fichier
                plateau = new Plateau(chemin_fichier_plateau);
                Console.WriteLine("\nPlateau chargé depuis le fichier :");
            }
            else
            {
                // Si le choix est invalide, on génère un plateau aléatoire par défaut
                Console.WriteLine("Choix invalide. Génération d'un plateau aléatoire par défaut.");
                plateau = new Plateau("Lettres.txt", 8, 8);
            }

            // On affiche le plateau de jeu initial
            Console.WriteLine("\nVoici le plateau de jeu :");
            Console.WriteLine(plateau.ToString());

            // 4. On crée une instance du jeu avec les joueurs, le plateau et le dictionnaire
            Jeu jeu = new Jeu(joueur_un, joueur_deux, plateau, dictionnaire);

            // 5. On démarre la partie
            Console.WriteLine("\nLa partie va commencer !\nC'est à la guerre que partent nos deux adversaires du jour ! C'est parti !\nAppuyez sur une touche pour démarrer...");
            Console.ReadKey(); // On attend que l'utilisateur appuie sur une touche pour commencer
            jeu.DemarrerPartie(); // On démarre la partie
        }
    }
}
