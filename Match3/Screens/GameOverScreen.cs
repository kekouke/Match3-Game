using Match3.GameEntity.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Match3.Screens
{
    public class GameOverScreen : GameScreen
    {
        private Sprite _background;
        private Button _button;


        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            var viewport = ScreenManager.Game.GraphicsDevice.Viewport;

            var backgroundTexture = _content.Load<Texture2D>("Images/GameOverBackground");
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var textFont = _content.Load<SpriteFont>("Fonts/Font");

            _background = new Sprite(backgroundTexture)
            {
                Position = new Vector2(viewport.Width / 2 - backgroundTexture.Width / 2,
                                       viewport.Height / 2 - backgroundTexture.Height / 2)
            };

            _button = new Button(buttonTexture, "OK", textFont)
            {
                Position = _background.Rectangle.Center.ToVector2() -
                new Vector2(buttonTexture.Width / 2, buttonTexture.Height - 100)
            };

            _button.Clicked += OnClicked;
        }

        private void OnClicked(object sender, EventArgs e)
        {
            ScreenManager.Clear();
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        public override void UnloadContent()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            _button.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background.Texture, _background.Rectangle, Color.White);
            _button.Draw(gameTime, spriteBatch);
        }
    }
}
