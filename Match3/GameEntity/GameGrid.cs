using Match3.GameEntity.Tiles;
using Match3.LevelGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Match3.GameEntity
{
    public enum BoardState
    {
        Initial,
        TileSelected,
        TileMoving
    }

    public class GameGrid : IGameEntity
    {
        private List<Tile> _tiles;
        private Tile[,] _tilesArray;
        private List<Tile> _movingTiles = new List<Tile>();

        private IGenerationStrategy _levelGenerator;

        private readonly int COLS = Settings.GRID_ROWS;
        private readonly int ROWS = Settings.GRID_COLS;

        private BoardState _currentState = BoardState.Initial;
        private Tile _selectedTile;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _tiles.ForEach(x => x.Draw(gameTime, spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
            _tiles.ForEach(x => x.Update(gameTime));

            switch (_currentState)
            {
                case BoardState.TileMoving:
                    _movingTiles = _movingTiles.Where(x => x.IsMoving).ToList();

                    if (_movingTiles.Count < 1)
                        _currentState = BoardState.Initial;

                    break;
            }
        }

        public void HandleInput(Point mousePosition)
        {
            switch (_currentState)
            {
                case BoardState.Initial:

                    _selectedTile = SelectTile(mousePosition);

                    if (_selectedTile == null)
                        return;

                    //TODO
                    _selectedTile.inFocus = true;
                    _currentState = BoardState.TileSelected;
                    break;

                case BoardState.TileSelected:

                    var tile = SelectTile(mousePosition);

                    if (_selectedTile.CheckNeighbourhoodTo(tile))
                    {
                        _currentState = BoardState.TileMoving;

                        _selectedTile.MoveTo(tile.Position);
                        tile.MoveTo(_selectedTile.Position);

                        _movingTiles.Add(_selectedTile);
                        _movingTiles.Add(tile);

                        break;
                    }

                    _currentState = BoardState.Initial;

                    _selectedTile.inFocus = false;
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
                for (int i = 0; i < _tiles.Count; i++)
                {
                    if (_tiles[i].State == TileState.MarkHorizontal ||
                        _tiles[i].State == TileState.MarkVertical)
                    {
                        var pos = _tiles[i].Position;
                        _tiles[i] = _levelGenerator.GenerateTile();
                        _tiles[i].Position = pos;
                    }
                }
            }
        }

        // TODO
        private bool DetectNodes()
        {
            bool isDetected = false;

            var tiles = new Tile[ROWS, COLS];            

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    tiles[i, j] = _tiles[i * ROWS + j];
                }
            }

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    if (j > 5 || tiles[j, i].State == TileState.MarkHorizontal)
                        continue;

                    int n = j + 1;
                    var list = new List<Tile>() { tiles[j, i] };
                    while (n < tiles.GetLength(1) && tiles[j, i].GetType().Equals(tiles[n, i].GetType()))
                    {
                        list.Add(tiles[n, i]);
                        n++;
                    }

                    if (list.Count >= 3)
                    {
                        list.ForEach(x => x.State = TileState.MarkHorizontal);
                        isDetected = true;
                    }
                }
            }

            for (int i = 0; i < tiles.GetLength(1); i++)
            {
                for (int j = 0; j < tiles.GetLength(0); j++)
                {
                    if (j > 5 || tiles[i, j].State == TileState.MarkVertical)
                        continue;

                    int n = j + 1;
                    var list = new List<Tile>() { tiles[i, j] };
                    while (n < tiles.GetLength(1) && tiles[i, j].GetType().Equals(tiles[i, n].GetType()))
                    {
                        list.Add(tiles[i, n]);
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

            var tile = _tiles.Where(x => mouseRect.Intersects(x.Rectangle)).SingleOrDefault();

            return tile;
        }


        public GameGrid(IGenerationStrategy strategy)
        {
            _levelGenerator = strategy;
        }
    }
}
