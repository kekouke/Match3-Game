using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Match3.GameEntity.Tiles
{
    public enum TileType
    {
        Circle,
        Square,
        Triangle,
        Hexagon,
        Diamond
    }

    public enum TileState
    {
        Nothing,
        Mark
    }

    public abstract class Tile : IGameEntity
    {
        public virtual Texture2D Texture { get; }

        private TileType _tileType;

        public Vector2 Position { get; set; }

        public TileState State;
        public TileType Type { get => _tileType; set => _tileType = value; }

        public Rectangle Rectangle
        {
            get => new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                Texture.Width,
                Texture.Height
            );
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = State == TileState.Nothing ? Color.White : Color.Black;
            spriteBatch.Draw(Texture, Rectangle, color);
        }

        public void Update(GameTime gameTime)
        {
        }

    }
}
