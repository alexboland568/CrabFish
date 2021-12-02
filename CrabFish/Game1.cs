using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CrabFish
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch batch;

        private Input input;

        private Texture2D boardTexture;

        private Texture2D highlight;
        private List<Projectile> projectile = new List<Projectile>();

        private List<Piece> pieces = new List<Piece>();

        private PieceTypes[] board = new PieceTypes[64];

        private int selectedIndex = 0;
        

        private enum PieceTypes : int
        {

            NOPIECE = 0,PAWN = 1, KNIGHT, BISHOP, ROOK, QUEEN, KING, BLACK_PAWN, BLACK_KNIGHT, BLACK_BISHOP, BLACK_ROOK, BLACK_QUEEN, BLACK_KING
            
        }

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

        private PieceTypes Types(Piece i)
        {

            PieceTypes p = PieceTypes.NOPIECE;
            if (i.GetName() == "Pawn")
                p = PieceTypes.PAWN;
            else if (i.GetName() == "Knight")
                p = PieceTypes.KNIGHT;
            else if (i.GetName() == "Rook")
                p = PieceTypes.ROOK;
            else if (i.GetName() == "Bishop")
                p = PieceTypes.BISHOP;
            else if (i.GetName() == "Queen")
                p = PieceTypes.QUEEN;
            else if (i.GetName() == "King")
                p = PieceTypes.KING;

            if (i.GetColor() == "Black")
            {
                p += 6;

            }

            return p;

        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);

            boardTexture = Content.Load<Texture2D>("chessgrille");

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

            for (int i = 0; i < board.Length; i++)
                board[i] = PieceTypes.NOPIECE;

            foreach (Piece i in pieces)
            {

                PieceTypes p = Types(i);
                board[i.GetPos().Item1 + (i.GetPos().Item2 * 8)] = p;
                    

            }

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
                                board[pieces[i].GetPos().Item1 + (pieces[i].GetPos().Item2 * 8)] = PieceTypes.NOPIECE;
                                pieces[i].selected = false;
                                pieces[i].firstMove = false;

                                for (int k = 0; k < pieces.Count; k++)
                                {

                                    if (input.GetPos() == pieces[k].GetPos())
                                    {

                                        pieces.RemoveAt(k);
                                        break;

                                    }

                                }

                                pieces[i].Move(input.GetPos());

                                board[pieces[i].GetPos().Item1 + (pieces[i].GetPos().Item2 * 8)] = Types(pieces[i]);

                            }

                        }

                    }
                    pieces[i].Select(input.GetPos());


                    //else if (input.GetPos() == pieces[i].GetPos())
                    //{
                    //    ValidMove(i);
                    //    pieces[i].selected = true;

                    //}

                }

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            batch.Begin();

            batch.Draw(boardTexture, new Rectangle(0, 0, 400, 400), Color.White);

            for (int i = 0; i < pieces.Count;i++)
            {

                if (pieces[i].selected)
                {

                    batch.Draw(highlight, pieces[i].GetRect(), Color.White * 0.5f);
                    
                    
                }

            }

            for (int i = 0; i < pieces.Count; i++)
            {
                batch.Draw(pieces[i].GetTexture(), pieces[i].GetRect(), Color.White);
            }
            for (int i = 0;i < pieces.Count;i++)
            {
                if (pieces[i].selected)
                    for (int j = 0; j < projectile.Count; j++)
                        batch.Draw(projectile[j].GetTexture(), projectile[j].GetRectangle(), Color.White);
            }

            batch.End();

            base.Draw(gameTime);
        }

        public void AddProjectile(string color, (int, int) pos)
        {

            Texture2D texture;
            if (color == "White")
                texture = Content.Load<Texture2D>("w-projectile");
            else
                texture = Content.Load<Texture2D>("b-projectile");

            projectile.Add(new Projectile(texture, new Rectangle(pos.Item1 * 50 + 12, pos.Item2 * 50 + 12, 25, 25), pos));

        }

        public bool OutOfBounds((int, int) pos)
        {

            if (pos.Item1 > 7 || pos.Item1 < 0 || pos.Item2 > 7 || pos.Item2 < 0)
            {

                return true;
                
            }

            else
            {

                return false;

            }

        }

        public (int, int) GetBoardPos(int i)
        {

            int n = i;
            int count = 0;
            while (n >= 8)
            {

                n -= 8;
                count++;

            }
            int x = n;
            int y = count;

            return (x, y);
        }

        public List<Projectile> ValidMove(int index)
        {

            projectile.Clear();

            if (pieces[index].GetName() == "Pawn")
            {

                (int, int) checkPos, checkPos2;
                bool success = true;

                if (pieces[index].GetColor() == "White")
                {

                    if (pieces[index].firstMove)
                    {
                        checkPos = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 - 2);
                        checkPos2 = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 - 1);
                        for (int i = 0; i < pieces.Count; i++)
                        {

                            if (pieces[i].GetPos() == checkPos || pieces[i].GetPos() == checkPos2)
                            {

                                success = false;
                                break;

                            }

                        }

                        if (success)
                        {

                            AddProjectile(pieces[index].GetColor(), checkPos);

                        }

                    }

                    success = true;
                    checkPos = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 - 1);

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

                        AddProjectile(pieces[index].GetColor(), checkPos);
                    }

                    // Capture 
                    checkPos = (pieces[index].GetPos().Item1 + 1, pieces[index].GetPos().Item2 - 1);

                    for (int i = 0; i < pieces.Count; i++)
                    {

                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == "Black")
                            {

                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;
                            }

                        }

                    }

                    checkPos = (pieces[index].GetPos().Item1 - 1, pieces[index].GetPos().Item2 - 1);

                    for (int i = 0; i < pieces.Count; i++)
                    {

                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == "Black")
                            {

                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;
                            }

                        }

                    }

                }

                else if (pieces[index].GetColor() == "Black")
                {

                    if (pieces[index].firstMove)
                    {
                        checkPos = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 + 2);
                        checkPos2 = (pieces[index].GetPos().Item1, pieces[index].GetPos().Item2 + 1);

                        success = true;
                        for (int i = 0; i < pieces.Count; i++)
                        {

                            if (pieces[i].GetPos() == checkPos || pieces[i].GetPos() == checkPos2)
                            {

                                success = false;
                                break;

                            }

                        }

                        if (success)
                        {

                            AddProjectile(pieces[index].GetColor(), checkPos);

                        }
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

                        AddProjectile(pieces[index].GetColor(), checkPos);

                    }

                    checkPos = (pieces[index].GetPos().Item1 + 1, pieces[index].GetPos().Item2 + 1);

                    for (int i = 0; i < pieces.Count; i++)
                    {

                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == "White")
                            {

                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;
                            }

                        }

                    }

                    checkPos = (pieces[index].GetPos().Item1 - 1, pieces[index].GetPos().Item2 + 1);

                    for (int i = 0; i < pieces.Count; i++)
                    {

                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == "White")
                            {

                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;
                            }

                        }

                    }

                }
            }

            else if (pieces[index].GetName() == "Knight")
            {

                (int, int) checkPos;
                bool success = true;

                checkPos = (pieces[index].GetPos().Item1 + 1, pieces[index].GetPos().Item2 - 2);
                if (!OutOfBounds(checkPos)) {
                    for (int i = 0; i < pieces.Count; i++)
                    {


                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == pieces[index].GetColor())
                            {

                                success = false;
                                break;

                            }

                            else
                            {

                                success = false;
                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;

                            }

                        }
                    }
                    if (success)
                        AddProjectile(pieces[index].GetColor(), checkPos);

                }

                checkPos = (pieces[index].GetPos().Item1 - 1, pieces[index].GetPos().Item2 - 2);
                success = true;

                if (!OutOfBounds(checkPos))
                {

                    for (int i = 0; i < pieces.Count; i++)
                    {


                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == pieces[index].GetColor())
                            {

                                success = false;
                                break;

                            }

                            else
                            {

                                success = false;
                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;

                            }

                        }

                    }

                    if (success)
                        AddProjectile(pieces[index].GetColor(), checkPos);

                }
                checkPos = (pieces[index].GetPos().Item1 + 1, pieces[index].GetPos().Item2 + 2);
                success = true;

                if (!OutOfBounds(checkPos))
                {
                    for (int i = 0; i < pieces.Count; i++)
                    {


                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == pieces[index].GetColor())
                            {

                                success = false;
                                break;

                            }

                            else
                            {

                                success = false;
                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;

                            }

                        }

                    }

                    if (success)
                        AddProjectile(pieces[index].GetColor(), checkPos);
                }
                checkPos = (pieces[index].GetPos().Item1 - 1, pieces[index].GetPos().Item2 + 2);
                success = true;

                if (!OutOfBounds(checkPos))
                {
                    for (int i = 0; i < pieces.Count; i++)
                    {


                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == pieces[index].GetColor())
                            {

                                success = false;
                                break;

                            }

                            else
                            {

                                success = false;
                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;

                            }

                        }

                    }

                    if (success)
                        AddProjectile(pieces[index].GetColor(), checkPos);
                }
                checkPos = (pieces[index].GetPos().Item1 + 2, pieces[index].GetPos().Item2 - 1);
                success = true;

                if (!OutOfBounds(checkPos))
                {
                    for (int i = 0; i < pieces.Count; i++)
                    {


                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == pieces[index].GetColor())
                            {

                                success = false;
                                break;

                            }

                            else
                            {

                                success = false;
                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;

                            }

                        }

                    }

                    if (success)
                        AddProjectile(pieces[index].GetColor(), checkPos);

                }
                checkPos = (pieces[index].GetPos().Item1 + 2, pieces[index].GetPos().Item2 + 1);
                success = true;
                if (!OutOfBounds(checkPos))
                {
                    for (int i = 0; i < pieces.Count; i++)
                    {


                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == pieces[index].GetColor())
                            {

                                success = false;
                                break;

                            }

                            else
                            {

                                success = false;
                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;

                            }

                        }

                    }

                    if (success)
                        AddProjectile(pieces[index].GetColor(), checkPos);
                }

                checkPos = (pieces[index].GetPos().Item1 - 2, pieces[index].GetPos().Item2 - 1);
                success = true;

                if (!OutOfBounds(checkPos))
                {
                    for (int i = 0; i < pieces.Count; i++)
                    {


                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == pieces[index].GetColor())
                            {

                                success = false;
                                break;

                            }

                            else
                            {

                                success = false;
                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;

                            }

                        }

                    }

                    if (success)
                        AddProjectile(pieces[index].GetColor(), checkPos);
                }

                checkPos = (pieces[index].GetPos().Item1 - 2, pieces[index].GetPos().Item2 + 1);
                success = true;

                if (!OutOfBounds(checkPos))
                {

                    for (int i = 0; i < pieces.Count; i++)
                    {


                        if (pieces[i].GetPos() == checkPos)
                        {

                            if (pieces[i].GetColor() == pieces[index].GetColor())
                            {

                                success = false;
                                break;

                            }

                            else
                            {

                                success = false;
                                AddProjectile(pieces[index].GetColor(), checkPos);
                                break;

                            }

                        }

                    }

                    if (success)
                        AddProjectile(pieces[index].GetColor(), checkPos);
                }

            }

            else if (pieces[index].GetName() == "Rook")
            {

                int i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                // Left

                while (i >= 0)
                {

                    if (i % 8 == 0)
                        break;
                    i -= 1;

                    if (i < 0)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;

                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); 
                        break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); 
                        break;
                    }

                    else
                    {
                        break;
                    }

                }
                // Right
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i <= 63)
                {
                    if (i % 8 == 7)
                        break;
                    i += 1;
                    if (i > 63)
                        break;
                    //Debug.WriteLine(i);
                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }

                }
                //Up
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i >= 0)
                {

                    i -= 8;
                    if (i < 0)
                        break;
                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i <= 63)
                {

                    i += 8;

                    if (i > 63)
                        break;
                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }

                }

            }

            else if (pieces[index].GetName() == "Bishop")
            {

                int i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i >= 0)
                {
                    if (i % 8 == 7)
                        break;
                    i -= 7;
                    if (i < 0)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;

                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i >= 0)
                {
                    if (i % 8 == 0)
                        break;
                    i -= 9;
                    if (i < 0)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i <= 63)
                {
                    if (i % 8 == 0)
                        break;
                    i += 7;
                    if (i > 63)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i <= 63)
                {
                    if (i % 8 == 7)
                        break;
                    i += 9;
                    if (i > 63)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }

            }

            else if (pieces[index].GetName() == "Queen")
            {

                int i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                // Left

                while (i >= 0)
                {

                    if (i % 8 == 0)
                        break;
                    i -= 1;

                    if (i < 0)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;

                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }

                }
                // Right
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i <= 63)
                {
                    if (i % 8 == 7)
                        break;
                    i += 1;
                    if (i > 63)
                        break;
                    //Debug.WriteLine(i);
                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }

                }
                //Up
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i >= 0)
                {

                    i -= 8;
                    if (i < 0)
                        break;
                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i <= 63)
                {

                    i += 8;

                    if (i > 63)
                        break;
                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }

                }

                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);

                while (i >= 0)
                {
                    if (i % 8 == 7)
                        break;
                    i -= 7;
                    if (i < 0)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;

                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i >= 0)
                {
                    if (i % 8 == 0)
                        break;
                    i -= 9;
                    if (i < 0)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i <= 63)
                {
                    if (i % 8 == 0)
                        break;
                    i += 7;
                    if (i > 63)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }
                i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                while (i <= 63)
                {
                    if (i % 8 == 7)
                        break;
                    i += 9;
                    if (i > 63)
                        break;

                    int x = GetBoardPos(i).Item1;
                    int y = GetBoardPos(i).Item2;
                    if (board[i] == PieceTypes.NOPIECE)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }

                    else if (pieces[index].GetColor() == "White" && (int)board[i] >= 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }
                    else if (pieces[index].GetColor() == "Black" && (int)board[i] < 7)
                    {
                        AddProjectile(pieces[index].GetColor(), (x, y)); break;
                    }

                    else
                    {
                        break;
                    }
                }

            }

            else if (pieces[index].GetName() == "King")
            {

                int i = pieces[index].GetPos().Item1 + (pieces[index].GetPos().Item2 * 8);
                int x = GetBoardPos(i).Item1;
                int y = GetBoardPos(i).Item2;
                // Right
                if (!OutOfBounds(GetBoardPos(i + 1)))
                {
                    if (board[i + 1] == PieceTypes.NOPIECE || pieces[index].GetColor() == "White" && (int)board[i + 1] >= 7 || pieces[index].GetColor() == "Black" && (int)board[i + 1] < 7)
                    {

                        x = GetBoardPos(i + 1).Item1;
                        y = GetBoardPos(i + 1).Item2;

                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                }

                // Left
                if (!OutOfBounds(GetBoardPos(i - 1)))
                {
                    if (board[i - 1] == PieceTypes.NOPIECE || pieces[index].GetColor() == "White" && (int)board[i - 1] >= 7 || pieces[index].GetColor() == "Black" && (int)board[i - 1] < 7)
                    {

                        x = GetBoardPos(i - 1).Item1;
                        y = GetBoardPos(i - 1).Item2;

                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                }

                // Up
                if (!OutOfBounds(GetBoardPos(i - 8)))
                {
                    if (board[i - 8] == PieceTypes.NOPIECE || pieces[index].GetColor() == "White" && (int)board[i - 8] >= 7 || pieces[index].GetColor() == "Black" && (int)board[i - 8] < 7)
                    {

                        x = GetBoardPos(i - 8).Item1;
                        y = GetBoardPos(i - 8).Item2;

                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                }
                // Down
                if (!OutOfBounds(GetBoardPos(i + 8)))
                {
                    if (board[i + 8] == PieceTypes.NOPIECE || pieces[index].GetColor() == "White" && (int)board[i + 8] >= 7 || pieces[index].GetColor() == "Black" && (int)board[i + 8] < 7)
                    {

                        x = GetBoardPos(i + 8).Item1;
                        y = GetBoardPos(i + 8).Item2;

                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                }

                // Up Right
                if (!OutOfBounds(GetBoardPos(i - 7)))
                {
                    if (board[i - 7] == PieceTypes.NOPIECE || pieces[index].GetColor() == "White" && (int)board[i - 7] >= 7 || pieces[index].GetColor() == "Black" && (int)board[i - 7] < 7)
                    {

                        x = GetBoardPos(i - 7).Item1;
                        y = GetBoardPos(i - 7).Item2;

                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                }

                // Down Right
                if (!OutOfBounds(GetBoardPos(i + 9)))
                {
                    if (board[i + 9] == PieceTypes.NOPIECE || pieces[index].GetColor() == "White" && (int)board[i + 9] >= 7 || pieces[index].GetColor() == "Black" && (int)board[i + 9] < 7)
                    {

                        x = GetBoardPos(i + 9).Item1;
                        y = GetBoardPos(i + 9).Item2;

                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                }

                // Up Left
                if (!OutOfBounds(GetBoardPos(i - 9)))
                    if (board[i - 9] == PieceTypes.NOPIECE || pieces[index].GetColor() == "White" && (int)board[i - 8] >= 7 || pieces[index].GetColor() == "Black" && (int)board[i - 8] < 7)
                    {

                        x = GetBoardPos(i - 9).Item1;
                        y = GetBoardPos(i - 9).Item2;

                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }

                // Down Left
                if (!OutOfBounds(GetBoardPos(i + 7)))
                {
                    if (board[i + 7] == PieceTypes.NOPIECE || pieces[index].GetColor() == "White" && (int)board[i + 7] >= 7 || pieces[index].GetColor() == "Black" && (int)board[i + 7] < 7)
                    {

                        x = GetBoardPos(i + 7).Item1;
                        y = GetBoardPos(i + 7).Item2;

                        AddProjectile(pieces[index].GetColor(), (x, y));
                    }
                }

            }

            return projectile;

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

           // Debug.WriteLine(pos);
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
