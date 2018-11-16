/*
Function: 		Represents a simple (non-JigLibX) form of axis-aligned box collision primitive that can be attached to a PrimitiveObject
                to enable simple CD/CR. Used for your I-CA project - see Main::InitializePrimtives().

Author: 		NMCG
Version:		1.0
Date Updated:	27/11/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class BoxCollisionPrimitive : ICollisionPrimitive
    {
        #region Variables
        private static Vector3 min = -1 * Vector3.One, max = Vector3.One;
        private BoundingBox boundingBox, originalBoundingBox;
        private Transform3D transform3D;
        #endregion

        #region Properties
        public BoundingBox BoundingBox
        {
            get
            {
                return boundingBox;
            }
        }
        private Transform3D Transform3D
        {
            get
            {
                return transform3D;
            }
        }
        #endregion

        public BoxCollisionPrimitive(Transform3D transform3D) 
        {
            this.transform3D = transform3D;
            this.boundingBox = new BoundingBox(transform3D.Scale/2 * min, transform3D.Scale/2 * max);
            this.originalBoundingBox = this.boundingBox;
        }

        public bool Intersects(BoundingBox box)
        {
            return this.boundingBox.Intersects(box);      
        }

        public bool Intersects(BoundingSphere sphere)
        {
            return this.boundingBox.Intersects(sphere);
        }

        public bool Intersects(ICollisionPrimitive collisionPrimitive)
        {
            return collisionPrimitive.Intersects(this.boundingBox);
        }

        //tests if the bounding box for this primitive, when moved, will intersect with the collisionPrimitive passed into the method
        public bool Intersects(ICollisionPrimitive collisionPrimitive, Vector3 translation)
        {
            BoundingBox projectedBox = this.boundingBox;
            projectedBox.Max += translation;
            projectedBox.Min += translation;
            return collisionPrimitive.Intersects(projectedBox);
        }

        public bool Intersects(Ray ray)
        {
            return (ray.Intersects(this.boundingBox) > 0);
        }

        //detect intersection and passes back distance to intersected primitive
        public bool Intersects(Ray ray, out float? distance)
        {
            distance = ray.Intersects(this.boundingBox);
            return (distance > 0);
        }

        public bool Intersects(BoundingFrustum frustum)
        {
            return ((frustum.Contains(this.boundingBox) == ContainmentType.Contains)
            || (frustum.Contains(this.boundingBox) == ContainmentType.Intersects));
        }

        public void Update(GameTime gameTime, Transform3D transform)
        {
            this.boundingBox.Max = originalBoundingBox.Max + transform.Translation;
            this.boundingBox.Min = originalBoundingBox.Min + transform.Translation;
        }

        public override string ToString()
        {
            return this.boundingBox.ToString();
        }

        public object Clone()
        {
            return new BoxCollisionPrimitive((Transform3D)this.Transform3D.Clone());
        }
    }
}
