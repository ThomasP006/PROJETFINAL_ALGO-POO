using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROJETFINAL_ALGO_POO;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestMotsGlisses
{
    [TestClass]
    public class UnitTest1
    {
        private string FichierDicoTest;
        private string FichierPlateauTest;

        [TestInitialize]
        public void Setup()
        {
            FichierDicoTest = Path.Combine(Path.GetTempPath(), $"test_dico_{Guid.NewGuid()}.txt");
            FichierPlateauTest = Path.Combine(Path.GetTempPath(), $"test_plateau_{Guid.NewGuid()}.csv");

            /// On crée un tableau de 26 lignes (une pour chaque lettre)
            string[] lignesDico = new string[26];
            for (int i = 0; i < 26; i++) lignesDico[i] = ""; /// On initialise tout à vide

            /// On place les mots sur les lignes correspondantes aux lettres
            lignesDico[0] = "ABRICOT ARBRE"; /// Index 0 = A
            lignesDico[1] = "BANANE";        /// Index 1 = B
            lignesDico[2] = "CERISE";        /// Index 2 = C
            lignesDico[25] = "ZOO";          /// Index 25 = Z

            File.WriteAllLines(FichierDicoTest, lignesDico);

            File.WriteAllLines(FichierPlateauTest, new string[]
            {
                "A,B,C",
                "D,E,F",
                "G,H,I"
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                if (File.Exists(FichierDicoTest)) File.Delete(FichierDicoTest);
                if (File.Exists(FichierPlateauTest)) File.Delete(FichierPlateauTest);
            }
            catch { }
        }

        [TestMethod]
        public void Test_Joueur_Add_Score()
        {
            Joueur joueur = new Joueur("Toto");
            Assert.AreEqual(0, joueur.Scores_plateau);

            joueur.Add_Score(10);
            Assert.AreEqual(10, joueur.Scores_plateau);

            joueur.Add_Score(5);
            Assert.AreEqual(15, joueur.Scores_plateau);
        }

        [TestMethod]
        public void Test_Joueur_Add_Mot_Et_Contient()
        {
            Joueur joueur = new Joueur("Tata");
            Assert.IsFalse(joueur.Contient("MAISON"));

            joueur.Add_Mot("MAISON");

            Assert.IsTrue(joueur.Contient("MAISON"));
            Assert.IsFalse(joueur.Contient("ARBRE"));
        }

        [TestMethod]
        public void Test_Dictionnaire_RechDichoRecursif()
        {
            Dictionnaire dico = new Dictionnaire(FichierDicoTest);

            Assert.IsTrue(dico.RechDichoRecursif("ABRICOT"));
            Assert.IsTrue(dico.RechDichoRecursif("BANANE"));
            Assert.IsTrue(dico.RechDichoRecursif("ZOO"));

            Assert.IsFalse(dico.RechDichoRecursif("POMME"));
            Assert.IsFalse(dico.RechDichoRecursif("VOITURE"));
        }

        [TestMethod]
        public void Test_Plateau_EstVide()
        {
            string fichierVide = Path.Combine(Path.GetTempPath(), $"vide_{Guid.NewGuid()}.csv");
            File.WriteAllLines(fichierVide, new string[] { " , , ", " , , " });

            try
            {
                Plateau plateauVide = new Plateau(fichierVide);
                Assert.IsTrue(plateauVide.EstVide());
            }
            finally
            {
                if (File.Exists(fichierVide)) File.Delete(fichierVide);
            }

            Plateau plateauPlein = new Plateau(FichierPlateauTest);
            Assert.IsFalse(plateauPlein.EstVide());
        }

        [TestMethod]
        public void Test_Plateau_RechercherMot()
        {
            string fichierTestRecherche = Path.Combine(Path.GetTempPath(), $"recherche_{Guid.NewGuid()}.csv");

            /// On construit un plateau où le mot "MOT" commence en bas et monte
            /// Ligne 0 : T , X , X
            /// Ligne 1 : O , X , X
            /// Ligne 2 : M , X , X
            File.WriteAllLines(fichierTestRecherche, new string[]
            {
                "T,X,X",
                "O,X,X",
                "M,X,X"
            });

            try
            {
                Plateau plateau = new Plateau(fichierTestRecherche);

                List<Position> resultat = plateau.RechercherMot("MOT");

                /// Le test devrait maintenant réussir car le M est sur la base (ligne 2)
                Assert.IsNotNull(resultat, "Le mot MOT n'a pas été trouvé alors qu'il part de la base.");
                Assert.AreEqual(3, resultat.Count);

                /// Vérification qu'un mot qui ne part pas de la base n'est pas trouvé
                List<Position> resultatInexistant = plateau.RechercherMot("RIZ");
                Assert.IsNull(resultatInexistant);
            }
            finally
            {
                if (File.Exists(fichierTestRecherche)) File.Delete(fichierTestRecherche);
            }
        }
    }
}