using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaTi
{    
    public  class Game1 : Microsoft.Xna.Framework.Game
    {
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
//Változók deklarálása:
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Font1; 
        Texture2D hattersprite, bal_kijelol, jobb_kijelol, analog, balpanel, jobbpanel;
        double datiatlagleutes,elteltperc,zeroleutes;
        string szoveg=String.Empty,balchar,jobbchar,elbalchar,eljobbchar,counting="Counting...",output2,clickhang;
        float left_x, left_y, right_x, right_y;
        int left_angle,right_angle,balp_pozic,jobbp_pozic,kozeledspeed=14,karakterszam=0,updateido=0;
        AnimatedSprite animhelp;
        bool isConnected,activatedhelp = false,l2lenyomva,r2lenyomva,ell2lenyomva,elr2lenyomva,activated=false;
        Vector2 balpanelhely,jobbpanelhely;
        ButtonState ell1, elr1, elhelp, elspace1, elspace2, eltorol, mosthelp,mostl1,mostr1,mostspace1,mostspace2,mosttorol;
        SoundEffect click, kijelolvalto;
        SoundEffectInstance instanceclick, instancekijelolvalto;
        
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/    
        public Game1()
        {   graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
//Meghatározzuk a kívánt felbontást(lehetelőleg ne változtasd
            graphics.PreferredBackBufferHeight = 576;
            graphics.PreferredBackBufferWidth = 1024;
        }
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        private double RandomNumber(int min, int max)
        {
            Random szerencseszam = new Random();
            return szerencseszam.NextDouble();
        }


        private void balkar(int min, int max, string nagy, string spec, string kischar)
        {string kis=kischar.Substring(1,1);
            if (left_angle <= max & left_angle > min & l2lenyomva)
            { balchar = nagy; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += nagy; } }
            else
            {
                if (left_angle <= max & left_angle > min & r2lenyomva)
                { balchar = spec; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += spec; } }

                else
                {
                    if (left_angle <= max & left_angle > min) { balchar = kischar; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += kis; } }
                }
            }
        }
 /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        private void jobbkar(int min, int max, string nagy, string specchar, string spec, string kischar, string kis)
        {
            if (right_angle <= max & right_angle > min & l2lenyomva)
            { jobbchar = nagy; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += nagy; } }
            else
            {
                if (right_angle <= max & right_angle > min & r2lenyomva)
                { jobbchar = specchar; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += spec; } }
                else
                {
                    if (right_angle <= max & right_angle > min) { jobbchar = kischar; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += kis; } }
                }
            }
        }
        
/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        protected override void Initialize()
        { base.Initialize();}    
        protected override void LoadContent()
        {   spriteBatch = new SpriteBatch(GraphicsDevice);           
            Font1 = Content.Load<SpriteFont>("Comic Sans MS");  
            hattersprite=Content.Load<Texture2D>("hatter");
            analog = Content.Load<Texture2D>("analog");
            balpanel = Content.Load<Texture2D>("balpanel");
            jobbpanel = Content.Load<Texture2D>("jobbpanel");
            Texture2D texture = Content.Load<Texture2D>("animsheet");
            animhelp = new AnimatedSprite(texture,6,7);
            kijelolvalto = Content.Load<SoundEffect>("kijelolovalto");
            instancekijelolvalto = kijelolvalto.CreateInstance();
            
            
        }
        protected override void UnloadContent()
        {}    
        protected override void Update(GameTime gameTime){  
//Kilépés:         
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                this.Exit();
//Az analógok x és y koordinátáinak meghatározása:
            left_x = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            left_y = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
            right_x = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
            right_y = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
//Szögkiszámítás:          
            left_angle = Convert.ToInt32(Math.Round((float)Math.Atan2(left_y, left_x) * 57));
            right_angle = Convert.ToInt32(Math.Round((float)Math.Atan2(right_y, right_x) * 57)); ;
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/                
// Bekérjük a mostani lenyomott billenytűket.
                 mostl1 = GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder;
                 mostr1 = GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder;
                 mostspace1 = GamePad.GetState(PlayerIndex.One).Buttons.LeftStick;
                 mostspace2 = GamePad.GetState(PlayerIndex.One).Buttons.RightStick;
                 mosttorol = GamePad.GetState(PlayerIndex.One).Buttons.X;
                 mosthelp = GamePad.GetState(PlayerIndex.One).Buttons.Y;
                isConnected = GamePad.GetState(PlayerIndex.One).IsConnected;
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
// Csak akkor dolgozzuk fel az információt ha be van dugva a kontroller.
                if (isConnected)
                {
                    
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/                  
// Ha nincs lenyomva semmi akkor legyen a kiválasztás képe üres...
                                       if (left_x==0 & left_y==0)
                                       { balchar = "balk_ures"; }
                                       if (right_x== 0 & right_y== 0)
                                       { jobbchar = "jobbk_ures"; }
//Ha a felénél jobban le van nyomva valamelyik alsó kar a legyen igaz a megfelelő logikai változó
                                       if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 0.5f)
                                       { if (l2lenyomva == false) { instancekijelolvalto.Play(); }; balchar = "bal_ures"; jobbchar = "jobb_ures"; l2lenyomva = true; }
                                       else { l2lenyomva = false; }
                                       if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 0.5f)
                                       { if (r2lenyomva == false) { instancekijelolvalto.Play(); } balchar = "bku"; jobbchar = "jku"; r2lenyomva = true; }
                                       else
                                       { r2lenyomva = false; }



                                       double randomszam = RandomNumber(0, 1);
                                       if (randomszam > 0.5f) { clickhang = "click"; } else { clickhang = "click2"; }
                                       click = Content.Load<SoundEffect>(clickhang);
                                       instanceclick = click.CreateInstance();
                                       
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Balkar
                                         Példa:
                    
                                         ha balkarfok<= fok1 és balkarfok> fok2 és az l2 le van nyomva
                                         akkor  balkarkiválasztáskép: nagybetű; ha le van nyoma az l1 ,de eddig nem volt, akkor adjon hozzá a szöveghez egy "nagybetű"-t;
                                         más esetben
                                         ha balkarfok<= fok1 és balkarfok> fok2 és az r2 le van nyomva
                                         akkor  balkarkiválasztáskép: specbetű; ha le van nyoma az l1 ,de eddig nem volt, akkor adjon hozzá a szöveghez egy "specbetű"-t
                                         más esetben
                                         ha balkarfok<= fok1 és balkarfok> fok2 
                                         akkor 
                                         akkor  balkarkiválasztáskép: kisbetű; ha le van nyoma az l1 ,de eddig nem volt, akkor adjon hozzá a szöveghez egy "kisbetű"-t
                     
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/         
                    balkar(-27, -1, "G", "5", "kg");

                    if (left_angle <= 25 & left_angle > -1 & (Convert.ToInt32(left_x) != int.Parse("0")) & l2lenyomva)
                    { balchar = "F"; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += "F"; } }
                    else
                    {
                        if (left_angle <= 25 & left_angle > -1 & (Convert.ToInt32(left_x) != int.Parse("0")) & r2lenyomva)
                        { balchar = "4"; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += "4"; } }

                        else
                        {
                            if (left_angle <= 25 & left_angle > -1 & (Convert.ToInt32(left_x) != int.Parse("0"))) { balchar = "kf"; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += "f"; } }
                        }
                    }
                    balkar(25, 51, "T", "3", "kt");
                    balkar(51, 77, "R", "2", "kr");
                    balkar(77, 103, "E", "1",  "ke");
                    balkar(103, 129, "W", "0",  "kw");
                    balkar(129, 153, "Q", "@",  "kq");
                    balkar(153, 179, "A", "$",  "ka");


                    if ((left_angle <= 180 & left_angle > 179) | (left_angle <= -155 & left_angle > -180) & l2lenyomva)
                    { balchar = "S"; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += "S"; } }
                    else
                    {
                        if ((left_angle <= 180 & left_angle > 179) | (left_angle <= -155 & left_angle > -180) & r2lenyomva)
                        { balchar = "csillag"; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += "*"; } }
                        else
                        {
                            if ((left_angle <= 180 & left_angle > 179) | (left_angle <= -155 & left_angle > -180)) { balchar = "ks"; if (mostl1 == ButtonState.Pressed & ell1 == ButtonState.Released) { szoveg += "s"; } }
                        }
                    }

                    balkar(-155, -129, "D", "_",  "kd");
                    balkar(-129, -104, "Z", "9",  "kz");
                    balkar(-104, -79, "X", "8",  "kx");
                    balkar(-79, -52, "C", "7",  "kc");
                    balkar(-52, -27, "V", "6",  "kv");
                    
               
                    //Betöltöm a kiválasztás képét...
                    bal_kijelol = Content.Load<Texture2D>(balchar);
/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
                    jobbkar(-27, -1, "K", "%", "%", "kk", "k");
                    
                    if (right_angle <= 25 & right_angle > -1 & (Convert.ToInt32(right_x) != int.Parse("0")) & l2lenyomva)
                    { jobbchar = "L"; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += "L"; } }
                    else
                    {
                        if (right_angle <= 25 & right_angle > -1 & (Convert.ToInt32(right_x) != int.Parse("0")) & r2lenyomva)
                        { jobbchar = "ketaposztrof"; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += "\""; } }
                        else
                        {
                            if (right_angle <= 25 & right_angle > -1 & (Convert.ToInt32(right_x) != int.Parse("0"))) { jobbchar = "kl"; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += "l"; } }
                        }
                    }
                    jobbkar(25, 51, "P", ";", ";", "kp", "p");
                    jobbkar(51, 77, "O", ")", ")", "ko", "o");
                    jobbkar(77, 103, "I", "kettospont", ":", "ki", "i");
                    jobbkar(103, 129, "U", "(", "(", "ku", "u");
                    jobbkar(129, 153, "Y", "+", "+", "ky", "y");
                    jobbkar(153, 179, "J", "=", "=", "kj", "j");

                    
                    if ((right_angle <= 180 & right_angle > 179) |(right_angle <= -155 & right_angle > -180)& l2lenyomva)
                    { jobbchar = "H"; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += "H"; } }
                    else
                    {
                        if ((right_angle <= 180 & right_angle > 179) | (right_angle <= -155 & right_angle > -180) & r2lenyomva)
                        { jobbchar = "#"; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += "#"; } }
                        else
                        {
                            if ((right_angle <= 180 & right_angle > 179) |(right_angle <= -155 & right_angle > -180)) { jobbchar = "kh"; if (mostr1 == ButtonState.Pressed & elr1 == ButtonState.Released) { szoveg += "h"; } }
                        }
                    }
                    jobbkar(-155, -129, "B", "bper", "\\", "kb", "b");
                    jobbkar(-129, -104, "N", "bkacsa", "<", "kn", "n");
                    jobbkar(-104, -79, "M", "-", "-", "km", "m");
                    jobbkar(-79, -52, "'", "jkacsa", ">", "k.", ".");
                    jobbkar(-52, -27, "!", "jper", "/", "kkerdo", "?");
                    jobb_kijelol = Content.Load<Texture2D>(jobbchar);
/*---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
//Szövegműveletek     
//Szóköz
                    if (mostspace1 == ButtonState.Pressed & elspace1 == ButtonState.Released)
                    { szoveg += " "; }
                    if (mostspace2 == ButtonState.Pressed & elspace2 == ButtonState.Released)
                    { szoveg += " "; }
                    if (szoveg.Length != 0)
/*Gyorstörlés*/     {    if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed && mosttorol == ButtonState.Pressed)
                            { szoveg = szoveg.Substring(0, (szoveg.Length - 1)); }
                        else
 /*Törlés*/             {if (mosttorol == ButtonState.Pressed & eltorol == ButtonState.Released)
                            { szoveg = szoveg.Substring(0, (szoveg.Length - 1)); }}
                    }
/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/                    
//Animáció/lejátszási sebesség
                    bool kijeloleslejatszas;
                    updateido++;
                    if (updateido>30) { updateido = 0; }
                    if (activatedhelp == true & balpanelhely.X == 0& updateido==30) { animhelp.Update(); };
                    if ((l2lenyomva & ell2lenyomva == false) | (r2lenyomva & elr2lenyomva == false)) { kijeloleslejatszas = true; instancekijelolvalto.Play(); }
                    else { kijeloleslejatszas = false; }
                    if ((balchar != elbalchar) | (jobbchar != eljobbchar)) { if (kijeloleslejatszas == false & (elbalchar != "bal_ures" | eljobbchar != "jobb_ures") & (eljobbchar != "jku" | elbalchar != "bku") & activated == false) { instanceclick.Play(); } }
                    
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
//Panelek megjelenítése
                    if (mosthelp == ButtonState.Pressed & elhelp == ButtonState.Released)
                    { if (activatedhelp == true & balpanelhely.X == 0) {  activatedhelp=false;}
                      else
                      {activatedhelp = true;}
                    }
                    if (activatedhelp == false)
                    { if (balp_pozic != 0) { balp_pozic -= kozeledspeed; } if (jobbp_pozic != 0) { jobbp_pozic += kozeledspeed; } if (balp_pozic < 0) { balp_pozic = 0; } if (jobbp_pozic > 0) { jobbp_pozic = 0; } }
                    if(activatedhelp==true){
                     if (balp_pozic < balpanel.Width) { balp_pozic += kozeledspeed; }; if (jobbp_pozic > -jobbpanel.Width) { jobbp_pozic -= kozeledspeed; } if (balp_pozic > balpanel.Width) { balp_pozic = balpanel.Width; } if (jobbp_pozic > jobbpanel.Width) { jobbp_pozic = jobbpanel.Width; } }
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/                  
//Leütés/perc   
                    
                    elteltperc =gameTime.TotalGameTime.TotalMinutes;
                    if (elteltperc == 0) { elteltperc = 0.00000001; }
                    if (karakterszam < szoveg.Length) { karakterszam = szoveg.Length; }
                    if (szoveg.Length==0) { zeroleutes = gameTime.TotalGameTime.TotalMinutes; zeroleutes-=0.00001f;}
                    datiatlagleutes = karakterszam / (elteltperc - zeroleutes);
                    if (datiatlagleutes > 300) {  }
                    
                    String.Format("{0:0.##}", datiatlagleutes);
                }
                else
                {
                    hattersprite = Content.Load<Texture2D>("notfound");
                   
                }
/* -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
 //A mostani lenyomott gombokat eltároljuk a régi változóban.
                    ell1 = mostl1;
                    elr1 = mostr1;
                    elspace1 = mostspace1;
                    elspace2 = mostspace2;
                    eltorol = mosttorol;
                    elhelp = mosthelp;
                    ell2lenyomva = l2lenyomva;
                    elr2lenyomva = r2lenyomva;
                    elbalchar= balchar;
                    eljobbchar= jobbchar;
                
                base.Update(gameTime);
    }

        protected override void Draw(GameTime gameTime)
        { GraphicsDevice.Clear(Color.Black);
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
//Sprite-ok helyének meghatározása
                    jobbpanelhely = new Vector2(GraphicsDevice.Viewport.X+706+jobbpanel.Width+jobbp_pozic , GraphicsDevice.Viewport.Y);
                    balpanelhely = new Vector2(GraphicsDevice.Viewport.X-balpanel.Width+balp_pozic,GraphicsDevice.Viewport.Y);
                    Vector2 outputhely = new Vector2(GraphicsDevice.Viewport.X+173,GraphicsDevice.Viewport.Y+160);
                    Vector2 balkijhely = new Vector2(GraphicsDevice.Viewport.X+299,GraphicsDevice.Viewport.Y+209);
                    Vector2 balanaloghely = new Vector2(GraphicsDevice.Viewport.X + 357 + (left_x * 10), GraphicsDevice.Viewport.Y + 267 - (left_y * 10));
                    Vector2 jobbanaloghely = new Vector2(GraphicsDevice.Viewport.X + 591 + (right_x * 10), GraphicsDevice.Viewport.Y + 267 - (right_y * 10));
                    Vector2 jobbkijhely = new Vector2(GraphicsDevice.Viewport.X + 533, GraphicsDevice.Viewport.Y + 209);
                    Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    Vector2 datihely = new Vector2(balpanelhely.X+130, GraphicsDevice.Viewport.Y + 243);
                    Vector2 helpanimhely = new Vector2(balpanelhely.X+790, GraphicsDevice.Viewport.Y + 236);
                    Vector2 keyboardhely = new Vector2(balpanelhely.X+130, GraphicsDevice.Viewport.Y + 284);
                    Vector2 ps3hely = new Vector2(balpanelhely.X+ 130, GraphicsDevice.Viewport.Y + 324);
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------     
!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Minél lejjeb van annál magasabban helyezkedik el a képen!!!(rétegek)!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/ 
            spriteBatch.Begin();
            spriteBatch.Draw(hattersprite, screen , Color.White);           
            spriteBatch.End();
            
            if (isConnected)
            {if(datiatlagleutes<300){output2=datiatlagleutes.ToString(".");}else{output2=counting;}
                spriteBatch.Begin();
                spriteBatch.Draw(hattersprite, screen, Color.White);
                spriteBatch.Draw(balpanel, balpanelhely, Color.White); spriteBatch.Draw(jobbpanel, jobbpanelhely, Color.White);
                spriteBatch.Draw(bal_kijelol, balkijhely, Color.White); spriteBatch.Draw(jobb_kijelol, jobbkijhely, Color.White);
                spriteBatch.DrawString(Font1, szoveg, outputhely, Color.White);
                spriteBatch.DrawString(Font1,output2, datihely, Color.White);
                spriteBatch.DrawString(Font1, "90", keyboardhely, Color.White);
                spriteBatch.DrawString(Font1, "62", ps3hely, Color.White);
                spriteBatch.Draw(analog, balanaloghely, Color.White); spriteBatch.Draw(analog, jobbanaloghely, Color.White);
                spriteBatch.End();
            }
           
/*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
//Animáció kirajzolása
            if (activatedhelp == true & balpanelhely.X == 0)
            {
                animhelp.Draw(spriteBatch, helpanimhely);
            }
            
            base.Draw(gameTime);
        }
        }
    }
/*Easter Egg's:
Thank you Marton Miklos from Sony Contact Center for your professional help to provide project's forwarding to Sony Entertainment!
*/