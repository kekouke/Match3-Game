using Match3.GameEntity.Tiles;
using Match3.LevelGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
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
        private Tile[,] _tiles;

        private IGenerationStrategy _levelGenerator;

        private readonly int COLS = Settings.GRID_ROWS;
        private readonly int ROWS = Settings.GRID_COLS;

        private BoardState _currentState = BoardState.Initial;
        private Tile _selectedTile;
        private Tile _swappedTile;

        private MouseState _lastMouseState;
        private MouseState _currentMouseState;
        private Point _mousePosition = new Point(-1, -1);


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
            {
                if (tile == null) continue;

                tile.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var tile in _tiles)
            {
                tile.Update(gameTime);
            }


            _currentState = GetState();

            switch (_currentState)
            {
                case BoardState.Initial:

                    ControlInput();
                    var clickedTile = SelectTile(_mousePosition);

                    if (clickedTile != null)
                    {
                        _selectedTile = clickedTile;
                        // TODO: Start animation
                    }

                    break;

                case BoardState.TileSelected:

                    ControlInput();
                    var tile = SelectTile(_mousePosition);

                    if (tile != null)
                    {
                        if (_selectedTile.CheckNeighbourhoodTo(tile))
                        {
                            _swappedTile = tile;

                            SwapTiles(_selectedTile, _swappedTile);

                            break;
                        }

                        _selectedTile = null;

                    }


                    break;

                case BoardState.TileSwapped:
                    if (!DetectNodes())
                    {
                        SwapTiles(_selectedTile, _swappedTile);
                    }

                    _swappedTile = null;
                    _selectedTile = null;

                    break;

                case BoardState.TileMoving:
                    break;

            }
        }

        public void InitializeCells()
        {
            _tiles = _levelGenerator.GenerateTiles();

            while (DetectNodes())
            {
                for (int i = 0; i < ROWS; i++)
                {
                    for (int j = 0; j < COLS; j++)
                    {
                        if (_tiles[i, j].State == TileState.MarkHorizontal ||
                            _tiles[i, j].State == TileState.MarkVertical)
                        {
                            var pos = _tiles[i, j].Position;
                            _tiles[i, j] = _levelGenerator.GenerateTile();
                            _tiles[i, j].Position = pos;
                            _tiles[i, j].ArrayPosition = new Point(i, j);
                        }
                    }
                }
            }
        }

        // TODO
        private bool DetectNodes()
        {
            bool isDetected = false;

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    if (j > 5 || _tiles[j, i].State == TileState.MarkHorizontal)
                        continue;

                    int n = j + 1;
                    var list = new List<Tile>() { _tiles[j, i] };
                    while (n < _tiles.GetLength(1) && _tiles[j, i].GetType().Equals(_tiles[n, i].GetType()))
                    {
                        list.Add(_tiles[n, i]);
                        n++;
                    }

                    if (list.Count >= 3)
                    {
                        list.ForEach(x => x.State = TileState.MarkHorizontal);
                        isDetected = true;
                    }
                }
            }

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    if (j > 5 || _tiles[i, j].State == TileState.MarkVertical)
                        continue;

                    int n = j + 1;
                    var list = new List<Tile>() { _tiles[i, j] };
                    while (n < _tiles.GetLength(1) && _tiles[i, j].GetType().Equals(_tiles[i, n].GetType()))
                    {
                        list.Add(_tiles[i, n]);
                        n++;
                    }

                    if (list.Count >= 3)
                    {
                        list.ForEach(x => x.State = TileState.MarkVertical);
                        isDetected = true;
                    }
                }
            }

            return isDetected;
        }

        private Tile SelectTile(Point position)
        {
            var mouseRect = new Rectangle(position, new Point(1, 1));

            var tiles = ConvertToList();

            var tile = tiles.Where(x => mouseRect.Intersects(x.Rectangle)).SingleOrDefault();

            return tile;
        }

        private BoardState GetState()
        {
            var tiles = ConvertToList();

            if (tiles.Any(x => x == null))
                return BoardState.HasEmptyFields;
            if (tiles.Any(x => x.IsMoving))
                return BoardState.TileMoving;
            if (_swappedTile != null)
                return BoardState.TileSwapped;
            if (_selectedTile != null)
                return BoardState.TileSelected;

            return BoardState.Initial;
        }

        private List<Tile> ConvertToList()
        {
            var result = new List<Tile>();
            for (int i = 0; i < ROWS; i++)
            {
                result.AddRange(Enumerable.Range(0, COLS).Select(x => _tiles[i, x]));
            }

            return result;
        }

        private void SwapTiles(Tile first, Tile second)
        {
            var firstPos = first.ArrayPosition;
            var secondPos = second.ArrayPosition;

            first.MoveTo(second.Position, secondPos);
            second.MoveTo(first.Position, firstPos);

            _tiles[firstPos.X, firstPos.Y] = second;
            _tiles[secondPos.X, secondPos.Y] = first;
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

        public GameGrid(IGenerationStrategy strategy)
        {
            _levelGenerator = strategy;
        }
    }
}
