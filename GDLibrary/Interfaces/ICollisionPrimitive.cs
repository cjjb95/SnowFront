/*
Function: 		Represents the parent interface for all simple collision primitives (sphere and AA box).

Author: 		NMCG
Version:		1.0
Date Updated:	27/11/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public interface ICollisionPrimitive
    {
        bool Intersects(BoundingBox box);
        bool Intersects(BoundingSphere sphere);
        bool Intersects(BoundingFrustum frustum);
        bool Intersects(Ray ray);
        bool Intersects(ICollisionPrimitive collisionPrimitive);
        bool Intersects(ICollisionPrimitive collisionPrimitive, Vector3 translation);
        bool Intersects(Ray ray, out float? distance);
        void Update(GameTime gameTime, Transform3D transform);

        object Clone();
    }
}
