using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ricoh2DFramework;
using Ricoh2DFramework.Graphics;
namespace RoomMoonLune
{
    class SpawningObject : Sprite
    {
        float               scaleSpeed;
        Random              rand;
        Vector2             randomDirection;

        public SpawningObject(Texture2D texture, int width, int height): base (texture,width,height) 
        {
            rand = new Random();
            double mantissa = (rand.NextDouble() * 2.0) - 1.0;

            
            this.scale = new Vector2(0.0f, 0.0f);
            scaleSpeed = 0.03f;
            
            SetStartingPosition();

           
            randomDirection = new Vector2((float)(rand.NextDouble() * 2.0) - 1.0f 
                , (float)(rand.NextDouble() * 2.0) - 1.0f);

           


        }

        public void SetStartingPosition()
        {
            position = new Vector2(RGlobal.Resolution.VirtualWidth, RGlobal.Resolution.VirtualHeight)/2;
            //new Vector2(rand.Next(0,RGlobal.Resolution.VirtualWidth),rand.Next(0, RGlobal.Resolution.VirtualHeight));
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 scaleUp = new Vector2(scale.X + scaleSpeed* (float)gameTime.ElapsedGameTime.TotalSeconds
                , scale.Y + scaleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (scale.X < 0.4)
            {
                scale = scaleUp;
            }

            position += randomDirection;
        }

    }
}
