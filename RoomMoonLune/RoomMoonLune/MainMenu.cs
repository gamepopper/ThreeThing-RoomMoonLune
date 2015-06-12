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
    class MainMenu : IRState
    {
        Random rand;
        Text title1;
        Text title2;
        Text title3;
        Text title4;
        StarField starField;
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

            starField = new StarField(Content.Load<Texture2D>("Star"), 100, 100, rand,true);
        }

        public void UnloadContent()
        {
           
        }

        public void Update(GameTime gameTime)
        {
            starField.Update(gameTime);
            title1.Update(gameTime);
            title2.Update(gameTime);
            title3.Update(gameTime);
            title4.Update(gameTime);
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
