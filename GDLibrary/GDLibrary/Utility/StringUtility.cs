using System.Text.RegularExpressions;

namespace GDLibrary
{
    public class StringUtility
    {
        //parse a file name from a path + name string
        public static string ParseNameFromPath(string path)
        { 
            //e.g. "Assets/Textures/sky" returns "sky"
            return Regex.Match(path, @"[^\\/]*$").Value;
        }
    }
}
