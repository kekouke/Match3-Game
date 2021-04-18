using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Match3.GameEntity
{
    public class GameGrid : IGameEntity
    {
        List<Tile> _tiles;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
