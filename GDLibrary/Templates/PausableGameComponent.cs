/*
Function: 		Creates a class based on the GameComponent class that can be paused when the menu is shown.
Author: 		NMCG
Version:		1.0
Date Updated:	27/10/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class PausableGameComponent : GameComponent
    {
        #region Fields
        private StatusType statusType;
        private EventDispatcher eventDispatcher;
        #endregion

        #region Properties 
        private EventDispatcher EventDispatcher
        {
            get
            {
                return this.eventDispatcher;
            }
            set
            {
                this.eventDispatcher = value;
            }
        }
        public StatusType StatusType
        {
            get
            {
                return this.statusType;
            }
            set
            {
                this.statusType = value;
            }
        }
        #endregion

        public PausableGameComponent(Game game, EventDispatcher eventDispatcher, StatusType statusType)
            : base(game)
        {
            //store handle to event dispatcher for event registration and de-registration
            this.eventDispatcher = eventDispatcher;

            //allows us to start the game component with drawing and/or updating paused
            this.statusType = statusType;

            //register with the event dispatcher for the events of interest
            RegisterForEventHandling(eventDispatcher);
        }

        #region Event Handling
        protected virtual void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.MenuChanged += EventDispatcher_MenuChanged;
        }

        protected virtual void EventDispatcher_MenuChanged(EventData eventData)
        {

        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if ((this.statusType & StatusType.Update) != 0) //if update flag is set
            {
                ApplyUpdate(gameTime);
                base.Update(gameTime);
            }
        }


        protected virtual void ApplyUpdate(GameTime gameTime)
        {

        }

        protected virtual void HandleInput(GameTime gameTime)
        {

        }

        protected virtual void HandleMouse(GameTime gameTime)
        {

        }

        protected virtual void HandleKeyboard(GameTime gameTime)
        {

        }

        protected virtual void HandleGamePad(GameTime gameTime)
        {

        }

    }
}