using Match3.GameEntity.Tiles;
using Match3.LevelGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        public int Score { get; private set; }

        private Tile[,] _tiles;

        private IGenerationStrategy _levelGenerator;

        private readonly int COLS = Settings.GRID_ROWS;
        private readonly int ROWS = Settings.GRID_COLS;

        private BoardState _currentState;
        private Tile _selectedTile;
        private Tile _swappedTile;

        private bool _tilesCollapse;
        private MouseState _lastMouseState;
        private MouseState _currentMouseState;
        private Point _mousePosition = new Point(-1, -1);

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
            {
                tile?.Draw(gameTime, spriteBatch);
            }

        }

        public void Update(GameTime gameTime)
        {
            foreach (var tile in _tiles)
            {
                tile?.Update(gameTime);
            }

            _currentState = GetState();

            switch (_currentState)
            {
                case BoardState.Initial:

                    var matches1 = DetectNodes();

                    if (RemoveMatches(matches1))
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

                            SwapTiles(_selectedTile, _swappedTile);

                            break;
                        }
                        _selectedTile = null;
                    }


                    break;

                case BoardState.TileSwapped:

                    var matches = DetectNodes();

                    if (!RemoveMatches(matches))
                    {
                        SwapTiles(_selectedTile, _swappedTile);
                    }

                    _swappedTile = null;
                    _selectedTile = null;

                    break;

                case BoardState.HasEmptyFields:
                    MoveDown();
                    break;

                case BoardState.TileMoving:
                    break;

            }
        }

        public static Vector2 CoordToTilePosition(Point point)
        {
            int y_startPoint = 154;

            var position = new Vector2(Settings.TILE_SIZE.X * point.Y,
                y_startPoint + Settings.TILE_SIZE.Y * point.X);

            return position;
        }

        public void InitializeCells()
        {
            _tiles = _levelGenerator.GenerateTiles();

            var matches = DetectNodes();
            while (matches.Count > 0)
            {

                for (int i = 0; i < matches.Count; i++)
                {
                    var arrayPosition = matches[i].ArrayPosition;
                    var pos = matches[i].Position;

                    _tiles[arrayPosition.X, arrayPosition.Y] = _levelGenerator.GenerateTile();
                    _tiles[arrayPosition.X, arrayPosition.Y].Position = pos;
                    _tiles[arrayPosition.X, arrayPosition.Y].ArrayPosition = arrayPosition;
                }

                matches = DetectNodes();
            }

            ConvertToList().ForEach(x => x.Removed += OnTileRemove);
        }

        private List<Tile> DetectNodes()
        {
            var matches = new List<Tile>();

            for (int i = 0; i < ROWS; i++)
            {
                matches.AddRange(DetectLineMatches(GetTilesRow(i)));
                matches.AddRange(DetectLineMatches(GetTilesCol(i)));
            }

            return matches.Distinct().ToList();
        }
        
        private List<Tile> GetTilesRow(int index)
        {
            if (index < 0 || index >= ROWS)
                return null;

            return Enumerable.Range(0, ROWS).Select(x => _tiles[index, x]).ToList();
        }

        private List<Tile> GetTilesCol(int index)
        {
            if (index < 0 || index >= COLS)
                return null;

            return Enumerable.Range(0, COLS).Select(x => _tiles[x, index]).ToList();
        }

        private List<Tile> DetectLineMatches(List<Tile> tiles)
        {
            var matches = new List<Tile>();

            for (int i = 0; i < tiles.Count; i++)
            {
                var list = new List<Tile>() { tiles[i] };

                int n = i + 1;
                while (n < tiles.Count && tiles[i].GetType().Equals(tiles[n].GetType()))
                {
                    list.Add(tiles[n]);
                    n++;
                }

                if (list.Count >= 3)
                    matches.AddRange(list);
            }

            return matches;
        }

        private Tile SelectTile(Point position)
        {
            var mouseRect = new Rectangle(position, new Point(1, 1));

            var tiles = ConvertToList();

            var tile = tiles.Where(x => mouseRect.Intersects(x.Rectangle)).FirstOrDefault();

            return tile;
        }

        public BoardState GetState()
        {
            var tiles = ConvertToList();

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

        private bool RemoveMatches(List<Tile> tiles)
        {
            Score += tiles.Count;

            if (tiles.Count != 0)
            {
                tiles.ForEach(x =>
                {
                    var pos = x.ArrayPosition;
                    _tiles[pos.X, pos.Y].IsRemoving = true;
                });
                return true;
            }

            return false;
        }

        private void MoveDown()
        {
            if (!_tilesCollapse)
            {
                _tilesCollapse = true;

                for (int i = ROWS - 1; i >= 0; i--)
                {
                    for (int j = 0; j < COLS; j++)
                    {
                        if (_tiles[i, j] == null)
                        {
                            var tiles = Enumerable.Range(0, i).Select(x => _tiles[x, j]).Reverse().ToList();
                            var tile = tiles.Where(x => x != null).FirstOrDefault();

                            if (tile != null)
                            {
                                _tiles[tile.ArrayPosition.X, tile.ArrayPosition.Y] = null;

                                tile.MoveTo(CoordToTilePosition(new Point(i, j)),
                                                new Point(i, j));

                                _tiles[i, j] = tile;
                            }
                        }
                    }
                }
            }
            else
            {
                var tiles = ConvertToList().Where(x => x != null && x.IsMoving == true).ToList();

                if (tiles.Count == 0)
                {
                    FillBoard();
                    _tilesCollapse = false;
                }
            }
        }

        private void FillBoard()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    var tile = _tiles[i, j];

                    if (tile != null)
                        continue;

                    tile = _levelGenerator.GenerateTile();
                    tile.ArrayPosition = new Point(i, j);
                    tile.Position = CoordToTilePosition(tile.ArrayPosition);
                    tile.Removed += OnTileRemove;

                    _tiles[i, j] = tile;
                }
            }
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

        private void OnTileRemove(object sender, EventArgs e)
        {
            var tile = sender as Tile;

            if (tile != null)
            {
                _tiles[tile.ArrayPosition.X, tile.ArrayPosition.Y] = null;
            }
        }

        public GameGrid(IGenerationStrategy strategy)
        {
            _levelGenerator = strategy;
        }
    }
}
