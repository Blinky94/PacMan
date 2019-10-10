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
    public enum Dir_Ghost { Up, Down, Left, Right };

    /// <summary>
    /// Classe héritée de la SuperClasse Sprite
    /// </summary>
    class Ghost : Sprite
    {
        #region FIELDS

        private Dir_Ghost enumHashCode;               //Variable enum direction fantome        
        private int _line = 0;
        private Node[,] Maze2D;
        //Animation des phantomes
        private int frameToDraw, _frameLine;
        private int _timer_anim = 0;
        private int _incr_anim_ghost = 20;
        //Variables de sons
        private int countSoundPlayed = 0;
        private SoundEffect sndGhostEat;
        private SoundEffectInstance instSndGhostEat;

        //Traçage et debug
        private SpriteFont _PAC_Trace;

        #endregion


        #region CONSTRUCTOR

        public Ghost() { }

        /// <summary>
        /// Constructeur de la classe PacMan
        /// </summary>
        public Ghost(Vector2 position_initiale) : base(new Vector2(0, 0)) { }

        #endregion

        /// <summary>
        /// Méthode JupeGhostAnimation
        /// Alterne la ligne d'animation de la frame
        /// </summary>
        private void JupeGhostAnimation()
        {
            switch (_line)
            {
                case 0:
                    _timer_anim++;
                    if (_timer_anim == _incr_anim_ghost)
                    {
                        _timer_anim = 0;
                        _line = 1;
                        break;
                    }
                    ; break;

                case 1:
                    _timer_anim++;
                    if (_timer_anim == _incr_anim_ghost)
                    {
                        _timer_anim = 0;
                        _line = 0;
                        break;
                    }
                    ; break;

                default: ; break;
            }
        }

        /// <summary>
        /// Méthode d'initialisation des variables locale de la classe
        /// </summary>
        public override void Initialize(float[] coefArray, Vect vect_ghost)
        {
            VectDir = vect_ghost;
            //enumHashCode = Dir_Ghost.Down ;
            frameToDraw = 0;
            _frameLine = 0;
            _timer_anim = 0;

            LerpCoefArray = coefArray;

            base.Initialize(LerpCoefArray, VectDir);
        }

        /// <summary>
        /// Méthode de chargement des textures
        /// positionnement initial de PacMan dans le labyrinthe
        /// </summary>
        /// <param name="content"></param>
        /// <param name="textureName"></param>
        /// <param name="pacPos"></param>
        public override void LoadContent(ContentManager Content, string textureName, Vector2 ghostPos, Node[,] Node2D)
        {
            //Charge le son lorsque PacMan mange un fantôme
            string refString = "pacman_eatghost";
            sndGhostEat = Content.Load<SoundEffect>(refString);
            instSndGhostEat = sndGhostEat.CreateInstance();
            instSndGhostEat.IsLooped = false;
            instSndGhostEat.Play();
            instSndGhostEat.Pause();

            SpriteTexture = Content.Load<Texture2D>(textureName);

            _PAC_Trace = Content.Load<SpriteFont>("fullscreen_font");

            SpritePos = ghostPos;
            Maze2D = Node2D;

            base.LoadContent(Content, textureName, SpritePos, Node2D);
        }

        /// <summary>
        /// Méthode simple UpdateMoveAndPosition
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="ghostPos"></param>
        public override void UpdateMoveAndPosition(GameTime gameTime, Vector2 GHOST_POS)
        {
            base.UpdateMoveAndPosition(gameTime, SpritePos);
        }

        /// <summary>
        /// Méthode Update_Alea
        /// Permet de donner un déplacement aléatoire au fantôme
        /// dans le labyrinthe
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="hashCode"></param>
        /// <param name="listNextNodes"></param>
        /// <param name="ghostPos"></param>
        /// <param name="pacPos"></param>
        /// <param name="ListNextIsWall"></param>
        /// <param name="leftCoridor"></param>
        /// <param name="rightCoridor"></param>
        public override void UpdateMoveAndPosition(GameTime gameTime, int hashCode, List<Vector2> listNextNodes, Vector2 ghostPos, List<bool> _listNextIsWall,
            Vector2 leftCoridor, Vector2 rightCoridor, Vector2 upCoridor, Vector2 downCoridor, float[] listCoefLerp)
        {
            HashCodeDir = hashCode;
            NextNode = listNextNodes;
            SpritePos = ghostPos;
            LerpCoefArray = listCoefLerp;

            //Gestion du dessin des yeux du fantôme suivant le HashCodeDir
            switch (HashCodeDir)
            {
                case 32: enumHashCode = Dir_Ghost.Left; break;
                case 64: enumHashCode = Dir_Ghost.Up; break;
                case 128: enumHashCode = Dir_Ghost.Right; break;
                case 256: enumHashCode = Dir_Ghost.Down; break;
            }

            base.UpdateMoveAndPosition(gameTime, HashCodeDir, NextNode, SpritePos, _listNextIsWall, leftCoridor, rightCoridor, upCoridor, downCoridor, listCoefLerp);
        }


        /// <summary>
        /// Méthode DrawNormal
        /// </summary>
        /// <param name="spritebatch"></param>
        public void DrawNormal(SpriteBatch spritebatch)
        {
            //Dessin des "frames" suivant les directions de PacMan              
            switch (enumHashCode.ToString())
            {
                //Affiche le fantome suivant la direction et Paire                   
                case "Up": this.frameToDraw = 2; break;     //Si direction en haut : dessin 4                   
                case "Down": this.frameToDraw = 1; break;     //Si direction en bas : dessin 2                   
                case "Left": this.frameToDraw = 0; break;     //Si direction à gauche : dessin 1                   
                case "Right": this.frameToDraw = 3; break;     //Si direction à droite : dessin 3     
            }

            //Lance l'animation des "jupes" des fantômes
            JupeGhostAnimation();

            //Dessin du fantôme
            SpriteColor = Color.White;
            RecDestination = new Rectangle((int)SpritePos.X - 3, (int)SpritePos.Y - 3, 30 - 7, 30 - 7);
            RecSource = new Rectangle((this.frameToDraw) * 30, (this._frameLine + _line) * 30, 30, 30);

            base.Draw(spritebatch, SpriteTexture, RecDestination, RecSource, SpriteColor, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

        }


        /// <summary>
        /// Méthode DrawFear
        /// </summary>
        /// <param name="spritebatch"></param>
        public void DrawFear(SpriteBatch spritebatch)
        {
            frameToDraw = 4;
            countSoundPlayed = 0;

            //Lance l'animation des "jupes" des fantômes
            JupeGhostAnimation();

            //Dessin du fantôme
            SpriteColor = Color.White;
            RecDestination = new Rectangle((int)SpritePos.X - 3, (int)SpritePos.Y - 3, 30 - 7, 30 - 7);
            RecSource = new Rectangle((this.frameToDraw) * 30, (this._frameLine + _line) * 30, 30, 30);

            base.Draw(spritebatch, SpriteTexture, RecDestination, RecSource, SpriteColor, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Méthode DrawEaten
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawEaten(SpriteBatch spriteBatch)
        {
            frameToDraw = 5;

            if (countSoundPlayed < 35)
            {
                instSndGhostEat.Play();
                countSoundPlayed++;
            }
            else
            {
                instSndGhostEat.Pause();
            }

            JupeGhostAnimation();

            //Dessin du fantôme
            SpriteColor = Color.White;
            RecDestination = new Rectangle((int)SpritePos.X - 3, (int)SpritePos.Y - 3, 30 - 7, 30 - 7);
            RecSource = new Rectangle((this.frameToDraw) * 30, (this._frameLine + _line) * 30, 30, 30);

            base.Draw(spriteBatch, SpriteTexture, RecDestination, RecSource, SpriteColor, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
        }
    }
}