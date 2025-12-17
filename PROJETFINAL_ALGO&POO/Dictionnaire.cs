using System;
using System.Collections.Generic;
using System.IO;

namespace PROJETFINAL_ALGO_POO
{
    public class Dictionnaire
    {
        // -----on initialise d'abord les attributs-----
        private List<string>[] mots;
        /**Cet attribut est un tableau de listes.
         Chaque liste correspondera plus tard à une lettre de l'alphabet
         Il y aura donc 26 listes au total**/
       
        // -----on fait le constructeur-----
        public Dictionnaire(string nomdufichier)
        {
            this.mots = new List<string>[26]; // Comme dit plus haut on veut un tableau de 26 listes pour chaque lettre de l'alphabet
            InitialiserLeDictionnaire(nomdufichier); // On utilise la fonction que l'on a créer plus bas pour charger chaque mot du fichier dans notre tableau de liste
            for (int i = 0; i < 26; i++)
            {
                if (mots[i] != null && mots[i].Count > 1)
                {
                    TriParQuickSort(mots[i], 0, mots[i].Count - 1); // On trie chaque liste du tableau avec le tri par quicksort
                }
            }
        }


        public void InitialiserLeDictionnaire(string chemindufichier)
        {
            try /** On peut souvent avoir des erreurs lorsque l'on cherche à lire un fichier donc on
            utilise un try/catch comme ca s'il y a une erreur de lecture, on affiche le problème 
            et le programme continue.**/
            {
                using (StreamReader lecteur_fichier = new StreamReader(chemindufichier))
                /** Le Streamreader va permettre d'ouvrir le fichier dont le chemin d'accès est donné en
                paramètre pour ensuite extirper du fichier les mots pour les mettre dans les listes
                **/
                {
                    string? ligne; 
                    /** On initialise une variable string qui peut prendre la valeur null
                    d'où le "?" Cette variable va stocker les lignes de notre fichier
                    **/
                    int compteurLigne = 0; 
                    /**le compteur va nous permettre de changer de liste à chaque incrémentation
                    si compteurligne = 0 alors on est sur la liste de mots commencant en A puis B etc..**/
                    while ((ligne = lecteur_fichier.ReadLine()) != null && compteurLigne < 26)
                    /** Cette boucle while va nous permettre de lire toutes les lignes du 
                    fichier une une par une jusqu'à ce que les lignes du fichiers soit vides ou que on est arrivé à la fin de l'alphabet
                    ce qui signifique que tous les mots du fichier ont été rentrés dans les listes.
                    **/
                    {

                        mots[compteurLigne] = new List<string>();
                        /** On initialise chaque liste du tableau mots à chaque incrémentation de notre compteur**/
                        string motEnCours = ""; /** Cette variable permet de récuperer chaque lettre 
                        d'un mot de la ligne et de le construire jusqu'à arriver à un espace**/

                        foreach (char c in ligne) /**Notre boucle va permettre de construire les mots 
                        ou de les ajouters à notre liste selon si le caractère observé est une lettre 
                        ,et dans ce cas là il appartient encore à notre mot, ou si c'est un espace**/
                        {
                            if (c != ' ') // Dans le cas où le caractère n'est pas un espace
                            {
                                motEnCours += c; // On ajoute le caractère à notre mot 
                            }
                            else // Dans le cas où le caractère est un espace
                            {
                                if (!string.IsNullOrEmpty(motEnCours)) /**Si le mot que l'on a construit n'est pas 
                                un espace(dans le cas où il y aurait plusieurs espaces entre les mots dans le fichier)**/
                                {
                                    mots[compteurLigne].Add(motEnCours.ToUpper()); // On ajoute le mot à la liste
                                    motEnCours = ""; // On va reconstruire un mot après notre espace donc on remet la variable à vide.
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(motEnCours))
                        {
                            mots[compteurLigne].Add(motEnCours.ToUpper()); 
                        }

                        compteurLigne++; // on incrémente notre compteur pour passer à la lettre suivante de l'alphabet 
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la lecture : " + e.Message);
            }
        }

        public string toString()
        {
            char[] alphabet = new char[]{'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 
            'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            string resultat = "La langue de notre dictionnaire est en francais.\nVoici le nombre de mots par lettre :\n"; // On veut afficher le nombre de lettre par mot des listes et la langue.
            for (int i = 0; i < mots.Length; i++) // La boucle va permettre de passer par chaque mot du tableau donc du dictionnaire
            {
                char lettre = alphabet[i]; // on initialise une varibale qui récupère les lettres de l'alphabet
                int nombreMots; // Cette variable va prendre le nombre de mot contenus par liste donc par lettre de l'alphabet
                if (mots[i] != null) nombreMots = mots[i].Count; // Si la liste n'est pas vide on compte le nombre d'élements 
                else nombreMots = 0; //sinon comme la liste est vide cette lettre de l'alphabet ne contient aucun mot dans le dictionnaire
                resultat += "Lettre " + lettre + " : " + nombreMots + " mots\n";/**On ajoute à
                l'affichage la lettre et le nombre de mots associés**/
            }
            return resultat; // On retourne l'affichage
        }

        public bool RechDichoRecursif(string mot) //Fonction de recherche dichotomique comme demandé.
        //On renvoie faux si le mot qu'a entré le joueur n'est pastrouvé dans le dictionnaire et vrai sinon
        {
            char[] alphabet = new char[]{'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 
            'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            int index=-1;//cette variabme nous permet de savoir dans quelle liste du tableau chercher le mot.
            if (string.IsNullOrEmpty(mot)) return false; // Si le mot que le joueur entre est null ou vide alors on renvoie faux
            string motMaj = mot.ToUpper();/**Le fichier dictionnaire a été donné avec tous les mots 
            en majuscules, donc au moment de vérifier, il faut mettre le mot entré par le joueur
            en majuscule pour qu'il puisse être trouvé dans le tableau de liste**/
            for (int i = 0; i< 26; i++)
            {
                if(motMaj[0]==alphabet[i]) {index =i;break;}
            }
            if (index < 0 || index > 25) return false; /**On vérifie que l'index de notre 
            première lettre du mot est bien comprise entre 0 et 25 soit les colonnes
             possibles de notre tableau de liste.**/

            List<string> maListe = mots[index];
            if (maListe == null || maListe.Count == 0) return false;
            return RechDichoInterne(maListe, motMaj, 0, maListe.Count - 1); /**on ne peut pas
            intialiser notre simple fonction avec une liste et un début et une fin au début
            ducoup on utilise une autre fonction qui va elle faire à 100% une recherche dichotomique**/
        }
 private bool RechDichoInterne(List<string> liste, string motCherche, int debut, int fin)
        {
            if (debut > fin) return false; //Si le début dépasse la fin, le mot n'est pas dans la liste
            int milieu = (debut + fin) / 2; // On utilise le principe de la recherche dichotomique en divisant la liste en deux à chaque appel récursif
            int comparaison = string.Compare(motCherche, liste[milieu]); /**string.Compare permet
            de renvoyer une valeur par rapport au positionnement alphabétique de deux chaînes 
            de caractères**/
            if (comparaison==0) return true; //le cas où les deux mots sont égaux
            else if (comparaison<0) return RechDichoInterne(liste, motCherche, debut, milieu - 1);//le cas où le mot cherché est avant le mot du milieu 
            else return RechDichoInterne(liste, motCherche, milieu + 1, fin); // le cas où le mot cherché est après le mot du milieu
        }

        private void TriParQuickSort(List<string> liste, int gauche, int droite)
{
    // Si la sous-liste a moins de 2 éléments, elle est déjà triée
    if (gauche >= droite)
    {
        return;
    }

    // Choix du pivot : ici, on prend le dernier élément de la sous-liste
    string pivot = liste[droite];
    int pivotIndex = gauche;

    // Partitionnement : on place les éléments plus petits que le pivot à gauche
    for (int i = gauche; i < droite; i++)
    {
        // Si l'élément courant est inférieur ou égal au pivot
        if (string.Compare(liste[i], pivot) <= 0)
        {
            // Échange les éléments pour les placer dans le bon ordre
            string temp = liste[pivotIndex];
            liste[pivotIndex] = liste[i];
            liste[i] = temp;
            pivotIndex++;
        }
    }

    // Place le pivot à sa position définitive
    string tempPivot = liste[pivotIndex];
    liste[pivotIndex] = liste[droite];
    liste[droite] = tempPivot;

    // Appels récursifs pour trier les sous-listes gauche et droite
    TriParQuickSort(liste, gauche, pivotIndex - 1);
    TriParQuickSort(liste, pivotIndex + 1, droite);
}
        private void TriInsertion(List<string> liste, int debut, int fin)
        {
            for (int i = debut + 1; i <= fin; i++)
            {
                string cle = liste[i];
                int j = i - 1;

                while (j >= debut && string.Compare(liste[j], cle) > 0)
                {
                    liste[j + 1] = liste[j];
                    j--;
                }
                liste[j + 1] = cle;
            }
        }
    }
}
