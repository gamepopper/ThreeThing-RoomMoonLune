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
    public enum TransitionOptions
    {
        TOLEVEL1,
        TOLEVEL2
    }

    class TransitionState : IRState
    {
        StarField starfield;
        Sprite ship;

        TransitionOptions option;

        public TransitionState(TransitionOptions option)
        {
            this.option = option;
        }

        public void Initialize()
        {
            if (option == TransitionOptions.TOLEVEL1)
            {

            }
            if (option == TransitionOptions.TOLEVEL2)
            {

            }
        }

        public void LoadContent(ContentManager Content)
        {
            starfield = new StarField(Content.Load<Texture2D>("Star"), 100, 100, new Random(),true);
            ship = new Sprite(Content.Load<Texture2D>("TempShip"), 160, 90);

            if (option == TransitionOptions.TOLEVEL1)
            {
                ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight);
                ship.Rotation = 3 * MathHelper.PiOver2;
                ship.Velocity.Y = -200;
            }
            if (option == TransitionOptions.TOLEVEL2)
            {
                ship.Position = new Vector2(RGlobal.Resolution.VirtualWidth / 2, RGlobal.Resolution.VirtualHeight/2);
                ship.Rotation = MathHelper.PiOver2;
                ship.Velocity.Y = 200;
            }
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            starfield.Update(gameTime);
            ship.Update(gameTime);

            if (option == TransitionOptions.TOLEVEL1)
            {
                if (ship.Position.Y < -ship.Collider.Box.Height)
                {
                    RGlobal.Game.SwitchState(new Stage1());
                }
            }
            if (option == TransitionOptions.TOLEVEL2)
            {
                if (ship.Position.Y > RGlobal.Resolution.VirtualHeight + ship.Collider.Box.Height)
                {
                    RGlobal.Game.SwitchState(new Stage2());
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            starfield.Draw(spriteBatch);
            ship.Draw(spriteBatch);

            if (option == TransitionOptions.TOLEVEL1)
            {

            }
            if (option == TransitionOptions.TOLEVEL2)
            {

            }
            spriteBatch.End();
        }
    }
}
