using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    class Enemy : Entity
    {
        Random r = new Random();
        int VerticalSpeed;
        public bool hasShot = false;
        int rX;
        public int rY;

        public Enemy(Texture2D sprite, int verticalSpeed) : base (sprite)
        {
            rX = r.Next(0, 800-Bounds.Width); // Generates a random position for the enemy to spawn at
            rY = r.Next(0, (600 - Bounds.Height)); // Generates a random position at which the enemy will shoot
            Bounds = new Rectangle(rX, 0, Bounds.Width, Bounds.Height);
            VerticalSpeed = verticalSpeed;
        }

        public void Update(GameTime gameTime)
        {
            bounds.Y += VerticalSpeed;
        }
    }
}
