using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrabFish
{
    class Piece
    {

        private Texture2D texture;

        private Rectangle dstrect;
        private (int, int) pos;

        private string type, color;

        public bool selected = false;
        public bool firstMove = true;

        public Piece(string type, string color, Texture2D texture, (int, int) pos)
        {

            this.type = type;
            this.color = color; 
            this.texture = texture;
            this.pos = pos;

            this.dstrect = new Rectangle(this.pos.Item1 * 50, this.pos.Item2 * 50, 50, 50);

        }

        public void Move((int, int) pos)
        {

            this.pos = pos;
            this.dstrect = new Rectangle(this.pos.Item1 * 50, this.pos.Item2 * 50, 50, 50);

        }

        public string GetName()
        {

            return this.type;

        }

        public string GetColor()
        {

            return this.color; 

        }

        public (int, int) GetPos()
        {

            return this.pos;

        }

        public Texture2D GetTexture()
        {

            return this.texture;

        }

        public Rectangle GetRect()
        {

            return this.dstrect;

        }

    }
}
