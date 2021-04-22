using Match3.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private ScreenManager _screenManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = Settings.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = Settings.SCREEN_HEIGHT;

            _screenManager = new ScreenManager(this);
            _screenManager.AddScreen(new MainMenuScreen());

            Components.Add(_screenManager);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
