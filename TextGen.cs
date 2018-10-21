using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Solitaire
{
    public static class TexGen
    {
        public static Texture2D white = null;
        /// <summary>
        /// Returns a single pixel white texture, if it doesn't exist, it creates one
        /// </summary>
        /// <exception cref="System.Exception">Please set the SpriteBatchEx.GraphicsDevice to your graphicsdevice before drawing lines.</exception>
        public static Texture2D White
        {
            get
            {
                if (white == null)
                {
                    if (SpriteBatchEx.GraphicsDevice == null)
                        throw new Exception("Please set the SpriteBatchEx.GraphicsDevice to your GraphicsDevice before drawing lines.");

                    white = new Texture2D(SpriteBatchEx.GraphicsDevice, 1, 1);

                    Color[] color = new Color[1];

                    color[0] = Color.White;

                    white.SetData<Color>(color);
                }

                return white;
            }
        }
    }
}