using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrabFish
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch batch;

        private Texture2D board;

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

            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();


        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);

            board = Content.Load<Texture2D>("Board1");

            Piece pawn = new Piece("Pawn", "White", Content.Load<Texture2D>("Figure1"), (0, 7));

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            batch.Begin();

            batch.Draw(board, new Rectangle(0, 0, 400, 400), Color.White);

            batch.End();

            base.Draw(gameTime);
        }
    }
}
