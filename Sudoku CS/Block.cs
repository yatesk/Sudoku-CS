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

        public enum BlockBackground { None, Revealed, Selected };

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

            else if (background == BlockBackground.Selected)
            {
                spriteBatch.Draw(selectedBlockImage, position);
            }

            if (number != 0)
            {
                // Kind of centered.
                spriteBatch.DrawString(font, number.ToString(), new Vector2(position.X + 25, position.Y + 10), Color.Black);
            }
        }


        public static Tuple<int, int> WhichBlock(int _x, int _y)
        {
            int x = _x;
            int y = _y;

            // margin
            x -= 56;
            y -= 56;

            // bugs
            x /= 96;
            y /= 96;

            return new Tuple<int, int>(x, y);
        }
    }
}
