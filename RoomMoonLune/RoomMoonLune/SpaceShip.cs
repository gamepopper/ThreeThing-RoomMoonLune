using Microsoft.Xna.Framework.Graphics;
using Ricoh2DFramework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RoomMoonLune
{
    class SpaceShip : Sprite
    {
        public float Health;
        public float MaxHealth
        {
            get
            {
                return 100;
            }
        }

        public SpaceShip(Texture2D texture, int width, int height) : base(texture, width, height)
        {
            Health = 100;
        }

        public override void Update(GameTime gameTime)
        {
            color = Color.Lerp(Color.Red, Color.White, Health / MaxHealth);

            base.Update(gameTime);
        }
    }
}
