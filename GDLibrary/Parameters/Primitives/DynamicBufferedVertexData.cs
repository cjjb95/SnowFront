/*
Function: 		This child class for drawing primitives where the vertex data can change over time (e.g. where you use a controller to modify the position of vertices).
                You can use this class to create a wall of vertices that undulate over time (i.e. like a animated polygon sea as in something like Monument Valley).
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
using System;

namespace GDLibrary
{
    public class DynamicBufferedVertexData<T> : BufferedVertexData<T> where T : struct, IVertexType
    {
        #region Variables
        private DynamicVertexBuffer vertexBuffer;
        #endregion

        #region Properties
        public new DynamicVertexBuffer VertexBuffer
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
        #endregion

        //allows developer to pass in vertices AND buffer - more efficient since buffer is defined ONCE outside of the object instead of a new VertexBuffer for EACH instance of the class
        public DynamicBufferedVertexData(GraphicsDevice graphicsDevice, T[] vertices, DynamicVertexBuffer vertexBuffer, PrimitiveType primitiveType, int primitiveCount)
            : base(graphicsDevice, vertices, vertexBuffer, primitiveType, primitiveCount)
        {
            RegisterForEventHandling();
        }

        //buffer is created INSIDE the class so each class has a buffer - not efficient
        public DynamicBufferedVertexData(GraphicsDevice graphicsDevice, T[] vertices, PrimitiveType primitiveType, int primitiveCount)
            : base(graphicsDevice, vertices, primitiveType, primitiveCount)
        {
            this.VertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(T), vertices.Length, BufferUsage.None);
            RegisterForEventHandling();
        }

        private void RegisterForEventHandling()
        {
            //add an event listener to reset the data if another game object access the graphics device and (potentially) resets buffer contents
            this.vertexBuffer.ContentLost += vertexBuffer_ContentLost;
        }

        //called automatically when developer changes the GFX card loses control to another draw call and the vertex data needs to be reset
        void vertexBuffer_ContentLost(object sender, EventArgs e)
        {
            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }

        public new object Clone()
        {
            return new DynamicBufferedVertexData<T>(this.GraphicsDevice,  //shallow - reference
                this.Vertices,  //shallow - reference
                this.VertexBuffer, //shallow - reference
                this.PrimitiveType,  //struct - deep
                this.PrimitiveCount);  //deep - primitive
        }
    }
}
