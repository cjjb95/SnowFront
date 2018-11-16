/*
Function: 		Represents the parent class for all updateable AND drawn 3D game objects. Notice that Effect has been added.
Author: 		NMCG
Version:		1.0
Date Updated:	17/8/17
Bugs:			None
Fixes:			None
*/

using System;

namespace GDLibrary
{
    public class DrawnActor3D : Actor3D, ICloneable
    {
        #region Fields
        private EffectParameters effectParameters;
        #endregion

        #region Properties
        public EffectParameters EffectParameters
        {
            get
            {
                return this.effectParameters;
            }
            set
            {
                this.effectParameters = value;
            }
        }

        public float Alpha
        {
            get
            {
                return this.EffectParameters.Alpha;
            }
            set
            {
                //opaque to transparent AND valid (i.e. 0 <= x < 1)
                if(this.EffectParameters.Alpha == 1 && value < 1)
                {
                    EventDispatcher.Publish(new EventData("OpTr", this, EventActionType.OnOpaqueToTransparent, EventCategoryType.Opacity));
                }
                //transparent to opaque
                else if (this.EffectParameters.Alpha < 1 && value == 1)
                {
                    EventDispatcher.Publish(new EventData("TrOp", this, EventActionType.OnTransparentToOpaque, EventCategoryType.Opacity));
                }
                this.EffectParameters.Alpha = value;
            }
        }
        #endregion

        //used when we don't want to specify status type
        public DrawnActor3D(string id, ActorType actorType, Transform3D transform, EffectParameters effectParameters)
            : this(id, actorType, transform, effectParameters, StatusType.Drawn | StatusType.Update) 
        {
        }

        public DrawnActor3D(string id, ActorType actorType, Transform3D transform, EffectParameters effectParameters, StatusType statusType) 
            : base(id, actorType, transform, statusType)
        {
            this.effectParameters = effectParameters;
        }

        public override float GetAlpha()
        {
            return this.EffectParameters.Alpha;
        }

        public override bool Equals(object obj)
        {
            DrawnActor3D other = obj as DrawnActor3D;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.effectParameters.Equals(other.EffectParameters) && this.Alpha.Equals(other.Alpha) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.effectParameters.GetHashCode();
            hash = hash * 43 + base.GetHashCode();
            return hash;
        }

        public new object Clone()
        {
            IActor actor = new DrawnActor3D("clone - " + ID, //deep
                this.ActorType, //deep
                (Transform3D)this.Transform.Clone(), //deep - calls the clone for Transform3D explicitly
                this.EffectParameters.GetDeepCopy(), //hybrid - shallow (texture and effect) and deep (all other fields) 
                this.StatusType); //deep - a simple numeric type

            if (this.ControllerList != null)
            {
                //clone each of the (behavioural) controllers
                foreach (IController controller in this.ControllerList)
                    actor.AttachController((IController)controller.Clone());
            }

            return actor;
        }

        public override bool Remove()
        {
            this.effectParameters = null;
            return base.Remove();
        }


    }
}
