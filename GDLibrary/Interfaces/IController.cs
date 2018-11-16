/*
Function: 		Represents the parent interface for all object controllers. A controller can be attached to an actor and modify its state e.g.
                a FirstPersonCameraController attached to a Camera3D actor will handle keyboard and mouse input and translate into changes in the actors Transform3D properties.
Author: 		NMCG
Version:		1.1
Date Updated:	6/10/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public interface IController : ICloneable
    {
        void Update(GameTime gameTime, IActor actor); //update the actor controller by this controller
        void SetActor(IActor actor); //reset the actor controller by this controller
        string GetID(); //used when we want to interrogate a controller and see if it is "the one" that we want to enable/disable, based on ID.

        //allows us to play, pause, reset, stop a controller
        bool SetControllerPlayStatus(PlayStatusType playStatusType);

    }
}
