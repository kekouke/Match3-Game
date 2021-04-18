using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Match3.Screens
{
    public class GameplayScreen : GameScreen
    {
        Texture2D texture;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ScreenManager.Game.GraphicsDevice.Viewport.Bounds, Color.White);
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            // TODO: Add logic for loading content
            texture = _content.Load<Texture2D>("Images/Background");
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
