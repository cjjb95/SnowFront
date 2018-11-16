/*
Function: 		Represents the parent class for all updateable AND drawn 2D menu and UI objects. 
Author: 		NMCG
Version:		1.0
Date Updated:	27/9/17
Bugs:			None
Fixes:			None
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class DrawnActor2D : Actor2D
    {
        #region Fields
        private Color color, originalColor;
        private float layerDepth, originalLayerDepth;
        private SpriteEffects originalSpriteEffects, spriteEffects;
        #endregion

        #region Properties
        public Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }
        public Color OriginalColor
        {
            get
            {
                return this.originalColor;
            }
            set
            {
                this.originalColor = value;
            }
        }

        public float LayerDepth
        {
            get
            {
                return this.layerDepth;
            }
            set
            {
                this.layerDepth = ((value >= 0) && (value <= 1))
                    ? value : 0;
            }
        }
        public float OriginalLayerDepth
        {
            get
            {
                return this.originalLayerDepth;
            }
            private set
            {
                this.originalLayerDepth = value;
            }
        }
        public SpriteEffects SpriteEffects
        {
            get
            {
                return this.spriteEffects;
            }
            set
            {
                this.spriteEffects = value;
            }
        }
        public SpriteEffects OriginalSpriteEffects
        {
            get
            {
                return this.originalSpriteEffects;
            }
            private set
            {
                this.originalSpriteEffects = value;
            }
        }
        #endregion

        public DrawnActor2D(string id, ActorType actorType, Transform2D transform, StatusType statusType, 
            Color color, SpriteEffects spriteEffects, float layerDepth)
            : base(id, actorType, transform, statusType)
        {
            this.color = color;
            this.originalColor = color;
            this.spriteEffects = spriteEffects;
            this.LayerDepth = layerDepth;
            this.originalLayerDepth = LayerDepth;
            this.spriteEffects = spriteEffects;
        }

        public override bool Equals(object obj)
        {
            DrawnActor2D other = obj as DrawnActor2D;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.Color.Equals(other.Color) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.Color.GetHashCode();
            hash = hash * 17 + base.GetHashCode();
            return hash;
        }

        public new object Clone()
        {
            IActor actor = new DrawnActor2D("clone - " + ID, //deep
                this.ActorType, //deep
                (Transform2D)this.Transform.Clone(), //deep - calls the clone for Transform3D explicitly
                this.StatusType, //deep - enum type
                this.Color, //deep 
                this.spriteEffects, //deep - enum type
                this.LayerDepth); //deep - a simple numeric type

            //clone each of the (behavioural) controllers
            foreach (IController controller in this.ControllerList)
                actor.AttachController((IController)controller.Clone());

            return actor;
        }
    }
}
