/*
Function: 		Parent class for all controllers which adds id and controller type
Author: 		NMCG
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class Controller : IController
    {
        #region Fields
        private string id;
        private ControllerType controllerType;
        private PlayStatusType playStatusType;
        #endregion

        #region Properties
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
        public ControllerType ControllerType
        {
            get
            {
                return this.controllerType;
            }
            set
            {
                this.controllerType = value;
            }
        }
        public PlayStatusType PlayStatusType
        {
            get
            {
                return this.playStatusType;
            }
            set
            {
                this.playStatusType = value;
            }
        }
        #endregion
        public Controller(string id, ControllerType controllerType)
            : this(id, controllerType, PlayStatusType.Play)
        {

        }

        public Controller(string id, ControllerType controllerType, PlayStatusType playStatusType)
        {
            this.id = id;
            this.controllerType = controllerType;
            this.playStatusType = playStatusType;
        }

        public virtual string GetID()
        {
            return this.ID;
        }

        public virtual void SetActor(IActor actor)
        {
            //does nothing - no point in child classes calling this - see UIScaleLerpController::Reset()
        }

        public virtual void Update(GameTime gameTime, IActor actor)
        {
            //does nothing - no point in child classes calling this.
        }

        //allows us to play, pause, stop, reset a controller
        public virtual void SetPlayStatus(PlayStatusType playStatusType)
        {
            this.playStatusType = playStatusType;
        }
        
        public virtual PlayStatusType GetPlayStatus()
        {
            return this.playStatusType;
        }

        public virtual ControllerType GetControllerType()
        {
            return this.controllerType;
        }

        public override bool Equals(object obj)
        {
            Controller other = obj as Controller;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.ID.Equals(other.ID) 
                && this.controllerType.Equals(other.ControllerType)
                    && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.ID.GetHashCode();
            hash = hash * 17 + this.controllerType.GetHashCode();
            return hash;
        }

        public virtual object Clone()
        {
            return new Controller("clone - " + this.ID, this.controllerType);
        }
    }
}
