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

namespace RoomMoonLune
{
    class Stage2 : IRState
    {
        Sprite Moon;
        SpawningObject spawnObj;
        public void Initialize()
        {
            RGlobal.BackgroundColor = Color.CornflowerBlue;
           
        }

        public void LoadContent(ContentManager Content)
        {
            Moon = new Sprite(Content.Load<Texture2D>("Moon"),1280,720);
            Moon.Position = new Vector2(RGlobal.Resolution.ScreenWidth/2, RGlobal.Resolution.ScreenHeight + 150) ;

            spawnObj = new SpawningObject(Content.Load<Texture2D>("Moon"), 1280, 720);
            
        }

        public void UnloadContent()
        {
           
        }

        public void Update(GameTime gameTime)
        {
            Moon.Rotation += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds ;

            spawnObj.Update(gameTime);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //Moon.Draw(spriteBatch);
            spawnObj.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
