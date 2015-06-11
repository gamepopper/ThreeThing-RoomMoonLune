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
        Texture2D moonOreTexture;
        Texture2D particleTexture;
        List<SpawningObject> spawningObjects = new List<SpawningObject>();
        float spawningTime;
        SpaceShip Ship;
        List<Sprite> RenderList = new List<Sprite>();

        int moonOreCount = 0;

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
            asteroidTexture = Content.Load<Texture2D>("Moon");
            moonOreTexture = Content.Load<Texture2D>("MoonOre");
            particleTexture = Content.Load<Texture2D>("Particle");

            Ship = new SpaceShip(Content.Load<Texture2D>("TempShip"),particleTexture, 160, 90,rand);
            Ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth, RGlobal.Resolution.VirtualHeight) / 2;
            Ship.Acceleration.Y = 98.1f;
            Ship.Drag = new Vector2(1.01f, 1);

            Moon = new Sprite(Content.Load<Texture2D>("Moon"), 240, 216);
            Moon.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight + 150);
            
            

            for (int i = 0; i < 10; i++)
            {
                spawningObjects.Add(new SpawningObject(asteroidTexture,moonOreTexture, 240, 216, rand));
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

            Ship.Rotation = 3 * MathHelper.PiOver2 + (RGlobal.Input.LeftAnalogStick.X * MathHelper.PiOver4/2);

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
                switch (obj.Type)
                {
                    case EnemyType.asteroid:
                        if (obj.IsAlive && obj.Scale.X > 0.9f && obj.Scale.X < 1.0f)
                        {
                            if (CollisionManager.Collide(Ship.Collider, obj.Collider, CollisionType.Box))
                            {
                                obj.Kill();
                                Ship.Health -= 5;
                            }
                        }
                        break;
                    case EnemyType.moonOre:
                        if (obj.IsAlive && obj.Scale.X > 0.3f && obj.Scale.X < 0.4f)
                        {
                            if (CollisionManager.Collide(Ship.Collider, obj.Collider, CollisionType.Box))
                            {
                                obj.Kill();
                                moonOreCount += 100;
                                
                            }
                        }
                        break;
                    default:
                        break;
                }

               
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
