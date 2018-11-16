using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class DrawnActor3D : Actor3D
    {
        #region Fields
        private ColorParameters colorParameters;
        private Effect effect;
        #endregion

        #region Properties
        public Effect Effect
        {
            get
            {
                return this.effect;
            }
            set
            {
                this.effect = value;
            }
        }
        public ColorParameters ColorParameters
        {
            get
            {
                return this.colorParameters;
            }
            set
            {
                this.colorParameters = value;
            }
        }
        #endregion

        public DrawnActor3D(string id, ActorType actorType, Transform3D transform,
             ColorParameters colorParameters, Effect effect) : base(id, actorType, transform)
        {
            this.colorParameters = colorParameters;
            this.effect = effect;
        }

        public float GetAlpha()
        {
            return this.colorParameters.Alpha;
        }

    }
}
