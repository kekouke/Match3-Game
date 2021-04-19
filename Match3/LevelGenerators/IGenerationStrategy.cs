using Match3.GameEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.LevelGenerators
{
    public interface IGenerationStrategy
    {
        List<Tile> CreateTiles(int qty);
        Tile GenerateTile();
    }
}
