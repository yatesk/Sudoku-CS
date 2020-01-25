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

        Board board;

        MouseState lastMState;
        MouseState currentMState;

        KeyboardState lastKState;
        KeyboardState currentKState;

        int screenWidth = 1200;
        int screenHeight = 1000;

        Tuple<int, int> lastBlock; // last selected block
        Tuple<int, int> currentBlock; // current selected block

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            this.Window.Title = "Sudoku";
            this.IsMouseVisible = true;

            lastBlock = new Tuple<int, int>(-1, -1);
            currentBlock = new Tuple<int, int>(-1, -1);
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

            if (currentMState.LeftButton == ButtonState.Pressed && lastMState.LeftButton == ButtonState.Released)
            {
                //System.Diagnostics.Debug.WriteLine(currentMouseState.X + " " + currentMouseState.Y);

                // Check pause button
                if (currentMState.X >= 550 && currentMState.X <= 566 && currentMState.Y >= 4 && currentMState.Y <= 20)
                    if (board.isPaused)
                        board.isPaused = false;
                    else
                        board.isPaused = true;


                currentBlock = Block.WhichBlock(currentMState.X, currentMState.Y);

                if (currentBlock.Item1 != -1)
                { 
                    if (board.grid[currentBlock.Item1, currentBlock.Item2].background == Block.BlockBackground.None)
                    {
                        if (lastBlock.Item1 != -1)  
                        {
                            board.grid[lastBlock.Item1, lastBlock.Item2].background = Block.BlockBackground.None;
                        }
                        board.grid[currentBlock.Item1, currentBlock.Item2].background = Block.BlockBackground.Selected;
                        lastBlock = currentBlock;
                        board.blockSelected = true;
                    }
                    else if (board.grid[currentBlock.Item1, currentBlock.Item2].background == Block.BlockBackground.Selected)
                    {
                        board.grid[currentBlock.Item1, currentBlock.Item2].AddOrRemoveCandidate(currentMState.X, currentMState.Y);
                    }
                }
            }
            else if ((currentMState.RightButton == ButtonState.Pressed && lastMState.RightButton == ButtonState.Released))
            {
                currentBlock = Block.WhichBlock(currentMState.X, currentMState.Y);

                if (board.grid[currentBlock.Item1, currentBlock.Item2].background == Block.BlockBackground.Selected)
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].ChangeNumberWithMouse(currentMState.X, currentMState.Y);
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }
            }

            // Keyboard Input   currentBlock is (-1, -1) if mouse click was out of bounds.
            // refactor to replace tuple (x, y) to int block id 0-80 or 1-81
            if (currentBlock.Item1 != -1 && board.grid[currentBlock.Item1, currentBlock.Item2].background == Block.BlockBackground.Selected)
            {
                if ((currentKState.IsKeyDown(Keys.NumPad1) && lastKState.IsKeyUp(Keys.NumPad1)) || (currentKState.IsKeyDown(Keys.D1) && lastKState.IsKeyUp(Keys.D1)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 1;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.NumPad2) && lastKState.IsKeyUp(Keys.NumPad2) || (currentKState.IsKeyDown(Keys.D2) && lastKState.IsKeyUp(Keys.D2)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 2;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.NumPad3) && lastKState.IsKeyUp(Keys.NumPad3) || (currentKState.IsKeyDown(Keys.D3) && lastKState.IsKeyUp(Keys.D3)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 3;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.NumPad4) && lastKState.IsKeyUp(Keys.NumPad4) || (currentKState.IsKeyDown(Keys.D4) && lastKState.IsKeyUp(Keys.D4)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 4;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.NumPad5) && lastKState.IsKeyUp(Keys.NumPad5) || (currentKState.IsKeyDown(Keys.D5) && lastKState.IsKeyUp(Keys.D5)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 5;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.NumPad6) && lastKState.IsKeyUp(Keys.NumPad6) || (currentKState.IsKeyDown(Keys.D6) && lastKState.IsKeyUp(Keys.D6)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 6;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.NumPad7) && lastKState.IsKeyUp(Keys.NumPad7) || (currentKState.IsKeyDown(Keys.D7) && lastKState.IsKeyUp(Keys.D7)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 7;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.NumPad8) && lastKState.IsKeyUp(Keys.NumPad8) || (currentKState.IsKeyDown(Keys.D8) && lastKState.IsKeyUp(Keys.D8)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 8;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.NumPad9) && lastKState.IsKeyUp(Keys.NumPad9) || (currentKState.IsKeyDown(Keys.D9) && lastKState.IsKeyUp(Keys.D0)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 9;
                    board.ValidOrInvalidNumber(currentBlock.Item1, currentBlock.Item2);
                }

                if (currentKState.IsKeyDown(Keys.Decimal) && lastKState.IsKeyUp(Keys.Decimal) || (currentKState.IsKeyDown(Keys.Back) && lastKState.IsKeyUp(Keys.Back)))
                {
                    board.grid[currentBlock.Item1, currentBlock.Item2].number = 0;
                }
            }


            // checks to see if player won
            // refactor
            System.Diagnostics.Debug.WriteLine(board.correctBlocks);

            if (board.correctBlocks == 81)
            {
                System.Diagnostics.Debug.WriteLine("YOU WON");
            }

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

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
