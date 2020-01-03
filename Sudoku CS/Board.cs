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
        Random r = new Random();

        // Easy = 36+ blocks revealed. Medium = 27-36 blocks revealed. Hard = 19-26 blocks revealed.
        public int[,] blockGrid = new int[9, 9];

        public int[,] winningBlockGrid = new int[9, 9];


        public Texture2D backgroundImage;
        public Vector2 backgroundPosition;

        SpriteFont font;

        int blocksRevealed = 38;

        public Board(ContentManager Content)
        {
            content = Content;

            Array.Clear(blockGrid, 0, blockGrid.Length);

            NewWinningGrid();

            // 38 revealed
            // check for collisions
            for (int i = 0; i < blocksRevealed; i++)
            {
                int x = r.Next(0, 9);
                int y = r.Next(0, 9);

                blockGrid[x, y] = winningBlockGrid[x, y];
            }




            System.Diagnostics.Debug.WriteLine(blockGrid[5, 5]);


            backgroundPosition = new Vector2(50, 50);

            LoadContent();
        }

        public void NewWinningGrid()
        {
            HashSet<int> randomNumbersRow = new HashSet<int>();

            for (int i = 1; i < 10; i++)
                while (!randomNumbersRow.Add(r.Next(1, 10))) ;


            // WORKING HERE
            int count = 0;

            foreach (var item in randomNumbersRow)
            {
                winningBlockGrid[count, 0] = item;
                count += 1;
            }





            //foreach (var item in randomNumbers3)
            //{
            //    System.Diagnostics.Debug.WriteLine(item);
            //}
            //System.Diagnostics.Debug.WriteLine(randomNumbers3);
        







            for (int i = 1; i < 10; i++)
            {

                //randomNumbers.Add(i, r.Next(0, 1000));
                //System.Diagnostics.Debug.WriteLine(randomNumbers);
            }


            //for (int i = 0; i < 9; i++)
            //{
            //    for(int j = 0; j < 9; j++)
            //    {
            //        // delete
            //        winningBlockGrid[i, j] = 5;
            //    }
            //}
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
