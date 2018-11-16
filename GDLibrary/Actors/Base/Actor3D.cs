/*
Function: 		Represents the parent class for all updateable 3D game objects. Notice that Transform3D and List<IController> has been added.
Author: 		NMCG
Version:		1.0
Date Updated:	17/8/17
Bugs:			None
Fixes:			None
*/

using System;
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class Actor3D : Actor, ICloneable
    {
        #region Fields
        private Transform3D transform;
        #endregion

        #region Properties
        public Transform3D Transform
        {
            get
            {
                return this.transform;
            }
            set
            {
                this.transform = value;
            }
        }
        #endregion

        public Actor3D(string id, ActorType actorType,
                            Transform3D transform, StatusType statusType)
            : base(id, actorType, statusType)
        {

            this.transform = transform;
        }

        //returns the compound matrix transformation that will scale, rotate and place the actor in the 3D world of the game
        public override Matrix GetWorldMatrix()
        {
            return this.transform.World;
        }

        public virtual void Draw(GameTime gameTime)
        {

        }

        public override bool Equals(object obj)
        {
            Actor3D other = obj as Actor3D;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.Transform.Equals(other.Transform) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.Transform.GetHashCode();
            hash = hash * 17 + base.GetHashCode();
            return hash;
        }

        public new object Clone()
        {
            IActor actor = new Actor3D("clone - " + ID, //deep
                this.ActorType, //deep
                (Transform3D)this.transform.Clone(), //deep
                this.StatusType); //shallow

            //clone each of the (behavioural) controllers
            foreach(IController controller in this.ControllerList)
                actor.AttachController((IController)controller.Clone());

            return actor;
        }

        public override bool Remove()
        {
            //tag for garbage collection
            this.transform = null;
            return base.Remove();
        }
    }
}
