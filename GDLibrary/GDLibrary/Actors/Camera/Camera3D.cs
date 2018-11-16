/*
Function: 		Represents a simple static camera in our 3D world to which we will later attach controllers. 
Author: 		NMCG
Version:		1.1
Date Updated:	
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary
{
    //Represents the base camera class to which controllers can be attached (to do...)
    public class Camera3D : Actor3D
    {
        #region Fields
        private ProjectionParameters projectionParameters;
        private Viewport viewport;
        private float drawDepth;
        #endregion

        #region Properties
        public float DrawDepth
        {
            get
            {
                return this.drawDepth;
            }
            set
            {
                this.drawDepth = (value >= 0 && value <= 1) ? value : 0;
            }
        }
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
                return this.viewport;
            }
            set
            {
                this.viewport = value;
            }
        }
        #endregion

        public Camera3D(string id, ActorType actorType,
           Transform3D transform, ProjectionParameters projectionParameters,
           Viewport viewport)
           : this(id, actorType,transform, projectionParameters,viewport, 1)
        {

        }


        public Camera3D(string id, ActorType actorType,
            Transform3D transform, ProjectionParameters projectionParameters, 
            Viewport viewport, float drawDepth)
            : base(id, actorType, transform)
        {
            this.projectionParameters = projectionParameters;
            this.viewport = viewport;
            this.DrawDepth = drawDepth; //used for stacking cameras when in PiP mode
        }


        public override bool Equals(object obj)
        {
            Camera3D other = obj as Camera3D;

            return Vector3.Equals(this.Transform.Translation, other.Transform.Translation)
                && Vector3.Equals(this.Transform.Look, other.Transform.Look)
                    && Vector3.Equals(this.Transform.Up, other.Transform.Up)
                        && this.ProjectionParameters.Equals(other.ProjectionParameters)
                            && this.viewport.Equals(other.Viewport);
        }
        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.Transform.Translation.GetHashCode();
            hash = hash * 17 + this.Transform.Look.GetHashCode();
            hash = hash * 13 + this.Transform.Up.GetHashCode();
            hash = hash * 53 + this.ProjectionParameters.GetHashCode();
            hash = hash * 61 + this.viewport.GetHashCode();
            return hash;
        }
        public new object Clone()
        {
            return new Camera3D("clone - " + this.ID,
                this.ActorType, (Transform3D)this.Transform.Clone(),
                (ProjectionParameters)this.projectionParameters.Clone(), this.viewport);
        }

        public override string ToString()
        {
            return this.ID + "[" 
                           + "Look: " + MathUtility.Round(this.Transform.Look, 1)
                           + ", Translation: " + MathUtility.Round(this.Transform.Translation, 1)
                           + "]";
        }

        public string GetDebugDescription()
        {
            return this.ID + "[T:"
                           + MathUtility.Round(this.Transform.Translation, 0)
                           + ",L:" + MathUtility.Round(this.Transform.Look, 0)
                           + "]";
        }
    }
}
