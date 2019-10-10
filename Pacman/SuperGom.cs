using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    public enum Couleur { one, two, tree };       //Classe d'enumeration des couleurs des SuperGoms

    /// <summary>
    /// Classe SuperGom
    /// </summary>
    class SuperGom
    {
        #region FIELDS

        /// <summary>
        /// Poitions, Dessins, Couleurs des PacGoms
        /// </summary>
        protected Texture2D _dessin_supergom;
        protected bool _Anime_SP;
        private float _count_invincible;
        protected float timer_SP_1 = 0;
        protected float timer_SP_2 = 0;
        protected float timer_SP_3 = 0;
        protected float timer_SP_4 = 0;
        private int _tile_length = 15;

        //Variables pour l'animation de SuperGom
        private int _frameColumn, _frameLine;
        private int _timer;
        private int _incr_anim = 5;
        //Variable pour la couleur de la SuperGom (Enum)
        private Couleur _Color;
        protected SpriteFont trace;

        #endregion

        #region PROPERTIES

        public bool IsEaten           //Exporte la propriété SuperGom IsEaten
        { get; set; }

        //Ajoute les points lorsque la SuperGom est mangée
        public int Point { get; set; }

        //Exporte le nombre de SuperGom restantes
        public int Count { get; set; }

        #endregion

        /// <summary>
        /// Constructeur SuperGom
        /// </summary>
        /// <param name="_level_table2D"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SuperGom()
        {
            //Mise à 0 du compteur d'invincibilité
            _count_invincible = 0f;

            //Mise à la valeur true l'animation de la SuperGom
            _Anime_SP = true;

            Count = 0;

            //Animation des framerates des SuperGom
            this._frameColumn = 0;
            this._frameLine = 0;
        }

        /// <summary>
        /// Méthode Count_Invincible qui test si PacMan a mangé une Super Pac Gomme 
        /// Active ou désactive les sons d'invincibilité
        /// Décompte le compteur _count_invincible
        /// Et renvoi un booléen pour l'animation de la Super PacGomme absorbée à false
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Anime_SP"></param>
        /// <returns></returns>
        private void Count_Invincible(int x, int y, Node[,] _level_table2D)
        {
            //SUPER PAC GOMME EN HAUT A GAUCHE                         
            if (_level_table2D[x, y].IsEaten)
            {
                if (_count_invincible > 0)
                {
                    _count_invincible--;
                    IsEaten = true;
                }

                else
                {
                    IsEaten = false;
                    _level_table2D[x, y].IsEaten = false;
                }
            }
        }

        /// <summary>
        /// Méthode qui change la valeur du charactère à 'p' qui renvoi à un élément vide
        /// initialise le compteur _count_invincible à 650f
        /// Et intitialise le booléen de la PacGomme mangée à Is_Eaten = true
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool SuperGom_Eaten(Node[,] _level_table2D, int x, int y)
        {
            _level_table2D[x, y].IsChar = 'p';
            _level_table2D[x, y].ColoR = Color.Black;
            _level_table2D[x, y].IsEaten = true;
            _count_invincible += 650f;
            Point += 50;

            if (Count > 0)
            {
                Count = Count - 1;

            }

            return true;
        }

        /// <summary>
        /// Test_SuperGom_Eaten
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="_PAC_POS"></param>
        /// <param name="_level_table2D"></param>
        /// <returns></returns>
        private bool Test_SuperGom_Eaten(int x, int y, Vector2 _PAC_POS, Node[,] _level_table2D)
        {
            if (_PAC_POS == _level_table2D[x, y].Position && _level_table2D[x, y].IsChar == 'P'
                || _PAC_POS == _level_table2D[x, y].Position && _level_table2D[x, y].IsChar == 'X'
                || _PAC_POS == _level_table2D[x, y].Position && _level_table2D[x, y].IsChar == 'x'
                || _PAC_POS == _level_table2D[x, y].Position && _level_table2D[x, y].IsChar == 'Z')
            {
                _level_table2D[x, y].IsEaten = SuperGom_Eaten(_level_table2D, x, y);
            }
            return _level_table2D[x, y].IsEaten;
        }

        /// <summary>
        /// Anim_SuperGom
        /// prend en paramètre d'entrée une variable de type string de la couleur précédente
        /// </summary>
        /// <param name="color"></param>
        private void Anime_SuperGom(string color)
        {
            switch (color)
            {
                case "one":
                    _timer++;
                    if (this._timer == this._incr_anim)
                    {
                        _timer = 0;
                        this._Color = Couleur.two;
                        break;
                    }

                    ; break;

                case "two":
                    _timer++;
                    if (this._timer == this._incr_anim)
                    {
                        _timer = 0;
                        this._Color = Couleur.tree;
                        break;
                    }

                    ; break;

                case "tree":
                    _timer++;
                    if (this._timer == this._incr_anim)
                    {
                        _timer = 0;
                        this._Color = Couleur.one;
                        break;
                    }

                    ; break;
            }
        }

        /// <summary>
        /// Methode de chargement des SuperPacgommes
        /// </summary>
        /// <param name="content"></param>
        /// <param name="textureName"></param>
        public void LoadContent(ContentManager content, string textureName)
        {
            trace = content.Load<SpriteFont>("fullscreen_font");
            _dessin_supergom = content.Load<Texture2D>(textureName);
        }

        /// <summary>
        /// Méthode Update_PacGoms 
        /// pendant 10 secondes.
        /// Emploi la méthode Is_PacMan_Invincible qui renvoi un booléan si PACMAN est invincible
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="_PAC_POS_INIT"></param>
        /// <param name="level_table2D"></param>  
        public void UpdateSuperGoms(int x, int y, Vector2 _PAC_POS, Node[,] _level_table2D)
        {
            //Animation des SuperGom
            Anime_SuperGom(_Color.ToString());

            //Test si la SuperGom est mangée
            _level_table2D[x, y].IsEaten = Test_SuperGom_Eaten(x, y, _PAC_POS, _level_table2D);

            //Test si PacMan est invincible
            Count_Invincible(x, y, _level_table2D);
        }

        /// <summary>
        /// Méthode publique d'affichage des PacGommes
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="level"></param>
        public void Draw_SuperGoms(SpriteBatch spriteBatch, Node[,] level_table2D, int x, int y)
        {
            if (level_table2D[x, y].IsChar == 'P' || level_table2D[x, y].IsChar == 'X' || level_table2D[x, y].IsChar == 'x' || level_table2D[x, y].IsChar == 'Z')
            {
                spriteBatch.Draw(_dessin_supergom,                                                                                  //Charge le Sprite
                new Rectangle((int)level_table2D[x, y].Position.X, (int)level_table2D[x, y].Position.Y, _tile_length, _tile_length),                    //Rectangle de Destination
                new Rectangle((this._frameColumn) * _tile_length, (this._frameLine) * _tile_length, _tile_length, _tile_length),                                            //Rectangle Source 
                Color.White,                                                                                                        //Defini la couleur
                0f,
                new Vector2(0, 0),
                SpriteEffects.None,
                0f);
            }

            switch (this._Color)
            {
                case Couleur.one: this._frameColumn = 0; break;     //Couleur 1 de la bandelette de texture2D
                case Couleur.two: this._frameColumn = 1; break;     //Couleur 2 de la bandelette de texture2D
                case Couleur.tree: this._frameColumn = 2; break;    //Couleur 3 de la bandelette de texture2D       
            }
            #region Tests

            //   string text1 = string.Format("Is _number_supergom ? = {0}", _number_supergom);
            //spriteBatch.DrawString(this.trace, text1, new Vector2(0, 30), Color.Yellow);

            //  string text2 = string.Format("Letter ? = {0}", level_table2D[2, 3].IsChar);
            //  spriteBatch.DrawString(this.trace, text2, new Vector2(0, 20), Color.Red);

            #endregion
        }
    }
}