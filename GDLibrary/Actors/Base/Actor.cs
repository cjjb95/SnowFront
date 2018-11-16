/*
Function: 		Represents the parent class for all updateable 3D AND 2D game objects (e.g. camera,pickup, player, menu text). Notice that this class doesn't 
                have any positional information (i.e. a Transform3D or Transform2D). This will allow us to use Actor as the parent for both 3D and 2D game objects (e.g. a player or a string of menu text).
Author: 		NMCG
Version:		1.0
Date Updated:	17/8/17
Bugs:			None
Fixes:			None
*/

using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GDLibrary
{
    public class Actor : IActor, ICloneable
    {
        #region Fields
        private string id;
        private ActorType actorType;
        private StatusType statusType;
        private List<IController> controllerList;
        private GroupParameters groupParameters;
        #endregion

        #region Properties
        public ActorType ActorType
        {
            get
            {
                return this.actorType;
            }
            set
            {
                this.actorType = value;
            }
        }
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public StatusType StatusType
        {
            get
            {
                return this.statusType;
            }
            set
            {
                this.statusType = value;
            }
        }

        public List<IController> ControllerList
        {
            get
            {
                return this.controllerList;
            }
        }

        /* We wont add this new parameter to the constructor as it will affect too many child classes.
         * Instead, if a user wishes to set the parameter, s/he can use code similar to the code below:
         * 
         *  Camera3D camera = new Camera(...);
         *  camera.GroupParameters = new GroupParameters(...);
         */

        public GroupParameters GroupParameters
        {
            get
            {
                return this.groupParameters;
            }
            set
            {
                this.groupParameters = value;
            }
        }
        #endregion

        public Actor(string id, ActorType actorType, StatusType statusType)
        {
            this.id = id;
            this.actorType = actorType;
            this.statusType = statusType;
        }

        public virtual Matrix GetWorldMatrix()
        {
            return Matrix.Identity; //does nothing - see derived classes especially CollidableObject
        }
        public virtual ActorType GetActorType()
        {
            return this.actorType;
        }
        public virtual string GetID()
        {
            return this.id;
        }
        public virtual float GetAlpha()
        {
            return 1;
        }
        public virtual StatusType GetStatusType()
        {
            return this.statusType;
        }

        public override bool Equals(object obj)
        {
            Actor other = obj as Actor;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            bool bEquals = this.id.Equals(other.ID)
                && this.actorType == other.ActorType
                    && this.statusType.Equals(other.StatusType);

            //update for new parameter
            if (this.groupParameters != null)
                bEquals = bEquals && this.groupParameters.Equals(other.GroupParameters);

            return bEquals;

        }
        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 7 + this.ID.GetHashCode();
            hash = hash * 11 + this.actorType.GetHashCode();
            hash = hash * 17 + this.statusType.GetHashCode();

            //update for new parameter
            if (this.groupParameters != null)
                hash = hash * 17 + this.groupParameters.GetHashCode();

            return hash;
        }
        public object Clone()
        {
            //update for new parameter
            Actor clone = new Actor(this.id, this.ActorType, this.StatusType);
            //remember using "as" is more flexible than a traditional typecast. Why?
            clone.GroupParameters = this.groupParameters.Clone() as GroupParameters;
            return clone;
        }
        public virtual bool Remove()
        {
            //tag for garbage collection
            if (this.controllerList != null)
            {
                this.controllerList.Clear();
                this.controllerList = null;
            }

            return true;
        }
        public virtual void Update(GameTime gameTime)
        {
            if (this.controllerList != null &&  ((this.StatusType & StatusType.Update) == StatusType.Update))
            {
                foreach (IController controller in this.controllerList)
                    controller.Update(gameTime, this); //you control me, update!
            }
        }

        #region Controller Specific
        public virtual void AttachController(IController controller)
        {
            if (this.controllerList == null)
                this.controllerList = new List<IController>();
            this.controllerList.Add(controller); //duplicates?
        }
        public virtual bool DetachController(IController controller)
        {
            if (this.controllerList != null)
                return this.controllerList.Remove(controller);

            return false;
        }
        public virtual int DetachControllers(Predicate<IController> predicate)
        {
            List<IController> findList = FindControllers(predicate);

            if (findList != null)
            {
                foreach (IController controller in findList)
                    this.controllerList.Remove(controller);
            }

            return findList.Count;
        }
        public List<IController> FindControllers(Predicate<IController> predicate)
        {
            return this.controllerList.FindAll(predicate);
        }
        //allows us to set the PlayStatus for all controllers simultaneously (e.g. play all, reset all, stop all)
        public virtual void SetAllControllers(PlayStatusType playStatusType)
        {
            if (this.controllerList != null)
            {
                foreach (IController controller in this.controllerList)
                    controller.SetControllerPlayStatus(playStatusType);
            }
        }
        //allows us to set the PlayStatus for all controllers with the same GROUP parameters simultaneously (e.g. "play" all controllers with a group ID of 1)
        public virtual void SetAllControllers(PlayStatusType playStatusType, Predicate<IController> predicate)
        {
            List<IController> findList = FindControllers(predicate);
            if (findList != null)
            {
                foreach (IController controller in findList)
                    controller.SetControllerPlayStatus(playStatusType);
            }
        }
        #endregion

    }
}
