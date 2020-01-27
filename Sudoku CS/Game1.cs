using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sudoku_CS
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Board board;

        private MouseState lastMState;
        private MouseState currentMState;

        private KeyboardState lastKState;
        private KeyboardState currentKState;

        private int screenWidth = 1000;
        private int screenHeight = 850;

        private Tuple<int, int> clickedBlock; // current selected block

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            this.Window.Title = "Sudoku";
            this.IsMouseVisible = true;

            clickedBlock = new Tuple<int, int>(-1, -1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            board = new Board(Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            lastMState = currentMState;
            currentMState = Mouse.GetState();

            lastKState = currentKState;
            currentKState = Keyboard.GetState();

            if (currentKState.IsKeyDown(Keys.Escape))
                Exit();

            if (currentMState.RightButton == ButtonState.Pressed && lastMState.RightButton == ButtonState.Released)
            {
                //System.Diagnostics.Debug.WriteLine(currentMouseState.X + " " + currentMouseState.Y);

                clickedBlock = Block.WhichBlock(currentMState.X, currentMState.Y);

                if (clickedBlock.Item1 != -1 && !board.isPaused)
                {

                    if (board.grid[clickedBlock.Item1, clickedBlock.Item2].number == 0)
                        board.grid[clickedBlock.Item1, clickedBlock.Item2].AddOrRemoveCandidate(currentMState.X, currentMState.Y);
                }
            }
            else if ((currentMState.LeftButton == ButtonState.Pressed && lastMState.LeftButton == ButtonState.Released))
            {
                //// Check pause button
                if (currentMState.X >= 900 && currentMState.X <= 916 && currentMState.Y >= 53 && currentMState.Y <= 69)
                    if (board.isPaused)
                        board.isPaused = false;
                    else
                        board.isPaused = true;
                // check save button
                else if (currentMState.X >= 850 && currentMState.X <= 1000 && currentMState.Y >= 150 && currentMState.Y <= 200)
                {
                    System.Diagnostics.Debug.WriteLine("saved");
                    board.SaveBoard();
                }
                // check new puzzle button
                else if (currentMState.X >= 850 && currentMState.X <= 1000 && currentMState.Y >= 800 && currentMState.Y <= 850)
                {
                    board.NewGame();
                }




                clickedBlock = Block.WhichBlock(currentMState.X, currentMState.Y);

                if (clickedBlock.Item1 != -1 && !board.isPaused && board.grid[clickedBlock.Item1, clickedBlock.Item2].revealed != true)
                {
                    board.grid[clickedBlock.Item1, clickedBlock.Item2].ChangeNumberWithMouse(currentMState.X, currentMState.Y);
                    board.ValidOrInvalidNumber(clickedBlock.Item1, clickedBlock.Item2);
                }
            }
            else if ((currentMState.MiddleButton == ButtonState.Pressed && lastMState.MiddleButton == ButtonState.Released) && !board.isPaused)
            {
                clickedBlock = Block.WhichBlock(currentMState.X, currentMState.Y);

                if (clickedBlock.Item1 != -1 && board.grid[clickedBlock.Item1, clickedBlock.Item2].revealed != true)
                { 
                    board.grid[clickedBlock.Item1, clickedBlock.Item2].number = 0;
                    board.ValidOrInvalidNumber(clickedBlock.Item1, clickedBlock.Item2);
                }
            }

            // Keyboard Input   currentBlock is (-1, -1) if mouse click was out of bounds.
            // refactor to replace tuple (x, y) to int block id 0-80 or 1-81
            //if (currentBlock.Item1 != -1)// && board.grid[currentBlock.Item1, currentBlock.Item2].background == Block.BlockBackground.Selected)
            //{
            //    if ((currentKState.IsKeyDown(Keys.NumPad1) && lastKState.IsKeyUp(Keys.NumPad1)) || (currentKState.IsKeyDown(Keys.D1) && lastKState.IsKeyUp(Keys.D1)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 1;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.NumPad2) && lastKState.IsKeyUp(Keys.NumPad2) || (currentKState.IsKeyDown(Keys.D2) && lastKState.IsKeyUp(Keys.D2)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 2;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.NumPad3) && lastKState.IsKeyUp(Keys.NumPad3) || (currentKState.IsKeyDown(Keys.D3) && lastKState.IsKeyUp(Keys.D3)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 3;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.NumPad4) && lastKState.IsKeyUp(Keys.NumPad4) || (currentKState.IsKeyDown(Keys.D4) && lastKState.IsKeyUp(Keys.D4)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 4;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.NumPad5) && lastKState.IsKeyUp(Keys.NumPad5) || (currentKState.IsKeyDown(Keys.D5) && lastKState.IsKeyUp(Keys.D5)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 5;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.NumPad6) && lastKState.IsKeyUp(Keys.NumPad6) || (currentKState.IsKeyDown(Keys.D6) && lastKState.IsKeyUp(Keys.D6)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 6;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.NumPad7) && lastKState.IsKeyUp(Keys.NumPad7) || (currentKState.IsKeyDown(Keys.D7) && lastKState.IsKeyUp(Keys.D7)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 7;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.NumPad8) && lastKState.IsKeyUp(Keys.NumPad8) || (currentKState.IsKeyDown(Keys.D8) && lastKState.IsKeyUp(Keys.D8)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 8;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.NumPad9) && lastKState.IsKeyUp(Keys.NumPad9) || (currentKState.IsKeyDown(Keys.D9) && lastKState.IsKeyUp(Keys.D0)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 9;
            //        board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
            //    }

            //    if (currentKState.IsKeyDown(Keys.Decimal) && lastKState.IsKeyUp(Keys.Decimal) || (currentKState.IsKeyDown(Keys.Back) && lastKState.IsKeyUp(Keys.Back)))
            //    {
            //        board.grid[currentBlock.Item1, currentBlock.Item2].number = 0;
            //    }
            //}

            board.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            board.Draw(spriteBatch);

            // checks to see if player won
            if (board.correctBlocks == 81)
            {
                spriteBatch.DrawString(Block.numberFont, "YOU", new Vector2(800, 400), Color.Black);
                spriteBatch.DrawString(Block.numberFont, "WON", new Vector2(800, 475), Color.Black);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
