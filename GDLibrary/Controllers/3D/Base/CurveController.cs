/*
Function: 		Creates a controller which moves the attached object along a user defined 3D curve
Author: 		NMCG
Version:		1.1
Date Updated:	8/10/17
Bugs:			Judder in path
Fixes:			Updated evaluation precision on UpdateTrack() to a default of 4 point decimal precision
*/

using Microsoft.Xna.Framework;
namespace GDLibrary
{
    //used to cause an object (or camera) to move along a predefined track
    public class CurveController : Controller
    {
        private static readonly int DefaultCurveEvaluationPrecision = 4;

        #region Fields
        private Transform3DCurve transform3DCurve;
        private PlayStatusType playStatusType;
        private float elapsedTimeInMs;
        private int curveEvaluationPrecision;
        #endregion

        #region Properties
        public Transform3DCurve Transform3DCurve
        {
            get
            {
                return this.transform3DCurve;
            }
            set
            {
                this.transform3DCurve = value;
            }
        }
        public PlayStatusType PlayStatusType
        {
            get
            {
                return this.playStatusType;
            }
            set
            {
                this.playStatusType = value;
            }
        }
        public int CurveEvaluationPrecision
        {
            get
            {
                return this.curveEvaluationPrecision;
            }
            set
            {
                this.curveEvaluationPrecision = value;
            }
        }
        #endregion

        //pre-curveEvaluationPrecision compatability constructor
        public CurveController(string id, ControllerType controllerType,
            Transform3DCurve transform3DCurve, PlayStatusType playStatusType)
            : this(id, controllerType, transform3DCurve, playStatusType, DefaultCurveEvaluationPrecision)
        {
        }

        public CurveController(string id, ControllerType controllerType,
            Transform3DCurve transform3DCurve, PlayStatusType playStatusType,
            int curveEvaluationPrecision)
            : base(id, controllerType)
        {
            this.transform3DCurve = transform3DCurve;
            this.playStatusType = playStatusType;
            this.elapsedTimeInMs = 0;
            this.curveEvaluationPrecision = curveEvaluationPrecision;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;

            if (this.playStatusType == PlayStatusType.Play)
                UpdateTrack(gameTime, parentActor);
            else if ((this.playStatusType == PlayStatusType.Reset) || (this.playStatusType == PlayStatusType.Stop))
                this.elapsedTimeInMs = 0;
        }

        private void UpdateTrack(GameTime gameTime, Actor3D parentActor)
        {
            if (parentActor != null)
            {
                this.elapsedTimeInMs += gameTime.ElapsedGameTime.Milliseconds;

                Vector3 translation, look, up;
                this.transform3DCurve.Evalulate(elapsedTimeInMs, this.curveEvaluationPrecision,
                    out translation, out look, out up);

                parentActor.Transform.Translation = translation;
                parentActor.Transform.Look = look;
                parentActor.Transform.Up = up;
            }
        }

        //add clone...
    }
}

