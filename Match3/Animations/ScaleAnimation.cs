using Match3.GameEntity.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace Match3.Animations
{
    public class ScaleAnimation : Animation
    {
        private Tile target;
        private bool _isActive;

        private float maxScale = 1.2f;
        private float minScale = 0.7f;
        private float _speed = 0.1f;


        public override void Start(Tile entity)
        {
            _isActive = true;
            target = entity;
        }

        public override void Stop(Tile entity)
        {
            _isActive = false;
            target.Scale = 1f;
        }

        public override void Animate(GameTime gameTime)
        {
            if (!_isActive)
                return;

            if (target.Scale <= minScale || target.Scale >= maxScale)
                _speed *= -1;


            target.Scale += _speed;


        }
    }
}
