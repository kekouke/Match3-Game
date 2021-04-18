using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.GameEntity
{
    public enum TileType
    {
        Circle,
        Square,
        Triangle,
        Hexagon,
        Diamond
    }

    public class Tile : IGameEntity
    {
        private Texture2D _texture;
        private TileType _tileType;

        public Vector2 Position { get; set; } = Vector2.Zero;
        public TileType Type => _tileType;

        public Rectangle Rectangle { get => 
                new Rectangle((int)Position.X,
                    (int)Position.Y,
                    _texture.Width,
                    _texture.Height);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public Tile(Texture2D texture)
        {
            _texture = texture;
        }
    }
}
