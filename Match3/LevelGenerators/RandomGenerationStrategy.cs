using Match3.GameEntity;
using System;
using System.Collections.Generic;

namespace Match3.LevelGenerators
{
    public class RandomGenerationStrategy : IGenerationStrategy
    {
        public List<Tile> CreateTiles(int qty)
        {
            var result = new List<Tile>();
            for (int i = 0; i < qty; i++)
            {
                var tile = GenerateTile();
                result.Add(tile);
            }

            return result;
        }

        public Tile GenerateTile()
        {
            var tile = new Tile();
            var rand = new Random();

            var array = System.Enum.GetValues(typeof(TileType));
            int index = rand.Next(0, array.Length);
            tile.Type = (TileType)array.GetValue(index);

            return tile;
        }
    }
}
