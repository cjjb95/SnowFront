/*
Function: 		Enum to define the category type of the event e.g. MainMenu (category type) is sending an OnRestart (event type) message
Author: 		NMCG
Version:		1.0
Date Updated:	11/10/17
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum EventCategoryType : sbyte
    {
        //one category for each group of events in EventType
        MainMenu,
        UIMenu,
        Camera,
        Player,
        NonPlayer,
        Pickup,
        Door,
        Screen,
        Opacity,
        SystemAdd, //used to send add related events e.g. remove objects from ojectmanager, camera manager, ui manager
        SystemRemove, //used to send remove related events e.g. remove objects from ojectmanager, camera manager, ui manager
        Debug,   //used to send debug related events e.g. show/hide debug info
        Sound3D,
        Sound2D,
        ObjectPicking,   //used to notify listening objects that we have picked something with the mouse
        Mouse,              //used to send mouse related events e.g. set mouse position
        Video,
        GlobalSound
        //all other categories of sender...
    }
}
