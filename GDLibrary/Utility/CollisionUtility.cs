using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class CollisionUtility
    {
        private static Vector2 leftBottom;
        private static Vector2 leftTop;
        private static Vector2 max;
        private static Vector2 min;
        private static Vector2 rightBottom;
        private static Vector2 rightTop;

        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public static Rectangle CalculateTransformedBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            //   Matrix inverseMatrix = Matrix.Invert(transform);
            // Get all four corners in local space
            leftTop = new Vector2(rectangle.Left, rectangle.Top);
            rightTop = new Vector2(rectangle.Right, rectangle.Top);
            leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)Math.Round(min.X), (int)Math.Round(min.Y),
                                 (int)Math.Round(max.X - min.X), (int)Math.Round(max.Y - min.Y));
        }

        //a predicate function to be used by PickingManager for ray picking of collidable objects - defines what types are valid
        public static bool IsCollidableObjectOfInterest(CollidableObject collidableObject)
        {
            //shouldnt be able to pick immovable things
            if (collidableObject.Collision.Owner.Immovable)
                return false;

            return collidableObject.ActorType == ActorType.CollidableProp || collidableObject.ActorType == ActorType.CollidablePickup;
        }

        public static bool IsCollidableObjectPlayer(CollidableObject collidableObject)
        {
            //shouldnt be able to pick immovable things
            if (collidableObject.Collision.Owner.Immovable)
                return false;

            return collidableObject.ActorType == ActorType.Player;
        }

        public static string GetMouseStringFromCollidableObject(CollidableObject collidableObject, float distanceToObject)
        {
            return collidableObject.ID + " [" + distanceToObject + "]";
        }
    }
}
