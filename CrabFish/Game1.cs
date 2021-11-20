using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace CrabFish
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch batch;

        private Input input;

        private Texture2D board;

        private Texture2D highlight;
        private List<Projectile> projectile = new List<Projectile>(); 

        private List<Piece> pieces = new List<Piece>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            input = new Input();

            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();


        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);

            board = Content.Load<Texture2D>("chessgrille");

            highlight = new Texture2D(GraphicsDevice, 1, 1);
            highlight.SetData(new[] { Color.Red });

            for (int i = 0;i < 8;i++)
            {

                Piece pawn = new Piece("Pawn", "White", Content.Load<Texture2D>("w_pawn"), (i, 6));

                pieces.Add(pawn);

            }

            for (int i = 0;i < 2;i++)
            {

                Piece rook = new Piece("Rook", "White", Content.Load<Texture2D>("w_rook"), (i * 7, 7));

                pieces.Add(rook);

            }

            for (int i = 0;i < 2;i++)
            {

                Piece knight = new Piece("Knight", "White", Content.Load<Texture2D>("w_knight"), (i * 5 + 1, 7));

                pieces.Add(knight);

            }

            for (int i = 0; i < 2; i++)
            {

                Piece bishop = new Piece("Bishop", "White", Content.Load<Texture2D>("w_bishop"), (i * 3 + 2, 7));

                pieces.Add(bishop);

            }

            Piece whiteQueen = new Piece("Queen", "White", Content.Load<Texture2D>("w_queen"), (3, 7));
            Piece whiteKing = new Piece("King", "White", Content.Load<Texture2D>("w_king"), (4, 7));

            pieces.Add(whiteQueen);
            pieces.Add(whiteKing);

            for (int i = 0; i < 8; i++)
            {

                Piece pawn = new Piece("Pawn", "Black", Content.Load<Texture2D>("b_pawn"), (i, 1));

                pieces.Add(pawn);

            }

            for (int i = 0; i < 2; i++)
            {

                Piece rook = new Piece("Rook", "Black", Content.Load<Texture2D>("b_rook"), (i * 7, 0));

                pieces.Add(rook);

            }

            for (int i = 0; i < 2; i++)
            {

                Piece knight = new Piece("Knight", "Black", Content.Load<Texture2D>("b_knight"), (i * 5 + 1, 0));

                pieces.Add(knight);

            }

            for (int i = 0; i < 2; i++)
            {

                Piece bishop = new Piece("Bishop", "Black", Content.Load<Texture2D>("b_bishop"), (i * 3 + 2, 0));

                pieces.Add(bishop);

            }

            
            Piece blackQueen = new Piece("Queen", "Black", Content.Load<Texture2D>("b_queen"), (3, 0));
            Piece blackKing = new Piece("King", "Black", Content.Load<Texture2D>("b_king"), (4, 0));

            pieces.Add(blackQueen);
            pieces.Add(blackKing);


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            input.GetMouseState();
            
            if (input.IsClicked())
            {

                

                for (int i = 0; i < pieces.Count; i++)
                {

                    if (pieces[i].selected)
                    {

                        ValidMove(i);

                        for (int j = 0; j < projectile.Count; j++)
                        {

                            if (input.GetPos() == projectile[j].GetPos())
                            {

                                pieces[i].Move(projectile[j].GetPos());
                                pieces[i].selected = false;
                                pieces[i].firstMove = false;

                            }

                        }

                    }


                }

                for (int i = 0; i < pieces.Count; i++)
                {

                    if (input.GetPos() == pieces[i].GetPos())
                    {
                        if (pieces[i].selected == false)
                        {

                            pieces[i].selected = true;
                            return;
                        }

                    }

                    else
                    {

                        pieces[i].selected = false; 

                    }

                }


            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            batch.Begin();

            batch.Draw(board, new Rectangle(0, 0, 400, 400), Color.White);

            for (int i = 0; i < pieces.Count;i++)
            {

                if (pieces[i].selected)
                {

                    batch.Draw(highlight, pieces[i].GetRect(), Color.White * 0.5f);
                    
                    for (int j = 0; j < projectile.Count; j++)
                        batch.Draw(projectile[j].GetTexture(), projectile[j].GetRectangle(), Color.White);
                }

            }

            for (int i = 0; i < pieces.Count; i++)
            {
                batch.Draw(pieces[i].GetTexture(), pieces[i].GetRect(), Color.White);
            }

            batch.End();

            base.Draw(gameTime);
        }

        public void ValidMove(int index)
        {

            projectile.Clear();

            if (pieces[index].GetName() == "Pawn")
            {

                (int, int) checkPos;
                bool success = true; 

                if (pieces[index].GetColor() == "White")
                {

                    if (pieces[index].firstMove)
                    {
                        checkPos = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 - 2);
                        for (int i = 0; i < pieces.Count; i++)
                        {

                            if (pieces[i].GetPos() == checkPos)
                            {

                                success = false;
                                break;

                            }

                        }

                        if (success)
                        {

                            projectile.Add(new Projectile(Content.Load<Texture2D>("w-projectile"), new Rectangle(checkPos.Item1 * 50 + 12, checkPos.Item2 * 50 + 12, 25, 25), checkPos));

                        }

                    }

                    success = true; 
                    checkPos = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 - 1);

                    for (int i = 0;i < pieces.Count;i++)
                    {

                        if (pieces[i].GetPos() == checkPos)
                        {

                            success = false;
                            break;

                        }

                    }

                    if (success)
                    {

                        projectile.Add(new Projectile(Content.Load<Texture2D>("w-projectile"), new Rectangle(checkPos.Item1 * 50  + 12, checkPos.Item2 * 50 + 12, 25, 25), checkPos));

                    }

                }

                else if (pieces[index].GetColor() == "Black")
                {

                    checkPos = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 + 2);
                    success = true;
                    for (int i = 0; i < pieces.Count; i++)
                    {

                        if (pieces[i].GetPos() == checkPos)
                        {

                            success = false;
                            break;

                        }

                    }

                    if (success)
                    {

                        projectile.Add(new Projectile(Content.Load<Texture2D>("w-projectile"), new Rectangle(checkPos.Item1 * 50 + 12, checkPos.Item2 * 50 + 12, 25, 25), checkPos));

                    }

                    checkPos = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 + 1);
                    success = true; 
                    for (int i = 0; i < pieces.Count; i++)
                    {

                        if (pieces[i].GetPos() == checkPos)
                        {

                            success = false;
                            break;

                        }

                    }

                    if (success)
                    {

                        projectile.Add(new Projectile(Content.Load<Texture2D>("w-projectile"), new Rectangle(checkPos.Item1 * 50 + 12, checkPos.Item2 * 50 + 12, 25, 25), checkPos));

                    }

                }

            }

            //Debug.WriteLine(projectile.Count);

        }

    }

    public class Projectile
    {

        private Texture2D texture;
        private Rectangle rect;
        private (int, int) pos;

        public Projectile(Texture2D texture, Rectangle rect, (int, int) pos)
        {

            this.texture = texture;
            this.rect = rect;
            this.pos = pos;
        }

        public (int, int) GetPos()
        {

            return this.pos; 

        }

        public Texture2D GetTexture()
        {

            return this.texture;

        }

        public Rectangle GetRectangle()
        {

            return this.rect;

        }

    }
}
