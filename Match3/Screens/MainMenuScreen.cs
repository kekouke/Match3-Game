using Match3.GameEntity.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Match3.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private List<Button> _buttons;

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var playGameButton = new Button(buttonTexture, "Start Game", buttonFont);

            var screenCenter = new Vector2((Settings.SCREEN_WIDTH - playGameButton.Rectangle.Width) / 2,
                (Settings.SCREEN_HEIGHT - playGameButton.Rectangle.Height) / 2);

            playGameButton.Position = screenCenter;
            playGameButton.Clicked += OnStartGame;

            _buttons = new List<Button>()
            {
                playGameButton
            };
        }

        public override void UnloadContent()
        {
            _content.Unload();
        }

        private void OnStartGame(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new GameplayScreen());
            ScreenManager.RemoveScreen(this);
        }

        public override void Update(GameTime gameTime)
        {
            _buttons.ForEach(x => x.Update(gameTime));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _buttons.ForEach(x => x.Draw(gameTime, spriteBatch));
        }
    }
}
