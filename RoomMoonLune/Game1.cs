using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Ricoh2DFramework;
using Ricoh2DFramework.Audio;
using Ricoh2DFramework.Collisions;
using Ricoh2DFramework.Graphics;
using Ricoh2DFramework.Input;

namespace RoomMoonLune
{
    class Game1 : RGame
    {
        public Game1() : base(new TestState())
        {
            
        }
    }
}
