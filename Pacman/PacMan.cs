using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    public enum Direction { Up, Down, Left, Right };       //Classe d'enumeration des direction de PacMan

    /// <summary>
    /// Classe héritée de la SuperClasse Sprite
    /// </summary>
    class PacMan : Sprite
    {
        private int _frameColumn, _frameline;
        private int maxTimerOpenClose = 7;
        private int _timer;
        private int maxTimerAlive = 5;
        internal int countDeadStates = 0;
        private int timerOpenClose = 0;
        private int timerDead = 0;
        private int maxTimerDead = 8;
        private int _frameColumn_dead;
        private bool StopGame = false;
        private SoundEffect sndDeadPac;
        private SoundEffectInstance instanceSndDeadPac;
        private Rectangle recPacAlive, recPacDead, recSourceAlive, recSourceDead;
        private Node[,] Maze2D;
        private SpriteFont _PAC_Trace;
        //Tableau de coef pour le calcul du LERP
        private float[] tab_coef = new float[11];
        //private float[] tab_coef = new float[21];
        private Texture2D textureDeadPac, texturePac;

        public PacMan() { }

        /// <summary>
        /// Constructeur de la classe PacMan
        /// </summary>
        public PacMan(Vector2 position_initiale) : base(new Vector2(0, 0)) { }

        /// <summary>
        /// Methode qui anime PacMan suivant la direction definie par un entier
        /// </summary>
        /// <param name="directionStr"></param>
        private void PacManAnimation(string directionStr)
        {
            switch (directionStr)
            {
                case "Down": _timer++;

                    if (this._timer == this.maxTimerAlive)
                    {
                        this._timer = 0;
                        VectDir = Vect.Down;
                    } break;

                case "Left": _timer++;

                    if (this._timer == this.maxTimerAlive)
                    {
                        this._timer = 0;
                        VectDir = Vect.Left;
                    } break;

                case "Up": _timer++;

                    if (this._timer == this.maxTimerAlive)
                    {
                        this._timer = 0;
                        VectDir = Vect.Up;
                    } break;

                case "Right": _timer++;

                    if (this._timer == this.maxTimerAlive)
                    {
                        this._timer = 0;
                        VectDir = Vect.Right;
                    } break;
            }
        }

        /// <summary>
        /// Méthode d'initialisation des variables locale de la classe
        /// </summary>
        public override void Initialize(float[] tabcoef, Vect vect_pac)
        {
            VectDir = vect_pac;
            _frameColumn = 0;
            _frameColumn_dead = 0;
            _frameline = 0;
            _timer = 0;
            LerpCoefArray = tabcoef;

            base.Initialize(LerpCoefArray, VectDir);
        }


        /// <summary>
        /// Méthode de chargement des textures
        /// positionnement initial de PacMan dans le labyrinthe
        /// </summary>
        /// <param name="content"></param>
        /// <param name="textureName"></param>
        /// <param name="pacPos"></param>
        public override void LoadContent(ContentManager content, string textureName, Vector2 pacPos, Node[,] node2D)
        {
            string refSndDeadPac = "pacman_death";
            sndDeadPac = content.Load<SoundEffect>(refSndDeadPac);
            instanceSndDeadPac = sndDeadPac.CreateInstance();
            instanceSndDeadPac.IsLooped = false;
            instanceSndDeadPac.Play();
            instanceSndDeadPac.Pause();
            texturePac = content.Load<Texture2D>(textureName);
            string refTextDeadPac = "New_Paco_Dead";
            textureDeadPac = content.Load<Texture2D>(refTextDeadPac);
            _PAC_Trace = content.Load<SpriteFont>("fullscreen_font");
            SpritePos = pacPos;
            Maze2D = node2D;

            base.LoadContent(content, textureName, SpritePos, node2D);
        }


        /// <summary>
        /// Public UpdateMoveAndPosition de la classe PacMan
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="hashCode"></param>
        /// <param name="listNextNodes"></param>
        /// <param name="pacPos"></param>
        /// <param name="ListNextIsWall"></param>
        /// <param name="leftCoridor"></param>
        /// <param name="rightCoridor"></param>
        public override void UpdateMoveAndPosition(GameTime gameTime, int hashCodeDirPac, List<Vector2> listNextNodes, Vector2 pacPos, List<bool> listNextNodeIsWall, Vector2 leftCoridor, Vector2 rightCoridor, Vector2 upCoridor, Vector2 downCoridor, float[] lerpCoefArray)
        {
            HashCodeDir = hashCodeDirPac;
            NextNode = listNextNodes;
            SpritePos = pacPos;
            ListNextIsWall = listNextNodeIsWall;
            LerpCoefArray = lerpCoefArray;
            PacManAnimation(VectDir.ToString());

            base.UpdateMoveAndPosition(gameTime, HashCodeDir, NextNode, SpritePos, ListNextIsWall, leftCoridor, rightCoridor, upCoridor, downCoridor, LerpCoefArray);
        }

        /// <summary>
        /// Méthode OpenCloseMouth
        /// Animation de la bouche de PacMan
        /// </summary>
        private void OpenCloseMouth()
        {
            switch (_frameline)
            {
                case 0:
                    timerOpenClose++;
                    if (timerOpenClose == maxTimerOpenClose)
                    {
                        timerOpenClose = 0;
                        _frameline = 1;
                        break;
                    } break;

                case 1:
                    timerOpenClose++;
                    if (timerOpenClose == maxTimerOpenClose)
                    {
                        timerOpenClose = 0;
                        _frameline = 0;
                        break;
                    } break;
            }
        }


        /// <summary>
        /// Méthode DrawNormal
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawNormal(SpriteBatch spriteBatch)
        {
            OpenCloseMouth();

            //Dessin des "frames" suivant les directions de PacMan
            switch (VectDir.ToString())
            {
                case "Up": this._frameColumn = 3; break;
                case "Down": this._frameColumn = 1; break;
                case "Left": this._frameColumn = 0; break;
                case "Right": this._frameColumn = 2; break;
            }

            SpriteTexture = texturePac;

            recPacAlive = new Rectangle((int)SpritePos.X - 3, (int)SpritePos.Y - 3, 30 - 7, 30 - 7);
            recSourceAlive = new Rectangle((_frameColumn) * 30, (this._frameline) * 30, 30, 30);

            base.Draw(spriteBatch, SpriteTexture, recPacAlive, recSourceAlive, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Méthode DrawDead
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawDead(SpriteBatch spriteBatch, bool IsGameOver)
        {
            if (IsGameOver == false && StopGame == false)
            {
                instanceSndDeadPac.Play();

                if (timerDead < maxTimerDead)
                {
                    timerDead++;
                }
                else
                {
                    if (countDeadStates <= 8)
                    {
                        _frameColumn_dead = countDeadStates;
                        countDeadStates++;
                    }
                    else
                    {
                        countDeadStates = 0;
                        instanceSndDeadPac.Pause();
                    }
                    timerDead = 0;
                }
            }
            else if (IsGameOver == true && StopGame == false)
            {
                instanceSndDeadPac.Play();

                if (timerDead < maxTimerDead)
                {
                    timerDead++;
                }

                else
                {
                    if (countDeadStates <= 8)
                    {
                        _frameColumn_dead = countDeadStates;
                        countDeadStates++;
                    }
                    else
                    {
                        countDeadStates = 0;
                        instanceSndDeadPac.Pause();
                        StopGame = true;
                    }
                    timerDead = 0;
                }
            }


            SpriteTexture = textureDeadPac;

            recPacDead = new Rectangle((int)SpritePos.X - 3, (int)SpritePos.Y - 3, 30 - 7, 30 - 7);
            recSourceDead = new Rectangle((this._frameColumn_dead) * 30, 0, 30, 30);

            base.Draw(spriteBatch, SpriteTexture, recPacDead, recSourceDead, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            #region //trace DEBUGGING

            //_frameline = 0;           

            // /* HashCodeDir

            //  NextNode 
            //  SpritePos 
            //  ListNextIsWall */

            //      string text1 = String.Format("_frameline " + _frameline);
            //spriteBatch.DrawString(_PAC_Trace, text1, new Vector2(2, 10), Color.Yellow);

            //string text2 = String.Format("IsPacManEaten ? " + IsPacManEaten);
            //  spriteBatch.DrawString(_PAC_Trace, text2, new Vector2(2, 30), Color.Yellow);

            //string text2 = String.Format("NextNode? : " + NextNode);
            //spriteBatch.DrawString(_PAC_Trace, text2, new Vector2(2, 530), Color.Gold);

            // string text3 = String.Format("SpritePos  : " + SpritePos.X);
            // spriteBatch.DrawString(_PAC_Trace, text3, new Vector2(2, 520), Color.Red);

            //string text4 = String.Format("valeur ListNextIsWall : " + ListNextIsWall[1]);
            //spriteBatch.DrawString(_PAC_Trace, text4, new Vector2(2, 530), Color.Red);

            // string text4 = String.Format("valeur de listNextNodes : " + listNextNodes[1].X);
            //spriteBatch.DrawString(_PAC_Trace, text4, new Vector2(2, 530), Color.Red);

            // string text5 = String.Format("PACMAN CHANGE DE DIRECTION : " + _is_change_dir);
            // spriteBatch.DrawString(_PAC_Trace, text5, new Vector2(2, 510), Color.Red);

            //string text5 = String.Format("ancienne valeur de listNextNodes : " + _old_Tile_Successor[1].X);
            //spriteBatch.DrawString(_PAC_Trace, text5, new Vector2(2, 510), Color.Red); 

            #endregion
        }
    }
}