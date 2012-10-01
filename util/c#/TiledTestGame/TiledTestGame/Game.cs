using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tiled;


namespace TiledTestGame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;

        TiledMap orthoMap, isoMap;
        Vector2 orthoMapPosition, isoMapPosition;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            orthoMapPosition = new Vector2(0, 0);

            spriteBatch.PrerenderMap(orthoMap);

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            orthoMap = Content.Load<TiledMap>("map");
            isoMap = Content.Load<TiledMap>("isomap2");
            TilesetManager.LoadTextures(Content);
            Console.WriteLine("+++++++++++++++++++++++++++++++");
            Console.WriteLine(orthoMap);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ProcessKeyboard();

            base.Update(gameTime);
        }

        private void ProcessKeyboard()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up))
                orthoMapPosition.Y -= 5;
            if (keyState.IsKeyDown(Keys.Down))
                orthoMapPosition.Y += 5;
            if (keyState.IsKeyDown(Keys.Left))
                orthoMapPosition.X -= 5;
            if (keyState.IsKeyDown(Keys.Right))
                orthoMapPosition.X += 5;
            if (keyState.IsKeyDown(Keys.W))
                isoMapPosition.Y -= 5;
            if (keyState.IsKeyDown(Keys.S))
                isoMapPosition.Y += 5;
            if (keyState.IsKeyDown(Keys.A))
                isoMapPosition.X -= 5;
            if (keyState.IsKeyDown(Keys.D))
                isoMapPosition.X += 5;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            spriteBatch.Begin();
            spriteBatch.Draw(orthoMap, orthoMapPosition);
            spriteBatch.Draw(isoMap, isoMapPosition);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
