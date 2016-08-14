using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    //Based on the XNA tutorial for creating a 2D screen on ilearn
    class Camera2D
    {
        public Vector2 Position { get; set; }
        Vector2 Origin;

        public Camera2D(Viewport viewPort)
        {
            Origin = new Vector2(viewPort.X / 2.0f, viewPort.Y / 2.0f);
        }

        public Matrix getTransformMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0))
                * Matrix.CreateRotationZ(-0)
                * Matrix.CreateScale(1)
                * Matrix.CreateTranslation(new Vector3(Origin, 0));
        }

        public void moveUp(float speed)
        {
            Position += new Vector2(0, -speed);
        }
    }
}
