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
using Microsoft.Xna.Framework.Audio;

namespace RoomMoonLune
{
    class Stage1 : IRState
    {
        Random rand;
        Sprite Moon;
        Sprite bgMoon;
        Texture2D asteroidTexture;
        Texture2D moonOreTexture;
        Texture2D particleTexture;
        List<SpawningObject> spawningObjects = new List<SpawningObject>();
        float spawningTime;
        SpaceShip Ship;
        List<Sprite> RenderList = new List<Sprite>();

        Text Score;
        Text Health;
        StarField starField;

        public void Initialize()
        {
            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {
            asteroidTexture = Content.Load<Texture2D>("asteroids");
            moonOreTexture = Content.Load<Texture2D>("MoonOre");
            particleTexture = Content.Load<Texture2D>("Particle");

            Ship = new SpaceShip(Content.Load<Texture2D>("ShipSpritesheet"),particleTexture, 512, 512,rand);
            Ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth, RGlobal.Resolution.VirtualHeight) / 2;
            Ship.Acceleration.Y = 98.1f;
            Ship.Drag = new Vector2(1.01f, 1);

            Moon = new Sprite(Content.Load<Texture2D>("Moon"), 240, 216);
            Moon.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight + 150);

            bgMoon = new Sprite(Content.Load<Texture2D>("MoonBG"), 1280, 720);
            bgMoon.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight /2);

            starField = new StarField(Content.Load<Texture2D>("Star"), 100, 100,rand,false);

            for (int i = 0; i < 10; i++)
            {
                spawningObjects.Add(new SpawningObject(asteroidTexture,moonOreTexture, 200, 200,rand));
            }

            RenderList.AddRange(spawningObjects);
            RenderList.Add(Ship);

            Score = new Text(Content.Load<SpriteFont>("Font"), "Score: ", RGlobal.Resolution.VirtualWidth, TextAlignment.LEFT);
            Score.Position = new Vector2(0, 25);
            Health = new Text(Content.Load<SpriteFont>("Font"), ": Health", RGlobal.Resolution.VirtualWidth, TextAlignment.RIGHT);
            Health.Position = new Vector2(0, 25);

            RGlobal.Sound.Add("RocketJet", Content.Load<SoundEffect>("Rocket"));
            RGlobal.Sound.Add("Asteroid", Content.Load<SoundEffect>("Explosion"));
            RGlobal.Sound.Add("Ore", Content.Load<SoundEffect>("Ore"));
            RGlobal.Sound.Add("GameOver", Content.Load<SoundEffect>("GameOver"));
        }

        public void UnloadContent()
        {
            RGlobal.Sound.Remove("Asteroid");
            RGlobal.Sound.Remove("Ore");
        }

        public void Update(GameTime gameTime)
        {
            bgMoon.Update(gameTime);
            Ship.Acceleration.X = RGlobal.Input.LeftAnalogStick.X * 100;

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
                                RGlobal.Sound.Play("Asteroid");

                                Ship.Health -= 35;
                                if(Ship.Health <0)
                                {
                                    RGlobal.Game.SwitchState(new EndGame());
                                }
                            }
                        }
                        break;
                    case EnemyType.moonOre:
                        if (obj.IsAlive && obj.Scale.X > 0.3f && obj.Scale.X < 0.4f)
                        {
                            if (CollisionManager.Collide(Ship.Collider, obj.Collider, CollisionType.Box))
                            {
                                obj.Kill();
                                RGlobal.Sound.Play("Ore");
                                LevelSingleton.CargoMoonCount += 100;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            Score.TextString = "    Score: " + LevelSingleton.CargoMoonCount + "/1000";
            Health.TextString = "" + (int)Ship.Health + ": Health     ";

            if (LevelSingleton.CargoMoonCount >= 1000)
            {
                LevelSingleton.CargoMoonCount = 1000;
                LevelSingleton.Health = Ship.Health;
                RGlobal.Game.SwitchState(new Stage2());
            }

            starField.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            bgMoon.Draw(spriteBatch);
            starField.Draw(spriteBatch);
            for (int i = 0; i < RenderList.Count; i++)
            {
                RenderList[i].Draw(spriteBatch);
            }

            Score.Draw(spriteBatch);
            Health.Draw(spriteBatch);

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
