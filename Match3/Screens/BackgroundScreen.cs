using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Match3.GameEntity;

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

        public override void UnloadContent()
        {
            _content.Unload();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Settings.GRID_ROWS; i++)
            {
                for (int j = 0; j < Settings.GRID_COLS; j++)
                {
                    Vector2 vector2 = TileContainer.CoordToTilePosition(new Point(i, j));
                    spriteBatch.Draw(_backgroundTile, vector2, Color.White);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
