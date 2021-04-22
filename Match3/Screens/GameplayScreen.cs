using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Match3.GameEntity;
using Match3.LevelGenerators;
using Match3.GameEntity.Tiles;

namespace Match3.Screens
{
    public class GameplayScreen : GameScreen
    {
        private GameGrid _gameGrid;
        private SpriteFont _spriteFont;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(_spriteFont, "Score: " + _gameGrid.Score,
                Vector2.Zero, Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

            _gameGrid.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            TileTexturesProvider.LoadContent(_content);

            _spriteFont = _content.Load<SpriteFont>("Fonts/Font");

            InitializeCells();
        }
        public override void UnloadContent()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            _gameGrid.Update(gameTime);
        }

        public GameplayScreen()
        {
            _gameGrid = new GameGrid(new RandomGenerationStrategy());
        }

        private void InitializeCells()
        {
            _gameGrid.InitializeCells();
        }
    }
}
