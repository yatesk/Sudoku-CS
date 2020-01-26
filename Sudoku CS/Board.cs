﻿using Microsoft.Xna.Framework;
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
        // Easy = 38. Medium = 30. Hard = 24
        public enum Difficulty { Easy = 38, Medium = 30, Hard = 24 };

        public int blocksRevealed = 77; // (int)Difficulty.Easy;
        public int correctBlocks = 0;
        public Block[,] grid = new Block[9, 9];
        public List<int[]> winningBlockGrid = new List<int[]>();

        public Texture2D backgroundImage;
        public Vector2 backgroundPosition;

        public Texture2D pauseImage;

        public bool blockSelected = false;

        public float timer = 0f;
        public bool isPaused = false;

        private int boardBoarder = 10;

        public Board(ContentManager Content)
        {
            content = Content;
            backgroundPosition = new Vector2(boardBoarder, boardBoarder);

            // Fill blockGrid
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int gridMarginX = 7;
                    int gridMarginY = 7;

                    // better way?
                    if (i < 3)
                    {
                        gridMarginX += 3 * i;
                    }
                    else if (i < 6)
                    {
                        gridMarginX += (3 * i) + 3;  // subgrid margin - Block margin = 3
                    }
                    else if (i < 9)
                    {
                        gridMarginX += (3 * i) + 6;  // (subgrid margin - Block margin) * 2 = 6
                    }

                    if (j < 3)
                    {
                        gridMarginY += 3 * j;
                    }
                    else if (j < 6)
                    {
                        gridMarginY += (3 * j) + 3;
                    }
                    else if (j < 9)
                    {
                        gridMarginY += (3 * j) + 6;
                    }

                    grid[i, j] = new Block(new Vector2((i * 84) + boardBoarder + gridMarginX, boardBoarder + (j * 84) + gridMarginY), Block.BlockBackground.None, 0);

                    System.Diagnostics.Debug.WriteLine((i * 84) + boardBoarder + gridMarginX);
                }
            }

            NewWinningGrid();

            int counter = 0;

            while (counter < blocksRevealed)
            {
                int x = r.Next(0, 9);
                int y = r.Next(0, 9);

                if (grid[x, y].background != Block.BlockBackground.Revealed)
                {
                    grid[x, y].number = winningBlockGrid[x][y];
                    grid[x, y].background = Block.BlockBackground.Revealed;
                    grid[x, y].validNumber = true;
                    correctBlocks++;
                    counter++;
                }
            }

            LoadContent();
        }

        public void NewWinningGrid()
        {
            HashSet<int> randomNumbersRowSeed = new HashSet<int>();

            for (int i = 1; i < 10; i++)
                while (!randomNumbersRowSeed.Add(r.Next(1, 10))) ;

            int[] randomNumbersRow = new int[randomNumbersRowSeed.Count];
            randomNumbersRowSeed.CopyTo(randomNumbersRow);

            winningBlockGrid = SeedRowToWinningBoard(randomNumbersRow);
        }

        public List<int[]> SeedRowToWinningBoard(int[] seedRow)
        {
            List<int[]> winningBlockGrid = new List<int[]>();

            winningBlockGrid.Add(seedRow);

            seedRow = ShiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = ShiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = ShiftRowBy(seedRow, 1);
            winningBlockGrid.Add(seedRow);

            seedRow = ShiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = ShiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = ShiftRowBy(seedRow, 1);
            winningBlockGrid.Add(seedRow);

            seedRow = ShiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            seedRow = ShiftRowBy(seedRow, 3);
            winningBlockGrid.Add(seedRow);

            return winningBlockGrid;
        }

        public int[] ShiftRowBy(int[] row, int shift)
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

            pauseImage = content.Load<Texture2D>("pause");

            Block.selectedBlockImage = content.Load<Texture2D>("selectedBlock");
            Block.revealedBlockImage = content.Load<Texture2D>("revealedBlock");

            Block.invalidNumberImage = content.Load<Texture2D>("invalidNumber");

            Block.numberFont = content.Load<SpriteFont>("numberFont");
            Block.candidateFont = content.Load<SpriteFont>("canidateFont");
        }

        public void Update(GameTime gameTime)
        {
            if (!isPaused)
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, backgroundPosition);

            // Draw grid
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    grid[i, j].Draw(spriteBatch);

            //// Draw timer
            //// refactor
            //int time = (int)timer;
            
            //if (time % 60 < 10)
            //{
            //    spriteBatch.DrawString(Block.candidateFont, (time / 60).ToString() + ":0" + (time % 60).ToString(), new Vector2(450, 0), Color.Black);
            //}
            //else
            //{
            //    spriteBatch.DrawString(Block.candidateFont, (time / 60).ToString() + ":" + (time % 60).ToString(), new Vector2(450, 0), Color.Black);
            //}
            
            //spriteBatch.Draw(pauseImage, new Vector2(500, 4));
            
        }

        public void NewGame()
        {
            timer = 0f;
        }

        public void ValidOrInvalidNumber(int _x, int _y)
        {
            // check horizontal
            int count = 0;
            bool valid = true;

            for (int i = 0; i < 9; i++)
            {
                if (grid[i, _y].number == grid[_x, _y].number)
                {
                    count++;
                }
            }

            if (count != 1)
                valid = false;


            // check vertical
            count = 0;

            for (int i = 0; i < 9; i++)
            {
                if (grid[_x, i].number == grid[_x, _y].number)
                {
                    count++;
                }
            }

            if (count != 1)
                valid = false;


            // check subGrid
            int[] subGridStartingCoords = FindSubGrid(_x, _y);  
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

            if (count != 1)
                valid = false;


            if (grid[_x, _y].validNumber)
            {
                if(!valid)
                {
                    correctBlocks--;
                    grid[_x, _y].validNumber = false;
                }
            }
            else
            {
                if (valid)
                {
                    correctBlocks++;
                    grid[_x, _y].validNumber = true;
                }
            }
        }

        private int[] FindSubGrid(int x, int y)
        {
            int[] subGridStartingCoords = new int[2];

            if (x >= 0 && x <= 2)
                subGridStartingCoords[0] = 0; 
            else if (x >= 3 && x <= 5)
                subGridStartingCoords[0] = 3;
            else if (x >= 6 && x <= 8)
                subGridStartingCoords[0] = 6;

            if (y >= 0 && y <= 2)
                subGridStartingCoords[1] = 0;
            else if (y >= 3 && y <= 5)
                subGridStartingCoords[1] = 3;
            else if (y >= 6 && y <= 8)
                subGridStartingCoords[1] = 6;

            return subGridStartingCoords;
        }
    }
}