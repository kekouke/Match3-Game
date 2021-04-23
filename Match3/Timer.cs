using Microsoft.Xna.Framework;
using System;

namespace Match3
{
    public class Timer
    {
        private TimeSpan _timer = new TimeSpan();
        private bool _isActive;

        public TimeSpan Value { get => _timer; private set => _timer = value; }

        public EventHandler Finished;

        public void Update(GameTime gameTime)
        {
            if (_isActive)
            {
                _timer -= gameTime.ElapsedGameTime;

                if (_timer.Seconds <= 0)
                {
                    StopTimer();
                    Finished?.Invoke(this, null);
                }
            }
        }

        private void StopTimer()
        {
            _isActive = false;
        }

        public void SetTimer(TimeSpan time)
        {
            _timer = time;
            _isActive = true;
        }
    }
}
