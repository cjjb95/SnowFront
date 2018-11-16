/*
Function: 		Encapsulates the transformation, bounding rectangle, and World matrix specific parameters for any 2D entity that can have a position on screen (e.g. UI text, a clickable button, game state information)
Author: 		NMCG
Version:		1.0
Date Updated:	11/9/17
Bugs:			None
Fixes:			None
*/
using System;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class Transform2D : ICloneable
    {
        #region Statics
        public static Transform2D One
        {
            get
            {
                return new Transform2D(Vector2.One);
            }
        }
        #endregion

        #region Fields
        private Vector2 translation, scale;
        private float rotationInDegrees, rotationInRadians;
        private Vector2 origin;

        private bool bBoundsDirty;
        private Matrix originMatrix, translationMatrix, rotationMatrix, scaleMatrix;

        private Rectangle bounds, originalBounds;
        private Integer2 originalDimensions;

        //used to reset object
        private Transform2D originalTransform2D;
        #endregion

        #region Properties     
        protected Matrix RotationMatrix
        {
            get
            {
                return this.rotationMatrix;
            }
        }
        protected Matrix PositionMatrix
        {
            get
            {
                return this.translationMatrix;
            }
        }
        public Vector2 Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.translation = value;
                this.translationMatrix = Matrix.CreateTranslation(new Vector3(this.translation, 0));
                this.bBoundsDirty = true;
            }
        }
        public float RotationInDegrees
        {
            get
            {
                return this.rotationInDegrees;
            }
            set
            {
                this.rotationInDegrees = value;
                this.rotationInDegrees %= 360;
                this.rotationInRadians = MathHelper.ToRadians(rotationInDegrees);
                this.rotationMatrix = Matrix.CreateRotationZ(this.rotationInRadians);
                this.bBoundsDirty = true;
            }
        }
        public float RotationInRadians
        {
            get
            {
                return this.rotationInRadians;
            }
        }
        public Vector2 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                //do not allow scale to go to zero
                this.scale = (value != Vector2.Zero) ? value : Vector2.One;
                this.scaleMatrix = Matrix.CreateScale(new Vector3(this.scale, 1));
                this.bBoundsDirty = true;
            }
        }
        public Vector2 Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
                this.originMatrix = Matrix.CreateTranslation(new Vector3(-origin, 0));
                this.bBoundsDirty = true;
            }
        }
        public Matrix World
        {
            get
            {
                return originMatrix * scaleMatrix * rotationMatrix * translationMatrix;
            }
        }
        public Rectangle Bounds
        {
            get
            {
                if (this.bBoundsDirty)
                {
                    //calculate where the new bounding box is in screen space based on the ISRoT transformation from the World matrix
                    this.bounds = CollisionUtility.CalculateTransformedBoundingRectangle(this.originalBounds, this.World);
                    this.bBoundsDirty = false;
                }

                return this.bounds;
            }
        }
        public Rectangle OriginalBounds
        {            
            get
            {
                return this.originalBounds;
            }
        }
        public Transform2D OriginalTransform2D
        {
            get
            {
                return this.originalTransform2D;
            }
        }
        #endregion

        //used by dynamic sprites i.e. which need a look and right vector for movement
        public Transform2D(Vector2 translation, float rotationInDegrees, Vector2 scale,
            Vector2 origin, Integer2 dimensions)
        {
            Initialize(translation, rotationInDegrees, scale, origin, dimensions); 

            //store original values in case of reset
            this.originalTransform2D = new Transform2D();
            this.originalTransform2D.Initialize(translation, rotationInDegrees, scale, origin, dimensions);
        }

        //used by static background sprites that cover the entire screen OR more than the entire screen
        public Transform2D(Vector2 scale) : this(Vector2.Zero, 0, scale, Vector2.Zero, Integer2.Zero)
        {

        }
 
        //used internally when creating the originalTransform object
        private Transform2D()
        {

        }

        //called by constructor to setup the object
        protected void Initialize(Vector2 translation, float rotationInDegrees, Vector2 scale, Vector2 origin, Integer2 dimensions)
        {
            this.Translation = translation;
            this.Scale = scale;
            this.RotationInDegrees = rotationInDegrees;
            this.Origin = origin;

            //original bounding box based on the texture source rectangle dimensions
            this.originalBounds = new Rectangle(0, 0, dimensions.X, dimensions.Y);
            this.originalDimensions = dimensions;
        }

        //called if we ever wish to completely reset the object (e.g. after modifying a menu button with a controller we want to reset the button's position etc)
        public virtual void Reset()
        {
            Initialize(this.originalTransform2D.Translation, this.originalTransform2D.RotationInDegrees,
                this.originalTransform2D.Scale, this.originalTransform2D.Origin, this.originalTransform2D.originalDimensions);
        }

        public override bool Equals(object obj)
        {
            Transform2D other = obj as Transform2D;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.translation.Equals(other.Translation)
                && this.RotationInRadians.Equals(other.RotationInRadians)
                    && this.scale.Equals(other.Scale)
                        && this.origin.Equals(other.Origin);
        }
        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.translation.GetHashCode();
            hash = hash * 17 + this.RotationInRadians.GetHashCode();
            hash = hash * 13 + this.scale.GetHashCode();
            hash = hash * 7 + this.origin.GetHashCode();
            return hash;
        }
        public object Clone()
        {
            //deep because all variables are either C# types (e.g. primitives, structs, or enums) or  XNA types
            return this.MemberwiseClone();
        }
    }


}
