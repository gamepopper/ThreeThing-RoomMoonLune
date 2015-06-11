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
    class ParticleManager 
    {
        List<ThrustParticles> particles = new List<ThrustParticles>();
        float spawnTime = 0.15f;
        float curTime = 0.0f;
        Random rand;
        Vector2 spawnPosition;
        public List<ThrustParticles> Particles { get { return particles; } }
        public ParticleManager(Texture2D texture, int width, int height, Random rand)
        {
            this.rand = rand;
            for (int i = 0; i <= 50; i++)
            {
                particles.Add(new ThrustParticles(texture, width, height,rand));
            }

        }

        public void Update(GameTime gameTime, SpaceShip ship)
        {
            spawnPosition = ship.Position;

            curTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (curTime > spawnTime)
            {
                foreach (var particle in particles)
                {
                    if (!particle.IsAlive)
                    {
                        float angle = ship.Rotation;
                        angle -= 3 * MathHelper.PiOver2;
                        

                        particle.Respawn(new Vector2((float)(rand.NextDouble() * 50.0) - 25.0f - (angle * 20), (float)( 150.0f)), spawnPosition - new Vector2(angle * 75, 0));
                        curTime = 0.0f;
                        break;
                    }
                }
            }
            foreach (var particle in particles)
            {
                particle.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
