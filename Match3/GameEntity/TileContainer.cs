using Match3.GameEntity.Tiles;
using Match3.LevelGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Match3.GameEntity
{
    public class TileContainer : ITileContainer
    {
        private Tile[,] _tiles;
        private GameGrid _gameGrid;
        private bool _tilesCollapse;

        private IGenerationStrategy _levelGenerator;

        private readonly int COLS = Settings.GRID_ROWS;
        private readonly int ROWS = Settings.GRID_COLS;

        public TileContainer(GameGrid gameGrid, IGenerationStrategy strategy)
        {
            _gameGrid = gameGrid;
            _levelGenerator = strategy;
            InitializeCells();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var tile in _tiles)
            {
                tile?.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
            {
                tile?.Draw(gameTime, spriteBatch);
            }
        }

        public List<Tile> DetectNodes()
        {
            var matches = new List<Tile>();

            for (int i = 0; i < ROWS; i++)
            {
                matches.AddRange(DetectLineMatches(GetTilesRow(i)));
                matches.AddRange(DetectLineMatches(GetTilesCol(i)));
            }

            return matches.Distinct().ToList();
        }

        public bool RemoveMatches(List<Tile> tiles)
        {
            _gameGrid.Score += tiles.Count;

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


        public List<Tile> ToList()
        {
            var result = new List<Tile>();
            for (int i = 0; i < ROWS; i++)
            {
                result.AddRange(Enumerable.Range(0, COLS).Select(x => _tiles[i, x]));
            }

            return result;
        }

        public void SwapTiles(Tile first, Tile second)
        {
            var firstPos = first.ArrayPosition;
            var secondPos = second.ArrayPosition;

            first.MoveTo(second.Position, secondPos);
            second.MoveTo(first.Position, firstPos);

            _tiles[firstPos.X, firstPos.Y] = second;
            _tiles[secondPos.X, secondPos.Y] = first;
        }

        public void MoveDown()
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
                var tiles = ToList().Where(x => x != null && x.IsMoving == true).ToList();

                if (tiles.Count == 0)
                {
                    FillBoard();
                    _tilesCollapse = false;
                }
            }
        }

        public static Vector2 CoordToTilePosition(Point point)
        {
            int y_startPoint = 154;

            var position = new Vector2(Settings.TILE_SIZE.X * point.Y,
                y_startPoint + Settings.TILE_SIZE.Y * point.X);

            return position;
        }

        private void InitializeCells()
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

            ToList().ForEach(x => x.Removed += OnTileRemove);
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

        private void OnTileRemove(object sender, EventArgs e)
        {
            var tile = sender as Tile;

            if (tile != null)
            {
                _tiles[tile.ArrayPosition.X, tile.ArrayPosition.Y] = null;
            }
        }
    }
}
