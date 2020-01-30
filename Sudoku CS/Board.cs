using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

using System.Net;

namespace Sudoku_CS
{
    class Board
    {
        private ContentManager content;
        private readonly Random r = new Random();
        private readonly int boardMargin = 10;

        public string difficulty = "Easy";

        private int correctBlocks = 0;
        public Block[,] grid = new Block[9, 9];

        private Texture2D backgroundImage;
        private Vector2 backgroundPosition;

        private Texture2D newPuzzleImage;
        private Texture2D savePuzzleImage;
        private Texture2D difficultiesImage;
        private Texture2D nyTimesImage;

        private Texture2D pauseImage;

        private float timer = 0f;
        public bool isPaused = false;

        public bool newPuzzle = false;
        public bool newNYTimesPuzzle = false;

        public Board(ContentManager Content)
        {
            content = Content;
            backgroundPosition = new Vector2(boardMargin, boardMargin);

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

                    grid[i, j] = new Block(new Vector2((i * 84) + boardMargin + gridMarginX, boardMargin + (j * 84) + gridMarginY), false, 0);
                }
            }

            LoadBoardFromSavedTextfile();
            LoadContent();
        }

        public void LoadContent()
        {
            backgroundImage = content.Load<Texture2D>("background");
            newPuzzleImage = content.Load<Texture2D>("newPuzzle");
            savePuzzleImage = content.Load<Texture2D>("savePuzzle");
            difficultiesImage = content.Load<Texture2D>("difficulties");
            nyTimesImage = content.Load<Texture2D>("nyTimes");
            pauseImage = content.Load<Texture2D>("pause");

            Block.revealedBlockImage = content.Load<Texture2D>("revealedBlock");
            Block.invalidNumberImage = content.Load<Texture2D>("invalidNumber");
            Block.numberFont = content.Load<SpriteFont>("numberFont");
            Block.candidateFont = content.Load<SpriteFont>("canidateFont");
        }

        public void Update(GameTime gameTime)
        {
            if (!isPaused && correctBlocks != 81)
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, backgroundPosition);

            if (!isPaused)
            {
                // Draw grid
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        grid[i, j].Draw(spriteBatch);
            }
            else
                spriteBatch.DrawString(Block.numberFont, "PAUSED", new Vector2(375, 375), Color.Black);

            //// Draw timer
            int time = (int)timer;
           
            if (time % 60 < 10)
            {
                spriteBatch.DrawString(Block.candidateFont, (time / 60).ToString() + ":0" + (time % 60).ToString(), new Vector2(850, 50), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(Block.candidateFont, (time / 60).ToString() + ":" + (time % 60).ToString(), new Vector2(850, 50), Color.Black);
            }

            // checks to see if player won
            if (correctBlocks == 81)
            {
                spriteBatch.DrawString(Block.numberFont, "YOU", new Vector2(800, 400), Color.Black);
                spriteBatch.DrawString(Block.numberFont, "WON", new Vector2(800, 475), Color.Black);
            }

            spriteBatch.DrawString(Block.candidateFont, difficulty, new Vector2(850, 0), Color.Black);

            spriteBatch.Draw(pauseImage, new Vector2(900, 53));
            spriteBatch.Draw(nyTimesImage, new Vector2(850, 700));
            spriteBatch.Draw(newPuzzleImage, new Vector2(850, 800));
            spriteBatch.Draw(savePuzzleImage, new Vector2(850, 150));

            if (newPuzzle || newNYTimesPuzzle)
                spriteBatch.Draw(difficultiesImage, new Vector2(850, 500));
        }

        public void ClearBoard()
        {
            timer = 0f;

            correctBlocks = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j].number = 0;
                    grid[i, j].revealed = false;
                    grid[i, j].validNumber = false;
                    grid[i, j].candidates.Clear();
                }
            }
        }

        public void NewPuzzle()
        {
            ClearBoard();

            String line;
            List<char[]> level = new List<char[]>();

            FileStream fsSource = new FileStream(difficulty + ".txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fsSource))
            {
                int randomNumber = r.Next(0, 21);

                // Skip to random puzzle #0-21
                for (int i = 0; i < randomNumber*11; i++)
                {
                    line = sr.ReadLine();
                }

                for (int i = 0; i < 9; i++)
                {
                    line = sr.ReadLine();

                    for (int j = 0; j < 9; j++)
                    {
                        if (line[j] != '.')
                        {
                            grid[i, j].number = (int)char.GetNumericValue(line[j]);
                            grid[i, j].revealed = true;
                            grid[i, j].validNumber = true;
                            correctBlocks++;
                        }
                        else
                        {
                            grid[i, j].number = 0;
                            grid[i, j].revealed = false;
                            grid[i, j].validNumber = false;
                        }
                    }
                }
            }
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
                if (!valid)
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

        public void LoadBoardFromSavedTextfile()
        {
            String line;
            List<char[]> level = new List<char[]>();

            FileStream fsSource = new FileStream("savedBoard.txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fsSource))
            {
                difficulty = sr.ReadLine();
                timer = float.Parse(sr.ReadLine());

                for (int column = 0; column < 9; column++)
                {
                    for (int row = 0; row < 9; row++)
                    {
                        line = sr.ReadLine();
                        List<int> numbers = new List<int>(Array.ConvertAll(line.Split(' '), int.Parse));

                        grid[column, row].number = numbers[0];

                        if (numbers[1] == 1)
                        {
                            grid[column, row].revealed = true;
                        }
                        else
                        {
                            grid[column, row].revealed = false;
                        }

                        if (numbers[2] == 1)
                        {
                            grid[column, row].validNumber = true;
                            correctBlocks++;
                        }
                        else
                        {
                            grid[column, row].validNumber = false;
                        }

                        for (int i = 3; i < numbers.Count; i++)
                        {
                            grid[column, row].candidates.Add(numbers[i]);
                        }
                    }
                }
            }
        }

        public void GetNYTimesPuzzle()
        {
            string htmlCode = "";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://www.nytimes.com/puzzles/sudoku/easy");
            }

            // out of order on nytimes?
            int indexEasy = htmlCode.IndexOf("\"puzzle\":[");
            int indexHard = htmlCode.IndexOf("\"puzzle\":[", indexEasy + 161);
            int indexMedium = htmlCode.IndexOf("\"puzzle\":[", indexHard + 161);


            if (difficulty == "Easy")
            {
                LoadNYTimesPuzzle(htmlCode.Substring(indexEasy + 10, 161));
            }
            else if (difficulty == "Medium")
            {
                LoadNYTimesPuzzle(htmlCode.Substring(indexMedium + 10, 161));
            }
            else if (difficulty == "Hard")
            {
                LoadNYTimesPuzzle(htmlCode.Substring(indexHard + 10, 161));
            }
        }

        public void LoadNYTimesPuzzle(string puzzle)
        {
            // new puzzle refactor
            timer = 0f;
            correctBlocks = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j].number = 0;
                    grid[i, j].revealed = false;
                    grid[i, j].validNumber = false;
                    grid[i, j].candidates.Clear();
                }
            }

            string[] puzzleNumbers = puzzle.Split(',');
            int index = 0;

            //System.Diagnostics.Debug.WriteLine(puzzleNumbers);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int number = Int32.Parse(puzzleNumbers[index]);

                    if (number != 0)
                    {
                        grid[j, i].number = number;   //(int)char.GetNumericValue(line[j]);
                        grid[j, i].revealed = true;
                        grid[j, i].validNumber = true;
                        correctBlocks++;
                    }
                    else
                    {
                        grid[j, i].number = 0;
                        grid[j, i].revealed = false;
                        grid[j, i].validNumber = false;
                    }
                    index++;
                }
            }
        }

        public void SaveBoard()
        {
            StreamWriter sw = new StreamWriter("savedBoard.txt");  //FileStream("savedBoard.txt", FileMode.Open, FileAccess.Write);

            sw.WriteLine(difficulty);
            sw.WriteLine(timer);

            for (int column = 0; column < 9; column++)
            {
                for (int row = 0; row < 9; row++)
                {
                    string line = "";
                    line += grid[column, row].number + " ";

                    if (grid[column, row].revealed)
                        line += 1;
                    else
                        line += 0;

                    line += " ";

                    if (grid[column, row].validNumber && grid[column, row].number != 0)
                        line += 1;
                    else
                        line += 0;

                    foreach (int candidate in grid[column, row].candidates)
                    {
                        line += " ";
                        line += candidate;
                    }

                    sw.WriteLine(line);
                }
            }
            sw.Close();
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