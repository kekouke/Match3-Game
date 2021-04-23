using Match3.GameEntity.Tiles;
using Match3.LevelGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Match3.GameEntity
{
    public enum BoardState
    {
        Initial,
        TileSelected,
        TileMoving,
        TileSwapped,
        HasEmptyFields
    }

    public class GameGrid : IGameEntity
    {
        public int Score { get; set; }

        private ITileContainer _tiles;
        private BoardState _currentState;
        private Tile _selectedTile;
        private Tile _swappedTile;
        private MouseState _lastMouseState;
        private MouseState _currentMouseState;
        private Point _mousePosition = new Point(-1, -1);

        public GameGrid(IGenerationStrategy strategy)
        {
            _tiles = new TileContainer(this, strategy);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _tiles.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _tiles.Update(gameTime);

            _currentState = GetState();

            switch (_currentState)
            {
                case BoardState.Initial:

                    var matches1 = _tiles.DetectNodes();

                    if (_tiles.RemoveMatches(matches1))
                        return;

                    ControlInput();
                    var clickedTile = SelectTile(_mousePosition);

                    if (clickedTile != null)
                    {
                        _selectedTile = clickedTile;
                        _selectedTile.StartAnimation();
                    }

                    break;

                case BoardState.TileSelected:

                    ControlInput();
                    var tile = SelectTile(_mousePosition);

                    if (tile != null)
                    {
                        _selectedTile.StopAnimation();

                        if (_selectedTile.CheckNeighbourhoodTo(tile))
                        {
                            _swappedTile = tile;

                            _tiles.SwapTiles(_selectedTile, _swappedTile);

                            break;
                        }
                        _selectedTile = null;
                    }


                    break;

                case BoardState.TileSwapped:

                    var matches = _tiles.DetectNodes();

                    if (!_tiles.RemoveMatches(matches))
                    {
                        _tiles.SwapTiles(_selectedTile, _swappedTile);
                    }

                    _swappedTile = null;
                    _selectedTile = null;

                    break;

                case BoardState.HasEmptyFields:
                    _tiles.MoveDown();
                    break;

                case BoardState.TileMoving:
                    break;

            }
        }

        public BoardState GetState()
        {
            var tiles = _tiles.ToList();

            if (tiles.Any(x => x == null))
                return BoardState.HasEmptyFields;
            if (tiles.Any(x => x.IsMoving || x.IsRemoving))
                return BoardState.TileMoving;
            if (_swappedTile != null)
                return BoardState.TileSwapped;
            if (_selectedTile != null)
                return BoardState.TileSelected;

            return BoardState.Initial;
        }

        private void ControlInput()
        {
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (_lastMouseState.LeftButton == ButtonState.Pressed &&
                _currentMouseState.LeftButton == ButtonState.Released)
            {
                _mousePosition = _currentMouseState.Position;
                return;
            }

            _mousePosition = new Point(-1, -1);
        }

        private Tile SelectTile(Point position)
        {
            var mouseRect = new Rectangle(position, new Point(1, 1));

            var tiles = _tiles.ToList();

            var tile = tiles.Where(x => mouseRect.Intersects(x.Rectangle)).FirstOrDefault();

            return tile;
        }
    }
}
