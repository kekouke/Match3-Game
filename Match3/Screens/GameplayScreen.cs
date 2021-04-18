using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Match3.GameEntity;

namespace Match3.Screens
{
    public class GameplayScreen : GameScreen
    {
        Texture2D texture;
        Texture2D triangle;
        Texture2D hexagon;
        Texture2D circle;

        private GameGrid _grid;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _grid.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            triangle = _content.Load<Texture2D>("Images/Triangle");
            hexagon = _content.Load<Texture2D>("Images/Hexagon");
            circle = _content.Load<Texture2D>("Images/Circle");


        }

        public override void Update(GameTime gameTime)
        {
        }

        public GameplayScreen()
        {
            _grid = new GameGrid();
        }
    }
}
