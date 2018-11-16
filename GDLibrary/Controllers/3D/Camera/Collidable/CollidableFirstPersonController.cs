/*
Function: 		First person COLLIDABLE camera controller.
Author: 		NMCG
Version:		1.0
Date Updated:	6/10/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    /// <summary>
    /// A collidable camera has a body and collision skin from a player object but it has no modeldata or texture
    /// </summary>
    public class CollidableFirstPersonCameraController : FirstPersonCameraController
    {
        #region Fields
        private PlayerObject playerObject;
        private float radius, height;
        private float accelerationRate, decelerationRate, mass, jumpHeight;
        private Vector3 translationOffset;
        #endregion

        #region Properties
        public float Radius
        {
            get
            {
                return this.radius;
            }
            set
            {
                this.radius = (value > 0) ? value : 1;
            }
        }
        public float Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = (value > 0) ? value : 1;
            }
        }
        public float AccelerationRate
        {
            get
            {
                return this.accelerationRate;
            }
            set
            {
                this.accelerationRate = (value != 0) ? value : 1;
            }
        }
        public float DecelerationRate
        {
            get
            {
                return this.decelerationRate;
            }
            set
            {
                this.decelerationRate = (value != 0) ? value : 1;
            }
        }
        public float Mass
        {
            get
            {
                return this.mass;
            }
            set
            {
                this.mass = (value > 0) ? value : 1;
            }
        }
        public float JumpHeight
        {
            get
            {
                return this.jumpHeight;
            }
            set
            {
                this.jumpHeight = (value > 0) ? value : 1;
            }
        }
        #endregion

        //uses the default PlayerObject as the collidable object for the camera
        public CollidableFirstPersonCameraController(string id, ControllerType controllerType, Keys[] moveKeys, float moveSpeed, float strafeSpeed, float rotationSpeed,
           ManagerParameters managerParameters,
           IActor parentActor, float radius, float height, float accelerationRate, float decelerationRate,
           float mass, float jumpHeight, Vector3 translationOffset)
           : this(id, controllerType,moveKeys, moveSpeed, strafeSpeed, rotationSpeed,
            managerParameters,
            parentActor, radius, height, accelerationRate, decelerationRate,
            mass, jumpHeight, translationOffset, null)
        {
        }

        //allows developer to specify the type of collidable object to be used as basis for the camera
        public CollidableFirstPersonCameraController(string id, ControllerType controllerType, Keys[] moveKeys, float moveSpeed, float strafeSpeed, float rotationSpeed,
            ManagerParameters managerParameters,
            IActor parentActor, float radius, float height, float accelerationRate, float decelerationRate,
            float mass, float jumpHeight, Vector3 translationOffset, PlayerObject collidableObject)
            : base(id, controllerType, moveKeys, moveSpeed, strafeSpeed, rotationSpeed, managerParameters)
        {
            this.Radius = radius;
            this.height = height;
            this.AccelerationRate = accelerationRate;
            this.DecelerationRate = decelerationRate;
            this.Mass = mass;
            this.JumpHeight = jumpHeight;

            //allows us to tweak the camera position within the player object 
            this.translationOffset = translationOffset;

            /* Create the collidable player object which comes with a collision skin and position the parentActor (i.e. the camera) inside the player object.
             * notice that we don't pass any effect, model or texture information, since in 1st person perspective we dont want to look from inside a model.
             * Therefore, we wont actually render any drawn object - the effect, texture, model (and also Color) information are unused.
             * 
             * This code allows the user to pass in their own PlayerObject (e.g. HeroPlayerObject) to be used for the collidable object basis for the camera.
             */
            if (collidableObject != null)
            {
                this.playerObject = collidableObject;
            }
            else
            {
                this.playerObject = new PlayerObject(this.ID + " - player object", ActorType.CollidableCamera, (parentActor as Actor3D).Transform,
                 null, null, this.MoveKeys, radius, height, accelerationRate, decelerationRate, jumpHeight,
                 translationOffset, this.ManagerParameters.KeyboardManager);
            }

            playerObject.Enable(false, mass);

        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            //updates the sound manager to tell it where the 1st person camera is right now for any 3D sounds
            //did NOT use an event here as the frequency of calls to this event would FLOOD the system
            this.ManagerParameters.SoundManager.UpdateListenerPosition((actor as Actor3D).Transform.Translation);

            base.Update(gameTime, actor);
        }


        public override void HandleKeyboardInput(GameTime gameTime, Actor3D parentActor)
        {
            /* Notice in the code below that we are NO LONGER simply changing the camera translation value. 
             * Since this is now a collidable camera we, instead, modify the camera position by calling the PlayerObject move methods.
             * 
             * Q. Why do we still use the rotation methods of Transform3D? 
             * A. Rotating the camera doesnt affect CD/CR since the camera is modelled by a player object which has a capsule shape.
             *    A capsule's collision response won't alter as a result of any rotation since its cross-section is spherical across the XZ-plane.
             */

            if ((parentActor != null) && (parentActor != null))
            {
                //jump
                if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[4])) //check AppData.CameraMoveKeys for correct index of each move key
                {
                    this.playerObject.CharacterBody.DoJump(this.jumpHeight);
                }
                //crouch
                else if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[5]))
                {
                    this.playerObject.CharacterBody.IsCrouching = !this.playerObject.CharacterBody.IsCrouching;
                }

                //forward/backward
                if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[0]))
                {
                    Vector3 restrictedLook = parentActor.Transform.Look;
                    restrictedLook.Y = 0;
                    this.playerObject.CharacterBody.Velocity += restrictedLook * this.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
                }
                else if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[1]))
                {
                    Vector3 restrictedLook = parentActor.Transform.Look;
                    restrictedLook.Y = 0;
                    this.playerObject.CharacterBody.Velocity -= restrictedLook * this.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
                }
                else //decelerate to zero when not pressed
                {
                    this.playerObject.CharacterBody.DesiredVelocity = Vector3.Zero;
                }

                //strafe left/right
                if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[2]))
                {
                    Vector3 restrictedRight = parentActor.Transform.Right;
                    restrictedRight.Y = 0;
                    this.playerObject.CharacterBody.Velocity -= restrictedRight * this.StrafeSpeed * gameTime.ElapsedGameTime.Milliseconds;
                }
                else if (this.ManagerParameters.KeyboardManager.IsKeyDown(this.MoveKeys[3]))
                {
                    Vector3 restrictedRight = parentActor.Transform.Right;
                    restrictedRight.Y = 0;
                    this.playerObject.CharacterBody.Velocity += restrictedRight * this.StrafeSpeed * gameTime.ElapsedGameTime.Milliseconds;
                }
                else //decelerate to zero when not pressed
                {
                    this.playerObject.CharacterBody.DesiredVelocity = Vector3.Zero;
                }

                //update the camera position to reflect the collision skin position
                parentActor.Transform.Translation = this.playerObject.CharacterBody.Position;
            }

        }


        //to do - clone, dispose

    }
}
