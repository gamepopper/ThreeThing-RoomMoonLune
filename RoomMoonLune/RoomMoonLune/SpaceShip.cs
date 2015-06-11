using Microsoft.Xna.Framework.Graphics;
using Ricoh2DFramework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Ricoh2DFramework;
namespace RoomMoonLune
{
    class SpaceShip : Sprite
    {
        ParticleManager particleManager;
        public float Health;
        bool isDocking;

        public bool IsDocking
        { get { return isDocking; }
         set { isDocking = value; }
        }
        public float MaxHealth
        {
            get
            {
                return 100;
            }
        }

        public SpaceShip(Texture2D texture,Texture2D particleTex, int width, int height,Random rand) : base(texture, width, height)
        {
            Health = 100;
            particleManager = new ParticleManager(particleTex, 64, 64, rand);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDocking)
            {
                color = Color.Lerp(Color.Red, Color.White, Health / MaxHealth);

                particleManager.Update(gameTime, this);
                base.Update(gameTime);

                if (Position.Y + Collider.Box.Height / 2 > RGlobal.Resolution.VirtualHeight)
                {
                    Position = new Vector2(Position.X, RGlobal.Resolution.VirtualHeight - (Collider.Box.Height / 2));
                    Velocity.Y = 0;
                    Drag = new Vector2(1.01f, 1);
                }
                else if (Position.Y - Collider.Box.Height / 2 < 0)
                {
                    Position = new Vector2(Position.X, +(Collider.Box.Height / 2));
                    Velocity.Y = 0;
                    Drag = new Vector2(1.01f, 1);
                }
                else
                {
                    Drag = new Vector2(1.001f, 1);
                }

                if (Collider.Box.Right > RGlobal.Resolution.VirtualWidth + Collider.Box.Width)
                {
                    Position = new Vector2(-Collider.Box.Width / 2, Position.Y);
                }
                else if (Collider.Box.Left < -Collider.Box.Width)
                {
                    Position = new Vector2(RGlobal.Resolution.VirtualWidth + Collider.Box.Width / 2, Position.Y);
                }
            }
            else
            {

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            particleManager.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
