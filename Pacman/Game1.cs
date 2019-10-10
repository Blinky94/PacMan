using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection;

namespace PacMan
{
    /// <summary>
    /// Enumération des différentes vitesses
    /// </summary>
    public enum EnumSpeed { hardlySlow, VerySlow, slow, Static, quicky, veryQuicky, Panic };

    /// <summary>
    /// Enumération des différents états des fantômes
    /// </summary>
    public enum EnumStates { Frightened, Chase, Scatter, Eaten };

    /// <summary>
    /// Classe principale du jeu PacMan
    /// </summary>
    internal class Game1 : Microsoft.Xna.Framework.Game
    {
        #region FIELDS

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level_1 Level;
        PacMan PacMan;
        Ghost Inky, Pinky, Blinky, Klyde;
        astar astarInky, astarBlinky, astarPinky, astarKlyde;
        Randomized randomInky, randomBlinky, randomPinky, randomKlyde, randomBonus;
        PacGom pacGom;
        SuperGom superGomHG, superGomHD, superGomBG, superGomBD;
        LifeAndBonus lifeAndBonus;
        Score score;
        Vector2 GoalInky, GoalBlinky, GoalPinky, GoalKlyde;
        EnumStates inkyState, blinkyState, pinkyState, klydeState, pacManState;
        string levelPath, soundsPath, musicPath;

        //Variables globales
        private int timerOpeningSong = 0;
        private int maxTimerOpeningSong = 250;
        private int timerDead = 0;
        private int maxTimerDead = 90;
        private SoundEffect songOpening;
        private SoundEffectInstance instSongOpening;
        private bool IsNewGame = true;
        private int numLevel = 1;
        private bool endLevel = false;
        private bool isSndSuperPac = false;
        private SoundEffect sndSuperPacMan;
        private SoundEffectInstance sndInstSuperPacMan;
        private int _point_supergom_HG = 0, _point_supergom_HD = 0, _point_supergom_BG = 0, _point_supergom_BD = 0;
        private bool isSuperPacMan = false;
        private int life = 3;
        private int cumulPointGhostEaten = 0;
        private bool IsGameOver = false;
        private int totalPointGhost = 0;
        private int currentScore = 0;
        private int widthScr, heightScr;
        private string level_XML;
        private SpriteFont _PAC_Trace;
        private KeyboardState PlayerKeybState;
        private int HashDirInky = 0, HashDirBlinky = 0, HashDirPinky = 0, HashDirKlyde = 0, HashDirFruit = 0;
        private bool IsCollideBonus = false;
        private bool IsBonusEaten = false;
        //private bool IsPacEaten = false;
        private float[] SpeedOfInky = new float[1], SpeedOfPinky = new float[1], SpeedOfBlinky = new float[1], SpeedOfKlyde = new float[1], SpeedOfPacMan = new float[1], lerpcoefArrayBonus = new float[1];
        private SpriteFont trace;
        private Texture2D textureTest;
        private Texture2D tracetest;
        private Texture2D tracerecGhost;
        private Node NodePacMan, NodeInky, NodeBlinky, NodePinky, NodeKlyde, NodeBonus;
        private bool InkyCollide, PinkyCollide, BlinkyCollide, KlydeCollide;
        private int resultRand = 0;
        //Variables initiales pour la durée d'un état d'un fantôme
        private int timerInky = 0, timerBlinky = 0, timerPinky = 0, timerKlyde = 0;
        private int TimerMaxPinky = 0, TimerMaxInky = 0, TimerMaxBlinky = 0, TimerMaxKlyde = 0;
        private bool IsInkyTakenPlace = false, IsBlinkyTakenPlace = false, IsPinkyTakenPlace = false, IsKlydeTakenPlace = false;
        private String[] tabBonus = new String[] { "small_cerise", "small_fraise", "small_orange", "small_pomme", "small_melon", "small_galboss", "small_cloche", "small_cle" };
        private int indexTabBonus = 0;
        private Color ColorWallLevel;
        private int red = 0, green = 0, blue = 0;
        private bool IsColorWall_Black = false;


        #endregion

        #region PROPERTIES

        internal bool IsFullScreen { get { return graphics.IsFullScreen; } set { graphics.IsFullScreen = value; } }

        #endregion


        /// <summary>
        /// Méthode TimeToState
        /// Permet de définir la durée d'un état
        /// </summary>
        /// <returns></returns>
        internal int TimeToState()
        {
            int time = 0;
            Random rnd = new Random();
            time = rnd.Next(1, 500);

            return time;
        }

        /// <summary>
        ///  Fonction IsPacCollideGhost
        /// Test si la position de PACMAN est la même qu'un GHOST
        /// retourne s'il y a collision ou pas
        /// </summary>
        /// <param name="ghostNode"></param>
        /// <param name="pacNode"></param>
        /// <returns></returns>
        internal bool CollisionTesting(Vector2 ghostNode, Vector2 pacNode)
        {
            return (ghostNode == pacNode) ? true : false;
        }


        /// <summary>
        /// Fonction RecSpriteInNode
        /// </summary>
        /// <param name="SpriteRec"></param>
        /// <returns></returns>
        private Node GetNodeFromPosition(Rectangle SpriteRec)
        {
            Node nodeContainSprite = new Node();

            foreach (var node in Level.Maze2D)
            {
                if (node.Tile_Rec.Contains(SpriteRec))
                {
                    return nodeContainSprite = node;
                }
            }
            return nodeContainSprite;
        }


        /// <summary>
        /// Fonction TotalPointGhostEaten
        /// Calcul le nombre de points cumulés en fonction du nombre
        /// de fantômes mangés
        /// </summary>
        /// <param name="countghostEaten"></param>
        /// <param name="totalpointghost"></param>
        /// <returns></returns>
        internal int TotalPointGhostEaten(int countghostEaten, int totalpointghost)
        {
            int point = 200;

            switch (countghostEaten)
            {
                case 1: totalpointghost += point; break;
                case 2: totalpointghost += point * 2; break;
                case 3: totalpointghost += point * 4; break;
                case 4: totalpointghost += point * 8; break;
            }
            return totalpointghost;
        }


        /// <summary>
        /// Méthode DuringTheDead
        /// </summary>
        internal void DuringTheDead()
        {
            Inky.SpritePos = Level.InkyHome;
            Blinky.SpritePos = Level.BlinkyHome;
            Pinky.SpritePos = Level.PinkyHome;
            Klyde.SpritePos = Level.KlydeHome;

            if (timerDead < maxTimerDead)
            {
                SpeedOfInky = Inky.Speed(EnumSpeed.Static);
                SpeedOfPinky = Pinky.Speed(EnumSpeed.Static);
                SpeedOfBlinky = Blinky.Speed(EnumSpeed.Static);
                SpeedOfKlyde = Klyde.Speed(EnumSpeed.Static);
                SpeedOfPacMan = PacMan.Speed(EnumSpeed.Static);

                timerDead++;
            }

            else
            {
                if (!IsGameOver)
                {
                    timerDead = 0;
                    IsNewGame = true;
                }
                else
                {
                    IsNewGame = false;
                }
            }
        }


        /// <summary>
        /// Méthode ResetNewGame
        /// Initialise le début d'une nouvelle partie
        /// </summary>
        internal void ResetNewGame()
        {
            PacMan.SpritePos = Level.PacManHome;
            PacMan.VectDir = Sprite.Vect.Left;
            Inky.VectDir = Sprite.Vect.Left;
            Blinky.VectDir = Sprite.Vect.Left;
            Pinky.VectDir = Sprite.Vect.Right;
            Klyde.VectDir = Sprite.Vect.Right;
            SpeedOfPacMan = PacMan.Speed(EnumSpeed.veryQuicky);

            pacManState = EnumStates.Scatter;

            inkyState = RandomEnumStateGhost();
            pinkyState = RandomEnumStateGhost();
            blinkyState = RandomEnumStateGhost();
            klydeState = RandomEnumStateGhost();

            //RAZ Astar des fantômes
            astarInky.IsPathFound = false;
            astarBlinky.IsPathFound = false;
            astarPinky.IsPathFound = false;
            astarKlyde.IsPathFound = false;

            ResetMaze2DToDefaultPath("Inky");
            ResetMaze2DToDefaultPath("Blinky");
            ResetMaze2DToDefaultPath("Pinky");
            ResetMaze2DToDefaultPath("Klyde");

            IsInkyTakenPlace = false;
            IsBlinkyTakenPlace = false;
            IsPinkyTakenPlace = false;
            IsKlydeTakenPlace = false;

            instSongOpening.Play();

            if (timerOpeningSong < maxTimerOpeningSong)
            {
                timerOpeningSong++;
            }

            else
            {
                timerOpeningSong = 0;
                instSongOpening.Pause();
                IsNewGame = false;
            }
        }



        /// <summary>
        /// Constructeur de la classe Game1
        /// </summary>
        internal Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "content";
            levelPath = @"D:\GameDevelop\Pacman\Pacman\Levels\";
            //IsFixedTimeStep = true;
            //TargetElapsedTime = TimeSpan.FromSeconds(100);

            //IsFixedTimeStep = true;
            //TargetElapsedTime = TimeSpan.FromMilliseconds(5f);

            widthScr = Window.ClientBounds.Width;
            heightScr = Window.ClientBounds.Height;

            //Gestion du plein écran ou mode fenêtre
            graphics.IsFullScreen = false;

            //Charge les dimensions en mode fenêtre
            if (!graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = 650;
                graphics.PreferredBackBufferHeight = 530;
            }

            ///Instancie un objet de la classe PacGom
            pacGom = new PacGom();
            ///Instancie les 4 objets de la classe SuperGom
            superGomHG = new SuperGom();
            superGomHD = new SuperGom();
            superGomBG = new SuperGom();
            superGomBD = new SuperGom();
            //Intialise la direction aléatoire avec l'objet randomInky
            randomInky = new Randomized();
            randomBlinky = new Randomized();
            randomPinky = new Randomized();
            randomKlyde = new Randomized();
            //Chargement du Sprite PACMAN
            PacMan = new PacMan();
            //Chargement des Sprite GHOST
            Inky = new Ghost();
            Pinky = new Ghost();
            Blinky = new Ghost();
            Klyde = new Ghost();
            score = new Score();

            //Charge les vies de PacMan à l'écran ainsi que le fruit
            lifeAndBonus = new LifeAndBonus(Vector2.Zero, life, numLevel);
            Level = new Level_1(widthScr, heightScr);
            level_XML = @"G:\Level_1.xml";
            //initialise la direction aléatoire de l'objet fruit
            randomBonus = new Randomized();
        }



        /// <summary>
        /// Méthode Initialize principale de la classe Game1
        /// </summary>
        protected override void Initialize()
        {
            IsColorWall_Black = true;

            while (IsColorWall_Black == true)
            {
                Random rnd = new Random();
                Type colorType = typeof(System.Drawing.Color);
                PropertyInfo[] propInfo = colorType.GetProperties(BindingFlags.Static | BindingFlags.Public);

                System.Drawing.Color colorName = System.Drawing.Color.FromName(propInfo[rnd.Next
                 (0, propInfo.Length)].Name);

                if (colorName.R == 0 && colorName.G == 0 && colorName.B == 0)
                {
                    IsColorWall_Black = true;
                }
                else
                {
                    IsColorWall_Black = false;
                    red = colorName.R;
                    green = colorName.G;
                    blue = colorName.B;
                }
            }

            switch (indexTabBonus)
            {
                case 0: level_XML = levelPath + "Level_1.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 1: level_XML = levelPath + "Level_2.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 2: level_XML = levelPath + "Level_3.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 3: level_XML = levelPath + "Level_4.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 4: level_XML = levelPath + "Level_5.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 5: level_XML = levelPath + "Level_6.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 6: level_XML = levelPath + "Level_7.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 7: level_XML = levelPath + "Level_8.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 8: level_XML = levelPath + "Level_9.xml"; ColorWallLevel = new Color(red, green, blue); break;
                case 9: level_XML = levelPath + "Level_10.xml"; ColorWallLevel = new Color(red, green, blue); break;
            }

            Level.Initialize(level_XML);
            PacMan.Initialize(SpeedOfPacMan, PacMan.Vect.Left);
            Inky.Initialize(SpeedOfInky, Ghost.Vect.Right);
            Blinky.Initialize(SpeedOfBlinky, Ghost.Vect.Down);
            Pinky.Initialize(SpeedOfPinky, Ghost.Vect.Up);
            Klyde.Initialize(SpeedOfKlyde, Ghost.Vect.Right);
            lifeAndBonus.Initialize(lerpcoefArrayBonus, LifeAndBonus.Vect.Up);

            base.Initialize();
        }


        /// <summary>
        /// Méthode LoadSound
        /// Charge les effets sonores du jeu
        /// </summary>
        protected void LoadSound(ContentManager content)
        {
            //Sons PacMan mange
            sndSuperPacMan = content.Load<SoundEffect>("PAC_POWER2");
            sndInstSuperPacMan = sndSuperPacMan.CreateInstance();
            sndInstSuperPacMan.IsLooped = true;
            sndInstSuperPacMan.Play();
            sndInstSuperPacMan.Pause();

            //Musique de début de partie
            songOpening = content.Load<SoundEffect>("pacman_beginning");
            instSongOpening = songOpening.CreateInstance();
            instSongOpening.IsLooped = false;
            instSongOpening.Play();
            instSongOpening.Pause();
        }


        protected override void UnloadContent()
        {
            Content.Unload();

            base.UnloadContent();
        }
        /// <summary>
        /// Méthode LoadContent principale de la classe Game1
        /// </summary>
        protected override void LoadContent()
        {
            //Charge les effets sonores du jeu
            LoadSound(Content);

            randomInky.LoadContent(Content);
            randomPinky.LoadContent(Content);
            randomBlinky.LoadContent(Content);
            randomKlyde.LoadContent(Content);

            //LoadContent randomBonus
            randomBonus.LoadContent(Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            trace = Content.Load<SpriteFont>("fullscreen_font");
            _PAC_Trace = Content.Load<SpriteFont>("fullscreen_font");

            //Charge le contenu du labyrinthe
            Level.LoadContent(Content, "ABD", "ABG", "AHG", "ARV", "LH", "LV", "way_tile", "ADBD", "ADBG", "ADHG", "ADRV",
                "LHB", "LHH", "LVD", "LVG", "Door", "ADLG", "ALDB", "ALBG", "ALRV", "CC", "DD", "EE", "FF", "GG", "HH",
                this.IsFullScreen, "Pellet_tile", "Sup_Pellet_tile", graphics);

            //Charge les tiles avec les pacgommes,
            pacGom.LoadContent(Content, "Pellet_tile", "WAKA", Level.Maze2D);
            //Charge les textures de chaques SuperGoms
            superGomHG.LoadContent(Content, "Sup_Pellet_tile");
            superGomHD.LoadContent(Content, "Sup_Pellet_tile");
            superGomBG.LoadContent(Content, "Sup_Pellet_tile");
            superGomBD.LoadContent(Content, "Sup_Pellet_tile");

            PacMan.LoadContent(Content, "New_Paco", Level.PacPos, Level.Maze2D);
            Inky.LoadContent(Content, "Big_Inky", Level.InkyPos, Level.Maze2D);
            Blinky.LoadContent(Content, "Big_Blinky", Level.BlinkyPos, Level.Maze2D);
            Pinky.LoadContent(Content, "Big_Pinky", Level.PinkyPos, Level.Maze2D);
            Klyde.LoadContent(Content, "Big_Klyde", Level.KlydePos, Level.Maze2D);

            textureTest = Content.Load<Texture2D>("way_tile");
            tracetest = Content.Load<Texture2D>("detect_tile");
            tracerecGhost = Content.Load<Texture2D>("recGhost");

            if (endLevel == false)
            {
                score.Load_Score(Content);
            }

            lifeAndBonus.LoadContent(Content, numLevel, Level.bonusPos, IsFullScreen, tabBonus[indexTabBonus]);


            //Charge le dessin des vies de PacMan
            lifeAndBonus.DisplayBonus(Content, Level.bonusPos);

            astarInky = new astar(Level, Level.Width, Level.Height, "Inky");
            astarBlinky = new astar(Level, Level.Width, Level.Height, "Blinky");
            astarPinky = new astar(Level, Level.Width, Level.Height, "Pinky");
            astarKlyde = new astar(Level, Level.Width, Level.Height, "Klyde");

            base.LoadContent();
        }

        /// <summary>
        /// Méthode ResetMaze2DToDefaultPath
        /// Permet de rendre tous les noeuds à IsPath = false
        /// </summary>
        private void ResetMaze2DToDefaultPath(string GhostName)
        {
            //Reset des nodes en mode IsPath = true, à false

            if (GhostName == "Inky")
            {
                foreach (var node in Level.Maze2D)
                {
                    if (node.IsPathInky)
                        node.IsPathInky = false;
                }
            }

            else if (GhostName == "Blinky")
            {
                foreach (var node in Level.Maze2D)
                {
                    if (node.IsPathBlinky)
                        node.IsPathBlinky = false;
                }
            }

            else if (GhostName == "Pinky")
            {
                foreach (var node in Level.Maze2D)
                {
                    if (node.IsPathPinky)
                        node.IsPathPinky = false;
                }
            }

            else if (GhostName == "Klyde")
            {
                foreach (var node in Level.Maze2D)
                {
                    if (node.IsPathKlyde)
                        node.IsPathKlyde = false;
                }
            }
        }


        /// <summary>
        /// Function Select a GhostState randomly
        /// </summary>
        /// <returns></returns>
        private EnumStates RandomEnumStateGhost()
        {
            EnumStates state = EnumStates.Chase;

            Random rnd = new Random();
            resultRand = rnd.Next(1, 101);

            if (resultRand >= 1 && resultRand <= 85)
            {
                state = EnumStates.Chase;
            }
            if (resultRand > 86 && resultRand <= 100)
            {
                state = EnumStates.Scatter;
            }

            return state;
        }

        /// <summary>
        /// function TimerMax for calculate the max duration randomly
        /// </summary>
        /// <returns></returns>
        private int TimerMax()
        {
            int timerMax = 0;
            Random rnd = new Random();
            int rndResult = rnd.Next(1, 6);

            switch (rndResult)
            {
                case 1: timerMax = 50; break;
                case 2: timerMax = 100; break;
                case 3: timerMax = 200; break;
                case 4: timerMax = 500; break;
                case 5: timerMax = 1000; break;
            }

            return timerMax;
        }

        /// <summary>
        /// Méthode UpdateMoveAndPosition
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            #region UPDATE SUPERGOMS

            superGomHG.UpdateSuperGoms(Level.IndexXPosHG, Level.IndexYPosHG, Level.PacPos, Level.Maze2D); _point_supergom_HG = superGomHG.Point;
            superGomHD.UpdateSuperGoms(Level.IndexXPosHD, Level.IndexYPosHD, Level.PacPos, Level.Maze2D); _point_supergom_HD = superGomHD.Point;
            superGomBG.UpdateSuperGoms(Level.IndexXPosBG, Level.IndexYPosBG, Level.PacPos, Level.Maze2D); _point_supergom_BG = superGomBG.Point;
            superGomBD.UpdateSuperGoms(Level.IndexXPosBD, Level.IndexYPosBD, Level.PacPos, Level.Maze2D); _point_supergom_BD = superGomBD.Point;

            #endregion

            #region LOAD NEW LEVEL

            if (endLevel == true)
            {
                numLevel = numLevel + 1;

                if (indexTabBonus < 7)
                {
                    indexTabBonus++;
                }
                else
                {
                    indexTabBonus = 0;
                }

                endLevel = false;

                superGomHG.IsEaten = false;
                superGomHD.IsEaten = false;
                superGomBG.IsEaten = false;
                superGomBD.IsEaten = false;

                superGomHG.Count = 1;
                superGomHD.Count = 1;
                superGomBG.Count = 1;
                superGomBD.Count = 1;

                isSuperPacMan = false;
                isSndSuperPac = false;

                sndInstSuperPacMan.Pause();

                lifeAndBonus.SpriteColor = Color.White;
                lifeAndBonus.Timer_Delay = 0;
                IsBonusEaten = false;

                UnloadContent();
                this.Initialize();
                this.LoadContent();

                IsNewGame = true;
            }

            if (IsNewGame == true)
            {
                ResetNewGame();
            }

            #endregion

            #region UPDATE DU LEVEL

            else
            {
                //Regule la vitesse du Bonus dans le labyrinthe
                lerpcoefArrayBonus = lifeAndBonus.Speed(EnumSpeed.hardlySlow);

                //Calcul des voisins du node occupé par le Sprite
                Level.PacManNeighbourNodes(PacMan.SpritePos);
                Level.InkyNeighbourNodes(Inky.SpritePos);
                Level.BlinkyNeighbourNodes(Blinky.SpritePos);
                Level.PinkyNeighbourNodes(Pinky.SpritePos);
                Level.KlydeNeighbourNodes(Klyde.SpritePos);
                Level.BonusNeighbourNodes(lifeAndBonus.SpritePos);

                //Récupère les positions de chaque Sprite pour les noeuds du Level
                NodePacMan = GetNodeFromPosition(Level.PAC_REC);
                NodeInky = GetNodeFromPosition(Level.INKY_REC);
                NodePinky = GetNodeFromPosition(Level.PINKY_REC);
                NodeBlinky = GetNodeFromPosition(Level.BLINKY_REC);
                NodeKlyde = GetNodeFromPosition(Level.KLYDE_REC);
                NodeBonus = GetNodeFromPosition(Level.BONUS_REC);

                if (pacManState != EnumStates.Eaten)
                {
                    PacMan.UpdateMoveAndPosition(gameTime, PlayerKeybState.GetHashCode(), Level.ListNextNodesOfPacMan,
                      Level.PacPos, Level.ListIfNextNodesIsWallPacMan,
                      Level.LeftCoridor, Level.RightCoridor, Level.UpCoridor, Level.DownCoridor, SpeedOfPacMan);
                }

                if (inkyState != EnumStates.Eaten || NodeInky.Position != Level.InkyHome)
                {
                    Inky.UpdateMoveAndPosition(gameTime, HashDirInky, Level.listNextNodesInky, Level.InkyPos,
                        Level.listNextNodesIsWallInky,
                        Level.LeftCoridor, Level.RightCoridor, Level.UpCoridor, Level.DownCoridor, SpeedOfInky);
                }

                if (blinkyState != EnumStates.Eaten || NodeBlinky.Position != Level.BlinkyHome)
                {
                    Blinky.UpdateMoveAndPosition(gameTime, HashDirBlinky, Level.listNextNodesBlinky, Level.BlinkyPos,
                        Level.listNextNodesIsWallBlinky,
                        Level.LeftCoridor, Level.RightCoridor, Level.UpCoridor, Level.DownCoridor, SpeedOfBlinky);
                }

                if (pinkyState != EnumStates.Eaten || NodePinky.Position != Level.PinkyHome)
                {
                    Pinky.UpdateMoveAndPosition(gameTime, HashDirPinky, Level.listNextNodesPinky, Level.PinkyPos,
                        Level.listNextNodesIsWallPinky,
                        Level.LeftCoridor, Level.RightCoridor, Level.UpCoridor, Level.DownCoridor, SpeedOfPinky);
                }

                if (klydeState != EnumStates.Eaten || NodeKlyde.Position != Level.KlydeHome)
                {
                    Klyde.UpdateMoveAndPosition(gameTime, HashDirKlyde, Level.listNextNodesKlyde, Level.KlydePos,
                        Level.listNextNodesIsWallKlyde,
                        Level.LeftCoridor, Level.RightCoridor, Level.UpCoridor, Level.DownCoridor, SpeedOfKlyde);
                }

                #region METHODE UPDATE DE PACMAN

                //Récupération de la direction du joueur sur le clavier
                PlayerKeybState = Keyboard.GetState();
                SpeedOfPacMan = PacMan.Speed(EnumSpeed.veryQuicky);

                //Méthode de test permanent pour la collision des fantômes et de PacMan
                IsCollideBonus = CollisionTesting(NodeBonus.Position, NodePacMan.Position);

                //Si PacMan rentre en collision avec le fruit et que ce dernier est "visible" ou "eatable"
                if (IsCollideBonus && lifeAndBonus.IsBonusEatable == true)
                {
                    IsBonusEaten = true;
                }

                //Méthode UpdateMoveAndPosition de l'instance PacMan de la SuperClasse Sprite

                #endregion

                #region UPDATE DES SPRITES

                //Test de collision entre PacMan et les fantômes             
                InkyCollide = CollisionTesting(NodeInky.Position, NodePacMan.Position);
                PinkyCollide = CollisionTesting(NodePinky.Position, NodePacMan.Position);
                BlinkyCollide = CollisionTesting(NodeBlinky.Position, NodePacMan.Position);
                KlydeCollide = CollisionTesting(NodeKlyde.Position, NodePacMan.Position);

                //INKY
                if (IsInkyTakenPlace == false)
                {
                    foreach (var node in Level.Maze2D)
                    {
                        if (node.IsChar == 'X')
                        {
                            GoalInky = node.Position;
                        }
                    }

                    switch (astarInky.IsPathFound)
                    {
                        case false:

                            astarInky.SearchCible(Inky.SpritePos, GoalInky);

                            break;

                        case true:

                            if (NodeInky.Position != GoalInky)
                            {
                                HashDirInky = astarInky.GoToCible(Level, Level.listNextNodesInky);
                            }

                            else
                            {
                                SpeedOfInky = Inky.Speed(EnumSpeed.veryQuicky);
                            }
                            break;
                    }

                    IsInkyTakenPlace = true;
                }

                else
                {
                    switch (isSuperPacMan)
                    {
                        case true: //SuperPacMan

                            if (InkyCollide && inkyState == EnumStates.Frightened)
                            {
                                inkyState = EnumStates.Eaten;
                                ResetMaze2DToDefaultPath("Inky");
                                astarInky.IsPathFound = false;
                                cumulPointGhostEaten++; //PacMan marque des Points
                                totalPointGhost = TotalPointGhostEaten(cumulPointGhostEaten, totalPointGhost);
                            }

                            if (InkyCollide && inkyState == EnumStates.Eaten)
                            {
                                inkyState = EnumStates.Eaten;
                            }

                            if (!InkyCollide && inkyState == EnumStates.Scatter || !InkyCollide && inkyState == EnumStates.Chase)
                            {
                                inkyState = EnumStates.Frightened;

                                ResetMaze2DToDefaultPath("Inky");
                                astarInky.IsPathFound = false;
                            }

                            if (!InkyCollide && inkyState == EnumStates.Eaten)
                            {
                                inkyState = EnumStates.Eaten;
                            }

                            if (!InkyCollide && inkyState == EnumStates.Frightened)
                            {
                                ResetMaze2DToDefaultPath("Inky");
                                astarInky.IsPathFound = false;
                                inkyState = EnumStates.Frightened;
                            }

                            break;

                        case false: //Pas SuperPacMan

                            if (inkyState == EnumStates.Eaten || inkyState == EnumStates.Frightened)
                            {
                                //RAZ des états du fantôme
                                inkyState = EnumStates.Scatter;
                                astarInky.IsPathFound = false;
                                SpeedOfInky = Inky.Speed(EnumSpeed.veryQuicky);
                                ResetMaze2DToDefaultPath("Inky");
                                IsInkyTakenPlace = false;
                                cumulPointGhostEaten = 0;
                            }

                            if (inkyState == EnumStates.Scatter)
                            {
                                if (TimerMaxInky == 0)
                                {
                                    TimerMaxInky = TimerMax();
                                }

                                else if (timerInky < TimerMaxInky)
                                {
                                    timerInky++;
                                }

                                else
                                {
                                    ResetMaze2DToDefaultPath("Inky");
                                    astarInky.IsPathFound = false;
                                    inkyState = RandomEnumStateGhost();
                                    TimerMaxInky = TimerMax();
                                    timerInky = 0;
                                }
                            }

                            else if (inkyState == EnumStates.Chase)
                            {
                                if (GoalInky == NodeInky.Position)
                                {
                                    ResetMaze2DToDefaultPath("Inky");
                                    astarInky.IsPathFound = false;
                                    inkyState = RandomEnumStateGhost();
                                    TimerMaxInky = TimerMax();
                                    timerInky = 0;
                                }
                            }

                            if (InkyCollide)
                            {
                                if (pacManState != EnumStates.Eaten)
                                {
                                    pacManState = EnumStates.Eaten;
                                    SpeedOfPacMan = PacMan.Speed(EnumSpeed.Static);
                                    SpeedOfInky = Inky.Speed(EnumSpeed.Static);

                                    if (life > 1)
                                    {
                                        life--;
                                        IsGameOver = false;
                                    }

                                    else
                                    {
                                        life--;
                                        IsGameOver = true;
                                    }
                                }
                            }
                            break;
                    }

                    if (inkyState == EnumStates.Eaten)
                    {
                        SpeedOfInky = Inky.Speed(EnumSpeed.Panic);
                        GoalInky = Level.InkyHome;

                        switch (astarInky.IsPathFound)
                        {
                            case false:

                                ResetMaze2DToDefaultPath("Inky");
                                astarInky.SearchCible(Inky.SpritePos, GoalInky);

                                break;

                            case true:

                                if (NodeInky.Position != GoalInky)
                                {
                                    HashDirInky = astarInky.GoToCible(Level, Level.listNextNodesInky);
                                }

                                else
                                {
                                    SpeedOfInky = Inky.Speed(EnumSpeed.Static);
                                }
                                break;
                        }
                    }

                    else if (inkyState == EnumStates.Frightened)
                    {
                        SpeedOfInky = Inky.Speed(EnumSpeed.VerySlow);
                        HashDirInky = randomInky.HashRand(Level.listNextNodesIsWallInky);
                    }

                    else if (inkyState == EnumStates.Scatter)
                    {
                        astarInky.IsPathFound = false;
                        SpeedOfInky = Inky.Speed(EnumSpeed.veryQuicky);
                        HashDirInky = randomInky.HashRand(Level.listNextNodesIsWallInky);
                    }

                    else if (inkyState == EnumStates.Chase)
                    {
                        SpeedOfInky = Inky.Speed(EnumSpeed.veryQuicky);

                        switch (astarInky.IsPathFound)
                        {
                            case false:

                                GoalInky = NodePacMan.Position;
                                astarInky.SearchCible(Inky.SpritePos, GoalInky);

                                break;

                            case true:

                                if (GoalInky != NodeInky.Position)
                                {
                                    HashDirInky = astarInky.GoToCible(Level, Level.listNextNodesInky);
                                }

                                else
                                {
                                    astarInky.IsPathFound = false;
                                }
                                break;
                        }
                    }
                }

                //BLINKY
                if (IsBlinkyTakenPlace == false)
                {
                    foreach (var node in Level.Maze2D)
                    {
                        if (node.IsChar == 'P')
                        {
                            GoalBlinky = node.Position;
                        }
                    }

                    switch (astarBlinky.IsPathFound)
                    {
                        case false:

                            astarBlinky.SearchCible(NodeBlinky.Position, GoalBlinky);

                            break;

                        case true:

                            if (NodeBlinky.Position != GoalBlinky)
                            {
                                HashDirBlinky = astarBlinky.GoToCible(Level, Level.listNextNodesBlinky);
                            }

                            else
                            {
                                SpeedOfBlinky = Blinky.Speed(EnumSpeed.veryQuicky);
                            }
                            break;
                    }

                    IsBlinkyTakenPlace = true;
                }
                else
                {
                    switch (isSuperPacMan)
                    {
                        case true: //SuperPacMan

                            if (BlinkyCollide && blinkyState == EnumStates.Frightened)
                            {
                                blinkyState = EnumStates.Eaten;
                                ResetMaze2DToDefaultPath("Blinky");
                                astarBlinky.IsPathFound = false;
                            }

                            if (BlinkyCollide && blinkyState == EnumStates.Eaten)
                            {
                                blinkyState = EnumStates.Eaten;
                            }

                            if (!BlinkyCollide && blinkyState == EnumStates.Scatter || !BlinkyCollide && blinkyState == EnumStates.Chase)
                            {
                                blinkyState = EnumStates.Frightened;
                                ResetMaze2DToDefaultPath("Blinky");
                                astarBlinky.IsPathFound = false;
                                cumulPointGhostEaten++; //PacMan marque des Points
                                totalPointGhost = TotalPointGhostEaten(cumulPointGhostEaten, totalPointGhost);
                            }

                            if (!BlinkyCollide && blinkyState == EnumStates.Eaten)
                            {
                                blinkyState = EnumStates.Eaten;
                            }

                            if (!BlinkyCollide && blinkyState == EnumStates.Frightened)
                            {
                                ResetMaze2DToDefaultPath("Blinky");
                                astarBlinky.IsPathFound = false;
                                blinkyState = EnumStates.Frightened;
                            }
                            break;

                        case false: //Pas SuperPacMan

                            if (blinkyState == EnumStates.Eaten || blinkyState == EnumStates.Frightened)
                            {
                                //RAZ des états du fantôme
                                blinkyState = EnumStates.Scatter;
                                astarBlinky.IsPathFound = false;
                                SpeedOfBlinky = Blinky.Speed(EnumSpeed.veryQuicky);
                                ResetMaze2DToDefaultPath("Blinky");
                                cumulPointGhostEaten = 0;
                            }

                            if (blinkyState == EnumStates.Scatter)
                            {
                                if (TimerMaxBlinky == 0)
                                {
                                    TimerMaxBlinky = TimerMax();
                                }

                                else if (timerBlinky < TimerMaxBlinky)
                                {
                                    timerBlinky++;
                                }

                                else
                                {
                                    ResetMaze2DToDefaultPath("Blinky");
                                    astarBlinky.IsPathFound = false;
                                    blinkyState = RandomEnumStateGhost();
                                    TimerMaxBlinky = TimerMax();
                                    timerBlinky = 0;
                                }
                            }

                            else if (blinkyState == EnumStates.Chase)
                            {
                                if (GoalBlinky == NodeBlinky.Position)
                                {
                                    ResetMaze2DToDefaultPath("Blinky");
                                    astarBlinky.IsPathFound = false;
                                    blinkyState = RandomEnumStateGhost();
                                    TimerMaxBlinky = TimerMax();
                                    timerBlinky = 0;
                                }
                            }

                            if (BlinkyCollide)
                            {
                                if (pacManState != EnumStates.Eaten)
                                {
                                    pacManState = EnumStates.Eaten;
                                    SpeedOfPacMan = PacMan.Speed(EnumSpeed.Static);
                                    SpeedOfBlinky = Blinky.Speed(EnumSpeed.Static);

                                    if (life > 1)
                                    {
                                        life--;
                                        IsGameOver = false;
                                    }

                                    else
                                    {
                                        life--;
                                        IsGameOver = true;
                                    }
                                }
                            }

                            break;
                    }

                    if (blinkyState == EnumStates.Eaten)
                    {
                        SpeedOfBlinky = Blinky.Speed(EnumSpeed.Panic);
                        GoalBlinky = Level.BlinkyHome;

                        switch (astarBlinky.IsPathFound)
                        {
                            case false:

                                ResetMaze2DToDefaultPath("Blinky");
                                astarBlinky.SearchCible(Blinky.SpritePos, GoalBlinky);

                                break;

                            case true:

                                if (NodeBlinky.Position != GoalBlinky)
                                {
                                    HashDirBlinky = astarBlinky.GoToCible(Level, Level.listNextNodesBlinky);
                                }

                                else
                                {
                                    SpeedOfBlinky = Blinky.Speed(EnumSpeed.Static);
                                }
                                break;
                        }
                    }

                    else if (blinkyState == EnumStates.Frightened)
                    {
                        SpeedOfBlinky = Blinky.Speed(EnumSpeed.VerySlow);
                        HashDirBlinky = randomBlinky.HashRand(Level.listNextNodesIsWallBlinky);
                    }

                    else if (blinkyState == EnumStates.Scatter)
                    {
                        astarBlinky.IsPathFound = false;
                        SpeedOfBlinky = Blinky.Speed(EnumSpeed.veryQuicky);
                        HashDirBlinky = randomBlinky.HashRand(Level.listNextNodesIsWallBlinky);
                    }

                    else if (blinkyState == EnumStates.Chase)
                    {
                        SpeedOfBlinky = Blinky.Speed(EnumSpeed.veryQuicky);

                        switch (astarBlinky.IsPathFound)
                        {
                            case false:

                                GoalBlinky = NodePacMan.Position;
                                astarBlinky.SearchCible(Blinky.SpritePos, GoalBlinky);

                                break;

                            case true:

                                if (GoalBlinky != NodeBlinky.Position)
                                {
                                    HashDirBlinky = astarBlinky.GoToCible(Level, Level.listNextNodesBlinky);
                                }

                                else
                                {
                                    astarBlinky.IsPathFound = false;
                                }

                                break;
                        }
                    }
                }

                //PINKY
                if (IsPinkyTakenPlace == false)
                {
                    foreach (var node in Level.Maze2D)
                    {
                        if (node.IsChar == 'Z')
                        {
                            GoalPinky = node.Position;
                        }
                    }

                    switch (astarPinky.IsPathFound)
                    {
                        case false:

                            astarPinky.SearchCible(NodePinky.Position, GoalPinky);

                            break;

                        case true:

                            if (NodePinky.Position != GoalPinky)
                            {
                                HashDirPinky = astarPinky.GoToCible(Level, Level.listNextNodesPinky);
                            }

                            else
                            {
                                SpeedOfPinky = Pinky.Speed(EnumSpeed.veryQuicky);
                            }
                            break;
                    }

                    IsPinkyTakenPlace = true;
                }
                else
                {
                    switch (isSuperPacMan)
                    {
                        case true: //SuperPacMan

                            if (PinkyCollide && pinkyState == EnumStates.Frightened)
                            {
                                pinkyState = EnumStates.Eaten;
                                ResetMaze2DToDefaultPath("Pinky");
                                astarPinky.IsPathFound = false;
                                cumulPointGhostEaten++; //PacMan marque des Points
                                totalPointGhost = TotalPointGhostEaten(cumulPointGhostEaten, totalPointGhost);
                            }

                            if (PinkyCollide && pinkyState == EnumStates.Eaten)
                            {
                                pinkyState = EnumStates.Eaten;
                            }

                            if (!PinkyCollide && pinkyState == EnumStates.Scatter || !PinkyCollide && pinkyState == EnumStates.Chase)
                            {
                                pinkyState = EnumStates.Frightened;
                                ResetMaze2DToDefaultPath("Pinky");
                                astarPinky.IsPathFound = false;
                            }

                            if (!PinkyCollide && pinkyState == EnumStates.Eaten)
                            {
                                pinkyState = EnumStates.Eaten;
                            }

                            if (!PinkyCollide && pinkyState == EnumStates.Frightened)
                            {
                                ResetMaze2DToDefaultPath("Pinky");
                                astarPinky.IsPathFound = false;
                                pinkyState = EnumStates.Frightened;
                            }
                            break;

                        case false: //Pas SuperPacMan

                            if (pinkyState == EnumStates.Eaten || pinkyState == EnumStates.Frightened)
                            {
                                //RAZ des états du fantôme
                                pinkyState = EnumStates.Scatter;
                                astarPinky.IsPathFound = false;
                                SpeedOfPinky = Pinky.Speed(EnumSpeed.veryQuicky);
                                ResetMaze2DToDefaultPath("Pinky");
                                cumulPointGhostEaten = 0;
                            }

                            if (pinkyState == EnumStates.Scatter)
                            {
                                if (TimerMaxPinky == 0)
                                {
                                    TimerMaxPinky = TimerMax();
                                }

                                else if (timerPinky < TimerMaxPinky)
                                {
                                    timerPinky++;
                                }
                                else
                                {
                                    ResetMaze2DToDefaultPath("Pinky");
                                    astarPinky.IsPathFound = false;
                                    pinkyState = RandomEnumStateGhost();
                                    TimerMaxPinky = TimerMax();
                                    timerPinky = 0;
                                }
                            }
                            else if (pinkyState == EnumStates.Chase)
                            {
                                if (GoalPinky == NodePinky.Position)
                                {
                                    ResetMaze2DToDefaultPath("Pinky");
                                    astarPinky.IsPathFound = false;
                                    pinkyState = RandomEnumStateGhost();
                                    TimerMaxPinky = TimerMax();
                                    timerPinky = 0;
                                }
                            }

                            if (PinkyCollide)
                            {

                                if (pacManState != EnumStates.Eaten)
                                {
                                    pacManState = EnumStates.Eaten;
                                    SpeedOfPacMan = PacMan.Speed(EnumSpeed.Static);
                                    SpeedOfPinky = Pinky.Speed(EnumSpeed.Static);

                                    if (life > 1)
                                    {
                                        life--;
                                        IsGameOver = false;
                                    }

                                    else
                                    {
                                        life--;
                                        IsGameOver = true;
                                    }
                                }
                            }
                            break;
                    }

                    if (pinkyState == EnumStates.Eaten)
                    {
                        SpeedOfPinky = Pinky.Speed(EnumSpeed.Panic);
                        GoalPinky = Level.PinkyHome;

                        switch (astarPinky.IsPathFound)
                        {
                            case false:

                                ResetMaze2DToDefaultPath("Pinky");
                                astarPinky.SearchCible(Pinky.SpritePos, GoalPinky);

                                break;

                            case true:

                                if (NodePinky.Position != GoalPinky)
                                {
                                    HashDirPinky = astarPinky.GoToCible(Level, Level.listNextNodesPinky);
                                }
                                else
                                {
                                    SpeedOfPinky = Pinky.Speed(EnumSpeed.Static);
                                }
                                break;
                        }
                    }

                    else if (pinkyState == EnumStates.Frightened)
                    {
                        SpeedOfPinky = Pinky.Speed(EnumSpeed.VerySlow);
                        HashDirPinky = randomPinky.HashRand(Level.listNextNodesIsWallPinky);
                    }

                    else if (pinkyState == EnumStates.Scatter)
                    {
                        astarPinky.IsPathFound = false;
                        SpeedOfPinky = Pinky.Speed(EnumSpeed.veryQuicky);
                        HashDirPinky = randomPinky.HashRand(Level.listNextNodesIsWallPinky);
                    }

                    else if (pinkyState == EnumStates.Chase)
                    {
                        SpeedOfPinky = Pinky.Speed(EnumSpeed.veryQuicky);

                        switch (astarPinky.IsPathFound)
                        {
                            case false:

                                GoalPinky = NodePacMan.Position;
                                astarPinky.SearchCible(Pinky.SpritePos, GoalPinky);

                                break;

                            case true:

                                if (GoalPinky != NodePinky.Position)
                                {
                                    HashDirPinky = astarPinky.GoToCible(Level, Level.listNextNodesPinky);
                                }

                                else
                                {
                                    astarPinky.IsPathFound = false;
                                }
                                break;
                        }
                    }
                }

                //KLYDE
                if (IsKlydeTakenPlace == false)
                {
                    foreach (var node in Level.Maze2D)
                    {
                        if (node.IsChar == 'x')
                        {
                            GoalKlyde = node.Position;
                        }
                    }

                    switch (astarKlyde.IsPathFound)
                    {
                        case false:

                            astarKlyde.SearchCible(NodeKlyde.Position, GoalKlyde);

                            break;

                        case true:

                            if (NodeKlyde.Position != GoalKlyde)
                            {
                                HashDirKlyde = astarKlyde.GoToCible(Level, Level.listNextNodesKlyde);
                            }

                            else
                            {
                                SpeedOfKlyde = Klyde.Speed(EnumSpeed.veryQuicky);
                            }
                            break;
                    }
                    IsKlydeTakenPlace = true;
                }
                else
                {
                    switch (isSuperPacMan)
                    {
                        case true: //SuperPacMan

                            if (KlydeCollide && klydeState == EnumStates.Frightened)
                            {
                                klydeState = EnumStates.Eaten;
                                ResetMaze2DToDefaultPath("Klyde");
                                astarKlyde.IsPathFound = false;
                                cumulPointGhostEaten++; //PacMan marque des Points
                                totalPointGhost = TotalPointGhostEaten(cumulPointGhostEaten, totalPointGhost);
                            }

                            if (KlydeCollide && klydeState == EnumStates.Eaten)
                            {
                                klydeState = EnumStates.Eaten;
                            }

                            if (!KlydeCollide && klydeState == EnumStates.Scatter || !KlydeCollide && klydeState == EnumStates.Chase)
                            {
                                klydeState = EnumStates.Frightened;
                                ResetMaze2DToDefaultPath("Klyde");
                                astarKlyde.IsPathFound = false;
                            }

                            if (!KlydeCollide && klydeState == EnumStates.Eaten)
                            {
                                klydeState = EnumStates.Eaten;
                            }

                            if (!KlydeCollide && klydeState == EnumStates.Frightened)
                            {
                                ResetMaze2DToDefaultPath("Klyde");
                                astarKlyde.IsPathFound = false;
                                klydeState = EnumStates.Frightened;
                            }
                            break;

                        case false: //Pas SuperPacMan

                            if (klydeState == EnumStates.Eaten || klydeState == EnumStates.Frightened)
                            {
                                //RAZ des états du fantôme
                                klydeState = EnumStates.Scatter;
                                astarKlyde.IsPathFound = false;
                                SpeedOfKlyde = Klyde.Speed(EnumSpeed.veryQuicky);
                                ResetMaze2DToDefaultPath("Klyde");
                                cumulPointGhostEaten = 0;
                            }

                            if (klydeState == EnumStates.Scatter)
                            {
                                if (TimerMaxKlyde == 0)
                                {
                                    TimerMaxKlyde = TimerMax();
                                }
                                else if (timerKlyde < TimerMaxKlyde)
                                {
                                    timerKlyde++;
                                }
                                else
                                {
                                    ResetMaze2DToDefaultPath("Klyde");
                                    astarKlyde.IsPathFound = false;
                                    klydeState = RandomEnumStateGhost();
                                    TimerMaxKlyde = TimerMax();
                                    timerKlyde = 0;
                                }
                            }
                            else if (klydeState == EnumStates.Chase)
                            {

                                if (GoalKlyde == NodeKlyde.Position)
                                {
                                    ResetMaze2DToDefaultPath("Klyde");
                                    astarKlyde.IsPathFound = false;
                                    klydeState = RandomEnumStateGhost();
                                    TimerMaxKlyde = TimerMax();
                                    timerKlyde = 0;
                                }
                            }

                            if (KlydeCollide)
                            {
                                if (pacManState != EnumStates.Eaten)
                                {
                                    pacManState = EnumStates.Eaten;
                                    SpeedOfPacMan = PacMan.Speed(EnumSpeed.Static);
                                    SpeedOfKlyde = Klyde.Speed(EnumSpeed.Static);

                                    if (life > 1)
                                    {
                                        life--;
                                        IsGameOver = false;
                                    }
                                    else
                                    {
                                        life--;
                                        IsGameOver = true;
                                    }
                                }
                            }
                            break;
                    }

                    if (klydeState == EnumStates.Eaten)
                    {
                        SpeedOfKlyde = Klyde.Speed(EnumSpeed.Panic);
                        GoalKlyde = Level.KlydeHome;

                        switch (astarKlyde.IsPathFound)
                        {
                            case false:

                                ResetMaze2DToDefaultPath("Klyde");
                                astarKlyde.SearchCible(Klyde.SpritePos, GoalKlyde);

                                break;

                            case true:

                                if (NodeKlyde.Position != GoalKlyde)
                                {
                                    HashDirKlyde = astarKlyde.GoToCible(Level, Level.listNextNodesKlyde);
                                }

                                else
                                {
                                    SpeedOfKlyde = Klyde.Speed(EnumSpeed.Static);
                                }
                                break;
                        }
                    }
                    else if (klydeState == EnumStates.Frightened)
                    {
                        SpeedOfKlyde = Klyde.Speed(EnumSpeed.VerySlow);
                        HashDirKlyde = randomKlyde.HashRand(Level.listNextNodesIsWallKlyde);
                    }

                    else if (klydeState == EnumStates.Scatter)
                    {
                        astarKlyde.IsPathFound = false;
                        SpeedOfKlyde = Klyde.Speed(EnumSpeed.veryQuicky);
                        HashDirKlyde = randomKlyde.HashRand(Level.listNextNodesIsWallKlyde);
                    }
                    else if (klydeState == EnumStates.Chase)
                    {
                        SpeedOfKlyde = Klyde.Speed(EnumSpeed.veryQuicky);

                        switch (astarKlyde.IsPathFound)
                        {
                            case false:

                                GoalKlyde = NodePacMan.Position;
                                astarKlyde.SearchCible(Klyde.SpritePos, GoalKlyde);

                                break;

                            case true:

                                if (GoalKlyde != NodeKlyde.Position)
                                {
                                    HashDirKlyde = astarKlyde.GoToCible(Level, Level.listNextNodesKlyde);
                                }
                                else
                                {
                                    astarKlyde.IsPathFound = false;
                                }

                                break;
                        }
                    }
                }

                #endregion

                #region UPDATE DU BONUS

                //Génère le code de Hashage pour la direction aléatoire du FRUIT
                HashDirFruit = randomBonus.HashRand(Level.ListNextNodesIsWallBonus);

                //Mise à jour de la position du FRUIT
                lifeAndBonus.UpdateMoveAndPosition(gameTime, HashDirFruit, Level.ListNextNodesBonus, Level.bonusPos, Level.ListNextNodesIsWallBonus, Level.LeftCoridor, Level.RightCoridor, Level.UpCoridor, Level.DownCoridor, lerpcoefArrayBonus);

                #endregion

                #region UPDATE DES PACGOMS ET SUPERGOMS

                //Appel de la méthode UpdateMoveAndPosition de l'instance PacGom
                pacGom.Update_PacGoms(Level.PacPos, Level.Maze2D, pacGom.Point);

                if ((pacGom.NumberPacGom + superGomHG.Count + superGomHD.Count + superGomBG.Count + superGomBD.Count) == 0)
                {
                    endLevel = true;
                }

                //Test si PacMan est en mode invincible et valide si le son invincible doit etre activé (son Super_Pac)
                if (superGomHG.IsEaten || superGomHD.IsEaten || superGomBG.IsEaten || superGomBD.IsEaten)
                {
                    isSndSuperPac = true;
                    isSuperPacMan = true;
                }
                else
                {
                    isSndSuperPac = false;
                    isSuperPacMan = false;
                }

                //Test si le son invincible est activé et joue le son
                if (isSndSuperPac)
                {
                    sndInstSuperPacMan.Resume();
                }
                else
                {
                    sndInstSuperPacMan.Pause();
                }

                #endregion

                //Somme de tous les points et stockage dans la variable currentscore
                currentScore = pacGom.Point + superGomHG.Point + superGomHD.Point + superGomBG.Point + superGomBD.Point + lifeAndBonus.Point + totalPointGhost;

                //Calcul et mise en place de l'affichage du score par la méthode Update_Score
                score.Update_Score(IsGameOver, currentScore);

            #endregion

                base.Update(gameTime);
            }
        }


        /// <summary>
        /// Méthode Draw principale de la classe Game1
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Level.Draw(spriteBatch, ColorWallLevel);

            score.DisplayScore(spriteBatch, IsFullScreen, numLevel, Level.MazeRec);
            lifeAndBonus.DisplayLife(spriteBatch, life, Level.MazeRec);
            lifeAndBonus.DisplayBonus(spriteBatch, IsFullScreen, Level.MazeRec);

            //Affichage des superGoms suivant leurs coordonnées vectorielles          
            superGomHG.Draw_SuperGoms(spriteBatch, Level.Maze2D, Level.IndexXPosHG, Level.IndexYPosHG);
            superGomHD.Draw_SuperGoms(spriteBatch, Level.Maze2D, Level.IndexXPosHD, Level.IndexYPosHD);
            superGomBG.Draw_SuperGoms(spriteBatch, Level.Maze2D, Level.IndexXPosBG, Level.IndexYPosBG);
            superGomBD.Draw_SuperGoms(spriteBatch, Level.Maze2D, Level.IndexXPosBD, Level.IndexYPosBD);

            lifeAndBonus.Draw(spriteBatch, Color.White, IsBonusEaten);

            //string text23 = string.Format("level_XML : {0}", level_XML.ToString());
            //spriteBatch.DrawString(trace, text23, new Vector2(0, 15), Color.Red);

            //Affichage des tiles de chaque chemin d'un fantôme
            //foreach (var node in Level.Maze2D)
            //{
            //    if (node.IsPathInky)
            //    {
            //        spriteBatch.Draw(textureTest, new Vector2(node.Position.X, node.Position.Y), Color.LightBlue);
            //    }

            //    //    if (node.IsPathPinky)
            //    //    {
            //    //        spriteBatch.Draw(textureTest, new Vector2(node.Position.X, node.Position.Y), Color.Pink);
            //    //    }
            //    if (node.IsPathBlinky)
            //    {
            //        spriteBatch.Draw(textureTest, new Vector2(node.Position.X, node.Position.Y), Color.Red);
            //    }
            //}
            //    if (node.IsPathKlyde)
            //    {
            //        spriteBatch.Draw(textureTest, new Vector2(node.Position.X, node.Position.Y), Color.Orange);
            //    }
            //}

            if (IsNewGame)
            {
                PacMan.DrawNormal(spriteBatch);
                Inky.DrawNormal(spriteBatch);
                Blinky.DrawNormal(spriteBatch);
                Pinky.DrawNormal(spriteBatch);
                Klyde.DrawNormal(spriteBatch);
                score.DisplayReady(IsFullScreen, spriteBatch);
            }

            //Si ce n'est pas une nouvelle partie
            else
            {
                switch (inkyState)
                {
                    case EnumStates.Chase: Inky.DrawNormal(spriteBatch); break;
                    case EnumStates.Scatter: Inky.DrawNormal(spriteBatch); break;
                    case EnumStates.Frightened: Inky.DrawFear(spriteBatch); break;
                    case EnumStates.Eaten: Inky.DrawEaten(spriteBatch); break;
                }

                switch (pinkyState)
                {
                    case EnumStates.Chase: Pinky.DrawNormal(spriteBatch); break;
                    case EnumStates.Scatter: Pinky.DrawNormal(spriteBatch); break;
                    case EnumStates.Frightened: Pinky.DrawFear(spriteBatch); break;
                    case EnumStates.Eaten: Pinky.DrawEaten(spriteBatch); break;
                }

                switch (blinkyState)
                {
                    case EnumStates.Chase: Blinky.DrawNormal(spriteBatch); break;
                    case EnumStates.Scatter: Blinky.DrawNormal(spriteBatch); break;
                    case EnumStates.Frightened: Blinky.DrawFear(spriteBatch); break;
                    case EnumStates.Eaten: Blinky.DrawEaten(spriteBatch); break;
                }

                switch (klydeState)
                {
                    case EnumStates.Chase: Klyde.DrawNormal(spriteBatch); break;
                    case EnumStates.Scatter: Klyde.DrawNormal(spriteBatch); break;
                    case EnumStates.Frightened: Klyde.DrawFear(spriteBatch); break;
                    case EnumStates.Eaten: Klyde.DrawEaten(spriteBatch); break;
                }

                switch (pacManState)
                {
                    case EnumStates.Scatter: PacMan.DrawNormal(spriteBatch); break;
                    case EnumStates.Eaten:

                        if (!IsGameOver)
                        {
                            DuringTheDead();
                            PacMan.DrawDead(spriteBatch, IsGameOver);
                        }

                        else
                        {
                            DuringTheDead();
                            PacMan.DrawDead(spriteBatch, IsGameOver);
                            score.DisplayGameOver(IsFullScreen, spriteBatch);
                        } break;
                }
            }

            //astarInky.DrawPath(spriteBatch, textureTest);

            #region Tests

            //if (NodeInky != null)
            //{
            //    string text4 = string.Format("totalPointGhost {0}", totalPointGhost);
            //    spriteBatch.DrawString(this.trace, text4, new Vector2(150, 10), Color.Yellow);

            //    string text5 = string.Format("cumulPointGhostEaten= {0}", cumulPointGhostEaten);
            //    spriteBatch.DrawString(this.trace, text5, new Vector2(150, 30), Color.Yellow);
            //}
            //    string text6 = string.Format("pathfound= {0}", astarInky.IsPathFound);
            //    spriteBatch.DrawString(this.trace, text6, new Vector2(0, 560), Color.Yellow);

            //    string text7 = string.Format("pathfound?= {0}", astarBlinky.IsPathFound);
            //    spriteBatch.DrawString(this.trace, text7, new Vector2(0, 580), Color.Yellow);

            //    //if (astarInky.ListPath.Count > 0)
            //    //{
            //    //    for (int i = 0; i < astarInky.ListPath.Count - 1; i++)
            //    //    {
            //    //        string text6 = string.Format("{0}", astarInky.ListPath[i]);
            //    //        spriteBatch.DrawString(this.trace, text6, new Vector2(0, 200 + (i * 20)), Color.Yellow);                               
            //    //    }
            //    //}


            //    //bool is_true = false;

            //    //    for(int col = 0 ; col < Level.Maze2D.GetLength(0);col++)    
            //    //    {
            //    //        for (int row = 0 ; row < Level.Maze2D.GetLength(1);row++)
            //    //        {
            //    //             if (Level.Maze2D[col,row].Tile_Rec.Contains(Level.INKY_REC))
            //    //            {
            //    //                is_true = true;    
            //    //            }

            //    //             else                   
            //    //             {                        
            //    //                 is_true = false;                                        
            //    //             }                                             
            //    //    }                                           
            //    //}



            //    //spriteBatch.Draw(tracerecGhost, new Vector2((int)Level.INKY_REC.X, (int)Level.INKY_REC.Y), Color.Yellow);
            //    //string text7 = string.Format("Level.InkyPos != GoalInky= {0}", Level.InkyPos != GoalInky);
            //    //spriteBatch.DrawString(this.trace, text7, new Vector2(0, 60), Color.Yellow);

            //    //string text8 = string.Format("isSuperPac?= {0}", isSuperPacMan);
            //    //spriteBatch.DrawString(this.trace, text8, new Vector2(0, 110), Color.Yellow);
            //}


            #endregion

            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}