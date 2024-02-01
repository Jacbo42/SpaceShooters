using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        static Background MenuBackground;

        
        static Player player;
        static List<Enemy> enemies;
        static List<GoldCoin> goldCoins;
        static Texture2D goldCoinSprite;
        static PrintText printText;
        static Menu menu;
        static Shop shop;
        static HighScore highscore;
        static StatMaster statMaster;
        

        static List<EnemyBullet> enemybullets;

        // olika gamestates

        public enum State { Menu, Run, HighScore, Quit, Shop };
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
            highscore = new HighScore(10);


            //HighScore textfilen
            highscore.LoadFromFile("highscore.txt");

            menu.AddItem(content.Load<Texture2D>("images/menu/start"), (int)State.Run);
            menu.AddItem(content.Load<Texture2D>("images/menu/highscore"), (int)State.HighScore);
            menu.AddItem(content.Load<Texture2D>("images/menu/exit"), (int)State.Quit);
            menu.AddItem(content.Load<Texture2D>("images/menu/GOTOSHOP"), (int)State.Shop);

            //Meny bakgrund

            MenuBackground = new Background(content.Load<Texture2D>("images/menu/MenuBackground"), window);

            //Shop meny och shopbackground laddas in här
            shop = new Shop((int)State.Shop);
            shop.AddItem(content.Load<Texture2D>("images/shopmenu/shopbackground0"), (int)State.Shop);
            
            //Statistik för spelaren
            statMaster = new StatMaster();

            //Generell bakground (alltså bara rymden)
            background = new Background(content.Load<Texture2D>("images/background"), window);

            //Själva spelkaraktären
            player = new Player(content.Load<Texture2D>("ship"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("images/player/bullet")); //Ändra ship delen

            //skapa fiender
            Genererafiender(window, content);


            goldCoinSprite = content.Load<Texture2D>("images/powerups/coin");
            printText = new PrintText(content.Load<SpriteFont>("myFont"));
        }

        /// <summary>
        /// Används för att transportera värden från StatMaster till andra classer. Detta är den enda riktiga användningen av globala variabler, tyvärr.
        /// </summary>
        /// <returns></returns>
        public static StatMaster GetStatMasterInstance()
        {
            
            return statMaster;
        }

        //MenuUpdate(), kontrollerar om användaren väljer något av menyvalen

        public static State MenuUpdate(GameTime gameTime)
        {
            
            return (State)menu.Update(gameTime);
        }

        //MenuDraw(), ritar ut menyn

        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            //skapar menybakgrund
            MenuBackground.Draw(spriteBatch);
            //skapar meny valen
            menu.Draw(spriteBatch);

        }
        /// <summary>
        /// Detta uppdaterar butiken enligt speltiden. Här kan man köpa uppgraderingar, men bara till en viss gräns
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public static State ShopUpdate(GameTime gameTime)
        {
            //Detta gör det möjligt för datorn att ta emot tangent input
            KeyboardState keyboardState = Keyboard.GetState();
            //Detta tillåter spelaren att få kortare tid mellan skott, men kan bara uppgraderas 3 gånger
            if (keyboardState.IsKeyDown(Keys.D1) && player.UpgradeLimit <= 3)
            {
                if (statMaster.Cash >= 10)
                {
                    player.UpgradeLimit++;
                    player.TimeBetweenBullets -= 50;
                    statMaster.Cash -= 10;
                }
                
            }
            //Tillåter spelaren att köpa nya liv.
            if (keyboardState.IsKeyDown(Keys.D2))
            {
                if (statMaster.Cash >= 10)
                {
                    player.Starterlives += 1;
                    statMaster.Cash -= 10;
                }

            }
            //tillbaks till meny
            if (keyboardState.IsKeyDown(Keys.Escape)) { return State.Menu; }

            //Uppdaterar igen
            return (State)shop.Update(gameTime);
        }


        public static void ShopDraw(SpriteBatch spriteBatch)
        {
            //Ritar butiken
            shop.Draw(spriteBatch);
            //Ritar värdet på ens pengar
            printText.Print("Cash: " + statMaster.Cash, spriteBatch, new Vector2(0, 15), Color.Black);

        }


        //RunUpdate(), update-metod för själva spelet

        public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime)
        {
            //Uppdatera spelarens position
            player.Update(window, gameTime);
            //Gå igenom alla fiender
            //Möjliggör tangent input
            KeyboardState keyboardState = Keyboard.GetState();
            //Tillbaks till meny
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                return State.Menu;
            }
            
            //Själva collision logiken med fiende och player skott
            foreach (Enemy e in enemies.ToList())
            {
                //Kontrollera om fienden kolliderar med ett skott
                foreach (Bullet b in player.Bullets)
                {
                    if (e.CheckCollision(b))
                    {
                        e.IsAlive = false;
                        statMaster.Points++;
                    }
                }


                
                if (e.IsAlive) // Kontrollera om fienden lever
                {

                    if (e is UFO myUFO) //Om fienden är ett UFO så...
                    {


                        for (int i = myUFO.EnemyBullets.Count - 1; i >= 0; i--)
                        {
                            EnemyBullet bullet = myUFO.EnemyBullets[i];
                            if (bullet.IsAlive)
                            {
                                enemybullets.Add(bullet);

                            }
                            else
                            {
                                bullet = null;

                            }
                        }

                        foreach (EnemyBullet bullet in myUFO.EnemyBullets) // ...ska enemybullet läggas till i en global lista
                        {
                            enemybullets.Add(bullet);
                            
                        }
                    }
                    //Spelare förlorar liv i kontakt med fiende

                    player.LoseLife(window, gameTime, player, e);
                    
                    e.Update(window); //Flytta på dem
                }
                else // Ta bort fienden för den är död
                {
                    enemies.Remove(e);
                }

            }
            //Collision logik med fiende skott
            for (int i = enemybullets.Count - 1; i >= 0; i--)
            {
                EnemyBullet e = enemybullets[i];

                if (e.IsAlive)
                {
                    // Spelare förlorar liv i kontakt med skott
                    player.LoseLifeBullet(window, gameTime, player, e);
                    
                    e = null;
                }
                else
                {
                    
                    enemybullets.RemoveAt(i); // Ta bort skottet för den är död
                    e = null;
                }
            }

            //Ny level om alla fiender är döda
            if (enemies.Count == 0)
            {
                Genererafiender(window, content);
                statMaster.Level++;
            }
            


            //Guldmynten ska uppstå slumpmässigt, en chans på 125

            Random random = new Random();
            int newCoin = random.Next(1, 125);
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
                        statMaster.Points++; // och ge spelaren poäng
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
                        statMaster.Cash++; // och ge spelaren cash
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
                return State.HighScore;
            }
            //Updaterar bakgrunden så att det "flyter" neråt
            background.Update(window);

            //Tar bort alla bullets
            enemybullets.Clear();
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
            //Ritar ut olika värden som spelaren har.
            printText.Print("Points: " + statMaster.Points, spriteBatch, new Vector2(0, 0), Color.Black);
            printText.Print("Cash: " + statMaster.Cash, spriteBatch, new Vector2(0, 15), Color.Black);
            printText.Print("Time: " + Math.Ceiling(player.invulnerableTimer), spriteBatch, new Vector2(0, 30), Color.Black);
            printText.Print("Level: " + statMaster.Level, spriteBatch, new Vector2(0, 45), Color.Black);
            printText.Print("Lives: " +  player.Starterlives, spriteBatch, new Vector2(0, 60), Color.Black);


        }

        //HighScoreUpdate(), update-metod för highscore-listan

        public static State HighScoreUpdate(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            //Återgå till menyn om man trycker ESC
            if (keyboardState.IsKeyDown(Keys.Escape))
            { statMaster.Reset(); return State.Menu; }
            return State.HighScore;

        }

        //HighScoreDraw(), ritar ut highscorelistan

        public static void HighScoreDraw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Detta ritar ut highscore menyn, men till stor del sker allt "arbete" i HighScore classen själv, så denna metod behöver endast existera
        }

        //Börjar om spelet

        private static void Reset(GameWindow window, ContentManager content)
        {
            player.Reset(380, 400, 2.5f, 4.5f);
            // Skapa fiender
            enemies.Clear();

            Genererafiender(window, content);
        }
        /// <summary>
        /// Skapar fiender, såsom mine, tripod och UFO
        /// </summary>
        /// <param name="window"></param>
        /// <param name="content"></param>
        private static void Genererafiender(GameWindow window, ContentManager content)
        {
            //Sätter fiender i en lista...
            enemies = new List<Enemy>();
            Random random = new Random();
            //Laddar in texture...
            Texture2D tmpSprite = content.Load<Texture2D>("mine");
            for (int i = 0; i < 5; i++)
            {
                //Placerar ut fienden...
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Mine temp = new Mine(tmpSprite, rndX, rndY);
                enemies.Add(temp); //Lägg till i listan

            }
            //repetera dessa steg för alla andra fiender
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
                //UFO laddar in bullet texturen
                UFO temp = new UFO(tmpSprite, rndX, rndY, content.Load<Texture2D>("images/player/bullet"));
                enemies.Add(temp); //Lägg till i listan
            }
        }
    }
}
