using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Screens
{

    public enum ScreenState
    {
        Visible,
        Hide
    }
    public abstract class GameScreen
    {
        protected ContentManager _content;

        public ScreenManager ScreenManager { get; set; }
        public ScreenState State { get; set; } = ScreenState.Visible;

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        public abstract void LoadContent();

    }
}
