using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UIObject : DrawnActor2D
    {
        #region Fields
        private StatefulBool mouseOverState;
        #endregion

        #region Properties
        public StatefulBool MouseOverState
        {
            get
            {
                return this.mouseOverState;
            }
        }
        #endregion

        public UIObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth)
            : base(id, actorType, transform, statusType, color, spriteEffects, layerDepth)
        {
            this.mouseOverState = new StatefulBool(2); 
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.mouseOverState.GetHashCode();
            hash = hash * 17 + base.GetHashCode();
            return hash;
        }

        public override bool Remove()
        {
            this.mouseOverState = null;
            return base.Remove();
        }
    }
}
