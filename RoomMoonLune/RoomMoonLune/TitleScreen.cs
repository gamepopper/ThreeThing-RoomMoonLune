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

    enum State
    {
        titleScreen,
        mainMenu
    }
    class TitleScreen : IRState
    {
        Random rand;
        Text title1;
        Text title2;
        Text title3;
        Text title4;
        bool fadingDown = true;

        StarField starField;
        State gamestate = State.titleScreen;
        bool fadeAllOut = false;
        int option = 0;
        public void Initialize()
        {
            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {
            title1 = new Text(Content.Load<SpriteFont>("TitleFont"), "The Stafford Trio ", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title1.Scale = new Vector2(1.0f, 1.0f);
            title1.Position = new Vector2(0, 200);

            title2 = new Text(Content.Load<SpriteFont>("TitleFont"), "Presents ", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title2.Scale = new Vector2(1.0f, 1.0f);
            title2.Position = new Vector2(0, 300);

            title3 = new Text(Content.Load<SpriteFont>("TitleFont"), "Lunar Chaos", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title3.Scale = new Vector2(1.0f, 1.0f);
            title3.Position = new Vector2(0, 405);

            title4 = new Text(Content.Load<SpriteFont>("TitleFont"), "Press Start/Enter to Continue", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title4.Scale = new Vector2(0.6f, 0.6f);
            title4.Position = new Vector2(135, 605);
            starField = new StarField(Content.Load<Texture2D>("Star"), 100, 100, rand);

            RGlobal.Sound.Add("Start", Content.Load<SoundEffect>("Button"));

        }

        public void UnloadContent()
        {
           
        }

        public void Update(GameTime gameTime)
        {
            switch (gamestate)
            {
                case State.titleScreen:
                    title1.Update(gameTime);
                    title2.Update(gameTime);
                    title3.Update(gameTime);
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

                    title4.Update(gameTime);

                    if (RGlobal.Input.isGamePadButtonPressed(Buttons.Start) || RGlobal.Input.isKeyPressed(Keys.Enter))
                        fadeAllOut = true;

                    if(fadeAllOut)
                    {
                        if(title1.Opacity > 0)
                        {
                            title1.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title2.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title3.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title4.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            title1.TextString = "Main Menu";
                            title2.TextString = "Start Tutorial";
                            title3.TextString = "Start Game";
                            title4.TextString = "Quit";
                            title4.Position = new Vector2(0, 505);
                            title4.Scale = new Vector2(1.0f, 1.0f);

                            gamestate = State.mainMenu;
                        }
                    }
                    break;
                case State.mainMenu:
                    if (fadeAllOut)
                    {
                        if (title1.Opacity < 1)
                        {
                            title1.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title2.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title3.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            title4.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            if (RGlobal.Input.isGamePadButtonPressed(Buttons.DPadUp))
                            {
                                option--;
                                if (option < 0)
                                    option = 2;
                            }
                            if (RGlobal.Input.isGamePadButtonPressed(Buttons.DPadDown))
                            {
                                option++;
                                if (option > 2)
                                    option = 0;
                            }
                        }

                        switch (option)
                        {
                            case 0:
                                title2.Color = Color.Yellow;
                                title3.Color = Color.White;
                                title4.Color = Color.White;
                                    break;
                            case 1:
                                title2.Color = Color.White;
                                title3.Color = Color.Yellow;
                                title4.Color = Color.White;
                                break;
                            case 2:
                                title2.Color = Color.White;
                                title3.Color = Color.White;
                                title4.Color = Color.Yellow;
                                break;
                            default:
                                break;
                        }
                        if (RGlobal.Input.isGamePadButtonPressed(Buttons.A))
                        {
                            switch (option)
                            {
                                case 0:
                                  
                                    break;
                                case 1:
                                    RGlobal.Game.SwitchState(new Stage1());
                                    break;

                                case 2:
                                    App.Current.Exit();
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    break;
                default:
                    break;
            }
            starField.Update(gameTime);
           
            title4.Update(gameTime);

            if (RGlobal.Input.isGamePadButtonPressed(Buttons.Start) || RGlobal.Input.isKeyPressed(Keys.Enter))
            {
                RGlobal.Sound.Play("Start");
                RGlobal.Game.SwitchState(new Stage1());
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            title1.Draw(spriteBatch);
            title2.Draw(spriteBatch);
            title3.Draw(spriteBatch);
            title4.Draw(spriteBatch);
            starField.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
