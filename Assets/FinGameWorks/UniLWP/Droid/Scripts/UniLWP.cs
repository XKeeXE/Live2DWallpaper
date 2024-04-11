using System;

namespace FinGameWorks.UniLWP.Droid.Scripts
{
    public class UniLWP
    {
        public static String Version = "0.0.2";
        public static int Patch = 3;
        public static String Channel = "preview";
        public static String FullVersion()
        {
            return Version + " (" + Channel + "." + Patch + ")";
        }

        public static DateTime ReleaseDate = new DateTime(2023,8,9);
    }
}