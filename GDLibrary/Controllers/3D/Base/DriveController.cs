/*
Function: 		Creates a simple controller for drivable objects taking input from the keyboard
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
    public class DriveController : UserInputController
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        public DriveController(string id, ControllerType controllerType, Keys[] moveKeys, float moveSpeed, float strafeSpeed, float rotationSpeed,
            ManagerParameters managerParameters)
            : base(id, controllerType, moveKeys, moveSpeed, strafeSpeed, rotationSpeed, managerParameters)
        {

        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            base.Update(gameTime, actor);
        }

        public override void HandleKeyboardInput(GameTime gameTime, Actor3D parentActor)
        {
            Vector3 translation = Vector3.Zero;

            //move forward/backward
            if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[0]))
            {
                translation = gameTime.ElapsedGameTime.Milliseconds
                             * this.MoveSpeed * parentActor.Transform.Look;
            }
            else if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[1]))
            {
                translation = -gameTime.ElapsedGameTime.Milliseconds
                            * this.MoveSpeed * parentActor.Transform.Look;
            }

            //strafe
            if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[4]))
            {
                //What's the significance of the +=? Remove it and see if we can move forward/backward AND strafe.
                translation += -gameTime.ElapsedGameTime.Milliseconds
                             * this.StrafeSpeed * parentActor.Transform.Right;
            }
            else if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[5]))
            {
                //What's the significance of the +=? Remove it and see if we can move forward/backward AND strafe.
                translation += gameTime.ElapsedGameTime.Milliseconds
                            * this.StrafeSpeed * parentActor.Transform.Right;
            }

            //rotate
            if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[2]))
            {
                parentActor.Transform.RotateAroundYBy(gameTime.ElapsedGameTime.Milliseconds * this.RotationSpeed);
            }
            else if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[3]))
            {
                parentActor.Transform.RotateAroundYBy(-gameTime.ElapsedGameTime.Milliseconds * this.RotationSpeed);
            }

            //Was a move button(s) pressed?
            if (translation != Vector3.Zero)
            {
                //remove y-axis component of the translation
                translation.Y = 0;
                //apply
                parentActor.Transform.TranslateBy(translation);
            }
        }

        //Add Equals, Clone, ToString, GetHashCode...
    }
}