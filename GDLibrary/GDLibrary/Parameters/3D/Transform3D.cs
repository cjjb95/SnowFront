/*
Function: 		Encapsulates the transformation and World matrix specific parameters for any 3D entity that can have a position (e.g. a player, a prop, a camera)
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
    public class Transform3D : ICloneable
    {
        #region Statics
        public static Transform3D Zero
        {
            get
            {
                return new Transform3D(Vector3.Zero, Vector3.Zero, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            }
        }
        #endregion

        #region Fields
        private Vector3 translation, rotation, scale;
        private Vector3 look, up;

        //store original values for reset
        private Vector3 originalTranslation, originalRotation, originalScale;
        private Vector3 originalLook, originalUp;
		private bool bDirty = true;
		private Matrix world;
        private double distanceToCamera; //used to sort transparent objects by distance from camera - see screencast on ObjectManager.
        #endregion

        #region Properties
        public Matrix World
        {
            get
            {
				if(this.bDirty)
				{
					this.world = Matrix.Identity * Matrix.CreateScale(scale)
                                    * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                                            * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                                                * Matrix.CreateTranslation(translation);
					this.bDirty = false;							
				}
				return this.world;
            }
        }
        public Vector3 Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.translation = value;
				this.bDirty = true;
            }
        }

  
        public Vector3 Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
				this.bDirty = true;				
            }
        }
        public Vector3 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
				this.bDirty = true;				
            }
        }

        public Vector3 Target
        {
            get
            {
                return this.translation + this.look;
            }
        }
        public Vector3 Up
        {
            get
            {
                return this.up;
            }
            set
            {
                this.up = value;
            }
        }
        public Vector3 Look
        {
            get
            {
                return this.look;
            }
            set
            {
                this.look = value;
            }
        }
        public Vector3 Right
        {
            get
            {
                return Vector3.Normalize(Vector3.Cross(this.look, this.up));
            }
        }

        public Vector3 OriginalTranslation
        {
            get
            {
                return this.originalTranslation;
            }
        }
        public Vector3 OriginalRotation
        {
            get
            {
                return this.originalRotation;
            }
        }
        public Vector3 OriginalScale
        {
            get
            {
                return this.originalScale;
            }
        }
        public double DistanceToCamera
        {
            get
            {
                return this.distanceToCamera;
            }
            set
            {
                this.distanceToCamera = value;
            }
        }
        #endregion

        #region Constructors & Others
        public Transform3D(Vector3 translation, Vector3 rotation, Vector3 scale, Vector3 look, Vector3 up)
        {
            this.originalTranslation = Translation = translation;
            this.originalRotation = Rotation = rotation;
            this.originalScale = Scale = scale;

            this.originalLook = Look = Vector3.Normalize(look);
            this.originalUp = Up = Vector3.Normalize(up);
        }

        public override bool Equals(object obj)
        {
            Transform3D other = obj as Transform3D;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return Vector3.Equals(this.translation, other.Translation)
                && Vector3.Equals(this.rotation, other.Rotation)
                    && Vector3.Equals(this.scale, other.Scale)
                        && Vector3.Equals(this.look, other.Look)
                         && Vector3.Equals(this.up, other.Up);
        }

        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.translation.GetHashCode();
            hash = hash * 17 + this.look.GetHashCode();
            hash = hash * 13 + this.up.GetHashCode();
            return hash;
        }

        public object Clone()
        {
            //deep because all variables are either C# types (e.g. primitives, structs, or enums) or  XNA types
            return this.MemberwiseClone();
        }
        #endregion

        #region Transformation
        public void RotateBy(Vector3 rotateBy) //in degrees
        {
            //always rotate by adding to an original untouched value
            this.rotation = this.originalRotation + rotateBy;

            //update the look and up - RADIANS!!!!
            Matrix rot = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(this.rotation.X),
                MathHelper.ToRadians(this.rotation.Y), MathHelper.ToRadians(this.rotation.Z));

            this.look = Vector3.Transform(this.originalLook, rot);
            this.up = Vector3.Transform(this.originalUp, rot);
			
			this.bDirty = true;
        }

        public void RotateAroundYBy(float magnitude) //in degrees
        {
            this.rotation.Y += magnitude;
            this.look = Vector3.Normalize(Vector3.Transform(this.originalLook, Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))));
			this.bDirty = true;
        }

        public void TranslateTo(Vector3 translate)
        {
            this.translation = translate;
			this.bDirty = true;
        }

        public void TranslateBy(Vector3 translateBy)
        {
            this.translation += translateBy;
			this.bDirty = true;
        }

        public void ScaleTo(Vector3 scale)
        {
            this.scale = scale;
			this.bDirty = true;
        }

        public void ScaleBy(Vector3 scaleBy)
        {
            this.scale *= scaleBy;
			this.bDirty = true;
        }
        #endregion
    }
}