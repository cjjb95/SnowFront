/*
Function: 		Allows us to draw models objects. These are the FBX files we export from 3DS Max. 
Author: 		NMCG
Version:		1.0
Date Updated:	17/8/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class ModelObject : DrawnActor3D, ICloneable
    {
        #region Fields
        private Model model;
        private Matrix[] boneTransforms;
        #endregion

        #region Properties
        public Model Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }
        public Matrix[] BoneTransforms
        {
            get
            {
                return this.boneTransforms;
            }
            set
            {
                this.boneTransforms = value;
            }
        }
        public BoundingSphere BoundingSphere
        {
            get
            {
                //bug fix for disappearing skybox plane - scale the bounding sphere up by 10%
                return this.model.Meshes[model.Root.Index].BoundingSphere.Transform(Matrix.CreateScale(1.1f) * this.GetWorldMatrix());
            }
        }
        #endregion


        //default draw and update settings for statusType
        public ModelObject(string id, ActorType actorType,
           Transform3D transform, EffectParameters effectParameters, Model model)
           : this(id, actorType, transform, effectParameters, model, StatusType.Update | StatusType.Drawn)
        {

        }

        public ModelObject(string id, ActorType actorType, 
            Transform3D transform, EffectParameters effectParameters, Model model, StatusType statusType)
            : base(id, actorType, transform, effectParameters, statusType)
        {
            this.model = model;

            /* 3DS Max models contain meshes (e.g. a table might have 5 meshes i.e. a top and 4 legs) and each mesh contains a bone.
            *  A bone holds the transform that says "move this mesh to this position". Without 5 bones in a table all the meshes would collapse down to be centred on the origin.
            *  Our table, wouldnt look very much like a table!
            *  
            *  Take a look at the ObjectManager::DrawObject(GameTime gameTime, ModelObject modelObject) method and see if you can figure out what the line below is doing:
            *  
            *  effect.World = modelObject.BoneTransforms[mesh.ParentBone.Index] * modelObject.GetWorldMatrix();
            */
            InitializeBoneTransforms();
        }

        private void InitializeBoneTransforms()
        {
            //load bone transforms and copy transfroms to transform array (transforms)
            if (this.model != null)
            {
                this.boneTransforms = new Matrix[this.model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            }
        }

        public override bool Equals(object obj)
        {
            ModelObject other = obj as ModelObject;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.model.Equals(other.Model) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 11 + this.model.GetHashCode();
            hash = hash * 17 + base.GetHashCode();
            return hash;
        }

        public new object Clone()
        {
            ModelObject actor = new ModelObject("clone - " + ID, //deep
                this.ActorType,   //deep
                (Transform3D)this.Transform.Clone(),  //deep
                this.EffectParameters.GetDeepCopy(), //hybrid - shallow (texture and effect) and deep (all other fields) 
                this.model); //shallow i.e. a reference

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
            //tag for garbage collection
            this.boneTransforms = null;
            //notice how the base Remove() is called. What will happen when this is called? See DrawnActor3D::Remove().
            return base.Remove();
        }
    }
}
