using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    class EnemyShot : Entity
    {
        int VerticalSpeed;
        public double TimeOfDeath = Double.MaxValue;

        public EnemyShot(Texture2D sprite, Rectangle origin, int verticalSpeed) : base (sprite)
        {
            bounds.X = origin.X + (origin.Width / 2) - (Bounds.Width / 2);
            bounds.Y = origin.Y + (Bounds.Height / 2);
            VerticalSpeed = verticalSpeed;
        }

        public void Update(GameTime gameTime, Camera2D camera)
        {
            bounds.Y += VerticalSpeed;
        }
    }
}
