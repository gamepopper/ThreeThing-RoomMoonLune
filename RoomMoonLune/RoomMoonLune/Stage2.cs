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
        Random rand;
        Sprite Moon;
        SpawningObject spawnObj;
        List<SpawningObject> spawningObjects = new List<SpawningObject>();
        float spawningTime;

        //Textures
        Texture2D asteroidTexture;
        public void Initialize()
        {
            RGlobal.BackgroundColor = Color.CornflowerBlue;
            rand = new Random();

          
        }

        public void LoadContent(ContentManager Content)
        {
            Moon = new Sprite(Content.Load<Texture2D>("Moon"),1280,720);
            Moon.Position = new Vector2(RGlobal.Resolution.ScreenWidth/2, RGlobal.Resolution.ScreenHeight + 150) ;


            asteroidTexture = Content.Load<Texture2D>("Moon");
            for (int i = 0; i < 10; i++)
            {
                spawningObjects.Add(new SpawningObject(asteroidTexture, 1280, 720, rand));
            }
        }

        public void UnloadContent()
        {
           
        }

        public void Update(GameTime gameTime)
        {
             spawningTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spawningTime > 2.0f)
            {
                for (int i = 0; i < spawningObjects.Count - 1; i++)
                {
                    if (!spawningObjects[i].IsAlive)
                    {
                        spawningObjects[i].Respawn();
                        spawningTime = 0.0f;
                        break;
                    }
                }
            }

            Moon.Rotation += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds ;

            foreach (var obj in spawningObjects)
            {
                obj.Update(gameTime);
            }
           
            
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //Moon.Draw(spriteBatch);
            for (int i = spawningObjects.Count-1; i > 0; i--)
            {
                
                spawningObjects[i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
