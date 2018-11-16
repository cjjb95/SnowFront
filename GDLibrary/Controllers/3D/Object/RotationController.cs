/*
Function: 		Rotates (i.e. pitch, yaw, roll) target actor - around a specified axis and at an angular speed - defined by the rotation vector's direction and magnitude.
                For example rotation == (1,0,0) wil cause one degree of rotation around the x-axis (pitch) for each game update.
Author: 		NMCG
Version:		1.0
Date Updated:	24/10/17
Bugs:			None
Fixes:			None
*/
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class RotationController : Controller
    {
        #region Fields
        private Vector3 rotation;
        private int count = 0;
        #endregion

        #region Properties
        public Vector3 Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
            }
        }
        #endregion
        public RotationController(string id, ControllerType controllerType, Vector3 rotation)
            : base(id, controllerType)
        {
            this.rotation = rotation;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;
            if (parentActor != null)
            {
                parentActor.Transform.RotateBy(this.rotation * count * gameTime.ElapsedGameTime.Milliseconds);
                count++;
            }
        }

        public override bool Equals(object obj)
        {
            RotationController other = obj as RotationController;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.rotation.Equals(other.Rotation)
                        && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.rotation.GetHashCode();
            hash = hash * 11 + base.GetHashCode();
            return hash;
        }

        public override object Clone()
        {
            return new RotationController("clone - " + this.ID, //deep
                this.ControllerType, //deep
                this.rotation); //deep
        }
    }
}

