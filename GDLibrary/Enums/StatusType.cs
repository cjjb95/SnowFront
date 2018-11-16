/*
Function: 		Used by Actor to define the state of an actor e.g. is it drawn and updated currently? Perhaps its only drawn but not updated e.g. when the menu is onscreen
                but semi-transparent and we still want to see the game world, but in a paused state. 
Author: 		NMCG
Version:		1.0
Date Updated:	17/8/17
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum StatusType
    {
        //used for enabling objects for updating and drawing e.g. a model or a camera, or a controller
        Drawn = 1,
        Update = 2,
        Off = 4, //neither drawn, nor updated e.g. the objectmanager when the menu is shown at startup - see Main::InitializeManagers()
        
        /*
        * Q. Why do we use powers of 2? will it allow us to do anything different?
        * A. StatusType.Updated | StatusType.Drawn - See ObjectManager::Update() or Draw()
        */

    }
}
