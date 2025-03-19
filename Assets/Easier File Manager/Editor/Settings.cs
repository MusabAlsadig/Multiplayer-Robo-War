using UnityEditor;
using UnityEngine;

namespace EasierFileManager
{
    internal class Settings : EditorWindow
    {

        // max time between selecting
        internal static int maxCooldown = 200; // in ms (milliseconds)

        // min time between selecting
        internal static int minCooldown = 10; // in ms (milliseconds)

        internal static bool useDebug_Log = true;

        // this number will multiply with current cooldown after each selection
        // like, if current cooldown = 100, nex cooldown = 100 * 0.8 = 80
        internal static float renewRate = 0.8f;


        private void OnGUI()
        {
            maxCooldown = EditorGUILayout.IntField("Max Cooldown (ms)", maxCooldown);
            minCooldown = EditorGUILayout.IntField("Min Cooldown (ms)", minCooldown);

            useDebug_Log = EditorGUILayout.Toggle("Enable Debug.Log", useDebug_Log);
            renewRate = EditorGUILayout.Slider("Cooldown decrease rate", renewRate, 0, 1);

            GUILayout.Label("Cooldown = delay between each selection while holding a key");

        }
    }
}
