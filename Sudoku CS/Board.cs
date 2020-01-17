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

        public Vector2 backgroundPosition;

        int blocksRevealed = 38;

        public bool blockSelected = false;

        public Board(ContentManager Content)
        {
            content = Content;
            backgroundPosition = new Vector2(50, 50);

            //Array.Clear(blockGrid, 0, blockGrid.Length);

            // Fill blockGrid
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int gridMarginX = 0;
                    int gridMarginY = 0;

                    // better way?
                    if (i < 3)
                    {
                        gridMarginX = 2 * i;
                    }
                    else if (i < 6)
                    {
                        gridMarginX = (2 * i) + 4;
                    }
                    else if (i < 9)
                    {
                        gridMarginX = (2 * i) + 8;
                    }


                    if (j < 3)
                    {
                        gridMarginY = 2 * j;
                    }
                    else if (j < 6)
                    {
                        gridMarginY = (2 * j) + 4;
                    }
                    else if (j < 9)
                    {
                        gridMarginY = (2 * j) + 8;
                    }

                    grid[i,j] = new Block(new Vector2((i * 96) + 56 + gridMarginX, 56+(j * 96) + gridMarginY), Block.BlockBackground.None, 0);
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
                grid[x, y].background = Block.BlockBackground.Revealed;
            }

            // reveal entire winning grid to test
            //blockGrid = new List<int[]>(winningBlockGrid);

            

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

            Block.selectedBlockImage = content.Load<Texture2D>("selectedBlock");
            Block.revealedBlockImage = content.Load<Texture2D>("revealedBlock");

            Block.font = content.Load<SpriteFont>("Font");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, backgroundPosition);

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    grid[i, j].Draw(spriteBatch);        
        }
    }
}