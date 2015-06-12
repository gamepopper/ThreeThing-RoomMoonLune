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
    class StarField
    {
        List<Sprite> stars = new List<Sprite>();
        public StarField(Texture2D texture,int width, int height,Random rand,bool fullScreen)
        {
            for (int i = 0; i < 100; i++)
            {
                stars.Add(new Sprite(texture, width, height));
                float scaleNum = (float)rand.NextDouble();
                if (scaleNum > 0.15f)
                    scaleNum = 0.15f;
                stars[i].Scale = new Vector2(scaleNum,scaleNum);
                if(!fullScreen)
                    stars[i].Position = new Vector2(rand.Next(0, RGlobal.Resolution.VirtualWidth), rand.Next(0, RGlobal.Resolution.VirtualHeight/6*4 - 40));
                else
                    stars[i].Position = new Vector2(rand.Next(0, RGlobal.Resolution.VirtualWidth), rand.Next(0, RGlobal.Resolution.VirtualHeight));

                int colorNum = rand.Next(0, 5);
                switch (colorNum)
                {
                    case 0:
                        stars[i].Color = Color.LightBlue;
                        break;
                    case 1:
                        stars[i].Color = Color.AntiqueWhite;
                        break;
                    case 2:
                        stars[i].Color = Color.WhiteSmoke;
                        break;
                    case 3:
                        stars[i].Color = Color.NavajoWhite;
                        break;
                    case 4:
                        stars[i].Color = Color.FloralWhite;
                        break;
                    default:
                        break;
                }

                float opactiyNum = (float)rand.NextDouble();
                stars[i].Opacity = opactiyNum;
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var star in stars)
            {
                star.Draw(spriteBatch);
            }
        }
    }
}
