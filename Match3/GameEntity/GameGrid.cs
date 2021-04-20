using Match3.GameEntity.Tiles;
using Match3.LevelGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Match3.GameEntity
{
    public class GameGrid : IGameEntity
    {
        private List<Tile> _tiles;

        private IGenerationStrategy _levelGenerator;

        private readonly int COLS = Settings.GRID_ROWS;
        private readonly int ROWS = Settings.GRID_COLS;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _tiles.ForEach(x => x.Draw(gameTime, spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
        }

        public void InitializeCells()
        {
            _tiles = _levelGenerator.GenerateTiles();

            DetectNodes();

/*            while (DetectNodes())
            {
                for (int i = 0; i < _tiles.Count; i++)
                {
                    if (_tiles[i].State == TileState.Mark)
                    {
                        var pos = _tiles[i].Position;
                        _tiles[i] = _levelGenerator.GenerateTile();
                        _tiles[i].Position = pos;
                    }
                }
            }*/
        }

        private bool DetectNodes()
        {
            bool flag = false;

            var tiles = new Tile[ROWS, COLS];

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = _tiles[i * ROWS + j];
                }
            }

            for (int i = 0; i < tiles.GetLength(1); i++)
            {
                for (int j = 0; j < tiles.GetLength(0); j++)
                {
                    if (j > 5 || tiles[j, i].State == TileState.Mark)
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
                        list.ForEach(x => x.State = TileState.Mark);
                        flag = true;
                    }
                }
            }

            for (int i = 0; i < tiles.GetLength(1); i++)
            {
                for (int j = 0; j < tiles.GetLength(0); j++)
                {
                    if (j > 5 || tiles[i, j].State == TileState.Mark)
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
                        list.ForEach(x => x.State = TileState.Mark);
                        flag = true;
                    }
                }
            }

            return flag;
        }

        public GameGrid(IGenerationStrategy strategy)
        {
            _levelGenerator = strategy;
        }
    }
}
