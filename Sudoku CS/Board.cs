using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Sudoku_CS
{
    class Board
    {
        ContentManager content;
        Random r = new Random();

        // Easy = 36+ blocks revealed. Medium = 27-36 blocks revealed. Hard = 19-26 blocks revealed.
        public int[,] blockGrid = new int[9, 9];

        public int[,] winningBlockGrid = new int[9, 9];


        public Texture2D backgroundImage;
        public Vector2 backgroundPosition;

        SpriteFont font;

        int blocksRevealed = 38;

        public Board(ContentManager Content)
        {
            content = Content;

            Array.Clear(blockGrid, 0, blockGrid.Length);

            NewWinningGrid();

            // 38 revealed
            // check for collisions
            for (int i = 0; i < blocksRevealed; i++)
            {
                int x = r.Next(0, 9);
                int y = r.Next(0, 9);

                blockGrid[x, y] = winningBlockGrid[x, y];
            }



            //// temp reveal entire winning grid to test
            //for (int i = 0; i < 9; i++)
            //{
            //    for (int j = 0; j < 9; j++)
            //    {
            //        blockGrid[i, j] = winningBlockGrid[i, j];
            //    }
            //}


            System.Diagnostics.Debug.WriteLine(blockGrid[5, 5]);


            backgroundPosition = new Vector2(50, 50);

            LoadContent();
        }

        public void NewWinningGrid()
        {
            HashSet<int> randomNumbersRowSet = new HashSet<int>();

            for (int i = 1; i < 10; i++)
                while (!randomNumbersRowSet.Add(r.Next(1, 10))) ;


            System.Diagnostics.Debug.WriteLine(randomNumbersRowSet.Count);

            int[] randomNumbersRow = new int[randomNumbersRowSet.Count];
            randomNumbersRowSet.CopyTo(randomNumbersRow);



            // temp
            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 0] = randomNumbersRow[i];
            }

            randomNumbersRow = shiftBoardBy(randomNumbersRow, 3);

            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 1] = randomNumbersRow[i];
            }

            randomNumbersRow = shiftBoardBy(randomNumbersRow, 3);

            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 2] = randomNumbersRow[i];
            }


            randomNumbersRow = shiftBoardBy(randomNumbersRow, 1);

            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 3] = randomNumbersRow[i];
            }

            randomNumbersRow = shiftBoardBy(randomNumbersRow, 3);

            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 4] = randomNumbersRow[i];
            }

            randomNumbersRow = shiftBoardBy(randomNumbersRow, 3);

            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 5] = randomNumbersRow[i];
            }


            randomNumbersRow = shiftBoardBy(randomNumbersRow, 1);

            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 6] = randomNumbersRow[i];
            }

            randomNumbersRow = shiftBoardBy(randomNumbersRow, 3);

            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 7] = randomNumbersRow[i];
            }

            randomNumbersRow = shiftBoardBy(randomNumbersRow, 3);

            for (int i = 0; i < 9; i++)
            {
                winningBlockGrid[i, 8] = randomNumbersRow[i];
            }


            //System.Diagnostics.Debug.WriteLine(randomNumbers3);
        }

        public int[] shiftBoardBy(int[] row, int shift)
        {
            int[] newRow = new int[9];

            for (int i = 0; i < 9; i++)
            {
                newRow[i] = row[(i + shift) % 9];
            }

            return newRow;
        }

        public void LoadContent()
        {
            backgroundImage = content.Load<Texture2D>("background");

            font = content.Load<SpriteFont>("Font");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, backgroundPosition);


            for (int i = 0; i < blockGrid.GetLength(0); i++)
                for (int j = 0; j < blockGrid.GetLength(1); j++)
                    if (blockGrid[i,j] != 0)
                    {
                        spriteBatch.DrawString(font, blockGrid[i, j].ToString(), new Vector2(50 + (i*100) + 25, 50 + (j*100) ), Color.Black);
                    }
        }
    }
}
