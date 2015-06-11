using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ricoh2DFramework;
using Ricoh2DFramework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace RoomMoonLune
{
    class Stage1 : IRState
    {
        Sprite Ship;
        
        public void Initialize()
        {
            RGlobal.BackgroundColor = Color.CornflowerBlue;
            RGlobal.Input.InvertedLeft = true;
            RGlobal.Input.InvertedRight = true;
            TouchPanel.EnableMouseGestures = true;
            TouchPanel.EnableMouseTouchPoint = true;
        }

        public void LoadContent(ContentManager Content)
        {
            Ship = new Sprite(Content.Load<Texture2D>("TempShip"));
            Ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth, RGlobal.Resolution.VirtualHeight) / 2;
            Ship.Acceleration.Y = 98.1f;
            Ship.Drag = new Vector2(1.01f, 1);
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            Ship.Acceleration.X = RGlobal.Input.LeftAnalogStick.X * 100;

            if (Ship.Acceleration.X < 0)
            {
                Ship.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (Ship.Acceleration.X > 0)
            {
                Ship.SpriteEffects = SpriteEffects.None;
            }

            if (RGlobal.Input.isGamePadButtonDown(Buttons.A))
            {
                Ship.Velocity.Y -= 5;
            }

            Ship.Update(gameTime);

            if (Ship.Position.Y + Ship.Collider.Box.Height/2 > RGlobal.Resolution.ScreenHeight)
            {
                Ship.Position = new Vector2(Ship.Position.X, RGlobal.Resolution.ScreenHeight - (Ship.Collider.Box.Height / 2));
                Ship.Velocity.Y = 0;
                Ship.Drag = new Vector2(1.02f, 1);
            }
            else
            {
                Ship.Drag = new Vector2(1.001f, 1);
            }

            if (RGlobal.Input.isKeyPressed(Keys.P))
            {
                RGlobal.Game.SwitchState(new Stage2());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Ship.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
