using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.GameEntity.Tiles
{
    public class Diamond : Tile 
    {
        public override Texture2D Texture => TileTexturesProvider.Diamond;
    }
}
