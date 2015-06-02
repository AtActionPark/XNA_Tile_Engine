using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TileEngine
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private float updateFPS, drawFPS;

        private TileMap map;
        private Camera camera= new Camera();
        private Input input = new Input();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            graphics.PreferredBackBufferHeight = (int)Static.ScreenSize.Y;
            graphics.PreferredBackBufferWidth = (int)Static.ScreenSize.X;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);
            map = new TileMap(input, (int)Static.MapSize.X, (int)Static.MapSize.Y);
            camera.Position = new Vector2(Static.ScreenSize.X / 2, Static.ScreenSize.Y / 2);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();
            camera.Update(input);
            map.Update();
            if (gameTime.ElapsedGameTime.Milliseconds != 0)
                updateFPS = 1000 / gameTime.ElapsedGameTime.Milliseconds;
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (gameTime.ElapsedGameTime.Milliseconds != 0)
                drawFPS = 1000 / gameTime.ElapsedGameTime.Milliseconds;

            this.Window.Title = string.Concat("Update : " + updateFPS + " fps   " + "Draw : " + drawFPS + " fps");

            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.GetTransformation());

            map.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            if(input.IsWheelButtonDown)
                spriteBatch.Draw(Art.Cursor2, new Vector2(input.mouseState.X, input.mouseState.Y), Color.White);
            else
                spriteBatch.Draw(Art.Cursor, new Vector2(input.mouseState.X, input.mouseState.Y), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
