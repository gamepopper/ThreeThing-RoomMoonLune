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
using Microsoft.Xna.Framework.Audio;

namespace RoomMoonLune
{
    enum Stage2State
    {
        docking,
        leaving
    }
    class Stage2 : IRState
    {
        Sprite Moon;
        Random rand;
        Sprite landing;
        Texture2D particleTexture;
        SpaceShip Ship;

        Stage2State gameState = Stage2State.docking;
        Text Score;
        Text Health;
        StarField starField;

        public void Initialize()
        {
            rand = new Random();
            
        }

        public void LoadContent(ContentManager Content)
        {
            Moon = new Sprite(Content.Load<Texture2D>("Landing"), 1348, 1300);
            landing = new Sprite(Content.Load<Texture2D>("Landing"), 350, 400);
            //Moon = new Sprite(Content.Load<Texture2D>("MoonSpriteSheet"), 640, 360);
            //Moon.Animation.Add("standard", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 }, true, 11);
            //Moon.Animation.Play("standard");
            Moon.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight + 150);
            Moon.Scale = new Vector2(1.0f, 1.0f);
            Moon.Rotation = (float)rand.NextDouble() * MathHelper.TwoPi;

            landing.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight + 150);
            landing.Origin = new Vector2(landing.Origin.X, 750);
            landing.Scale = new Vector2(0.75f, 0.75f);
            landing.Collider.Offset = -50;
            particleTexture = Content.Load<Texture2D>("Particle");

            Ship = new SpaceShip(Content.Load<Texture2D>("ShipSpritesheet"), particleTexture, 256, 256, rand);
            Ship.Position = new Vector2((float)rand.NextDouble() * RGlobal.Resolution.VirtualWidth, 150) / 2;
            Ship.Velocity.X = (float)(rand.NextDouble() / 2) + 1;
            Ship.Velocity.X *= 50;
            Ship.Acceleration.Y = 98.1f;
            Ship.Drag = new Vector2(1.01f, 1);
            Ship.Health = LevelSingleton.Health;

            Score = new Text(Content.Load<SpriteFont>("Font"), "Score: ", RGlobal.Resolution.VirtualWidth, TextAlignment.LEFT);
            Score.Position = new Vector2(0, 25);
            Health = new Text(Content.Load<SpriteFont>("Font"), ": Health", RGlobal.Resolution.VirtualWidth, TextAlignment.RIGHT);
            Health.Position = new Vector2(0, 25);

            starField =new StarField(Content.Load<Texture2D>("Star"), 100, 100, rand,true);
            RGlobal.Sound.Add("Landing", Content.Load<SoundEffect>("Land"));
        }

        public void UnloadContent()
        {
            RGlobal.Sound.Remove("Landing");
        }

        public void Update(GameTime gameTime)
        {
            starField.Update(gameTime);
            if (!Ship.IsDocking && !Ship.IsLeaving)
            {
                Ship.Acceleration.X = RGlobal.Input.LeftAnalogStick.X * 100;
                
                if (RGlobal.Input.isGamePadButtonDown(Buttons.A))
                {
                    Ship.Velocity.Y -= 5;
                }

                if (LevelSingleton.CargoMoonCount == 0)
                {
                    Ship.IsLeaving = true;
                    Ship.IsDocking = false;
                
                }
            }
            else if (Ship.IsLeaving)
            {
                Ship.Acceleration.X = RGlobal.Input.LeftAnalogStick.X * 100;
                
                if (RGlobal.Input.isGamePadButtonDown(Buttons.A))
                {
                    Ship.Velocity.Y -= 5;
                }

                if (Ship.Position.Y  < 20)
                {
                    RGlobal.Game.SwitchState(new Stage1());
                }
            }
            else
            {
                if (LevelSingleton.CargoMoonCount > 0)
                    LevelSingleton.CargoMoonCount -= 70 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (LevelSingleton.CargoMoonCount < 0)
                    LevelSingleton.CargoMoonCount = 0;
                Ship.Position = Moon.Position;
                Ship.Position += 625 * new Vector2((float)Math.Cos(Moon.Rotation - MathHelper.PiOver2), (float)Math.Sin(Moon.Rotation - MathHelper.PiOver2));
                Ship.Rotation = Moon.Rotation;

                if (RGlobal.Input.isGamePadButtonDown(Buttons.A) || LevelSingleton.CargoMoonCount == 0)
                {
                    LevelSingleton.TotalMoonCount += 1000;
                    Ship.IsDocking = false;
                    Ship.Rotation = 0;
                }
            }
            Ship.Update(gameTime);

            //Moon.Update(gameTime);
            Moon.Rotation += 0.4f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            landing.Rotation = Moon.Rotation;
            landing.Update(gameTime);

            RGlobal.BackgroundColor = Color.Black;
            if (!Ship.IsLeaving)
            {
                if (CollisionManager.Collide(Ship.Collider, landing.Collider, CollisionType.Box))
                {
                    if (RGlobal.Input.isGamePadButtonUp(Buttons.A) && Math.Abs(Ship.Velocity.X) < 15 && Math.Abs(Ship.Velocity.Y) < 13)
                    {
                        RGlobal.Sound.Play("Landing");
                        Ship.IsDocking = true;
                        foreach (var particle in Ship.PManager.Particles)
                        {
                            particle.IsAlive = false;
                        }
                    }
                }
            }
            Score.TextString = "    Score: " + (int)LevelSingleton.CargoMoonCount + "/1000";
            Health.TextString = "" + (int)Ship.Health + ": Health     ";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            starField.Draw(spriteBatch);
            Moon.Draw(spriteBatch);
           // landing.Draw(spriteBatch);
            Ship.Draw(spriteBatch);
            Score.Draw(spriteBatch);
            Health.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
