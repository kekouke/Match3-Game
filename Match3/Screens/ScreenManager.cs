using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Match3.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        private List<GameScreen> _screens;
        private SpriteBatch _spriteBatch;

        bool _initialized = false;

        public ScreenManager(Game game) : base(game)
        {
            _screens = new List<GameScreen>();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var screen in _screens)
            {
                screen.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            _screens[0].Update(gameTime);
        }

        public override void Initialize()
        {
            _initialized = true;

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            foreach (var screen in _screens)
            {
                screen.Draw(gameTime, _spriteBatch);
            }

            _spriteBatch.End();
        }

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;

            if (_initialized)
                screen.LoadContent();

            _screens.Add(screen);
        }
    }
}
