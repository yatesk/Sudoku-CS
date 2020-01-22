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

        public static int size = 96;

        // better way?  Used to determine which grid block is clicked.
        public static int[] xGridCoords = new int[] {56, 154, 252, 354, 452, 550, 652, 750, 848, 944}; 
        public static int[] yGridCoords = new int[] {56, 154, 252, 354, 452, 550, 652, 750, 848, 944}; 

        // int[] array1 = new int[] { 1, 3, 5, 7, 9 };
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

                if (invalidNumber)
                {
                    spriteBatch.Draw(invalidNumberImage, new Vector2(position.X + size - 24, position.Y + size - 24));
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

            // better way? refactor
            if (relativeY < 32)
            {
                if ( relativeX < 32)
                {
                    if (candidates.Contains(1))
                    {
                        candidates.Remove(1);
                    }
                    else
                    {
                        candidates.Add(1);
                    }
                }
                else if (relativeX < 64)
                {
                    if (candidates.Contains(2))
                    {
                        candidates.Remove(2);
                    }
                    else
                    {
                        candidates.Add(2);
                    }
                }
                else if (relativeX < 96)
                {
                    if (candidates.Contains(3))
                    {
                        candidates.Remove(3);
                    }
                    else
                    {
                        candidates.Add(3);
                    }
                }
            }
            else if (relativeY < 64)
            {
                if (relativeX < 32)
                {
                    if (candidates.Contains(4))
                    {
                        candidates.Remove(4);
                    }
                    else
                    {
                        candidates.Add(4);
                    }
                }
                else if (relativeX < 64)
                {
                    if (candidates.Contains(5))
                    {
                        candidates.Remove(5);
                    }
                    else
                    {
                        candidates.Add(5);
                    }
                }
                else if (relativeX < 96)
                {
                    if (candidates.Contains(6))
                    {
                        candidates.Remove(6);
                    }
                    else
                    {
                        candidates.Add(6);
                    }
                }
            }
            else if (relativeY < 96)
            {
                if (relativeX < 32)
                {
                    if (candidates.Contains(7))
                    {
                        candidates.Remove(7);
                    }
                    else
                    {
                        candidates.Add(7);
                    }
                }
                else if (relativeX < 64)
                {
                    if (candidates.Contains(8))
                    {
                        candidates.Remove(8);
                    }
                    else
                    {
                        candidates.Add(8);
                    }
                }
                else if (relativeX < 96)
                {
                    if (candidates.Contains(9))
                    {
                        candidates.Remove(9);
                    }
                    else
                    {
                        candidates.Add(9);
                    }
                }
            }

            //System.Diagnostics.Debug.WriteLine(relativeX + " " + relativeY);
        }

        public static Tuple<int, int> WhichBlock(int _x, int _y)
        {
            int xIndex = -1;
            int yIndex = -1;

            for (int i = 0; i < xGridCoords.Length - 1; i++)
            {
                if (_x >= xGridCoords[i] && _x <= xGridCoords[i+1])
                {
                    xIndex = i;
                }
            }

            for (int i = 0; i < yGridCoords.Length - 1; i++)
            {
                if (_y >= yGridCoords[i] && _y <= yGridCoords[i + 1])
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
