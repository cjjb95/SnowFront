using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public class ColorParameters : ICloneable
    {
        private Color diffuseColor;
        private Texture2D texture;
        private float alpha;

        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
            }
        }
        public float Alpha
        {
            get { return this.alpha; }

            set
            {
                this.alpha = (value >= 0 && value <= 1) ? value : 1;
            }
        }  
        public Color DiffuseColor
        {
            get
            {
                return this.diffuseColor;
            }
            set
            {
                this.diffuseColor = value;
            }
        }

        //white opaque things
        public ColorParameters(Texture2D texture)
            : this(Color.White, texture, 1)
        {
        }

        public ColorParameters(Color diffuseColor, Texture2D texture, float alpha)
        {
            this.diffuseColor = diffuseColor;
            this.texture = texture;
            this.alpha = alpha;
        }

        public override bool Equals(object obj)
        {
            var parameters = obj as ColorParameters;
            return parameters != null &&
                   diffuseColor.Equals(parameters.diffuseColor) &&
                   EqualityComparer<Texture2D>.Default.Equals(texture, parameters.texture) &&
                   alpha == parameters.alpha;
        }

        public override int GetHashCode()
        {
            var hashCode = -877543569;
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(diffuseColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<Texture2D>.Default.GetHashCode(texture);
            hashCode = hashCode * -1521134295 + alpha.GetHashCode();
            return hashCode;
        }

        //added clone to support a deep copy of this object in ModelObject::Clone() - remember the Skybox cloning code in Main.cs?
        public object Clone()
        {
            return new ColorParameters(this.diffuseColor, this.texture, this.alpha);
        }

        //Clone


    }
}