/*
Function: 		Used to represent compass directions - See MouseManager::IsMouseInsideGameWindow()
Author: 		NMCG
Version:		1.0
Date Updated:	6/10/17
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum CompassDirectionType : sbyte
    {
        Centre = 0,
        North = 1,
        South = 2,
        East = 4,
        West = 8
    }
}
