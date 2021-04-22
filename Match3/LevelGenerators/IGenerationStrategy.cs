using Match3.GameEntity.Tiles;

namespace Match3.LevelGenerators
{
    public interface IGenerationStrategy
    {
        Tile[,] GenerateTiles();
        Tile GenerateTile();
    }
}
