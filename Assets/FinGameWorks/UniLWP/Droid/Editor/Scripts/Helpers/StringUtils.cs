using System;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers
{
    public static class StringUtils
    {
        public static String Indent(this String str)
        {
            return "\t" + str.Replace("\n", "\n\t");
        }
        
        public static String ToUnixPathStyle(this String str)
        {
            return str.Replace('/', '\\');
        }
    }
}