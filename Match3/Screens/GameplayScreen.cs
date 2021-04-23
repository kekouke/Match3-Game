using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Match3.GameEntity;
using Match3.LevelGenerators;
using Match3.GameEntity.Tiles;
using System;

namespace Match3.Screens
{
    public class GameplayScreen : GameScreen
    {
        private GameGrid _gameGrid;
        private SpriteFont _spriteFont;
        private Timer _timer = new Timer();
        private bool _isEndGame;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_spriteFont, "Score: " + _gameGrid.Score,
                Vector2.Zero, Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

            spriteBatch.DrawString(_spriteFont, "Game Session: " + _timer.Value.Seconds,
                new Vector2(0, 25), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

            _gameGrid.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            TileTexturesProvider.LoadContent(_content);

            _spriteFont = _content.Load<SpriteFont>("Fonts/Font");

            StartGame();
        }

        public override void UnloadContent()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            _gameGrid.Update(gameTime);
            _timer.Update(gameTime);

            if (_isEndGame)
                EndGame();
        }

        public GameplayScreen()
        {
            _gameGrid = new GameGrid(new RandomGenerationStrategy());
        }

        private void InitializeCells()
        {
            _gameGrid.InitializeCells();
        }

        private void StartGame()
        {
            InitializeCells();

            _timer.SetTimer(new TimeSpan(0, 0, 60));
            _timer.Finished += OnTimerFinished;
            _timer.Start();
        }

        private void OnTimerFinished(object sender, EventArgs e)
        {
            _isEndGame = true;
        }

        private void EndGame()
        {
            var boardState = _gameGrid.GetState();
            if (boardState == BoardState.Initial || boardState == BoardState.TileSelected)
            {
                ScreenManager.AddScreen(new GameOverScreen());

                State = ScreenState.NonActive;
            }
        }
    }
}
