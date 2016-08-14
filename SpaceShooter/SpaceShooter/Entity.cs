using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    class Entity
    {
        protected Texture2D sprite;
        protected Rectangle bounds;

        public Texture2D Sprite
        {
            set
            {
                sprite = value;
            }
            get
            {
                return sprite;
            }
        }
        public Rectangle Bounds
        {
            set
            {
                bounds = value;
            }
            get
            {
                return bounds;
            }
        }

        public Entity(Texture2D sprite)
        {
            Sprite = sprite;
            Bounds = sprite.Bounds;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Draw(Sprite, new Vector2(Bounds.X, Bounds.Y) + camera.Position, Color.White);
        }

    }
}
