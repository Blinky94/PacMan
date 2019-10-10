using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    /// <summary>
    /// Classe Randomized
    /// </summary>
    class Randomized
    {
        #region FIELD

        //Constantes multiplicatives pour déterminer la direction (HashCode)
        private int up = 64, right = 128, down = 256, left = 32;
        //Indice de parcours d'une liste
        private int indice;
        //entier qui determine la direction
        private int _direction;
        //entier qui determine la longueur du chemin
        private int _length;
        //Liste utilisée pour la longeur du chemin
        private int[] myListRandom;
        //Le code HashcodeDir généré
        private int hashCodeGenerated = 0;
        //Liste temporaire
        private int[] list_tmp;

        //Variables de test
        protected SpriteFont trace;

        protected bool[] list_test_succ_Is_Wall = new bool[0];

        #endregion

        /// <summary>
        /// Méthode de génération aléatoire de nombres
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();

            return random.Next(min, max);
        }

        /// <summary>
        /// Méthode private GenListRandom
        /// Retourne un nom de Liste suivant le nombre entier tiré au sort
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int[] GenListRandom(int num)
        {
            int[] selected_list = new int[0];

            //Intialisation de la longueur du tableau, fonction du tirage au sort
            switch (num)
            {
                case 1: selected_list = new int[30];
                    ; break;
                case 2: selected_list = new int[60];
                    ; break;
                case 3: selected_list = new int[120];
                    ; break;
                case 4: selected_list = new int[240];
                    ; break;
                case 5: selected_list = new int[5];
                    break;
                default: selected_list = new int[10];
                    break;
            }

            return selected_list;
        }

        /// <summary>
        /// Méthode DirHashCodeSprite
        /// Transforme la liste avec la bonne suite de valeurs
        /// en fonction de l'entier "num" aléatoire qui détermine la direction
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int[] DirHashCodeSprite(int[] list, int num)
        {
            //Fonction du code "num" qui determine la direction, affecte un multiple qui génère le code Hash
            switch (num)
            {
                //HAUT
                case 0:
                    for (int i = 0; i <= list.Length - 1; i++)
                    {
                        list[i] = up;
                    }; break;

                //DROITE
                case 1:
                    for (int i = 0; i <= list.Length - 1; i++)
                    {
                        list[i] = right;
                    }; break;

                //BAS
                case 2:
                    for (int i = 0; i <= list.Length - 1; i++)
                    {
                        list[i] = down;
                    }; break;

                //GAUCHE
                default:
                    for (int i = 0; i <= list.Length - 1; i++)
                    {
                        list[i] = left;
                    }; break;
            }
            return list;
        }


        /// <summary>
        /// Constructor
        /// Prend en compte la liste des successeurs de la case occupée par le Sprite et délimite le choix en fonction des obstacles
        /// </summary>
        public Randomized()
        {
            ///Partie qui s'occupe de la longueur de la liste générée

            //Initialise au départ la longeur à parcourir pour le Sprite
            _length = RandomNumber(1, 6);
            myListRandom = new int[0];
            //Fonction de la longueur aléatoire séléctionnée
            myListRandom = GenListRandom(_length);

            ///Partie qui s'occupe de la direction que prend le sprite et qui change la valeur dans la liste de chaque élémen

            //intialise au départ la direction aléatoire
            _direction = RandomNumber(0, 3);
            //Crée une liste temporaire
            list_tmp = new int[0];
            //Méthode qui génère la liste finale en fonction de la direction choisie
            myListRandom = GenListRandom(_length);
            //Enfin initialise l'indice de parcours de la liste au début de la liste
            indice = 0;
            //Renvoi le premier HashCode pour la direction initiale du Sprite
            hashCodeGenerated = myListRandom[indice];
        }

        /// <summary>
        /// LoadContent
        /// Méthode de test pour tracer
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager Content)
        {
            trace = Content.Load<SpriteFont>("fullscreen_font");
        }

        /// <summary>
        /// GenHashCodeFromListIsWall
        /// Fait corespondre le hashcode à la position du index dans la liste<bool> listNextNodesIsWall
        /// Pour valider s'il s'agit d'un mur et si on doit arrêter le parcours de la liste
        /// </summary>
        /// <returns></returns>
        private int GenHashCodeFromListIsWall()
        {
            int indice = 0;

            if (hashCodeGenerated == 64) { indice = 0; }
            if (hashCodeGenerated == 128) { indice = 1; }
            if (hashCodeGenerated == 256) { indice = 2; }
            if (hashCodeGenerated == 32) { indice = 3; }

            return indice;
        }

        /// <summary>
        /// Méthode HashRand
        /// Génère un nombre entier aléatoire
        /// qui permet le déplacement d'un sprite dans le labyrinthe
        /// dans la continuité du construteur "Randomized()"
        /// Prend en compte la liste des successeurs de la case occupée par le Sprite et délimite le choix en fonction des obstacles
        /// </summary>
        /// <returns></returns>
        public int HashRand(List<bool> listNextNodesIsWall)
        {
            int index = GenHashCodeFromListIsWall();

            if ((indice < myListRandom.Length) && listNextNodesIsWall[index] == false)
            {
                //On attribue le contenu de la liste à la position de l'indice
                hashCodeGenerated = myListRandom[indice];

                //incrémentation de l'indice ensuite
                indice++;
            }

            ///Si la liste est parcouru complétement ou qu'on est devant un mur, on régénère une liste vide                          
            else
            {
                ///Partie qui s'occupe de la longueur de la liste générée

                //Initialise au départ la longeur à parcourir pour le Sprite
                _length = RandomNumber(1, 6);
                myListRandom = new int[0];
                //Fonction de la longueur aléatoire séléctionnée
                myListRandom = GenListRandom(_length);

                ///Partie qui s'occupe de la direction que prend le sprite et qui change la valeur dans la liste de chaque élément
                ///Prend en compte la liste des successeurs suivants qu'ils ne soient pas des obstacles

                //Création d'une liste locale des "num_de_direction" à trier
                int[] listWayToFlollow = new int[4];

                for (int j = 0; j <= listNextNodesIsWall.Count - 1; j++)
                {
                    if (listNextNodesIsWall[j] == false)
                    {
                        listWayToFlollow[j] = j;
                    }
                }

                //Stock le nombre d'éléments restants 
                int countList = listWayToFlollow.Count();

                //intialise au départ la direction aléatoire
                _direction = RandomNumber(0, countList);

                //Crée une liste temporaire
                list_tmp = new int[0];
                list_tmp = myListRandom;
                //Méthode qui génère la liste finale en fonction de la direction choisie
                myListRandom = DirHashCodeSprite(list_tmp, listWayToFlollow[_direction]);
                //Enfin initialise l'indice de parcours de la liste au début de la liste
                indice = 0;
                //Renvoi le premier HashCode pour la direction initiale du Sprite
                hashCodeGenerated = myListRandom[indice];
            }

            //Retourne le code hashCodeGenerated final généré                
            return hashCodeGenerated;
        }


        /// <summary>
        /// Méthode de test Draw pour tracer le test
        /// </summary>
        /// <param name="spriteBatch"></param>    
        public void Draw(SpriteBatch spriteBatch)
        {
            //string text1 = string.Format("hashCodeGenerated = {0}", hashCodeGenerated);        
            //spriteBatch.DrawString(this.trace, text1, new Vector2(0, 10), Color.Red);

            //string text2 = string.Format("indice = {0}", indice);
            //spriteBatch.DrawString(this.trace, text2, new Vector2(0, 20), Color.Yellow);

            //string text3 = string.Format("BAS = {0}", list_test_succ_Is_Wall[2]);
            //spriteBatch.DrawString(this.trace, text3, new Vector2(200, 10), Color.Blue);

            //string text4 = string.Format("GAUCHE = {0}", list_test_succ_Is_Wall[3]);
            //spriteBatch.DrawString(this.trace, text4, new Vector2(200, 20), Color.Green);    
        }
    }
}