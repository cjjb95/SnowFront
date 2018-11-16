/*
Function: 		Enables support for split screen and overlapping (e.g. rear-view mirror) camera viewports 
Author: 		NMCG
Version:		1.0
Date Updated:	24/8/17
Bugs:			Need to address bug when one camera view is "inside another" i.e. over-lapping screen space
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class ScreenManager : PausableDrawableGameComponent
    {
        #region Fields
        private ScreenUtility.ScreenType screenType;
        private ObjectManager objectManager;
        private CameraManager cameraManager;

        //showing and hiding the menu - see ApplyUpdate()
        private KeyboardManager keyboardManager;
        private Keys pauseKey;

        private bool bLayoutDirty = true;
        private GraphicsDeviceManager graphics;
        private Viewport fullScreenViewport;


        #endregion

        #region Properties 
        public Integer2 ScreenResolution
        {
            get
            {
                return new Integer2(this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            }
            set
            {
                graphics.PreferredBackBufferWidth = value.X;
                graphics.PreferredBackBufferHeight = value.Y;
                //if we forget to apply the changes then our resolution wont be set!
                graphics.ApplyChanges();
            }
        }
        public ScreenUtility.ScreenType ScreenType
        {
            get
            {
                return this.screenType;
            }
            set
            {
                this.screenType = value;
                this.bLayoutDirty = true;
            }
        }
        public float AspectRatio
        {
            get
            {
                return (float)graphics.PreferredBackBufferWidth / graphics.PreferredBackBufferHeight;
            }
        }
        public Viewport FullScreenViewport
        {
            get
            {
                return fullScreenViewport;
            }
        }
        #endregion

        //removes necessity to specify starting screen type layout (e.g. single, multi)
        public ScreenManager(Game game, GraphicsDeviceManager graphics, Integer2 screenResolution,
          ObjectManager objectManager, CameraManager cameraManager, KeyboardManager keyboardManager, 
          Keys pauseKey, EventDispatcher eventDispatcher, StatusType statusType)
          : this(game, graphics, screenResolution, ScreenUtility.ScreenType.SingleScreen, objectManager, cameraManager,
          keyboardManager, pauseKey, eventDispatcher, statusType)
        {
          
        }

        public ScreenManager(Game game, GraphicsDeviceManager graphics, Integer2 screenResolution, 
            ScreenUtility.ScreenType screenType, ObjectManager objectManager, CameraManager cameraManager, KeyboardManager keyboardManager, 
            Keys pauseKey, EventDispatcher eventDispatcher, StatusType statusType)
            : base(game, eventDispatcher, statusType)
        {
            this.ScreenType = screenType;
            this.objectManager = objectManager;
            this.cameraManager = cameraManager;

            //showing and hiding the menu - see ApplyUpdate()
            this.keyboardManager = keyboardManager;
            this.pauseKey = pauseKey;

            this.graphics = graphics;

            //set the resolution using the property
            this.ScreenResolution = screenResolution;
            this.fullScreenViewport = new Viewport(0, 0, screenResolution.X, screenResolution.Y);
        }

        #region Event Handling
        protected override void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.ScreenChanged += EventDispatcher_ScreenChanged;
            base.RegisterForEventHandling(eventDispatcher);
        }

        protected virtual void EventDispatcher_ScreenChanged(EventData eventData)
        {
            //has someone requested a change to the screen layout
            if (eventData.EventType == EventActionType.OnScreenLayoutChange)
            {
                //this.ScreenType = (ScreenUtilityScreenType)eventData.AdditionalEventParameters[0];

                this.ScreenType = ScreenUtility.ScreenType.SingleScreen;
            }
        }

        //See MenuManager::EventDispatcher_MenuChanged to see how it does the reverse i.e. they are mutually exclusive
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

        public bool ToggleFullScreen()
        {
            //flip the screen mode
            this.graphics.IsFullScreen = !this.graphics.IsFullScreen;
            this.graphics.ApplyChanges();

            //return new state
            return this.graphics.IsFullScreen;
        }

        protected override void ApplyUpdate(GameTime gameTime)
        {          
            #region Update Views
            //if one camera needs to be drawn on top of another then we need to do a depth sort each time we change the layout
            if (this.bLayoutDirty && this.screenType == ScreenUtility.ScreenType.MultiPictureInPicture)
            {
                //sort so that the top-most camera (i.e. closest draw depth to 0 will be the last camera drawn)
                this.cameraManager.SortByDepth(SortDirectionType.Ascending);
                this.bLayoutDirty = false;
            }

            //explicit call, as mentioned in Wiki - 2.4. Camera Viewports
            this.objectManager.Update(gameTime);
            #endregion
        }

        protected override void ApplyDraw(GameTime gameTime)
        {
            if (this.screenType == ScreenUtility.ScreenType.SingleScreen)
            {
                this.objectManager.Draw(gameTime, this.cameraManager.ActiveCamera);
            }
            else
            {
                //foreach is enabled by making CameraManager implement IEnumerator
                foreach (Camera3D camera in cameraManager)
                {
                    this.objectManager.Draw(gameTime, camera);
                }
            }

            //reset the viewport to fullscreen
            this.Game.GraphicsDevice.Viewport = this.fullScreenViewport;
        }

        protected override void HandleInput(GameTime gameTime)
        {
            #region Menu Handling
            //if user presses menu button then either show or hide the menu
            if (this.keyboardManager != null && this.keyboardManager.IsFirstKeyPress(this.pauseKey))
            {
                //if game is paused then publish a play event
                if (this.StatusType == StatusType.Off)
                {
                    //will be received by the menu manager and screen manager and set the menu to be shown and game to be paused
                    EventDispatcher.Publish(new EventData(EventActionType.OnStart, EventCategoryType.MainMenu));
                }
                //if game is playing then publish a pause event
                else if (this.StatusType != StatusType.Off)
                {
                    //will be received by the menu manager and screen manager and set the menu to be shown and game to be paused
                    EventDispatcher.Publish(new EventData(EventActionType.OnPause, EventCategoryType.MainMenu));
                }
            }
            #endregion
        }
    }
}
