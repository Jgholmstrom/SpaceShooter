using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    class Shot : Entity
    {
        int VerticalSpeed = 20;
        Texture2D DeathSprite;
        public double TimeOfDeath = Double.MaxValue;

        public Shot(Texture2D sprite, Texture2D deathSprite, Rectangle origin) : base (sprite)
        {
            DeathSprite = deathSprite;
            bounds.X = origin.X + (origin.Width / 2) - (Bounds.Width / 2);
            bounds.Y = origin.Y - (Bounds.Height / 2);
        }

        // Initializes removal of a sprite, causing a short "death" animation
        public void InitDeath(double timeOfDeath)
        {
            TimeOfDeath = timeOfDeath;
            Sprite = DeathSprite;
            // Places the center of the new "death" sprite on the center of the old sprite, as well as updates the width and height of the shot
            Bounds = new Rectangle(Bounds.X + (Bounds.Width / 2) - (Sprite.Width / 2), Bounds.Y + (Bounds.Height / 2) - (Sprite.Height / 2), Sprite.Width, Sprite.Height);
            VerticalSpeed = 0;
        }

        public void Update(GameTime gameTime, Camera2D camera)
        {
            bounds.Y -= VerticalSpeed;
        }
    }
}
