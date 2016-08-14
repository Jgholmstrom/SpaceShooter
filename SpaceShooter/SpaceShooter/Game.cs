using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        //Basic
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera2D camera;
        SpriteFont font;
        KeyboardState prevKbState;
        GamePadState prevGpState;
        Rectangle bounds;
        Random r = new Random();
        int level = 1;
        int spawnFreq = 1000;
        int enemyVerticalSpeed = 2;
        int enemyShotSpeed = 3;
        int displayWidth = 800;
        int displayHeight = 600;
        double spawnTime;
        enum ScreenState
        {
            StartScreen,
            PauseScreen,
            GameScreen,
            GameOverScreen
        }
        ScreenState currentScreen = ScreenState.StartScreen;

        //Player
        Player player;
        Texture2D playerSprite;
        Texture2D playerSpriteL;
        Texture2D playerSpriteR;

        //Shot
        List<Shot> shotList = new List<Shot>();
        Texture2D shotGreen;
        Texture2D shotGreenExplode;
        double lastShot;

        //Enemy
        List<Enemy> enemyList = new List<Enemy>();
        Texture2D enemySprite;

        //Enemy shot
        List<EnemyShot> enemyShotList = new List<EnemyShot>();
        Texture2D shotRed;

        //Background
        Texture2D bgSprite;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = displayHeight;
            graphics.PreferredBackBufferWidth = displayWidth;
            bounds = new Rectangle(0, 0, displayWidth, displayHeight);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            camera = new Camera2D(GraphicsDevice.Viewport);
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");

            //Player
            playerSprite = Content.Load<Texture2D>("player");
            playerSpriteL = Content.Load<Texture2D>("playerLeft");
            playerSpriteR = Content.Load<Texture2D>("playerRight");
            

            //Shot
            shotGreen = Content.Load<Texture2D>("laserGreen");
            shotGreenExplode = Content.Load<Texture2D>("laserGreenShot");

            //Enemy
            enemySprite = Content.Load<Texture2D>("enemyShip");

            //Enemy Shot
            shotRed = Content.Load<Texture2D>("laserRedShot");

            //Background
            bgSprite = Content.Load<Texture2D>("starBackground");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            GamePadState gpState = GamePad.GetState(PlayerIndex.One);
            switch (currentScreen)
            {
                case ScreenState.StartScreen:
                    {
                        UpdateStartScreen(kbState, gpState);
                        break;
                    }
                case ScreenState.PauseScreen:
                    {
                        UpdatePauseScreen(kbState, gpState);
                        break;
                    }
                case ScreenState.GameScreen:
                    {
                        UpdateGameScreen(gameTime, kbState, gpState);
                        break;
                    }
                case ScreenState.GameOverScreen:
                    {
                        UpdateGameOverScreen(kbState, gpState);
                        break;
                    }
            }
            prevKbState = kbState;
            prevGpState = gpState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (currentScreen)
            {
                case ScreenState.StartScreen:
                    {
                        DrawStartScreen();
                        break;
                    }
                case ScreenState.PauseScreen:
                    {
                        DrawPauseScreen();
                        break;
                    }
                case ScreenState.GameScreen:
                    {
                        DrawGameScreen();
                        break;
                    }
                case ScreenState.GameOverScreen:
                    {
                        DrawGameOverScreen();
                        break;
                    }
            }
            base.Draw(gameTime);
        }

        private void UpdateStartScreen(KeyboardState kbState, GamePadState gpState)
        {
            if ((kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter)) || (gpState.IsButtonDown(Buttons.Start) && prevGpState.IsButtonUp(Buttons.Start)))
            {
                NewGame();
                currentScreen = ScreenState.GameScreen;
            }
        }

        private void UpdatePauseScreen(KeyboardState kbState, GamePadState gpState)
        {
            if ((kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter)) || (gpState.IsButtonDown(Buttons.Start) && prevGpState.IsButtonUp(Buttons.Start)))
            {
                currentScreen = ScreenState.GameScreen;
            }
        }

        private void UpdateGameScreen(GameTime gameTime, KeyboardState kbState, GamePadState gpState)
        {
            
            //Game Over
            if (player.Lives < 0)
            {
                currentScreen = ScreenState.GameOverScreen;
            }

            //Constant camera/object movement
            camera.moveUp(1);

            //Update level
            if (player.Score > 10000 * level)
            {
                level++;
                spawnFreq -= 100;
                enemyVerticalSpeed++;
                enemyShotSpeed += 2;
            }

            //Player input
            player.Input(kbState, gpState);
            if ((kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter)) || (gpState.IsButtonDown(Buttons.Start) && prevGpState.IsButtonUp(Buttons.Start)))
            {
                currentScreen = ScreenState.PauseScreen;
            }
            //Player Collision
            if (player.Bounds.Y < bounds.Top)
            {
                player.Bounds = new Rectangle(player.Bounds.X, player.Bounds.Y + player.Speed, player.Bounds.Width, player.Bounds.Height);
            }
            if (player.Bounds.Y + player.Bounds.Height > bounds.Bottom)
            {
                player.Bounds = new Rectangle(player.Bounds.X, player.Bounds.Y - player.Speed, player.Bounds.Width, player.Bounds.Height);
            }
            if (player.Bounds.X < bounds.Left)
            {
                player.Bounds = new Rectangle(player.Bounds.X + player.Speed, player.Bounds.Y, player.Bounds.Width, player.Bounds.Height);
            }
            if (player.Bounds.X + player.Bounds.Width > bounds.Right)
            {
                player.Bounds = new Rectangle(player.Bounds.X - player.Speed, player.Bounds.Y, player.Bounds.Width, player.Bounds.Height);
            }

            //Player shooting
            if ((kbState.IsKeyDown(Keys.Space) || gpState.IsButtonDown(Buttons.A)) && gameTime.TotalGameTime.TotalMilliseconds - lastShot > 200 && player.Invuln == false) //Shoots every 200ms as long as the player is vulnerable
            {
                shotList.Add(new Shot(shotGreen, shotGreenExplode, player.Bounds));
                lastShot = gameTime.TotalGameTime.TotalMilliseconds;
            }
            player.Update(gameTime);
            //Summon enemy
            if (spawnTime + spawnFreq < gameTime.TotalGameTime.TotalMilliseconds)
            {
                enemyList.Add(new Enemy(enemySprite, enemyVerticalSpeed));
                spawnTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            //Shot logic
            for (int i = shotList.Count - 1; i >= 0; i--)
            {
                Shot s = shotList[i];
                if (s.TimeOfDeath + 100 < gameTime.TotalGameTime.TotalMilliseconds) // Removes the shot's "death" animation 100ms after it hit an enemy
                {
                    shotList.Remove(s);
                }
                else if (!s.Bounds.Intersects(bounds)) // Removes a shot that has missed entirely and is out of bounds
                {
                    shotList.Remove(s);
                    player.Score -= 100;
                }
                else
                {
                    s.Update(gameTime, camera);
                    if (s.TimeOfDeath == Double.MaxValue) //Disables collision detection once a shot has hit an enemy
                    {
                        for (int j = enemyList.Count - 1; j >= 0; j--)
                        {
                            if (enemyList[j].Bounds.Intersects(s.Bounds))
                            {
                                player.Score += 600 - enemyList[j].Bounds.Y;
                                enemyList.RemoveAt(j);
                                s.InitDeath(gameTime.TotalGameTime.TotalMilliseconds);
                                
                            }
                        }
                    }
                }
            }
            //Enemy logic
            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                
                enemyList[i].Update(gameTime);
                if ((enemyList[i].Bounds.Bottom > enemyList[i].rY) && (enemyList[i].hasShot == false)) //Shoots when the specific enemy arrives at a random, predetermined position along Y 
                {
                    enemyShotList.Add(new EnemyShot(shotRed, enemyList[i].Bounds, enemyShotSpeed));
                    enemyList[i].hasShot = true;
                }
                if (!enemyList[i].Bounds.Intersects(bounds))
                {
                    enemyList.RemoveAt(i);
                }
                else if (enemyList[i].Bounds.Intersects(player.Bounds)) //Collision detection
                {
                    if (player.Invuln == false)
                    {
                        player.InitHit(gameTime.TotalGameTime.TotalMilliseconds);
                    }
                }
            }
            //Enemy shot logic
            for (int i = enemyShotList.Count - 1; i >= 0; i--)
            {
                enemyShotList[i].Update(gameTime, camera);
                if (!enemyShotList[i].Bounds.Intersects(bounds))
                {
                    enemyShotList.RemoveAt(i);
                }
                else if (enemyShotList[i].Bounds.Intersects(player.Bounds)) //Collision detection
                {
                    if (player.Invuln == false)
                    {
                        player.InitHit(gameTime.TotalGameTime.TotalMilliseconds);
                    }
                }
            }
        }

        private void UpdateGameOverScreen(KeyboardState kbState, GamePadState gpState)
        {
            if (kbState.IsKeyDown(Keys.Enter) || gpState.IsButtonDown(Buttons.Start))
            {
                currentScreen = ScreenState.StartScreen;
            }
        }

        private void DrawStartScreen()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Press Enter/Start to begin\n\n" +
                "Instructions:\n" +
                "Try to get the high score by shooting enemies\n" +
                "Scores:\n" +
                "Kill an enemy: +0-600 depending on how fast you shoot it\n" +
                "Miss a shot: -100\n" +
                "Lose a life: -1000\n" +
                "Difficulty increases every 10000 points\n\n" +
                "Controls:\n" + 
                "Keyboard - Arrows to move, Space to shoot, Enter to pause\n" +
                "Xbox 360 controller - Left Stick to move, A to shoot, Start to pause\n\n" +
                "Art Credits: www.kenney.nl", Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void DrawPauseScreen()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Press Enter/Start to return to the game", Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void DrawGameScreen()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.getTransformMatrix());
            //Background
            for (int j = bounds.Height; j >= camera.Position.Y - bgSprite.Height; j -= bgSprite.Height)
            {
                for (int i = 0; i <= bounds.Width; i += bgSprite.Width)
                {
                    spriteBatch.Draw(bgSprite, new Vector2(i, j), Color.White);
                }
            }
            //UI
            spriteBatch.DrawString(font, String.Format("Score: {0}", player.Score), new Vector2(10, 0) + camera.Position, Color.White);
            spriteBatch.DrawString(font, String.Format("Level: {0}", level), new Vector2(10, bounds.Bottom - 30) + camera.Position, Color.White);
            spriteBatch.DrawString(font, String.Format("Lives: {0}", player.Lives), new Vector2(bounds.Right - 100, bounds.Bottom - 30) + camera.Position, Color.White);
            //Draw shots
            foreach (Shot s in shotList)
            {
                s.Draw(spriteBatch, camera);
            }
            //Draw enemies
            foreach (Enemy e in enemyList)
            {
                e.Draw(spriteBatch, camera);
            }
            //Draw enemy shots
            foreach (EnemyShot es in enemyShotList)
            {
                es.Draw(spriteBatch, camera);
            }
            //Active player sprite
            player.Draw(spriteBatch, camera);

            spriteBatch.End();
        }

        private void DrawGameOverScreen()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, String.Format("Game Over! Total Score: {0}\nPress Enter/Start to return to the main menu", player.Score), Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void NewGame()
        {
            level = 1;
            spawnFreq = 1000;
            enemyVerticalSpeed = 2;
            enemyShotSpeed = 3;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            enemyList.Clear();
            shotList.Clear();
            enemyShotList.Clear();
            player = new Player(playerSprite, playerSpriteL, playerSpriteR, new Vector2(displayWidth, displayHeight));
        }
    }
}
