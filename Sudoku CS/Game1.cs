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

        MouseState lastMouseState;
        MouseState currentMouseState;

        KeyboardState lastKeyboardState;
        KeyboardState currentKeyboardState;

        int screenWidth = 1200;
        int screenHeight = 1000;

        Tuple<int, int> lastSelectedBlock;
        Tuple<int, int> currentSelectedBlock;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            this.Window.Title = "Sudoku";
            this.IsMouseVisible = true;

            lastSelectedBlock = new Tuple<int, int>(-1, -1);
            currentSelectedBlock = new Tuple<int, int>(-1, -1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                //System.Diagnostics.Debug.WriteLine("X = " + currentMouseState.X + '\n' + "Y = " + currentMouseState.Y + '\n');
                //System.Diagnostics.Debug.WriteLine(currentMouseState.X + " " + currentMouseState.Y);
   
                currentSelectedBlock = Block.WhichBlock(currentMouseState.X, currentMouseState.Y);

                if (currentSelectedBlock.Item1 != -1)
                {
                    Block.BlockBackground selectedBlockBackground = board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background;

                    if (selectedBlockBackground == Block.BlockBackground.None)
                    {
                        if (lastSelectedBlock.Item1 != -1)  
                        {
                            board.grid[lastSelectedBlock.Item1, lastSelectedBlock.Item2].background = Block.BlockBackground.None;
                        }
                        board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background = Block.BlockBackground.Selected;
                        lastSelectedBlock = currentSelectedBlock;
                        board.blockSelected = true;
                    }
                    else if (selectedBlockBackground == Block.BlockBackground.Selected)
                    {
                        board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].addOrRemoveCandidate(currentMouseState.X, currentMouseState.Y);
                    }
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad1) && lastKeyboardState.IsKeyUp(Keys.NumPad1) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 1;
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad2) && lastKeyboardState.IsKeyUp(Keys.NumPad2) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 2;
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad3) && lastKeyboardState.IsKeyUp(Keys.NumPad3) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 3;
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad4) && lastKeyboardState.IsKeyUp(Keys.NumPad4) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 4;
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad5) && lastKeyboardState.IsKeyUp(Keys.NumPad5) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 5;
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad6) && lastKeyboardState.IsKeyUp(Keys.NumPad6) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 6;
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad7) && lastKeyboardState.IsKeyUp(Keys.NumPad7) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 7;
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad8) && lastKeyboardState.IsKeyUp(Keys.NumPad8) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 8;
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad9) && lastKeyboardState.IsKeyUp(Keys.NumPad9) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 9;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Decimal) && lastKeyboardState.IsKeyUp(Keys.Decimal) && board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].background == Block.BlockBackground.Selected)
            {
                board.grid[currentSelectedBlock.Item1, currentSelectedBlock.Item2].number = 0;
            }

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
