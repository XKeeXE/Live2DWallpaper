using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers
{
    public static class ProjectUtils
    {
        public static List<string> GetSymbolDefines()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Split(';').ToList();
        }
    }
}