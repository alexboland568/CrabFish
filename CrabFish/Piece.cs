using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrabFish
{
    class Piece
    {

        private Texture2D texture;

        private Rectangle dstrect, srcrect;
        private (int, int) pos;

        private string type, color;

        public Piece(string type, string color, Texture2D texture, (int, int) pos)
        {

            this.type = type;
            this.color = color; 
            this.texture = texture;
            this.pos = pos;

            dstrect = 

            if (type == "Pawn")
            {



            }

        }

        public void Draw(SpriteBatch batch)
        {

            batch.Draw(texture, dstrect, srcrect, Color.White);

        }

    }
}
