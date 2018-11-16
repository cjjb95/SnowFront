/*
Function: 		Provides debug/logging/exception related methods
Author: 		NMCG
Version:		1.0
Date Updated:	25/11/17
Bugs:			None
Fixes:			None
*/
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GDLibrary
{
    public class DebugUtility
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            return new StackTrace().GetFrame(1).GetMethod().Name;
        }
    }
}
