﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Ricoh2DFramework;
using Ricoh2DFramework.Graphics;
using System;
using System.Collections.Generic;

namespace RoomMoonLune
{
    class Stage1 : IRState
    {
        Random rand;
        Sprite Moon;
        Texture2D asteroidTexture;
        List<SpawningObject> spawningObjects = new List<SpawningObject>();
        float spawningTime;
        Sprite Ship;

        public void Initialize()
        {
            RGlobal.Input.InvertedLeft = true;
            RGlobal.Input.InvertedRight = true;
            TouchPanel.EnableMouseGestures = true;
            TouchPanel.EnableMouseTouchPoint = true;
            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {
            Ship = new Sprite(Content.Load<Texture2D>("TempShip"));
            Ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth, RGlobal.Resolution.VirtualHeight) / 2;
            Ship.Acceleration.Y = 98.1f;
            Ship.Drag = new Vector2(1.01f, 1);

            Moon = new Sprite(Content.Load<Texture2D>("Moon"), 1280, 720);
            Moon.Position = new Vector2(RGlobal.Resolution.ScreenWidth / 2, RGlobal.Resolution.ScreenHeight + 150);
            
            asteroidTexture = Content.Load<Texture2D>("Moon");
            for (int i = 0; i < 10; i++)
            {
                spawningObjects.Add(new SpawningObject(asteroidTexture, 1280, 720, rand));
            }
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

            if (Ship.Collider.Box.Right > RGlobal.Resolution.ScreenWidth + Ship.Collider.Box.Width)
            {
                Ship.Position = new Vector2(-Ship.Collider.Box.Width / 2, Ship.Position.Y);
            }
            else if (Ship.Collider.Box.Left < -Ship.Collider.Box.Width)
            {
                Ship.Position = new Vector2(RGlobal.Resolution.ScreenWidth + Ship.Collider.Box.Width / 2, Ship.Position.Y);
            }

            spawningTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spawningTime > 2.0f)
            {
                for (int i = 0; i < spawningObjects.Count - 1; i++)
                {
                    if (!spawningObjects[i].IsAlive)
                    {
                        spawningObjects[i].Respawn();
                        spawningTime = 0.0f;
                        break;
                    }
                }
            }

            Moon.Rotation += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var obj in spawningObjects)
            {
                obj.Update(gameTime);
            }

            if (RGlobal.Input.isKeyPressed(Keys.P))
            {
                RGlobal.Game.SwitchState(new Stage2());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (int i = spawningObjects.Count - 1; i > 0; i--)
            {
                spawningObjects[i].Draw(spriteBatch);
            }

            Ship.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
