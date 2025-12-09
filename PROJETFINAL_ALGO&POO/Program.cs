namespace PROJETFINAL_ALGO_POO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== TEST DE LA CLASSE JOUEUR ===\n");

            // 1. Test du constructeur
            Joueur j = new Joueur("Alice");
            Console.WriteLine(">> Joueur créé !");
            Console.WriteLine(j.toString());
            Console.WriteLine("---------------------------------\n");

            // 2. Test de Add_Mot
            Console.WriteLine(">> Test Add_Mot()");
            j.Add_Mot("MAISON");
            j.Add_Mot("ARBRE");
            j.Add_Mot("SOLEIL");
            Console.WriteLine("Mots ajoutés avec Add_Mot().");
            Console.WriteLine(j.toString());
            Console.WriteLine("---------------------------------\n");

            // 3. Test de Contient
            Console.WriteLine(">> Test Contient()");
            Console.WriteLine("Contient 'MAISON' ? " + j.Contient("MAISON"));
            Console.WriteLine("Contient 'VOITURE' ? " + j.Contient("VOITURE"));
            Console.WriteLine("---------------------------------\n");

            // 4. Test Add_Score
            Console.WriteLine(">> Test Add_Score()");
            j.Add_Score(10);
            j.Add_Score(15);
            Console.WriteLine("Score ajouté via Add_Score().");
            Console.WriteLine(j.toString());
            Console.WriteLine("---------------------------------\n");

            // 5. Test final de toString()
            Console.WriteLine(">> Test final toString()");
            Console.WriteLine(j.toString());
            Console.WriteLine("\n=== TEST TERMINÉ ===");
            
            string nomFichier = @"C:\Users\mathu\Documents\GitHub\PROJETFINAL_ALGO-POO\PROJETFINAL_ALGO&POO\bin\Debug\net8.0\Mots_Français.txt";

            if (!File.Exists(nomFichier))
            {
                Console.WriteLine($"ATTENTION : Le fichier {nomFichier} n'est pas trouvé !");
                Console.WriteLine("Placez-le dans le dossier de l'exécutable (bin/Debug/...).");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Chargement et Tri du fichier '{nomFichier}'...");
            DateTime debut = DateTime.Now;

            // 1. INSTANCIATION (Lecture + Tri Hybride QuickSort)
            Dictionnaire monDico = new Dictionnaire(nomFichier);

            DateTime fin = DateTime.Now;
            Console.WriteLine($"Dictionnaire chargé et trié en : {(fin - debut).TotalMilliseconds} ms !");

            // 2. TEST DE L'AFFICHAGE
            // On n'affiche pas tout le toString() car c'est trop long, juste un aperçu
            Console.WriteLine("\n--- Aperçu du contenu ---");
            Console.WriteLine(monDico.toString());

            // 3. TEST DE RECHERCHE SUR DES VRAIS MOTS
            Console.WriteLine("\n--- Test de Recherche ---");
            string[] motsATester = { "MAISON", "ETUDIANT", "ALGORITHME", "XYLOPHONE", "INEXISTANTXYZ" };

            foreach (string mot in motsATester)
            {
                bool trouve = monDico.RechDichoRecursif(mot);
                Console.WriteLine($"Mot '{mot}' : " + (trouve ? "TROUVÉ (OK)" : "Non trouvé"));
            }

            Console.ReadKey();
        }
    }

}


