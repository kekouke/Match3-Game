using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Match3.Screens
{
    public class BackgroundScreen : GameScreen
    {
        private Texture2D _backgroundTile;

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _backgroundTile = _content.Load<Texture2D>("Images/Background");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int cols = Settings.GRID_COLS;
            int rows = Settings.GRID_ROWS;

            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= cols; j++)
                {
                    var rect = new Rectangle(Settings.SCREEN_WIDTH - i * 64,
                        Settings.SCREEN_HEIGHT - j * 64,
                        _backgroundTile.Width,
                        _backgroundTile.Height);

                    spriteBatch.Draw(_backgroundTile, rect, Color.White);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
