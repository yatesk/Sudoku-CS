using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Sudoku_CS
{
    class Block
    {
        static public SpriteFont font;
        static public Texture2D selectedBlockImage;
        static public Texture2D revealedBlockImage;


        public int number;

        public enum BlockBackground { Revealed, Selected, White };

        public BlockBackground background;
        public Vector2 position;

        public static int size = 96;

        public Block(Vector2 _position, BlockBackground _background, int _number = 0)
        {
            position = _position;
            background = _background;
            number = _number;
        }

        // Draw Text Number, canadidate numbers, and background color.
        public void Draw(SpriteBatch spriteBatch)
        {
            if (background == BlockBackground.Revealed)
            {
                spriteBatch.Draw(revealedBlockImage, position);
            }

            spriteBatch.DrawString(font, number.ToString(), position, Color.Black);
        }
    }
}
