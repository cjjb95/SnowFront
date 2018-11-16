/*
Function: 		Flight controllers allows movement in any XYZ direction 
Author: 		NMCG
Version:		1.0
Date Updated:	30/8/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    
    public class FlightCameraController : UserInputController
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        public FlightCameraController(string id, ControllerType controllerType, Keys[] moveKeys, float moveSpeed, float strafeSpeed, float rotationSpeed, 
            ManagerParameters managerParameters)
            : base(id, controllerType, moveKeys, moveSpeed, strafeSpeed, rotationSpeed, managerParameters)
        {

        }

        public override void HandleMouseInput(GameTime gameTime, Actor3D parentActor)
        {
            Vector2 mouseDelta = Vector2.Zero;
            mouseDelta = -this.ManagerParameters.MouseManager.GetDeltaFromCentre(this.ManagerParameters.CameraManager.ActiveCamera.ViewportCentre);
            mouseDelta *= gameTime.ElapsedGameTime.Milliseconds;
            mouseDelta *= this.RotationSpeed;

            //only rotate if something has changed with the mouse
            if(mouseDelta.Length() != 0) 
                parentActor.Transform.RotateBy(new Vector3(mouseDelta.X, mouseDelta.Y, 0));
        }

        public override void HandleKeyboardInput(GameTime gameTime, Actor3D parentActor)
        {
            if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[0]))
            {
                parentActor.Transform.TranslateBy(gameTime.ElapsedGameTime.Milliseconds
                             * this.MoveSpeed * parentActor.Transform.Look);
            }
            else if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[1]))
            {
                parentActor.Transform.TranslateBy(-gameTime.ElapsedGameTime.Milliseconds
                             * this.MoveSpeed * parentActor.Transform.Look);
            }

            if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[2]))
            {
                parentActor.Transform.TranslateBy(-gameTime.ElapsedGameTime.Milliseconds
                             * this.StrafeSpeed * parentActor.Transform.Right);
            }
            else if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[3]))
            {
                parentActor.Transform.TranslateBy(gameTime.ElapsedGameTime.Milliseconds
                    * this.StrafeSpeed * parentActor.Transform.Right);
            }
        }

        //Add Equals, Clone, ToString, GetHashCode...
    }
}