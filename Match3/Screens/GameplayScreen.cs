using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Match3.GameEntity;
using System.Collections.Generic;
using Match3.LevelGenerators;
using Match3.GameEntity.Tiles;
using Microsoft.Xna.Framework.Input;

namespace Match3.Screens
{
    public class GameplayScreen : GameScreen
    {
        private GameGrid _gameGrid;

        private MouseState _lastMouseState;
        private MouseState _currentMouseState;


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _gameGrid.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            TileTexturesProvider.LoadContent(_content);

            InitializeCells();
        }

        public override void Update(GameTime gameTime)
        {
            ControlInput();

            _gameGrid.Update(gameTime);
        }

        public GameplayScreen()
        {
            _gameGrid = new GameGrid(new RandomGenerationStrategy());
        }

        private void InitializeCells()
        {
            _gameGrid.InitializeCells();
        }

        private void ControlInput()
        {
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (_lastMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Released)
            {
                _gameGrid.HandleInput(_currentMouseState.Position);
            }
        }
    }
}
