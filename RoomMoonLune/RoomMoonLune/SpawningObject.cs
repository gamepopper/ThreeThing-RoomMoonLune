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

using System.Diagnostics;
namespace RoomMoonLune
{
    enum EnemyType
    {
        asteroid,
        collectable
    }
    class SpawningObject : Sprite
    {
        #region Variables
        float               scaleSpeed;
        float               fadingSpeed = 0.8f;
        float               maxScale = 1.5f;
        Vector2             randomDirection;
        Random              rand;
        bool                isAlive;
        EnemyType           type;

        #endregion

        #region GetSetters
        public bool IsAlive { get { return isAlive; } }
        public EnemyType Type {  get { return type; } }
        #endregion
        public SpawningObject(Texture2D texture, int width, int height, Random rand): base (texture,width,height) 
        {
            this.rand = rand;
            this.scale = new Vector2(0.0f, 0.0f);
            scaleSpeed = 0.09f;
            SetStartingPosition();
            isAlive = false;

            int randomNum = rand.Next(0, 1);
            if (randomNum == 0)
            {
                type = EnemyType.asteroid;
                //Animation.Add("asteroid", new int[] { 0, 1, 2, 3, 4, 5 }, true);
                //Animation.Play("asteroid");
            }
            else if (randomNum == 1)
            {
                type = EnemyType.collectable;
                //Animation.Add("collectable", new int[] { 6, 7, 8, 9, 10, 11 }, true);
                //Animation.Play("collectable");
            }

           
        }

        public void SetStartingPosition()
        {
            position = new Vector2(RGlobal.Resolution.VirtualWidth, RGlobal.Resolution.VirtualHeight)/2;

            randomDirection = new Vector2((float)(rand.NextDouble() * 2.0) - 1.0f
                , (float)(rand.NextDouble() * 2.0) - 1.0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (isAlive)
            {
                //scale up to give effect of travelling closer to screen
                Vector2 scaleUp = new Vector2(scale.X + scaleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 
                    scale.Y + scaleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                //check if scaled to max then start fading out
                if (scale.X < maxScale)
                    scale = scaleUp;
                else
                {
                    if (opacity > 0.0f)
                        opacity -= fadingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        isAlive = false;
                }

                //update position
                position += randomDirection;
                base.Update(gameTime);
            }
        }

        public void Respawn()
        {
            isAlive = true;
            opacity = 1.0f;
            scale = new Vector2(0.0f, 0.0f);
            SetStartingPosition();
        }

        public void Kill()
        {
            isAlive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
           if(isAlive)
                base.Draw(spriteBatch);
        }

    }
}
