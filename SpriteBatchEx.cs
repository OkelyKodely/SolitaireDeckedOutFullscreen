using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Solitaire
{
    public static class SpriteBatchEx
    {
        /// <summary>
        /// The graphics device, set this before drawing lines
        /// </summary>
        public static GraphicsDevice GraphicsDevice;

        /// <summary>
        /// Draws a single line. 
        /// Require SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);

            Vector2 v = Vector2.Normalize(begin - end);

            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));

            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;

            spriteBatch.Draw(TexGen.White, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}