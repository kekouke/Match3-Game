using Match3.GameEntity.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3
{
    public interface ITileContainer
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        List<Tile> DetectNodes();
        bool RemoveMatches(List<Tile> tiles);
        List<Tile> ToList();
        void SwapTiles(Tile first, Tile second);
        void MoveDown();
    }
}
