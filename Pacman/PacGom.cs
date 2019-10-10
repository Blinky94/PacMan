using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    /// <summary>
    /// Classe PacGom
    /// </summary>
    class PacGom
    {
        #region FIELDS

        protected Texture2D _dessin_pacgom;
        protected SpriteFont trace;
        private SoundEffect _WAKA_PAC;
        protected float timer_SP_1 = 0;
        protected float timer_SP_2 = 0;
        protected float timer_SP_3 = 0;
        protected float timer_SP_4 = 0;
        private int _point_pacgom = 0;

        #endregion

        #region PROPERTIES

        public int Point { get { return _point_pacgom; } set { _point_pacgom = value; } }
        public int NumberPacGom { get; set; }

        #endregion

        /// <summary>
        /// Methode Decompte le nombre de PacGom restants
        /// dans le niveau de jeu et renvoi
        /// un booleen a "true" s'il n'y en a plus
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>  
        public bool Count_PacGom(int count)
        {
            bool end_level = false;

            switch (count)
            {
                case 0: end_level = true; break;
                default: end_level = false; break;
            }

            return end_level;
        }

        /// <summary>
        /// Methode de chargement des SPRITES
        /// des PacGoms
        /// </summary>
        /// <param name="content"></param>
        /// <param name="textureName"></param>
        /// <param name="textureName2"></param>
        public void LoadContent(ContentManager content, string textureName, string Waka, Node[,] level_table2D)
        {
            trace = content.Load<SpriteFont>("fullscreen_font");

            NumberPacGom = 0;

            _dessin_pacgom = content.Load<Texture2D>(textureName);

            //Charge le son de PACMAN mangeant des PacGommes
            _WAKA_PAC = content.Load<SoundEffect>(Waka);

            foreach (var node in level_table2D)
            {
                if (node.IsChar == 'C')
                {
                    NumberPacGom++;
                }
            }
        }

        /// <summary>
        /// Méthode Update_PacGoms 
        /// pendant 10 secondes.
        /// Emploi la méthode Is_PacMan_Invincible qui renvoi un booléan si PACMAN est invincible
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="_PAC_POS_INIT"></param>
        /// <param name="level_table2D"></param>  
        public void Update_PacGoms(Vector2 _PAC_POS, Node[,] level_table2D, int point_gom)
        {
            //Lorsque PacMan mange des PacGommes
            foreach (Node tile in level_table2D)
            {
                if (_PAC_POS == tile.Position && tile.IsChar == 'C')
                {
                    //incrémente de 10 points si PacMan mange une PacGom
                    _point_pacgom = point_gom + 10;
                    NumberPacGom--;

                    _WAKA_PAC.Play();
                    tile.IsChar = 'p';
                    tile.ColoR = Color.Black;
                }
            }
        }

        /// <summary>
        /// Méthode publique d'affichage des PacGommes
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="level"></param>
        public void Draw_PacGoms(SpriteBatch spriteBatch, Node[,] level)
        {
            foreach (Node PacGom in level)
            {
                if (PacGom.IsChar == 'C')
                {
                    spriteBatch.Draw(_dessin_pacgom, new Vector2(PacGom.Position.X, PacGom.Position.Y), PacGom.ColoR);
                }

                else if (PacGom.IsChar == 'p')
                {
                    spriteBatch.Draw(_dessin_pacgom, new Vector2(PacGom.Position.X, PacGom.Position.Y), PacGom.ColoR);
                }
            }

            #region Tests

            //foreach (Level_Col obj in _level_table2D)
            //{
            //    string text1 = string.Format("(_PAC_POS == obj.Position && obj.IsChar == 'C') = {0}", (pacPos == obj.Position && obj.IsChar == 'C'));
            //    spriteBatch.DrawString(this.trace, text1, new Vector2(0, 100), Color.Red);
            //}

            #endregion
        }
    }
}