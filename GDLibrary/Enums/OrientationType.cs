/*
Function: 		Used when making primitive circle vertex data to say which plane to align the vertices on
                        //see VertexInfo::GetVerticesPostionColorCircle
Author: 		NMCG
Version:		1.0
Date Updated:	27/11/17
Bugs:			None
Fixes:			None
*/
namespace GDLibrary
{
    public enum OrientationType : sbyte
    {
        XYAxis,
        XZAxis,
        YZAxis
    }
}
