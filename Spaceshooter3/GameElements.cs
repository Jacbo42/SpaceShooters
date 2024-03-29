﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{
    //GameState, statisk klass med metoder för olika gamestates. 
    //Varje game-stae har två metoder vardera. en för update-funktionalitetet och en för draw-funktionalitet.
    //Quit har inga egna metoder utan gör så att spelet avslutas direkt

    static class GameElements
    {
        static Background background;
        
        static Texture2D menuSprite;
        static Vector2 menuPos;
        static Player player;
        static List<Enemy> enemies;
        static List<GoldCoin> goldCoins;
        static Texture2D goldCoinSprite;
        static PrintText printText;
        static Menu menu;
        static Shop shop;
        static Texture2D shopSprite;
        
        static Vector2 shopPos;
        static List<EnemyBullet> enemybullets;


        //Agenda

        //Ta bort "Continue" knappen i shoppen och istället gör det möjligt att bara tryck ESC, komma tillbaka till menyn,
        //sedan trycka t.ex. "c" för continue, vilket bara tar en tillbaka till samma spel man körde innan utan någon slags reset
        //Centrera shopmenyn så att den inte ser konstig ut, och ta bort dess förmåga att vara en knapp och gör det till en background istället.
        //Sedan gör två olika classer relaterade till player classen: Powerup och Upgrade. En av dem tillåter spelaren att använda powerups,
        //såsom invincibility kraften, medan den andra tillåter spelaren att upgradera sig själv, med extra fart och lägre tid mellan bullets.

        //Spelare måste kunna se hur mycket "cash" de har i shop menyn. Just nu gör de inte det


        //Efter detta är det kanske väldigt få prioriteringar kvar? Optimera levels osv?
                                                                                                                                                                                





        // olika gamestates

        public enum State { Menu, Run, HighScore, Quit, Shop, Continue};
        public static State currentState;

        //Initalize(); anropas av Game1.Initialize() då spelet startar. Här ligger all kod för att initiera objekt och skapa dem dock 
        //inte laddningen av olika filer

        public static void Initialize()
        {
            goldCoins = new List<GoldCoin>();
            enemybullets = new List<EnemyBullet>();

        }

        //LoadContent(), anropas av Game1.LoadContent() då spelet startar.
        //Här laddas alla objekt/filer in (bilder, ljud, etc)

        public static void LoadContent(ContentManager content, GameWindow window)
        {
            menu = new Menu((int)State.Menu);

            menu.AddItem(content.Load<Texture2D>("images/menu/continue"), (int)State.Continue);
            menu.AddItem(content.Load<Texture2D>("images/menu/start"), (int)State.Run);
            menu.AddItem(content.Load<Texture2D>("images/menu/highscore"), (int)State.HighScore);
            menu.AddItem(content.Load<Texture2D>("images/menu/exit"), (int)State.Quit);
            menu.AddItem(content.Load<Texture2D>("images/menu/GOTOSHOP"), (int)State.Shop);

            //Shop meny laddas in här
            shop = new Shop((int)State.Shop, player);
            shop.AddItem(content.Load<Texture2D>("images/shopmenu/shopbackground"), (int)State.Shop);



            background = new Background(content.Load<Texture2D>("images/background"), window);

            player = new Player(content.Load<Texture2D>("ship"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("images/player/bullet")); //Ändra ship delen

            //skapa fiender
            Genererafiender(window, content);


            goldCoinSprite = content.Load<Texture2D>("images/powerups/coin");
            printText = new PrintText(content.Load<SpriteFont>("myFont"));




        }

        //MenuUpdate(), kontrollerar om användaren väljer något av menyvalen

        public static State MenuUpdate(GameTime gameTime)
        {
            return (State)menu.Update(gameTime);
        }

        //MenuDraw(), ritar ut menyn

        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }

        public static State ShopUpdate(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape)) { return State.Menu; }
            if (keyboardState.IsKeyDown(Keys.C)) { return State.Continue; }

            return (State)shop.Update(gameTime);
        }

        //MenuDraw(), ritar ut menyn

        public static void ShopDraw(SpriteBatch spriteBatch)
        {
            shop.Draw(spriteBatch);
            printText.Print("Cash: " + player.Cash, spriteBatch, new Vector2(0, 15), Color.Black);

        }


        //RunUpdate(), update-metod för själva spelet

        public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime)
        {
            

            //Uppdatera spelarens position
            player.Update(window, gameTime);
            //Gå igenom alla fiender

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                return State.Menu;
            }

            foreach (Enemy e in enemies.ToList())
            {
                //Kontrollera om fienden kolliderar med ett skott
                foreach (Bullet b in player.Bullets)
                {
                    if (e.CheckCollision(b)) 
                    {
                        e.IsAlive = false;
                        player.Points++;
                    }
                }


              

                if (e.IsAlive) // Kontrollera om fienden lever
                {

                    if (e is UFO myUFO)
                    {
                        
                        foreach (EnemyBullet bullet in myUFO.EnemyBullets)
                        {

                            enemybullets.Add(bullet);
                        }
                    }

                    player.LoseLife(window, gameTime, player, e);
                    e.Update(window); //Flytta på dem
                }
                else // Ta bort fienden för den är död
                {
                    enemies.Remove(e);
                }

            }

            foreach (EnemyBullet e in enemybullets.ToList())
            {

                if (e.IsAlive) // Kontrollera om fienden lever
                {
                    player.LoseLifeBullet(window, gameTime, player, e);
                    e.Update(window); //Flytta på dem
                }
                else // Ta bort fienden för den är död
                {
                    enemybullets.Remove(e);
                }

            }

            //Ny level

            if (player.Points == 10)
            {
                player.Points = 0;
                Genererafiender(window, content);
                player.Level++;
                
            }
            

            //Guldmynten ska uppstå slumpmässigt, en chans på 200

            Random random = new Random();
            int newCoin = random.Next(1, 200);
            if (newCoin == 1) //Ok, nytt guldmynt ska uppstå
            {
                //var ska guldmyntet ska uppstå:
                int rndX = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);

                // lägg til myntet i listan
                goldCoins.Add(new GoldCoin(goldCoinSprite, rndX, rndY, gameTime));
            }
            // Gå igenom hela listan med existerande guldmynt
            foreach (GoldCoin gc in goldCoins.ToList())
            {
                if (gc.IsAlive)// Kontrollerar om guldmyntet lever
                {
                    //gc.Update() kollar om guldmyntet har blivit för gammalt
                    //för att få leva vidare
                    gc.Update(gameTime);

                    //Kontrollera om de kolliderat med spelaren
                    if (gc.CheckCollision(player))
                    {
                        // Ta bort myntet vid kollision:
                        goldCoins.Remove(gc);
                        player.Points++; // och ge spelaren poäng
                    }

                }
                if (gc.IsAlive)// Kontrollerar om guldmyntet lever
                {
                    //gc.Update() kollar om guldmyntet har blivit för gammalt
                    //för att få leva vidare
                    gc.Update(gameTime);

                    //Kontrollera om de kolliderat med spelaren
                    if (gc.CheckCollision(player))
                    {
                        // Ta bort myntet vid kollision:
                        goldCoins.Remove(gc);
                        player.Cash++; // och ge spelaren cash
                    }

                }

                else // ta bort guldmyntet för det är dött
                {
                    goldCoins.Remove(gc);
                }
            }

            if (!player.IsAlive) //spelaren är död
            {
                Reset(window, content);
                return State.Menu;

            }

            background.Update(window);


            return State.Run;


        }

        //RunDraw(), metod för att rita ut "själva spelet"

        public static void RunDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
            foreach (GoldCoin gc in goldCoins)
            {
                gc.Draw(spriteBatch);
            }
            printText.Print("Points: " + player.Points, spriteBatch, new Vector2(0, 0), Color.Black);
            printText.Print("Cash: " + player.Cash, spriteBatch, new Vector2(0, 15), Color.Black);
            printText.Print("Time: " + player.invulnerableTimer, spriteBatch, new Vector2(0, 30), Color.Black);
            printText.Print("Level: " + player.Level, spriteBatch, new Vector2(0, 45), Color.Black);

           



        }

        //HighScoreUpdate(), update-metod för highscore-listan

        public static State HighScoreUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            //Återgå till menyn om man trycker ESC
            if (keyboardState.IsKeyDown(Keys.Escape)) { return State.Menu; }
            return State.HighScore;

        }

        //HighScoreDraw(), ritar ut highscorelistan

        public static void HighScoreDraw (SpriteBatch spriteBatch)
        {
            //Rita ut highscore- listan
        }

        private static void Reset(GameWindow window, ContentManager content)
        {
            player.Reset(380, 400, 2.5f, 4.5f);

            // Skapa fiender
            enemies.Clear();

            Genererafiender(window, content);
            // gör en metod för att generera fiender, lika gärna
        }

        private static void Genererafiender(GameWindow window, ContentManager content)
        {
            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("mine");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Mine temp = new Mine(tmpSprite, rndX, rndY);
                enemies.Add(temp); //Lägg till i listan

            }
            tmpSprite = content.Load<Texture2D>("tripod");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Tripod temp = new Tripod(tmpSprite, rndX, rndY);
                enemies.Add(temp); // Lägg till i listan


            }
            tmpSprite = content.Load<Texture2D>("UFO");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                UFO temp = new UFO(tmpSprite, rndX, rndY, content.Load<Texture2D>("images/player/bullet"));
                enemies.Add(temp); //Lägg till i listan
            }
        }


    }
}
