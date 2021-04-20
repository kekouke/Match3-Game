using Microsoft.Xna.Framework.Graphics;

namespace Match3.GameEntity.Tiles
{
    public class Square : Tile
    {
        public override Texture2D Texture => TileTexturesProvider.Square;
    }
}
