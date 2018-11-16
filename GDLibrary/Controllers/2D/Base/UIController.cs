/*
Function: 		Base class for all controllers that modify menu or UI actors. Checks mouseover state to enable mouse effects.
Author: 		NMCG
Version:		1.0
Date Updated:	29/9/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class UIController : Controller
    {
        #region Fields
        private bool bEnabled;
        private int totalElapsedTime;
        #endregion

        #region Properties
        #endregion

        public UIController(string id, ControllerType controllerType) : base(id, controllerType)
        {

        }

        //marking a method or class as "sealed" prevent the class from being inherited from, or the method from being overridden
        //we do this here because I don't want to allow the developer to change the Update() behaviour.
        public sealed override void Update(GameTime gameTime, IActor actor)
        {
            //cast to access transform, color etc.
            UIObject uiObject = actor as UIObject;
            if (uiObject.MouseOverState.IsActivating())
            {
                this.totalElapsedTime = 0;
                this.bEnabled = true;
            }
            else if (uiObject.MouseOverState.IsActive())
            {
                this.totalElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                this.bEnabled = true;
            }
            else
            {
                this.bEnabled = false;
                this.SetActor(uiObject);
            }

            //if mouse over then apply the controller's behaviour
            if (this.bEnabled)
            {
                this.ApplyController(gameTime, uiObject, this.totalElapsedTime);
            }
        }

        //apply whatever change the controller is designed for e.g. if the mouse is over the UI object
        protected virtual void ApplyController(GameTime gameTime, UIObject uiObject, float totalElapsedTime)
        {

        }
    }
}
