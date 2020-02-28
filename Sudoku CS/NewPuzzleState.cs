using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sudoku_CS
{
    class NewPuzzleState : State
    {
        private List<Button> buttons;

        MouseState previousMouseState;
        MouseState currentMouseState;

        private SpriteFont titleFont;
        private SpriteFont labelFont;

        private string title;

        public NewPuzzleState(Game1 game, ContentManager content) : base(game, content)
        {
            title = "New Puzzle";
            LoadContent();
        }

        public override void LoadContent()
        {
            titleFont = content.Load<SpriteFont>("titleFont");
            labelFont = content.Load<SpriteFont>("candidateFont");


            buttons = new List<Button>
            {
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) - 275, (Game1.screenHeight / 2) - 75), "Easy", Color.Green, content),
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) - 275, (Game1.screenHeight / 2) + 25), "Medium", Color.Blue, content),
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) - 275, (Game1.screenHeight / 2) + 125), "Hard", Color.Red, content),

                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) + 25, (Game1.screenHeight / 2) - 75), "Simple", Color.Green, content),
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) + 25, (Game1.screenHeight / 2) + 25), "Easy", Color.Blue, content),
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) + 25, (Game1.screenHeight / 2) + 125), "Intermediate", Color.Orange, content),
                new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) + 25, (Game1.screenHeight / 2) + 225), "Expert", Color.Red, content)

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
                            case "Easy":
                                if (button.getTextColor() == Color.Green) // NY Times
                                    game.ChangeState(new GameState(game, content));
                                else if(button.getTextColor() == Color.Blue) // QQ Wong
                                    game.ChangeState(new GameState(game, content));
                                
                                break;

                            case "QQ Wing":


                                break;


                                //case "1player":
                                //    game.ChangeState(new GameState(game, content));
                                //    break;

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
            float x = Game1.screenWidth / 2 - (titleFont.MeasureString(title).X / 2);

            spriteBatch.DrawString(titleFont, title, new Vector2(x, 50), Color.Black);

            spriteBatch.DrawString(labelFont, "NY Times", new Vector2((Game1.screenWidth / 2) - 275, (Game1.screenHeight / 2) - 200), Color.Black);
            spriteBatch.DrawString(labelFont, "QQ Wing", new Vector2((Game1.screenWidth / 2) + 25, (Game1.screenHeight / 2) - 200), Color.Black);


            //new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) - 275, (Game1.screenHeight / 2) - 200), "NY Times", Color.Black, content),
                //new Button("button250-100", "buttonFont", new Vector2((Game1.screenWidth / 2) + 25, (Game1.screenHeight / 2) - 200), "QQ Wing", Color.Black, content),


            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
