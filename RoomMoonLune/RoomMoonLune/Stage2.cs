using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ricoh2DFramework;
using Ricoh2DFramework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ricoh2DFramework.Collisions;

namespace RoomMoonLune
{
    class Stage2 : IRState
    {
        Sprite Moon;
        Random rand;
        Sprite landing;
        Texture2D particleTexture;
        SpaceShip Ship;
        
        public void Initialize()
        {
            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {
            Moon = new Sprite(Content.Load<Texture2D>("Moon"), 800, 800);
            landing = new Sprite(Content.Load<Texture2D>("Landing"), 350, 400);
            //Moon = new Sprite(Content.Load<Texture2D>("MoonSpriteSheet"), 640, 360);
            //Moon.Animation.Add("standard", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 }, true, 11);
            //Moon.Animation.Play("standard");
            Moon.Position = new Vector2(RGlobal.Resolution.ScreenWidth / 2, RGlobal.Resolution.ScreenHeight + 150);
            Moon.Scale = new Vector2(1.0f, 1.0f);

            landing.Position = new Vector2(RGlobal.Resolution.ScreenWidth / 2, RGlobal.Resolution.ScreenHeight + 150);
            landing.Origin = new Vector2(landing.Origin.X, 750);
            landing.Scale = new Vector2(0.75f, 0.75f);

            particleTexture = Content.Load<Texture2D>("Particle");

            Ship = new SpaceShip(Content.Load<Texture2D>("TempShip"), particleTexture, 160, 90, rand);
            Ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth, RGlobal.Resolution.VirtualHeight) / 2;
            Ship.Acceleration.Y = 98.1f;
            Ship.Drag = new Vector2(1.01f, 1);
        }

        public void UnloadContent()
        {
           
        }

        public void Update(GameTime gameTime)
        {
            if (!Ship.IsDocking)
            {
                Ship.Acceleration.X = RGlobal.Input.LeftAnalogStick.X * 100;

                Ship.Rotation = 3 * MathHelper.PiOver2 + (RGlobal.Input.LeftAnalogStick.X * MathHelper.PiOver4 / 2);
                if (RGlobal.Input.isGamePadButtonDown(Buttons.A))
                {
                    Ship.Velocity.Y -= 5;
                }
            }
            else
            {
                Ship.Position = Moon.Position;
                Ship.Position += 625 * new Vector2((float)Math.Cos(Moon.Rotation - MathHelper.PiOver2), (float)Math.Sin(Moon.Rotation - MathHelper.PiOver2));
                Ship.Rotation = Moon.Rotation - MathHelper.PiOver2;

                if (RGlobal.Input.isGamePadButtonDown(Buttons.A))
                {
                    Ship.IsDocking = false;
                }
            }

            Moon.Update(gameTime);
            Moon.Rotation += 0.4f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            landing.Rotation = Moon.Rotation;
            landing.Update(gameTime);

            Ship.Update(gameTime);

            RGlobal.BackgroundColor = Color.Black;
            if (CollisionManager.Collide(Ship.Collider, landing.Collider, CollisionType.Box))
            {
                if (RGlobal.Input.isGamePadButtonUp(Buttons.A))
                {
                    Ship.IsDocking = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Moon.Draw(spriteBatch);
            landing.Draw(spriteBatch);
            Ship.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
