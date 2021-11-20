using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrabFish
{
    class Input
    {

        private MouseState currentMouseState, previousMouseState;

        public MouseState GetMouseState()
        {

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            return currentMouseState;

        }

        public bool IsClicked()
        {

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {

                return true;

            }

            previousMouseState = currentMouseState;

            return false;

        }

        public (int, int) GetMouse()
        {

            return (currentMouseState.X, currentMouseState.Y);

        }

        public (int, int) GetPos()
        {

            return (currentMouseState.X / 50, currentMouseState.Y / 50);

        }

    }
}
