using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Ricoh2DFramework;
using Ricoh2DFramework.Graphics;
using Ricoh2DFramework.Collisions;
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
        SpaceShip Ship;
        List<Sprite> RenderList = new List<Sprite>();

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
            Ship = new SpaceShip(Content.Load<Texture2D>("TempShip"), 160, 90);
            Ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth, RGlobal.Resolution.VirtualHeight) / 2;
            Ship.Acceleration.Y = 98.1f;
            Ship.Drag = new Vector2(1.01f, 1);

            Moon = new Sprite(Content.Load<Texture2D>("Moon"), 240, 216);
            Moon.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight + 150);
            
            asteroidTexture = Content.Load<Texture2D>("Moon");
            for (int i = 0; i < 10; i++)
            {
                spawningObjects.Add(new SpawningObject(asteroidTexture, 240, 216, rand));
            }

            RenderList.AddRange(spawningObjects);
            RenderList.Add(Ship);
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

            spawningTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spawningTime > 2.0f)
            {
                for (int i = 0; i < spawningObjects.Count - 1; i++)
                {
                    if (!spawningObjects[i].IsAlive)
                    {
                        spawningObjects[i].Respawn();
                        spawningTime = 0.0f;
                        RenderList.Sort(ByScale);
                        break;
                    }
                }
            }

            Moon.Rotation += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            RGlobal.BackgroundColor = Color.Black;

            foreach (SpawningObject obj in spawningObjects)
            {
                obj.Update(gameTime);

                if (obj.IsAlive && obj.Scale.X > 0.9f && obj.Scale.X < 1.0f)
                {
                    if (CollisionManager.Collide(Ship.Collider, obj.Collider, CollisionType.Box))
                    {
                        obj.Kill();
                        Ship.Health -= 5;
                    }
                }
            }

            if (Ship.Position.Y + Ship.Collider.Box.Height/2 > RGlobal.Resolution.VirtualHeight)
            {
                Ship.Position = new Vector2(Ship.Position.X, RGlobal.Resolution.VirtualHeight - (Ship.Collider.Box.Height / 2));
                Ship.Velocity.Y = 0;
                Ship.Drag = new Vector2(1.01f, 1);
            }
            else if (Ship.Position.Y - Ship.Collider.Box.Height/2 < 0)
            {
                Ship.Position = new Vector2(Ship.Position.X, +(Ship.Collider.Box.Height / 2));
                Ship.Velocity.Y = 0;
                Ship.Drag = new Vector2(1.01f, 1);
            }
            else
            {
                Ship.Drag = new Vector2(1.001f, 1);
            }

            if (Ship.Collider.Box.Right > RGlobal.Resolution.VirtualWidth + Ship.Collider.Box.Width)
            {
                Ship.Position = new Vector2(-Ship.Collider.Box.Width / 2, Ship.Position.Y);
            }
            else if (Ship.Collider.Box.Left < -Ship.Collider.Box.Width)
            {
                Ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth + Ship.Collider.Box.Width / 2, Ship.Position.Y);
            }

            if (RGlobal.Input.isKeyPressed(Keys.P))
            {
                RGlobal.Game.SwitchState(new Stage2());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (int i = 0; i < RenderList.Count; i++)
            {
                RenderList[i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        int ByScale(Sprite a, Sprite b)
        {
            if (a.Scale.X > b.Scale.X)
                return 1;
            if (a.Scale.X < b.Scale.X)
                return -1;
            else
                return 0;
        }
    }
}
