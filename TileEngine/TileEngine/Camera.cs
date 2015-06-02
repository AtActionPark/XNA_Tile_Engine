using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    class Camera
    {
        public static Camera Instance;

        private float moveScale = 10f;
        private float zoomScale = 1.15f;
        private float rotationScale = 0.10f;
        private float grabMoveScale = 1/25f;

        private float zoom;
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set{rotation =value;}
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Matrix transform;

        public Camera()
        {
            position = new Vector2(0, 0);
            Instance = this;
            zoom = 1f;
        }

        public void Move(Vector2 amount)
        {
            position += amount;

            KeepInBounds();
        }

        public void Update(Input input)
        {
            KeyboardState keyState = Keyboard.GetState();

            #region keys
            Move(input.displacement*grabMoveScale/Zoom);

            if (input.keyboardState.IsKeyDown(Keys.Up))      
            {
                Move(new Vector2(0, -moveScale / zoom));
            }
            if (input.keyboardState.IsKeyDown(Keys.Down))    
            {
                Move(new Vector2(0, moveScale / zoom));
            }

            if (input.keyboardState.IsKeyDown(Keys.Right))   
            {
                Move(new Vector2(moveScale / zoom, 0));
            }
            if (input.keyboardState.IsKeyDown(Keys.Left))   
            {
                Move(new Vector2(-moveScale / zoom, 0));
            }

            if (input.keyboardState.IsKeyDown(Keys.P)|| input.mouseState.ScrollWheelValue > input.oldMouseState.ScrollWheelValue)
            {
                Zoom *= zoomScale;
            }
            if (input.keyboardState.IsKeyDown(Keys.M)||input.mouseState.ScrollWheelValue < input.oldMouseState.ScrollWheelValue)    
            {
                Zoom /=zoomScale;
            }

            if (input.keyboardState.IsKeyDown(Keys.A))    
            {
                Rotation += rotationScale;
            }
            if (input.keyboardState.IsKeyDown(Keys.Q))    
            {
                Rotation -= rotationScale;
            }

            #endregion
        }

        private void KeepInBounds()
        {
            if (position.X < 0)
                position.X = 0;
            if (position.X > Static.tileSize * Static.MapSize.X)
                position.X = Static.tileSize * Static.MapSize.X;

            if (position.Y < 0)
                position.Y = 0;
            if (position.Y > Static.tileSize * Static.MapSize.Y)
                position.Y = Static.tileSize * Static.MapSize.Y;
        }

        public Matrix GetTransformation()
        {
            transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(Static.ScreenSize.X / 2, Static.ScreenSize.Y / 2, 0));

            return transform;
        }
    }
}
