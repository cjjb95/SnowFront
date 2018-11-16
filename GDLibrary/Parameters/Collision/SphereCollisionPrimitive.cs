/*
Function: 		Represents a simple (non-JigLibX) form of sphere collision primitive that can be attached to a PrimitiveObject
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
    public class SphereCollisionPrimitive : ICollisionPrimitive
    {
        #region Variables
        private BoundingSphere boundingSphere;
        private float radius;
        private Transform3D transform3D;
        #endregion

        #region Properties
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value > 0 ? value : 1;
            }
        }
        public BoundingSphere BoundingSphere
        {
            get
            {
                return boundingSphere;
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

        public SphereCollisionPrimitive(Transform3D transform3D, float radius)
        {
            this.transform3D = transform3D;
            this.radius = radius;
        }

        public bool Intersects(ICollisionPrimitive collisionPrimitive)
        {
            return collisionPrimitive.Intersects(this.boundingSphere);
        }

        //tests if the bounding sphere for this primitive, when moved, will intersect with the collisionPrimitive passed into the method
        public bool Intersects(ICollisionPrimitive collisionPrimitive, Vector3 translation)
        {
            BoundingSphere projectedSphere = this.boundingSphere;
            projectedSphere.Center += translation;
            return collisionPrimitive.Intersects(projectedSphere);
        }

        public bool Intersects(BoundingBox box)
        {
            return this.boundingSphere.Intersects(box);      
        }

        public bool Intersects(BoundingSphere sphere)
        {
            return this.boundingSphere.Intersects(sphere);
        }

        public bool Intersects(Ray ray)
        {
            return (ray.Intersects(this.boundingSphere) > 0);
        }

        //detect intersection and passes back distance to intersected primitive
        public bool Intersects(Ray ray, out float? distance)
        {
            distance = ray.Intersects(this.boundingSphere);
            return (distance > 0);
        }

        public bool Intersects(BoundingFrustum frustum)
        {
            return ((frustum.Contains(this.boundingSphere) == ContainmentType.Contains)
            || (frustum.Contains(this.boundingSphere) == ContainmentType.Intersects));
        }

        public void Update(GameTime gameTime, Transform3D transform)
        {  
            this.boundingSphere = new BoundingSphere(transform.Translation, this.radius);
        }

       
        public override string ToString()
        {
            return this.boundingSphere.ToString();
        }

        public object Clone()
        {
            return new SphereCollisionPrimitive((Transform3D)this.Transform3D.Clone(), 
                this.Radius);
        }
    }
}
