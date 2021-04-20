using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.GameEntity.Tiles
{
    public static class TileTexturesProvider
    {
        public static Texture2D Circle { get; private set; }
        public static Texture2D Hexagon { get; private set; }
        public static Texture2D Square { get; private set; }
        public static Texture2D Diamond { get; private set; }
        public static Texture2D Triangle { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Circle = content.Load<Texture2D>("Images/" + nameof(Circle));
            Hexagon = content.Load<Texture2D>("Images/" + nameof(Hexagon));
            Square = content.Load<Texture2D>("Images/" + nameof(Square));
            Diamond = content.Load<Texture2D>("Images/" + nameof(Diamond));
            Triangle = content.Load<Texture2D>("Images/" + nameof(Triangle));
        }
    }
}
