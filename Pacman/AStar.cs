using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    class astar
    {
        #region FIELDS

        private Vector2 starTile;
        private Vector2 endTile;
        private Node currentTile;
        private List<Node> openList = new List<Node>();
        private List<Node> closedList = new List<Node>();
        private Level_1 Level;
        private int width, height;
        private SpriteFont trace;
        private List<Vector2> listPath = new List<Vector2>();
        private int nodelength = 15;
        //private string file_name = "astar_traces.txt";
        private string GhostName;

        #endregion

        #region PROPERTIES

        public List<int> HashCodeDir { get; set; }
        public int Heuristic { get; set; }
        public bool IsPathFound { get; set; }
        public List<Vector2> ListPath { get; set; }

        #endregion


        /// <summary>
        /// Méthode GetAdjacentTiles
        /// </summary>
        /// <param name="currentTile"></param>
        /// <returns></returns>
        private List<Node> GetAdjacentTiles(Node currentTile)
        {
            List<Node> adjacentTiles = new List<Node>();
            Node adjacentTile;

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    {
                        //tile au-dessus
                        adjacentTile = new Node() { Position = new Vector2(currentTile.Position.X, currentTile.Position.Y - nodelength) };

                        if (adjacentTile.Position == Level.Maze2D[col, row].Position && Level.Maze2D[col, row].IsWall == false)
                        {
                            adjacentTiles.Add(adjacentTile);
                        }
                        //Tile à droite
                        adjacentTile = new Node() { Position = new Vector2(currentTile.Position.X + nodelength, currentTile.Position.Y) };

                        if (adjacentTile.Position == Level.Maze2D[col, row].Position && Level.Maze2D[col, row].IsWall == false)
                        {
                            adjacentTiles.Add(adjacentTile);
                        }
                        //Tile en dessous
                        adjacentTile = new Node() { Position = new Vector2(currentTile.Position.X, currentTile.Position.Y + nodelength) };

                        if (adjacentTile.Position == Level.Maze2D[col, row].Position && Level.Maze2D[col, row].IsWall == false)
                        {
                            adjacentTiles.Add(adjacentTile);
                        }
                        //Tile à gauche
                        adjacentTile = new Node() { Position = new Vector2(currentTile.Position.X - nodelength, currentTile.Position.Y) };

                        if (adjacentTile.Position == Level.Maze2D[col, row].Position && Level.Maze2D[col, row].IsWall == false)
                        {
                            adjacentTiles.Add(adjacentTile);
                        }
                    }
                }
            }

            return adjacentTiles;
        }



        /// <summary>
        /// Méthode GetLowerCostTile
        /// Retourne le noeud avec la plus petite valeur de F
        /// </summary>
        /// <param name="openList"></param>
        /// <returns></returns>
        private Node GetLowerCostTile(List<Node> openList)
        {
            Node lowerCost_F_Node = new Node();

            //Requête LINQ pour séléctionner les noeud dans une variable <IEnumerable>(T)
            var request = from node in openList
                          select node;

            //Séléction du noeud avec la plus petite valeur F
            var min = request.Min(f => f.F);

            //Recherche du noeud correspondant à la plus petite valeur F dans la openList
            foreach (Node node in openList)
            {
                if (node.F == min)
                {
                    lowerCost_F_Node = node;
                }
            }

            //Retourne le noeud avec cette plus petite valeur F
            return lowerCost_F_Node;
        }



        /// <summary>
        /// Méthode ManhanttanDistance
        /// Calcul de la distance heuristique entre le noeud courant et le noeud cible
        /// </summary>
        /// <param name="adjacenttile"></param>
        /// <returns></returns>
        private int ManhanttanDistance(Node adjacenttile)
        {
            int manhathan = (Math.Abs((int)(endTile.X - adjacenttile.Position.X)) + Math.Abs((int)(endTile.Y - adjacenttile.Position.Y))) * 10;

            return manhathan;
        }


        /// <summary>
        /// Méthode EuclidianDistance
        /// Calcul de la distance heuristique entre le noeud courant et le noeud cible
        /// </summary>
        /// <param name="adjacenttile"></param>
        /// <returns></returns>
        private int EuclidianDistance(Node adjacenttile)
        {
            double euclidian = Math.Sqrt(Math.Pow((adjacenttile.Position.X - endTile.X), 2) + Math.Pow((adjacenttile.Position.Y - endTile.Y), 2));

            return Convert.ToInt32(euclidian);
        }


        /// <summary>
        /// Méthode GoToCible
        /// Permet d'atteindre le Goal de la recherche
        /// </summary>
        /// <param name="Level"></param>
        /// <param name="HashDirSprite"></param>
        /// <returns></returns>
        public int GoToCible(Level_1 _Level, List<Vector2> listNextNodesSprite)
        {
            int HashDirSprite = 0;

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    Vector2 nodeNextUp = new Vector2(listNextNodesSprite[0].X, listNextNodesSprite[0].Y);
                    Vector2 nodeNextDown = new Vector2(listNextNodesSprite[2].X, listNextNodesSprite[2].Y);
                    Vector2 nodeNextLeft = new Vector2(listNextNodesSprite[3].X, listNextNodesSprite[3].Y);
                    Vector2 nodeNextRight = new Vector2(listNextNodesSprite[1].X, listNextNodesSprite[1].Y);

                    switch (GhostName)
                    {
                        case "Inky":

                            if (_Level.Maze2D[col, row].Position == nodeNextUp && _Level.Maze2D[col, row].IsPathInky)
                            {
                                HashDirSprite = 64;
                                _Level.Maze2D[col, row].IsPathInky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextRight && _Level.Maze2D[col, row].IsPathInky)
                            {
                                HashDirSprite = 128;
                                _Level.Maze2D[col, row].IsPathInky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextDown && _Level.Maze2D[col, row].IsPathInky)
                            {
                                HashDirSprite = 256;
                                _Level.Maze2D[col, row].IsPathInky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextLeft && _Level.Maze2D[col, row].IsPathInky)
                            {
                                HashDirSprite = 32;
                                _Level.Maze2D[col, row].IsPathInky = false;
                            }
                            break;


                        case "Pinky":

                            if (_Level.Maze2D[col, row].Position == nodeNextUp && _Level.Maze2D[col, row].IsPathPinky)
                            {
                                HashDirSprite = 64;
                                _Level.Maze2D[col, row].IsPathPinky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextRight && _Level.Maze2D[col, row].IsPathPinky)
                            {
                                HashDirSprite = 128;
                                _Level.Maze2D[col, row].IsPathPinky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextDown && _Level.Maze2D[col, row].IsPathPinky)
                            {
                                HashDirSprite = 256;
                                _Level.Maze2D[col, row].IsPathPinky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextLeft && _Level.Maze2D[col, row].IsPathPinky)
                            {
                                HashDirSprite = 32;
                                _Level.Maze2D[col, row].IsPathPinky = false;
                            }
                            break;


                        case "Blinky":

                            if (_Level.Maze2D[col, row].Position == nodeNextUp && _Level.Maze2D[col, row].IsPathBlinky)
                            {
                                HashDirSprite = 64;
                                _Level.Maze2D[col, row].IsPathBlinky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextRight && _Level.Maze2D[col, row].IsPathBlinky)
                            {
                                HashDirSprite = 128;
                                _Level.Maze2D[col, row].IsPathBlinky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextDown && _Level.Maze2D[col, row].IsPathBlinky)
                            {
                                HashDirSprite = 256;
                                _Level.Maze2D[col, row].IsPathBlinky = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextLeft && _Level.Maze2D[col, row].IsPathBlinky)
                            {
                                HashDirSprite = 32;
                                _Level.Maze2D[col, row].IsPathBlinky = false;
                            }
                            break;


                        case "Klyde":

                            if (_Level.Maze2D[col, row].Position == nodeNextUp && _Level.Maze2D[col, row].IsPathKlyde)
                            {
                                HashDirSprite = 64;
                                _Level.Maze2D[col, row].IsPathKlyde = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextRight && _Level.Maze2D[col, row].IsPathKlyde)
                            {
                                HashDirSprite = 128;
                                _Level.Maze2D[col, row].IsPathKlyde = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextDown && _Level.Maze2D[col, row].IsPathKlyde)
                            {
                                HashDirSprite = 256;
                                _Level.Maze2D[col, row].IsPathKlyde = false;
                            }

                            else if (_Level.Maze2D[col, row].Position == nodeNextLeft && _Level.Maze2D[col, row].IsPathKlyde)
                            {
                                HashDirSprite = 32;
                                _Level.Maze2D[col, row].IsPathKlyde = false;
                            }
                            break;
                    }
                }
            }
            return HashDirSprite;
        }


        /// <summary>
        /// Méthode ReconstructPath
        /// Permet de retracer le chemin à partir de la liste close
        /// </summary>
        /// <param name="closedlist"></param>
        /// <returns></returns>
        private List<Vector2> ReconstructPath(Node currentnode)
        {
            List<Vector2> listPath = new List<Vector2>();

            Node curNode = new Node();

            curNode = currentnode;
            listPath.Add(curNode.Position);

            while (curNode.ParentNode.X != 0 && curNode.ParentNode.Y != 0)
            {
                var queryChercheNode = from pNode in closedList
                                       select pNode.Position;

                foreach (var chercheNode in queryChercheNode)
                {
                    if (curNode.ParentNode == chercheNode)
                    {
                        listPath.Add(chercheNode);

                        foreach (var node in closedList)
                        {
                            if (node.Position == chercheNode)
                            {
                                curNode = node;
                            }
                        }
                    }
                }
            }

            return listPath;
        }


        /// <summary>
        /// Constructeur astar
        /// </summary>
        /// <param name="Level"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public astar(Level_1 Level, int width, int height, string GhostName)
        {
            this.Level = Level;
            this.width = width;
            this.height = height;
            IsPathFound = false;
            this.GhostName = GhostName;

            openList = new List<Node>();
            closedList = new List<Node>();
            ListPath = new List<Vector2>();
        }


        /// <summary>
        /// Méthode SearchCible
        /// Recherche du chemin de la source vers la cible
        /// suivant l'implémentation de l'algorithme A*
        /// </summary>
        /// <param name="startTile"></param>
        /// <param name="endTile"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SearchCible(Vector2 startTile, Vector2 endTile)
        {
            this.starTile = startTile;
            this.endTile = endTile;
            currentTile = new Node();

            //Vidage des listes
            openList.Clear();
            closedList.Clear();
            ListPath.Clear();

            //Calcul des valeurs G,H,F et du noeud parent du noeud de départ (0,0)          
            currentTile.Position = new Vector2(starTile.X, starTile.Y);
            currentTile.ParentNode = Vector2.Zero;
            currentTile.G = 0;
            currentTile.H = ManhanttanDistance(currentTile);
            currentTile.F = currentTile.G + currentTile.H;
            Heuristic = currentTile.H;

            //Ajout du noeud courant dans la liste close
            closedList.Add(currentTile);

            //Début
            do
            {
                //Calcul des noeuds adjacents au noeud courant et stockage dans la liste des noeuds adjacents                
                List<Node> adjacentTiles = GetAdjacentTiles(currentTile);

                //Calcul pour tous les noeuds adjacents compris dans la liste adjacentTiles
                foreach (var adj in adjacentTiles)
                {
                    //Determine de la même manière si le noeud adjacent est déjà dans la liste close
                    var closedListContainAdj = closedList.Any(p => p.Position == adj.Position);

                    //Détermine si le noeud adjacent est déjà dans la liste ouverte
                    var openListContainAdj = openList.Any(p => p.Position == adj.Position);

                    //Si le noeud voisin est dans la liste close, on l'ignore et on passe au suivant
                    if (closedListContainAdj)
                    {
                        continue;
                    }

                    //Sinon, si la liste ouverte contient le noeud adjacent
                    else if (openListContainAdj)
                    {
                        for (int i = 0; i < openList.Count; i++)
                        {
                            if (openList[i].Position == adj.Position)
                            {
                                //On calcul sa valeur G
                                adj.G = currentTile.G + 10;

                                //Si la valeur G du noeud voisin est plus petite que celle du noeud de la liste ouverte
                                if (openList[i].G > adj.G)
                                {
                                    //On calcul sa valeur H
                                    adj.H = ManhanttanDistance(adj);
                                    //On calcul sa valeur F
                                    adj.F = adj.G + adj.H;
                                    //On rajoute son noeud parent
                                    adj.ParentNode = currentTile.Position;

                                    //On remplace le noeud de la liste ouverte par le noeud voisin
                                    openList.Remove(openList[i]);
                                    openList.Add(adj);
                                }
                            }
                        }
                    }

                    //Sinon (il s'agit d'un nouveau noeud)
                    else
                    {
                        //On calcul sa valeur G
                        adj.G = currentTile.G + 10;
                        //On calcul sa valeur H
                        adj.H = ManhanttanDistance(adj);
                        //On calcul sa valeur F
                        adj.F = adj.G + adj.H;
                        //On rajoute son noeud parent
                        adj.ParentNode = currentTile.Position;
                        //On rajoute le noeud dans la liste ouverte
                        openList.Add(adj);
                    }


                }//Fin de foreach ici

                if (openList.Count == 0)
                {
                    //Si la liste ouverte est vide, on sort de la boucle
                    break;
                }

                //Si la liste ouverte n'est pas vide, on prend le noeud qui a la valeur F la plus petite, et on continue
                currentTile = GetLowerCostTile(openList);

                openList.Remove(currentTile);
                closedList.Add(currentTile);

            } while (currentTile.Position != endTile); //Tant que le noeud courant n'est pas à l'emplacement du noeud cible

            //test si la position est trouvée, si oui : booléen à true, sinon, à false
            IsPathFound = (currentTile.Position == endTile) ? true : false;

            closedList.Reverse();

            listPath = ReconstructPath(closedList[0]);
            listPath.Reverse();

            //Exporte la liste du chemin trouvé
            ListPath = listPath;

            //Initialise un array2D temporaire qui comportera le chemin avec le booléen IsWalkable pour le sprite correspondant
            Node[,] Tableau2D = Level.Maze2D;

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    for (int i = 1; i < listPath.Count; i++)
                    {
                        if (ListPath[i] == Tableau2D[col, row].Position)
                        {
                            switch (GhostName)
                            {
                                case "Inky": Tableau2D[col, row].IsPathInky = true; break;
                                case "Pinky": Tableau2D[col, row].IsPathPinky = true; break;
                                case "Blinky": Tableau2D[col, row].IsPathBlinky = true; break;
                                case "Klyde": Tableau2D[col, row].IsPathKlyde = true; break;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Méthode DrawPath
        /// Permet de visualiser le pathfinder
        /// </summary>
        /// <param name="spritebatch"></param>
        public void DrawPath(SpriteBatch spritebatch, Texture2D way_tile, Color color)
        {
            List<Vector2> thisPath = new List<Vector2>();
        }
    }
}