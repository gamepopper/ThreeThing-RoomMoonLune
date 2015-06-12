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
    enum DoTutorialState
    {
        movement,
        aim,
        docking
    }
    
    class DoTutorial : IRState
    {

        Random rand;
        Text title1;
        Text title2;
        Text title3; 
        Text title4;
        Text title5;
       
        StarField starField;
        DoTutorialState gamestate = DoTutorialState.movement;
        SpaceShip Ship;
        Texture2D particleTexture;
        bool fadingDown = true;
        bool fadeAllOut = false;
        Sprite bgMoon;
        //stage 1 tutorial
        Sprite asteroid;
        Sprite ore;

        //final tutorial - docking
        Sprite Moon;
        Sprite landing;
        bool canMoveToFinal;
        public void Initialize()
        {
            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {
            bgMoon = new Sprite(Content.Load<Texture2D>("MoonBG"), 1280, 720);
            bgMoon.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight / 2);

            title1 = new Text(Content.Load<SpriteFont>("TitleFont"), "Do-Tutorial ", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title1.Scale = new Vector2(1.0f, 1.0f);
            title1.Position = new Vector2(0, 100);

            title2 = new Text(Content.Load<SpriteFont>("DoTutorialFont"), "Move left and right with the Left Analogue Stick \npress 'A' to fly upwards ", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title2.Scale = new Vector2(1.0f, 1.0f);
            title2.Position = new Vector2(0, 200);

            title3 = new Text(Content.Load<SpriteFont>("DoTutorialFont"), "You can move through the side of the screens (Left/Right)", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title3.Scale = new Vector2(1.0f, 1.0f);
            title3.Position = new Vector2(0, 305);

            title5 = new Text(Content.Load<SpriteFont>("DoTutorialFont"), "", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title5.Scale = new Vector2(1.0f, 1.0f);
            title5.Position = new Vector2(0, 305);

            title4 = new Text(Content.Load<SpriteFont>("TitleFont"), "Press Start to Continue", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title4.Scale = new Vector2(0.6f, 0.6f);
            title4.Position = new Vector2(230, 605);
            starField = new StarField(Content.Load<Texture2D>("Star"), 100, 100, rand,false);

            particleTexture = Content.Load<Texture2D>("Particle");
            Ship = new SpaceShip(Content.Load<Texture2D>("ShipSpritesheet"), particleTexture, 512, 512, rand);
            Ship.Position = new Vector2((float)rand.NextDouble() * RGlobal.Resolution.VirtualWidth, 0) / 2;
            Ship.Velocity.X = (float)(rand.NextDouble() / 2) + 1;
            Ship.Velocity.X *= 50;
            Ship.Acceleration.Y = 98.1f;
            Ship.Drag = new Vector2(1.01f, 1);

            //tutorial stage 1
            Texture2D asteroidTexture = Content.Load<Texture2D>("Asteroid");
            Texture2D moonOreTexture = Content.Load<Texture2D>("MoonOre");

            asteroid = new Sprite(asteroidTexture, 240, 216);
            asteroid.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 3, 400);
            asteroid.Scale = new Vector2(0.6f, 0.6f);
            ore = new Sprite(moonOreTexture, 240, 216);
            ore.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 3 * 2, 400);
            ore.Scale = new Vector2(0.6f, 0.6f);

            //tutorial stage 2
            Moon = new Sprite(Content.Load<Texture2D>("Moon"), 800, 800);
            landing = new Sprite(Content.Load<Texture2D>("Landing"), 350, 400);

            Moon.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight + 150);
            Moon.Scale = new Vector2(1.0f, 1.0f);
            Moon.Rotation = (float)rand.NextDouble() * MathHelper.TwoPi;

            landing.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight + 150);
            landing.Origin = new Vector2(landing.Origin.X, 750);
            landing.Scale = new Vector2(0.75f, 0.75f);
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            bgMoon.Update(gameTime);
            starField.Update(gameTime);
            title1.Update(gameTime);
            title2.Update(gameTime);
            title3.Update(gameTime);
            title4.Update(gameTime);
            Ship.Acceleration.X = RGlobal.Input.LeftAnalogStick.X * 100;
            
            if (RGlobal.Input.isGamePadButtonDown(Buttons.A))
            {
                Ship.Velocity.Y -= 5;
            }
            Ship.Update(gameTime);

            if (fadingDown)
            {
                if (title4.Opacity > 0)
                    title4.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    fadingDown = false;
            }
            else
            {
                if (title4.Opacity < 1)
                    title4.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    fadingDown = true;
            }
            switch (gamestate)
            {
                 
                case DoTutorialState.movement:
                   
                    
                    if (RGlobal.Input.isGamePadButtonPressed(Buttons.Start))
                    {
                        fadeAllOut = true;
                    }
                    if (fadeAllOut)
                    {
                        if (title1.Opacity > 0)
                        {
                            title1.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title2.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title3.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title4.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            
                        }
                        else
                        {
                            title1.TextString = "Aim Of Game";
                            title2.TextString = "Dodge the Asteroids while collecting Ore - Collect enough \nand move onto the next stage";
                            title3.Position = new Vector2(RGlobal.Resolution.VirtualWidth/10 * 3, 290);
                            title3.Alignment = TextAlignment.LEFT;
                            title3.TextString = "Asteroid";

                            //title4.TextString = "Quit";
                            //title4.Position = new Vector2(0, 505);
                            //title4.Scale = new Vector2(1.0f, 1.0f);

                            title5.TextString = "Ore";
                            title5.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 10 * 6.5f, 290);
                            title5.Alignment = TextAlignment.LEFT;
                            title5.Scale = new Vector2(1.0f, 1.0f);
                            gamestate = DoTutorialState.aim;
                        }
                    }
                    break;
                case DoTutorialState.aim:
                   
                    if (fadeAllOut)
                    {
                        if (title1.Opacity < 1)
                        {
                            title1.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title2.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title3.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title4.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title5.Update(gameTime);
                        }
                        else
                        {
                            fadeAllOut = false;
                            title1.Update(gameTime);
                            title2.Update(gameTime);
                            title3.Update(gameTime);
                            title4.Update(gameTime);
                            title5.Update(gameTime);
                            asteroid.Update(gameTime);
                            ore.Update(gameTime);

                        }
                    }
                    else if(canMoveToFinal)
                    {
                       
                        if (fadingDown)
                        {
                            if (title1.Opacity > 0)
                            {
                                title1.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                                title2.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                                title3.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                                title4.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                            }
                            else
                            {
                                title1.TextString = "Final Stage";
                                title2.TextString = "Navigate the ship down gently to land onto the landing dock \nthis stores the ore you collected in the previous stage";
                                fadeAllOut = true;
                                Moon.Update(gameTime);
                                Moon.Rotation += 0.4f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                landing.Rotation = Moon.Rotation;
                                landing.Update(gameTime);
                                gamestate = DoTutorialState.docking;
                            }
                        }
                    }
                    if (RGlobal.Input.isGamePadButtonPressed(Buttons.Start))
                    {
                       
                        canMoveToFinal = true;
                    }


                    break;
                case DoTutorialState.docking:
                    if (fadeAllOut)
                    {
                        if (title1.Opacity < 1)
                        {
                            title1.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title2.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                           // title3.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                           // title4.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            //title5.Update(gameTime);
                        }
                        else
                        {
                            Moon.Update(gameTime);
                            Moon.Rotation += 0.4f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            landing.Rotation = Moon.Rotation;
                            landing.Update(gameTime);
                           
                            if(Ship.IsDocking)
                            {
                                title1.TextString = "Tutorial Finished";
                                title2.TextString = "Press start to Main Menu";
                                Ship.Position = Moon.Position;
                                Ship.Position += 625 * new Vector2((float)Math.Cos(Moon.Rotation - MathHelper.PiOver2), (float)Math.Sin(Moon.Rotation - MathHelper.PiOver2));
                                Ship.Rotation = Moon.Rotation;

                                if (RGlobal.Input.isGamePadButtonPressed(Buttons.Start))
                                {
                                    RGlobal.Game.SwitchState(new TitleScreen());
                                }
                            }
                            if (CollisionManager.Collide(Ship.Collider, landing.Collider, CollisionType.Box))
                            {
                                Ship.IsDocking = true;
                                foreach (var particle in Ship.PManager.Particles)
                                {
                                    particle.IsAlive = false;
                                }

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

                    }
                            break;
                default:
                    break;
            }
           
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            bgMoon.Draw(spriteBatch);
            if(gamestate == DoTutorialState.movement)
            {
                starField.Draw(spriteBatch);
                Ship.Draw(spriteBatch);
                title1.Draw(spriteBatch);
                title2.Draw(spriteBatch);
                title3.Draw(spriteBatch);
                title4.Draw(spriteBatch);
                
            }
           else if(gamestate == DoTutorialState.aim)
            {
                starField.Draw(spriteBatch);
                Ship.Draw(spriteBatch);
                asteroid.Draw(spriteBatch);
                ore.Draw(spriteBatch);
               
                title1.Draw(spriteBatch);
                title2.Draw(spriteBatch);
                title3.Draw(spriteBatch);
                title4.Draw(spriteBatch);
                title5.Draw(spriteBatch);
            }
            else if(gamestate == DoTutorialState.docking)
            {
                starField.Draw(spriteBatch);
                Moon.Draw(spriteBatch);
                landing.Draw(spriteBatch);
                Ship.Draw(spriteBatch);
                title1.Draw(spriteBatch);
                title2.Draw(spriteBatch);
                title3.Draw(spriteBatch);
               
               
            }
            spriteBatch.End();
        }
    }
}
