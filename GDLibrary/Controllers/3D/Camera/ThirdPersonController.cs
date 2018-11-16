/*
Function: 		A third person controller to a camera (i.e. the parent actor) which causes the camera to track a target actor in 3rd person mode.
Author: 		NMCG
Version:		1.0
Date Updated:	24/10/17
Bugs:			None
Fixes:			None
*/
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class ThirdPersonController : TargetController
    {
        #region Fields
        private float elevationAngle, distance, scrollSpeedDistanceMultiplier, scrollSpeedElevationMultiplier;

        //used to dampen camera movement
        private Vector3 oldTranslation;
        private Vector3 oldCameraToTarget;
        private float translationLerpSpeed, lookLerpSpeed;
        private MouseManager mouseManager;
        #endregion

        #region Properties
        public float TranslationLerpSpeed
        {
            get
            {
                return translationLerpSpeed;
            }
            set
            {

                //lerp speed should be in the range >0 and <=1
                translationLerpSpeed = (value > 0) && (value <= 1) ? value : 0.1f;
            }
        }
        public float LookLerpSpeed
        {
            get
            {
                return lookLerpSpeed;
            }
            set
            {

                //lerp speed should be in the range >0 and <=1
                lookLerpSpeed = (value > 0) && (value <= 1) ? value : 0.1f;
            }
        }
        public float ScrollSpeedDistanceMultiplier
        {
            get
            {
                return scrollSpeedDistanceMultiplier;
            }
            set
            {
                //distanceScrollMultiplier should not be lower than 0
                scrollSpeedDistanceMultiplier = (value > 0) ? value : 1;
            }
        }

        public float ScrollSpeedElevationMultiplier
        {
            get
            {
                return scrollSpeedElevationMultiplier;
            }
            set
            {
                //scrollSpeedElevationMulitplier should not be lower than 0
                scrollSpeedElevationMultiplier = (value > 0) ? value : 1;
            }
        }

        public float Distance
        {
            get
            {
                return distance;
            }
            set
            {
                //distance should not be lower than 0
                distance = (value > 0) ? value : 1;
            }
        }
        public float ElevationAngle
        {
            get
            {
                return elevationAngle;
            }
            set
            {
                elevationAngle = value % 360;
                elevationAngle = MathHelper.ToRadians(elevationAngle);
            }
        }
        #endregion

        public ThirdPersonController(string id, ControllerType controllerType, Actor targetActor,
            float distance, float scrollSpeedDistanceMultiplier, float elevationAngle, 
                float scrollSpeedElevationMultiplier, float translationLerpSpeed, float lookLerpSpeed, MouseManager mouseManager)
            : base(id, controllerType, targetActor)
        {
            //call properties to set validation on distance and radian conversion
            this.Distance = distance;

            //allows us to control distance and elevation from the mouse scroll wheel
            this.ScrollSpeedDistanceMultiplier = scrollSpeedDistanceMultiplier;
            this.ScrollSpeedElevationMultiplier = scrollSpeedElevationMultiplier;
            //notice that we pass the incoming angle through the property to convert it to radians
            this.ElevationAngle = elevationAngle;

            //dampen camera translation reaction
            this.translationLerpSpeed = translationLerpSpeed;
            //dampen camera rotation reaction
            this.lookLerpSpeed = lookLerpSpeed;

            //used to change elevation angle or distance from target - See UpdateFromScrollWheel()
            this.mouseManager = mouseManager;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            UpdateFromScrollWheel(gameTime);
            UpdateParent(gameTime, actor as Actor3D, this.TargetActor as Actor3D);
        }

        private void UpdateParent(GameTime gameTime, Actor3D parentActor, Actor3D targetActor)
        {
            if (targetActor != null)
            {
                //rotate the target look around the target right to get a vector pointing away from the target at a specified elevation
                Vector3 cameraToTarget = Vector3.Transform(targetActor.Transform.Look,
                    Matrix.CreateFromAxisAngle(targetActor.Transform.Right, this.elevationAngle));

                //normalize to give unit length, otherwise distance from camera to target will vary over time
                cameraToTarget.Normalize();

                //set the position of the camera to be a set distance from target and at certain elevation angle
                parentActor.Transform.Translation = Vector3.Lerp(this.oldTranslation, cameraToTarget * this.distance + targetActor.Transform.Translation, this.translationLerpSpeed);

                //set the camera to look at the target object
                parentActor.Transform.Look = Vector3.Lerp(this.oldCameraToTarget, cameraToTarget, this.lookLerpSpeed);

                //store old values for lerp
                this.oldTranslation = parentActor.Transform.Translation;
                this.oldCameraToTarget = -cameraToTarget;
            }
        }

        private void UpdateFromScrollWheel(GameTime gameTime)
        {
            //get magnitude and direction of scroll change
            float scrollWheelDelta = -this.mouseManager.GetDeltaFromScrollWheel() * gameTime.ElapsedGameTime.Milliseconds;

            //if something changed then update
            if (scrollWheelDelta != 0)
            {
                //move camera closer to, or further, from the target
                this.Distance += this.scrollSpeedDistanceMultiplier * scrollWheelDelta;

                //try uncommenting this line and see how we can affect the elevation angle also
                //this.ElevationAngle += this.scrollSpeedElevationMultiplier * scrollWheelDelta;
            }
        }

        public override bool Equals(object obj)
        {
            ThirdPersonController other = obj as ThirdPersonController;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.elevationAngle.Equals(other.ElevationAngle)
                    && this.distance.Equals(other.Distance)
                      && this.translationLerpSpeed.Equals(other.TranslationLerpSpeed)
                        && this.lookLerpSpeed.Equals(other.LookLerpSpeed)
                        && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.elevationAngle.GetHashCode();
            hash = hash * 17 + this.distance.GetHashCode();
            hash = hash * 41 + this.translationLerpSpeed.GetHashCode();
            hash = hash * 47 + this.lookLerpSpeed.GetHashCode();
            hash = hash * 11 + base.GetHashCode();
            return hash;
        }

        //be careful when cloning this controller as we will need to reset the target actor - assuming the clone attaches to a different target
        public override object Clone()
        {
            return new ThirdPersonController("clone - " + this.ID, //deep
                this.ControllerType, //deep
                this.TargetActor as Actor, //shallow - a ref
                this.distance, //deep
                this.scrollSpeedDistanceMultiplier, //deep
                this.elevationAngle, //deep
                this.scrollSpeedElevationMultiplier, //deep
                this.translationLerpSpeed, //deep
                this.lookLerpSpeed, //deep
                this.mouseManager); //shallow - a ref
        }
    }
}
