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
    class EndGame : IRState
    {
        Random rand;
        Text title1;
        Text title2;
        Text title3;
        StarField starField;
        Sprite bgMoon;
        public void Initialize()
        {
            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {
            bgMoon = new Sprite(Content.Load<Texture2D>("MoonBG"), 1280, 720);
            bgMoon.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight / 2);

            title1 = new Text(Content.Load<SpriteFont>("TitleFont"), "Game Over! ", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title1.Scale = new Vector2(1.0f, 1.0f);
            title1.Position = new Vector2(0, 200);

            title2 = new Text(Content.Load<SpriteFont>("TitleFont"), "Score:  " + LevelSingleton.TotalMoonCount, RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title2.Scale = new Vector2(1.0f, 1.0f);
            title2.Position = new Vector2(0, 300);

            title3 = new Text(Content.Load<SpriteFont>("TitleFont"), "Press Start to Main Menu", RGlobal.Resolution.VirtualWidth, TextAlignment.CENTER);
            title3.Scale = new Vector2(1.0f, 1.0f);
            title3.Position = new Vector2(0, 605);
            starField = new StarField(Content.Load<Texture2D>("Star"), 100, 100, rand,false);
            RGlobal.Sound.Add("GameOver", Content.Load<SoundEffect>("GameOver"));
            RGlobal.Sound.Play("GameOver");
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            bgMoon.Update(gameTime);
            title1.Update(gameTime);
            title2.Update(gameTime);
            title3.Update(gameTime);
            starField.Update(gameTime);

            if (RGlobal.Input.isGamePadButtonPressed(Buttons.Start))
                RGlobal.Game.SwitchState(new TitleScreen());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            bgMoon.Draw(spriteBatch);
            title1.Draw(spriteBatch);
            title2.Draw(spriteBatch);
            title3.Draw(spriteBatch);
           
            starField.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
