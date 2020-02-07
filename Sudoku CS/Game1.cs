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
        private int screenWidth = 1000;
        private int screenHeight = 850;

        private KeyboardState keyboardState;
        private MouseInput mouseInput;
        private Board board;
        private Tuple<int, int> clickedBlock;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            this.Window.Title = "Sudoku";
            this.IsMouseVisible = true;

            clickedBlock = new Tuple<int, int>(-1, -1);
            mouseInput = new MouseInput(Mouse.GetState());
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
            mouseInput.Update(Mouse.GetState());
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (mouseInput.RightClick() && this.IsActive)
            {
                clickedBlock = Block.WhichBlock(mouseInput.getMouseX(), mouseInput.getMouseY());

                if (clickedBlock.Item1 != -1 && !board.isPaused)
                {
                    if (board.grid[clickedBlock.Item1, clickedBlock.Item2].number == 0)
                        board.grid[clickedBlock.Item1, clickedBlock.Item2].AddOrRemoveCandidate(mouseInput.getMouseX(), mouseInput.getMouseY());
                }
            }
            else if (mouseInput.LeftClick() && this.IsActive)
            {
                //// Check pause button
                if (mouseInput.getMouseX() >= 900 && mouseInput.getMouseX() <= 916 && mouseInput.getMouseY() >= 53 && mouseInput.getMouseY() <= 69)
                    if (board.isPaused)
                        board.isPaused = false;
                    else
                        board.isPaused = true;
                // check save button
                else if (mouseInput.getMouseX() >= 850 && mouseInput.getMouseX() <= 1000 && mouseInput.getMouseY() >= 150 && mouseInput.getMouseY() <= 200)
                {
                    board.SaveBoard();
                }
                // check new puzzle button
                else if (mouseInput.getMouseX() >= 850 && mouseInput.getMouseX() <= 1000 && mouseInput.getMouseY() >= 800 && mouseInput.getMouseY() <= 850)
                {
                    board.newPuzzle = true;
                }
                // check ny times button
                else if (mouseInput.getMouseX() >= 850 && mouseInput.getMouseX() <= 1000 && mouseInput.getMouseY() >= 700 && mouseInput.getMouseY() <= 750)
                {
                    board.newNYTimesPuzzle = true;
                }
                // check puzzle difficulty button
                else if (mouseInput.getMouseX() >= 850 && mouseInput.getMouseX() <= 1000 && mouseInput.getMouseY() >= 500 && mouseInput.getMouseY() <= 650 && (board.newPuzzle || board.newNYTimesPuzzle))
                {
                    if (mouseInput.getMouseY() >= 500 && mouseInput.getMouseY() <= 550)
                    {
                        board.difficulty = "Easy";
                    }
                    else if (mouseInput.getMouseY() > 550 && mouseInput.getMouseY() < 600)
                    {
                        board.difficulty = "Medium";
                    }
                    else if (mouseInput.getMouseY() >= 600 && mouseInput.getMouseY() <= 650)
                    {
                        board.difficulty = "Hard";
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
            else if (mouseInput.MiddleClick() && !board.isPaused && this.IsActive)
            {
                clickedBlock = Block.WhichBlock(mouseInput.getMouseX(), mouseInput.getMouseY());

                if (clickedBlock.Item1 != -1 && board.grid[clickedBlock.Item1, clickedBlock.Item2].revealed != true)
                {
                    board.grid[clickedBlock.Item1, clickedBlock.Item2].number = 0;
                    board.ValidOrInvalidNumber(clickedBlock.Item1, clickedBlock.Item2);
                }
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