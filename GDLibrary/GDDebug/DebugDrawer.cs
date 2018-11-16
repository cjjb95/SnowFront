/*
Function: 		Draws debug information to the screen for each camera (based on ScreenManager layout)
Author: 		NMCG
Version:		1.1
Date Updated:	11/9/17
Bugs:			None
Fixes:			None
Mods:           None
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GDLibrary
{
    public class DebugDrawer : PausableDrawableGameComponent
    {

        #region Fields 
        //statics
        private static readonly float DefaultLayerDepth = 0;
        private ManagerParameters managerParameters;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private Color textColor;
        private Vector2 textHoriVertOffset;
        private int totalElapsedTime;
        private Vector2 textPosition;
        private int frameCount;
        private StringBuilder fpsText;
        private float textHeight;
        #endregion

        #region Properties 
        #endregion
        public DebugDrawer(Game game, ManagerParameters managerParameters,
            SpriteBatch spriteBatch, SpriteFont spriteFont, Color textColor, Vector2 textHoriVertOffset, 
            EventDispatcher eventDispatcher,
            StatusType statusType)
            : base(game, eventDispatcher, statusType)
        {
            this.managerParameters = managerParameters;
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.textColor = textColor;
            this.textHoriVertOffset = textHoriVertOffset;

            this.fpsText = new StringBuilder("FPS:N/A");
            //measure string height so we know how much vertical spacing is needed for multi-line debug info
            this.textHeight = this.spriteFont.MeasureString(this.fpsText).Y;
        }

        protected override void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.DebugChanged += EventDispatcher_DebugChanged;
            base.RegisterForEventHandling(eventDispatcher);
        }

        #region Event Handling
        //enable dynamic show/hide of debug info
        private void EventDispatcher_DebugChanged(EventData eventData)
        {
            if(eventData.EventType == EventActionType.OnToggleDebug)
            {
                if (this.StatusType == StatusType.Off)
                    this.StatusType = StatusType.Drawn | StatusType.Update;
                else
                    this.StatusType = StatusType.Off;
            }
        }

        //Same as ScreenManager::EventDispatcher_MenuChanged i.e. show if we're in-game and not in-menu
        protected override void EventDispatcher_MenuChanged(EventData eventData)
        {
            //did the event come from the main menu and is it a start game event
            if (eventData.EventType == EventActionType.OnStart)
            {
                //turn on update and draw i.e. hide the menu
                this.StatusType = StatusType.Update | StatusType.Drawn;
            }
            //did the event come from the main menu and is it a start game event
            else if (eventData.EventType == EventActionType.OnPause)
            {
                //turn off update and draw i.e. show the menu since the game is paused
                this.StatusType = StatusType.Off;
            }
        }
        #endregion

        protected override void ApplyUpdate(GameTime gameTime)
        {

            //total time since last update to FPS text
            this.totalElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            this.frameCount++;

            //if 1 second has elapsed
            if (this.totalElapsedTime >= 1000)
            {
                //set the FPS text
                this.fpsText = new StringBuilder("FPS:" + this.frameCount);
                //reset the count and the elapsed time
                this.totalElapsedTime = 0;
                this.frameCount = 0;
            }
        }

        protected override void ApplyDraw(GameTime gameTime)
        {
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, null);
            if (this.managerParameters.ScreenManager.ScreenType == ScreenUtility.ScreenType.SingleScreen)
            {
                DrawDebugInfo(this.managerParameters.CameraManager.ActiveCamera);
            }
            else
            {
                foreach (Camera3D camera in this.managerParameters.CameraManager)
                    DrawDebugInfo(camera);
            }
            this.spriteBatch.End();
        }

        private void DrawDebugInfo(Camera3D camera)
        {     
            this.textPosition = new Vector2(camera.Viewport.X, camera.Viewport.Y) + this.textHoriVertOffset;
            this.spriteBatch.DrawString(this.spriteFont, "ID:" + camera.ID, this.textPosition, this.textColor, 
                0, Vector2.Zero, 1, SpriteEffects.None, DefaultLayerDepth); 

            this.textPosition.Y += this.textHeight;
            this.spriteBatch.DrawString(this.spriteFont, this.fpsText, this.textPosition, this.textColor,
                0, Vector2.Zero, 1, SpriteEffects.None, DefaultLayerDepth);

            this.textPosition.Y += this.textHeight;
            this.spriteBatch.DrawString(this.spriteFont, 
                "Pos:" + MathUtility.Round(camera.Transform.Translation, 1).ToString(), this.textPosition, this.textColor,
                0, Vector2.Zero, 1, SpriteEffects.None, DefaultLayerDepth);

            this.textPosition.Y += this.textHeight;
            this.spriteBatch.DrawString(this.spriteFont,
                "Look:" + MathUtility.Round(camera.Transform.Look, 1).ToString(), this.textPosition, this.textColor,
                0, Vector2.Zero, 1, SpriteEffects.None, DefaultLayerDepth);

            this.textPosition.Y += this.textHeight;
            this.spriteBatch.DrawString(this.spriteFont,
                "Nr. Drawn Obj.:" + this.managerParameters.ObjectManager.DebugDrawCount, this.textPosition, this.textColor,
                0, Vector2.Zero, 1, SpriteEffects.None, DefaultLayerDepth);
        }
    }
}
