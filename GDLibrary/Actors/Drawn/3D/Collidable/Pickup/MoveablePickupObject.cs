/*
Function: 		Represents a moveable, collectable and collidable object within the game that can be picked up (e.g. ammo that can fall off a table) 
Author: 		NMCG
Version:		1.0
Date Updated:	13/11/17
Bugs:			None
Fixes:			None
*/
using GDLibrary;
using JigLibX.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class MoveablePickupObject : CollidableObject
    {
        #region Fields
        private PickupParameters pickupParameters;
        #endregion

        #region Properties
        public PickupParameters PickupParameters
        {
            get
            {
                return this.pickupParameters;
            }
            set
            {
                this.pickupParameters = value;
            }
        }
        #endregion

        public MoveablePickupObject(string id, ActorType actorType, Transform3D transform, EffectParameters effectParameters,
            Model model, PickupParameters pickupParameters)
            : base(id, actorType, transform, effectParameters, model)
        {
            this.pickupParameters = pickupParameters;

            //register for callback on CDCR
            this.Body.CollisionSkin.callbackFn += CollisionSkin_callbackFn;
        }

        #region Event Handling
        protected virtual bool CollisionSkin_callbackFn(CollisionSkin collider, CollisionSkin collidee)
        {
            HandleCollisions(collider.Owner.ExternalData as CollidableObject, collidee.Owner.ExternalData as CollidableObject);
            return true;
        }
        //how do we want this object to respond to collisions?
        private void HandleCollisions(CollidableObject collidableObjectCollider, CollidableObject collidableObjectCollidee)
        {
            ////add your response code here...
            //if(collidableObjectCollider.ActorType == ActorType.Player)
            //{
            //    EventDispatcher.Publish(new EventData(this, EventActionType.OnRemoveActor, EventCategoryType.SystemRemove));
            //}
        }
        #endregion

        //public new object Clone()
        //{
        //    return new CollidableObject("clone - " + ID, //deep
        //        this.ActorType,   //deep
        //        (Transform3D)this.Transform.Clone(),  //deep
        //        this.Effect, //shallow i.e. a reference
        //        new ColorParameters(this.Color, this.Alpha),  //deep
        //        this.Texture, //shallow i.e. a reference
        //        this.Model); //shallow i.e. a reference
        //}

        //public override bool Remove()
        //{
        //    this.body = null;
        //    return base.Remove();
        //}

    }
}
