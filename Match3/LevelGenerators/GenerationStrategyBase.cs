using Match3.GameEntity.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.LevelGenerators
{
    public abstract class GenerationStrategyBase : IGenerationStrategy
    {
        public abstract Tile GenerateTile();

        public abstract Tile[,] GenerateTiles();

        protected Tile[] Tiles =
        {
            new Circle(),
            new Square(),
            new Triangle(),
            new Hexagon(),
            new Diamond()
        };
    }
}
