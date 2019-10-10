using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{

    /// <summary>
    /// Classe de gestion des directions enumerées
    /// </summary>
    public enum BonusDirection { Up, Down, Left, Right };


    /// <summary>
    /// Classe LifeAndBonus
    /// </summary>
    class LifeAndBonus : Sprite
    {
        #region FIELDS

        //Trace debugging SpriteFont
        protected SpriteFont trace;

        //Points pour les bonus
        private const int _point_cerise = 100;
        private const int _point_fraise = 300;
        private const int _point_orange = 500;
        private const int _point_pomme = 700;
        private const int _point_melon = 1000;
        private const int _point_galboss = 2000;
        private const int _point_cloche = 3000;
        private const int _point_cle = 5000;

        private int totalPointBonus = 0;
        private BonusDirection bonusDirection = BonusDirection.Left;
        private bool _Is_FullScreen = false;

        //Determine le délai d'affichage du fruit bonus
        private int timer = 0;
        private int maxTimer = 1500;

        //Variables des fruits dans le labyrinthe
        private Texture2D _texture_fruit, _texture_big_fruit, _fruit_display;

        //Dessin des vie du joueur
        private Texture2D _dessin_life, _life_display;
        private SoundEffect sndBonusEaten;
        private SoundEffectInstance instSndBonusEaten;
        private int _num_life_PacMan;

        //Variable globale qui récupère le level du constructeur
        private int _level = 0;
        private string levelName;

        #endregion

        #region PROPERTIES

        //Exporte les points en fonction du fruits
        public int Point { get; set; }

        //Exporte la variable qui valide l'affichage du fruit Bonus dans le Level
        public bool IsBonusEatable { get { return Is_Fruit_Visible(); } set { Is_Fruit_Visible(); } }

        //Défini un reset à 0 de la valeur du timerOpeningSong lors d'une nouvelle partie
        public int Timer_Delay { get { return timer; } set { timer = value; } }

        #endregion


        /// <summary>
        /// Méthode Is_Fruit_Visible
        /// Valide l'affichage du fruit
        /// </summary>
        /// <returns></returns>
        private bool Is_Fruit_Visible()
        {
            if (timer < maxTimer)
            {
                timer++;

                return false;
            }

            else
            {
                return true;
            }
        }


        /// <summary>
        /// Constructeur LifeAndBonus
        /// </summary>
        /// <param name="SpritePos"></param>
        /// <param name="level"></param>
        /// <param name="isFullScreen"></param>
        public LifeAndBonus(Vector2 sprite_position, int level, int num_life)
            : base(new Vector2(0, 0), level, num_life)
        {
            _num_life_PacMan = num_life;
            SpritePos = sprite_position;
            _level = level;
        }

        /// <summary>
        /// Méthode LoadContent
        /// </summary>
        /// <param name="content"></param>
        /// <param name="textureName"></param>
        /// <param name="PosSprite"></param>
        public override void LoadContent(ContentManager content, int level, Vector2 PosSprite, bool isFullScreen, string levelname)
        {
            //LoadContent traceDebugging
            trace = content.Load<SpriteFont>("fullscreen_font");

            levelName = levelname;

            //Charge le son lorsque le FRUIT est mangé
            sndBonusEaten = content.Load<SoundEffect>("pacman_eatfruit");
            instSndBonusEaten = sndBonusEaten.CreateInstance();
            instSndBonusEaten.IsLooped = false;
            instSndBonusEaten.Play();
            instSndBonusEaten.Pause();

            //Initialise la position de PACMAN sur le tile 'A'
            SpritePos = PosSprite;

            //Récupère la variable d'affichage plein écran
            _Is_FullScreen = isFullScreen;

            _level = level;

            //Chargement du fruit et des points            
            _texture_fruit = content.Load<Texture2D>(levelname);
            _fruit_display = content.Load<Texture2D>("Fruit_display");
            _life_display = content.Load<Texture2D>("life_display");

            totalPointBonus = _point_cerise;

            switch (levelname)
            {
                case "small_cerise": totalPointBonus = _point_cerise; break;
                case "small_fraise": totalPointBonus = _point_fraise; break;
                case "small_orange": totalPointBonus = _point_orange; break;
                case "small_pomme": totalPointBonus = _point_pomme; break;
                case "small_melon": totalPointBonus = _point_melon; break;
                case "small_galboss": totalPointBonus = _point_galboss; break;
                case "small_cloche": totalPointBonus = _point_cloche; break;
                case "small_cle": totalPointBonus = _point_cle; break;
            }

            base.LoadContent(content, _level, SpritePos, isFullScreen, levelname);
        }

        /// <summary>
        /// Méthode DisplayBonus
        /// Permet de charger l'image des "Life" de PacMan
        /// </summary>
        /// <param name="content"></param>
        public void DisplayBonus(ContentManager content, Vector2 Fruit_Pos)
        {
            {
                //Dessin du nombre de vies de PacMan                
                _dessin_life = content.Load<Texture2D>("dessin_PacMan");

                switch (levelName)
                {
                    case "small_cerise": _texture_big_fruit = content.Load<Texture2D>("cerise"); break;
                    case "small_fraise": _texture_big_fruit = content.Load<Texture2D>("fraise"); break;
                    case "small_orange": _texture_big_fruit = content.Load<Texture2D>("orange"); break;
                    case "small_pomme": _texture_big_fruit = content.Load<Texture2D>("pomme"); break;
                    case "small_melon": _texture_big_fruit = content.Load<Texture2D>("melon"); break;
                    case "small_galboss": _texture_big_fruit = content.Load<Texture2D>("galboss"); break;
                    case "small_cloche": _texture_big_fruit = content.Load<Texture2D>("cloche"); break;
                    case "small_cle": _texture_big_fruit = content.Load<Texture2D>("cle"); break;
                }
            }
        }

        /// <summary>
        /// Méthode Initialize
        /// </summary>
        /// <param name="coefArray"></param>
        /// <param name="vect_sprite"></param>
        public override void Initialize(float[] tabcoef, Vect vect_sprite)
        {
            VectDir = vect_sprite;
            LerpCoefArray = tabcoef;

            base.Initialize(tabcoef, vect_sprite);
        }


        /// <summary>
        /// Méthode UpdateBonus
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="_hashCodeDir"></param>
        /// <param name="listNextNodes"></param>
        /// <param name="spritePos"></param>
        /// <param name="targetPos"></param>
        /// <param name="_listNextNodeIsWall"></param>
        /// <param name="leftCoridor"></param>
        /// <param name="rightCoridor"></param>
        /// <param name="table_coef_sprite"></param>
        public override void UpdateMoveAndPosition(GameTime gameTime, int _hashCodeDir, List<Vector2> _listNextNodes, Vector2 spritePos,
            List<bool> _listNextNodeIsWall, Vector2 leftCoridor, Vector2 rightCoridor, Vector2 upCoridor, Vector2 downCoridor, float[] listCoefLerp)
        {
            if (Is_Fruit_Visible() == true)
            {
                HashCodeDir = _hashCodeDir;
                NextNode = _listNextNodes;
                SpritePos = spritePos;
                ListNextIsWall = _listNextNodeIsWall;
                LerpCoefArray = listCoefLerp;

                //Gestion du dessin des yeux du fantôme suivant le HashCodeDir
                switch (HashCodeDir)
                {
                    case 32: bonusDirection = BonusDirection.Left; break;
                    case 64: bonusDirection = BonusDirection.Up; break;
                    case 128: bonusDirection = BonusDirection.Right; break;
                    case 256: bonusDirection = BonusDirection.Down; break;
                }
            }

            base.UpdateMoveAndPosition(gameTime, _hashCodeDir, _listNextNodes, spritePos, _listNextNodeIsWall, leftCoridor, rightCoridor, upCoridor, downCoridor, listCoefLerp);
        }

        /// <summary>
        /// Méthode DisplayLife
        /// Permet de dessiner sur l'écran de jeu les vies de PacMan
        /// </summary>
        /// <param name="spritebatch"></param>
        public void DisplayLife(SpriteBatch spritebatch, int num_life, Rectangle _rec_maze)
        {
            _num_life_PacMan = num_life;
            Rectangle _rec_life = new Rectangle((int)_rec_maze.X - _life_display.Width, (int)_rec_maze.Y + 15 + (int)_rec_maze.Height - _life_display.Height, _life_display.Width, _life_display.Height);

            spritebatch.Draw(_life_display, new Vector2(_rec_life.X, _rec_life.Y), Color.White);

            switch (_num_life_PacMan)
            {
                case 1: spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 40, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); break;
                case 2: spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 30, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 50, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); break;
                case 3: spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 20, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 40, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 60, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); break;
                case 4: spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 10, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 30, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 50, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 70, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); break;
                case 5: spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 20, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 40, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 60, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 80, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); break;
                case 6: spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X - 10, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 10, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 30, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 50, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 70, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); spritebatch.Draw(_dessin_life, new Vector2(_rec_life.X + 90, _rec_life.Y + _rec_life.Height / 2), Color.Yellow); break;
            }
        }

        /// <summary>
        /// Méthode DisplayBonus
        /// Permet de dessiner sur l'écran de jeu le fruit du level
        /// </summary>
        /// <param name="spritebatch"></param>
        public void DisplayBonus(SpriteBatch spritebatch, bool Is_Full_Screen, Rectangle _rec_maze)
        {

            Rectangle _rec_fruit_bonus = new Rectangle((int)_rec_maze.X + (int)_rec_maze.Width + 15, (int)_rec_maze.Y + (int)_rec_maze.Height - _fruit_display.Height + _fruit_display.Height / 4, _fruit_display.Width, _fruit_display.Height);

            spritebatch.Draw(_fruit_display, new Vector2(_rec_fruit_bonus.X, _rec_fruit_bonus.Y), Color.White);
            spritebatch.Draw(_texture_big_fruit, new Vector2(_rec_fruit_bonus.X + 30, _rec_fruit_bonus.Y + 45), Color.White);
        }


        /// <summary>
        /// Méthode Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="color"></param>
        /// <param name="_is_fruit_eaten"></param>
        public void Draw(SpriteBatch spriteBatch, Color color, bool _is_fruit_eaten)
        {
            //Si le fruit n'est pas mangé, on dessine
            if (_is_fruit_eaten == false && Is_Fruit_Visible() == true)
            {
                SpriteTexture = _texture_fruit;
                Rectangle dest_rec = new Rectangle((int)SpritePos.X, (int)SpritePos.Y, 20, 20);
                Rectangle source_rec = new Rectangle(0, 0, 20, 20);
                color = Color.White;
                float rotation = 0f;
                Vector2 origin = Vector2.Zero;
                SpriteEffects effects = SpriteEffects.None;
                float layer = 0f;

                base.Draw(spriteBatch, SpriteTexture, dest_rec, source_rec, color, rotation, origin, effects, layer);
            }

            else if (_is_fruit_eaten == true && SpriteColor == Color.White)
            {
                instSndBonusEaten.Volume = 1.0f;
                instSndBonusEaten.Play();
                SpriteColor = Color.Black;
                Point = totalPointBonus;

            }

            #region Test

            //string text1 = string.Format("_is_fruit_eaten ? = {0}", _is_fruit_eaten);
            //spriteBatch.DrawString(this.trace, text1, new Vector2(0, 30), Color.Yellow);

            //  string text2 = string.Format("Letter ? = {0}", level_table2D[2, 3].IsChar);
            //  spriteBatch.DrawString(this.trace, text2, new Vector2(0, 20), Color.Red);

            #endregion
        }
    }
}