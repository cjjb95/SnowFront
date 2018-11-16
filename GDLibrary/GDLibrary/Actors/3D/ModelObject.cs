using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class ModelObject : DrawnActor3D
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

        public ModelObject(string id, ActorType actorType, 
            Transform3D transform, Model model, ColorParameters colorParameters, Effect effect) 
            : base(id, actorType, transform, colorParameters, effect)
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, Camera3D camera)
        {
            BasicEffect basicEffect = this.Effect as BasicEffect;

            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.Texture = this.ColorParameters.Texture;
            basicEffect.DiffuseColor = this.ColorParameters.DiffuseColor.ToVector3();
            basicEffect.Alpha = this.ColorParameters.Alpha;


            //gfx card - set these variable for subsequent draw!
            basicEffect.CurrentTechnique.Passes[0].Apply();

            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = basicEffect;
                }

                //this change means that models with multiple meshes will NOT collapse to the origin
                basicEffect.World = this.GetWorldMatrix();
                mesh.Draw();
            }

            base.Draw(gameTime, camera);
        }

        public new object Clone()
        {
           return new ModelObject(
                "clone - " + this.ID,                                   //deep copy i.e. a separate object is created
                this.ActorType,                                         //deep copy
                this.Transform.Clone() as Transform3D,                  //deep copy
                this.Model,                                             //shallow reference to a common object
                this.ColorParameters.Clone() as ColorParameters,        //deep copy
              // this.colorParameters,                                  //shallow
               this.Effect                                              //shallow clone
               );

        }
    }
}
