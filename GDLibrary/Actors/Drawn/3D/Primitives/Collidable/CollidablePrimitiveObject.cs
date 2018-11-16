using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class CollidablePrimitiveObject : PrimitiveObject
    {
        #region Variables
        //the skin used to wrap the object
        private ICollisionPrimitive collisionPrimitive;

        //the object that im colliding with
        private Actor collidee;
        private ObjectManager objectManager;

        #endregion

        #region Properties
        //returns a reference to whatever this object is colliding against
        public Actor Collidee
        {
            get
            {
                return collidee;
            }
            set
            {
                collidee = value;
            }
        }
        public ICollisionPrimitive CollisionPrimitive
        {
            get
            {
                return collisionPrimitive;
            }
            set
            {
                collisionPrimitive = value;
            }
        }
        public ObjectManager ObjectManager
        {
            get
            {
                return this.objectManager;
            }
        }

        #endregion

        //used to draw collidable primitives that have a texture i.e. use VertexPositionColor vertex types only
        public CollidablePrimitiveObject(string id, ActorType actorType, Transform3D transform,
            EffectParameters effectParameters, StatusType statusType, IVertexData vertexData,
             ICollisionPrimitive collisionPrimitive, ObjectManager objectManager)
            : base(id, actorType, transform, effectParameters, statusType, vertexData)
        {
            this.collisionPrimitive = collisionPrimitive;
            //unusual to pass this in but we use it to test for collisions - see Update();
            this.objectManager = objectManager;
        }


        public override void Update(GameTime gameTime)
        {
            //reset collidee to prevent colliding with the same object in the next update
            collidee = null;

            //reset any movements applied in the previous update from move keys
            this.Transform.TranslateIncrement = Vector3.Zero;
            this.Transform.RotateIncrement = 0;

            //update collision primitive with new object position
            if (collisionPrimitive != null)
                collisionPrimitive.Update(gameTime, this.Transform);

            base.Update(gameTime);
        }

        //read and store movement suggested by keyboard input
        protected virtual void HandleInput(GameTime gameTime)
        {

        }

        //define what happens when a collision occurs
        protected virtual void HandleCollisionResponse(Actor collidee)
        {

        }

        //test for collision against all opaque and transparent objects
        protected virtual Actor CheckCollisions(GameTime gameTime)
        {
           
            foreach (IActor actor in this.objectManager.OpaqueDrawList)
            {
                collidee = CheckCollisionWithActor(gameTime, actor as Actor3D);
                if (collidee != null)
                    return collidee;
            }

            foreach (IActor actor in this.objectManager.TransparentDrawList)
            {
                collidee = CheckCollisionWithActor(gameTime, actor as Actor3D);
                if (collidee != null)
                    return collidee;
            }

            return null;
        }

        //test for collision against a specific object
        private Actor CheckCollisionWithActor(GameTime gameTime, Actor3D actor3D)
        {
            //dont test for collision against yourself - remember the player is in the object manager list too!
            if (this != actor3D)
            {
                if (actor3D is CollidablePrimitiveObject)
                {
                    CollidablePrimitiveObject collidableObject = actor3D as CollidablePrimitiveObject;
                    if (this.CollisionPrimitive.Intersects(collidableObject.CollisionPrimitive, this.Transform.TranslateIncrement))
                        return collidableObject;
                }
                else if (actor3D is SimpleZoneObject)
                {
                    SimpleZoneObject zoneObject = actor3D as SimpleZoneObject;
                    if (this.CollisionPrimitive.Intersects(zoneObject.CollisionPrimitive, this.Transform.TranslateIncrement))
                        return zoneObject;
                }
            }

            return null;
        }

        //apply suggested movement since no collision will occur if the player moves to that position
        protected virtual void ApplyInput(GameTime gameTime)
        {
            //was a move/rotate key pressed, if so then these values will be > 0 in dimension
            if (this.Transform.TranslateIncrement != Vector3.Zero)
                this.Transform.TranslateBy(this.Transform.TranslateIncrement);

            if (this.Transform.RotateIncrement != 0)
                this.Transform.RotateAroundYBy(this.Transform.RotateIncrement);
        }

        public new object Clone()
        {
            return new CollidablePrimitiveObject("clone - " + ID, //deep
             this.ActorType, //deep
             (Transform3D)this.Transform.Clone(), //deep
             (EffectParameters)this.EffectParameters.Clone(), //deep
             this.StatusType, //deep
             this.VertexData, //shallow - its ok if objects refer to the same vertices
             (ICollisionPrimitive)this.CollisionPrimitive.Clone(), //deep
             this.objectManager); //shallow - reference
        }
    }
}
