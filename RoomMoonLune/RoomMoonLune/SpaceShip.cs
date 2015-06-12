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
        bool isLeaving;

        public bool IsDocking
        { get { return isDocking; }
         set { isDocking = value; }
        }

        public bool IsLeaving
        {
            get { return isLeaving; }
            set { isLeaving = value; }
        }
        public float MaxHealth
        {
            get
            {
                return 100;
            }
        }
        public ParticleManager PManager { get { return particleManager; } }
        public SpaceShip(Texture2D texture,Texture2D particleTex, int width, int height,Random rand) : base(texture, width, height)
        {
            Health = 100;
            Collider.Offset = -60;
            Animation.Add("Left", new int[] { 0, 1, 2, 3 }, false, 4);
            Animation.Add("Right", new int[] { 4, 5, 6, 7 }, false, 4);
            scale = Vector2.One / 2;
            particleManager = new ParticleManager(particleTex, 64, 64, rand);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDocking && !isLeaving)
            {
                color = Color.Lerp(Color.Red, Color.White, Health / MaxHealth);

                if (Acceleration.X < 0)
                {
                    Animation.Play("Left");
                }
                else if (Acceleration.X > 0)
                {
                    Animation.Play("Right");
                }
                else
                {
                    Animation.Reverse();
                }

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

                RGlobal.Sound.Play("RocketJet", 0.1f, 0, 0, true);
            }
            else if(!isDocking && isLeaving)
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
            }
            else
            {

                RGlobal.Sound.Stop("RocketJet");

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            particleManager.Draw(spriteBatch);
        }
    }
}
