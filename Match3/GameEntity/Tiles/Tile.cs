using Match3.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Match3.GameEntity.Tiles
{
   public enum TileState
    {
        Nothing,
        MarkHorizontal,
        MarkVertical
    }

    public abstract class Tile : IGameEntity
    {
        private Vector2 _targetPosition;
        private float _speed = 0.5f;
        private Animation _animation = new ScaleAnimation();

        public bool inFocus = false;
        public bool IsMoving { get; private set; }

        public virtual Texture2D Texture { get; }

        public Vector2 Position { get; set; }
        public Point ArrayPosition { get; set; }
        public float Scale { get; set; } = 1f;

        public TileState State { get; set; }

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

            var center = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position + center, null, color, 0, center, Scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {
            if (IsMoving)
            {
                Move(gameTime);
            }

            _animation.Animate(gameTime);
        }

        public void Move(GameTime gameTime)
        {
            Vector2 movement = Vector2.Subtract(_targetPosition, Position);

            if (Math.Abs(movement.Length()) < 6)
            {
                Position = _targetPosition;
                IsMoving = false;
                return;
            }

            movement.Normalize();
            Position += movement * _speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void MoveTo(Vector2 targetPosition, Point arrayPosition)
        {
            _targetPosition = targetPosition;
            ArrayPosition = arrayPosition;
            IsMoving = true;
        }

        public bool CheckNeighbourhoodTo(Tile tile)
        {
            var rect1 = Rectangle;
            var rect2 = tile.Rectangle;

            return (rect1.X == rect2.X && Math.Abs(rect1.Y - rect2.Y) == Settings.TILE_SIZE.Y)
                ^ (rect1.Y == rect2.Y && Math.Abs(rect1.X - rect2.X) == Settings.TILE_SIZE.X);
        }

        public void StartAnimation()
        {
            _animation.Start(this);
        }

        public void StopAnimation()
        {
            _animation.Stop(this);
        }
    }
}
