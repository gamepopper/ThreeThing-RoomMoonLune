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
    class ThrustParticles : Sprite
    {
        float aliveTime;
        float curTime;
        float scaleSpeed = 0.5f;
        float fadeSpeed = 1.0f;
        bool isAlive;
        Vector2 direction;
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }


public ThrustParticles(Texture2D texture,int width, int height,Random rand):base(texture,width,height)
        {
            isAlive = false;
            aliveTime = 4.0f;
            curTime = 0.0f;
            position = new Vector2(350, 150);
            this.scale = new Vector2(1.0f, 1.0f);
            color = Color.Yellow;
            direction = new Vector2((float)(rand.NextDouble() * 50.0) - 25.0f, (float)(30.0f));
            
        }

        public override void Update(GameTime gameTime)
        {
            if (isAlive)
            {
                curTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (curTime < aliveTime)
                {
                    if (scale.X < 2.4f)
                    {
                        Vector2 scaleUp = new Vector2(scale.X + scaleSpeed , scale.Y + scaleSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        scale += scaleUp;

                        if (scale.X < 0.6)
                        {
                            color = Color.Lerp(color, Color.Red, 5 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        }
                        else if (scale.X < 1.2)
                        {
                            color = Color.Lerp(color, Color.Black, 3 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        }
                    }

                    //update position
                    position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    //change fading values
                    opacity -= fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                    isAlive = false;

                base.Update(gameTime);
            }
           

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isAlive)
                base.Draw(spriteBatch);
        }

        public void Respawn(Vector2 dir, Vector2 spawnPosition)
        {
            isAlive = true;
            aliveTime = 2.0f;
            curTime = 0.0f;
            position = spawnPosition;
            position.Y = spawnPosition.Y + 65.0f;
            this.scale = new Vector2(0.2f, 0.2f);
            color = Color.Yellow;
            opacity = 1;
            direction = dir;
        }
    }
}
