using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class BasicEffectParameters : EffectParameters
    {
        #region Fields
        //statics
        protected static readonly Color DefaultWorldAmbientColor = Color.Black;
        protected static readonly Color DefaultSpecularColor = Color.White;
        protected static readonly int DefaultSpecularPower = 32; //1 - 256 - higher value (e.g. > 128) is more computationally expensive  
        protected static readonly Color DefaultEmissiveColor = Color.Black;

        //color specific
        private Color ambientColor = DefaultWorldAmbientColor; //brighten the objects a little vs using Color.Black
        private Color specularColor = DefaultSpecularColor;
        private Color emissiveColor = DefaultEmissiveColor;
        private int specularPower = DefaultSpecularPower;

        //reset
        private BasicEffectParameters originalEffectParameters;
        #endregion

        #region Properties
        public Color AmbientColor
        {
            get
            {
                return this.ambientColor;
            }
            set
            {
                this.ambientColor = value;
            }
        }
        public Color SpecularColor
        {
            get
            {
                return this.specularColor;
            }
            set
            {
                this.specularColor = value;
            }
        }
        public int SpecularPower
        {
            get
            {
                return this.specularPower;
            }
            set
            {
                /*
                 * Ranges in value from 1 - 256 but becomes more computationally intensive as values increases since specular intensity, Si = pow(N.H, specularPower)
                 * Where N is the surface Normal and H is the surface Half Vector (i.e. H = N + V, where V is the View vector).
                 * This theory (Phong Reflection) will be covered in Year 4, Special Lighting Effects.
                 * See https://en.wikipedia.org/wiki/Phong_reflection_model
                 * 
                 * As specularPower, Sp, increases the specular response on a surface narrows and more resembles a polished surface with fewer diffuse microfacets (i.e. it diffuses/scatters the incident light less).
                 * Think about how light reflects off a diamond (high Sp) vs shiny leather (low Sp)
                 * 
                 */

                this.specularPower = (value > 0 && value <= 256) ? value : DefaultSpecularPower;
            }
        }
        public Color EmissiveColor
        {
            get
            {
                return this.emissiveColor;
            }
            set
            {
                this.emissiveColor = value;
            }
        }
        public new BasicEffectParameters OriginalEffectParameters
        {
            get
            {
                return this.originalEffectParameters;
            }
        }
        #endregion


        //used to create the simplest instance of the class - fields will be set by each instanciating object - see Main::InitializeEffects()
        public BasicEffectParameters(Effect effect)
            : base(effect)
        {

        }

        public BasicEffectParameters(Effect effect, Texture2D texture, Color ambientColor, Color diffuseColor, Color specularColor, 
                    Color emissiveColor, int specularPower, float alpha)
            : base(effect, texture, diffuseColor, alpha)
        {
            Initialize(ambientColor, diffuseColor, specularColor, emissiveColor, specularPower);

            //store original values in case of reset
            this.originalEffectParameters = new BasicEffectParameters(effect);
            this.originalEffectParameters.Initialize(ambientColor, diffuseColor, specularColor, emissiveColor, specularPower);

        }

        protected void Initialize(Color ambientColor, Color diffuseColor, Color specularColor, Color emmissiveColor, int specularPower)
        {
            this.ambientColor = ambientColor;
            this.specularColor = specularColor;
            this.emissiveColor = emmissiveColor;
            this.specularPower = specularPower;
        }

        protected override void Reset()
        {
            base.Reset();
            this.Initialize(this.originalEffectParameters.Effect, this.originalEffectParameters.Texture, this.originalEffectParameters.DiffuseColor, this.originalEffectParameters.Alpha);
        }

        public override void SetParameters(Camera3D camera)
        {
            BasicEffect bEffect = this.Effect as BasicEffect;
            bEffect.View = camera.View;
            bEffect.Projection = camera.Projection;

            bEffect.AmbientLightColor = this.AmbientColor.ToVector3();
            bEffect.DiffuseColor = this.DiffuseColor.ToVector3();
            bEffect.SpecularColor = this.SpecularColor.ToVector3();
            bEffect.SpecularPower = this.SpecularPower;
            bEffect.EmissiveColor = this.EmissiveColor.ToVector3();
            bEffect.Alpha = this.Alpha;

            //Not all models NEED a texture. Does a semi-transparent window need a texture?
            if (this.Texture != null)
            {
                bEffect.TextureEnabled = true;
                bEffect.Texture = this.Texture;
            }
            else
            {
                bEffect.TextureEnabled = false;
            }

            base.SetParameters(camera);
        }

        public override void SetWorld(Matrix world)
        {
            (this.Effect as BasicEffect).World = world;
        }

        //add equals, gethashcode...

        public override EffectParameters GetDeepCopy()
        {
            return new BasicEffectParameters(this.Effect, //shallow - a reference
                this.Texture, //shallow - a reference
                this.AmbientColor, //deep
                this.DiffuseColor,//deep
                this.SpecularColor,//deep
                this.EmissiveColor,//deep
                this.SpecularPower,//deep
                this.Alpha);//deep
        }

        public override object Clone()
        {
            return GetDeepCopy();
        }

    }
}
