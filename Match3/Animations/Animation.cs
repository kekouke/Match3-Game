using Match3.GameEntity.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Animations
{
    public abstract class Animation
    {
        public abstract void Start(Tile entity);
        public abstract void Stop(Tile entity);
        public abstract void Animate(GameTime gameTime);
    }
}
