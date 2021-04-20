using Match3.GameEntity.Tiles;
using Match3.LevelGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Match3.GameEntity
{
    public class GameGrid : IGameEntity
    {
        private List<Tile> _tiles;

        private IGenerationStrategy _levelGenerator;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int cols = Settings.GRID_COLS;
            int rows = Settings.GRID_ROWS;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var tile = _tiles[i * 8 + j];
                    tile.Draw(gameTime, spriteBatch);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void InitializeCells()
        {
            _tiles = _levelGenerator.GenerateTiles();
        }

        public GameGrid(IGenerationStrategy strategy)
        {
            _levelGenerator = strategy;
        }
    }
}
