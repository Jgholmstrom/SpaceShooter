using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    class Player : Entity
    {
        bool blinking = false;
        public bool Invuln = false;
        double TimeOfHit = Double.MaxValue;
        Texture2D SpriteL, SpriteR;
        public int Speed = 12;
        public int Lives = 3;
        public int Score;
        enum Sprites
        {
            Neutral,
            Left,
            Right
        }
        Sprites CurrentSprite = Sprites.Neutral;

        public Player(Texture2D sprite, Texture2D spriteL, Texture2D spriteR, Vector2 scrSize) : base (sprite)
        {
            SpriteL = spriteL;
            SpriteR = spriteR;
            bounds.X = Convert.ToInt32((scrSize.X / 2) - (sprite.Width / 2));
            bounds.Y = Convert.ToInt32((scrSize.Y / 1.5f) - (sprite.Height / 2));
        }

        public void Input(KeyboardState kbState, GamePadState gpState)
        {
            if (kbState.IsKeyDown(Keys.Up) || gpState.IsButtonDown(Buttons.LeftThumbstickUp))
            {
                bounds.Y -= Speed;
            }
            if (kbState.IsKeyDown(Keys.Down) || gpState.IsButtonDown(Buttons.LeftThumbstickDown))
            {
                bounds.Y += Speed;
            }
            if (kbState.IsKeyDown(Keys.Left) || gpState.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                bounds.X -= Speed;
                CurrentSprite = Sprites.Left;
            }
            if (kbState.IsKeyDown(Keys.Right) || gpState.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                bounds.X += Speed;
                CurrentSprite = Sprites.Right;
            }
            // Causes the active player sprite to be the "neutral stance" whenever the player is stationary, rather than saving the stance of the last direction the player turned
            if (((kbState.IsKeyUp(Keys.Left) && kbState.IsKeyUp(Keys.Right)) || (kbState.IsKeyDown(Keys.Left) && kbState.IsKeyDown(Keys.Right))) && gpState.ThumbSticks.Left == new Vector2(0,0))
            {
                CurrentSprite = Sprites.Neutral;
            }
            
        }

        public void InitHit(double timeOfHit)
        {
                TimeOfHit = timeOfHit;
                Lives--;
                Invuln = true;
                Score -= 1000;
        }

        public void Update(GameTime gameTime)
        {
            if (TimeOfHit < Double.MaxValue)
            {
                if ((gameTime.TotalGameTime.TotalMilliseconds - TimeOfHit) > 2000)
                {
                    TimeOfHit = Double.MaxValue;
                    Invuln = false;
                    blinking = false;
                }
                else if ((gameTime.TotalGameTime.TotalMilliseconds - TimeOfHit) % 500 < 250)
                {
                    
                    blinking = true;
                }
                else
                {
                    blinking = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            if (!blinking)
            {
                if (CurrentSprite == Sprites.Neutral)
                    spriteBatch.Draw(Sprite, new Vector2(Bounds.X, Bounds.Y) + camera.Position, Color.White);
                else if (CurrentSprite == Sprites.Left)
                    spriteBatch.Draw(SpriteL, new Vector2(Bounds.X, Bounds.Y) + camera.Position, Color.White);
                else if (CurrentSprite == Sprites.Right)
                    spriteBatch.Draw(SpriteR, new Vector2(Bounds.X, Bounds.Y) + camera.Position, Color.White);
            }
        }
    }
}
