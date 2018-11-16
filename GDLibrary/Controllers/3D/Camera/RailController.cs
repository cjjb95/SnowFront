/*
Function: 		Rail controller constrains an actor to movement along a rail and causes the actor to focus on a target.
Author: 		NMCG
Version:		1.0
Date Updated:	30/8/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RailController : TargetController
    {
        #region Fields
        private RailParameters railParameters;
        private bool bFirstUpdate = true;
        #endregion

        #region Properties
        public RailParameters RailParameters
        {
            get
            {
                return this.railParameters;
            }
            set
            {
                this.railParameters = value;
            }
        }
        #endregion

        public RailController(string id, ControllerType controllerType, IActor targetActor, 
                                RailParameters railParameters)
            : base(id, controllerType, targetActor)
        {
            this.railParameters = railParameters;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;
            DrawnActor3D targetDrawnActor = this.TargetActor as DrawnActor3D;

            if (targetDrawnActor != null)
            {
                if (this.bFirstUpdate)
                {
                    //set the initial position of the camera
                    parentActor.Transform.Translation = railParameters.MidPoint;
                    this.bFirstUpdate = false;
                }

                //get look vector to target
                Vector3 cameraToTarget = MathUtility.GetNormalizedObjectToTargetVector(parentActor.Transform, targetDrawnActor.Transform);
                cameraToTarget = MathUtility.Round(cameraToTarget, 3); //round to prevent floating-point precision errors across updates

                //new position for camera if it is positioned between start and the end points of the rail
                Vector3 projectedCameraPosition = parentActor.Transform.Translation + Vector3.Dot(cameraToTarget, railParameters.Look) * railParameters.Look;// gameTime.ElapsedGameTime.Milliseconds; //removed gameTime multiplier - was causing camera judder when object close to camera
                projectedCameraPosition = MathUtility.Round(projectedCameraPosition, 3); //round to prevent floating-point precision errors across updates

                //do not allow the camera to move outside the rail
                if (railParameters.InsideRail(projectedCameraPosition))
                {
                    parentActor.Transform.Translation = projectedCameraPosition;
                }

                //set the camera to look at the object
                parentActor.Transform.Look = cameraToTarget;
            }
        }

        //Add Equals, Clone, ToString, GetHashCode...
    }
}
