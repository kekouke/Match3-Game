using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        private List<GameScreen> _screens;
        private SpriteBatch _spriteBatch;

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

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _screens[0].Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;

            _screens.Add(screen);
        }
    }
}
