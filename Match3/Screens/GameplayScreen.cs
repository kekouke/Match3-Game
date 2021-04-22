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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _gameGrid.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            TileTexturesProvider.LoadContent(_content);

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
