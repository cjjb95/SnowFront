/*
Function: 		Security camera controller with cyclical panning movement where panning speed is defined by the developer. 
Author: 		NMCG
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    //by default when VS creates a class it doesnt add the "public" access modifier - ensure you manually add to enable visibility of class from outside
    public class SecurityCameraController : Controller
    {
        #region Fields
        private float rotationAmplitude;
        private float rotationSpeedMultiplier;
        private Vector3 rotationVector;
        #endregion

        #region Properties
        public float RotationAmplitude
        {
            get
            {
                return this.rotationAmplitude;
            }
            set
            {
                //clamp to prevent <= 0 amplitude values
                this.rotationAmplitude = (value > 0) ? value : 1;
            }
        }

        public float RotationSpeedMultiplier
        {
            get
            {
                return this.rotationSpeedMultiplier;
            }
            set
            {
                //clamp to prevent <= 0 amplitude values
                this.rotationSpeedMultiplier = (value > 0) ? value : 1;
            }
        }

        public Vector3 RotationVector
        {
            get
            {
                return this.rotationVector;
            }
            set
            {
                //clamp to prevent vector length == 0
                this.rotationVector = (value != Vector3.Zero) ? value : Vector3.UnitX;

                //we always want this vector be normalised i.e. to indicate direction only and not direction AND magnitude != 1
                this.rotationVector.Normalize();
            }
        }
        #endregion

        public SecurityCameraController(string id, ControllerType controllerType, float rotationAmplitude, float rotationSpeedMultiplier, 
            Vector3 rotationVector) : base(id, controllerType)
        {
            this.RotationAmplitude = rotationAmplitude;
            this.RotationSpeedMultiplier = rotationSpeedMultiplier;
            this.RotationVector = rotationVector;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            //limit angle to 360 using a modulus
            float time = (float)gameTime.TotalGameTime.TotalSeconds%360;

            //bounded angle amount by which to yaw (i.e. rotate around Up vector) the camera
            float boundedRotationAngle = this.rotationAmplitude * (float)Math.Sin(this.rotationSpeedMultiplier * time);

            //cast to access the Transform3D
            Actor3D actor3D = actor as Actor3D;

            if (actor3D != null)
            {
                //Apply the rotation to get the camera to yaw (i.e. rotate around the Y axis)
                //This is a little counter-intuitive (since we set a x-ordinate value on the Vector3 below)
                //You need to look at the RotateBy() method to understand what is happening
                actor3D.Transform.RotateBy(this.rotationVector * boundedRotationAngle);
            }
            base.Update(gameTime, actor);
        }

        //Add Equals, Clone, ToString, GetHashCode...
    }
}
