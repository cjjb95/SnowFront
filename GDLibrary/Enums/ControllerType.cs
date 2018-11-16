/*
Function: 		Used by Controller to define what action the controller applies to its attached Actor. For example, a FirstPersonCamera has
                an attached controller of type FirstPerson.
Author: 		NMCG
Version:		1.1
Date Updated:	29/9/17
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum ControllerType : sbyte
    {
        Drive, //applied to 3D model
        FirstPerson, //applied to camera
        ThirdPerson, //applied to camera
        Rail,  //applied to camera or model
        Track, //applied to camera or model
        Security, //applied to camera

        //applied to any 3D actor (i.e. camera or model)
        Rotation,
        Translation,
        Scale,
        Color,
        LerpRotation,
        LerpTranslation,
        ScaleLerp,
        ColorLerp,
        TextureLerp,

        //applied to any 2D actor (e.g. menu button, game state info)
        SineScaleLerp,
        SineRotationLerp,
        SineColorLerp,

        //JibLibX related
        CollidableFirstPerson,

        //UI related
        UIProgress, //see UIProgressController demo
        Video
    }
}
