/*
Function: 		Represents the parent class for all updateable 3D AND 2D game objects (e.g. camera,pickup, player, menu text). Notice that this class doesn't 
                have any positional information (i.e. a Transform3D or Transform2D). This will allow us to use Actor as the parent for both 3D and 2D game objects (e.g. a player or a string of menu text).
Author: 		NMCG
Version:		1.0
Last Updated:
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using System;
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
        #endregion

        #region Properties
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
        protected List<IController> ControllerList
        {
            get
            {
                return this.controllerList;
            }
        }
        #endregion

        #region Constructors & Others
        public Actor(string id, ActorType actorType)
            : this(id, actorType, StatusType.Drawn | StatusType.Update)
        {
            
        }

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

        public virtual StatusType GetStatusType()
        {
            return this.statusType;
        }
        #endregion

        public virtual void Update(GameTime gameTime)
        {
            if (this.controllerList != null && ((this.StatusType & StatusType.Update) == StatusType.Update))
            {
                foreach (IController controller in this.controllerList)
                {
                    //new - only update the individual controller if it is playing
                    if(controller.GetPlayStatus() == PlayStatusType.Play)
                        controller.Update(gameTime, this); //update the actor that controller is attached to
                }
            }
        }

        #region Controller Specific
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

        //allows us to set the PlayStatus for all controllers (e.g. play all, reset all, stop all) corresponding to the predicate
        public virtual int SetControllerPlayStatus(PlayStatusType playStatusType, Predicate<IController> predicate)
        {
            List<IController> findList = null;

            if (this.controllerList != null)
            {
                findList = FindControllers(predicate);
                foreach (IController controller in findList)
                    controller.SetPlayStatus(playStatusType);
            }

            return findList == null ? 0 : findList.Count;
        }

        //allows us to set the PlayStatus for all controllers simultaneously (e.g. play all, reset all, stop all)
        public virtual int SetAllControllersPlayStatus(PlayStatusType playStatusType)
        {
            if (this.controllerList != null)
            {
                foreach (IController controller in this.controllerList)
                    controller.SetPlayStatus(playStatusType);
            }

            return this.controllerList == null ? 0 : this.controllerList.Count;
        }
        #endregion

        #region Clone, GetHashCode, Equals
        public override bool Equals(object obj)
        {
            Actor other = obj as Actor;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            bool bEquals = this.id.Equals(other.ID)
                && this.actorType == other.ActorType;

            return bEquals;

        }
        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 7 + this.ID.GetHashCode();
            hash = hash * 11 + this.actorType.GetHashCode();

            return hash;
        }
        public object Clone()
        {
            return new Actor(this.id, this.ActorType);
        }
        #endregion


    }
}