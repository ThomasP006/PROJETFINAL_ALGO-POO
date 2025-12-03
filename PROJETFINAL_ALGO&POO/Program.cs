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
            
        }
    }

}


