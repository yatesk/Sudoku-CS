﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;


namespace Sudoku_CS
{
    class GameState : State
    {
        private MouseInput mouseInput;
        private Board board;
        private Tuple<int, int> clickedBlock;


        public GameState(Game1 game, ContentManager content, string puzzleSource, string puzzleDifficulty) : base(game, content)
        {

            clickedBlock = new Tuple<int, int>(-1, -1);
            mouseInput = new MouseInput(Mouse.GetState());


            board = new Board(content, puzzleSource, puzzleDifficulty);


            LoadContent();
        }



        public override void LoadContent()
        {       
        }


        public override void Update(GameTime gameTime)
        {
            mouseInput.Update(Mouse.GetState());

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();

            if (mouseInput.RightClick() && game.IsActive)
            {
                clickedBlock = Block.WhichBlock(mouseInput.getMouseX(), mouseInput.getMouseY());

                if (clickedBlock.Item1 != -1 && !board.isPaused)
                {
                    if (board.grid[clickedBlock.Item1, clickedBlock.Item2].number == 0)
                        board.grid[clickedBlock.Item1, clickedBlock.Item2].AddOrRemoveCandidate(mouseInput.getMouseX(), mouseInput.getMouseY());
                }
            }
            else if (mouseInput.LeftClick() && game.IsActive)
            {
                //// Check pause button
                if (board.pauseButton.Clicked(mouseInput.getMouseX(), mouseInput.getMouseY()))
                    if (board.isPaused)
                        board.isPaused = false;
                    else
                        board.isPaused = true;
                // check save button
                else if (board.showCandidatesButton.Clicked(mouseInput.getMouseX(), mouseInput.getMouseY()))
                {
                    board.ShowCandidates();

                }
                else if (board.savePuzzleButton.Clicked(mouseInput.getMouseX(), mouseInput.getMouseY()))
                {
                    board.SaveBoard();
                }
                // check new puzzle button
                else if (board.newPuzzleButton.Clicked(mouseInput.getMouseX(), mouseInput.getMouseY()))
                {
                    board.newPuzzle = true;
                }
                // check ny times button
                else if (board.nyTimesButton.Clicked(mouseInput.getMouseX(), mouseInput.getMouseY()))
                {
                    board.newNYTimesPuzzle = true;
                }
                // check puzzle difficulty button
                else if ((board.newPuzzle || board.newNYTimesPuzzle) && mouseInput.getMouseX() >= 850 && mouseInput.getMouseX() <= 1000 && mouseInput.getMouseY() >= 500 && mouseInput.getMouseY() <= 650)
                {
                    if (mouseInput.getMouseY() <= 550)
                    {
                        board.puzzleDifficulty = "Easy";
                    }
                    else if (mouseInput.getMouseY() <= 600)
                    {
                        board.puzzleDifficulty = "Medium";
                    }
                    else if (mouseInput.getMouseY() <= 650)
                    {
                        board.puzzleDifficulty = "Hard";
                    }

                    if (board.newPuzzle)
                    {
                        board.NewPuzzle();
                        board.newPuzzle = false;
                    }
                    else if (board.newNYTimesPuzzle)
                    {
                        board.GetNYTimesPuzzle();  // refactor
                        board.newNYTimesPuzzle = false;
                    }
                }

                clickedBlock = Block.WhichBlock(mouseInput.getMouseX(), mouseInput.getMouseY());

                if (clickedBlock.Item1 != -1 && !board.isPaused && board.grid[clickedBlock.Item1, clickedBlock.Item2].revealed != true)
                {
                    board.grid[clickedBlock.Item1, clickedBlock.Item2].ChangeNumberWithMouse(mouseInput.getMouseX(), mouseInput.getMouseY());
                    board.ValidOrInvalidNumber(clickedBlock.Item1, clickedBlock.Item2);
                }
            }
            else if (mouseInput.MiddleClick() && !board.isPaused && game.IsActive)
            {
                clickedBlock = Block.WhichBlock(mouseInput.getMouseX(), mouseInput.getMouseY());

                if (clickedBlock.Item1 != -1 && board.grid[clickedBlock.Item1, clickedBlock.Item2].revealed != true)
                {
                    board.grid[clickedBlock.Item1, clickedBlock.Item2].number = 0;
                    board.ValidOrInvalidNumber(clickedBlock.Item1, clickedBlock.Item2);
                }
            }

            board.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            board.Draw(spriteBatch);
        }
    }
}
