/*
Function: 		This child class for drawing billboard primitives - these are primitives that automatically orient themselves in a specific way
                in relation to the camera (i.e. cylindrical, spherical) or point in a user-defined direction.
                Note: 
                - The class is generic and can be used to draw VertexPositionColor, VertexPositionColorTexture, VertexPositionColorNormal types etc.
                - For each draw call the vertex data is sent from RAM to VRAM. You an imagine that this is expensive - See BufferedVertexData.

Author: 		NMCG
Version:		1.0
Date Updated:	27/11/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GDLibrary
{
    //this is the contents sent to the GFX card for each vertex drawn
    public struct VertexBillboard : IVertexType
    {
        #region Variables
        public Vector3 position;
        public Vector4 texCoordAndOffset;
        #endregion

        public VertexBillboard(Vector3 position, Vector4 texCoordAndOffset)
        {
            this.position = position;
            this.texCoordAndOffset = texCoordAndOffset;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
    }

    public class BillboardVertexData<T> : BufferedVertexData<T> where T : struct, IVertexType
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        //allows developer to pass in vertices AND buffer - more efficient since buffer is defined ONCE outside of the object instead of a new VertexBuffer for EACH instance of the class
        public BillboardVertexData(GraphicsDevice graphicsDevice, T[] vertices, DynamicVertexBuffer vertexBuffer, PrimitiveType primitiveType, int primitiveCount)
            : base(graphicsDevice, vertices, vertexBuffer, primitiveType, primitiveCount)
        {

        }

        //buffer is created INSIDE the class so each class has a buffer - not efficient
        public BillboardVertexData(GraphicsDevice graphicsDevice, T[] vertices, PrimitiveType primitiveType, int primitiveCount)
            : base(graphicsDevice, vertices, primitiveType, primitiveCount)
        {

        }
    }
}
