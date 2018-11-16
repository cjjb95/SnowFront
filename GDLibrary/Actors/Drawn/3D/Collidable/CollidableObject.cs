using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class CollidableObject : ModelObject
    {
        #region Variables
        private Body body;
        private CollisionSkin collision;
        private float mass;
        #endregion

        #region Properties
        public float Mass
        {
            get
            {
                return mass;
            }
            set
            {
                mass = value;
            }
        }
        public CollisionSkin Collision
        {
            get
            {
                return collision;
            }
            set
            {
                collision = value;
            }
        }
        public Body Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }
        #endregion

        public CollidableObject(string id, ActorType actorType, Transform3D transform, EffectParameters effectParameters, Model model)
            : base(id, actorType, transform, effectParameters, model)
        {
            this.body = new Body();
            this.body.ExternalData = this;
            this.collision = new CollisionSkin(this.body);
            this.body.CollisionSkin = this.collision;

            //we will only add this event handling in a class that sub-classes CollidableObject e.g. PickupCollidableObject or PlayerCollidableObject
            //this.body.CollisionSkin.callbackFn += CollisionSkin_callbackFn;
        }

        //we will only add this method in a class that sub-classes CollidableObject e.g. PickupCollidableObject or PlayerCollidableObject
        //private bool CollisionSkin_callbackFn(CollisionSkin skin0, CollisionSkin skin1)
        //{
        //    return true;
        //}

        public override Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(this.Transform.Scale) *
                    this.collision.GetPrimitiveLocal(0).Transform.Orientation *
                        this.body.Orientation *
                            this.Transform.Orientation *
                                Matrix.CreateTranslation(this.body.Position);
        }


        protected Vector3 SetMass(float mass)
        {
            PrimitiveProperties primitiveProperties = new PrimitiveProperties(PrimitiveProperties.MassDistributionEnum.Solid, PrimitiveProperties.MassTypeEnum.Density, mass);

            float junk;
            Vector3 com;
            Matrix it, itCoM;

            this.collision.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);
            body.BodyInertia = itCoM;
            body.Mass = junk;

            return com;
        }

        public void AddPrimitive(Primitive primitive, MaterialProperties materialProperties)
        {
            this.collision.AddPrimitive(primitive, materialProperties);
        }

        public virtual void Enable(bool bImmovable, float mass)
        {
            this.mass = mass;

            //set whether the object can move
            this.body.Immovable = bImmovable;
            //calculate the centre of mass
            Vector3 com = SetMass(mass);
            //adjust skin so that it corresponds to the 3D mesh as drawn on screen
            this.body.MoveTo(this.Transform.Translation, Matrix.Identity);
            //set the centre of mass
            this.collision.ApplyLocalTransform(new Transform(-com, Matrix.Identity));
            //enable so that any applied forces (e.g. gravity) will affect the object
            this.body.EnableBody();
        }

     
        public new object Clone()
        {
            return new CollidableObject("clone - " + ID, //deep
                this.ActorType,   //deep
                (Transform3D)this.Transform.Clone(),  //deep
                this.EffectParameters.GetDeepCopy(), //hybrid - shallow (texture and effect) and deep (all other fields) 
                this.Model); //shallow i.e. a reference
        }

        public override bool Remove()
        {
            this.body = null;
            return base.Remove();
        }

    }
}
