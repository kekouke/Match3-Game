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

        public List<Tile> Tiles => _tiles;

        private IGenerationStrategy _levelGenerator;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var tile = _tiles[i * 8 + j];
                    tile.Draw(gameTime, spriteBatch);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public GameGrid(IGenerationStrategy strategy)
        {
            _levelGenerator = strategy;
        }

        public void InitializeCells()
        {
            _tiles = _levelGenerator.CreateTiles(64);
        }
    }
}
