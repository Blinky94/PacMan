using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.Xml.Serialization;

namespace PacMan
{
    /// <summary>
    /// Classe pour l'affichage des scores
    /// et du comptage des points
    /// </summary>
    class Score
    {
        #region FIELDS

        private SpriteFont _fullscreen_font, _windowed_font;
        private List<int> list_char;
        private XmlDocument doc;
        private string table_str = "";
        private Color _color_score = new Color(64, 68, 235);
        //Affichage des points
        private char high_score_1, high_score_10, high_score_100, high_score_1000, high_score_10000, high_score_100000;
        private int bestScore = 0;
        private string score_saved = "";
        //Affichage des textes des scores
        Texture2D _game_score_display, _high_score_display, _level_display;
        private string text_score = string.Format("GAME");
        private string text_score2 = string.Format("SCORE");
        private string text_score3 = string.Format("HIGH");
        private string text_GameOver = string.Format("GAME OVER");
        private string text_Ready = string.Format("READY");
        //timerOpeningSong GAME OVER
        private int timer_over = 0;
        private int end_timer_over = 50;
        private Color color_Over = Color.Red;
        private Color color_Ready = Color.Yellow;
        private string num_level;
        private string number_score100000;
        private string number_score10000;
        private string number_score1000;
        private string number_score100;
        private string number_score10;
        private string number_score1;
        private string high_score100000;
        private string high_score10000;
        private string high_score1000;
        private string high_score100;
        private string high_score10;
        private string high_score1;
        //Variables globales pour le score
        private int score_1 = 0, score_10 = 0, score_100 = 0, score_1000 = 0, score_10000 = 0, score_100000 = 0;
        string scoreSaved = @"D:\GameDevelop\Pacman\Pacman\ScoreSaved\ScoreSaved.xml";

        #endregion

        #region METHODS

        /// <summary>
        /// Méthode Load_Score
        /// Permet de charger le score à l'écran
        /// </summary>
        /// <param name="content"></param>
        public void Load_Score(ContentManager content)
        {
            //fichier XML qui contient le meilleurs score
            score_saved = scoreSaved;

            _fullscreen_font = content.Load<SpriteFont>("fullscreen_font"); //FullScreen
            _windowed_font = content.Load<SpriteFont>("windowed_font"); //Windowed

            //Charge la texture pour l'affichage des cadres des scores (game_score,high_score)
            _game_score_display = content.Load<Texture2D>("game_score");
            _high_score_display = content.Load<Texture2D>("high_score");
            _level_display = content.Load<Texture2D>("Level");

            //Charge le dernier meilleurs "HIGH SCORE" enregistré dans le fichier "score_saved"

            //Créé l'objet List<char>
            list_char = new List<int> { };

            //Créé l'objet XmlDocument
            doc = new XmlDocument();

            //Charge le fichier maze_fic
            doc.Load(score_saved);

            //Parcours le fichier XML et récupère la valeur texte des noeuds pour stocker dans une chaine de caractères
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                table_str += node.InnerText;
            }

            int lengthTable = table_str.Length;

            bestScore = Convert.ToInt32(table_str);

            //Ajoute caractère par caractère dans une liste de caractères
            foreach (int car in table_str)
            {
                list_char.Add(car);
            }

            list_char.Reverse();
            switch (lengthTable)
            {
                case 1: high_score_1 = Convert.ToChar(list_char[0]);
                    high_score_10 = '0';
                    high_score_100 = '0';
                    high_score_1000 = '0';
                    high_score_10000 = '0';
                    high_score_100000 = '0';
                    break;
                case 2: high_score_1 = Convert.ToChar(list_char[0]);
                    high_score_10 = Convert.ToChar(list_char[1]);
                    high_score_100 = '0';
                    high_score_1000 = '0';
                    high_score_10000 = '0';
                    high_score_100000 = '0';
                    break;
                case 3: high_score_1 = Convert.ToChar(list_char[0]);
                    high_score_10 = Convert.ToChar(list_char[1]);
                    high_score_100 = Convert.ToChar(list_char[2]);
                    high_score_1000 = '0';
                    high_score_10000 = '0';
                    high_score_100000 = '0';
                    break;
                case 4: high_score_1 = Convert.ToChar(list_char[0]);
                    high_score_10 = Convert.ToChar(list_char[1]);
                    high_score_100 = Convert.ToChar(list_char[2]);
                    high_score_1000 = Convert.ToChar(list_char[3]);
                    high_score_10000 = '0';
                    high_score_100000 = '0';
                    break;
                case 5: high_score_1 = Convert.ToChar(list_char[0]);
                    high_score_10 = Convert.ToChar(list_char[1]);
                    high_score_100 = Convert.ToChar(list_char[2]);
                    high_score_1000 = Convert.ToChar(list_char[3]);
                    high_score_10000 = Convert.ToChar(list_char[4]);
                    high_score_100000 = '0';
                    break;
                case 6: high_score_1 = Convert.ToChar(list_char[0]);
                    high_score_10 = Convert.ToChar(list_char[1]);
                    high_score_100 = Convert.ToChar(list_char[2]);
                    high_score_1000 = Convert.ToChar(list_char[3]);
                    high_score_10000 = Convert.ToChar(list_char[4]);
                    high_score_100000 = Convert.ToChar(list_char[5]);
                    break;
            }

            table_str = "";
        }


        /// <summary>
        /// Méthode Save_Score
        /// Permet de sauvegarder le meilleurs score
        /// </summary>
        public void Save_Score(bool IsGameOver, int currentscore)
        {
            string saved_game = @"G:\GAMEDEV\score_saved2.xml";
            XmlDocument myXmlDocument = new XmlDocument();

            if (IsGameOver == true)
            {
                //Si le score courant est supérieur à celui enregistré à la partie précédente, on le remplace
                if (currentscore > bestScore)
                {
                    myXmlDocument.Load(saved_game);

                    XmlNodeList highscore = myXmlDocument.SelectNodes("high_score");

                    foreach (XmlNode high in highscore)
                    {
                        high.InnerText = currentscore.ToString();
                    }

                    //Sauvegarde du fichier score_saved.xml
                    myXmlDocument.Save(saved_game);
                }
            }
        }


        /// <summary>
        ///  Méthode Update_Score
        /// Met à jour le score suivant les points marqués
        /// </summary>
        /// <param name="point_gom"></param>
        /// <param name="point_supergom_HG"></param>
        /// <param name="point_supergom_HD"></param>
        /// <param name="point_supergom_BG"></param>
        /// <param name="point_supergom_BD"></param>
        /// <param name="point_ghost"></param>
        public void Update_Score(bool IsGameOver, int finalScore)
        {
            //Ecrit dans le fichier score_saved.xml le nouveau score si celui-ci est supérieur au précédent
            if (IsGameOver == true)
            {
                Save_Score(IsGameOver, finalScore);
            }

            //Initialisation de la table du score à 0 pour chaque digit
            string[] table_score = new string[] { "0", "0", "0", "0", "0", "0" };

            //Ajout des nouveaux nombres dans la liste
            int lengh_score = finalScore.ToString().Length;

            string tab = finalScore.ToString();

            for (int i = lengh_score - 1; i >= 0; i--)
            {
                table_score[(lengh_score - 1) - i] = tab[i].ToString();
            }

            //Inverse l'ordre du tableau pour son affichage à l'écran
            Array.Reverse(table_score);

            //Récupération des variables locales par les variables globales de la classe Score
            score_1 = Convert.ToInt32(table_score[5]);
            score_10 = Convert.ToInt32(table_score[4]);
            score_100 = Convert.ToInt32(table_score[3]);
            score_1000 = Convert.ToInt32(table_score[2]);
            score_10000 = Convert.ToInt32(table_score[1]);
            score_100000 = Convert.ToInt32(table_score[0]);
        }

        /// <summary>
        /// Méthode DisplayReady
        /// Signale le début de la partie
        /// </summary>
        /// <param name="is_fullscreen"></param>
        /// <param name="spritebatch"></param>
        public void DisplayReady(bool is_fullscreen, SpriteBatch spritebatch)
        {
            if (timer_over < end_timer_over)
            {
                timer_over++;
            }

            else
            {
                timer_over = 0;

                if (color_Ready == Color.Yellow)
                {
                    color_Ready = Color.Transparent;
                }
                else
                {
                    color_Ready = Color.Yellow;
                }
            }

            //Affichage en FULLSCREEN
            if (is_fullscreen == true)
            {
                //Colonne GAME SCORE
                spritebatch.DrawString(this._fullscreen_font, text_Ready, new Vector2(100, 300), color_Ready);
            }

            //Affichage en WINDOWED
            else
            {
                //Colonne GAME SCORE
                spritebatch.DrawString(this._windowed_font, text_Ready, new Vector2(280, 295), color_Ready);
            }
        }

        /// <summary>
        /// Méthode DisplayGameOver
        /// Signale la fin de la partie
        /// </summary>
        public void DisplayGameOver(bool is_fullscreen, SpriteBatch spritebatch)
        {
            if (timer_over < end_timer_over)
            {
                timer_over++;
            }

            else
            {
                timer_over = 0;

                if (color_Over == Color.Red)
                {
                    color_Over = Color.Transparent;
                }
                else
                {
                    color_Over = Color.Red;
                }
            }

            //Affichage en FULLSCREEN
            if (is_fullscreen == true)
            {
                //Colonne GAME SCORE
                spritebatch.DrawString(this._fullscreen_font, text_GameOver, new Vector2(100, 300), color_Over);
            }

            //Affichage en WINDOWED
            else
            {
                //Colonne GAME SCORE
                spritebatch.DrawString(this._windowed_font, text_GameOver, new Vector2(241, 295), color_Over);
            }
        }

        /// <summary>
        /// Méthode DisplayScore
        /// Permet d'afficher le score à l'écran
        /// </summary>
        /// <param name="spritebatch"></param>
        /// <param name="score_1"></param>
        /// <param name="score_10"></param>
        /// <param name="score_100"></param>
        /// <param name="score_1000"></param>
        /// <param name="score_10000"></param>
        /// <param name="score_100000"></param>
        /// <param name="is_fullscreen"></param>
        public void DisplayScore(SpriteBatch spritebatch, bool is_fullscreen, int _num_level, Rectangle _rec_level)
        {
            Color color_text = _color_score;
            Color color_number = Color.Yellow;
            Color color_level = Color.DeepPink;

            num_level = _num_level.ToString();

            //Charge les rectangles pour l'encadrement et la disposition des scores
            Rectangle _rec_score = new Rectangle(_rec_level.X, _rec_level.Y, _game_score_display.Width, _game_score_display.Height);
            Rectangle _rec_high_score = new Rectangle(_rec_level.X, _rec_level.Y, _high_score_display.Width, _high_score_display.Height);
            Rectangle _rec_level_disp = new Rectangle(((int)_rec_level.X + (((int)_rec_level.Width / 2) - _level_display.Width / 2)), (int)_rec_level.Y - _level_display.Height, _level_display.Width, _level_display.Height);

            spritebatch.Draw(_game_score_display, new Vector2(_rec_score.X - _game_score_display.Width, _rec_score.Y), Color.White);

            number_score100000 = string.Format("{0}", score_100000);
            spritebatch.DrawString(this._windowed_font, number_score100000, new Vector2(_rec_score.Center.X - _game_score_display.Width - 9, _rec_score.Y + 55), color_number);

            number_score10000 = string.Format("{0}", score_10000);
            spritebatch.DrawString(this._windowed_font, number_score10000, new Vector2(_rec_score.Center.X - _game_score_display.Width - 9, _rec_score.Y + 75), color_number);

            number_score1000 = string.Format("{0}", score_1000);
            spritebatch.DrawString(this._windowed_font, number_score1000, new Vector2(_rec_score.Center.X - _game_score_display.Width - 9, _rec_score.Y + 95), color_number);

            number_score100 = string.Format("{0}", score_100);
            spritebatch.DrawString(this._windowed_font, number_score100, new Vector2(_rec_score.Center.X - _game_score_display.Width - 9, _rec_score.Y + 115), color_number);

            number_score10 = string.Format("{0}", score_10);
            spritebatch.DrawString(this._windowed_font, number_score10, new Vector2(_rec_score.Center.X - _game_score_display.Width - 9, _rec_score.Y + 135), color_number);

            number_score1 = string.Format("{0}", score_1);
            spritebatch.DrawString(this._windowed_font, number_score1, new Vector2(_rec_score.Center.X - _game_score_display.Width - 9, _rec_score.Y + 155), color_number);

            //Colonne HIGH SCORE        

            spritebatch.Draw(_high_score_display, new Vector2(_rec_high_score.X + _rec_level.Width + 17, _rec_high_score.Y), Color.White);

            high_score100000 = string.Format("{0}", high_score_100000);
            spritebatch.DrawString(this._windowed_font, high_score100000, new Vector2(_rec_high_score.Center.X + _rec_level.Width + 8, _rec_high_score.Y + 55), color_number);

            high_score10000 = string.Format("{0}", high_score_10000);
            spritebatch.DrawString(this._windowed_font, high_score10000, new Vector2(_rec_high_score.Center.X + _rec_level.Width + 8, _rec_high_score.Y + 75), color_number);

            high_score1000 = string.Format("{0}", high_score_1000);
            spritebatch.DrawString(this._windowed_font, high_score1000, new Vector2(_rec_high_score.Center.X + _rec_level.Width + 8, _rec_high_score.Y + 95), color_number);

            high_score100 = string.Format("{0}", high_score_100);
            spritebatch.DrawString(this._windowed_font, high_score100, new Vector2(_rec_high_score.Center.X + _rec_level.Width + 8, _rec_high_score.Y + 115), color_number);

            high_score10 = string.Format("{0}", high_score_10);
            spritebatch.DrawString(this._windowed_font, high_score10, new Vector2(_rec_high_score.Center.X + _rec_level.Width + 8, _rec_high_score.Y + 135), color_number);

            high_score1 = string.Format("{0}", high_score_1);
            spritebatch.DrawString(this._windowed_font, high_score1, new Vector2(_rec_high_score.Center.X + _rec_level.Width + 8, _rec_high_score.Y + 155), color_number);

            //Affichage du texte LEVEL
            spritebatch.Draw(_level_display, new Vector2(_rec_level_disp.X, _rec_level_disp.Y), Color.White);

            //Affichage du numéro du level
            spritebatch.DrawString(this._windowed_font, num_level, new Vector2(_rec_level_disp.X + _level_display.Width / 1.5f, _rec_level_disp.Y + 5), color_number);

            #region Traçage de test



            //string text1 = string.Format("{0}", _rec_level.X);

            //spritebatch.DrawString(_fullscreen_font, text1, new Vector2(0, 10), Color.Yellow);

            //        string text2 = string.Format("{0}", bestScore);

            //        spritebatch.DrawString(_fullscreen_font, text2, new Vector2(0, 30), Color.Red);

            //string text3 = string.Format("{0}", list_char[3]);

            //spritebatch.DrawString(_fullscreen_font, text3, new Vector2(0, 50), Color.Red);

            //string text4 = string.Format("{0}", list_char[2]);

            //spritebatch.DrawString(_fullscreen_font, text4, new Vector2(0, 70), Color.Red);

            //string text5 = string.Format("{0}", list_char[1]);

            //spritebatch.DrawString(_fullscreen_font, text5, new Vector2(0, 90), Color.Red);

            //string text6 = string.Format("{0}", list_char[0]);

            //spritebatch.DrawString(_fullscreen_font, text6, new Vector2(0, 110), Color.Red);


            #endregion
        }

        #endregion
    }
}
