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

namespace RoomMoonLune
{
    class Stage2 : IRState
    {
        Sprite Moon;
        Random rand;

        public void Initialize()
        {
            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {
            Moon = new Sprite(Content.Load<Texture2D>("MoonSpriteSheet"), 640, 360);
            Moon.Animation.Add("standard", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 }, true, 11);
            Moon.Animation.Play("standard");
            Moon.Position = new Vector2(RGlobal.Resolution.ScreenWidth / 2, RGlobal.Resolution.ScreenHeight/2);
        }

        public void UnloadContent()
        {
           
        }

        public void Update(GameTime gameTime)
        {
           
            Moon.Update(gameTime);
           // Moon.Rotation += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
           // Moon.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
