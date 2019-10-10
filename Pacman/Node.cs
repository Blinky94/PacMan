using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.Linq;
namespace PacMan
{
    class Node
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Tile_Rec { get; set; }
        public bool IsWall { get; set; }
        public bool IsEaten { get; set; }
        public char IsChar { get; set; }
        public Color ColoR { get; set; }
        public SpriteEffects spriteEffect { get; set; }
        public List<Vector2> List_Successor { get; set; }
        public List<bool> ListNextNodesIsWall { get; set; }
        public int Tile_Length { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }
        public bool IsPathInky { get; set; }
        public bool IsPathPinky { get; set; }
        public bool IsPathBlinky { get; set; }
        public bool IsPathKlyde { get; set; }
        public Vector2 ParentNode { get; set; }
    }
}
