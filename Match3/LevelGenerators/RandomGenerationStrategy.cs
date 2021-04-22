using Match3.GameEntity;
using Match3.GameEntity.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace Match3.LevelGenerators
{
    public class RandomGenerationStrategy : GenerationStrategyBase
    {
        public override Tile[,] GenerateTiles()
        {
            var tiles = new Tile[Settings.GRID_ROWS, Settings.GRID_COLS];

            for (int row = 0; row < Settings.GRID_ROWS; row++)
            {
                for (int col = 0; col < Settings.GRID_COLS; col++)
                {
                    var tile = GenerateTile();

                    tile.Position = GameGrid.CoordToTilePosition(new Point(row, col));
                    tile.ArrayPosition = new Point(row, col);

                    tiles[row, col] = tile;
                }
            }

            return tiles;
        }

        public override Tile GenerateTile()
        {
            var rand = new Random();

            var tile = Tiles[rand.Next(0, Tiles.Length)];

            return Activator.CreateInstance(tile.GetType()) as Tile;
        }
    }
}
