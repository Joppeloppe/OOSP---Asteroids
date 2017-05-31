using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Asteroid_Joakim_Levin
{
    public class Asteroid
    {
        public Texture2D asteroid_texture;
        public Vector2 asteroid_position;
        public float asteroid_velocity;
        public Rectangle asteroid_boundingbox;

        Random random = new Random();
        int randY;

        public Asteroid(Vector2 position, Texture2D texture)
        {
            asteroid_position = position;
            asteroid_texture = texture;

            randY = random.Next(1, 3);      //Randoms the asteroids velocity by randoming how much the asteroid moves on the Y-axis.
            asteroid_velocity = randY;

        }

        public void Update(GameTime gameTime)
        {
            //asteroid_position.X += velocity;              //Makes the asteroid fly horisontally
            asteroid_position.Y += asteroid_velocity;       //Makes the asteroid fall vertically.
            //To make the asteroids fall diagonnally, activate both asteroid_postition.X and asteroid_position.Y 

            asteroid_boundingbox = new Rectangle((int)asteroid_position.X, (int)asteroid_position.Y, 75, 75); //Creates a boundingbox that follows the asteroid, the square is 75x75 pixels big.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(asteroid_texture, asteroid_position, Color.White);       //Draws the asteroid with awesome texture.
            
        }
        

    }
}
