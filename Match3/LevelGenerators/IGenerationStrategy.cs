using Match3.GameEntity;
using Match3.GameEntity.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.LevelGenerators
{
    public interface IGenerationStrategy
    {
        Tile[,] GenerateTiles();
        Tile GenerateTile();
    }
}
