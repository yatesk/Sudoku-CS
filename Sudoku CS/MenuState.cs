using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sudoku_CS
{
    class MenuState : State
    {
        private List<Button> buttons;

        private MouseState previousMouseState;
        private MouseState currentMouseState;

        private SpriteFont buttonFont;

        private string gameName;

        public MenuState(Game1 game, ContentManager content) : base(game, content)
        {
            gameName = "Sudoku";
            LoadContent();
        }

        public override void LoadContent()
        {
            buttonFont = content.Load<SpriteFont>("titleFont");

            buttons = new List<Button>
            {
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) - 125, (Game1.screenHeight / 2) - 100), "New Puzzle", Color.Black, content),
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) - 125, (Game1.screenHeight / 2)), "Load Puzzle", Color.Black, content),
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) - 125, (Game1.screenHeight / 2) + 100), "Quit", Color.Black, content)
            };
        }

        public override void Update(GameTime gameTime)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                foreach (var button in buttons)
                {
                    if (button.Clicked(currentMouseState.X, currentMouseState.Y))
                    {
                        switch (button.getText())
                        {
                            case "New Puzzle":
                                game.ChangeState(new NewPuzzleState(game, content));
                                return;

                            case "Load Puzzle":
                                game.ChangeState(new GameState(game, content, "Load Save"));
                                return;

                            case "Quit":
                                game.Exit();
                                return;
                        }
                    }
                }
            }
            else
            {
                foreach (var button in buttons)
                {
                    button.Update(currentMouseState.X, currentMouseState.Y);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float x = Game1.screenWidth / 2 - (buttonFont.MeasureString(gameName).X / 2);

            spriteBatch.DrawString(buttonFont, gameName, new Vector2(x, 100), Color.Black);

            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
