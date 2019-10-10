using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.Linq;

namespace PacMan
{
    class Level_1
    {
        #region FIELDS

        private int _width_scr, _height_scr;
        private string _level_xml;
        private List<char> list_char;
        private XmlDocument doc;
        private Node[,] level_table2D;
        private const int nodeLength = 15;
        private int width, height;
        private SpriteFont trace;
        private int width_maze;
        private int r = 0, v = 0, b = 0;

        //Variables pour les coordonnées des couloirs gauche et droit
        int? _ind_col_I = 0, _ind_row_I = 0;
        int? _ind_col_Q = 0, _ind_row_Q = 0;
        int? _ind_col_L = 0, _ind_row_L = 0;
        int? _ind_col_W = 0, _ind_row_W = 0;

        private Color _Color_Wall;

        private bool IsFullScreen = false;
        private Rectangle mazeRec;

        #endregion

        #region PROPRETIES

        //Rectangle de dimension du labyrinthe
        public Rectangle MazeRec { get { return mazeRec; } set { mazeRec = value; } }
        public string FruitLevel { get; set; }

        public int IndexXPosHG { get; set; } public int IndexYPosHG { get; set; }
        public int IndexXPosHD { get; set; } public int IndexYPosHD { get; set; }
        public int IndexXPosBG { get; set; } public int IndexYPosBG { get; set; }
        public int IndexXPosBD { get; set; } public int IndexYPosBD { get; set; }

        //valeurs booléennes des cases voisines de la case occupée par PACMAN
        public List<bool> ListIfNextNodesIsWallPacMan { get; set; }
        //coordonnées centrales de chaque voisin de la case courante de PACMAN
        public List<Vector2> ListNextNodesOfPacMan { get; set; }
        //Vecteur d'export de la direction de PACMAN
        public Vector2 PacPos { get; set; }
        //Rectangle de collision
        public Rectangle PAC_REC { get; set; }
        public Vector2 PacManHome { get; set; }

        //valeurs booléennes des cases voisines de la case occupée par INKY
        public List<bool> listNextNodesIsWallInky { get; set; }
        //coordonnées centrales de chaque voisin de la case courante de INKY
        public List<Vector2> listNextNodesInky { get; set; }
        //Vecteur d'export de la direction de INKY
        public Vector2 InkyPos { get; set; }
        //Rectangle de collision
        public Rectangle INKY_REC { get; set; }
        public Rectangle InkyRecDectect { get; set; }
        public Vector2 InkyHome { get; set; }

        //valeurs booléennes des cases voisines de la case occupée par BLINKY
        public List<bool> listNextNodesIsWallBlinky { get; set; }
        //coordonnées centrales de chaque voisin de la case courante de BLINKY
        public List<Vector2> listNextNodesBlinky { get; set; }
        //Vecteur d'export de la direction de BLINKY
        public Vector2 BlinkyPos { get; set; }
        //Rectangle de collision
        public Rectangle BLINKY_REC { get; set; }
        public Rectangle BlinkyRecDectect { get; set; }
        public Vector2 BlinkyHome { get; set; }

        //valeurs booléennes des cases voisines de la case occupée par PINKY
        public List<bool> listNextNodesIsWallPinky { get; set; }
        //coordonnées centrales de chaque voisin de la case courante de PINKY
        public List<Vector2> listNextNodesPinky { get; set; }
        //Vecteur d'export de la direction de PINKY
        public Vector2 PinkyPos { get; set; }
        //Rectangle de collision
        public Rectangle PINKY_REC { get; set; }
        public Rectangle PinkyRecDectect { get; set; }
        public Vector2 PinkyHome { get; set; }

        //valeurs booléennes des cases voisines de la case occupée par KLYDE
        public List<bool> listNextNodesIsWallKlyde { get; set; }
        //coordonnées centrales de chaque voisin de la case courante de KLYDE
        public List<Vector2> listNextNodesKlyde { get; set; }
        //Vecteur d'export de la direction de KLYDE
        public Vector2 KlydePos { get; set; }
        //Rectangle de collision
        public Rectangle KLYDE_REC { get; set; }
        public Rectangle KlydeRecDectect { get; set; }
        public Vector2 KlydeHome { get; set; }

        //valeurs booléennes des cases voisines de la case occupée par le FRUIT
        public List<bool> ListNextNodesIsWallBonus { get; set; }
        //coordonnées centrales de chaque voisin de la case courante du FRUIT
        public List<Vector2> ListNextNodesBonus { get; set; }
        //Vecteur d'export de la direction de KLYDE
        public Vector2 bonusPos { get; set; }
        //Rectangle de collision
        public Rectangle BONUS_REC { get; set; }

        //Exporte les Vecteurs de position des Tiles des couloirs raccourcis de gauche
        public Vector2 LeftCoridor
        { get { return level_table2D[(int)_ind_col_I + 1, (int)_ind_row_I].Position; } set { level_table2D[(int)_ind_col_I + 1, (int)_ind_row_I].Position = value; } }

        //Exporte les Vecteurs de position des Tiles des couloirs raccourcis de droite
        public Vector2 RightCoridor
        { get { return level_table2D[(int)_ind_col_Q - 1, (int)_ind_row_Q].Position; } set { level_table2D[(int)_ind_col_Q - 1, (int)_ind_row_Q].Position = value; } }

        //Exporte les Vecteurs de position des Tiles des couloirs raccourcis de haut
        public Vector2 UpCoridor
        { get { return level_table2D[(int)_ind_col_L, (int)_ind_row_L + 1].Position; } set { level_table2D[(int)_ind_col_L, (int)_ind_row_L + 1].Position = value; } }

        //Exporte les Vecteurs de position des Tiles des couloirs raccourcis de bas
        public Vector2 DownCoridor
        { get { return level_table2D[(int)_ind_col_W, (int)_ind_row_W - 1].Position; } set { level_table2D[(int)_ind_col_W, (int)_ind_row_W - 1].Position = value; } }

        //Export des dimensions du labyrinthe
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }

        //Exporte le level_2D
        public Node[,] Maze2D { get; set; }

        #endregion

        //Variables pour le positionnement visuel du labyrinthe (scaling)
        public Matrix Transform { get; set; }
        private Viewport _viewport;
        private float _zoom = 0.87f;

        #region CONSTRUCTORS

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="widthScr"></param>
        /// <param name="heightScr"></param>
        public Level_1(int width_scr, int height_scr)
        {
            this._width_scr = width_scr;
            this._height_scr = height_scr;
        }

        /// <summary>
        /// Méthode Initialize
        /// </summary>
        public virtual void Initialize(string level_xml)
        {
            //Créé l'objet List<char>
            list_char = new List<char> { };
            list_char.Clear();

            //Génère le Level2D à partir des informations récupérées dans le fichier XML
            list_char = XML_Maze_To_Table2D(level_xml);

            //Défini la longueur du labyrinthe
            width_maze = width * nodeLength;

            //initialise le tableau 2D de charactères à vide
            level_table2D = new Node[width, height];
        }

        #endregion

        /// <summary>
        /// Méthode XML_Maze_To_Table2D
        /// Récupère les informations nécessaires pour générer le labyrinthe sous forme de tableau2D
        /// A partir du fichier XML
        /// </summary>
        private List<char> XML_Maze_To_Table2D(string level_xml)
        {
            #region PARTIE QUI RECUPERE LES ELEMENTS DU LABYRINTHE

            _level_xml = level_xml;

            List<char> _list_char = new List<char> { };
            _list_char.Clear();
            string table_str = "";

            //Créé l'objet XmlDocument
            doc = new XmlDocument();

            //Charge le fichier maze_fic
            doc.Load(_level_xml);

            //Parcours le fichier XML et récupère la valeur texte des noeuds pour stocker dans une chaine de caractères
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                table_str += node.InnerText;
            }

            //Ajoute caractère par caractère dans une liste de caractères
            foreach (char car in table_str)
            {
                _list_char.Add(car);
            }

            #endregion

            #region PARTIE QUI RECUPERE LES DIMENSIONS DU LABYRINTHE

            //Récupère le tag nommé "Width" et "Height" du fichier Xml
            XmlNodeList width_node = doc.GetElementsByTagName("Width");
            XmlNodeList height_node = doc.GetElementsByTagName("Height");

            foreach (XmlNode node in width_node)
            {
                width = Convert.ToInt32(node.InnerText);
            }

            foreach (XmlNode node in height_node)
            {
                height = Convert.ToInt32(node.InnerText);
            }

            #endregion

            #region PARTIE QUI RECUPERE LA COULEUR DU LABYRINTHE

            //Récupère le tag nommé "Rouge" "Vert" "Bleu" du fichier Xml
            XmlNodeList color_red_node = doc.GetElementsByTagName("Rouge");
            XmlNodeList color_green_node = doc.GetElementsByTagName("Vert");
            XmlNodeList color_blue_node = doc.GetElementsByTagName("Bleu");

            foreach (XmlNode node in color_red_node)
            {
                r = Convert.ToInt32(node.InnerText);
            }

            foreach (XmlNode node in color_green_node)
            {
                v = Convert.ToInt32(node.InnerText);
            }

            foreach (XmlNode node in color_blue_node)
            {
                b = Convert.ToInt32(node.InnerText);
            }

            _Color_Wall = new Color(r, v, b);

            #endregion

            #region PARTIE QUI RECUPERE LE FRUIT DU LEVEL

            XmlNodeList fruit_node = doc.GetElementsByTagName("Fruit");

            foreach (XmlNode node in fruit_node)
            {
                FruitLevel = node.InnerText;
            }


            #endregion

            return _list_char;
        }

        #region LOADCONTENT

        /// <summary>
        /// Méthode LoadContent
        /// Qui charge à partir du fichier XML les éléments du labyrinthe
        /// </summary>
        /// <param name="content"></param>
        /// <param name="level_XML"></param>
        /// <param name="_IsFullScreen"></param>
        /// <param name="Pellet_tile"></param>
        /// <param name="Sup_Pellet_tile"></param>
        /// <param name="graphics"></param>
        /// <param name="fruit_texture"></param>
        public virtual void LoadContent(ContentManager content,
            string ABD, string ABG, string AHG, string ARV, string LH, string LV, string way_tile, string ADBD, string ADBG, string ADHG, string ADRV,
            string LHB, string LHH, string LVD, string LVG, string Door, string ADLG, string ALDB, string ALBG, string ALRV, string CC, string DD, string EE,
            string FF, string GG, string HH, bool _IsFullScreen, string Pellet_tile, string Sup_Pellet_tile, GraphicsDeviceManager graphics)
        {

            //Recupération du booléen plein écran
            IsFullScreen = _IsFullScreen;

            //Charge le viewport pour le scaling du jeu
            _viewport = graphics.GraphicsDevice.Viewport;

            #region XML PARSE AND LEVEL GENERATED

            trace = content.Load<SpriteFont>("fullscreen_font");

            #endregion

            #region ATTRIBUTE OF TILES

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    //Instancie un objet collection dans chaque champs du tableau 2D
                    level_table2D[col, row] = new Node();

                    level_table2D[col, row].ParentNode = new Vector2(int.MaxValue, int.MaxValue);
                    level_table2D[col, row].F = int.MaxValue;
                    level_table2D[col, row].G = int.MaxValue;
                    level_table2D[col, row].H = int.MaxValue;


                    //Rempli le tableau2D par les caractères de la listes générée
                    level_table2D[col, row].IsChar = list_char[row * width + col];

                    //Défini le booléen IsWall pour chaque caractère                   
                    switch (level_table2D[col, row].IsChar)
                    {
                        //'M' Is Horizontal side wall
                        case 'M':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(LH);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        // 'N' Is Vertical side Wall
                        case 'N':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(LV);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        // 'E' Up,Left side Wall
                        case 'E':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ABD);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        // 'D' Is Up,Right side Wall
                        case 'D':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ABG);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        // 'H' Is Down, Left side Wall
                        case 'H':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ARV);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        // 'F' Is Down, Right side Wall
                        case 'F':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(AHG);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //'C' Left to Right
                        case 'p':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        case '1':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //'a' Est l'emplacement de départ de PACMAN
                        case 'a':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Contour doublé haut/gauche
                        case 'V':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ADBD);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé haut/droit
                        case 'O':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ADBG);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé bas/gauche
                        case 'S':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ADRV);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé bas/gauche
                        case 'R':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ADHG);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé ligne gauche verticale
                        case 'B':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(LVG);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé ligne droite verticale
                        case 'Y':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(LVD);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé ligne Horizontale haute
                        case 'G':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(LHH);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé ligne Horizontale haute
                        case 'A':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(LHB);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé ligne Horizontale haute
                        case 'U':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(Door);
                            level_table2D[col, row].ColoR = Color.Gainsboro;
                            break;
                        //Contour doublé bas gauche
                        case 'b':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ALRV);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé bas droit
                        case 'c':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ADLG);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé haut droit
                        case 'd':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ALBG);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour doublé haut gauche
                        case 'e':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(ALDB);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour arondit haut droit et mur horizontal
                        case 'j':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(CC);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour arondit haut gauche et mur horizontal
                        case 'k':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(DD);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour arondit bas droit et mur vertical
                        case 'l':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(EE);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour arondit haut droit et mur vertical
                        case 'm':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(FF);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour arondit bas gauche et mur vertical
                        case 'n':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(GG);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Contour arondit haut gauche et mur vertical
                        case 'o':
                            level_table2D[col, row].IsWall = true;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(HH);
                            level_table2D[col, row].ColoR = _Color_Wall;
                            break;
                        //Tile de raccourci aux extremités gauches 
                        case 'I':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Tile de raccourci aux extremités droites
                        case 'Q':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Tile de raccourci aux extremités haut 
                        case 'L':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Tile de raccourci aux extremités bas
                        case 'W':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Tile pour les PacGommes
                        case 'C':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(Pellet_tile);
                            level_table2D[col, row].ColoR = Color.PaleGoldenrod;
                            break;
                        //'s' Est l'emplacement de départ de INKY
                        case 's':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //'u' Est l'emplacement de départ de BLINKY
                        case 'u':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //'K' Est l'emplacement de départ de PINKY
                        case 'K':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //'h' Est l'emplacement de départ de KLYDE
                        case 'h':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(way_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Tile pour la superGoms HG
                        case 'P':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(Sup_Pellet_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Tile pour la superGoms HD
                        case 'X':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(Sup_Pellet_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Tile pour la superGoms BG
                        case 'x':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(Sup_Pellet_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                        //Tile pour la superGoms BD
                        case 'Z':
                            level_table2D[col, row].IsWall = false;
                            level_table2D[col, row].Texture = content.Load<Texture2D>(Sup_Pellet_tile);
                            level_table2D[col, row].ColoR = Color.Black;
                            break;
                    }
                }
            }

            //Defini la position du labyrinthe en fonction du choix du joueur pour les options d'affichage à l'écran (full screen, windowed)
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (IsFullScreen)
                    {
                        //Defini la position initiale du labyrinthe au centre de l'écran de jeu
                        level_table2D[col, row].Position = new Vector2((_width_scr / 2 - (width_maze / 2)) + (col * nodeLength), (row * nodeLength));
                        mazeRec = new Rectangle((int)level_table2D[0, 0].Position.X, (int)level_table2D[0, 0].Position.Y, col * nodeLength, row * nodeLength);
                    }
                    else
                    {
                        //Defini la position initiale du labyrinthe dans la fenêtre (mode windowed)
                        level_table2D[col, row].Position = new Vector2((_width_scr / 2 - (width_maze / 1.5f)) + (col * nodeLength), row * nodeLength + 50);
                        mazeRec = new Rectangle((int)level_table2D[0, 0].Position.X, (int)level_table2D[0, 0].Position.Y, col * nodeLength, row * nodeLength);
                    }
                }
            }

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    //Positionnement et scaling des éléments du jeu           
                    Transform = Matrix.CreateTranslation(new Vector3(-level_table2D[col, row].Position.X + 200, -level_table2D[col, row].Position.Y + 220, 0)) *
                        Matrix.CreateRotationZ(0) *
                        Matrix.CreateScale(new Vector3(_zoom, _zoom, 0)) *
                        Matrix.CreateTranslation(new Vector3(_viewport.Width / 2, _viewport.Height / 2, 0));
                }
            }

            //Génère les tiles du labyrinthe 2D
            foreach (Node obj in level_table2D)
            {
                obj.Tile_Rec = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, nodeLength, nodeLength);
            }

            #endregion

            //Exporte les coordonnées initiales du Tile ou doit se situer PACMAN (centre du "tile" - longueur du rectangle de PacMan / 2)
            foreach (Node obj in level_table2D)
            {
                if (obj.IsChar == 's')
                {
                    InkyPos = new Vector2(obj.Position.X, obj.Position.Y);
                    InkyHome = InkyPos;
                }

                if (obj.IsChar == 'u')
                {
                    BlinkyPos = new Vector2(obj.Position.X, obj.Position.Y);
                    BlinkyHome = BlinkyPos;
                }

                if (obj.IsChar == 'K')
                {
                    PinkyPos = new Vector2(obj.Position.X, obj.Position.Y);
                    PinkyHome = PinkyPos;
                }

                if (obj.IsChar == 'h')
                {
                    KlydePos = new Vector2(obj.Position.X, obj.Position.Y);
                    KlydeHome = KlydePos;
                }

                //Si le "tile" est marqué de la lettre 'a', PacMan est placé en son centre
                if (obj.IsChar == 'a')
                {
                    PacPos = new Vector2(obj.Position.X, obj.Position.Y);
                    PacManHome = PacPos;
                }
            }

            bool _is_find = false;
            int _ind_col_fruit = 0;
            int _ind_row_fruit = 0;

            while (_is_find == false)
            {
                Random rnd = new Random();
                _ind_col_fruit = rnd.Next(0, width);
                _ind_row_fruit = rnd.Next(0, height);

                if (level_table2D[_ind_col_fruit, _ind_row_fruit].IsChar == 'C')
                {
                    level_table2D[_ind_col_fruit, _ind_row_fruit].IsChar = 'f';
                    level_table2D[_ind_col_fruit, _ind_row_fruit].IsWall = false;
                    level_table2D[_ind_col_fruit, _ind_row_fruit].Texture = content.Load<Texture2D>(FruitLevel);
                    level_table2D[_ind_col_fruit, _ind_row_fruit].ColoR = Color.Black;
                    _is_find = true;
                }
            }

            //Exporte les coordonnées initiales du Tile ou doit se situer PACMAN (centre du "tile" - longueur du rectangle de PacMan / 2)
            foreach (Node tile in level_table2D)
            {
                if (tile.IsChar == 'f')
                {
                    bonusPos = new Vector2(tile.Position.X, tile.Position.Y);
                }
            }


            //Récupère les coordonnées des couloirs gauche et droit

            bool I_exist = false, Q_exist = false, L_exist = false, W_exist = false;

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    //Lettre couloir horizontal gauche (sortie/entrée)
                    if (level_table2D[col, row].IsChar == 'I')
                    {
                        _ind_col_I = col;
                        _ind_row_I = row;
                        I_exist = true;
                    }

                    //Lettre couloir horizontal droite (entrée/sortie)
                    if (level_table2D[col, row].IsChar == 'Q')
                    {
                        _ind_col_Q = col;
                        _ind_row_Q = row;
                        Q_exist = true;
                    }

                    //lettre couloir vertical haut (entrée/sortie)
                    if (level_table2D[col, row].IsChar == 'L')
                    {
                        _ind_col_L = col;
                        _ind_row_L = row;
                        L_exist = true;
                    }

                    //lettre couloir vertical bas (sortie/entrée)
                    if (level_table2D[col, row].IsChar == 'W')
                    {
                        _ind_col_W = col;
                        _ind_row_W = row;
                        W_exist = true;
                    }
                }
            }

            //Defini le booléen en fonction de la présence des lettres 'I','Q','L','W' (couloir gauche,droite,haut,bas)
            if (I_exist == false) { _ind_col_I = 1; _ind_row_I = 1; }
            if (Q_exist == false) { _ind_col_Q = 1; _ind_row_Q = 1; }
            if (L_exist == false) { _ind_col_L = 1; _ind_row_L = 1; }
            if (W_exist == false) { _ind_col_W = 1; _ind_row_W = 1; }

            //Etabli la liste des coordonnées des successeurs possibles pour chaque "tile" (haut,droit,bas,gauche)                      
            for (int col = 1; col <= width - 2; col++)
            {
                for (int row = 1; row <= height - 2; row++)
                {
                    //Dans l'intervalle qui ne dépasse pas les limites du tableau 2D                                        
                    if (col >= 1 || col < width - 1 || row >= 1 || row < height - 1)
                    {
                        if (level_table2D[col, row].Position != level_table2D[(int)_ind_col_I, (int)_ind_row_I].Position
                            || level_table2D[col, row].Position != level_table2D[(int)_ind_col_Q, (int)_ind_row_Q].Position
                            || level_table2D[col, row].Position != level_table2D[(int)_ind_col_L, (int)_ind_row_L].Position
                            || level_table2D[col, row].Position != level_table2D[(int)_ind_col_W, (int)_ind_row_W].Position
                            )
                        {
                            //Initialise la liste des successeurs (haut,droite,bas,gauche)                      
                            level_table2D[col, row].List_Successor = new List<Vector2>();

                            //Récupère les coordonnées du "tile" du haut                         
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col, row - 1].Position.X, level_table2D[col, row - 1].Position.Y));

                            //Récupère les coordonnées du "tile" de droite                          
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col + 1, row].Position.X, level_table2D[col + 1, row].Position.Y));

                            //Récupère les coordonnées du "tile" du bas                           
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col, row + 1].Position.X, level_table2D[col, row + 1].Position.Y));

                            //Récupère les coordonnées du "tile" de gauche                            
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col - 1, row].Position.X, level_table2D[col - 1, row].Position.Y));
                        }

                        else if (level_table2D[col, row].Position == level_table2D[(int)_ind_col_I, (int)_ind_row_I].Position)
                        {
                            //Initialise la liste des successeurs (haut,droite,bas,gauche)                      
                            level_table2D[col, row].List_Successor = new List<Vector2>();

                            //Récupère les coordonnées du "tile" du haut                         
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col, row - 1].Position.X, level_table2D[col, row - 1].Position.Y));

                            //Récupère les coordonnées du "tile" de droite                          
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col + 1, row].Position.X, level_table2D[col + 1, row].Position.Y));

                            //Récupère les coordonnées du "tile" du bas                           
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col, row + 1].Position.X, level_table2D[col, row + 1].Position.Y));

                            //Récupère les coordonnées du "tile" qui se trouve à l'entrée du couloir opposé                            
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[(int)_ind_col_Q, (int)_ind_row_Q].Position.X, level_table2D[(int)_ind_col_Q, (int)_ind_row_Q].Position.Y));
                        }

                        else if (level_table2D[col, row].Position == level_table2D[(int)_ind_col_Q, (int)_ind_row_Q].Position)
                        {
                            //Initialise la liste des successeurs (haut,droite,bas,gauche)                      
                            level_table2D[col, row].List_Successor = new List<Vector2>();

                            //Récupère les coordonnées du "tile" du haut                         
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col, row - 1].Position.X, level_table2D[col, row - 1].Position.Y));

                            //Récupère les coordonnées du "tile" de droite                          
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[(int)_ind_col_I, (int)_ind_row_I].Position.X, level_table2D[(int)_ind_col_I, (int)_ind_row_I].Position.Y));

                            //Récupère les coordonnées du "tile" du bas                           
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col, row + 1].Position.X, level_table2D[col, row + 1].Position.Y));

                            //Récupère les coordonnées du "tile" de gauche                            
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col - 1, row].Position.X, level_table2D[col - 1, row].Position.Y));
                        }

                        else if (level_table2D[col, row].Position == level_table2D[(int)_ind_col_L, (int)_ind_row_L].Position)
                        {
                            //Initialise la liste des successeurs (haut,droite,bas,gauche)                      
                            level_table2D[col, row].List_Successor = new List<Vector2>();

                            //Récupère les coordonnées du "tile" du haut                           
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[(int)_ind_col_W, (int)_ind_row_W].Position.X, level_table2D[(int)_ind_col_W, (int)_ind_row_W].Position.Y));

                            //Récupère les coordonnées du "tile" de droite                          
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col + 1, row].Position.X, level_table2D[col + 1, row].Position.Y));

                            //Récupère les coordonnées du "tile" du bas                           
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col, row + 1].Position.X, level_table2D[col, row + 1].Position.Y));

                            //Récupère les coordonnées du "tile" de gauche                            
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col - 1, row].Position.X, level_table2D[col - 1, row].Position.Y));
                        }

                        else if (level_table2D[col, row].Position == level_table2D[(int)_ind_col_W, (int)_ind_row_W].Position)
                        {
                            //Initialise la liste des successeurs (haut,droite,bas,gauche)                      
                            level_table2D[col, row].List_Successor = new List<Vector2>();

                            //Récupère les coordonnées du "tile" du haut                         
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col, row - 1].Position.X, level_table2D[col, row - 1].Position.Y));

                            //Récupère les coordonnées du "tile" de droite                          
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col + 1, row].Position.X, level_table2D[col + 1, row].Position.Y));

                            //Récupère les coordonnées du "tile" du bas                         
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[(int)_ind_col_L, (int)_ind_row_L].Position.X, level_table2D[(int)_ind_col_L, (int)_ind_row_L].Position.Y));

                            //Récupère les coordonnées du "tile" de gauche                            
                            level_table2D[col, row].List_Successor.Add(new Vector2(level_table2D[col - 1, row].Position.X, level_table2D[col - 1, row].Position.Y));
                        }
                    }
                }
            }

            //Defini la position des SuperGoms HG HD BG et BD
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (level_table2D[col, row].IsChar == 'P')
                    {
                        IndexXPosHG = col;
                        IndexYPosHG = row;
                    }

                    if (level_table2D[col, row].IsChar == 'X')
                    {
                        IndexXPosHD = col;
                        IndexYPosHD = row;
                    }

                    if (level_table2D[col, row].IsChar == 'x')
                    {
                        IndexXPosBG = col;
                        IndexYPosBG = row;
                    }

                    if (level_table2D[col, row].IsChar == 'Z')
                    {
                        IndexXPosBD = col;
                        IndexYPosBD = row;
                    }
                }
            }


            //for (int col = 0; col < level_table2D.GetLength(0);col++ )
            //{
            //    for (int row = 0 ; row < level_table2D.GetLength(1);row++)
            //    {
            //        level_table2D[col, row].Tile_Rec = new Rectangle((int)level_table2D[col, row].Position.X, (int)level_table2D[col, row].Position.Y, 15, 15);
            //    }
            //}

            Maze2D = level_table2D;
        }

        #endregion



        /// <summary>
        /// Méthode ResetMaze2DToDefaultPath
        /// Permet de rendre tous les noeuds à IsPath = false
        /// </summary>
        public void ResetMaze2DToDefaultPath(string GhostName)
        {
            //Reset des nodes en mode IsPath = true, à false

            if (GhostName == "Inky")
            {
                foreach (var node in Maze2D)
                {
                    if (node.IsPathInky)
                        node.IsPathInky = false;
                }
            }

            else if (GhostName == "Blinky")
            {
                foreach (var node in Maze2D)
                {
                    if (node.IsPathBlinky)
                        node.IsPathBlinky = false;
                }
            }

            else if (GhostName == "Pinky")
            {
                foreach (var node in Maze2D)
                {
                    if (node.IsPathPinky)
                        node.IsPathPinky = false;
                }
            }

            else if (GhostName == "Klyde")
            {
                foreach (var node in Maze2D)
                {
                    if (node.IsPathKlyde)
                        node.IsPathKlyde = false;
                }
            }
        }


        /// <summary>
        /// Méthode InkyNeighbourNodes
        /// Calcul et exporte sous forme de liste :
        /// les cases voisines du tile courant occupé par Inky
        /// détermine un booléen IsWall pour chaque case voisine et l'enregistre dans la liste
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="inkyPos"></param>
        public void InkyNeighbourNodes(Vector2 inkyPos)
        {
            InkyPos = inkyPos;

            INKY_REC = new Rectangle((int)InkyPos.X + 7, (int)InkyPos.Y + 7, 1, 1);

            //Export des listes de successeurs du tile courant vers la classe Ghost
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (InkyPos == level_table2D[col, row].Position && level_table2D[col, row].List_Successor != null)
                    {
                        //Initialise la liste booléenne des successeurs du tile courant
                        level_table2D[col, row].ListNextNodesIsWall = new List<bool> { };

                        //Rempli la liste des booléens IsWall exportée vers la classe Ghost
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row - 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col + 1, row].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row + 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col - 1, row].IsWall);

                        //Exporte la liste des booléens des tiles voisins vers la classe Ghost
                        listNextNodesIsWallInky = level_table2D[col, row].ListNextNodesIsWall;

                        //Exporte dans la même liste des coordonnées des tiles voisins, la position de départ de INKY (LERP)
                        level_table2D[col, row].List_Successor.Add(InkyPos);

                        //Exporte la liste des coordonnées des tiles voisins vers la classe Ghost                                                                     
                        listNextNodesInky = level_table2D[col, row].List_Successor;
                    }
                }
            }
        }


        /// <summary>
        /// Méthode Update_Klyde
        /// Calcul et exporte sous forme de liste :
        /// les cases voisines du tile courant occupé par Klyde
        /// détermine un booléen IsWall pour chaque case voisine et l'enregistre dans la liste
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="inkyPos"></param>
        public void KlydeNeighbourNodes(Vector2 klydePos)
        {
            KlydePos = klydePos;

            KLYDE_REC = new Rectangle((int)KlydePos.X + 7, (int)KlydePos.Y + 7, 1, 1);

            //Export des listes de successeurs du tile courant vers la classe Ghost
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (KlydePos == level_table2D[col, row].Position && level_table2D[col, row].List_Successor != null)
                    {
                        //Initialise la liste booléenne des successeurs du tile courant
                        level_table2D[col, row].ListNextNodesIsWall = new List<bool> { };

                        //Rempli la liste des booléens IsWall exportée vers la classe Ghost
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row - 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col + 1, row].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row + 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col - 1, row].IsWall);

                        //Exporte la liste des booléens des tiles voisins vers la classe Ghost
                        listNextNodesIsWallKlyde = level_table2D[col, row].ListNextNodesIsWall;

                        //Exporte dans la même liste des coordonnées des tiles voisins, la position de départ de INKY (LERP)
                        level_table2D[col, row].List_Successor.Add(KlydePos);

                        //Exporte la liste des coordonnées des tiles voisins vers la classe Ghost                                                                     
                        listNextNodesKlyde = level_table2D[col, row].List_Successor;
                    }
                }
            }
        }


        /// <summary>
        /// Méthode BlinkyNeighbourNodes
        /// Calcul et exporte sous forme de liste :
        /// les cases voisines du tile courant occupé par Pinky
        /// détermine un booléen IsWall pour chaque case voisine et l'enregistre dans la liste
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="inkyPos"></param>
        public void PinkyNeighbourNodes(Vector2 pinkyPos)
        {
            PinkyPos = pinkyPos;

            PINKY_REC = new Rectangle((int)PinkyPos.X + 7, (int)PinkyPos.Y + 7, 1, 1);

            //Export des listes de successeurs du tile courant vers la classe Ghost
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (PinkyPos == level_table2D[col, row].Position && level_table2D[col, row].List_Successor != null)
                    {
                        //Initialise la liste booléenne des successeurs du tile courant
                        level_table2D[col, row].ListNextNodesIsWall = new List<bool> { };

                        //Rempli la liste des booléens IsWall exportée vers la classe Ghost
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row - 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col + 1, row].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row + 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col - 1, row].IsWall);

                        //Exporte la liste des booléens des tiles voisins vers la classe Ghost
                        listNextNodesIsWallPinky = level_table2D[col, row].ListNextNodesIsWall;

                        //Exporte dans la même liste des coordonnées des tiles voisins, la position de départ de INKY (LERP)
                        level_table2D[col, row].List_Successor.Add(PinkyPos);

                        //Exporte la liste des coordonnées des tiles voisins vers la classe Ghost                                                                     
                        listNextNodesPinky = level_table2D[col, row].List_Successor;
                    }
                }
            }
        }


        /// <summary>
        /// Méthode Update_Blinky
        /// Calcul et exporte sous forme de liste :
        /// les cases voisines du tile courant occupé par Blinky
        /// détermine un booléen IsWall pour chaque case voisine et l'enregistre dans la liste
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="inkyPos"></param>
        public void BlinkyNeighbourNodes(Vector2 blinkyPos)
        {
            BlinkyPos = blinkyPos;

            BLINKY_REC = new Rectangle((int)BlinkyPos.X + 7, (int)BlinkyPos.Y + 7, 1, 1);

            //Export des listes de successeurs du tile courant vers la classe Ghost
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (BlinkyPos == level_table2D[col, row].Position && level_table2D[col, row].List_Successor != null)
                    {
                        //Initialise la liste booléenne des successeurs du tile courant
                        level_table2D[col, row].ListNextNodesIsWall = new List<bool> { };

                        //Rempli la liste des booléens IsWall exportée vers la classe Ghost
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row - 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col + 1, row].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row + 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col - 1, row].IsWall);

                        //Exporte la liste des booléens des tiles voisins vers la classe Ghost
                        listNextNodesIsWallBlinky = level_table2D[col, row].ListNextNodesIsWall;

                        //Exporte dans la même liste des coordonnées des tiles voisins, la position de départ de INKY (LERP)
                        level_table2D[col, row].List_Successor.Add(BlinkyPos);

                        //Exporte la liste des coordonnées des tiles voisins vers la classe Ghost                                                                     
                        listNextNodesBlinky = level_table2D[col, row].List_Successor;
                    }
                }
            }
        }

        /// <summary>
        /// Méthode PacManNeighbourNodes
        /// Calcul et exporte sous forme de liste :
        /// les cases voisines du tile courant occupé par PacMan
        /// détermine un booléen IsWall pour chaque case voisine et l'enregistre dans la liste
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="PacPos"></param>
        public void PacManNeighbourNodes(Vector2 PacPos)
        {
            this.PacPos = PacPos;

            PAC_REC = new Rectangle((int)PacPos.X, (int)PacPos.Y, 1, 1);

            //Export des listes de successeurs du tile courant vers la classe PacMan
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (PacPos == level_table2D[col, row].Position && level_table2D[col, row].List_Successor != null)
                    {
                        //Initialise la liste booléenne des successeurs du tile courant
                        level_table2D[col, row].ListNextNodesIsWall = new List<bool> { };

                        //Rempli la liste des booléens IsWall exportée vers la classe PacMan
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row - 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col + 1, row].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row + 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col - 1, row].IsWall);

                        //Exporte la liste des booléens des tiles voisins vers la classe PacMan
                        ListIfNextNodesIsWallPacMan = level_table2D[col, row].ListNextNodesIsWall;

                        //Exporte dans la même liste des coordonnées des tiles voisins, la position de départ de PACMAN (LERP)
                        level_table2D[col, row].List_Successor.Add(PacPos);

                        //Exporte la liste des coordonnées des tiles voisins vers la classe PacMan                                                                     
                        ListNextNodesOfPacMan = level_table2D[col, row].List_Successor;
                    }
                }
            }
        }

        /// <summary>
        /// Méthode PacManNeighbourNodes
        /// Calcul et exporte sous forme de liste :
        /// les cases voisines du tile courant occupé par PacMan
        /// détermine un booléen IsWall pour chaque case voisine et l'enregistre dans la liste
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="PacPos"></param>
        public void BonusNeighbourNodes(Vector2 bonusPos)
        {
            this.bonusPos = bonusPos;

            BONUS_REC = new Rectangle((int)bonusPos.X + 7, (int)bonusPos.Y + 7, 1, 1);

            //Export des listes de successeurs du tile courant vers la classe PacMan
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (bonusPos == level_table2D[col, row].Position && level_table2D[col, row].List_Successor != null)
                    {
                        //Initialise la liste booléenne des successeurs du tile courant
                        level_table2D[col, row].ListNextNodesIsWall = new List<bool> { };

                        //Rempli la liste des booléens IsWall exportée vers la classe PacMan
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row - 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col + 1, row].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col, row + 1].IsWall);
                        level_table2D[col, row].ListNextNodesIsWall.Add(level_table2D[col - 1, row].IsWall);

                        //Exporte la liste des booléens des tiles voisins vers la classe PacMan
                        ListNextNodesIsWallBonus = level_table2D[col, row].ListNextNodesIsWall;

                        //Exporte dans la même liste des coordonnées des tiles voisins, la position de départ de PACMAN (LERP)
                        level_table2D[col, row].List_Successor.Add(bonusPos);

                        //Exporte la liste des coordonnées des tiles voisins vers la classe PacMan                                                                     
                        ListNextNodesBonus = level_table2D[col, row].List_Successor;
                    }
                }
            }
        }

        /// <summary>
        /// Méthode Draw
        /// Permet de dessiner le labyrinthe
        /// </summary>
        /// <param name="spritebatch"></param>
        /// <param name="points_ghost"></param>
        public void Draw(SpriteBatch spritebatch, Color ColorLevel)
        {
            foreach (Node tile in level_table2D)
            {
                switch (tile.IsChar)
                {
                    case 'o': tile.ColoR = ColorLevel; break;
                    case 'n': tile.ColoR = ColorLevel; break;
                    case 'm': tile.ColoR = ColorLevel; break;
                    case 'l': tile.ColoR = ColorLevel; break;
                    case 'k': tile.ColoR = ColorLevel; break;
                    case 'e': tile.ColoR = ColorLevel; break;
                    case 'd': tile.ColoR = ColorLevel; break;
                    case 'c': tile.ColoR = ColorLevel; break;
                    case 'b': tile.ColoR = ColorLevel; break;
                    case 'A': tile.ColoR = ColorLevel; break;
                    case 'G': tile.ColoR = ColorLevel; break;
                    case 'Y': tile.ColoR = ColorLevel; break;
                    case 'B': tile.ColoR = ColorLevel; break;
                    case 'R': tile.ColoR = ColorLevel; break;
                    case 'S': tile.ColoR = ColorLevel; break;
                    case 'O': tile.ColoR = ColorLevel; break;
                    case 'V': tile.ColoR = ColorLevel; break;
                    case 'H': tile.ColoR = ColorLevel; break;
                    case 'F': tile.ColoR = ColorLevel; break;
                    case 'D': tile.ColoR = ColorLevel; break;
                    case 'E': tile.ColoR = ColorLevel; break;
                    case 'N': tile.ColoR = ColorLevel; break;
                    case 'M': tile.ColoR = ColorLevel; break;
                    case 'j': tile.ColoR = ColorLevel; break;
                }

                spritebatch.Draw(tile.Texture, new Vector2(tile.Tile_Rec.X, tile.Tile_Rec.Y), tile.ColoR);
            }

            //foreach(var node in level_table2D)
            //{
            //    if(node.Tile_Rec == node.Position)
            //    spritebatch.Draw(node.Texture, node.Position, Color.Blue);
            //}

            Maze2D = level_table2D;

            #region Tests

            //string text1 = string.Format(" listChar {0}", list_char.Count);
            //spritebatch.DrawString(this.trace, text1, new Vector2(0, 70), Color.Red);

            //string text2 = string.Format(" docName : {0}", );
            //spritebatch.DrawString(this.trace, text2, new Vector2(0, 70), Color.Yellow);

            //string text12 = string.Format("MPAC POS Y : {0} ",_PAC_POS_INIT.Y);
            //spritebatch.DrawString(this.trace, text12, new Vector2(0, 90), Color.White);

            //string text3 = string.Format("Is Wall ?: X = {0} ", level_table2D[0, 14].IsWall);
            //spritebatch.DrawString(this.trace, text3, new Vector2(0, 100), Color.Red);

            #endregion

        }
    }
}