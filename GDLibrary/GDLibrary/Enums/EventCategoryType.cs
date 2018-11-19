/*
Function: 		Enum to define the category type of the event e.g. MainMenu (category type) is sending an OnRestart (event type) message
Author: 		NMCG
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum EventCategoryType : sbyte
    {
        //one category for each group of events in EventType
        Camera,
        Player,
        NonPlayer,
        Pickup,
        SystemAdd,
        SystemRemove,
        Menu,
        Ice,
        Coat,
    }
}
