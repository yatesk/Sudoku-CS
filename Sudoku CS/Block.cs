using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sudoku_CS
{
    class Block
    {
        static public SpriteFont numberFont;
        static public SpriteFont candidateFont;

        static public Texture2D selectedBlockImage;
        static public Texture2D revealedBlockImage;

        static public Texture2D invalidNumberImage;

        public int number;
        public bool invalidNumber;

        public List<int> candidates = new List<int>();

        public enum BlockBackground { None, Revealed, Selected };

        public BlockBackground background;
        public Vector2 position;

        // better way?  Used to determine which grid block is clicked.
        public static int[] gridCoords = new int[] { 56, 154, 252, 354, 452, 550, 652, 750, 848, 944 };

        public static int Size = 96;

        public Block(Vector2 _position, BlockBackground _background, int _number = 0)
        {
            position = _position;
            background = _background;
            number = _number;
            invalidNumber = false;
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
                spriteBatch.DrawString(numberFont, number.ToString(), new Vector2(position.X + 25, position.Y + 10), Color.Black);

                // Draws red circle if the number is invalid.
                if (invalidNumber)
                {
                    spriteBatch.Draw(invalidNumberImage, new Vector2(position.X + Size - 24, position.Y + Size - 24));
                }
            }
            else
            {
                foreach (int item in candidates)
                {
                    spriteBatch.DrawString(candidateFont, item.ToString(), new Vector2(position.X + 5 + (((item - 1) % 3) * 35), position.Y + ((item - 1) / 3) * 35), Color.Black);
                }
            }
        }

        public void addOrRemoveCandidate(int _x, int _y)
        {
            int relativeX = _x - (int)this.position.X;
            int relativeY = _y - (int)this.position.Y;

            int column = 0;
            int row = 0;

            if (relativeX < 32)
                column = 1;
            else if (relativeX < 64)
                column = 2;
            else if (relativeX < 96)
                column = 3;

            if (relativeY < 32)
                row = 1;
            else if (relativeY < 64)
                row = 2;
            else if (relativeY < 96)
                row = 3;

            int whichCanidate = column + (row - 1) * 3;

            if (candidates.Contains(whichCanidate))
            {
                candidates.Remove(whichCanidate);
            }
            else
            {
                candidates.Add(whichCanidate);
            }

            //System.Diagnostics.Debug.WriteLine(relativeX + " " + relativeY);
        }

        public static Tuple<int, int> WhichBlock(int _x, int _y)
        {
            int xIndex = -1;
            int yIndex = -1;

            for (int i = 0; i < gridCoords.Length - 1; i++)
            {
                if (_x >= gridCoords[i] && _x <= gridCoords[i + 1])
                {
                    xIndex = i;
                }
            }

            for (int i = 0; i < gridCoords.Length - 1; i++)
            {
                if (_y >= gridCoords[i] && _y <= gridCoords[i + 1])
                {
                    yIndex = i;
                }
            }

            // If out of bounds return (-1, -1) or else return correct value.
            if (xIndex < 0 || xIndex > 8 || yIndex < 0 || yIndex > 8)
            {
                return new Tuple<int, int>(-1, -1);
            }
            else
            {
                return new Tuple<int, int>(xIndex, yIndex);
            }
        }
    }
}
