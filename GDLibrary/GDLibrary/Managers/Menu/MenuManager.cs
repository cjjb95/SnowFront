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
        private Dictionary<string, List<DrawnActor2D>> menuDictionary;
        private List<DrawnActor2D> activeList = null;

        private SpriteBatch spriteBatch;
        private InputManagerParameters inputManagerParameters;
        private CameraManager cameraManager;
        #endregion

        #region Properties
        public List<DrawnActor2D> ActiveList
        {
            get
            {
                return this.activeList;
            }
        }
        #endregion

        public MenuManager(Game game, InputManagerParameters inputManagerParameters,
            CameraManager cameraManager, SpriteBatch spriteBatch, EventDispatcher eventDispatcher, 
            StatusType statusType)
            : base(game, eventDispatcher, statusType)
        {
            this.menuDictionary = new Dictionary<string, List<DrawnActor2D>>();

            //used to listen for input
            this.inputManagerParameters = inputManagerParameters;

            this.cameraManager = cameraManager;

            //used to render menu and UI elements
            this.spriteBatch = spriteBatch;
        }

        #region Event Handling
        protected override void EventDispatcher_MenuChanged(EventData eventData)
        {
            if(eventData.EventType == EventActionType.OnNewMenu)
            {
                this.SetActiveMenu(eventData.ID);
            }

            base.EventDispatcher_MenuChanged(eventData);
        }
        #endregion

        public void Add(string menuSceneID, DrawnActor2D actor)
        {
            if(this.menuDictionary.ContainsKey(menuSceneID))
            {
                this.menuDictionary[menuSceneID].Add(actor);
            }
            else
            {
                List<DrawnActor2D> newList = new List<DrawnActor2D>();
                newList.Add(actor);
                this.menuDictionary.Add(menuSceneID, newList);
            }

            //if the user forgets to set the active list then set to the sceneID of the last added item
            if(this.activeList == null)
            {
                SetActiveMenu(menuSceneID);
                   
            }
        }

        public DrawnActor2D Find(string menuSceneID, Predicate<DrawnActor2D> predicate)
        {
            if (this.menuDictionary.ContainsKey(menuSceneID))
            {
                return this.menuDictionary[menuSceneID].Find(predicate);
            }
            return null;
        }

        public bool Remove(string menuSceneID, Predicate<DrawnActor2D> predicate)
        {
            DrawnActor2D foundUIObject = Find(menuSceneID, predicate);

            if (foundUIObject != null)
                return this.menuDictionary[menuSceneID].Remove(foundUIObject);

            return false;
        }

        //e.g. return all the actor2D objects associated with the "main menu" or "audio menu"
        public List<DrawnActor2D> FindAllBySceneID(string menuSceneID)
        {
            if (this.menuDictionary.ContainsKey(menuSceneID))
            {
                return this.menuDictionary[menuSceneID];
            }
            return null;
        }

        public bool SetActiveMenu(string menuSceneID)
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
                foreach (DrawnActor2D currentUIObject in this.activeList)
                {
                    if ((currentUIObject.GetStatusType() & StatusType.Update) != 0) //if update flag is set
                    {
                        currentUIObject.Update(gameTime);
                        //check if mouse position
                    }
                }
            }

        }

        protected override void ApplyDraw(GameTime gameTime)
        {
            if (this.activeList != null)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                foreach (DrawnActor2D currentUIObject in this.activeList)
                {
                    if ((currentUIObject.GetStatusType() & StatusType.Drawn) != 0) //if drawn flag is set
                        currentUIObject.Draw(gameTime, spriteBatch);
                }
                spriteBatch.End();
            }
        }

        protected virtual void HandleMouseOver(DrawnActor2D uiObject, GameTime gameTime)
        {
            //developer implements in subclass of MenuManager - see MyMenuManager.cs
            HandleMouseClick(uiObject, gameTime);
        }

        protected virtual void HandleMouseClick(DrawnActor2D uiObject, GameTime gameTime)
        {
            //developer implements in subclass of MenuManager - see MyMenuManager.cs
        }

    }
}
