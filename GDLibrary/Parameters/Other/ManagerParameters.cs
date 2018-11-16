/*
Function: 		Encapsulates manager parameters for those classes (e.g. UIMouseObject) that need access to a large number of managers.
                Used by UIMouseObject.
Author: 		NMCG
Version:		1.0
Date Updated:	25/11/17
Bugs:			None
Fixes:			None
*/
namespace GDLibrary
{
    public class ManagerParameters
    {
        #region Fields
        private ObjectManager objectManager;
        private CameraManager cameraManager;
        private MouseManager mouseManager;
        private KeyboardManager keyboardManager;
        private GamePadManager gamePadManager;
        private ScreenManager screenManager;
        private SoundManager soundManager;
        #endregion

        #region Properties
        //only getters since we would rarely want to re-define a manager during gameplay
        public ObjectManager ObjectManager
        {
            get
            {
                return this.objectManager;
            }
        }
        public CameraManager CameraManager
        {
            get
            {
                return this.cameraManager;
            }
        }
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
        public GamePadManager GamePadManager
        {
            get
            {
                return this.gamePadManager;
            }
        }
        public ScreenManager ScreenManager
        {
            get
            {
                return this.screenManager;
            }
        }
        public SoundManager SoundManager
        {
            get
            {
                return this.soundManager;
            }
        }
        #endregion


        //useful for objects that need access to ALL managers
        public ManagerParameters(ObjectManager objectManager, CameraManager cameraManager,
            MouseManager mouseManager, KeyboardManager keyboardManager, GamePadManager gamePadManager, 
            ScreenManager screenManager, SoundManager soundManager)
        {
            this.objectManager = objectManager;
            this.cameraManager = cameraManager;
            this.mouseManager = mouseManager;
            this.keyboardManager = keyboardManager;
            this.gamePadManager = gamePadManager;
            this.screenManager = screenManager;
            this.soundManager = soundManager;
        }


    }
}
