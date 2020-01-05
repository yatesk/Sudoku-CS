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
        public Block[,] grid = new Block[9, 9];
        public List<int[]> winningBlockGrid = new List<int[]>();

        public Texture2D backgroundImage;
        public Texture2D selectedBlockImage;
        public Texture2D revealedBlockImage;

        public Vector2 backgroundPosition;

        SpriteFont font;

        int blocksRevealed = 38;

        public Board(ContentManager Content)
        {
            content = Content;

            //Array.Clear(blockGrid, 0, blockGrid.Length);

            // Fill blockGrid
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i,j] = new Block(new Vector2(0,0), Block.BlockBackground.White, 0);
                }
            }

            NewWinningGrid();

            // 38 revealed
            // check for collisions
            for (int i = 0; i < blocksRevealed; i++)
            {
                int x = r.Next(0, 9);
                int y = r.Next(0, 9);

                grid[x, y].number = winningBlockGrid[x][y];
            }

            // reveal entire winning grid to test
            //blockGrid = new List<int[]>(winningBlockGrid);

            backgroundPosition = new Vector2(50, 50);

            LoadContent();
        }

        public void NewWinningGrid()
        {
            HashSet<int> randomNumbersRowSeed = new HashSet<int>();

            for (int i = 1; i < 10; i++)
                while (!randomNumbersRowSeed.Add(r.Next(1, 10))) ;

            int[] randomNumbersRow = new int[randomNumbersRowSeed.Count];
            randomNumbersRowSeed.CopyTo(randomNumbersRow);

            winningBlockGrid = seedRowToWinningBoard(randomNumbersRow);

            //System.Diagnostics.Debug.WriteLine(randomNumbers3);
        }

        
        public List<int[]> seedRowToWinningBoard(int[] seedRow)
        {
            List<int[]> winningBlockGrid = new List<int[]>();

            winningBlockGrid.Add(seedRow);

            seedRow = shiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = shiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = shiftRowBy(seedRow, 1);
            winningBlockGrid.Add(seedRow);

            seedRow = shiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = shiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = shiftRowBy(seedRow, 1);
            winningBlockGrid.Add(seedRow);

            seedRow = shiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = shiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            return winningBlockGrid;
        }

        public int[] shiftRowBy(int[] row, int shift)
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

            /////////////
            /////////////////
            ///



            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (grid[i,j].number != 0)
                    {
                        spriteBatch.DrawString(font, grid[i,j].number.ToString(), new Vector2(50 + (i*100) + 25, 50 + (j*100) ), Color.Black);
                    }
        }
    }
}
