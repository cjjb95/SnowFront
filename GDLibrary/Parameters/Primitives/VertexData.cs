/*
Function: 		This is the parent class for all primitives drawn where the developer explicitly defines the vertex data (i.e. position, color, normal, UV).
                Note: 
                - The class is generic and can be used to draw VertexPositionColor, VertexPositionColorTexture, VertexPositionColorNormal types etc.
                - For each draw call the vertex data is sent from RAM to VRAM. You an imagine that this is expensive - See BufferedVertexData.

Author: 		NMCG
Version:		1.0
Date Updated:	27/11/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class VertexData<T> : IVertexData where T : struct, IVertexType
    {
        #region Variables
        private T[] vertices;
        private PrimitiveType primitiveType;
        private int primitiveCount;
        #endregion

        #region Properties
        public PrimitiveType PrimitiveType
        {
            get
            {
                return this.primitiveType;
            }
        }
        public int PrimitiveCount
        {
            get
            {
                return this.primitiveCount;
            }
        }
        public T[] Vertices
        {
            get
            {
                return this.vertices;
            }
            set
            {
                this.vertices = value;
            }
        }
        #endregion

        public VertexData(T[] vertices, PrimitiveType primitiveType, int primitiveCount)
        {
            this.vertices = vertices;
            this.primitiveType = primitiveType;
            this.primitiveCount = primitiveCount;
        }

        public virtual void Draw(GameTime gameTime, Effect effect)
        {
            effect.GraphicsDevice.DrawUserPrimitives<T>(this.primitiveType, this.vertices, 0, this.primitiveCount);
        }

        public object Clone()
        {
            return new VertexData<T>(
                this.Vertices, //shallow - reference
                this.PrimitiveType,  //struct - deep
                this.PrimitiveCount);  //deep - primitive
        }
    }
}
