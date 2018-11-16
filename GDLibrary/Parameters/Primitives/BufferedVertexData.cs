/*
Function: 		This child class for drawing primitives where the vertex data is buffered on the GFX card in VRAM.
                Note: 
                - The class is generic and can be used to draw VertexPositionColor, VertexPositionColorTexture, VertexPositionColorNormal types etc.
                - For each draw the GFX card refers to vertex data that has already been buffered to VRAM 
                - This is a more efficient approach than either using the VertexData or DynamicBufferedVertexData classes if
                  you wish to draw a large number of primitives on screen.

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
    public class BufferedVertexData<T> : VertexData<T> where T : struct, IVertexType
    {
        #region Variables
        private VertexBuffer vertexBuffer;
        private GraphicsDevice graphicsDevice;
        #endregion

        #region Properties
        public VertexBuffer VertexBuffer
        {
            get
            {
                return vertexBuffer;
            }
            set
            {
                vertexBuffer = value;

            }
        }
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return this.graphicsDevice;
            }
        }
        #endregion

        //allows developer to pass in vertices AND buffer - more efficient since buffer is defined ONCE outside of the object instead of a new VertexBuffer for EACH instance of the class
        public BufferedVertexData(GraphicsDevice graphicsDevice, T[] vertices, VertexBuffer vertexBuffer, PrimitiveType primitiveType, int primitiveCount)
            : base(vertices, primitiveType, primitiveCount)
        {
            this.graphicsDevice = graphicsDevice;
            this.VertexBuffer = vertexBuffer;
            
            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }
    
        //buffer is created INSIDE the class so each class has a buffer - not efficient
        public BufferedVertexData(GraphicsDevice graphicsDevice, T[] vertices, PrimitiveType primitiveType, int primitiveCount)
            : base(vertices, primitiveType, primitiveCount)
        {
            this.graphicsDevice = graphicsDevice;
            this.VertexBuffer = new VertexBuffer(graphicsDevice, typeof(T), vertices.Length, BufferUsage.None);
            
            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }


        public void SetData(T[] vertices)
        {
            this.Vertices = vertices;
            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }

        public override void Draw(GameTime gameTime, Effect effect)
        {
            //this is what we want GFX to draw
            effect.GraphicsDevice.SetVertexBuffer(this.vertexBuffer);

            //draw!
            effect.GraphicsDevice.DrawPrimitives(this.PrimitiveType, 0, this.PrimitiveCount);           
        }

        public new object Clone()
        {
            return new BufferedVertexData<T>(this.graphicsDevice,  //shallow - reference
                this.Vertices, //shallow - reference
                this.PrimitiveType, //struct - deep
                this.PrimitiveCount); //deep - primitive
        }
    }
}
