using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public class MenuManager : PausableDrawableGameComponent
    {
        #region Fields
        //stores the actors shown for a particular menu scene (e.g. for the "main menu" scene we would have actors: startBtn, ExitBtn, AudioBtn)
        private Dictionary<string, List<UIObject>> menuDictionary;
        private List<UIObject> activeList = null;

        private SpriteBatch spriteBatch;
        private MouseManager mouseManager;
        private KeyboardManager keyboardManager;
        private CameraManager cameraManager;

        //tracks last object mouse-ed over by the cursor
        private UIObject oldUIObjectMouseOver;
        #endregion

        #region Properties
        protected UIObject OldUIObjectMouseOver
        {
            get
            {
                return this.oldUIObjectMouseOver;
            }
        }

        public List<UIObject> ActiveList
        {
            get
            {
                return this.activeList;
            }
        }
        #endregion

        public MenuManager(Game game, MouseManager mouseManager, KeyboardManager keyboardManager, 
            CameraManager cameraManager, SpriteBatch spriteBatch, EventDispatcher eventDispatcher, 
            StatusType statusType)
            : base(game, eventDispatcher, statusType)
        {
            this.menuDictionary = new Dictionary<string, List<UIObject>>();

            //used to listen for input
            this.mouseManager = mouseManager;
            this.keyboardManager = keyboardManager;
            this.cameraManager = cameraManager;

            //used to render menu and UI elements
            this.spriteBatch = spriteBatch;
        }

        #region Event Handling
        //See ScreenManager::EventDispatcher_MenuChanged to see how it does the reverse i.e. they are mutually exclusive
        protected override void EventDispatcher_MenuChanged(EventData eventData)
        {
            
            //did the event come from the main menu and is it a start game event
            if (eventData.EventType == EventActionType.OnStart)
            {
                //turn off update and draw i.e. hide the menu
                this.StatusType = StatusType.Off;
                //hide the mouse - comment out this line if you want to see the mouse cursor in-game
                this.Game.IsMouseVisible = false;
            }
            //did the event come from the main menu and is it a start game event
            else if (eventData.EventType == EventActionType.OnPause)
            {
                //turn on update and draw i.e. show the menu since the game is paused
                this.StatusType = StatusType.Update | StatusType.Drawn;
                //show the mouse
                this.Game.IsMouseVisible = true;
            }

            //set the mouse to look directly forward otherwise the camera would move forward based on some random mouse orientation
            this.mouseManager.SetPosition(this.cameraManager.ActiveCamera.ViewportCentre);
        }
        #endregion

        public void Add(string menuSceneID, UIObject actor)
        {
            if(this.menuDictionary.ContainsKey(menuSceneID))
            {
                this.menuDictionary[menuSceneID].Add(actor);
            }
            else
            {
                List<UIObject> newList = new List<UIObject>();
                newList.Add(actor);
                this.menuDictionary.Add(menuSceneID, newList);
            }

            //if the user forgets to set the active list then set to the sceneID of the last added item
            if(this.activeList == null)
            {
                SetActiveList(menuSceneID);
                   
            }
        }

        public UIObject Find(string menuSceneID, Predicate<UIObject> predicate)
        {
            if (this.menuDictionary.ContainsKey(menuSceneID))
            {
                return this.menuDictionary[menuSceneID].Find(predicate);
            }
            return null;
        }

        public bool Remove(string menuSceneID, Predicate<UIObject> predicate)
        {
            UIObject foundUIObject = Find(menuSceneID, predicate);

            if (foundUIObject != null)
                return this.menuDictionary[menuSceneID].Remove(foundUIObject);

            return false;
        }

        //e.g. return all the actor2D objects associated with the "main menu" or "audio menu"
        public List<UIObject> FindAllBySceneID(string menuSceneID)
        {
            if (this.menuDictionary.ContainsKey(menuSceneID))
            {
                return this.menuDictionary[menuSceneID];
            }
            return null;
        }

        public bool SetActiveList(string menuSceneID)
        {
            if (this.menuDictionary.ContainsKey(menuSceneID))
            {
                this.activeList = this.menuDictionary[menuSceneID];
                return true;
            }

            return false;
        }

        protected override void ApplyUpdate(GameTime gameTime)
        {
            if (this.activeList != null)
            {
                //update all the updateable menu items (e.g. make buttons pulse etc)
                foreach (UIObject currentUIObject in this.activeList)
                {
                    if ((currentUIObject.GetStatusType() & StatusType.Update) != 0) //if update flag is set
                        currentUIObject.Update(gameTime);
                }
                //check for mouse over and mouse click on a menu item
                CheckMouseOverAndClick(gameTime);
            }

        }

        private void CheckMouseOverAndClick(GameTime gameTime)
        {
            foreach (UIObject currentUIObject in this.activeList)
            {
                //only handle mouseover and mouse click for buttons
                if (currentUIObject.ActorType == ActorType.UIButton)
                {
                    //add an if to check that this is a interactive UIButton object
                    if (currentUIObject.Transform.Bounds.Intersects(this.mouseManager.Bounds))
                    {
                        //if mouse is over a new ui object then set old to "IsMouseOver=false"
                        if (this.oldUIObjectMouseOver != null && this.oldUIObjectMouseOver != currentUIObject)
                        {
                            oldUIObjectMouseOver.MouseOverState.Update(false);
                        }

                        //update the current state of the currently mouse-over'ed ui object
                        currentUIObject.MouseOverState.Update(true);

                        //apply any mouse over or mouse click actions
                        HandleMouseOver(currentUIObject, gameTime);
                        if (this.mouseManager.IsLeftButtonClickedOnce())
                            HandleMouseClick(currentUIObject, gameTime);

                        //store the current as old for the next update
                        this.oldUIObjectMouseOver = currentUIObject;
                    }
                    else
                    {
                        //set the mouse as not being over the current ui object
                        currentUIObject.MouseOverState.Update(false);
                    }
                }
            }
        }

        protected override void ApplyDraw(GameTime gameTime)
        {
            if (this.activeList != null)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                foreach (UIObject currentUIObject in this.activeList)
                {
                    if ((currentUIObject.GetStatusType() & StatusType.Drawn) != 0) //if drawn flag is set
                        currentUIObject.Draw(gameTime, spriteBatch);
                }
                spriteBatch.End();
            }
        }

        protected virtual void HandleMouseOver(UIObject currentUIObject, GameTime gameTime)
        {
            //developer implements in subclass of MenuManager - see MyMenuManager.cs
        }

        protected virtual void HandleMouseClick(UIObject clickedUIObject, GameTime gameTime)
        {
            //developer implements in subclass of MenuManager - see MyMenuManager.cs
        }

    }
}
