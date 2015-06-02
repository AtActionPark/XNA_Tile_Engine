using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class Input
    {
        public KeyboardState keyboardState, oldKeyboardState = Keyboard.GetState();
        public MouseState mouseState, oldMouseState= Mouse.GetState();

        public Vector2 MouseToGrid
        {
            get{
                Vector2 result = new Vector2(mouseState.X, mouseState.Y) + Camera.Instance.Position*Camera.Instance.Zoom - Static.ScreenSize / 2;
            result /= 32;
            result /= Camera.Instance.Zoom;
            result.X = (float)Math.Floor(result.X);
            result.Y = (float)Math.Floor(result.Y);
                return  result;
            }
        }

        public bool IsWheelButtonDown
        {
            get 
            {
                return mouseState.MiddleButton == ButtonState.Pressed;
            }
        }
        private bool WasWheelButtonDown
        {
            get
            {
                return oldMouseState.MiddleButton == ButtonState.Pressed;
            }
        }
        public bool IsLeftButtonDown
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Pressed;
            }
        }
        public bool WasLeftButtonDown
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
            }
        }
        public bool IsRightButtonDown
        {
            get
            {
                return mouseState.RightButton == ButtonState.Pressed;
            }
        }
        public bool WasRightButtonDown
        {
            get
            {
                return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
            }
        }

        private Vector2 ClickedPosition;
        public Vector2 displacement;
        

        public void Update()
        {
            oldKeyboardState = keyboardState;
            oldMouseState = mouseState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            DragAndDrop();
        }

        public bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool WasKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key);
        }

        private void DragAndDrop()
        {
            if (!IsWheelButtonDown)
            {
                ClickedPosition = Vector2.Zero;
            }

            else
            {
                if (IsWheelButtonDown && !WasWheelButtonDown)
                    ClickedPosition = new Vector2(mouseState.X, mouseState.Y);

                if (ClickedPosition != Vector2.Zero)
                {
                    if (oldMouseState.X != mouseState.X || oldMouseState.Y != mouseState.Y)
                        displacement = ClickedPosition - new Vector2(mouseState.X, mouseState.Y);
                }
            }

            if (displacement.LengthSquared() > 10)
                displacement *= 0.95f;
            else
                displacement = Vector2.Zero;
        }

        

        
    }
}
