using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public interface IEffectParameters : ICloneable
    {
        void SetParameters(Camera3D camera);
        void SetWorld(Matrix world);
    }
}
