/*
Function: 		Represents the parent interface for all game objects. This interface should really only have an Update() method since a Camera doesn't need a Draw() - this is just a concession to simplify the hierarchy.
Author: 		NMCG
Version:		1.0
Date Updated:	17/8/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

//base class from which all drawn, collidable, 
//non-collidable, trigger volumes, and camera inherit
namespace GDLibrary
{
    public interface IActor
    {

        string GetID();
        float GetAlpha();
        ActorType GetActorType();
        StatusType GetStatusType();

        bool Remove();
        void Update(GameTime gameTime);

        //controller specific methods
        void AttachController(IController controller);
        bool DetachController(IController controller);
        int DetachControllers(Predicate<IController> predicate);

        //find all based on user-defined properties (e.g. ID, ControllerType etc)
        List<IController> FindControllers(Predicate<IController> predicate);
        //allows us to set the PlayStatus for all controllers simultaneously (e.g. play all, reset all, stop all)
        void SetAllControllers(PlayStatusType playStatusType);
        //allows us to set the PlayStatus for all controllers with the same GROUP parameters simultaneously (e.g. "play" all controllers with a group ID of 1)
        void SetAllControllers(PlayStatusType playStatusType, Predicate<IController> predicate);



    }
}
