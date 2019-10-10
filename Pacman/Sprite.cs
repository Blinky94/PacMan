using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    /// <summary>
    /// Super classe qui gère le chargement des Sprites du jeu PacMan
    /// </summary>
    class Sprite
    {
        public enum Vect { Up, Down, Left, Right, nul };

        protected float nodeToRight, nodeToLeft, nodeToUp, nodeToDown, currentNode;
        protected bool isSameDirection = true;
        protected int indice_UD = 0;
        protected int indice_LR = 0;
        protected Vect _vector_start = Vect.nul;
        //private string file_name = "Traces.txt";
        private SpriteFont _PAC_Trace;

        //Propriétés de la méthode UpdateMoveAndPosition
        public int HashCodeDir { get; set; }
        public List<Vector2> NextNode { get; set; }
        public List<bool> ListNextIsWall { get; set; }
        public Vect VectDir { get; set; }

        //Propriétés de la méthode Draw
        public Vector2 SpritePos { get; set; }
        public Texture2D SpriteTexture { get; set; }
        public Color SpriteColor { get; set; }
        public Rectangle RecDestination { get; set; }
        public Rectangle RecSource { get; set; }
        public float[] LerpCoefArray { get; set; }


        /// <summary>
        /// Constructeur SuperClasse Sprite
        /// </summary>
        public Sprite()
        {

        }

        /// <summary>
        /// Constructeur SuperClasse Sprite
        /// </summary>
        /// <param name="SpritePos"></param>
        public Sprite(Vector2 spritePos)
        {
            this.SpritePos = spritePos;
        }

        /// <summary>
        /// 2nd constructeur SuperClasse Sprite
        /// </summary>
        /// <param name="SpritePos"></param>
        /// <param name="level"></param>
        public Sprite(Vector2 spritePos, int level, int lifeNum)
        {
            this.SpritePos = spritePos;
        }

        /// <summary>
        /// Initialize SuperClasse Sprite
        /// </summary>
        /// <param name="coefArray"></param>
        /// <param name="vect_sprite"></param>
        public virtual void Initialize(float[] tabcoef, Vect vect_sprite)
        {
            this.LerpCoefArray = tabcoef;
            this.VectDir = vect_sprite;
        }


        /// <summary>
        /// LoadContent SuperClasse Sprite
        /// </summary>
        /// <param name="content"></param>
        /// <param name="textureName"></param>
        /// <param name="PosSprite"></param>
        public virtual void LoadContent(ContentManager Content, string textureName, Vector2 SPRITE_POS_INIT, Node[,] node)
        {
            SpriteTexture = Content.Load<Texture2D>(textureName);

            _PAC_Trace = Content.Load<SpriteFont>("fullscreen_font");
        }

        /// <summary>
        /// LoadContent SuperClasse Sprite
        /// </summary>
        /// <param name="content"></param>
        /// <param name="textureName"></param>
        /// <param name="PosSprite"></param>
        public virtual void LoadContent(ContentManager Content, int Level, Vector2 SPRITE_POS_INIT, bool Is_Full_Screen, string level_fruit)
        {
            //Empty intentionnaly
        }

        /// <summary>
        /// Methode pour tracer dans un fichier les valeurs du Lerp suivant la direction choisie par le joueur
        /// </summary>
        /// <param name="_dir"></param>
        /// <param name="_value1"></param>
        /// <param name="_value2"></param>
        /// <param name="_coef"></param>
        /// <param name="_is_under_lerp"></param>
        /// <param name="_current_equal_ori"></param>
        //private void Trace_Lerp_File(string _dir, float _value1, float _value2, float _coef, bool _is_under_lerp, bool _current_equal_ori)
        //{
        //    string[] lines = { "Lerp " + _dir + " : " + (_value1 + (_value2 - _value1) * _coef) + " = " + _value1 + " + (" + _value2 + " - " + _value1 + ") X " + _coef + " entre ]0,1] : " + _is_under_lerp + " et meme sens ? : " + _current_equal_ori };

        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"G:\" + file_name, true))
        //    {
        //        foreach (string line in lines)
        //        {
        //            file.WriteLine(line);
        //        }
        //    }
        //}

        #region MANAGE LERP DIRECTIONS

        /// <summary>
        /// Méthode incrémente LERP Horizontal
        /// </summary>
        /// <returns></returns>
        protected int Lerp_Hor_Plus()
        {
            if (indice_LR < LerpCoefArray.Length - 1) { indice_LR++; } else { indice_LR = 0; }

            return indice_LR;
        }

        /// <summary>
        /// Méthode décrémente LERP Horizontal
        /// </summary>
        /// <returns></returns>
        protected int Lerp_Hor_Moins()
        {
            if (indice_LR > 0) { indice_LR--; } else { indice_LR = LerpCoefArray.Length - 1; }

            return indice_LR;
        }

        /// <summary>
        /// Méthode incrémente LERP Vertical
        /// </summary>
        /// <returns></returns>
        protected int Lerp_Ver_Plus()
        {
            if (indice_UD < LerpCoefArray.Length - 1) { indice_UD++; } else { indice_UD = 0; }

            return indice_UD;
        }

        /// <summary>
        /// Méthode décrémente LERP Vertical
        /// </summary>
        /// <returns></returns>
        protected int Lerp_Ver_Moins()
        {
            if (indice_UD > 0) { indice_UD--; } else { indice_UD = LerpCoefArray.Length - 1; }

            return indice_UD;
        }

        /// <summary>
        /// Méthode qui détermine si le vecteur d'origine est le même que le vecteur en cours dans le LERP
        /// </summary>
        /// <param name="Vector_Start"></param>
        /// <param name="cur_vect_state"></param>
        /// <returns></returns>
        protected bool IsSameDirectionLikeOrigin(Vect _vector_Start, Vect _cur_vect_state)
        {
            return (_vector_Start == _cur_vect_state);
        }

        /// <summary>
        /// Méthode qui détermine si PacMan est dans le Lerp ]0,1] 
        /// </summary>
        /// <param name="indice_Lerp"></param>
        /// <returns></returns>
        protected bool IsInTheWay(int indice_Lerp)
        {
            if (LerpCoefArray.Length <= indice_Lerp) { indice_Lerp = 0; }
            return (LerpCoefArray[indice_Lerp] > 0 && LerpCoefArray[indice_Lerp] <= 1);
        }

        /// <summary>
        /// Méthode qui détermine et retourne le vecteur de départ en 0 (Point de départ dans le LERP du sprite)
        /// </summary>
        /// <param name="indice_Lerp"></param>
        /// <param name="cur_vect_state"></param>
        /// <returns></returns>
        protected Vect Vector_Start(int indice_Lerp, Vect cur_vect_state)
        {
            if (LerpCoefArray.Length <= indice_Lerp) { indice_Lerp = 0; }

            if (LerpCoefArray[indice_Lerp] == 0)
            {
                _vector_start = cur_vect_state;
            }

            return _vector_start;
        }

        #endregion


        /// <summary>
        /// UpdateMoveAndPosition SuperClasse Sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spritePos"></param>
        public virtual void UpdateMoveAndPosition(GameTime gameTime, Vector2 SPRITE_POS)
        {
            SpritePos = SPRITE_POS;
            VectDir = Vect.Right;
        }

        /// <summary>
        /// Méthode private GenLerpArray
        /// Méthode interne qui génère la listCoefLerp 
        /// en fonction de la séléction de la vitesse
        /// </summary>
        /// <param name="table_length"></param>
        /// <param name="coef"></param>
        /// <returns></returns>
        private float[] GenLerpArray(int table_length, float coef)
        {
            float[] table_coef = new float[table_length];

            for (int i = 0; i < table_coef.Length; i++)
            {
                table_coef[i] += coef * i;
            }
            return table_coef;
        }


        /// <summary>
        /// Méthode Speed
        /// Détermine la vitesse d'un sprite
        /// 7 etats : hardlySlow,VerySlow, slow, Static, quicky, veryQuicky, Panic
        /// </summary>
        /// <param name="selected_speed"></param>
        /// <returns></returns>
        public float[] Speed(EnumSpeed selected_speed)
        {
            float[] arrayLerp = new float[0];

            switch (selected_speed.ToString())
            {
                case "hardlySlow": arrayLerp = GenLerpArray(101, 0.01f); break;
                case "slow": arrayLerp = GenLerpArray(26, 0.04f); break;
                case "VerySlow": arrayLerp = GenLerpArray(51, 0.02f); break;
                case "quicky": arrayLerp = GenLerpArray(21, 0.05f); break;
                case "veryQuicky": arrayLerp = GenLerpArray(11, 0.1f); break;
                case "Panic": arrayLerp = GenLerpArray(6, 0.2f); break;
                default: arrayLerp = GenLerpArray(1, 1f); break;
            }

            return arrayLerp;
        }


        /// <summary>
        /// UpdateMoveAndPosition SuperClasse Sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="_hashCodeDir"></param>
        /// <param name="listNextNodes"></param>
        /// <param name="spritePos"></param>
        /// <param name="_listNextNodeIsWall"></param>
        /// <param name="leftCoridor"></param>
        /// <param name="rightCoridor"></param>
        /// <param name="cur_sprite_vect"></param>
        public virtual void UpdateMoveAndPosition(GameTime gameTime, int _hashCodeDir, List<Vector2> Tile_Successor, Vector2 SPRITE_POS,
            List<bool> List_Succ_IsWall, Vector2 Couloir_Gauche_Pos, Vector2 Couloir_Droite_Pos, Vector2 Couloir_Haut_Pos, Vector2 Couloir_Bas_Pos,
            float[] table_coef_sprite)
        {
            HashCodeDir = _hashCodeDir;
            NextNode = Tile_Successor;
            SpritePos = SPRITE_POS;
            ListNextIsWall = List_Succ_IsWall;
            LerpCoefArray = table_coef_sprite;

            // recSprite = new Rectangle((int)posSprite.X + (15 / 2), (int)posSprite.Y + (15 / 2), 1, 1);

            //EN HAUT                  
            if (HashCodeDir == 64 && !ListNextIsWall[0] || VectDir == Vect.Up && !ListNextIsWall[0])
            {
                //Reintitialise la valeur de l'indice indice_LR à 0 s'il change de sens
                if (indice_LR != 0 || indice_LR != 1) { indice_LR = 0; }

                VectDir = Vect.Up;
                currentNode = NextNode[4].Y;
                nodeToUp = NextNode[0].Y;

                //Test si le Sprite va dans le même sens qu'au départ de l'interpolation (0)
                isSameDirection = IsSameDirectionLikeOrigin(Vector_Start(indice_UD, VectDir), VectDir);


                if (SpritePos != Couloir_Haut_Pos)
                {
                    //Si le sprite est dans le Lerp et dans le sens inverse
                    if (IsInTheWay(indice_UD) == true && isSameDirection == false)
                    {
                        Lerp_Ver_Moins();

                        //Calcul du LERP dans le sens de bas vers le haut
                        SpritePos = new Vector2(NextNode[0].X, (currentNode + (nodeToDown - currentNode) * LerpCoefArray[indice_UD]));

                        //Trace_Lerp_File(VectDir.ToString(), currentNode, nodeToDown, LerpCoefArray[indice_UD], IsInTheWay(indice_UD), (isSameDirection));
                    }

                    else
                    {
                        Lerp_Ver_Plus();

                        //Calcul du LERP dans le sens de bas vers le haut
                        SpritePos = new Vector2(NextNode[0].X, (currentNode + (nodeToUp - currentNode) * LerpCoefArray[indice_UD]));

                        //Trace_Lerp_File(VectDir.ToString(), currentNode, nodeToUp, LerpCoefArray[indice_UD], IsInTheWay(indice_UD), (isSameDirection));
                    }
                }

                else
                {
                    SpritePos = new Vector2(Couloir_Bas_Pos.X, Couloir_Bas_Pos.Y);
                }
            }

            //EN BAS             
            if (HashCodeDir == 256 && !ListNextIsWall[2] || VectDir == Vect.Down && !ListNextIsWall[2])
            {
                //Reintitialise la valeur de l'indice indice_LR à 0 s'il change de sens
                if (indice_LR != 0 || indice_LR != 1) { indice_LR = 0; }

                VectDir = Vect.Down;
                currentNode = NextNode[4].Y;
                nodeToDown = NextNode[2].Y;

                //Test si le Sprite va dans le même sens qu'au départ de l'interpolation (0)
                isSameDirection = IsSameDirectionLikeOrigin(Vector_Start(indice_UD, VectDir), VectDir);

                if (SpritePos != Couloir_Bas_Pos)
                {
                    //Si le sprite est dans le Lerp et dans le sens inverse
                    if (IsInTheWay(indice_UD) == true && isSameDirection == false)
                    {
                        Lerp_Ver_Moins();

                        //Calcul du LERP dans le sens de bas vers le haut
                        SpritePos = new Vector2(NextNode[2].X, (currentNode + (nodeToUp - currentNode) * LerpCoefArray[indice_UD]));

                        //Trace_Lerp_File(VectDir.ToString(), currentNode, nodeToUp, LerpCoefArray[indice_UD], IsInTheWay(indice_UD), (isSameDirection));
                    }

                    else
                    {
                        Lerp_Ver_Plus();

                        //Calcul du LERP dans le sens de bas vers le haut
                        SpritePos = new Vector2(NextNode[2].X, (currentNode + (nodeToDown - currentNode) * LerpCoefArray[indice_UD]));

                        //Trace_Lerp_File(VectDir.ToString(), currentNode, nodeToDown, LerpCoefArray[indice_UD], IsInTheWay(indice_UD), (isSameDirection));
                    }
                }

                else
                {
                    SpritePos = new Vector2(Couloir_Haut_Pos.X, Couloir_Haut_Pos.Y);
                }
            }

            //A DROITE                             
            if (HashCodeDir == 128 && !ListNextIsWall[1] || VectDir == Vect.Right && !ListNextIsWall[1])
            {
                //Reintitialise la valeur de l'indice indice_UD à 0 s'il change de sens
                if (indice_UD != 0 || indice_UD != 1) { indice_UD = 0; }

                VectDir = Vect.Right;
                currentNode = NextNode[4].X;
                nodeToRight = NextNode[1].X;

                //Test si le Sprite va dans le même sens qu'au départ de l'interpolation (0)
                isSameDirection = IsSameDirectionLikeOrigin(Vector_Start(indice_LR, VectDir), VectDir);

                if (SpritePos != Couloir_Droite_Pos)
                {
                    //Si le sprite est dans le Lerp et dans le sens inverse
                    if (IsInTheWay(indice_LR) == true && isSameDirection == false)
                    {
                        Lerp_Hor_Moins();
                        //Calcul du LERP au départ dans le sens DROITE
                        SpritePos = new Vector2((currentNode + (nodeToLeft - currentNode) * LerpCoefArray[indice_LR]), NextNode[1].Y);

                        //Trace_Lerp_File(VectDir.ToString(), currentNode, nodeToLeft, LerpCoefArray[indice_LR], IsInTheWay(indice_LR), (isSameDirection));                       
                    }

                    else
                    {
                        Lerp_Hor_Plus();
                        //Calcul du LERP au départ dans le sens DROITE
                        SpritePos = new Vector2((currentNode + (nodeToRight - currentNode) * LerpCoefArray[indice_LR]), NextNode[1].Y);

                        //Trace_Lerp_File(VectDir.ToString(), currentNode, nodeToRight, LerpCoefArray[indice_LR], IsInTheWay(indice_LR), (isSameDirection));
                    }
                }

                else
                {
                    SpritePos = new Vector2(Couloir_Gauche_Pos.X, Couloir_Gauche_Pos.Y);
                }
            }

            //A GAUCHE                          
            if (HashCodeDir == 32 && !ListNextIsWall[3] || VectDir == Vect.Left && !ListNextIsWall[3])
            {
                //Reintitialise la valeur de l'indice indice_UD à 0 s'il change de sens
                if (indice_UD != 0 || indice_UD != 1) { indice_UD = 0; }

                VectDir = Vect.Left;
                currentNode = NextNode[4].X;
                nodeToLeft = NextNode[3].X;

                //Test si le Sprite va dans le même sens qu'au départ de l'interpolation (0)
                isSameDirection = IsSameDirectionLikeOrigin(Vector_Start(indice_LR, VectDir), VectDir);

                if (SpritePos != Couloir_Gauche_Pos)
                {
                    //Si le sprite est dans le Lerp et dans le sens inverse
                    if (IsInTheWay(indice_LR) == true && isSameDirection == false)
                    {
                        Lerp_Hor_Moins();

                        //Calcul du LERP au départ dans le sens GAUCHE
                        SpritePos = new Vector2((currentNode + (nodeToRight - currentNode) * LerpCoefArray[indice_LR]), NextNode[3].Y);

                        //Trace_Lerp_File(VectDir.ToString(), currentNode, nodeToRight, LerpCoefArray[indice_LR], IsInTheWay(indice_LR), (isSameDirection));
                    }

                    else
                    {
                        Lerp_Hor_Plus();

                        //Calcul du LERP au départ dans le sens GAUCHE
                        SpritePos = new Vector2((currentNode + (nodeToLeft - currentNode) * LerpCoefArray[indice_LR]), NextNode[3].Y);

                        //Trace_Lerp_File(VectDir.ToString(), currentNode, nodeToLeft, LerpCoefArray[indice_LR], IsInTheWay(indice_LR), (isSameDirection));
                    }
                }

                else
                {
                    SpritePos = new Vector2(Couloir_Droite_Pos.X, Couloir_Droite_Pos.Y);
                }
            }
        }

        /// <summary>
        /// Méthode Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="sprite_texture"></param>
        /// <param name="dest_rec"></param>
        /// <param name="source_rec"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="effects"></param>
        /// <param name="layer"></param>
        public virtual void Draw(SpriteBatch spriteBatch, Texture2D sprite_texture, Rectangle dest_rec, Rectangle source_rec, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layer)
        {
            SpriteColor = color;
            RecDestination = dest_rec;
            RecSource = source_rec;
            SpriteTexture = sprite_texture;

            // texture, destination rectangle, source rectangle, color, rotation, origin, effects and layer                    

            spriteBatch.Draw(SpriteTexture, RecDestination, RecSource, SpriteColor, rotation, origin, effects, layer);

            #region //trace DEBUGGING

            // string text0 = String.Format("CurPacVec :" + VectDir);
            // spriteBatch.DrawString(_PAC_Trace, text0, new Vector2(220, 500), Color.Yellow);

            // string text0 = String.Format("Vector_Start(indice_LR, VectDir):" + Vector_Start(indice_LR, VectDir));
            // spriteBatch.DrawString(_PAC_Trace, text0, new Vector2(2, 500), Color.Yellow);

            /* HashCodeDir
       
             NextNode 
             SpritePos 
             ListNextIsWall */

            //string text1 = String.Format("VectDir ? " + VectDir);
            //spriteBatch.DrawString(_PAC_Trace, text1, new Vector2(2, 10), Color.Yellow);

            //string text1 = String.Format("_Is_indice_reverse ? " + _Is_indice_reverse);
            //spriteBatch.DrawString(_PAC_Trace, text1, new Vector2(2, 500), Color.Yellow);

            //string text0 = String.Format("isSameDirection :" + isSameDirection);
            //spriteBatch.DrawString(_PAC_Trace, text0, new Vector2(0, 500), Color.Yellow);

            //string text1 = String.Format("Vector_Start(indice_UD, VectDir)" + Vector_Start(indice_UD, VectDir));
            //spriteBatch.DrawString(_PAC_Trace, text1, new Vector2(0, 510), Color.Yellow);

            //string text2 = String.Format("VectDir? : " + HashCodeDir);
            //spriteBatch.DrawString(_PAC_Trace, text2, new Vector2(2, 520), Color.Gold);

            //string text3 = String.Format("Coef LR  : " + LerpCoefArray[indice_LR]);
            //spriteBatch.DrawString(_PAC_Trace, text3, new Vector2(2, 530), Color.Red);

            //string text4 = String.Format("Coef UD  : " + LerpCoefArray[indice_UD]);
            // spriteBatch.DrawString(_PAC_Trace, text4, new Vector2(2, 540), Color.Red);
            /*
            string text0 = String.Format("isSameDirection :" + isSameDirection);
            spriteBatch.DrawString(_PAC_Trace, text0, new Vector2(0, 500), Color.Yellow);

            string text1 = String.Format("Vector_Start(indice_UD, VectDir)" + Vector_Start(indice_UD, _cur_PacMan_Vect));
            spriteBatch.DrawString(_PAC_Trace, text1, new Vector2(0, 510), Color.Yellow);

            string text2 = String.Format("VectDir? : " + _cur_PacMan_Vect);
            spriteBatch.DrawString(_PAC_Trace, text2, new Vector2(2, 520), Color.Gold);

            string text3 = String.Format("Coef LR  : " + tab_coef[indice_LR]);
            spriteBatch.DrawString(_PAC_Trace, text3, new Vector2(2, 530), Color.Red);

            string text4 = String.Format("Coef UD  : " + tab_coef[indice_UD]);
            spriteBatch.DrawString(_PAC_Trace, text4, new Vector2(2, 540), Color.Red);

            // string text4 = String.Format("valeur de listNextNodes : " + listNextNodes[1].X);
            //spriteBatch.DrawString(_PAC_Trace, text4, new Vector2(2, 530), Color.Red);

            // string text5 = String.Format("PACMAN CHANGE DE DIRECTION : " + _is_change_dir);
            // spriteBatch.DrawString(_PAC_Trace, text5, new Vector2(2, 510), Color.Red);

            //string text5 = String.Format("ancienne valeur de listNextNodes : " + _old_Tile_Successor[1].X);
            //spriteBatch.DrawString(_PAC_Trace, text5, new Vector2(2, 510), Color.Red); 
            */
            #endregion
        }
    }
}