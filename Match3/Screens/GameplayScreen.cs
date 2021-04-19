using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Match3.GameEntity;
using System.Collections.Generic;
using Match3.LevelGenerators;

namespace Match3.Screens
{
    public class GameplayScreen : GameScreen
    {
        private Dictionary<TileType, Texture2D> _tileTextures = new Dictionary<TileType, Texture2D>();

        private GameGrid _gameGrid;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _gameGrid.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            foreach (var enumName in System.Enum.GetNames(typeof(TileType)))
            {
                var texture = _content.Load<Texture2D>("Images/" + enumName);
                var enumState = (TileType) System.Enum.Parse(typeof(TileType), enumName);

                _tileTextures.Add(enumState, texture);
            }

            InitializeCells();
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

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    int row = i - 1;
                    int col = j - 1;

                    var tile = _gameGrid.Tiles[row * 8 + col];

                    tile.Position = new Vector2(Settings.SCREEN_WIDTH - i * 64,
                        Settings.SCREEN_HEIGHT - j * 64);

                    tile.Texture = _tileTextures[tile.Type];
                }
            }
        }
    }
}
