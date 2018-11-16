/*
Function: 		Encapsulates manager parameters for those classes (e.g. UIMouseObject) that need access to a large number of managers.
                Used by UIMouseObject.
Author: 		NMCG
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/
namespace GDLibrary
{
    public class InputManagerParameters
    {
        #region Fields
        private MouseManager mouseManager;
        private KeyboardManager keyboardManager;
        #endregion

        #region Properties
        //only getters since we would rarely want to re-define a manager during gameplay
        public MouseManager MouseManager
        {
            get
            {
                return this.mouseManager;
            }
        }
        public KeyboardManager KeyboardManager
        {
            get
            {
                return this.keyboardManager;
            }
        }
        #endregion


        //useful for objects that need access to ALL managers
        public InputManagerParameters(MouseManager mouseManager, KeyboardManager keyboardManager)
        {
            this.mouseManager = mouseManager;
            this.keyboardManager = keyboardManager;
        }


    }
}
