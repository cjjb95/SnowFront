/*
Function: 		Represents a simple static camera in our 3D world to which we will later attach controllers. 
Author: 		NMCG
Version:		1.1
Date Updated:	24/8/17
Bugs:			None
Fixes:			None
*/


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    //Represents the base camera class to which controllers can be attached (to do...)
    public class Camera3D : Actor3D
    {
        #region Fields
        private ProjectionParameters projectionParameters;
        private Viewport viewPort;
        //centre for each cameras viewport - important when deciding how much to turn the camera when a particular camera view, in a multi-screen layout, is in focus
        private Vector2 viewportCentre;
        //used to sort cameras by depth on screen where 0 = top-most, 1 = bottom-most (i.e. 0 for rear-view mirror and > 0 for main game screen)
        private float drawDepth;
        #endregion

        #region Properties
        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(this.Transform.Translation,
                    this.Transform.Translation + this.Transform.Look,
                    this.Transform.Up);
            }
        }
        public Matrix Projection
        {
            get
            {
                return this.projectionParameters.Projection;
            }
        }
        public ProjectionParameters ProjectionParameters
        {
            get
            {
                return this.projectionParameters;
            }
            set
            {
                this.projectionParameters = value;
            }
        }
        public Viewport Viewport
        {
            get
            {
                return this.viewPort;
            }
            set
            {
                this.viewPort = value;
                this.viewportCentre = new Vector2(this.viewPort.Width / 2.0f, this.viewPort.Height / 2.0f);
            }
        }

        public Vector2 ViewportCentre
        {
            get
            {
                return this.viewportCentre;
            }
        }
        public float DrawDepth
        {
            get
            {
                return this.drawDepth;
            }
            set
            {
                this.drawDepth = value;
            }
        }
        public BoundingFrustum BoundingFrustum
        {
            get
            {
                return new BoundingFrustum(this.View * this.projectionParameters.Projection);
            }
        }
        #endregion

        //constructor with default drawDepth set to 0
        public Camera3D(string id, ActorType actorType,
            Transform3D transform, ProjectionParameters projectionParameters,
            Viewport viewPort, StatusType statusType)
            : this(id, actorType, transform, projectionParameters, viewPort, 0, statusType)
        {
        }

        public Camera3D(string id, ActorType actorType,
            Transform3D transform, ProjectionParameters projectionParameters,
            Viewport viewPort, float drawDepth, StatusType statusType)
            : base(id, actorType, transform, statusType)
        {
            this.projectionParameters = projectionParameters;
            this.Viewport = viewPort;
            this.DrawDepth = drawDepth;

        }

        //creates a default camera3D - we can use this for a fixed camera archetype i.e. one we will clone - see MainApp::InitialiseCameras()
        public Camera3D(string id, ActorType actorType, Viewport viewPort)
            : this(id, actorType, Transform3D.Zero,
            ProjectionParameters.StandardMediumFourThree, viewPort, 0, StatusType.Update)
        {

        }

        public override bool Equals(object obj)
        {
            Camera3D other = obj as Camera3D;

            return Vector3.Equals(this.Transform.Translation, other.Transform.Translation)
                && Vector3.Equals(this.Transform.Look, other.Transform.Look)
                    && Vector3.Equals(this.Transform.Up, other.Transform.Up)
                        && this.ProjectionParameters.Equals(other.ProjectionParameters);
        }
        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.Transform.Translation.GetHashCode();
            hash = hash * 17 + this.Transform.Look.GetHashCode();
            hash = hash * 13 + this.Transform.Up.GetHashCode();
            hash = hash * 53 + this.ProjectionParameters.GetHashCode();
            return hash;
        }
        public new object Clone()
        {
            return new Camera3D("clone - " + this.ID,
                this.ActorType, (Transform3D)this.Transform.Clone(), 
                (ProjectionParameters)this.projectionParameters.Clone(), this.Viewport, 0, StatusType.Update);
        }
        public override string ToString()
        {
            return this.ID
                + ", Translation: " + MathUtility.Round(this.Transform.Translation, 0)
                    + ", Look: " + MathUtility.Round(this.Transform.Look, 0)
                        + ", Up: " + MathUtility.Round(this.Transform.Up, 0)
                            +", Depth: " + this.drawDepth;

        }
    }
}

