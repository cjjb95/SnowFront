/*
Function: 		Used by Actor to help us distunguish one type of actor from another when we perform CD/CR or when we want to enable/disable certain game entities
                e.g. hide all the pickups.
Author: 		NMCG
Version:		1.0
Date Updated:	17/8/17
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum ActorType : sbyte
    {
        Player,
        Decorator, //i.e.  architecture
        Billboard, //i.e. an imposter for a 3D object e.g. distant tree or facade of a building

        Camera,
        Zone, //i.e. invisible and triggers events e.g. walk through a bounding volume and trigger game end or camera change
        Helper, //i.e.. a wireframe visualisation for an entitiy e.g. camera, camera path, bounding box of a pickip

        UIStaticTexture,  //i.e. a static illustrative texture e.g. a background menu image
        UIStaticText,     //i.e. menu text shown beside the value of game state e.g. Health: XXX
        UIButton,          //i.e. menu text representing a menu choice e.g. "Pause"
        UITexture, //i.e. a HUD texture that changes over time e.g. a mouse reticule that changes based on mouse-over object
        UIDynamicText,    //i.e. a HUD text that changes over time e.g. time remaining - 84, 83, 83,...

        //new with JigLibX
        CollidableGround,
        CollidableDecorator,    //e.g. a table, lampshade
        CollidableProp,         //i.e. an interactable prop related to game narrative or game state e.g. door
        CollidableCamera,       //used by first person collidable camera controller
        CollidablePickup,        //i.e. something we can add to inventory e.g. ammo
        CollidableArchitecture,
        CollidableRecording,      //audio recordings from players that can be picked up and played
        CollidableProjectile,
        CollidableDoor,
        CollidableAmmo,
        Primitive
    }
}
