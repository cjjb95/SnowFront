/*
Function: 		Used in Main::InitializeCameras() to specify the desired camera layout
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum ScreenLayoutType : sbyte
    {
        FirstPerson,
        ThirdPerson,
        Multi2x2,
        Flight,
        Security,
        Multi1x3,
        Rail,
        Track,
        Multi1x4,
        Pip,
    }
}
