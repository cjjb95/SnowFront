using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class DebugDrawer : PausableDrawableGameComponent
    {
        #region Fields
        private SpriteBatch spriteBatch;
        private CameraManager cameraManager;
        private SpriteFont spriteFont;
        private Vector2 position;
        private Color color;
        private int fpsRate;
        private int totalTime, count;
        private string strInfo = "Drive - Numpad[8,5,4,6,1,3], O/P - pause/play controller on torus";
        private Vector2 positionOffset = new Vector2(0, 20);
        private int fpsTime;
        private float temperature;
        private float dropRate;
        private string status;
        private string slipStatus;
        private EventDispatcher eventDispatcher;
        private bool iceEntered;
        private int totalTimeOnIce;
        private int slipChance;
        private Random rnd;
        private int randomNum;
        #endregion

        public DebugDrawer(Game game, CameraManager cameraManager, SpriteBatch spriteBatch,
            SpriteFont spriteFont, Vector2 position,
            float temperature, float dropRate,
            Color color, EventDispatcher eventDispatcher,
            StatusType statusType) : base(game, eventDispatcher, statusType)
        {
            this.spriteBatch = spriteBatch;
            this.cameraManager = cameraManager;
            this.spriteFont = spriteFont;
            this.position = position;
            this.color = color;
            this.temperature = temperature;
            this.status = "alive";
            this.slipStatus = "not slip";
            this.dropRate = dropRate;
            this.eventDispatcher = eventDispatcher;
            this.rnd = new Random();

            //run any event in this method!
            DemoEventHandling(eventDispatcher);
        }

        public void DemoEventHandling(EventDispatcher eventDispatcher)
        {	// call event CoatOn
            this.eventDispatcher.CoatOn += EventDispatcher_CoatOn;
            this.eventDispatcher.EnteringIce += EventDispatcher_EnteringIce;

        }

        private void EventDispatcher_EnteringIce(EventData eventData)
        {
            this.iceEntered = true;
        }

        private void EventDispatcher_CoatOn(EventData eventData)
        {
            // if eventType is OnCoat
            if (eventData.EventType == EventActionType.OnCoat)// wears the coat
            {
                this.dropRate -= 0.2f;
            }
            else if (eventData.EventType == EventActionType.OffCoat)// take of coat
            {
                this.dropRate += 0.2f;
            }
        }

        protected override void ApplyUpdate(GameTime gameTime)
        {
            this.fpsTime += gameTime.ElapsedGameTime.Milliseconds;
            this.totalTime += gameTime.ElapsedGameTime.Milliseconds;
            
            this.count++;

            if (iceEntered)
            {
                this.totalTimeOnIce += gameTime.ElapsedGameTime.Milliseconds;
                if(totalTimeOnIce % 1000 == 0)
                {
                    this.slipChance++;
                    this.randomNum = rnd.Next(3, 8);
                    Console.WriteLine(randomNum);

                    if(randomNum<=slipChance)
                    {
                        slipStatus = "U Slip";
                        this.slipChance = 0;
                    }
                }
                
            }

            if (this.temperature < 24)
            {
                this.status = "dead";
            }

            if (this.totalTime % 1000 == 0)
            {
                this.temperature -= this.dropRate;
            }

            if (this.fpsTime >= 1000) //1 second
            {
                this.fpsRate = count;
                this.fpsTime = 0;
                this.count = 0;
            }

            base.ApplyUpdate(gameTime);
        }

        protected override void ApplyDraw(GameTime gameTime)
        {
            //draw the debug info for all of the cameras in the cameramanager
            foreach (Camera3D activeCamera in this.cameraManager)
            {
                //set the viewport dimensions to the size defined by the active camera
                Game.GraphicsDevice.Viewport = activeCamera.Viewport;
                this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                //frame rate
                this.spriteBatch.DrawString(this.spriteFont, "FPS: " + this.fpsRate, this.position, this.color);
                //camera info
                this.spriteBatch.DrawString(this.spriteFont, activeCamera.GetDebugDescription(), this.position + this.positionOffset, this.color);
                //str info
                this.spriteBatch.DrawString(this.spriteFont, this.strInfo, this.position + 2 * this.positionOffset, this.color);
                this.spriteBatch.DrawString(this.spriteFont, "Temperature: " + this.temperature.ToString("F1"),
                this.position + 3 * this.positionOffset, this.color);
                this.spriteBatch.DrawString(this.spriteFont, "Status: " + this.status, this.position + 4 * this.positionOffset, this.color);
                this.spriteBatch.DrawString(this.spriteFont, "DropRate: " + this.dropRate, this.position + 5 * this.positionOffset, this.color);
                this.spriteBatch.DrawString(this.spriteFont, "SlipChance: " + this.slipChance, this.position + 6 * this.positionOffset, this.color);
                this.spriteBatch.DrawString(this.spriteFont, "Slip Status: " + this.slipStatus, this.position + 7 * this.positionOffset, this.color);
                this.spriteBatch.End();
            }

            base.ApplyDraw(gameTime);
        }
    }
}
