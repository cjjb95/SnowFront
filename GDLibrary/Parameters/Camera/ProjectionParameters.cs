/*
Function: 		Encapsulates the projection matrix specific parameters for the camera class
Author: 		NMCG
Version:		1.0
Date Updated:	1/10/17
Bugs:			None
Fixes:			None
*/

using System;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class ProjectionParameters : ICloneable
    {
        #region Statics
        //Deep relates to the distance between the near and far clipping planes i.e. 1 to 2500
        public static ProjectionParameters StandardDeepFiveThree
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 5.0f / 3, 0.1f, 2500);
            }
        }

        public static ProjectionParameters StandardDeepFourThree
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 4.0f / 3, 0.1f, 2500);
            }
        }


        public static ProjectionParameters StandardDeepSixteenTen
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 16.0f / 10, 0.1f, 2500);
            }
        }

        public static ProjectionParameters StandardDeepSixteenNine
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver4, 16.0f / 9, 1, 2500);
            }
        }

        //Medium relates to the distance between the near and far clipping planes i.e. 1 to 1000
        public static ProjectionParameters StandardMediumFiveThree
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 5.0f / 3, 0.1f, 1000);
            }
        }

        public static ProjectionParameters StandardMediumFourThree
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 4.0f / 3, 0.1f, 1000);
            }
        }

        public static ProjectionParameters StandardMediumSixteenTen
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 16.0f / 10, 0.1f, 1000);
            }
        }

        public static ProjectionParameters StandardMediumSixteenNine
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver4, 16.0f / 9, 0.1f, 1000);
            }
        }

        //Shallow relates to the distance between the near and far clipping planes i.e. 1 to 500
        public static ProjectionParameters StandardShallowFiveThree
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 5.0f / 3, 0.1f, 500);
            }
        }

        public static ProjectionParameters StandardShallowFourThree
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 4.0f / 3, 0.1f, 500);
            }
        }

        public static ProjectionParameters StandardShallowSixteenTen
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 16.0f / 10, 0.1f, 500);
            }
        }

        public static ProjectionParameters StandardShallowSixteenNine
        {
            get
            {
                return new ProjectionParameters(MathHelper.PiOver2, 16.0f / 9, 0.1f, 500);
            }
        }
        #endregion

        #region Fields
        //used by perspective projections
        private float fieldOfView, aspectRatio, nearClipPlane, farClipPlane;

        //used by orthographic projections
        private Rectangle rectangle;
        private bool isPerspectiveProjection;

        //used by both
        private Matrix projection;
        private ProjectionParameters originalProjectionParameters;
        private bool isDirty;
        #endregion

        #region Properties

        #region Orthographic Specific Properties
        public Rectangle Rectangle
        {
            get
            {
                return this.rectangle;
            }
            set
            {
                this.rectangle = value;
                this.isDirty = true;
            }
        }
        public bool IsPerspectiveProjection
        {
            get
            {
                return this.isPerspectiveProjection;
            }
            set
            {
                this.isPerspectiveProjection = value;
                this.isDirty = true;
            }
        }
        #endregion

        #region Perspective Specific Properties
        public float FOV
        {
            get
            {
                return this.fieldOfView;
            }
            set
            {
                this.fieldOfView = value;
                this.isDirty = true;
            }
        }
        public float AspectRatio
        {
            get
            {
                return this.aspectRatio;
            }
            set
            {
                this.aspectRatio = value;
                this.isDirty = true;
            }
        }
        #endregion

        public float NearClipPlane
        {
            get
            {
                return this.nearClipPlane;
            }
            set
            {
                this.nearClipPlane = value;
                this.isDirty = true;
            }
        }
        public float FarClipPlane
        {
            get
            {
                return this.farClipPlane;
            }
            set
            {
                this.farClipPlane = value;
                this.isDirty = true;
            }
        }

        public Matrix Projection
        {
            get
            {
                if (this.isDirty)
                {
                    if (this.isPerspectiveProjection)
                    {
                        this.projection = Matrix.CreatePerspectiveFieldOfView(
                            this.fieldOfView, this.aspectRatio,
                            this.nearClipPlane, this.farClipPlane);
                    }
                    else
                    {
                        this.projection = Matrix.CreateOrthographicOffCenter(
                            this.rectangle.X, this.rectangle.Y, this.rectangle.X, this.rectangle.Y,
                            this.nearClipPlane, this.farClipPlane);
                    }
                    this.isDirty = false; 
                }
                return this.projection;
            }
        }
        #endregion

        public ProjectionParameters(Rectangle rectangle,
          float nearClipPlane, float farClipPlane)
        {
            this.Rectangle = rectangle;
            this.NearClipPlane = nearClipPlane;
            this.FarClipPlane = farClipPlane;
            this.IsPerspectiveProjection = false;
            this.originalProjectionParameters = (ProjectionParameters)this.Clone();
        }

        public ProjectionParameters(float fieldOfView, float aspectRatio,
            float nearClipPlane, float farClipPlane)
        {
            this.FOV = fieldOfView;
            this.AspectRatio = aspectRatio;
            this.NearClipPlane = nearClipPlane;
            this.FarClipPlane = farClipPlane;
            this.IsPerspectiveProjection = true;
            this.originalProjectionParameters = (ProjectionParameters)this.Clone();
        }

        public void Reset()
        {
            this.FOV = this.originalProjectionParameters.FOV;
            this.AspectRatio = this.originalProjectionParameters.AspectRatio;
            this.NearClipPlane = this.originalProjectionParameters.NearClipPlane;
            this.FarClipPlane = this.originalProjectionParameters.FarClipPlane;
            this.Rectangle = this.originalProjectionParameters.Rectangle;
            this.IsPerspectiveProjection = this.originalProjectionParameters.IsPerspectiveProjection;
        }

        public object Clone() //deep copy
        {
            //remember we can use a simple this.MemberwiseClone() because all fields are primitive C# types
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            ProjectionParameters other = obj as ProjectionParameters;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return float.Equals(this.FOV, other.FOV)
                && float.Equals(this.AspectRatio, other.AspectRatio)
                    && float.Equals(this.NearClipPlane, other.NearClipPlane)
                        && float.Equals(this.FarClipPlane, other.FarClipPlane)
                            && this.Rectangle.Equals(other.Rectangle);
        }

        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.FOV.GetHashCode();
            hash = hash * 17 + this.AspectRatio.GetHashCode();
            hash = hash * 13 + this.NearClipPlane.GetHashCode();
            hash = hash * 59 + this.FarClipPlane.GetHashCode();
            hash = hash * 53 + this.Rectangle.GetHashCode();
            return hash;
        }
    }
}
