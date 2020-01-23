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
        int blocksRevealed = 75;
        public int blocksEntered = 75;
        public Block[,] grid = new Block[9, 9];
        public List<int[]> winningBlockGrid = new List<int[]>();

        public Texture2D backgroundImage;
        public Vector2 backgroundPosition;

        public bool blockSelected = false;

        public float timer = 0f;

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

                    grid[i, j] = new Block(new Vector2((i * 96) + 56 + gridMarginX, 56 + (j * 96) + gridMarginY), Block.BlockBackground.None, 0);
                }
            }

            NewWinningGrid();

            // 38 revealed
            int counter = 0;

            while (counter < blocksRevealed)
            {
                int x = r.Next(0, 9);
                int y = r.Next(0, 9);

                if (grid[x, y].background != Block.BlockBackground.Revealed)
                {
                    grid[x, y].number = winningBlockGrid[x][y];
                    grid[x, y].background = Block.BlockBackground.Revealed;
                    counter++;
                }
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

            Block.invalidNumberImage = content.Load<Texture2D>("invalidNumber");

            Block.numberFont = content.Load<SpriteFont>("numberFont");
            Block.candidateFont = content.Load<SpriteFont>("canidateFont");

        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, backgroundPosition);

            // Draw grid
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    grid[i, j].Draw(spriteBatch);

            // Draw timer
            // refactor
            int time = (int)timer;
            
            if (time % 60 < 10)
            {
                spriteBatch.DrawString(Block.candidateFont, (time / 60).ToString() + ":0" + (time % 60).ToString(), new Vector2(500, 0), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(Block.candidateFont, (time / 60).ToString() + ":" + (time % 60).ToString(), new Vector2(500, 0), Color.Black);
            }
            
        }

        public void NewGame()
        {
            timer = 0f;
        }

        public void CheckForInvalidNumber(int _x, int _y)
        {
            // refactor
            // check horizontal
            int count = 0;

            for (int i = 0; i < 9; i++)
            {
                if (grid[i, _y].number == grid[_x, _y].number)
                {
                    count++;
                }
            }

            if (count > 1)
            {
                grid[_x, _y].invalidNumber = true;
                return;
            }
            else
            {
                grid[_x, _y].invalidNumber = false;
            }


            // check vertical
            count = 0;

            for (int i = 0; i < 9; i++)
            {
                if (grid[_x, i].number == grid[_x, _y].number)
                {
                    count++;
                }
            }

            if (count > 1)
            {
                grid[_x, _y].invalidNumber = true;
                return;
            }
            else
            {
                grid[_x, _y].invalidNumber = false;
            }


            // check subGrid
            int[] subGridStartingCoords = new int[2];  // {x, y]

            subGridStartingCoords = findSubGrid(_x, _y);


            count = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (grid[i + subGridStartingCoords[0], j + subGridStartingCoords[1]].number == grid[_x, _y].number)
                    {
                        count++;
                    }
                }
            }

            if (count > 1)
            {
                grid[_x, _y].invalidNumber = true;
            }
            else
            {
                grid[_x, _y].invalidNumber = false;
            }
        }

        // refactor
        private int[] findSubGrid(int x, int y)
        {
            int[] subGridStartingCoords = new int[2];

            if (x == 0 || x == 1 || x == 2)
                subGridStartingCoords[0] = 0; 
            else if (x == 3 || x == 4 || x == 5)
                subGridStartingCoords[0] = 3;
            else if (x == 6 || x == 7 || x == 8)
                subGridStartingCoords[0] = 6;

            if (y == 0 || y == 1 || y == 2)
                subGridStartingCoords[1] = 0;
            else if (y == 3 || y == 4 || y == 5)
                subGridStartingCoords[1] = 3;
            else if (y == 6 || y == 7 || y == 8)
                subGridStartingCoords[1] = 6;

            return subGridStartingCoords;
        }
    }
}