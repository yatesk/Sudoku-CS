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
        public static ContentManager content;
        private readonly Random r = new Random();
        private readonly int boardMargin = 35;

        private int correctBlocks = 0;
        public Block[,] grid = new Block[9, 9];

        private Texture2D backgroundImage;
        private Vector2 backgroundPosition;

        private Texture2D difficultiesImage;

        private float timer = 0f;
        public bool isPaused = false;

        public bool newPuzzle = false;
        public bool newNYTimesPuzzle = false;

        public Button newPuzzleButton;
        public Button nyTimesButton;

        public Button pauseButton;
        public Button showCandidatesButton;

        public Button savePuzzleButton;

        public string puzzleSource;
        public string puzzleDifficulty;

        public Board(ContentManager _content, string _source = "", string _difficulty = "")
        {
            puzzleSource = _source;
            puzzleDifficulty = _difficulty;

            content = _content;
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

                    System.Diagnostics.Debug.WriteLine((i*84) + boardMargin + gridMarginX);
                    grid[i, j] = new Block(new Vector2((i * 84) + boardMargin + gridMarginX, boardMargin + (j * 84) + gridMarginY), false, 0);
                }
            }

            savePuzzleButton = new Button("save", new Vector2(700, 0), content, false);
            pauseButton = new Button("pause", new Vector2(455, 4), content);
            showCandidatesButton = new Button("showCandidate", new Vector2(775, 0), content, true);

            NewPuzzle();
            LoadContent();
        }

        public Board(ContentManager _content, string _savedGame)
        {
            if (_savedGame == "Load Save")
            {
                content = _content;
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

                savePuzzleButton = new Button("save", new Vector2(700, 0), content, false);
                pauseButton = new Button("pause", new Vector2(455, 4), content);
                showCandidatesButton = new Button("showCandidate", new Vector2(775, 0), content, true);

                LoadContent();
                LoadBoardFromSavedTextfile();
            }
        }

        public void NewPuzzle()
        {
            ClearBoard();

            if (puzzleSource == "NY Times")
            {
                GetNYTimesPuzzle();
            }
            else if (puzzleSource == "QQ Wing")
            {
                String line;
                List<char[]> level = new List<char[]>();

                FileStream fsSource = new FileStream(puzzleDifficulty + ".txt", FileMode.OpenOrCreate, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsSource))
                {
                    int randomNumber = r.Next(0, 21);

                    // Skip to random puzzle #0-21
                    for (int i = 0; i < randomNumber * 11; i++)
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
        }

        public void LoadContent()
        {
            backgroundImage = content.Load<Texture2D>("background");
            difficultiesImage = content.Load<Texture2D>("difficulties");

            Block.LoadContent();
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
            {
                // Displays "PAUSED" at the center of the game board.
                float x = boardMargin + grid[4,4].position.X + (Block.Size / 2) - (Block.numberFont.MeasureString("PAUSED").X / 2);
                float y = boardMargin + grid[4,4].position.Y + (Block.Size / 2) - (Block.numberFont.MeasureString("PAUSED").Y / 2);

                spriteBatch.DrawString(Block.numberFont, "PAUSED", new Vector2(x, y), Color.Black);
            }

            //// Draw timer
            int time = (int)timer;
           
            if (time % 60 < 10)
            {
                spriteBatch.DrawString(Block.candidateFont, (time / 60).ToString() + ":0" + (time % 60).ToString(), new Vector2(405, 0), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(Block.candidateFont, (time / 60).ToString() + ":" + (time % 60).ToString(), new Vector2(405, 0), Color.Black);
            }

            // checks to see if player won
            if (correctBlocks == 81)
            {
                spriteBatch.DrawString(Block.numberFont, "YOU", new Vector2(400, 400), Color.Black);
                spriteBatch.DrawString(Block.numberFont, "WON", new Vector2(400, 475), Color.Black);
            }

            spriteBatch.DrawString(Block.candidateFont, puzzleDifficulty, new Vector2(boardMargin, 0), Color.Black);
            spriteBatch.DrawString(Block.candidateFont, puzzleSource, new Vector2(boardMargin + 150, 0), Color.Black);

            pauseButton.Draw(spriteBatch);

            savePuzzleButton.Draw(spriteBatch);
            showCandidatesButton.Draw(spriteBatch);
        }

        public void ClearBoard()
        {
            timer = 0f;
            correctBlocks = 0;
            showCandidatesButton.toggle = false;

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

        //public void NewPuzzle()
        //{
        //    ClearBoard();

        //    String line;
        //    List<char[]> level = new List<char[]>();

        //    FileStream fsSource = new FileStream(puzzleDifficulty + ".txt", FileMode.OpenOrCreate, FileAccess.Read);
        //    using (StreamReader sr = new StreamReader(fsSource))
        //    {
        //        int randomNumber = r.Next(0, 21);

        //        // Skip to random puzzle #0-21
        //        for (int i = 0; i < randomNumber*11; i++)
        //        {
        //            line = sr.ReadLine();
        //        }

        //        for (int i = 0; i < 9; i++)
        //        {
        //            line = sr.ReadLine();

        //            for (int j = 0; j < 9; j++)
        //            {
        //                if (line[j] != '.')
        //                {
        //                    grid[i, j].number = (int)char.GetNumericValue(line[j]);
        //                    grid[i, j].revealed = true;
        //                    grid[i, j].validNumber = true;
        //                    correctBlocks++;
        //                }
        //                else
        //                {
        //                    grid[i, j].number = 0;
        //                    grid[i, j].revealed = false;
        //                    grid[i, j].validNumber = false;
        //                }
        //            }
        //        }
        //    }
        //}

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

            // MOVE???
            if (showCandidatesButton.toggle)
                ShowCandidates();
            
        }

        public void LoadBoardFromSavedTextfile()
        {
            String line;
            List<char[]> level = new List<char[]>();

            FileStream fsSource = new FileStream("savedBoard.txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fsSource))
            {
                puzzleSource = sr.ReadLine();
                puzzleDifficulty = sr.ReadLine();
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

        public void ShowCandidates()
        {
            // refactor
            // which candidates to remove
            HashSet<int> candidates = new HashSet<int>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    candidates.Clear();
                    grid[i, j].AddAllCandidates();

                    for (int column = 0; column < 9; column++)
                    {
                        if (grid[column, j].number != 0)
                            candidates.Add(grid[column, j].number);
                    }

                    for (int row = 0; row < 9; row++)
                    {
                        if (grid[i, row].number != 0)
                            candidates.Add(grid[i, row].number);
                    }

                    int[] subGridStartingCoords = FindSubGrid(i, j);

                    for (int column = 0; column < 3; column++)
                    {
                        for (int row = 0; row < 3; row++)
                        {
                            if (grid[column + subGridStartingCoords[0], row + subGridStartingCoords[1]].number != 0)
                                candidates.Add(grid[column + subGridStartingCoords[0], row + subGridStartingCoords[1]].number);
                        }
                    }

                    foreach (var candidate in candidates)
                    {
                        grid[i, j].candidates.Remove(candidate);
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


            if (puzzleDifficulty == "Easy")
            {
                LoadNYTimesPuzzle(htmlCode.Substring(indexEasy + 10, 161));
            }
            else if (puzzleDifficulty == "Medium")
            {
                LoadNYTimesPuzzle(htmlCode.Substring(indexMedium + 10, 161));
            }
            else if (puzzleDifficulty == "Hard")
            {
                LoadNYTimesPuzzle(htmlCode.Substring(indexHard + 10, 161));
            }
        }

        public void LoadNYTimesPuzzle(string puzzle)
        {
            ClearBoard();

            string[] puzzleNumbers = puzzle.Split(',');
            int index = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int number = Int32.Parse(puzzleNumbers[index]);

                    if (number != 0)
                    {
                        grid[j, i].number = number;  
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

            sw.WriteLine(puzzleSource);
            sw.WriteLine(puzzleDifficulty);
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