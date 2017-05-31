using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Joakim_Levin
{
    public class HUD
    {
        public SpriteFont hudFont;


        public Vector2 scorePosition = new Vector2(150, 470);
        public int Score = 0;

        public Vector2 lifePosition = new Vector2(810, 470);
        public int Life = 5;

        public HUD()
        { 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(hudFont, "Asteroids destroyed: " + Score.ToString(), scorePosition, Color.Violet);
            spriteBatch.DrawString(hudFont, "Buildings left: " + Life.ToString(), lifePosition, Color.Violet);
        }
    }
}
