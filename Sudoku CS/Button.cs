using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Sudoku_CS
{
    class Button
    {
        private Texture2D texture;
        private SpriteFont font;
        private Vector2 position;

        private string text;
        private Color textColor;

        private bool mouseOver;

        public Button(string textureName, string fontName, Vector2 _position, string _text, Color _textColor, ContentManager content)
        {
            texture = content.Load<Texture2D>(textureName);
            font = content.Load<SpriteFont>(fontName);
            position = _position;
            text = _text;
            textColor = _textColor;
        }

        public Button(string textureName, Vector2 _position, ContentManager content)
        {
            texture = content.Load<Texture2D>(textureName);
            position = _position;
        }

        public bool Clicked(int _x, int _y)
        {
            if (_x >= position.X && _x <= position.X + texture.Width && _y >= position.Y && _y <= position.Y + texture.Height)
                return true;
            else
                return false;
        }

        public void Update(int _x, int _y)
        {
            if (_x >= position.X && _x <= position.X + texture.Width && _y >= position.Y && _y <= position.Y + texture.Height)
                mouseOver = true;
            else
                mouseOver = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (mouseOver)
            {
                spriteBatch.Draw(texture, position, Color.Gray);
            }
            else
                spriteBatch.Draw(texture, position);

            if (!string.IsNullOrEmpty(text))
            {
                float x = position.X + (texture.Width / 2) - (font.MeasureString(text).X / 2);
                float y = position.Y + (texture.Height / 2) - (font.MeasureString(text).Y / 2);

                spriteBatch.DrawString(font, text, new Vector2(x, y), textColor);
            }
        }
    }
}
