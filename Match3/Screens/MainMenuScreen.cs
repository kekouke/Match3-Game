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
        private List<Button> _buttons = new List<Button>();

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            var playGameTexture = _content.Load<Texture2D>("Controls/Button");
            var font = _content.Load<SpriteFont>("Fonts/Font");
            var button = new Button(playGameTexture, "Start Game", font);

            var screenCenter = new Vector2((Settings.SCREEN_WIDTH - button.Rectangle.Width) / 2,
                (Settings.SCREEN_HEIGHT - button.Rectangle.Height) / 2);

            button.Position = screenCenter;
            button.Clicked += OnStartGame;

            _buttons.Add(button);
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
