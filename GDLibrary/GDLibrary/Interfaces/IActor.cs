/*
Function: 		Represents the parent interface for all game objects. This interface should really only have an Update() method since a Camera doesn't need a Draw() - this is just a concession to simplify the hierarchy.
Author: 		NMCG
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/


//base class from which all drawn, collidable, non-collidable, trigger volumes, and camera inherit
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public interface IActor
    {
        string GetID();
        ActorType GetActorType();
        StatusType GetStatusType();
        void Update(GameTime gameTime);

        //controller specific methods
        void AttachController(IController controller);
        bool DetachController(IController controller);
        int DetachControllers(Predicate<IController> predicate);

        //find all based on user-defined properties (e.g. ID, ControllerType etc)
        List<IController> FindControllers(Predicate<IController> predicate);
        //allows us to set the PlayStatus for all controllers simultaneously (e.g. play all, reset all, stop all)
        int SetAllControllersPlayStatus(PlayStatusType playStatusType);
        //allows us to set the PlayStatus for all controllers matching the predicate (e.g. "play" all controllers with a group ID of 1)
        int SetControllerPlayStatus(PlayStatusType playStatusType, Predicate<IController> predicate);
    }
}