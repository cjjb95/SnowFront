/*
Function: 		Used by Actor to help us distunguish one type of actor from another when we perform CD/CR or when we want to enable/disable certain game entities
                e.g. hide all the pickups.
Author: 		NMCG
Version:		1.0
Last Updated:
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum ActorType : sbyte
    {
        Player,
        Decorator, 
        Prop,
        Pickup,
        Camera,
        Zone,
        Helper,
        UIButton
    }
}














