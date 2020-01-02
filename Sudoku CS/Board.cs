using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Sudoku_CS
{
    class Board
    {
        ContentManager content;

        public int[,] blockGrid = new int[9, 9];


        public Texture2D backgroundImage;
        public Vector2 backgroundPosition;

        SpriteFont font;

        public Board(ContentManager Content)
        {
            content = Content;

            //for (int i = 0; i < blockGrid.Length; i++)
            //    for (int j = 0; j < blockGrid.Length; j++)
            //        blockGrid[i, j] = 0;

            Array.Clear(blockGrid, 0, blockGrid.Length);

            // tests
            blockGrid[5, 5] = 3;
            blockGrid[0, 7] = 7;
            blockGrid[6, 6] = 8;
            blockGrid[1, 2] = 4;

            backgroundPosition = new Vector2(50, 50);

            LoadContent();
        }

        public void LoadContent()
        {
            backgroundImage = content.Load<Texture2D>("background");

            font = content.Load<SpriteFont>("Font");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, backgroundPosition);


            for (int i = 0; i < blockGrid.GetLength(0); i++)
                for (int j = 0; j < blockGrid.GetLength(1); j++)
                    if (blockGrid[i,j] != 0)
                    {
                        spriteBatch.DrawString(font, blockGrid[i, j].ToString(), new Vector2(50 + (i*100) + 25, 50 + (j*100) ), Color.Black);
                    }
        }
    }
}
