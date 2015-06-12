using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ricoh2DFramework;

namespace RoomMoonLune
{
    class TransitionState : IRState
    {
        StarField starfield;

        public void Initialize()
        {
            
        }

        public void LoadContent(ContentManager Content)
        {
            starfield = new StarField(Content.Load<Texture2D>("Star"), 100, 100, new Random());
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            starfield.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            starfield.Draw(spriteBatch);
        }
    }
}
