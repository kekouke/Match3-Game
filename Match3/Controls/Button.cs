using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Match3.Controls
{
    public class Button : IGameEntity
    {

        private Texture2D _texture;
        private SpriteFont _spriteFont;
        private bool _isHovering;

        private MouseState _currentState;
        private MouseState _previousState;

        public Rectangle Rectangle 
        { 
            get => new Rectangle((int)Position.X,
            (int)Position.Y,
            _texture.Width,
            _texture.Height);
        }

        public string Text { get; set; }
        public EventHandler Clicked;
        public Vector2 Position { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = _isHovering ? Color.Gray : Color.White;

            spriteBatch.Draw(_texture, Rectangle, color);

            if (!string.IsNullOrEmpty(Text))
            {
                Vector2 textPosition = CalculateTextPosition();
                spriteBatch.DrawString(_spriteFont, Text, textPosition, Color.Black);
            }
        }

        public void Update(GameTime gameTime)
        {
            _isHovering = false;

            var mouse = Mouse.GetState();
            _previousState = _currentState;
            _currentState = mouse;

            var mouseBounds = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseBounds.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed)
                {
                    Clicked?.Invoke(this, new EventArgs());
                }
            }
        }

        private Vector2 CalculateTextPosition()
        {
            var x = (Rectangle.X + (Rectangle.Width / 2)) - (_spriteFont.MeasureString(Text).X / 2);
            var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_spriteFont.MeasureString(Text).Y / 2);

            return new Vector2(x, y);
        }

        public Button(Texture2D texture)
        {
            _texture = texture;
        }

        public Button(Texture2D texture, SpriteFont spriteFont) : this(texture)
        {
            _spriteFont = spriteFont;
        }

        public Button(Texture2D texture, string text, SpriteFont spriteFont) : this(texture, spriteFont)
        {
            Text = text;
        }
    }
}
