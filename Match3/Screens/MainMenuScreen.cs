using Match3.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private List<Button> _buttons = new List<Button>();
        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            var playGameTexture = _content.Load<Texture2D>("Controls/Button");
            var font = _content.Load<SpriteFont>("Fonts/Font");
            var button = new Button(playGameTexture, "Start Game", font);


            button.Position = new Vector2((Settings.SCREEN_WIDTH - button.Rectangle.Width) / 2,
                (Settings.SCREEN_HEIGHT - button.Rectangle.Height) / 2);
            button.Clicked += OnStartGame;

            _buttons.Add(button);
        }

        // TODO
        private void OnStartGame(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameplayScreen());
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var button in _buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
        }
    }
}
