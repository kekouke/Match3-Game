using Microsoft.Xna.Framework;

namespace Match3
{
    public static class Settings
    {
        public static int SCREEN_WIDTH { get; } = 512;

        public static int SCREEN_HEIGHT { get; } = 666;

        public static int GRID_COLS { get; } = 8;
        public static int GRID_ROWS { get; } = 8;

        public static Vector2 TILE_SIZE { get; } = new Vector2(64, 64);

    }
}
