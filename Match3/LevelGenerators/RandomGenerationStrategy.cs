using Match3.GameEntity;
using Match3.GameEntity.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Match3.LevelGenerators
{
    public class RandomGenerationStrategy : IGenerationStrategy
    {
        private Tile[] _tiles =
        {
            new Circle(),
            new Square(),
            new Triangle(),
            new Hexagon(),
            new Diamond()
        };

        public Tile[,] GenerateTiles()
        {
            var tiles = new Tile[Settings.GRID_ROWS, Settings.GRID_ROWS];

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    int row = i - 1;
                    int col = j - 1;

                    var tile = GenerateTile();

                    tile.Position = new Vector2(Settings.SCREEN_WIDTH - i * 64,
                        Settings.SCREEN_HEIGHT - j * 64);

                    tile.ArrayPosition = new Point(row, col);

                    tiles[row, col] = tile;
                }
            }

            return tiles;
        }

        public Tile GenerateTile()
        {
            var rand = new Random();

            var tile = _tiles[rand.Next(0, _tiles.Length)];

            return Activator.CreateInstance(tile.GetType()) as Tile;
        }
    }
}
