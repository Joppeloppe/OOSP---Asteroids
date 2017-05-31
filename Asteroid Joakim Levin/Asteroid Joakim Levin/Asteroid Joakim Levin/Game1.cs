using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroid_Joakim_Levin
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SoundEffect background_music;
        SoundEffect sound_city_hit;
        SoundEffect sound_asteroid_hit;
        SoundEffectInstance soundeffectinstance;
        Texture2D asteroid_texture;
        Texture2D background_texture;
        Texture2D city5_texture;
        Texture2D city4_texture;
        Texture2D city3_texture;
        Texture2D city2_texture;
        Texture2D city1_texture;
        Texture2D city0_texture;
        Rectangle city_boundingbox;
        HUD hud;

        int score;
        int life = 5;

        List<Asteroid> asteroid_list = new List<Asteroid>(); //Creates a list of Asteroids.
        Random random = new Random();

        bool running = true;                //When life = 0 the game stops by setting running to false.

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            this.Window.Title = "Asteroid           Asteroids destroyed: " + score + "          Buildings left: " + life;       //Writes out text in the window title.
            //Sets the screen size.
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 1220;
            graphics.IsFullScreen = false;

            random = new Random ();

            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            this.IsMouseVisible = true;                                     //Makes the mouse cursor visible.

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background_music = Content.Load<SoundEffect>(@"music_background");          //Loads the background music.
            sound_asteroid_hit = Content.Load<SoundEffect>(@"sound_asteroid_hit");      //Loads the sound that plays when an asteroid gets hit.
            sound_city_hit = Content.Load<SoundEffect>(@"sound_city_hit");              //Loads the sound that plays when an asteroid hits the city.
            asteroid_texture = Content.Load<Texture2D>(@"asteroid");                    //Loads the asteroid texture.
            background_texture = Content.Load<Texture2D>(@"background");                //Loads the background texture.
            city5_texture = Content.Load<Texture2D>(@"city_5");                        //Loads the city with 5 lives texture.
            city4_texture = Content.Load<Texture2D>(@"city_4");                        //Loads the city with 4 lives texture.
            city3_texture = Content.Load<Texture2D>(@"city_3");                        //Loads the city with 3 lives texture.
            city2_texture = Content.Load<Texture2D>(@"city_2");                        //Loads the city with 2 lives texture.
            city1_texture = Content.Load<Texture2D>(@"city_1");                        //Loads the city with 1 lives texture.
            city0_texture = Content.Load<Texture2D>(@"city_0");                        //Loads the destroyed city texture.


            city_boundingbox = new Rectangle(0, 400, 1220, 300);                     //Creates a boundingbox for the city.

            //Creates four asteroids at the start of the game.
            for (int i = 0; i < 4; i++)
                asteroid_list.Add(new Asteroid(new Vector2(random.Next(720 - 70), -70), asteroid_texture));

                soundeffectinstance = background_music.CreateInstance();
                soundeffectinstance.IsLooped = true;                                    //Makes the music_background loop during the game.
                soundeffectinstance.Play();

            hud = new HUD();
            hud.hudFont = Content.Load<SpriteFont>("TheFont");

        }

        protected override void UnloadContent()
        {
        }

        float spawn = 0;

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))     //Click the Escape button to close the program.
                this.Exit();

            KeyMouseReader.Update();

            if (running)
            {
                spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

                foreach (Asteroid a in asteroid_list)
                {

                    a.Update(gameTime);

                    if (KeyMouseReader.LeftClick() || KeyMouseReader.RightClick())
                    {
                        if (a.asteroid_boundingbox.Contains(KeyMouseReader.mouse_position))     //When you click this line will check to see if the mouse position is inside the asteroids boundingbox.
                        {
                            asteroid_list.Remove(a);            //If the mouse position is inside the asteroids boundingbox, the asteroid is removed.
                            sound_asteroid_hit.Play();          //It then plays the sound for asteroids getting hit.
                            hud.Score += 1;                     //Increases the score in the HUD by one per destoryed asteroid.
                            score = score + 1;                  //Increases the score in the window title by one per destroyed asteroid.
                            this.Window.Title = "Asteroid           Asteroids destroyed: " + score + "          Buildings left: " + life;
                            break;                              //Stops the if and updates the asteroid_list.
                        }
                    }


                    if (a.asteroid_boundingbox.Intersects(city_boundingbox))            //When the asteroids boundingbox hits the city's boundingbox
                    {
                        asteroid_list.Remove(a);                                        //The asteroid is removed.
                        sound_city_hit.Play();                                          //Then the sound is played for when an asteroid hits the city.
                        hud.Life -= 1;                                                  //Deceeases the amount of lives you have left in the HUD by one per asteroid that hits the city.
                        life = life - 1;                                                //Decreases the amount of lives you have left in the window title by one per asteroid that hits the city.
                        this.Window.Title = "Asteroid           Asteroids destroyed: " + score + "          Buildings left: " + life;
                        break;
                    }

                }

                if (life == 0)                  //When you run out of lives,
                {
                    this.Window.Title = "Asteroid           Asteroids destroyed: " + score + "          Buildings left: " + life + "      The city is lost!      Press Esc to run for your life!";
                    running = false;                //the game stops.
                }
            }

            LoadAsteroid();

            base.Update(gameTime);
        }

        //This makes sure that if there's less than three asteroids in-game it spawns asteroids untill there's seven of them.
        public void LoadAsteroid()
        {
            int randX = random.Next(50, 1170);

            if (spawn <= 2)
            {
                spawn = 0;
                if (asteroid_list.Count() < 7)
                {
                    asteroid_list.Add(new Asteroid(new Vector2(randX, -100), asteroid_texture));
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            spriteBatch.Draw(background_texture, new Vector2(0, 0), Color.White);

            //The game will draw out different city textures based of how many lives you have left.
            if (life == 5)
            {
                spriteBatch.Draw(city5_texture, new Vector2(0, 200), Color.White);
            }

            if (life == 4)
            {
                spriteBatch.Draw(city4_texture, new Vector2(0, 200), Color.White);
            }

            if (life == 3)
            {
                spriteBatch.Draw(city3_texture, new Vector2(0, 200), Color.White);
            }

            if (life == 2)
            {
                spriteBatch.Draw(city2_texture, new Vector2(0, 200), Color.White);
            }

            if (life == 1)
            {
                spriteBatch.Draw(city1_texture, new Vector2(0, 200), Color.White);
            }

            if (life == 0)
            {
                spriteBatch.Draw(city0_texture, new Vector2(0, 200), Color.White);
            }

            foreach (Asteroid a in asteroid_list)   //Draws every asteroid in asteroid_list.
                a.Draw(spriteBatch);

            hud.Draw(spriteBatch);                  //Draws the HUD.

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
